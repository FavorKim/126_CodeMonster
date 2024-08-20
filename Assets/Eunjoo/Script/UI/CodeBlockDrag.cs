using EnumTypes;
using EventLibrary;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI; // 드롭다운을 확인하기 위해 필요

public class CodeBlockDrag : MonoBehaviour
{
    //private bool _isDragging = false;
    private Vector3 _offset;
    private float _zCoordinate;

    private RectTransform _rectTransform;

    public BlockName BlockName;
    public BlockType BlockType;
    public BlockContainerManager BlockContainerUI;
    public GameObject PoolParent;

    private CustomGrabObject grab;

    private CodeBlockData _data;
    public CodeBlockData Data
    {
        get { return _data; }
        private set
        {
            _data = value;
        }
    }
    

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        if(gameObject.TryGetComponent(out CustomGrabObject grabobj))
        {
            grab = grabobj;
        }
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
        //Data = SetData(BlockName);
    }

    /* 데이터 드리븐 SetData
    private CodeBlockData SetData (BlockName bName)
    {
        CodeBlockData data = null;

        switch (bName)
        {
            case BlockName.LeftMoveCodeBlock:
                DataManagerTest.Inst.GetCodeBlockData("Block_Move_Left");
                break;
            case BlockName.RightMoveCodeBlock:
                DataManagerTest.Inst.GetCodeBlockData("Block_Move_Right");
                break;
            case BlockName.UpMoveCodeBlock:
                DataManagerTest.Inst.GetCodeBlockData("Block_Move_Up");
                break;
            case BlockName.DownMoveCodeBlock:
                DataManagerTest.Inst.GetCodeBlockData("Block_Move_Down");
                break;
            case BlockName.FireAttackCodeBlock:
                DataManagerTest.Inst.GetCodeBlockData("Block_Attack_Fire");
                break;
            case BlockName.WaterAttackCodeBlock:
                DataManagerTest.Inst.GetCodeBlockData("Block_Attack_Water");
                break;
            case BlockName.GrassAttackCodeBlock:
                DataManagerTest.Inst.GetCodeBlockData("Block_Attac_Grass");
                break;
            case BlockName.LoopCodeBlock:
                DataManagerTest.Inst.GetCodeBlockData("");
                break;
            case BlockName.CondionalCodeBlock:
                DataManagerTest.Inst.GetCodeBlockData("Block_Condition_Attribute");
                break;
            default:
                break;
        }
        
        return data;
    }
    */
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
            Debug.LogWarning($"OnEnable : {gameObject.name}'s grab is null");

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
        else
            Debug.LogWarning($"OnDisable : {gameObject.name}'s grab is null");
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
        //if (_isDragging == false) return;
        //_isDragging = false;
        if (BlockContainerUI != null && BlockContainerUI.transform.childCount < UIManager.Instance.BlockContainerLength)
        {
            BlockContainerManager.Instance.AddBlock(gameObject);
            //DebugBoxManager.Instance.Txt_DebugMsg.text += "Add Box";
        }
        else
        {
            //DebugBoxManager.Instance.Txt_DebugMsg.text += "Reset Box";
            ReturnToPool();
        }
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
        //_isDragging = true;

        if (BlockContainerUI == null)
        {
            GameObject objInstance = ObjectPoolManager.Instance.GetObject(BlockName);
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

    /*
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("BlockContainerUI"))
        {
            BlockContainerUI = other.GetComponent<BlockContainerManager>();
        }
    }
    */

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

    public void ReturnToPool()
    {
        BlockContainerUI = null;

        transform.SetParent(PoolParent.transform, false);

        // SetParent 바뀌어서 피벗 맞춰주기
        _rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        _rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        _rectTransform.pivot = new Vector2(0.5f, 0.5f);

        _rectTransform.localPosition = Vector3.zero;
        ObjectPoolManager.Instance.ReturnObject(gameObject, BlockName);
        EventManager<UIEvent>.TriggerEvent(UIEvent.SetBlockCount, UIManager.Instance.BlockContainerLength - BlockContainerManager.Instance.CountCodeBlockDragComponents());
    }
}
