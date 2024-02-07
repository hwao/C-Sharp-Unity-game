using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounterVisual : MonoBehaviour
{
  [SerializeField] private StoveCounter _stoveCounter;
  [SerializeField] private GameObject stoveOnGameObject;
  [SerializeField] private GameObject particlesGameObject;

  private void Start()
  {
    _stoveCounter.OnStateChanged += StoveCounterOnStateChanged;
  }

  private void StoveCounterOnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e)
  {
    bool showVisual = e.state is StoveCounter.State.Fried or StoveCounter.State.Frying;
    stoveOnGameObject.SetActive(showVisual);
    particlesGameObject.SetActive(showVisual);
  }
}
