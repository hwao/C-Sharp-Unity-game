using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlatesCounterVisual : MonoBehaviour
{
    [SerializeField] private PlatesCounter _platesCounter;
    [SerializeField] private Transform _counterTopPoint;
    [SerializeField] private Transform _plateVisualPrefab;

    private List<GameObject> _plateVisualGameObjectList;

    private void Awake()
    {
        _plateVisualGameObjectList = new List<GameObject>();
    }

    private void Start()
    {
        _platesCounter.OnPlateSpawned += PlatesCounterOnOnPlateSpawned;
        _platesCounter.OnPlateRemoved += PlatesCounterOnPlateRemoved;
    }

    private void PlatesCounterOnPlateRemoved(object sender, EventArgs e)
    {
        GameObject plate = _plateVisualGameObjectList[_plateVisualGameObjectList.Count - 1];
        _plateVisualGameObjectList.Remove(plate);
        Destroy(plate);
    }

    private void PlatesCounterOnOnPlateSpawned(object sender, EventArgs e)
    {
        Transform plateVisualGameObject = Instantiate(_plateVisualPrefab, _counterTopPoint);
        float plateOffsetY = .1f;
        plateVisualGameObject.localPosition = new Vector3(0, plateOffsetY * _plateVisualGameObjectList.Count, 0);
        
        _plateVisualGameObjectList.Add(plateVisualGameObject.gameObject);
    }
}