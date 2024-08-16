using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebugBoxManager : Singleton<DebugBoxManager>
{
    public TMP_Text Txt_DebugMsg;
    [SerializeField] CustomPokedObject startBtn;
    [SerializeField] CustomPokedObject resetBtn;

    private void OnEnable()
    {
        startBtn.OnPoke += DebugOnPoke;
        resetBtn.OnPokeRelease += DebugOnPokeRelease;
    }

    private void OnDisable()
    {
        resetBtn.OnPokeRelease -= DebugOnPokeRelease;
        startBtn.OnPoke -= DebugOnPoke;
    }

    void DebugOnPoke()
    {
        Txt_DebugMsg.text += "Poked";
    }
    void DebugOnPokeRelease()
    {
        Txt_DebugMsg.text += "UnPoked";
    }
}
