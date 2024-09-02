using EnumTypes;
using EventLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeLoopBlockContainerManager : BlockContainerManager
{

    private void Start()
    {
        InteractEventManager.Instance.RegistOnPokeBtn(PokeButton.LOOPMAKE, UIManager.Instance.MakeLoopBlockBoxEnable);
        InteractEventManager.Instance.RegistOnPokeBtn(PokeButton.LOOPMAKE, GetMakeLoopBlocksName);
    }

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("UPDATE");
            GetMakeLoopBlocksName();
        }
    }


    public override void AddBlock(GameObject newBlock)
    {
        // 기존 블록들 existingBlocks에 저장
        List<Transform> existingBlocks = new List<Transform>();

        foreach (Transform child in this.transform)
        {
            existingBlocks.Add(child);
        }

        // 새로 추가될 블록의 월드 좌표
        Vector3 newBlockWorldPosition = newBlock.transform.position;

        // existingBlocks에 있는 블럭들과 새 블록의 월드 좌표를 비교해서
        // 순서대로 sortedBlocks List에 저장
        List<Transform> sortedBlocks = new List<Transform>();

        bool blockInserted = false;
        foreach (Transform block in existingBlocks)
        {
            if (!blockInserted && newBlockWorldPosition.y > block.position.y)
            {
                sortedBlocks.Add(newBlock.transform);
                blockInserted = true;
            }
            sortedBlocks.Add(block);
        }

        // 새 블록이 제일 마지막에 위치할 경우
        if (!blockInserted)
        {
            sortedBlocks.Add(newBlock.transform);
        }

        // sortedBlock 순대로 BlockContainer UI 하위로 정렬
        for (int i = 0; i < sortedBlocks.Count; i++)
        {
            sortedBlocks[i].SetParent(this.transform, false);
            // Z 좌표를 0으로 설정
            Vector3 newPosition = sortedBlocks[i].localPosition;
            newPosition.z = 0;
            sortedBlocks[i].localPosition = newPosition;
            sortedBlocks[i].SetSiblingIndex(i);
            sortedBlocks[i].rotation = Quaternion.Euler(new Vector3(45, 0, 0));
        }
    }

    public override void ResetBlockContainer()
    {
        base.ResetBlockContainer();
    }

    public override List<int> GetContatinerBlocks()
    {
        return base.GetContatinerBlocks();
    }

    public override void SetBlockMaterial(int index, MaterialType type)
    {
        base.SetBlockMaterial(index, type);
    }

    public override int CountCodeBlockDragComponents()
    {
        return base.CountCodeBlockDragComponents();
    }

    public void GetMakeLoopBlocksName()
    {
        UIManager.Instance.LoopBlockList.Clear();

        if (this.transform.childCount <= 0)
        {
            DebugBoxManager.Instance.Log("루프블록 자식 갯수 0");
            return;
        }

        for (int i = 0; i < this.transform.childCount; i++)
        {
            BlockName codeBlockDrag = transform.GetChild(i).GetComponent<CodeBlockDrag>().BlockName;
            UIManager.Instance.LoopBlockList.Add(codeBlockDrag);
        }
    }

    public void LoseMakeLoopBlocksName()
    {
        ResetBlockContainer();
        UIManager.Instance.LoopBlockList.Clear();
    }
}
