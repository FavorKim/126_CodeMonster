using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutgameUIManager : MonoBehaviour
{
    [SerializeField] GameObject IntroUI;
    [SerializeField] GameObject ChapterUI;
    [SerializeField] GameObject StageUI;
    
    public void ClickStartButton()
    {
        IntroUI.SetActive(false);
        ChapterUI.SetActive(true);
    }

    public void ClickGoToIntroUI()
    {
        IntroUI.SetActive(true);
        ChapterUI.SetActive(false);
    }

    public void ClickChapter()
    {
        ChapterUI.SetActive(false);
        StageUI.SetActive(true);
    }

    public void ClickGoToChapter()
    {
        ChapterUI.SetActive(true);
        StageUI.SetActive(false);
    }

    public void ClickStage()
    {
        StageUI.SetActive(false);
    }
}
