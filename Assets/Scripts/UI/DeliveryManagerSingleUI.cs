using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryManagerSingleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI recipiNameText;
    [SerializeField] private Transform iconContainer;
    [SerializeField] private Transform iconTemplate;

    private void Awake()
    {
        iconTemplate.gameObject.SetActive(false);
    }

    public void SetRecipeSo(RecipeSo recipeSo)
    {
        recipiNameText.text = recipeSo.recipeName;

        foreach (Transform child in iconContainer)
        {
            if (child == iconTemplate) continue;

            Destroy(child.gameObject);
        }

        foreach (KitchenObjectSO kitchenObjectSo in recipeSo.kitchenObjectSoList)
        {
            Transform iconTemplate = Instantiate(this.iconTemplate, iconContainer);
            iconTemplate.gameObject.SetActive(true);
            iconTemplate.GetComponent<Image>().sprite = kitchenObjectSo.sprite;
        }
    }
}