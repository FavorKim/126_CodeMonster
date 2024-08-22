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
    [SerializeField]private MaterialChanger matChanger;
    private CustomGrabObject grab;

    

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        if(gameObject.TryGetComponent(out CustomGrabObject grabobj))
        {
            grab = grabobj;
        }
        PoolParent = transform.parent.gameObject;
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
            Debug.LogWarning($"OnEnable : {gameObject.name}'s grab is null");

            //DebugBoxManager.Instance.Txt_DebugMsg.text = "Event Didn't Regist";
        }
        if (matChanger == null)
            matChanger = GetComponent<MaterialChanger>();

        matChanger.ChangeMaterial(MaterialType.NORMAL_CODEBLOCK_MATERIAL);
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

    

    private void OnBoxRelease()
    {
        if (BlockContainerUI != null && BlockContainerUI.transform.childCount < UIManager.Instance.BlockContainerLength)
        {
            BlockContainerManager.Instance.AddBlock(gameObject);
        }
        else
        {
            ReturnToPool();
        }
    }

    private void OnBoxGrabbed()
    {
        // 부모 변경 전에 현재 월드 좌표를 저장
        Vector3 worldPositionBeforeChange = _rectTransform.position;


        // 부모 변경 후에도 같은 월드 좌표를 유지하도록 다시 설정
        _rectTransform.position = worldPositionBeforeChange;

        
        if (BlockContainerUI == null)
        {
            GameObject objInstance = ObjectPoolManager.Instance.GetObject(BlockName);
            objInstance.transform.rotation = Quaternion.Euler(new Vector3(45, 0, 0));

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "BlockContainerUI")
        {
            BlockContainerUI = other.GetComponent<BlockContainerManager>();
            matChanger.ChangeMaterial(MaterialType.OUTLINE_CODEBLOCK_MATERIAL);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "BlockContainerUI")
        {
            BlockContainerUI = null;
            matChanger.ChangeMaterial(MaterialType.NORMAL_CODEBLOCK_MATERIAL);
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
