using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateComplateVisual : MonoBehaviour
{
    [Serializable]
    public struct KitchenObjectSO_GameObject
    {
        public KitchenObjectSO KitchenObjectSo;
        public GameObject GameObject;
    }

    [SerializeField] private PlateKitchenObject _plateKitchenObject;
    [SerializeField] private List<KitchenObjectSO_GameObject> _kitchenObjectSoGameObjectList;

    private void Start()
    {
        _plateKitchenObject.OnIngredientAdded += PlateKitchenObjectOnIngredientAdded;

        foreach (KitchenObjectSO_GameObject kitchenObjectSoGameObject in _kitchenObjectSoGameObjectList)
        {
            kitchenObjectSoGameObject.GameObject.SetActive(false);
        }
    }

    private void PlateKitchenObjectOnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e)
    {
        foreach (KitchenObjectSO_GameObject kitchenObjectSoGameObject in _kitchenObjectSoGameObjectList)
        {
            if (kitchenObjectSoGameObject.KitchenObjectSo == e.kitchenObjectSo)
            {
                kitchenObjectSoGameObject.GameObject.SetActive(true);
            }
        }
    }
}