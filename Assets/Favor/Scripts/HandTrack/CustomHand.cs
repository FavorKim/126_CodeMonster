using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomHand : MonoBehaviour
{
    HandGrabInteractor grab;
    PokeInteractor poke;

    private void OnEnable()
    {
        if (grab != null)
        {
            grab.OnGrab += OnGrab_DisablePoke;
            grab.OnRelease += OnRelease_EnablePoke;
        }
    }

    private void OnDisable()
    {
        if(grab != null)
        {
            grab.OnRelease -= OnGrab_DisablePoke;
            grab.OnGrab -= OnGrab_DisablePoke;
        }
    }

    private void Awake()
    {
        grab = GetComponentInChildren<HandGrabInteractor>();
        poke = GetComponentInChildren<PokeInteractor>();
    }

    private void OnGrab_DisablePoke()
    {
        poke.enabled = false;
    }

    private void OnRelease_EnablePoke()
    {
        grab.enabled = true;
    }

}
