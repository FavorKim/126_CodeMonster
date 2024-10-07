using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetConditionBlockUI : MonoBehaviour
{
    [SerializeField] GameObject TrueBlockBox;
    [SerializeField] GameObject FalseBlockBox;
    [SerializeField] TextMeshProUGUI TrueText;
    [SerializeField] TextMeshProUGUI FalseText;
    [SerializeField] GameObject ConditionBlockListImage;



    private void Start()
    {
        DisableConditionBlockListImage();
    }
    public void ResetBlock()
    {
        if (TrueBlockBox.transform.childCount > 0)
        {
            Transform child = TrueBlockBox.transform.GetChild(0); // 첫 번째 자식 객체
            CodeBlockDrag codeBlockDrag = child.GetComponent<CodeBlockDrag>();

            if (codeBlockDrag != null)
            {
                codeBlockDrag.ReturnToPool(); // 블록을 풀로 반환
            }
        }

        if (FalseBlockBox.transform.childCount > 0)
        {
            Transform child = FalseBlockBox.transform.GetChild(0); // 첫 번째 자식 객체
            CodeBlockDrag codeBlockDrag = child.GetComponent<CodeBlockDrag>();

            if (codeBlockDrag != null)
            {
                codeBlockDrag.ReturnToPool(); // 블록을 풀로 반환
            }
        } 
    }


    public void EnableConditionBlockListImage()
    {
        ConditionBlockListImage.SetActive(true);
    }

    public void DisableConditionBlockListImage()
    {
        ConditionBlockListImage.SetActive(false);
    }

    public void AddTrueBlock(GameObject newBlock)
    {
        //newBlock.transform.parent = null;
        newBlock.transform.SetParent(TrueBlockBox.transform, false);
        //newBlock.transform.parent = TrueBlockBox.transform;
        newBlock.transform.localScale = Vector3.one;
    }

    public void AddFalseBlock(GameObject newBlock)
    {
        //newBlock.transform.parent = null;
        newBlock.transform.SetParent(FalseBlockBox.transform, false);
        //newBlock.transform.parent = FalseBlockBox.transform;
        newBlock.transform.localScale = Vector3.one;
    }

    public void TrueTextColorChangeRed()
    {
        TrueText.color = Color.red;
    }

    public void TrueTextColorChangeWhite()
    {
        TrueText.color = Color.white;
    }
    public void FalseTextColorChangeRed()
    {
        FalseText.color = Color.red;
    }

    public void FalseTextColorChangeWhite()
    {
        FalseText.color = Color.white;
    }

    public void AccentTrueBlock()
    {
        TrueTextColorChangeRed();
        FalseTextColorChangeWhite();
        CodeBlockDrag trueBlock = TrueBlockBox.transform.GetChild(0).GetComponent<CodeBlockDrag>();
        CodeBlockDrag falseBlock = FalseBlockBox.transform.GetChild(0).GetComponent<CodeBlockDrag>();
        trueBlock.SetMaterial(MaterialType.OUTLINE_CODEBLOCK_MATERIAL);
        falseBlock.SetMaterial(MaterialType.USE_CODEBLOCK_MATERIAL);
    }

    public void AccentFalseBlock()
    {
        FalseTextColorChangeRed();
        TrueTextColorChangeWhite();
        CodeBlockDrag trueBlock = TrueBlockBox.transform.GetChild(0).GetComponent<CodeBlockDrag>();
        CodeBlockDrag falseBlock = FalseBlockBox.transform.GetChild(0).GetComponent<CodeBlockDrag>();
        falseBlock.SetMaterial(MaterialType.OUTLINE_CODEBLOCK_MATERIAL);
        trueBlock.SetMaterial(MaterialType.USE_CODEBLOCK_MATERIAL);
    }

    public void SetTrueText(int dropDownValue)
    {
        switch (dropDownValue)
        {
            case 1:
                TrueText.text = "불속성 공격을 받으면";
                break;
            case 2:
                TrueText.text = "물속성 공격을 받으면";
                break;
            case 3:
                TrueText.text = "풀속성 공격을 받으면";
                break;
            default:
                break;
        }
    }
}
