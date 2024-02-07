using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DeliveryManager : MonoBehaviour
{
    public event EventHandler OnRecipeSpawned;
    public event EventHandler OnRecipeComplicated;
    public event EventHandler OnRecipeSuccess;
    public event EventHandler OnRecipeFailed;

    public static DeliveryManager Instance { get; private set; }

    [SerializeField] private RecipeListSo _recipeListSo;
    private List<RecipeSo> _waitingRecipeSoList;

    private float _spawnRecipeTimer;
    private float _spawnRecipeTimerMax = 4f;
    private int _waitingRecipeMax = 4;

    private int successfulRecipesAmount = 0;


    private void Awake()
    {
        Instance = this;
        _waitingRecipeSoList = new List<RecipeSo>();
    }

    private void Update()
    {
        _spawnRecipeTimer -= Time.deltaTime;
        if (_spawnRecipeTimer <= 0f)
        {
            _spawnRecipeTimer = _spawnRecipeTimerMax;

            if (_waitingRecipeSoList.Count < _waitingRecipeMax)
            {
                RecipeSo randomRecipe =
                    _recipeListSo._recipeSoList[UnityEngine.Random.Range(0, _recipeListSo._recipeSoList.Count - 1)];

                _waitingRecipeSoList.Add(randomRecipe);

                OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
    {
        bool plateContentsMatchesRecipe = true;

        for (int i = 0; i < _waitingRecipeSoList.Count; i++)
        {
            RecipeSo waitingRecipeSo = _waitingRecipeSoList[i];

            if (waitingRecipeSo.kitchenObjectSoList.Count == plateKitchenObject.GetKitchenObjectList().Count)
            {
                // Has the same number of ingredients
                plateContentsMatchesRecipe = true;

                foreach (KitchenObjectSO recipeKitchenObjectSo in waitingRecipeSo.kitchenObjectSoList)
                {
                    // Cycling through all ingredients in the Recipe
                    bool ingredientFound = false;

                    foreach (KitchenObjectSO plateKitchenObjectSo in plateKitchenObject.GetKitchenObjectList())
                    {
                        // Cycling through all ingredients in the Plate
                        if (plateKitchenObjectSo == recipeKitchenObjectSo)
                        {
                            // Ingredient matches!
                            ingredientFound = true;
                            break;
                        }
                    }

                    if (!ingredientFound)
                    {
                        // This Recipe ingredient was not found on the Plate
                        plateContentsMatchesRecipe = false;
                    }
                }


                if (plateContentsMatchesRecipe)
                {
                    // Player delivered the correct recipe!
                    Debug.Log("Player delivered the correct recipe!");
                    _waitingRecipeSoList.RemoveAt(i);

                    successfulRecipesAmount++;

                    OnRecipeComplicated?.Invoke(this, EventArgs.Empty);
                    OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
                    return;
                }
            }
        }

        OnRecipeFailed?.Invoke(this, EventArgs.Empty);
        // No Matches Found!
        Debug.Log("player did not deliver a correct recipe");
    }

    public List<RecipeSo> GetWaitingRecipeSoList()
    {
        return _waitingRecipeSoList;
    }

    public int GetSuccessfulRecipesAmount()
    {
        return successfulRecipesAmount;
    }
}