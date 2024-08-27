using EnumTypes;
using EventLibrary;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public static class UIConstants
{
    public const int ATTACK_BLOCK_COUNT = 3;
    public const int MOVE_BLOCK_COUNT = 4;
    public const int ATTACK_MOVE_BLOCK_SIZE = 50;
    public const int CONTAINER_WIDTH_SIZE = 60;
    public const int CONTAINER_HEIGHT_SIZE = 50;
    public const int LOOP_CON_BLOCK_SIZE = 120;
}

public class UIManager : Singleton<UIManager>
{
    [Header("UI List")]
    public GameObject BlockContainerUI;
    public GameObject AttackBlockUI;
    public GameObject MoveBlockUI;
    public GameObject LoopBlockUI;
    public GameObject ConditionalBlockUI;
    public GameObject HintBox;


    [Header("BlockContainer UI")]
    public int BlockContainerLength;
    private bool PlusContainerUI = false;

    [Header("StageBlockList UI")]
    public int[] BlockIndexList;
    private int BlockIndexLength;

    [Header("HintBox UI")]
    [SerializeField]public int HintCount;


    private BlockContainerManager BlockContainerManager;
    private StageBlockUIManager StageBlockManager;

    protected override void Start()
    {
        base.Start();
        BlockContainerManager = BlockContainerUI.GetComponent<BlockContainerManager>();
        StageBlockManager = AttackBlockUI.GetComponent<StageBlockUIManager>();

        SetUIManager();
        StartCoroutine(SetHintTimer());
    }

    private void SetUIManager()
    {
        BlockIndexLength = BlockIndexList.Length;
        BlockContainerLength = DataManagerTest.Instance.GetStageMapData(1).BlockContainerLength;

        // BlockIndexList에 있는 인덱스 숫자 체크 후 UI 활성화 
        BlockIndexListCheck();

        // BlockIndexList에 있는 인덱스 숫자 체크 후 UI Size 설정 
        //BlockUISizeSet();

        BlockContainerManager.Instance.SetBlockContainerUISize(BlockContainerLength, PlusContainerUI);

        EventManager<UIEvent>.TriggerEvent(UIEvent.SetBlockCount, BlockContainerLength);
    }

    IEnumerator SetHintTimer()
    {
        HintBox.SetActive(false);

        yield return new WaitForSeconds(HintCount);

        HintBox.SetActive(true);
    }

    private void BlockUISizeSet()
    {
        // foreach 문을 돌릴때 이동타입의 블럭인 0,1,2,3이 다있을 때 사이즈를 한번만 세팅하기 위해 bool 값 설정
        bool moveBlockUISizeSet = false;
        bool stageBlockUISizeSet = false;

        foreach (int index in BlockIndexList)
        {
            moveBlockUISizeSet = SetMoveBlockUISize(index, moveBlockUISizeSet);
            stageBlockUISizeSet = SetStageBlockUISize(index, stageBlockUISizeSet);
            CheckPlusContainerUI(index);
        }
    }

    private bool SetMoveBlockUISize(int index, bool moveBlockUISizeSet)
    {
        if (!moveBlockUISizeSet && (index == 0 || index == 1 || index == 2 || index == 3))
        {
            // UI사이즈 조절 함수, 그냥 원래 사이즈 그대로 둬도 될것 같아서 주석처리
            // MoveBlockUIManager.Instance.SetMoveBlockUISize(UIConstants.MOVE_BLOCK_COUNT);
            return true;
        }
        return moveBlockUISizeSet;
    }

    private bool SetStageBlockUISize(int index, bool stageBlockUISizeSet)
    {
        if (!stageBlockUISizeSet && (index == 4 || index == 5 || index == 6))
        {
            // UI사이즈 조절 함수, 그냥 원래 사이즈 그대로 둬도 될것 같아서 주석처리
            //StageBlockUIManager.Instance.SetStageBlockUISize(UIConstants.ATTACK_BLOCK_COUNT);
            return true;
        }
        return stageBlockUISizeSet;
    }

    private void CheckPlusContainerUI(int index)
    {
        if (index == 7 || index == 8)
        {
            PlusContainerUI = true;
        }
    }


    private void BlockIndexListCheck()
    {
        bool hasMoveBlock = false;
        bool hasAttackBlock = false;
        bool hasLoopConBlock = false;

        foreach (int index in BlockIndexList)
        {
            if (index == 0 || index == 1 || index == 2 || index == 3)
            {
                hasMoveBlock = true;
            }
            if (index == 4 || index == 5 || index == 6)
            {
                hasAttackBlock = true;
            }
            if (index == 7 || index == 8)
            {
                hasLoopConBlock = true;
            }
        }

        // 인덱스 리스트에 따라 UI 활성화/비활성화 처리
        MoveBlockUI.SetActive(hasMoveBlock);
        AttackBlockUI.SetActive(hasAttackBlock);

        // Loop랑 Conditional Block UI는 인덱스 리스트에 따라 판별 X 
        //Loop_ConBlockUI.SetActive(hasLoopConBlock);
    }
}
