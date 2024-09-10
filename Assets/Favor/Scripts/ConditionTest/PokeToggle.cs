using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PokeToggle : MonoBehaviour
{
    [SerializeField] PokeDropdown PokeDropdown;
    CustomPokedObject poke;
    Toggle toggle;

    private void Awake()
    {
        poke = GetComponent<CustomPokedObject>();
        toggle = transform.parent.GetComponent<Toggle>();
    }

    private void OnEnable()
    {
        if (poke != null)
        {
            poke.OnPoke.AddListener(OnPoked);
        }
        else
        {
            DebugBoxManager.Instance.Log("포크 널");
        }
    }

    private void OnDisable()
    {
        if (poke != null)
        {
            poke.OnPoke.RemoveAllListeners();
        }
        poke.OnPoke = null;
    }

    private void OnPoked()
    {
        PokeDropdown.OnPokeItem(toggle);
        DebugBoxManager.Instance.Log($"{PokeDropdown.GetSelectedValue()}");
    }
}
