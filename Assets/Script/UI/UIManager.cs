using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : Singleton<UIManager>
{
    [Header("UI List")]
    public InventoryManager InventoryManager;

    [Header("Inventory UI")]
    public int InventoryNum;
    //public GameObject InventoryListPrefab;



    void Start()
    {
        // InventoryNum = datamanager.instance.getstagedata(stageindex).codeblocklength;
        InventoryManager.Instance.SetInventoryUISize(InventoryNum);
        //for(int i =0; i< InventoryNum; i++)
        //{
        //    Instantiate(InventoryListPrefab, InventoryUI.transform);
        //}
    }
}
