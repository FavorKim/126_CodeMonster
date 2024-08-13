using Oculus.Interaction;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomPokedObject : MonoBehaviour
{
    PokeInteractable pokeInteractable;
    void Start()
    {
        pokeInteractable = GetComponent<PokeInteractable>();
    }
    private void OnEnable()
    {
        
    }
}
