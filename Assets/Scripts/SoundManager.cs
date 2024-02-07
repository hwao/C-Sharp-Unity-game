using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    [SerializeField] private AudioClipRefsSo _audioClipRefsSo;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManagerOnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManagerOnRecipeFailed;

        CuttingCounter.OnAnyCut += (sender, args) =>
            PlaySound(_audioClipRefsSo.chop, (sender as CuttingCounter).transform.position);

        PlayerScript.Instance.OnPickedSomething += (sender, args) =>
            PlaySound(_audioClipRefsSo.objectPickup, PlayerScript.Instance.transform.position);

        BaseCounter.OnAnyObjectPlacedHere += (sender, args) =>
            PlaySound(_audioClipRefsSo.objectDrop, (sender as BaseCounter).transform.position);

        TrashCounter.OnAnyObjectTrashed += (sender, args) =>
            PlaySound(_audioClipRefsSo.trash, (sender as BaseCounter).transform.position);
    }

    private void DeliveryManagerOnRecipeFailed(object sender, EventArgs e)
    {
        PlaySound(_audioClipRefsSo.deliveryFail, DeliveryCounter.Instance.transform.position);
    }

    private void DeliveryManagerOnRecipeSuccess(object sender, EventArgs e)
    {
        PlaySound(_audioClipRefsSo.deliverySuccess, DeliveryCounter.Instance.transform.position);
    }

    private void PlaySound(AudioClip[] audioClip, Vector3 position, float volume = 1f)
    {
        PlaySound(
            audioClip[UnityEngine.Random.Range(0, audioClip.Length)],
            position,
            volume
        );
    }

    private void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f)
    {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }

    public void PlayFootstepsSound(Vector3 pos, float volume)
    {
        PlaySound(_audioClipRefsSo.footstep, pos, volume);
    }
}