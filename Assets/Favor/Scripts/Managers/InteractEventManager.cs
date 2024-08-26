using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractEventManager : Singleton<InteractEventManager>
{
    [SerializeField] public CustomPokedObject startBtn;
    [SerializeField] CustomPokedObject resetBtn;
    [SerializeField] CustomPokedObject restartBtn;
    [SerializeField] CustomPokedObject pauseBtn;
  
    public void RegistOnClickStartBtn(Action action)
    {
        startBtn.OnPoke += action;
    }

    public void RegistOnClickResetBtn(Action action)
    {
        resetBtn.OnPoke += action;
    }
    public void RegistOnClickRestartBtn(Action action)
    {
        restartBtn.OnPoke += action;
    }
    public void RegistOnClickPauseBtn(Action action)
    {
        pauseBtn.OnPoke += action;
    }

    private void OnApplicationQuit()
    {
        startBtn.OnPoke = null;
        resetBtn.OnPoke = null;
        restartBtn.OnPoke = null;
        pauseBtn.OnPoke = null;
    }
}
