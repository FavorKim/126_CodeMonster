using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private void OnEnable()
    {
        RegistEvent();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            RestartGame();
        }
    }
    void RegistEvent()
    {
        InteractEventManager.Instance.RegistOnClickRestartBtn(RestartGame);
    }
    void RestartGame() 
    {
        SceneManager.LoadScene("PSW Test Scene");
    }

}
