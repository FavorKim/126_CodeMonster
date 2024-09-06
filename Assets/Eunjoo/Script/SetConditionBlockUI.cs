using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetConditionBlockUI : MonoBehaviour
{
    [SerializeField] GameObject TrueBlockBox;
    [SerializeField] GameObject FalseBlockBox;
    [SerializeField] GameObject ConditionBlockListImage;

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
        newBlock.transform.SetParent(TrueBlockBox.transform, false);
        newBlock.transform.localScale = Vector3.one;
    }

    public void AddFalseBlock(GameObject newBlock)
    {
        newBlock.transform.SetParent(FalseBlockBox.transform, false);
        newBlock.transform.localScale = Vector3.one;
    }
}
