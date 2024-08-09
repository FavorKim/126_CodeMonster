using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CodeBlock : MonoBehaviour
{
    public InventoryList _inventoryList;
    
    private void OnTriggerEnter(Collider other)
    {
        // 코드 블럭이 UICodeBlockCollider랑 닿았을시
        if (other.tag == "UICodeBlockCollider")
        {
            _inventoryList = other.GetComponentInParent<InventoryList>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 코드 블럭이 UI Code Block Collider랑 나갔을때는 _inventoryList null로 
        if (other.tag == "UICodeBlockCollider")
        {
            _inventoryList = null;
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _inventoryList != null)
        {
            // 블럭의 타입을 매개변수로 전달해주기
            _inventoryList.SetCodeBlock("test", "type");
        }
    }
}
