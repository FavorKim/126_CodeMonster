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

    public bool isAttack = false;
    public bool isGameOver = false;
    //bool isMove;
    public bool IsPlaying
    {
        get;
        private set;
    }

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
        InteractEventManager.Instance.RegistOnPokeBtn(PokeButton.START, StartPlayerAction);
        InteractEventManager.Instance.RegistOnPokeBtn(PokeButton.RESTART, ResetPlayer);
        InteractEventManager.Instance.RegistOnPokeBtn(PokeButton.PAUSE, ResetPlayer);
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
        stateMachine.AddState(PlayerStateName.CONDITION, new ConditionState(this));
        stateMachine.AddState(PlayerStateName.DIEMOVE, new DIEMOVE(this));
        stateMachine.AddState(PlayerStateName.DIEHIT, new DIEHIT(this));
        stateMachine.AddState(PlayerStateName.HINTACCENT, new HINTACCENT(this));
    }

    public void StartPlayerAction()
    {
        if (!IsPlaying)
        {
            IsPlaying = true;
            blockIndexList = UIManager.Instance.BlockContainerManager.GetContatinerBlocks();
            CurrentIndex = 0;
            stateMachine.ChangeState(PlayerStateName.CHECK);
        }
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
                //DebugBoxManager.Instance.Log("몬스터와 같은 자리에 위치함");
                UIManager.Instance.PrintUIText(TextTypeName.SMALLHINT);
            }
            else if (GameRule.CheckPlayerPosInDeadzone(position))
            {
                //DebugBoxManager.Instance.Log("잘못된 경로. 게임오버");
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

    public void Execute(int subBlockIndex)
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
        else if(subBlockIndex == 9)
        {
            stateMachine.ChangeState(PlayerStateName.CONDITION);
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


    private int GetAttackTypeFromBlock(int blockIndex)
    {
        // AttackBlock의 타입에 따른 처리 (5: Grass, 6: Water, 7: Fire)
        return blockIndex; // 블록의 인덱스 자체를 공격 타입으로 사용
    }

    private Vector2Int GetDirectionFromBlock(int blockIndex)
    {
        // MoveBlock의 방향에 따라 반환되는 Vector2Int 설정
        switch (blockIndex)
        {
            case 1: return Vector2Int.up;
            case 2: return Vector2Int.down;
            case 3: return Vector2Int.left;
            case 4: return Vector2Int.right;
            default: return Vector2Int.zero;
        }
    }

    /*
    private IEnumerator PlayerAction()
    {
        isPlaying = true;
        int index = 0;
        List<int> indexList =  UIManager.Instance.BlockContainerManager.GetContatinerBlocks();
        if (indexList == null)
        {
            //DebugBoxManager.Instance.Log("indexList is Null");
        }

        while (indexList.Count > index && isGameOver == false)
        {

            //이동중일때 멈춤

            Execute(indexList[index]);
            UIManager.Instance.BlockContainerManager.SetBlockMaterial(index, MaterialType.OUTLINE_CODEBLOCK_MATERIAL);
            if (index > 0)
                UIManager.Instance.BlockContainerManager.SetBlockMaterial(index - 1, MaterialType.USE_CODEBLOCK_MATERIAL);
            //공격중일때 멈춤
            index++;
            yield return new WaitWhile(() => isAttack);
            yield return new WaitWhile(() => isMove);
            //yield return new WaitForSeconds(1);
        }

        DebugBoxManager.Instance.Log("플레이어 행동 종료. 게임 오버");
        isPlaying = false;
        //if(isDie)
        //{
        //    this.gameObject.SetActive(false);
        //}
    }
    */

    public void ResetPlayer()
    {
        this.gameObject.SetActive(false);
        this.gameObject.SetActive(true);
        EnableTypeMonsterPrefab(4);
        position = stageManager.GetStartPosition();
        transform.position = stageManager.GetPlayerRestPos();
        IsPlaying = false;
        DebugBoxManager.Instance.ClearText();
    }

    public void Die()
    {
        this.gameObject.SetActive(false);
    }
}
