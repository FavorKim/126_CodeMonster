using UnityEngine;

public class ConditionBlock : MonoBehaviour
{
    public CodeBlockDrag TrueBlock { get; private set; }
    public CodeBlockDrag FalseBlock { get; private set; }
    public int indexValue {  get; private set; }

    CustomGrabObject grab;

    private void Start()
    {
        grab = GetComponent<CustomGrabObject>();
        grab.OnGrab += OnGrabSetData;
        grab.OnGrab += () => { DebugBoxManager.Instance.Log($"{indexValue}"); };
    }

    private void OnApplicationQuit()
    {
        grab.OnGrab -= () => { DebugBoxManager.Instance.Log($"{indexValue}"); };
        grab.OnGrab -= OnGrabSetData;
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
        if(monster.TypeIndex == indexValue)
        {
            return (int)TrueBlock.BlockName + 1;
        }
        else
        {
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
