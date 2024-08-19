using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractEventManager : Singleton<InteractEventManager>
{
    [SerializeField] CustomPokedObject startBtn;
    [SerializeField] CustomPokedObject resetBtn;
    public Action OnClickStartBtn;

    private void OnEnable()
    {
        OnClickStartBtn += startBtn.OnPoke;
        RegistOnResetBtn();
    }

    private void OnDisable()
    {
        UnRegistOnResetBtn();
        UnRegistOnStartBtn();
    }
    public void RegistOnStartBtn()
    {
        //startBtn.OnPoke.AddListener(StageManager.Instance.GetPlayer().StartPlayerAction);
    }
    public void RegistOnResetBtn()
    {
        resetBtn.OnPoke+=BlockContainerManager.Instance.ResetBlockContainer;
    }

   void UnRegistOnResetBtn()
    {
        resetBtn.OnPoke-=BlockContainerManager.Instance.ResetBlockContainer;
    }
    void UnRegistOnStartBtn()
    {
        OnClickStartBtn -= startBtn.OnPoke;


    }
}
