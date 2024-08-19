using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractEventManager : Singleton<InteractEventManager>
{
    [SerializeField] public CustomPokedObject startBtn;
    [SerializeField] CustomPokedObject resetBtn;
  
    public void RegistOnClickStartBtn(Action action)
    {
        startBtn.OnPoke += action;
    }

    public void RegistOnClickResetBtn(Action action)
    {
        resetBtn.OnPoke += action;
    }

    private void OnApplicationQuit()
    {
        startBtn.OnPoke = null;
        resetBtn.OnPoke = null;
    }
}
