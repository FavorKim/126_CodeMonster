using EnumTypes;
using UnityEngine;

public class CodeBlockDrag : MonoBehaviour
{
    private bool isDragging = false;
    private Vector3 offset;
    private float zCoordinate;

    public BlockContainerManager BlockContainerUI;
    public BlockType blockType;

    private RectTransform rectTransform;
    public Vector3 originalPosition; // 원래 위치를 저장할 변수

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnMouseDown()
    {
        // 부모 변경 전에 현재 월드 좌표를 저장
        Vector3 worldPositionBeforeChange = rectTransform.position;

        // 부모를 UIManager로 변경
        Transform uiManagerTransform = GameObject.Find("UIManager").transform;
        transform.SetParent(uiManagerTransform, false); // 부모 변경, 월드 좌표는 일단 무시

        // 부모 변경 후에도 같은 월드 좌표를 유지하도록 다시 설정
        rectTransform.position = worldPositionBeforeChange;

        zCoordinate = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        offset = GetMouseWorldPos() - (Vector3)rectTransform.anchoredPosition;
        isDragging = true;

        originalPosition = rectTransform.anchoredPosition;
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
        //rectTransform.anchoredPosition = originalPosition;

        if (BlockContainerUI != null)
        {
            BlockContainerManager.Instance.AddBlock(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "BlockContainerUI")
        {
            BlockContainerUI = other.GetComponent<BlockContainerManager>();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("BlockContainerUI"))
        {
            BlockContainerUI = other.GetComponent<BlockContainerManager>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "BlockContainerUI")
        {
            BlockContainerUI = null;
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = zCoordinate;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}
