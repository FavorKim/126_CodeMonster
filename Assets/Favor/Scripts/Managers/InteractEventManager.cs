using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum PokeButton
{
    START,
    RESET,
    RESTART,
    PAUSE,
    HINT,
    LOOPMAKE,
    CONDITIONMAKE,
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

    private Dictionary<PokeButton, CustomPokedObject> btnDict = new Dictionary<PokeButton, CustomPokedObject>();

    private void Awake()
    {
        btnDict.Add(PokeButton.START, startBtn);
        btnDict.Add(PokeButton.RESET, resetBtn);
        btnDict.Add(PokeButton.RESTART, restartBtn);
        btnDict.Add(PokeButton.PAUSE, pauseBtn);
        btnDict.Add(PokeButton.HINT, hintBtn);
        btnDict.Add(PokeButton.LOOPMAKE, loopMakeBtn);
        btnDict.Add(PokeButton.CONDITIONMAKE, conditionMakeBtn);
    }

    public void RegistOnPokeBtn(PokeButton btn, Action action)
    {
        btnDict[btn].OnPoke += action;
    }



    private void OnApplicationQuit()
    {
        startBtn.OnPoke = null;
        resetBtn.OnPoke = null;
        restartBtn.OnPoke = null;
        pauseBtn.OnPoke = null;
        hintBtn.OnPoke = null;
        loopMakeBtn.OnPoke = null;
        conditionMakeBtn.OnPoke = null;
    }
}
