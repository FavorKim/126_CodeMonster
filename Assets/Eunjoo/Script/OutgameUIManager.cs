using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class OutgameUIManager : MonoBehaviour
{
    [SerializeField] GameObject IntroUI;
    [SerializeField] GameObject ChapterUI;
    [SerializeField] GameObject StageUI;
    [SerializeField] TextMeshProUGUI ChapterText; // 챕터 텍스트 UI

    string chapterNumber = ""; // 챕터 번호 저장 변수

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

        // 저장된 챕터 번호를 텍스트로 표시
        ChapterText.text = "챕터 " + chapterNumber;
    }

    public void ClickGoToChapter()
    {
        ChapterUI.SetActive(true);
        StageUI.SetActive(false);
    }

    public void ClickStage()
    {
        StageUI.SetActive(false);
        UIManager.Instance.IngameUI.SetActive(true);
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
    }

}
