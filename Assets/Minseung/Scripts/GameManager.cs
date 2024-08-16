using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] CustomPokedObject startBtn;
    private void OnEnable()
    {
        RegistEvent();
    }
    private void OnDisable()
    {
        UnRegistEvent();
    }
    void RegistEvent()
    {
        startBtn.OnPoke += CodeBlockManager.StartCodeBlocks;
    }
    void UnRegistEvent()
    {
        startBtn.OnPoke -= CodeBlockManager.StartCodeBlocks;
    }
}
