using EnumTypes;
using UnityEngine;

public class UICodeBlockDrag : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;
    private float zCoordinate;

    public BlockContainerManager inventory;

    private RectTransform rectTransform;
    private bool isUIElement = false;


    private void Start()
    {
        // RectTransform이 있는지 확인하여 UI 요소인지 판단
        rectTransform = GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            isUIElement = true;
        }
    }

    private void OnMouseDown()
    {
        zCoordinate = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        offset = GetMouseWorldPos() - (isUIElement ? (Vector3)rectTransform.anchoredPosition : transform.position);
        isDragging = true;
    }

    private void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector3 newPosition = GetMouseWorldPos() - offset;

            if (isUIElement)
            {
                rectTransform.anchoredPosition = new Vector2(newPosition.x, newPosition.y);
            }
            else
            {
                transform.position = newPosition;
            }
        }
    }

    private void OnMouseUp()
    {
        isDragging = false;
        if (inventory != null)
        {
            BlockContainerManager.Instance.RemoveCodeBlock(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("?");
        // 코드 블럭이 UICodeBlockCollider랑 닿았을시
        if (other.CompareTag("InventoryManager"))
        {
            inventory = other.GetComponent<BlockContainerManager>();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        Debug.Log("?");
        // 코드 블럭이 UICodeBlockCollider랑 닿았을시
        if (other.CompareTag("InventoryManager"))
        {
            inventory = other.GetComponent<BlockContainerManager>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 코드 블럭이 UI Code Block Collider랑 나갔을때는 _inventoryList null로 
        if (other.CompareTag("InventoryManager"))
        {
            inventory = null;
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoordinate;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}
