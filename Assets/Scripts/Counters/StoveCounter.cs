using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{
    public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
    public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

    public class OnStateChangedEventArgs : EventArgs
    {
        public State state;
    }

    public enum State
    {
        Idle,
        Frying,
        Fried,
        Bruned
    };

    [SerializeField] private FryingRecipeSO[] _fryingRecipeSOArray;

    private State state;

    private float fryingTimer;
    private float burningTimer;
    private FryingRecipeSO fryingRecipeSo;

    private void Start()
    {
        StateChange(State.Idle);
    }

    private void StateChange(State state)
    {
        this.state = state;
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
        {
            state = state
        });
    }

    private void Update()
    {
        switch (state)
        {
            case State.Idle:
                break;
            case State.Frying:
                UpdateFryingProgress(fryingTimer + Time.deltaTime);

                if (fryingTimer > fryingRecipeSo.fryingTimerMax)
                {
                    // Fried
                    GetKitchenObject().DestroySelf();

                    KitchenObject.SpawnKitchenObject(fryingRecipeSo.output, this);

                    StateChange(State.Fried);
                    burningTimer = 0f;
                }

                break;
            case State.Fried:
                burningTimer += Time.deltaTime;
                break;
            case State.Bruned:
                break;
        }

        if (HasKitchenObject())
        {
        }
    }

    public override void Interact(PlayerScript player)
    {
        if (!HasKitchenObject())
        {
            if (player.HasKitchenObject())
            {
                if (HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO()))
                {
                    player.GetKitchenObject().SetKitchenObjectParent(this);

                    fryingRecipeSo = GetFryingRecipeSOForInput(GetKitchenObject().GetKitchenObjectSO());

                    StateChange(State.Frying);
                    UpdateFryingProgress(0f);

                    // UpdateFryingProgress(0);
                }
            }
        }
        else
        {
            if (player.HasKitchenObject())
            {
                if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject))
                {
                    // Player is holding a Plate
                    if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO()))
                    {
                        GetKitchenObject().DestroySelf();
                        StateChange(State.Idle);
                    }
                }
            }
            else
            {
                GetKitchenObject().SetKitchenObjectParent(player);
                StateChange(State.Idle);
                UpdateFryingProgress(0f);
            }
        }
    }

    private FryingRecipeSO GetFryingRecipeSOForInput(KitchenObjectSO input)
    {
        foreach (FryingRecipeSO fryingRecipeSo in _fryingRecipeSOArray)
        {
            if (fryingRecipeSo.input == input)
            {
                return fryingRecipeSo;
            }
        }

        return null;
    }

    private KitchenObjectSO GetOutputFromInput(KitchenObjectSO input)
    {
        return GetFryingRecipeSOForInput(input)?.output;
    }

    private bool HasRecipeWithInput(KitchenObjectSO input)
    {
        return GetFryingRecipeSOForInput(input) != null;
    }

    private void UpdateFryingProgress(float i)
    {
        fryingTimer = i;

        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs()
        {
            progressNormalized = (float)fryingTimer / fryingRecipeSo.fryingTimerMax
        });
    }
}