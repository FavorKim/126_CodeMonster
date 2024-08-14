using EnumTypes;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


public class BlockContainerManager : Singleton<BlockContainerManager>
{
    [SerializeField]private RectTransform BlockContainerUIRectTransform;
    private BoxCollider BlockContainerBoxCollider;

    List<CodeBlockData> blocks = new List<CodeBlockData>();

    protected override void Awake()
    {
        base.Awake();
        BlockContainerUIRectTransform = GetComponent<RectTransform>();
        BlockContainerBoxCollider = GetComponent<BoxCollider>();
    }

    public void SetBlockContainerUISize(int BlockContainerLength, bool PlusContainerUI)
    {
        if( PlusContainerUI == true)
        {
            BlockContainerUIRectTransform.sizeDelta = new Vector2(BlockContainerLength * UIConstants.BigUISize, UIConstants.RegularUISize);
            BlockContainerBoxCollider.size = new Vector2(BlockContainerLength * UIConstants.BigUISize, UIConstants.RegularUISize);
        }
        else
        {
            BlockContainerUIRectTransform.sizeDelta = new Vector2(BlockContainerLength * UIConstants.RegularUISize, UIConstants.RegularUISize);
            BlockContainerBoxCollider.size = new Vector2(BlockContainerLength * UIConstants.RegularUISize, UIConstants.RegularUISize);
        }

    }

    public void AddBlock(GameObject newBlock)
    {
        // 기존 블록들 existingBlocks에 저장
        List<Transform> existingBlocks = new List<Transform>();

        blocks.Clear();
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
            if (!blockInserted && newBlockWorldPosition.x < block.position.x)
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
            sortedBlocks[i].rotation = Quaternion.Euler(new Vector3(45,0,0));

            blocks.Add(sortedBlocks[i].gameObject.GetComponent<CodeBlockDrag>()._data);
        }
    }

    public List<CodeBlockData> GetContatinerBlocks()
    {
        return blocks;
    }
}
