using EnumTypes;
using EventLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public enum TextTypeName
{
    STAGEINFO,
    BIGHINT,
    SMALLHINT
}


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
    public GameObject MakeLoopBlockUI;
    public GameObject MakeLoopBlockBoxUI;
    public GameObject MakeConditionalBlockUI;
    public GameObject MakeConditionalBlockBoxUI;
    public GameObject HintBoxUI;
    public GameObject VictoryUI;
    public GameObject GetMonsterUI;
    public GameObject BlockCountObject;
    [SerializeField] TMP_Text TextBox;


    [Header("BlockContainer UI")]
    public int BlockContainerLength;
    private bool PlusContainerUI = false;

    [Header("StageBlockList UI")]
    public int[] BlockIndexList;
    private int BlockIndexLength;

    [Header("HintBox UI")]
    [SerializeField] public int HintCount;

    [Header("MakeLoopBlock UI")]
    public int MakeLoopBlockContainerLength;
        
    public List<BlockName> LoopBlockList = new List<BlockName>();

    public BlockContainerManager BlockContainerManager;
    public MakeLoopBlockContainerManager MakeLoopBlockContainerManager;
    private StageBlockUIManager StageBlockManager;
    private BlockCountBox BlockCountBox;

    protected override void Start()
    {
        base.Start();
        BlockContainerManager = BlockContainerUI.GetComponent<BlockContainerManager>();
        MakeLoopBlockContainerManager = MakeLoopBlockUI.GetComponentInChildren<MakeLoopBlockContainerManager>();
        StageBlockManager = AttackBlockUI.GetComponent<StageBlockUIManager>();
        BlockContainerLength = DataManagerTest.Instance.GetStageMapData(1).BlockContainerLength;
        BlockCountBox = BlockCountObject.GetComponentInChildren<BlockCountBox>();

        InteractEventManager.Instance.RegistOnPokeBtn(PokeButton.HINT, PrintStageDirectHint);

        SetUIManager();
        StartCoroutine(SetHintTimer());
    }

    

    private void SetUIManager()
    {
        BlockIndexLength = BlockIndexList.Length;

        // BlockIndexList에 있는 인덱스 숫자 체크 후 UI 활성화 
        BlockIndexListCheck();

        BlockContainerManager.SetBlockContainerUISize(BlockContainerLength);
        EventManager<UIEvent>.TriggerEvent(UIEvent.BlockCountainerBlockCount, BlockContainerLength);
        EventManager<UIEvent>.TriggerEvent(UIEvent.LoopBlockContainerBlockCount, MakeLoopBlockContainerLength);

    }

    IEnumerator SetHintTimer()
    {
        HintBoxUI.SetActive(false);

        yield return new WaitForSeconds(HintCount);

        HintBoxUI.SetActive(true);
    }

    private void BlockIndexListCheck()
    {
        bool hasMoveBlock = false;
        bool hasAttackBlock = false;
        bool hasLoopBlock = false;
        bool hasConditionalBlock = false;

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
            if (index == 7)
            {
                hasLoopBlock = true;
            }
            if (index == 8)
            {
                hasConditionalBlock = true;
            }
        }

        // 인덱스 리스트에 따라 UI 활성화/비활성화 처리
        MoveBlockUI.SetActive(hasMoveBlock);
        AttackBlockUI.SetActive(hasAttackBlock);
        MakeLoopBlockUI.SetActive(hasLoopBlock);
        MakeConditionalBlockUI.SetActive(hasConditionalBlock);
    }

    public void VictoryUIEnable()
    {
        VictoryUI.SetActive(true);
    }

    public void VictoryUIDisable()
    {
        VictoryUI.SetActive(false);
    }

    public void GetMonsterUIEnable()
    {
        GetMonsterUI.SetActive(true);
    }

    public void GetMonsterUIDisable()
    {
        GetMonsterUI.SetActive(false);
    }

    public void MakeLoopBlockBoxEnable()
    {
        MakeLoopBlockBoxUI.SetActive(true);
    }
    public void MakeLoopBlockBoxDisable()
    {
        MakeLoopBlockBoxUI.SetActive(false);
    }

    public void MakeConditionalBlockBoxEnable()
    {
        DebugBoxManager.Instance.Log("Enabled");
        MakeConditionalBlockBoxUI.SetActive(true);
    }
    public void MakeConditionalBlockBoxDisable()
    {
        MakeConditionalBlockBoxUI.SetActive(false);
    }

    private void PrintStageDirectHint()
    {
        TextBox.text = $"{StageManager.Instance.GetStageMap().StageIndex} 스테이지 힌트";
        SetText(TextBox.text);
    }

    public void PrintUIText(TextTypeName type, int stageIndex = 0, int UITextindex = 0)//Text 출력을 요청하는 함수 ,필요시점에 호출하는 함수
    {
        string text = string.Empty;
        switch (type)
        {
            case TextTypeName.STAGEINFO:
                //text= DataManagerTest.Instance.GetStageMapData(stageIndex).stageInfo;
                break;
            case TextTypeName.SMALLHINT:
                text = DataManagerTest.Instance.GetTextData(UITextindex).Description;
                break;
            case TextTypeName.BIGHINT:
                //text = DataManagerTest.Instance.GetStageMapData(stageIndex).bighint;
                break;
        }
        SetText(text);
    }

    private void SetText(string text)
    {
        var elements = text.Split('/');

        var list = new List<string>();

        foreach (var element in elements)
        {
            list.Add(element);
        }

        StartCoroutine(PrintText(list));

    }

    private IEnumerator PrintText(List<string> list)//실제 줄별로 텍스트를 출력하는 함수 , 할당된 UI에 텍스틑를 넣는 함수
    {
        TextBox.transform.parent.gameObject.SetActive(true);
        TextBox.text = string.Empty;
        foreach (var element in list)
        {
            TextBox.text += element + "\n";
            yield return new WaitForSeconds(2);
        }

        yield return new WaitForSeconds(5);

        TextBox.transform.parent.gameObject.SetActive(false);
    }
}
