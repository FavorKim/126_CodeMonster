using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public enum PokeButton
{
    START,
    RESET,
    RESTART,
    PAUSE,
    HINT,
    LOOPMAKE,
    CONDITIONMAKE,
    LOOPRESET,
    CONDITIONRESET,
}
public class InteractEventManager : Singleton<InteractEventManager>
{
    [SerializeField] CustomPokedObject startBtn;
    [SerializeField] CustomPokedObject resetBtn;
    [SerializeField] CustomPokedObject restartBtn;
    [SerializeField] CustomPokedObject pauseBtn;
    [SerializeField] CustomPokedObject hintBtn;
    [SerializeField] CustomPokedObject loopMakeBtn;
    [SerializeField] CustomPokedObject conditionMakeBtn;
    [SerializeField] CustomPokedObject loopResetBtn;
    [SerializeField] CustomPokedObject conditionResetBtn;
    

    private Dictionary<PokeButton, CustomPokedObject> btnDict = new Dictionary<PokeButton, CustomPokedObject>();

    private void Awake()
    {
        InitDict();   
    }

    private void InitDict()
    {
        if (btnDict.Count == 0)
        {
            btnDict.Add(PokeButton.START, startBtn);
            btnDict.Add(PokeButton.RESET, resetBtn);
            btnDict.Add(PokeButton.RESTART, restartBtn);
            btnDict.Add(PokeButton.PAUSE, pauseBtn);
            btnDict.Add(PokeButton.HINT, hintBtn);
            btnDict.Add(PokeButton.LOOPMAKE, loopMakeBtn);
            btnDict.Add(PokeButton.CONDITIONMAKE, conditionMakeBtn);
            btnDict.Add(PokeButton.LOOPRESET, loopResetBtn);
            btnDict.Add(PokeButton.CONDITIONRESET, conditionResetBtn);
        }
    }
    public void RegistOnPokeBtn(PokeButton btn, UnityAction action)
    {
        InitDict();
        btnDict[btn].OnPoke?.AddListener(action);// += action;
    }



    private void OnApplicationQuit()
    {
        foreach (var btn in btnDict.Values)
        {
            btn.OnPoke = null;
        }
    }
}
