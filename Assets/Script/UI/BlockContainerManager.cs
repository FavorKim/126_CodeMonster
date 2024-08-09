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

    public Dictionary<BlockType, GameObject> CodeBlockprefabDictionary;

    public void Start()
    {
        BlockCountainerUIRectTransform = GetComponent<RectTransform>();
        BlockContainerBoxCollider = GetComponent<BoxCollider>();
        //CodeBlockprefabDictionary = new Dictionary<BlockType, GameObject>
        //{
        //    { BlockType.RedCodeBlock, RedCodeBlock },
        //    { BlockType.BlueCodeBlock, BlueCodeBlock },
        //    { BlockType.GreenCodeBlock, GreenCodeBlock }
        //};
    }

    public void SetBlockContainerUISize(int InventoryNum)
    {
        BlockCountainerUIRectTransform.sizeDelta = new Vector2(InventoryNum * 60, 60);
        BlockContainerBoxCollider.size = new Vector2(InventoryNum * 60, 60);
    }

    public void AddCodeBlock(BlockType blockType)
    {
        if (CodeBlockprefabDictionary.TryGetValue(blockType, out GameObject prefabToAdd))
        {
            GameObject instance = Instantiate(prefabToAdd, this.transform);
        }
        else
        {
            Debug.LogWarning("Prefab type not found: " + blockType);
        }
    }

    public void AddBlock(GameObject gameObject)
    {
        gameObject.transform.SetParent(this.transform);
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
