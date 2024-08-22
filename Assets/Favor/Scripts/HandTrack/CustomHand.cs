using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class CustomHand : MonoBehaviour
{
    [SerializeField] CustomGrabInteractor grab;
    [SerializeField] PokeInteractor poke;

    private void OnEnable()
    {
        
        if(grab == null)
        {
            grab = GetComponentInChildren<CustomGrabInteractor>();
            poke = GetComponentInChildren<PokeInteractor>();
        }
        if (grab != null)
        {
            grab.CustomOnGrab += OnGrab_DisablePoke;
            grab.CustomOnRelease += OnRelease_EnablePoke;
        }
    }

    private void OnDisable()
    {
        
        if(grab != null)
        {
            grab.CustomOnRelease -= OnGrab_DisablePoke;
            grab.CustomOnGrab -= OnGrab_DisablePoke;
        }
    }


    private void OnGrab_DisablePoke()
    {
        poke.enabled = false;
        DebugBoxManager.Instance.Log("포크 비활성화");
    }

    private void OnRelease_EnablePoke()
    {
        grab.enabled = true;
        DebugBoxManager.Instance.Log("포크 활성화");
    }
    event PropertyChangedEventHandler OnPropertyChanged;
}
