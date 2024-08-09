using EnumTypes;
using UnityEngine;

public class CodeBlockDrag : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;
    private float zCoordinate;

    public BlockContainerManager inventory;
    public BlockType blockType;

    private RectTransform rectTransform;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnMouseDown()
    {
        zCoordinate = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        offset = GetMouseWorldPos() - (Vector3)rectTransform.anchoredPosition;
        isDragging = true;
    }

    private void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector3 newPosition = GetMouseWorldPos() - offset;

            rectTransform.anchoredPosition = new Vector2(newPosition.x, newPosition.y);
        }
    }

    private void OnMouseUp()
    {
        isDragging = false;
        if (inventory == null)
        {
            BlockContainerManager.Instance.RemoveCodeBlock(gameObject);
        }
        else
        {
            BlockContainerManager.Instance.AddBlock(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // 코드 블럭이 UICodeBlockCollider랑 닿았을시
        if (other.tag == "BlockContainerUI")
        {
            inventory = other.GetComponent<BlockContainerManager>();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        // 코드 블럭이 UICodeBlockCollider랑 닿았을시
        if (other.CompareTag("BlockContainerUI"))
        {
            inventory = other.GetComponent<BlockContainerManager>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 코드 블럭이 UI Code Block Collider랑 나갔을때는 _inventoryList null로 
        if (other.tag == "BlockContainerUI")
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