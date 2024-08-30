using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PokeDropdown : MonoBehaviour
{
    Dropdown dropDown;
    CustomPokedObject poke;

    private void Awake()
    {
        dropDown = GetComponent<Dropdown>();
        poke = GetComponent<CustomPokedObject>();
    }

    private void OnEnable()
    {
        poke.OnPoke += dropDown.Show;
    }
    private void OnDisable()
    {
        poke.OnPoke -= dropDown.Show;
    }
}
