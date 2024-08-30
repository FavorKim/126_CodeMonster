using EnumTypes;
using EventLibrary;
using Oculus.Interaction.HandGrab;
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



    /*
    인풋 불 물 풀
    3

    조건블록은
    
    실행부 (Action)
    조건판단부 (Func<bool>)
    data의 인덱스가 조건이다
    bool


    9개
    
    
    1번은 블록컨테이너에서 조건블록이 들어왔냐?

    
    ? 우리가 채점하느냐? 답지를 주느냐. <<< 조건블럭이 들어가는스테이지는 답지가 있어야하지 않나?
    지금은? 연출상 굉장히 도움이됐는데
    조건은? 채점은 답지가 해주고
    연출은 어떻게든 알아서
    조건블럭은 답지를 봐서 제대로된 위치에 제대로 된 조건블럭인지 먼저 검사하기?
    

    if 조건인지(인덱스 비교)

    1스테이지 조건블럭 -> ?번째 위치에 ~~조건일 때 ~~행동을 하는 조건블럭이 와야함.


    조건상태 조건행동
    Change액션 (리스트를 쭉 받아왔음) - 기존

    리스트를 하나씩 빼와서
    
    그 빼온 인덱스 (i)
    
    switch(i)
    case 이동:
    Change이동

    case 공격:
    Change공격

    case 조건:
    Change조건

    조건state
    
    최종적으로는
    행동을수행.
    이 후로는
    이전과 같음.


    ?

    ?

    */


    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        if (gameObject.TryGetComponent(out CustomGrabObject grabobj))
        {
            grab = grabobj;
        }
        PoolParent = transform.parent.gameObject;
        // data = Setdata();
        // if (data.index == condition) addcomponent(ConditionBlock); (init)

        // var cond = getcomponent(conditionblock); (Get)

        // if(cond.condition) cond.Action(); (Condition Action)

        // 총합 인덱스 숫자 == 정답
        // 자식 인덱스가 == 불
        // 


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
        if (BlockType == BlockType.LoopCodeBlock /*|| BlockType == BlockType.ConditionalCodeBlock*/)
        {
            SetLoopBlockUI SetLoopBlockUI = gameObject.GetComponentInChildren<SetLoopBlockUI>();
            SetLoopBlockUI.DisableLoopBlockImage();
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
            DebugBoxManager.Instance.Log("참 등록");
            MakeConditionBlockUI.SetConditionBlock(this, true);
            isConditionTrue = false;
        }
        // 조건문 (else시 행동) 등록부
        else if (MakeConditionBlockUI != null && isConditionFalse)
        {
            DebugBoxManager.Instance.Log("거짓 등록");
            MakeConditionBlockUI.SetConditionBlock(this, false);
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
            EventManager<UIEvent>.TriggerEvent(UIEvent.SetBlockCount, UIManager.Instance.BlockContainerLength - UIManager.Instance.BlockContainerManager.CountCodeBlockDragComponents());
        }


        if (BlockType == BlockType.LoopCodeBlock /*|| BlockType == BlockType.ConditionalCodeBlock*/)
        {
            List<BlockName> loopBlockNames = UIManager.Instance.LoopBlockList;
            foreach (BlockName blockName in loopBlockNames)
            {

                SetLoopBlockUI SetLoopBlockUI= gameObject.GetComponentInChildren<SetLoopBlockUI>();
                if (SetLoopBlockUI.CountLoopBlockListBox() >= UIManager.Instance.LoopBlockList.Count) return;

                GameObject loopBlock = ObjectPoolManager.Instance.GetObject(blockName);
                HandGrabInteractable loopBlockHandGrab = loopBlock.GetComponent<HandGrabInteractable>();
                CodeBlockDrag loopBlockCodeBlockDrag = loopBlock.GetComponent<CodeBlockDrag>();
                loopBlockCodeBlockDrag.enabled = false;
                loopBlockHandGrab.enabled = false;
                SetLoopBlockUI.EnableLoopBlockImage();
                SetLoopBlockUI.AddBlock(loopBlock);
            }
        }
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
                DebugBoxManager.Instance.Log("참일 때 true");
                matChanger.ChangeMaterial(MaterialType.OUTLINE_CODEBLOCK_MATERIAL);
                break;
            case "MakeConditionFalse":
                if (MakeConditionBlockUI == null)
                {
                    MakeConditionBlockUI = other.transform.parent.GetComponent<MakeConditionBlockUIManager>();
                }
                isConditionFalse = true;
                DebugBoxManager.Instance.Log("거짓일 때 true");
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
                DebugBoxManager.Instance.Log("참일 때 false");
                matChanger.ChangeMaterial(MaterialType.NORMAL_CODEBLOCK_MATERIAL);
                break;
            case "MakeConditionFalse":
                if (MakeConditionBlockUI != null)
                    MakeConditionBlockUI = null; 
                
                isConditionFalse = false;
                DebugBoxManager.Instance.Log("거짓일 때 false");
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
        CodeBlockDrag loopBlockCodeBlockDrag = GetComponent<CodeBlockDrag>();
        loopBlockCodeBlockDrag.enabled = true;
        transform.SetParent(PoolParent.transform, false);
        transform.localScale = new Vector3(30f, 30f, 30f);

        // SetParent 바뀌어서 피벗 맞춰주기
        _rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        _rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        _rectTransform.pivot = new Vector2(0.5f, 0.5f);

        matChanger.ChangeMaterial(MaterialType.NORMAL_CODEBLOCK_MATERIAL);

        _rectTransform.localPosition = Vector3.zero;
        ObjectPoolManager.Instance.ReturnObject(gameObject, BlockName);
        EventManager<UIEvent>.TriggerEvent(UIEvent.SetBlockCount, UIManager.Instance.BlockContainerLength - UIManager.Instance.BlockContainerManager.CountCodeBlockDragComponents());
    }
}
