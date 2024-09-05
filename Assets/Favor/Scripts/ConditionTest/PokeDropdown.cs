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

    private void Start()
    {
        poke.OnPoke += OnPokeDropDown;
    }
    private void OnApplicationQuit()
    {
        poke.OnPoke -= OnPokeDropDown;
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

    // 현재 드랍다운에서 선택된 것의 텍스트를 반환
    public int GetSelectedValue()
    {
        return dropDown.value;
        //if(dropDown.IsExpanded)
    }

    private void OnPokeDropDown()
    {
        if (dropDown.IsExpanded)
        {
            dropDown.Hide();
        }
        else
        {
            dropDown.Show();
        }
    }
    
}
