using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlateSingleIconUI : MonoBehaviour
{
    [SerializeField] private Image _image;

    public void SetKitchenObjectSo(KitchenObjectSO kitchenObjectSo)
    {
        _image.sprite = kitchenObjectSo.sprite;
    }
}