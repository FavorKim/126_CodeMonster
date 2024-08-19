using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractEventManager : MonoBehaviour
{
    [SerializeField] CustomPokedObject startBtn;
    [SerializeField] CustomPokedObject resetBtn;
    

    private void OnEnable()
    {
        RegistOnResetBtn();
        RegistOnStartBtn();
    }

    private void OnDisable()
    {
        UnRegistOnResetBtn();
        UnRegistOnStartBtn();
    }
    private void RegistOnStartBtn()
    {
        
    }
    private void RegistOnResetBtn()
    {
        //resetBtn.OnPoke += BlockContainerManager.Instance.ResetBlockContainer;
    }

    void UnRegistOnResetBtn()
    {
        //resetBtn.OnPoke -= BlockContainerManager.Instance.ResetBlockContainer;
    }
    void UnRegistOnStartBtn()
    {

    }
}
