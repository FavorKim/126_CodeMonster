using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : Singleton<InventoryManager>
{

    private RectTransform InventoryUIRectTransform;
    public RectTransform InventoryBoxColliderRectTransform;
    public BoxCollider InventoryBoxCollider;

    public void SetInventoryUISize(int InventoryNum)
    {
        InventoryUIRectTransform = GetComponent<RectTransform>();
        InventoryBoxCollider = GetComponent<BoxCollider>();
        InventoryBoxColliderRectTransform = GetComponentInChildren<BoxCollider>().GetComponent<RectTransform>();
        
        InventoryUIRectTransform.sizeDelta = new Vector2(InventoryNum * 60, 60);
        InventoryBoxCollider.size = new Vector2(InventoryNum * 60, 60);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("enter");

        // 코드 블럭이 UICodeBlockCollider랑 닿았을시
        if (other.tag == "CodeBlock")
        {
            Debug.Log("uicodeblock");
        }
    }
}
