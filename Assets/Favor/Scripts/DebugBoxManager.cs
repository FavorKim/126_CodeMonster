using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugBoxManager : Singleton<DebugBoxManager>
{
    public TMP_Text Txt_DebugMsg;
    [SerializeField] CustomPokedObject startBtn;
    [SerializeField] CustomPokedObject resetBtn;
    [SerializeField] GameObject blockContainer;

    private void OnEnable()
    {
        startBtn.OnPoke += DebugOnPoke;
        //resetBtn.OnPokeRelease += DebugOnPokeRelease;
    }

    private void OnDisable()
    {
        //resetBtn.OnPokeRelease -= DebugOnPokeRelease;
        startBtn.OnPoke -= DebugOnPoke;
    }

    void DebugOnPoke()
    {
        for (int i = 0; i < blockContainer.transform.childCount; i++)
        {
            Txt_DebugMsg.text += $"{blockContainer.transform.GetChild(i)}\n";
        }
    }
    void DebugOnPokeRelease()
    {
        Txt_DebugMsg.text += "UnPoked";
    }
}
