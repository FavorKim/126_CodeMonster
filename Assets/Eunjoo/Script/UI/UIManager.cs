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
    PRAISE,
    COLLECTINFO
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
    [SerializeField] TMP_Text TMP_directBox;
    public GameObject IndirectHintBox;
    [SerializeField] TMP_Text TMP_IndirectBox;
    [SerializeField] GameObject CollectTextBox;
    [SerializeField] TMP_Text TMP_CollectTxt;
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
    [SerializeField] bool hintStop;
    [SerializeField] float debugT;
    bool isPraising = false;

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
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            PrintPraise();
        }
    }
    private void OnDisable()
    {
        DebugBoxManager.Instance.ClearText();
        StopAllCoroutines();
        DirectHintBox.SetActive(false);
        IndirectHintBox.SetActive(false);
        TMP_directBox.text = string.Empty;
        TMP_IndirectBox.text = string.Empty;

    }


    public void OnStartStage()
    {
        StopAllCoroutines();
        DirectHintBox.SetActive(false);
        IndirectHintBox.SetActive(false);
        TMP_directBox.text = string.Empty;
        TMP_IndirectBox.text = string.Empty;

        DebugBoxManager.Instance.ClearText();
        IngameUI.SetActive(true);
        BlockContainerLength = DataManagerTest.Instance.GetStageMapData(SelectChapterNum + SelectStageNum).BlockContainerLength;
        EventManager<UIEvent>.TriggerEvent(UIEvent.BlockCountainerBlockCount, BlockContainerLength);
        StageManager.Instance.gameObject.SetActive(true);
        cheerCount = 0;
        hintStop = false;
        StartCheerTimer();
        PrintStageInfo();
        ObjectPoolManager.Instance.SetSelectedAttackCodeBlock();
        BlockContainerManager.ResetBlockContainer();
        MakeLoopBlockContainerManager.ResetBlockContainer();
        MakeConditionBlockUIManager.Instance.ResetConditionContainer();
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

    IEnumerator SetCheerTimer()
    {
        while (true)
        {
            if (hintStop == false)
            {
                debugT = 0;
                while (debugT < HintCount)
                {
                    debugT += Time.deltaTime;
                    if (hintStop == true)
                    {
                        debugT = 0;
                        break;
                    }
                    yield return null;
                }
                debugT = 0;
                if (hintStop == true)
                    continue;
                if (cheerCount < 5)
                {
                    int rand = UnityEngine.Random.Range(0, 4);
                    PrintUITextByTextIndex(200 + rand, false);
                }
                else
                {
                    cheerCount = 0;
                    PrintUITextByTextIndex(210, false);
                }
            }
            else
            {
                yield return null;
            }
        }
    }

    void StartCheerTimer()
    {
        StartCoroutine(SetCheerTimer());
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
        MakeConditionalBlockBoxUI.SetActive(true);
    }
    public void MakeConditionalBlockBoxDisable()
    {
        MakeConditionalBlockBoxUI.SetActive(false);
    }

    public void PrintStageDirectHint()
    {
        StopPrintText(TextTypeName.BIGHINT);

        int curStageIndex = SelectChapterNum + SelectStageNum;
        PrintUITextByStageIndex(TextTypeName.BIGHINT, curStageIndex);
    }
    public void PrintStageInfo()
    {
        StopPrintText(TextTypeName.STAGEINFO);
        int curStageIndex = SelectChapterNum + SelectStageNum;
        PrintUITextByStageIndex(TextTypeName.STAGEINFO, curStageIndex);
    }
    public void PrintPraise()
    {
        StopPrintText(TextTypeName.PRAISE);
        hintStop = true;
        int curStageIndex = SelectChapterNum + SelectStageNum;
        List<string> text = DataManagerTest.Instance.GetPraiseByStageIndex(curStageIndex);
        SetText(text, TMP_directBox);

        StartPraiseCoroutine();
    }
    private void StartPraiseCoroutine()
    {
        if (SelectStageNum != 5 || SelectChapterNum + SelectStageNum == 4005)
            StartCoroutine(CorPraise());
        else
        {
            StartCoroutine(CorPraiseOnBoss());
        }
    }
    IEnumerator CorPraise()
    {
        isPraising = true;
        while (DirectHintBox.activeSelf == true)
        {
            yield return null;
        }
        isPraising = false;
        OutgameUIManager.SetClearUIActive(true);
    }
    IEnumerator CorPraiseOnBoss()
    {
        isPraising = true;
        while(DirectHintBox.activeSelf == true)
        {
            yield return null;
        }
        isPraising = false;
        Action action = GameManager.instance.StartCollectScene;
        GameManager.Instance.StartLoading(action);
    }

    public void PrintCollectStage()
    {
        StopPrintText(TextTypeName.COLLECTINFO);
        List<string> text = new List<string>();
        string correct = string.Empty;
        int curStageIndex = SelectChapterNum / 1000;
        switch (curStageIndex)
        {
            case 1:
                correct = "바나나";
                break;
            case 2:
                correct = "물고기";
                break;
            case 3:
                correct = "토마토";
                break;
        }
        text.Add($"유니크 몬스터는 {correct}를 좋아해! \n {correct}를 집어봐!");
        SetText(text, TMP_CollectTxt);
    }
    public void PrintOnGrabFood()
    {
        StopPrintText(TextTypeName.COLLECTINFO);
        List<string> text = new List<string>();
        string correct = string.Empty;
        int curStageIndex = SelectChapterNum / 1000;
        switch (curStageIndex)
        {
            case 1:
                correct = "바나나";
                break;
            case 2:
                correct = "물고기";
                break;
            case 3:
                correct = "토마토";
                break;
        }
        text.Add($"이제 {correct}를 검은색 위치에 두고 기다리고 있어봐");
        SetText(text, TMP_CollectTxt);
    }
    public void PrintOnSuccessCollect()
    {
        StopPrintText(TextTypeName.COLLECTINFO);
        List<string> text = new List<string>();
        text.Add("캐릭터가 수집됐어!");
        SetText(text, TMP_CollectTxt);
    }
    public void PrintOnFeeding()
    {
        StopPrintText(TextTypeName.COLLECTINFO);
        List<string> text = new List<string>();
        text.Add("좋아 그래로 몬스터가 먹이를 다 먹을 때까지 기다리고 있어~");
        SetText(text, TMP_CollectTxt);
    }

    public void PrintUITextByTextIndex(int textIndex, bool isDirectBox)
    {
        TMP_Text textBox;
        textBox = isDirectBox ? TMP_directBox : TMP_IndirectBox;
        List<string> description = DataManagerTest.instance.GetDescriptionByTextIndex(textIndex);
        SetText(description, textBox);
    }

    private void PrintUITextByStageIndex(TextTypeName type, int stageIndex = 0)//Text 출력을 요청하는 함수 ,필요시점에 호출하는 함수
    {
        List<string> text = new List<string>();
        string typeName = GetTypeNameByEnum(type);
        text = DataManagerTest.Instance.GetDescriptionByStageIndex(stageIndex, typeName);
        if(text == null)
        {
            // 없으면 출력하지 않음
            return;
        }
        TMP_Text textBox;
        switch (type)
        {
            case TextTypeName.STAGEINFO:
                textBox = TMP_directBox;
                break;
            case TextTypeName.BIGHINT:
                textBox = TMP_directBox;
                break;
            default:
                textBox = null;
                break;

        }
        if (textBox != null)
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
        hintStop = true;
        GameObject parent = hintBox == TMP_directBox ? DirectHintBox : IndirectHintBox;
        if (hintBox == TMP_directBox)
            parent = DirectHintBox;
        else if (hintBox == IndirectHintBox)
            parent = IndirectHintBox;
        else if (hintBox == TMP_CollectTxt)
            parent = CollectTextBox;

        parent.gameObject.SetActive(true);
        WaitForSeconds character = new WaitForSeconds(0.05f);
        WaitForSeconds next = new WaitForSeconds(2.0f);

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

        parent.gameObject.SetActive(false);

        hintStop = false;
    }
    public void OnClickIndirectBox()
    {
        StopPrintText(TextTypeName.SMALLHINT);
    }
    public void OnClickDirectBox()
    {

        StopPrintText(TextTypeName.BIGHINT);
    }
    public void StopPrintText(TextTypeName isDirectBox)
    {

        StopAllCoroutines();
        if (isPraising)
        {
            StartPraiseCoroutine();
        }
        switch (isDirectBox)
        {
            case TextTypeName.STAGEINFO:
            case TextTypeName.BIGHINT:
                DirectHintBox.gameObject.SetActive(false);
                hintStop = true;
                StartCheerTimer();
                hintStop = false;
                break;
            case TextTypeName.SMALLHINT:
            case TextTypeName.CHEER:
            case TextTypeName.PRAISE:
                IndirectHintBox.gameObject.SetActive(false);
                hintStop = true;
                StartCheerTimer();
                hintStop = false;
                break;
            case TextTypeName.COLLECTINFO:
                CollectTextBox.gameObject.SetActive(false);
                break;
            default:
                break;
        }
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

    public void SetCheerStop(bool isCheer)
    {
        hintStop = isCheer;
    }
}