using EnumTypes;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class BlockContainerManager : Singleton<BlockContainerManager>
{
    private RectTransform BlockCountainerUIRectTransform;
    private BoxCollider BlockContainerBoxCollider;

    public Dictionary<EnumTypes.BlockType, GameObject> CodeBlockprefabDictionary;

    public void Start()
    {
        BlockCountainerUIRectTransform = GetComponent<RectTransform>();
        BlockContainerBoxCollider = GetComponent<BoxCollider>();
    }

    public void SetBlockContainerUISize(int InventoryNum)
    {
        BlockCountainerUIRectTransform.sizeDelta = new Vector2(InventoryNum * 60, 60);
        BlockContainerBoxCollider.size = new Vector2(InventoryNum * 60, 60);
    }

    public void AddBlock(GameObject newBlock)
    {
        // 현재 BlockContainer에 있는 모든 자식 블록들을 가져옵니다.
        List<Transform> existingBlocks = new List<Transform>();
        foreach (Transform child in this.transform)
        {
            existingBlocks.Add(child);
        }

        // 새로 추가될 블록의 월드 좌표
        Vector3 newBlockWorldPosition = newBlock.transform.position;

        // 모든 블록들의 월드 좌표를 비교하여 새로운 순서를 계산합니다.
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

        // 새 블록이 제일 마지막에 위치해야 할 경우
        if (!blockInserted)
        {
            sortedBlocks.Add(newBlock.transform);
        }

        // 이제 블록들을 새로운 순서대로 배치합니다.
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


    public void RemoveCodeBlock(GameObject RemoveBlock)
    {
        Destroy(RemoveBlock);
    }

    public void ChangeChildOrder(int oldIndex, int newIndex)
    {
        if (oldIndex < 0 || newIndex < 0 || oldIndex >= transform.childCount || newIndex >= transform.childCount)
        {
            Debug.LogWarning("Invalid indices provided for changing child order.");
            return;
        }

        Transform child = transform.GetChild(oldIndex);
        child.SetSiblingIndex(newIndex);
    }





}
