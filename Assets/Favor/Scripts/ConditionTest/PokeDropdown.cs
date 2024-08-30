using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PokeDropdown : MonoBehaviour
{
    [SerializeField] TMP_Dropdown dropDown;
    CustomPokedObject poke;

    private void Awake()
    {
        poke = GetComponent<CustomPokedObject>();
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.E))
        {
            dropDown.Show();
        }
    }
    private void Start()
    {
        poke.OnPoke += dropDown.Show;
    }
    private void OnApplicationQuit()
    {
        poke.OnPoke -= dropDown.Show;
    }

    public void OnPokeItem(Toggle toggle)
    {
        if (!toggle.isOn)
        {
            toggle.isOn = true;
        }

        int num = -1;
        Transform transform = toggle.transform;
        Transform parent = transform.parent;
        for (int i = 0; i < parent.childCount; i++)
        {
            if (parent.GetChild(i) == transform)
            {
                num = i - 1;
                break;
            }
        }

        if (num >= 0)
        {
            dropDown.value = num;
            dropDown.Hide();
        }
    }

}
