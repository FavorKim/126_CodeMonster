using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("UI List")]
    public GameObject InventoryUI;

    [Header("Inventory UI")]
    public int InventoryNum;
    public GameObject InventoryListPrefab;

    private RectTransform InventoryUIRectTransform;

    void Start()
    {
        // InventoryNum = datamanager.instance.getstagedata(stageindex).codeblocklength;
        SetInventoryUISize();
    }

    private void SetInventoryUISize()
    {
        InventoryUIRectTransform = InventoryUI.GetComponent<RectTransform>();
        InventoryUIRectTransform.sizeDelta = new Vector2(InventoryNum * 60, 60);

        for(int i =0; i< InventoryNum; i++)
        {
            Instantiate(InventoryListPrefab, InventoryUI.transform);
        }
    }
}
