using EnumTypes;
using EventLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public enum TextTypeName
{
    STAGEINFO,
    BIGHINT,
    SMALLHINT,
    CHEER,
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
    public SelectCharacterUIManager SelectCharacterUIManager;
    private StageBlockUIManager StageBlockManager;
    private BlockCountBox BlockCountBox;
    [SerializeField] public OutgameUIManager OutgameUIManager;

    public int SelectChapterNum;
    public int SelectStageNum;

    int cheerCount;

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



    public void OnStartStage()
    {
        IngameUI.SetActive(true);
        BlockContainerLength = DataManagerTest.Instance.GetStageMapData(SelectChapterNum + SelectStageNum).BlockContainerLength;
        StageManager.Instance.gameObject.SetActive(true);
        cheerCount = 0;
        PrintStageInfo();
        
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

    IEnumerator SetCheerTimer(float waitingTime)
    {
        yield return new WaitForSeconds(waitingTime);
        while (true)
        {
            yield return new WaitForSeconds(HintCount);

            if (cheerCount < 5)
            {
                int rand = UnityEngine.Random.Range(0, 4);
                PrintUITextByTextIndex(200 + rand, false);
            }
            else
            {
                PrintUITextByTextIndex(210, false);
                break;
            }
        }
    }

    void StartCheerTimer(float waitingTime)
    {
        StartCoroutine(SetCheerTimer(waitingTime));
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

    public void PrintStageDirectHint()
    {
        int curStageIndex = SelectChapterNum + SelectStageNum;
        PrintUITextByStageIndex(TextTypeName.BIGHINT, curStageIndex);
    }
    public void PrintStageInfo()
    {
        int curStageIndex = SelectChapterNum + SelectStageNum;
        PrintUITextByStageIndex(TextTypeName.STAGEINFO, curStageIndex);
    }
    
    public void PrintUITextByTextIndex(int textIndex, bool isDirectBox)
    {
        TMP_Text textBox;
        textBox = isDirectBox ? DirectHintBox.GetComponentInChildren<TMP_Text>() : IndirectHintBox.GetComponentInChildren<TMP_Text>();
        List<string> description = DataManagerTest.instance.GetDescriptionByTextIndex(textIndex);
        SetText(description, textBox);
    }

    private void PrintUITextByStageIndex(TextTypeName type, int stageIndex = 0)//Text 출력을 요청하는 함수 ,필요시점에 호출하는 함수
    {
        List<string> text = new List<string>();
        string typeName = GetTypeNameByEnum(type);
        text = DataManagerTest.Instance.GetDescriptionByStageIndex(stageIndex, typeName);
        TMP_Text textBox;
        switch (type)
        {
            case TextTypeName.STAGEINFO:
                textBox = DirectHintBox.GetComponentInChildren<TMP_Text>();
                StartCheerTimer(15);
                break;
            case TextTypeName.BIGHINT:
                textBox = DirectHintBox.GetComponentInChildren<TMP_Text>();
                break;
            default:
                textBox=null;
                break;

        }
        if(textBox != null )
        {
            SetText(text, textBox);
        }
    }
    private string GetTypeNameByEnum(TextTypeName typeEnum)
    {
        string typeName = string.Empty;
        switch (typeEnum)
        {
            case TextTypeName.STAGEINFO:
                typeName = "Info";
                break;
            case TextTypeName.BIGHINT:
                typeName = "BigHint";
                break;
            case TextTypeName.SMALLHINT:
                typeName = "SmallHint";
                break;
            case TextTypeName.CHEER:
                typeName = "Cheer";
                break;
            default:
                break;
        }
        return typeName;
    }

    private void SetText(List<string> text, TMP_Text hintBox)
    {
        StartCoroutine(PrintText(text, hintBox));
    }

    private IEnumerator PrintText(List<string> list, TMP_Text hintBox)//실제 줄별로 텍스트를 출력하는 함수 , 할당된 UI에 텍스틑를 넣는 함수
    {
        hintBox.transform.parent.gameObject.SetActive(true);
        WaitForSeconds character = new WaitForSeconds(0.1f);
        WaitForSeconds next = new WaitForSeconds(3.0f);

        foreach (string s in list)
        {
            hintBox.text = string.Empty;
            foreach (char c in s)
            {
                hintBox.text += c;
                yield return character;
            }
            yield return next;
        }

        hintBox.transform.parent.gameObject.SetActive(false);
    }
    public void StopPrintText(bool isDirectBox)
    {
        StopAllCoroutines();
        StartCheerTimer(0);
        if(isDirectBox)
            DirectHintBox.gameObject.SetActive(false);
        else
            IndirectHintBox.gameObject.SetActive(false);
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