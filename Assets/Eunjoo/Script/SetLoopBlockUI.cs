using System.Collections.Generic;
using UnityEngine;

public class SetLoopBlockUI : MonoBehaviour
{
    [SerializeField] GameObject LoopBlockListBox;
    [SerializeField] GameObject LoopBlockListImage;
    public List<int> LoopBlockList = new List<int>();
    public int LoopCount { get; set; }

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
    }

    public void AddBlockName(int newBlock)
    {
        LoopBlockList.Add(newBlock);
    }

    public void ClearLoopBlockList()
    {
        LoopBlockList.Clear();
    }
}
