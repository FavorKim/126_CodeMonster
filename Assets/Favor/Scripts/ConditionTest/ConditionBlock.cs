using UnityEngine;

public class ConditionBlock : MonoBehaviour
{
    [SerializeField] CodeBlockDrag TrueType;
    [SerializeField] CodeBlockDrag FalseType;
    public CodeBlockDrag TrueBlock { get; private set; }
    public CodeBlockDrag FalseBlock { get; private set; }

    public int indexValueDebug;
    public int indexValue { get; private set; }

    CustomGrabObject grab;


    private void Start()
    {
        if(indexValueDebug !=0)
            indexValue = indexValueDebug;
        grab = GetComponent<CustomGrabObject>();
        grab.OnGrab.AddListener(OnGrabSetData);
        if (TrueType != null)
            TrueBlock = TrueType;
        if (FalseType != null)
            FalseBlock = FalseType;
        //grab.OnGrab += () => { DebugBoxManager.Instance.Log($"{indexValue}"); };
    }

    private void OnApplicationQuit()
    {
        //grab.OnGrab -= () => { DebugBoxManager.Instance.Log($"{indexValue}"); };
        grab.OnGrab.AddListener(OnGrabSetData);
    }

    // 조건 블록 정보 초기화
    public void InitConditionBlock(CodeBlockDrag trueBlock, CodeBlockDrag falseBlock, int indexValue)
    {
        this.indexValue = indexValue;
        this.TrueBlock = trueBlock;
        this.FalseBlock = falseBlock;
    }
    public void InitConditionBlock(ConditionBlock dest)
    {
        this.indexValue = dest.indexValue;
        this.TrueBlock = dest.TrueBlock;
        this.FalseBlock = dest.FalseBlock;
    }

    // 블록을 잡을 때마다 저장되어있는 정보로 본인의 정보를 초기화
    private void OnGrabSetData()
    {
        InitConditionBlock(MakeConditionBlockUIManager.Instance.GetConditionBlockInfo());
    }

    public int EvaluateCondition(Monster monster)
    {
        SetConditionBlockUI conUI = GetComponent<SetConditionBlockUI>();
        DebugBoxManager.Instance.Log($" 조건블록 인덱스 밸류 {indexValue}");
        if (monster.TypeIndex == indexValue)
        {
            //DebugBoxManager.Instance.Log("참 블록 평가완료");
            // true거 하이라이트 해주고
            conUI.AccentTrueBlock();
            return (int)TrueBlock.BlockName + 1;
        }
        else
        {
            //DebugBoxManager.Instance.Log("거짓 블록 평가완료");
            // false거 하이라이트 해주고
            conUI.AccentFalseBlock();
            return (int)FalseBlock.BlockName + 1;
        }
    }

    public int EvaluateCondition()
    {
        Player player = StageManager.Instance.GetPlayer();

        Vector2Int playerPosition = player.GetCurrentPosition();

        int randomIndex = Random.Range(0, 1);

        //GameObject monster = StageManager.Instance.GetMonsterWithPlayerPos(player.playerPosition);

        GameObject bushMonster = StageManager.Instance.GetMonsterInBush(playerPosition, randomIndex);

        if (bushMonster != null)
        {
            string bushMonsterName = bushMonster.name;

            Monster monsterData = DataManagerTest.Instance.GetMonsterData(bushMonsterName);

            if (monsterData != null && monsterData.TypeIndex == indexValue)
            {
                return (int)TrueBlock.BlockName + 1;
            }
        }

        return (int)FalseBlock.BlockName + 1;
    }
}
