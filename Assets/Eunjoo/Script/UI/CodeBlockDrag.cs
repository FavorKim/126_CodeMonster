using EnumTypes;
using EventLibrary;
using Oculus.Interaction.HandGrab;
using System;
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
    public MakeLoopBlockContainerManager MakeLoopBlockUI;
    public MakeConditionBlockUIManager MakeConditionBlockUI;
    public GameObject PoolParent;
    [SerializeField] private MaterialChanger matChanger;
    private CustomGrabObject grab;

    bool isConditionTrue;
    bool isConditionFalse;






    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        if (gameObject.TryGetComponent(out CustomGrabObject grabobj))
        {
            grab = grabobj;
        }
        PoolParent = transform.parent.gameObject;
    }


    private void OnEnable()
    {
        if (grab != null)
        {
            grab.OnGrab.AddListener(OnBoxGrabbed);
            grab.OnRelease.AddListener(OnBoxRelease);
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
            grab.OnRelease.RemoveAllListeners();// -= OnBoxRelease;
            grab.OnGrab.RemoveAllListeners();// -= OnBoxGrabbed;
        }
        else
            Debug.LogWarning($"OnDisable : {gameObject.name}'s grab is null");
    }


    private void OnBoxRelease()
    {
        if (BlockType == BlockType.LoopCodeBlock)
        {
            SetLoopBlockUI SetLoopBlockUI = gameObject.GetComponentInChildren<SetLoopBlockUI>();
            SetLoopBlockUI.DisableLoopBlockImage();
        }

        if (BlockType == BlockType.ConditionalCodeBlock)
        {
            SetConditionBlockUI SetConditionBlockUI = gameObject.GetComponentInChildren<SetConditionBlockUI>();
            SetConditionBlockUI.DisableConditionBlockListImage();
        }

        matChanger.ChangeMaterial(MaterialType.NORMAL_CODEBLOCK_MATERIAL);
        // Container 내에 있고 자식 수가 BlockContainer Length 보다 적을 때 
        if (BlockContainerUI != null && BlockContainerUI.transform.childCount < UIManager.Instance.BlockContainerLength)
        {
            BlockContainerUI.AddBlock(gameObject);
        }
        // Container 내에 있고 자식 수가 BlockContainer Length 보다 많을 때
        else if (BlockContainerUI != null && BlockContainerUI.transform.childCount >= UIManager.Instance.BlockContainerLength)
        {
            EventManager<UIEvent>.TriggerEvent(UIEvent.SetBlockCountError);
            ReturnToPool();
        }
        // 반복문 등록부
        else if (MakeLoopBlockUI != null && MakeLoopBlockUI.transform.childCount < UIManager.Instance.MakeLoopBlockContainerLength)
        {
            MakeLoopBlockUI.AddBlock(gameObject);
        }
        else if (MakeLoopBlockUI != null && MakeLoopBlockUI.transform.childCount >= UIManager.Instance.MakeLoopBlockContainerLength)
        {
            ReturnToPool();
        }
        // 조건문 (if시 행동) 등록부
        else if (MakeConditionBlockUI != null && isConditionTrue)
        {
            //DebugBoxManager.Instance.Log("참 등록");
            MakeConditionBlockUI.SetConditionBlockPos(this, true);
            isConditionTrue = false;
        }
        // 조건문 (else시 행동) 등록부
        else if (MakeConditionBlockUI != null && isConditionFalse)
        {
            //DebugBoxManager.Instance.Log("거짓 등록");
            MakeConditionBlockUI.SetConditionBlockPos(this, false);
            isConditionFalse = false;
        }
        else if (MakeConditionBlockUI != null && (!isConditionFalse || !isConditionTrue))
        {
            ReturnToPool();
        }
        // Container 외에 있을때 
        else
        {
            ReturnToPool();
        }
    }


    private void OnMouseDown()
    {
        OnBoxGrabbed();
    }
    private void OnMouseUp()
    {
        OnBoxRelease();
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


        if (BlockContainerUI == null)
        {
            GameObject objInstance = ObjectPoolManager.Instance.GetObject(BlockName);
        }
        else
        {
            EventManager<UIEvent>.TriggerEvent(UIEvent.BlockCountainerBlockCount, UIManager.Instance.BlockContainerLength - UIManager.Instance.BlockContainerManager.CountCodeBlockDragComponents());
        }
        if (MakeLoopBlockUI != null)
        {
            EventManager<UIEvent>.TriggerEvent(UIEvent.LoopBlockContainerBlockCount, UIManager.Instance.MakeLoopBlockContainerLength - UIManager.Instance.MakeLoopBlockContainerManager.CountCodeBlockDragComponents());
        }


        if (BlockType == BlockType.LoopCodeBlock)
        {
            GrabLoopCodeBlock();
        }

        if (BlockType == BlockType.ConditionalCodeBlock)
        {
            GrabConditionCodeBlock();
        }
    }



    private void GrabLoopCodeBlock()
    {
        List<int> loopBlockNames = UIManager.Instance.LoopBlockList;
        foreach (int blockName in loopBlockNames)
        {

            if (TryGetComponent(out SetLoopBlockUI SetLoopBlockUI))
                //SetLoopBlockUI SetLoopBlockUI= gameObject.GetComponent<SetLoopBlockUI>();
                SetLoopBlockUI.EnableLoopBlockImage();
            else
            {
                DebugBoxManager.Instance.Log("SetLoopBlockUI NULL!");
                return;
            }

            if (SetLoopBlockUI.CountLoopBlockListBox() >= UIManager.Instance.LoopBlockList.Count) return;

            GameObject loopBlock = ObjectPoolManager.Instance.GetObject((BlockName)blockName - 1);
            if (blockName == 8)
            {
                DebugBoxManager.Instance.Log("루프블록 안에 조건블록");
                loopBlock.GetComponent<CodeBlockDrag>().GrabConditionCodeBlock();
                loopBlock.GetComponent<ConditionBlock>().OnGrabSetData();
            }
            HandGrabInteractable loopBlockHandGrab = loopBlock.GetComponent<HandGrabInteractable>();
            BoxCollider loopBlockCodeBlockBoxCollider = loopBlock.GetComponent<BoxCollider>();
            loopBlockCodeBlockBoxCollider.enabled = false;
            loopBlockHandGrab.enabled = false;
            SetLoopBlockUI.AddBlockName(blockName);
            SetLoopBlockUI.AddBlock(loopBlock);
            SetLoopBlockUI.LoopCount = UIManager.Instance.MakeLoopBlockContainerManager.GetLoopCount();
            SetLoopBlockUI.SetLoopCountText(UIManager.Instance.MakeLoopBlockContainerManager.GetLoopCount());
        }
    }


    private void GrabConditionCodeBlock()
    {
        SetConditionBlockUI setcon;
        if (TryGetComponent(out setcon))
        {
            setcon.EnableConditionBlockListImage();
        }
        else
        {
            DebugBoxManager.Instance.Log("SetConditionBlockUI NULL!");
            return;
        }
        ConditionBlock condition = GetComponent<ConditionBlock>();
        if (condition == null)
        {
            DebugBoxManager.Instance.Log("ConditionBlockNULL!");
            return;
        }
        condition = MakeConditionBlockUIManager.Instance.GetConditionBlockInfo();
        GameObject trueBlock = ObjectPoolManager.Instance.GetObject(condition.TrueBlock.BlockName);  // true block 이름
        HandGrabInteractable trueBlockHandGrab = trueBlock.GetComponent<HandGrabInteractable>();
        BoxCollider trueBlockCodeBlockBoxCollider = trueBlock.GetComponent<BoxCollider>();
        trueBlockHandGrab.enabled = false;
        trueBlockCodeBlockBoxCollider.enabled = false;
        setcon.AddTrueBlock(trueBlock);

        GameObject falseBlock = ObjectPoolManager.Instance.GetObject(condition.FalseBlock.BlockName);  // false block 이름
        HandGrabInteractable falseBlockHandGrab = trueBlock.GetComponent<HandGrabInteractable>();
        BoxCollider falseBlockCodeBlockBoxCollider = trueBlock.GetComponent<BoxCollider>();
        falseBlockHandGrab.enabled = false;
        falseBlockCodeBlockBoxCollider.enabled = false;
        setcon.AddFalseBlock(falseBlock);

        setcon.SetTrueText(condition.indexValue);
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.tag)
        {
            case "BlockContainerUI":
                BlockContainerUI = other.GetComponent<BlockContainerManager>();
                matChanger.ChangeMaterial(MaterialType.OUTLINE_CODEBLOCK_MATERIAL);
                break;
            case "MakeLoopBlockUI":
                MakeLoopBlockUI = other.GetComponent<MakeLoopBlockContainerManager>();
                matChanger.ChangeMaterial(MaterialType.OUTLINE_CODEBLOCK_MATERIAL);
                break;
            case "MakeConditionTrue":
                if (MakeConditionBlockUI == null)
                {
                    MakeConditionBlockUI = other.transform.parent.GetComponent<MakeConditionBlockUIManager>();
                }
                isConditionTrue = true;
                //DebugBoxManager.Instance.Log("참일 때 true");
                matChanger.ChangeMaterial(MaterialType.OUTLINE_CODEBLOCK_MATERIAL);
                break;
            case "MakeConditionFalse":
                if (MakeConditionBlockUI == null)
                {
                    MakeConditionBlockUI = other.transform.parent.GetComponent<MakeConditionBlockUIManager>();
                }
                isConditionFalse = true;
                //DebugBoxManager.Instance.Log("거짓일 때 true");
                matChanger.ChangeMaterial(MaterialType.OUTLINE_CODEBLOCK_MATERIAL);
                break;
        }
        //if (other.tag == "BlockContainerUI")
        //{
        //    BlockContainerUI = other.GetComponent<BlockContainerManager>();
        //    matChanger.ChangeMaterial(MaterialType.OUTLINE_CODEBLOCK_MATERIAL);
        //}

    }



    private void OnTriggerExit(Collider other)
    {

        switch (other.tag)
        {
            case "BlockContainerUI":
                BlockContainerUI = null;
                matChanger.ChangeMaterial(MaterialType.NORMAL_CODEBLOCK_MATERIAL);
                break;
            case "MakeLoopBlockUI":
                MakeLoopBlockUI = null;
                matChanger.ChangeMaterial(MaterialType.NORMAL_CODEBLOCK_MATERIAL);
                break;
            case "MakeConditionTrue":
                if (MakeConditionBlockUI != null)
                    MakeConditionBlockUI = null;

                isConditionTrue = false;
                //DebugBoxManager.Instance.Log("참일 때 false");
                matChanger.ChangeMaterial(MaterialType.NORMAL_CODEBLOCK_MATERIAL);
                break;
            case "MakeConditionFalse":
                if (MakeConditionBlockUI != null)
                    MakeConditionBlockUI = null;

                isConditionFalse = false;
                //DebugBoxManager.Instance.Log("거짓일 때 false");
                matChanger.ChangeMaterial(MaterialType.NORMAL_CODEBLOCK_MATERIAL);
                break;
        }

        //if (other.tag == "BlockContainerUI")
        //{
        //    BlockContainerUI = null;
        //    matChanger.ChangeMaterial(MaterialType.NORMAL_CODEBLOCK_MATERIAL);
        //}
    }



    public void ReturnToPool()
    {
        BlockContainerUI = null;

        HandGrabInteractable BlockHandGrab = GetComponent<HandGrabInteractable>();
        BlockHandGrab.enabled = true;
        BoxCollider loopBlockCodeBlockBoxCollider = GetComponent<BoxCollider>();
        loopBlockCodeBlockBoxCollider.enabled = true;
        if (TryGetComponent(out SetLoopBlockUI setLoopBlockUI))
            setLoopBlockUI.ClearLoopBlockList();
        transform.SetParent(PoolParent.transform, false);
        transform.localScale = new Vector3(30f, 30f, 30f);

        // SetParent 바뀌어서 피벗 맞춰주기
        _rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        _rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        _rectTransform.pivot = new Vector2(0.5f, 0.5f);

        matChanger.ChangeMaterial(MaterialType.NORMAL_CODEBLOCK_MATERIAL);
        matChanger.DisableXIcon();

        _rectTransform.localPosition = Vector3.zero;
        ObjectPoolManager.Instance.ReturnObject(gameObject, BlockName);
        EventManager<UIEvent>.TriggerEvent(UIEvent.BlockCountainerBlockCount, UIManager.Instance.BlockContainerLength - UIManager.Instance.BlockContainerManager.CountCodeBlockDragComponents());
        EventManager<UIEvent>.TriggerEvent(UIEvent.LoopBlockContainerBlockCount, UIManager.Instance.MakeLoopBlockContainerLength - UIManager.Instance.MakeLoopBlockContainerManager.CountCodeBlockDragComponents());
    }

    public SetConditionBlockUI GetConditionBlockUI()
    {
        if (TryGetComponent(out SetConditionBlockUI condUI))
        {
            return condUI;
        }
        else
        {
            DebugBoxManager.Instance.Log("SetConditionBlockUI가 없습니다.");
            return null;
        }
    }
    public SetLoopBlockUI GetLoopBlockUI()
    {
        if (TryGetComponent(out SetLoopBlockUI condUI))
        {
            return condUI;
        }
        else
        {
            DebugBoxManager.Instance.Log("SetLoopBlockUI가 없습니다.");
            return null;
        }
    }

    public void SetMaterial(MaterialType type)
    {
        matChanger.ChangeMaterial(type);
    }
}
