using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OutgameUIManager : MonoBehaviour
{
    [SerializeField] GameObject IntroUI;
    [SerializeField] GameObject ChapterUI;
    [SerializeField] GameObject StageUI;
    [SerializeField] GameObject SelectCharacterUI;
    [SerializeField] GameObject ClearUI;
    [SerializeField] TextMeshProUGUI ChapterText; // 챕터 텍스트 UI

    string chapterNumber = ""; // 챕터 번호 저장 변수
    string stageNumber = ""; // 챕터 번호 저장 변수

    // 인트로에서 시작 버튼 클릭 시
    public void ClickStartButton()
    {
        IntroUI.SetActive(false);
        ChapterUI.SetActive(true);
    }

    // Select Chapter UI -> Intro UI
    public void ClickGoToIntroUI()
    {
        IntroUI.SetActive(true);
        ChapterUI.SetActive(false);
    }

    // Select Chapter UI -> Select Stage UI
    public void ClickChapter()
    {
        ChapterUI.SetActive(false);
        StageUI.SetActive(true);

        // 저장된 챕터 번호를 텍스트로 표시
        ChapterText.text = "챕터 " + chapterNumber;
    }

    // Select Stage UI -> Select Chapter UI 
    public void ClickGoToChapter()
    {
        ChapterUI.SetActive(true);
        StageUI.SetActive(false);
        UIManager.Instance.SelectChapterNum = 0;
    }

    // Select Stage UI -> Select Character UI
    public void ClickStage()
    {
        StageUI.SetActive(false);
        SelectCharacterUI.SetActive(true);
    }

    public void ClickGoToStage()
    {
        StageUI.SetActive(true);
        SelectCharacterUI.SetActive(false);
        UIManager.Instance.SelectStageNum = 0;
    }

    public void ClickStartStage()
    {
        SelectCharacterUI.SetActive(false);
        UIManager.Instance.OnStartStage();
    }

    

    // 챕터 버튼 클릭 시 호출되는 함수
    public void OnChapterClick(GameObject clickedObject)
    {
        // 클릭할 때마다 chapterNumber를 초기화하여 중복 방지
        chapterNumber = "";

        // 오브젝트 이름에서 숫자를 추출 (예: "CHAPTER1")
        string objectName = clickedObject.name; // 예: "CHAPTER1", "CHAPTER2"

        // 이름에서 숫자만 추출 (숫자라면 chapterNumber에 저장)
        foreach (char c in objectName)
        {
            if (char.IsDigit(c))
            {
                chapterNumber += c;  // 숫자를 chapterNumber에 추가
            }
        }

        if (!string.IsNullOrEmpty(chapterNumber))
        {
            int chapterValue = int.Parse(chapterNumber) * 1000;
            UIManager.Instance.SelectChapterNum = chapterValue;
            
        }
    }

    private void OnClickStageStart()
    {
        ClickStartStage();
        FieldManager.Instance.DisableAllMonsters();
    }
    private void OnClickBactToStage()
    {
        ClickGoToStage();
        UIManager.Instance.DisableIngameUI();
        UIManager.Instance.SelectCharacterUIManager.RemoveAllMonsters();
        FieldManager.Instance.EnableAllMonsters();
        FieldManager.Instance.MoveAllMonsters();
    }

    public void StartLoadingOnBackToStage()
    {
        GameManager.Instance.StartLoading(OnClickBactToStage);
    }

    public void StartLoadingOnStartStage()
    {
        GameManager.Instance.StartLoading(OnClickStageStart);
    }
    public void StartLoadingOnNextStage()
    {
        GameManager.Instance.StartLoading(GoToNextStage);
    }


    private void SetNextStageIndex()
    {
        if(UIManager.Instance.SelectStageNum<5)
        UIManager.Instance.SelectStageNum++;
        else
        {
            UIManager.Instance.SelectChapterNum += 1000;
            UIManager.Instance.SelectStageNum = 1;
        }
    }

    public void GoToNextStage()
    {
        Debug.LogError($"이전 스테이지 인덱스 : {UIManager.Instance.SelectChapterNum}챕터 {UIManager.Instance.SelectStageNum}스테이지");
        UIManager.Instance.DisableIngameUI();
        SetNextStageIndex();
        Debug.LogError($"변경 후 스테이지 인덱스 : {UIManager.Instance.SelectChapterNum}챕터 {UIManager.Instance.SelectStageNum}스테이지");

        ClickStage();
        FieldManager.Instance.OnPokeCharacterSelect();
    }

    public void OnStageClick(GameObject clickedObject)
    {
        // 클릭할 때마다 chapterNumber를 초기화하여 중복 방지
        stageNumber = "";

        // 오브젝트 이름에서 숫자를 추출 (예: "CHAPTER1")
        string objectName = clickedObject.name; // 예: "CHAPTER1", "CHAPTER2"

        // 이름에서 숫자만 추출 (숫자라면 chapterNumber에 저장)
        foreach (char c in objectName)
        {
            if (char.IsDigit(c))
            {
                stageNumber += c;  // 숫자를 chapterNumber에 추가
                UIManager.Instance.SelectStageNum = int.Parse(stageNumber);
            }
        }
    }

    public void SetClearUIActive(bool truefalse)
    {
        ClearUI.SetActive(truefalse);
        if(truefalse == true)
        {
            UIManager.Instance.DisableIngameUI();
        }
    }
}
