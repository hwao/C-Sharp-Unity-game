using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{
    public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;

    public class OnIngredientAddedEventArgs : EventArgs
    {
        public KitchenObjectSO kitchenObjectSo;
    }

    [SerializeField] private List<KitchenObjectSO> _validKitchenObjectSoList;

    private List<KitchenObjectSO> _kitchenObjectList;

    private void Awake()
    {
        _kitchenObjectList = new List<KitchenObjectSO>();
    }

    public bool TryAddIngredient(KitchenObjectSO kitchenObjectSo)
    {
        if (!_validKitchenObjectSoList.Contains(kitchenObjectSo))
        {
            // Not a valid ingridient
            return false;
        }

        if (_kitchenObjectList.Contains(kitchenObjectSo))
        {
            // mamy juz
            return false;
        }

        _kitchenObjectList.Add(kitchenObjectSo);
        OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs
        {
            kitchenObjectSo = kitchenObjectSo
        });
        return true;
    }

    public List<KitchenObjectSO> GetKitchenObjectList()
    {
        return _kitchenObjectList;
    }
}