using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    private PlayerScript _playerScript;

    private float footstepTimer;
    private float footstepTimerMax = .1f;

    private void Awake()
    {
        _playerScript = GetComponent<PlayerScript>();
    }

    private void Update()
    {
        footstepTimer -= Time.deltaTime;
        if (footstepTimer < 0f)
        {
            footstepTimer = footstepTimerMax;

            if (_playerScript.IsWalking)
            {
                float playerFootstepVolume = 1f;
                SoundManager.Instance.PlayFootstepsSound(_playerScript.transform.position, playerFootstepVolume);
            }
        }
    }
}