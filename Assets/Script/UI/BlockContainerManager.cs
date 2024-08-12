using EnumTypes;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


public class BlockContainerManager : Singleton<BlockContainerManager>
{
    private RectTransform BlockContainerUIRectTransform;
    private BoxCollider BlockContainerBoxCollider;

    public void Start()
    {
        BlockContainerUIRectTransform = GetComponent<RectTransform>();
        BlockContainerBoxCollider = GetComponent<BoxCollider>();
    }

    public void SetBlockContainerUISize(int BlockContainerLength, bool PlusContainerUI)
    {
        if( PlusContainerUI == true)
        {
            BlockContainerUIRectTransform.sizeDelta = new Vector2(BlockContainerLength * UIConstants.ConditionalCodeBoxSize, UIConstants.RegularBoxSize);
            BlockContainerBoxCollider.size = new Vector2(BlockContainerLength * UIConstants.ConditionalCodeBoxSize, UIConstants.RegularBoxSize);
        }
        else
        {
            BlockContainerUIRectTransform.sizeDelta = new Vector2(BlockContainerLength * UIConstants.RegularBoxSize, UIConstants.RegularBoxSize);
            BlockContainerBoxCollider.size = new Vector2(BlockContainerLength * UIConstants.RegularBoxSize, UIConstants.RegularBoxSize);
        }

    }

    public void AddBlock(GameObject newBlock)
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
        }
    }

    private void UpdateBlockContainerSize()
    {
        // 모든 자식 요소의 경계를 계산
        Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
        bool hasBounds = false;

        foreach (Transform child in transform)
        {
            RectTransform rect = child.GetComponent<RectTransform>();
            if (rect != null)
            {
                if (hasBounds)
                {
                    // Vector2를 Vector3로 변환하여 더하기
                    bounds.Encapsulate(rect.localPosition + new Vector3(rect.rect.min.x, rect.rect.min.y, 0));
                    bounds.Encapsulate(rect.localPosition + new Vector3(rect.rect.max.x, rect.rect.max.y, 0));
                }
                else
                {
                    // 처음에는 Vector3로 중앙을 설정
                    bounds = new Bounds(rect.localPosition + new Vector3(rect.rect.center.x, rect.rect.center.y, 0), new Vector3(rect.rect.size.x, rect.rect.size.y, 0));
                    hasBounds = true;
                }
            }
        }

        // 새로운 크기를 RectTransform과 BoxCollider에 반영
        BlockContainerUIRectTransform.sizeDelta = new Vector2(bounds.size.x, BlockContainerUIRectTransform.sizeDelta.y);
        BlockContainerBoxCollider.size = new Vector3(bounds.size.x, BlockContainerBoxCollider.size.y, BlockContainerBoxCollider.size.z);
    }

}
