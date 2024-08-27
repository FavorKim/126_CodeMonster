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

        if (grab == null)
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
        if (grab != null)
        {
            grab.CustomOnRelease -= OnRelease_EnablePoke;
            grab.CustomOnGrab -= OnGrab_DisablePoke;
        }

    }


    private void OnGrab_DisablePoke()
    {
        if (poke.enabled == true)
        {
            poke.enabled = false;
        }
    }

    private void OnRelease_EnablePoke()
    {
        if (poke.enabled == false)
        {
            poke.enabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Renderer rend))
        {

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Renderer rend))
        {
            
        }
    }

}
