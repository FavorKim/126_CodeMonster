using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetLoopBlockUI : MonoBehaviour
{
    [SerializeField] GameObject LoopBlockListBox;
    [SerializeField] GameObject LoopBlockListImage;
    [SerializeField] TMP_Text Text_LoopCountIndicator;

    public List<int> LoopBlockList = new List<int>();
    private List<MaterialChanger> materialChangers = new List<MaterialChanger>();

    public int loopCount;
    public int LoopCount 
    {
        get 
        {
            return loopCount;
        }
        set
        {
            loopCount = value;
        }
    }

    public int CountLoopBlockListBox()
    {
        return LoopBlockListBox.transform.childCount;
    }

    public void ResetBlock()
    {
        // 현재 컨테이너 하위에 있는 모든 자식 객체를 리스트에 저장
        List<Transform> children = new List<Transform>();
        foreach (Transform child in LoopBlockListBox.transform)
        {
            children.Add(child);
        }

        // 저장한 리스트를 순회하면서 블록들을 리셋
        foreach (Transform child in children)
        {
            CodeBlockDrag codeBlockDrag = child.GetComponent<CodeBlockDrag>();


            if (codeBlockDrag != null)
            {
                codeBlockDrag.ReturnToPool(); // 블록을 풀로 반환
            }
        }
    }

    public void EnableLoopBlockImage()
    {
        LoopBlockListImage.SetActive(true);
    }

    public void DisableLoopBlockImage()
    {
        LoopBlockListImage.SetActive(false);
    }

    public void AddBlock(GameObject newBlock)
    {
        newBlock.transform.SetParent(LoopBlockListBox.transform, false);
        newBlock.transform.localScale = Vector3.one;
        materialChangers.Add(newBlock.GetComponent<MaterialChanger>());
    }

    public void AddBlockName(int newBlock)
    {
        LoopBlockList.Add(newBlock);
    }

    public void ClearLoopBlockList()
    {
        LoopBlockList.Clear();
    }
    public void SetLoopCountText(int loopCount)
    {
        Text_LoopCountIndicator.text = $"{loopCount}회 반복";
    }
    public void SetBlockMaterial(int index, MaterialType type)
    {
        if(materialChangers.Count == 0)
        {
            for (int i = 0; i < LoopBlockListBox.transform.childCount; i++) 
            {
                materialChangers.Add(LoopBlockListBox.transform.GetChild(i).GetComponent<MaterialChanger>());
            }
        }
        materialChangers[index].ChangeMaterial(type);
    }

    public SetConditionBlockUI GetConditionByIndex(int index)
    {
        //DebugBoxManager.Instance.Log($"Loop : 매개 인덱스 : {index}, 차일드 카운트 : {LoopBlockListBox.transform.childCount}");

        if (LoopBlockList[index]== 8)
        {
            SetConditionBlockUI cond = LoopBlockListBox.transform.GetChild(index).GetComponent<SetConditionBlockUI>();
            return cond;
        }
        else
        {
            DebugBoxManager.Instance.Log("조건블록이 아닙니다 (루프블록)");
            return null;
        }
    }
    public ConditionBlock GetConditionBlockByIndex(int index)
    {
        //DebugBoxManager.Instance.Log($"Loop cb : 매개 인덱스 : {index}, 차일드 카운트 : {LoopBlockListBox.transform.childCount}");

        if (LoopBlockList[index] == 8)
        {
            ConditionBlock cond = LoopBlockListBox.transform.GetChild(index).GetComponent<ConditionBlock>();
            return cond;
        }
        else
        {
            DebugBoxManager.Instance.Log("조건블록이 아닙니다 (루프블록)");
            return null;
        }
    }
}
