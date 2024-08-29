using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Action WinEvent;
    private Vector2Int position;
    private StageManager stageManager;

    private StateMachine<Player> stateMachine;
    public int CurrentIndex { get; set; } = 0;

    private List<int> blockIndexList;

    private bool isAttack = false;
    public bool isGameOver = false;

    public Vector2Int playerPosition { get { return position; } }
    public StateMachine<Player> playerStateMachine { get { return stateMachine; } }

    //[SerializeField]
    //private List<GameObject> monsterPrefabs;

    public void InitPlayer(StageManager stageManager)
    {
        this.stageManager = stageManager;
        position = stageManager.GetStartPosition();
        
    }

    private void Awake()
    {
        InitPlayer(StageManager.Instance);
        InitStateMachine();
    }

    public void Start()
    {
        //SetPlayerType();
        //SetPlayerPrefab();
        InteractEventManager.Instance.RegistOnClickStartBtn(StartPlayerAction);
        InteractEventManager.Instance.RegistOnClickRestartBtn(ResetPlayer);
        InteractEventManager.Instance.RegistOnClickPauseBtn(ResetPlayer);
    }

    private void Update()
    {
        stateMachine.UpdateState();
    }
    public void InitStateMachine()
    {
        stateMachine = new StateMachine<Player>(PlayerStateName.IDLE, new Idle(this));
        stateMachine.AddState(PlayerStateName.CHECK, new CheckState(this));
        stateMachine.AddState(PlayerStateName.MOVE, new MoveState(this));
        stateMachine.AddState(PlayerStateName.ATTACK, new AttackState(this));
        stateMachine.AddState(PlayerStateName.LOOP, new LoopState(this));
        stateMachine.AddState(PlayerStateName.DIEMOVE, new DIEMOVE(this));
        stateMachine.AddState(PlayerStateName.DIEHIT, new DIEHIT(this));
        stateMachine.AddState(PlayerStateName.HINTACCENT, new HINTACCENT(this));
    }

    public void StartPlayerAction()
    {
        blockIndexList = BlockContainerManager.Instance.GetContatinerBlocks();
        CurrentIndex = 0;
        stateMachine.ChangeState(PlayerStateName.CHECK);
    }

    public int GetCurrentBlockIndex()
    {
        return blockIndexList != null && CurrentIndex < blockIndexList.Count ? blockIndexList[CurrentIndex] : -1;
    }

    public void Move(Vector2Int direction)
    {
        Vector2Int newPosition = position + direction;
        Vector3 movePos = new Vector3(newPosition.x, 0, newPosition.y);

        transform.position = Vector3.Lerp(transform.position, movePos, 0.05f);

        if (Vector3.Distance(transform.position, movePos) <= 0.1f)
        {
            position = newPosition;
            transform.position = movePos;

            if (GameRule.CheckPlayerPosAndMonster(position))
            {
                transform.position = StageManager.Instance.GetPlayerPosWithMonsterStage(position);
                DebugBoxManager.Instance.Log("몬스터와 같은 자리에 위치함");
            }
            else if (GameRule.CheckPlayerPosInDeadzone(position))
            {
                DebugBoxManager.Instance.Log("잘못된 경로. 게임오버");
                stateMachine.ChangeState(PlayerStateName.DIEMOVE);
                return;
            }

            CurrentIndex++;
            stateMachine.ChangeState(PlayerStateName.CHECK);
        }
    }

    public void Attack(int blockIndex)
    {
        int attackBlockType = blockIndex; // 블록의 인덱스 자체를 공격 타입으로 사용

        EnableTypeMonsterPrefab(attackBlockType);
        BattleManager.Instance.BattlePhase(position, attackBlockType);

        isAttack = true;
        WinEvent += PlayerWinEvent;
    }

    public void PlayerWinEvent()
    {
        isAttack = false;
        CurrentIndex++;
        WinEvent -= PlayerWinEvent;
        stateMachine.ChangeState(PlayerStateName.CHECK);
    }

    public void ExecuteLoopBlock(LoopBlock loopBlock)
    {
        StartCoroutine(ExecuteLoop(loopBlock)); 
    }

    private IEnumerator ExecuteLoop(LoopBlock loopBlock)
    {
        for(int i = 0; i < loopBlock.LoopCount; i++)
        {
            foreach(int subBlockIndex in loopBlock.SubBlockIndices)
            {
                Execute(subBlockIndex);
                yield return new WaitWhile(() => isAttack || isGameOver);
            }
            CurrentIndex++;
            stateMachine.ChangeState(PlayerStateName.CHECK);

        }
    }

    private void Execute(int subBlockIndex)
    {
        
        if (subBlockIndex <= 4)
        {
            stateMachine.ChangeState(PlayerStateName.MOVE);
        }
        else if (subBlockIndex <= 7)
        {
            stateMachine.ChangeState(PlayerStateName.ATTACK);
        }
        else if (subBlockIndex >= 8)
        {
            stateMachine.ChangeState(PlayerStateName.LOOP);
        }
    }
    //private void SetPlayerType()
    //{
    //    int index = DataManagerTest.Instance.LoadedMonsterType.Count;

    //    for (int i = 0; i < index; i++)
    //    {
    //        GameObject typeObj = new GameObject();
    //        typeObj.transform.SetParent(this.transform);
    //        typeObj.transform.localPosition = Vector3.zero;
    //        typeObj.gameObject.name = DataManagerTest.Instance.GetMonsterTypeData(i + 5).TypeName;
    //    }
    //}

    //private void SetPlayerPrefab()
    //{
    //    for (int i = 0; i < monsterPrefabs.Count; i++)
    //    {
    //        GameObject monster = Instantiate(monsterPrefabs[i]);
    //        monster.name = monsterPrefabs[i].name;
    //        monster.SetActive(true);
    //        SetPrefabsParent(monster);
    //        monster.transform.localPosition = Vector3.zero;

    //    }

    //    DisableTypeMonsterPrefab();
    //    EnableTypeMonsterPrefab(4);
    //}

    //private void SetPrefabsParent(GameObject monster)
    //{
    //    int monsterTypeIndex = DataManagerTest.Instance.GetMonsterData(monster.name).TypeIndex;

    //    switch (monsterTypeIndex)
    //    {
    //        case 5:
    //            monster.transform.SetParent(this.transform.GetChild(1));
    //            break;
    //        case 6:
    //            monster.transform.SetParent(this.transform.GetChild(2));
    //            break;
    //        case 7:
    //            monster.transform.SetParent(this.transform.GetChild(3));
    //            break;
    //    }

    //}

    private void DisableTypeMonsterPrefab()
    {
        for (int i = 0; i < 4; i++)
        {
            this.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void EnableTypeMonsterPrefab(int monsterTypeIndex)
    {
        DisableTypeMonsterPrefab();
        this.transform.GetChild(monsterTypeIndex - 4).gameObject.SetActive(true);
    }


    public void Win()
    {
        EnableTypeMonsterPrefab(4);
        WinEvent?.Invoke();
    }

    public void Defeat()
    {
        EnableTypeMonsterPrefab(4);
        stateMachine.ChangeState(PlayerStateName.DIEHIT);
    }

    public Vector2Int GetCurrentPosition()
    {
        return position;
    }



    public void ResetPlayer()
    {
        this.gameObject.SetActive(false);
        this.gameObject.SetActive(true);
        EnableTypeMonsterPrefab(4);
        position = stageManager.GetStartPosition();
        transform.position = stageManager.GetPlayerRestPos();

        DebugBoxManager.Instance.ClearText();
    }

    public void Die()
    {
        this.gameObject.SetActive(false);
    }
}
