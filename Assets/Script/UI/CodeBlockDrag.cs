using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; // 드롭다운을 확인하기 위해 필요

public class CodeBlockDrag : MonoBehaviour
{
    private bool _isDragging = false;
    private Vector3 _offset;
    private float _zCoordinate;

    private RectTransform _rectTransform;

    public BlockName BlockName;
    public BlockType BlockType;
    public BlockContainerManager BlockContainerUI;
    public GameObject PoolParent;

    [SerializeField] private CustomGrabObject grab;

    public CodeBlockData _data;


    private void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        /*
        if (TryGetComponent(out CustomGrabObject grabb))
        {
            grab = grabb;
            //grab.OnGrab += OnBoxGrabbed;
            //grab.OnRelease += OnBoxRelease;
            //DebugBoxManager.Instance.Txt_DebugMsg.text += "Event Successfully Regist\n";
        }
        else
        {
            DebugBoxManager.Instance.Txt_DebugMsg.text += "grab Didn't Init\n";
        }
        */
        PoolParent = transform.parent.gameObject;
        _data = SetData(BlockName);
    }

    private CodeBlockData SetData (BlockName bName)
    {
        CodeBlockData data = null;

        switch (bName)
        {
            case BlockName.LeftMoveCodeBlock:
                DataManagerTest.Inst.GetCodeBlockData("");
                break;
            case BlockName.RightMoveCodeBlock:
                DataManagerTest.Inst.GetCodeBlockData("");
                break;
            case BlockName.UpMoveCodeBlock:
                DataManagerTest.Inst.GetCodeBlockData("");
                break;
            case BlockName.DownMoveCodeBlock:
                DataManagerTest.Inst.GetCodeBlockData("");
                break;
            case BlockName.FireAttackCodeBlock:
                DataManagerTest.Inst.GetCodeBlockData("");
                break;
            case BlockName.WaterAttackCodeBlock:
                DataManagerTest.Inst.GetCodeBlockData("");
                break;
            case BlockName.GrassAttackCodeBlock:
                DataManagerTest.Inst.GetCodeBlockData("");
                break;
            case BlockName.LoopCodeBlock:
                DataManagerTest.Inst.GetCodeBlockData("");
                break;
            case BlockName.CondionalCodeBlock:
                DataManagerTest.Inst.GetCodeBlockData("");
                break;
            default:
                break;
        }

        return data;
    }

    private void OnEnable()
    {
        if (grab != null)
        {
            grab.OnGrab += OnBoxGrabbed;
            grab.OnRelease += OnBoxRelease;
            //DebugBoxManager.Instance.Txt_DebugMsg.text += "Event Successfully Regist";
        }
        else
        {
            //DebugBoxManager.Instance.Txt_DebugMsg.text = "Event Didn't Regist";
        }
    }
    private void OnDisable()
    {
        if (grab != null)
        {
            grab.OnRelease -= OnBoxRelease;
            grab.OnGrab -= OnBoxGrabbed;
        }
    }

    //private void OnMouseDown()
    //{
    //    // 부모 변경 전에 현재 월드 좌표를 저장
    //    Vector3 worldPositionBeforeChange = _rectTransform.position;

    //    // 부모를 UIManager로 변경
    //    Transform uiManagerTransform = GameObject.Find("UIManager").transform;
    //    transform.SetParent(uiManagerTransform, false); // 부모 변경, 월드 좌표는 일단 무시

    //    // 부모 변경 후에도 같은 월드 좌표를 유지하도록 다시 설정
    //    _rectTransform.position = worldPositionBeforeChange;

    //    _zCoordinate = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
    //    _offset = GetMouseWorldPos() - (Vector3)_rectTransform.anchoredPosition;
    //    _isDragging = true;

    //    if (BlockContainerUI == null)
    //    {
    //        GameObject objInstance = ObjectPoolManager.GetObject(BlockName);
    //    }
    //}

    //private void OnMouseDrag()
    //{
    //    if (_isDragging)
    //    {
    //        Vector3 newPosition = GetMouseWorldPos() - _offset;
    //        _rectTransform.anchoredPosition = new Vector2(newPosition.x, newPosition.y);
    //    }
    //}

    //private void OnMouseUp()
    //{
    //    _isDragging = false;

    //    if (BlockContainerUI != null && BlockContainerUI.transform.childCount < UIManager.Instance.BlockContainerLength)
    //    {
    //        BlockContainerManager.Instance.AddBlock(gameObject);
    //    }
    //    else
    //    {
    //        transform.SetParent(PoolParent.transform, false);

    //        // SetParent 바뀌어서 피벗 맞춰주기
    //        _rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
    //        _rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
    //        _rectTransform.pivot = new Vector2(0.5f, 0.5f);

    //        _rectTransform.localPosition = Vector3.zero;
    //        ObjectPoolManager.Instance.ReturnObject(gameObject, BlockName);
    //    }
    //}

    private void OnBoxRelease()
    {
        if (!_isDragging) return;
        if (BlockContainerUI != null && BlockContainerUI.transform.childCount < UIManager.Instance.BlockContainerLength)
        {
            BlockContainerManager.Instance.AddBlock(gameObject);
            //DebugBoxManager.Instance.Txt_DebugMsg.text += "Add Box";
        }
        else
        {
            //DebugBoxManager.Instance.Txt_DebugMsg.text += "Reset Box";
            transform.SetParent(PoolParent.transform, false);

            // SetParent 바뀌어서 피벗 맞춰주기
            _rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
            _rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
            _rectTransform.pivot = new Vector2(0.5f, 0.5f);

            _rectTransform.localPosition = Vector3.zero;
            ObjectPoolManager.Instance.ReturnObject(gameObject, BlockName);
        }

        _isDragging = false;
    }
    private void OnBoxGrabbed()
    {
        // 부모 변경 전에 현재 월드 좌표를 저장
        Vector3 worldPositionBeforeChange = _rectTransform.position;

        // 부모를 UIManager로 변경
        Transform uiManagerTransform = GameObject.Find("UIManager").transform;
        transform.SetParent(uiManagerTransform, false); // 부모 변경, 월드 좌표는 일단 무시

        // 부모 변경 후에도 같은 월드 좌표를 유지하도록 다시 설정
        _rectTransform.position = worldPositionBeforeChange;

        _zCoordinate = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        _offset = GetMouseWorldPos() - (Vector3)_rectTransform.anchoredPosition;
        _isDragging = true;

        if (BlockContainerUI == null)
        {
            GameObject objInstance = ObjectPoolManager.GetObject(BlockName);
            objInstance.transform.rotation = Quaternion.Euler(new Vector3(45, 0, 0));

            //DebugBoxManager.Instance.Txt_DebugMsg.text += "Copied";
        }
        //else
            //DebugBoxManager.Instance.Txt_DebugMsg.text += "NotCopied";
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
        mousePoint.z = _zCoordinate;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}
