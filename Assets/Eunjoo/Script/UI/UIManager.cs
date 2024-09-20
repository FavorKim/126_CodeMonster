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
    public GameObject IngameUI;

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
    public GameObject DirectHintBox;
    public GameObject IndirectHintBox;
    public GameObject StageTextBox;

    //[SerializeField] TMP_Text DirectHintBoxText;
    //[SerializeField] TMP_Text IndirectHintBoxText;
    [SerializeField] TMP_Text StageIndection;


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
        
    public List<int> LoopBlockList = new List<int>();

    public BlockContainerManager BlockContainerManager;
    public MakeLoopBlockContainerManager MakeLoopBlockContainerManager;
    private StageBlockUIManager StageBlockManager;
    private BlockCountBox BlockCountBox;

    public int SelectChapterNum;
    public int SelectStageNum;

    protected override void Start()
    {
        base.Start();
        BlockContainerManager = BlockContainerUI.GetComponent<BlockContainerManager>();
        MakeLoopBlockContainerManager = MakeLoopBlockUI.GetComponentInChildren<MakeLoopBlockContainerManager>();
        StageBlockManager = AttackBlockUI.GetComponent<StageBlockUIManager>();
        BlockCountBox = BlockCountObject.GetComponentInChildren<BlockCountBox>();

        InteractEventManager.Instance.RegistOnPokeBtn(PokeButton.HINT, PrintStageDirectHint);

        SetUIManager();
        IngameUI.SetActive(false);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            OnStartStage();
        }
    }

    public void OnStartStage()
    {
        IngameUI.SetActive(true);
        BlockContainerLength = DataManagerTest.Instance.GetStageMapData(SelectChapterNum + SelectStageNum).BlockContainerLength;
        StageManager.Instance.gameObject.SetActive(true);
        StartCoroutine(SetHintTimer());
    }
    


    private void SetUIManager()
    {
        BlockIndexLength = BlockIndexList.Length;

        // BlockIndexList에 있는 인덱스 숫자 체크 후 UI 활성화 
        BlockIndexListCheck();

        // 컨테이너 박스의 길이를 현재 컨테이너에 들어있는 블록에 따라 조절하는 기능
        //BlockContainerManager.SetBlockContainerUISize(BlockContainerLength);
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

    //public void DirectHintBoxEnable()
    //{
    //    DirectHintBox.SetActive(true);
    //}

    //public void DirectHintBoxDisble()
    //{
    //    DirectHintBox.SetActive(false);
    //}

    //public void IndirectHintBoxEnable()
    //{
    //    IndirectHintBox.SetActive(true);
    //}

    //public void IndirectHintBoxDisable()
    //{
    //    IndirectHintBox.SetActive(false);
    //}

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
        StageIndection.text = $"{StageManager.Instance.GetStageMap().StageIndex} 스테이지 힌트";
        SetText(StageIndection.text, StageIndection);
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
                TMP_Text IndirectHintBoxText = IndirectHintBox.GetComponent<TMP_Text>();
                SetText(text,IndirectHintBoxText);
                break;
            case TextTypeName.BIGHINT:
                //text = DataManagerTest.Instance.GetStageMapData(stageIndex).bighint;
                break;
        }
    }

    private void SetText(string text,TMP_Text hintBox)
    {
        var elements = text.Split('/');

        var list = new List<string>();

        foreach (var element in elements)
        {
            list.Add(element);
        }

        StartCoroutine(PrintText(list,hintBox));

    }

    private IEnumerator PrintText(List<string> list, TMP_Text hintBox)//실제 줄별로 텍스트를 출력하는 함수 , 할당된 UI에 텍스틑를 넣는 함수
    {
        hintBox.transform.parent.gameObject.SetActive(true);
        hintBox.text = string.Empty;
        foreach (var element in list)
        {
            hintBox.text += element + "\n";
            yield return new WaitForSeconds(2);
        }

        yield return new WaitForSeconds(5);

        hintBox.transform.parent.gameObject.SetActive(false);
    }



    public void EnableIngameUI()
    {
        IngameUI.SetActive(true);
    }

    public void DisableIngameUI()
    {
        IngameUI.SetActive(false);
    }



    public void EnableDirectHintBox()
    {
        DirectHintBox.SetActive(true);
        Invoke("DisableDirectHintBox", 5f);
    }

    public void EnableIndirectHintBox()
    {
        IndirectHintBox.SetActive(true);
        Invoke("DisableIndirectHintBox", 5f);
    }

    public void EnableStageTextBox()
    {
        StageTextBox.SetActive(true);
    }


    public void DisableDirectHintBox()
    {
        DirectHintBox.SetActive(false);
    }

    public void DisableIndirectHintBox()
    {
        IndirectHintBox.SetActive(false);
    }

    public void DisableStageTextBox()
    {
        StageTextBox.SetActive(false);
    }
}                    