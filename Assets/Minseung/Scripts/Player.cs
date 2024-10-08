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
    private SetLoopBlockUI loopBlock;

    private int currentIndex = 0;
    public int CurrentIndex 
    {
        get
        {
            if (isLoop == false)
                return currentIndex;
            else
                return CurLoopIndex;
        }
    }
    public int ForceGetCurrentIndex() { return currentIndex; }
    
    private int curLoopIndex = 0;
    public int CurLoopIndex
    {
        get { return curLoopIndex; }
        set
        {
            curLoopIndex = value;
            if (curLoopIndex >= maxLoopIndex)
            {
                CurLoopCount++;
                curLoopIndex = 0;
            }
        }
    }

    private int maxLoopIndex = 0;


    private int curLoopCount = 0;
    public int CurLoopCount
    {
        get { return curLoopCount;}
        set
        {
            curLoopCount = value;
            if(curLoopCount >= maxLoopCount)
            {
                isLoop = false;
                OnEndLoop.Invoke();
                OnEndLoop = null;
                currentIndex++;
            }
        }
    }

    private int maxLoopCount = 0;
    

    private List<int> blockIndexList;

    public bool isAttack = false;
    public bool isGameOver = false;
    public bool isLoop = false;
    private bool isPlaying = false;
    public bool IsPlaying
    {
        get
        {
            return isPlaying;
        }
        set
        {
            if (isPlaying == value) return;

            isPlaying = value;
            if(isPlaying == true)
            {
                UIManager.Instance.SetCheerStop(true);
            }
            else
            {
                UIManager.Instance.SetCheerStop(false);
            }
        }
    }

    

    private void ResetLoopVariable()
    {
        maxLoopCount = 0;
        maxLoopIndex = 0;
        CurLoopIndex = 0;
        CurLoopCount = 0;
        isLoop = false;

    }

    public Vector2Int playerPosition { get { return position; } }
    public StateMachine<Player> playerStateMachine { get { return stateMachine; } }

    [SerializeField]
    private List<GameObject> monsterPrefabs;

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
        SetPlayerType();
        SetPlayerPrefab();
        IsPlaying = false;
        InteractEventManager.Instance.RegistOnPokeBtn(PokeButton.START, StartPlayerAction);
        InteractEventManager.Instance.RegistOnPokeBtn(PokeButton.RESTART, ResetPlayer);
        InteractEventManager.Instance.RegistOnPokeBtn(PokeButton.BACTTOMAIN, ResetPlayer);
        InteractEventManager.Instance.RegistOnPokeBtn(PokeButton.PAUSE, ResetPlayer);
    }

    

    private void Update()
    {
        stateMachine.UpdateState();

        if (Input.GetKeyDown(KeyCode.H))
        {
            StartPlayerAction();
        }
    }


    public void InitStateMachine()
    {
        stateMachine = new StateMachine<Player>(PlayerStateName.IDLE, new Idle(this));
        stateMachine.AddState(PlayerStateName.CHECK, new CheckState(this));
        stateMachine.AddState(PlayerStateName.MOVE, new MoveState(this));
        stateMachine.AddState(PlayerStateName.ATTACK, new AttackState(this));
        stateMachine.AddState(PlayerStateName.Loop, new LoopState(this));
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
            currentIndex = 0;
            stateMachine.ChangeState(PlayerStateName.CHECK);
        }
    }

    public int GetCurrentBlockIndex()
    {
        if (isLoop == false)
        {
            if (blockIndexList != null)
            {
                return currentIndex < blockIndexList.Count ? blockIndexList[currentIndex] : -1;
            }
            else
            {
                return -1;
            }
        }
        else
        {
            if (loopBlock.LoopBlockList != null)
                return CurLoopIndex < loopBlock.LoopBlockList.Count ? loopBlock.LoopBlockList[CurLoopIndex] : -1;
            else
                return -1;
        }

        //return blockIndexList != null && CurrentIndex < blockIndexList.Count ? blockIndexList[CurrentIndex] : -1;
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
                //UIManager.Instance.PrintUIText(TextTypeName.SMALLHINT);
                
            }
            else if (GameRule.CheckPlayerPosInDeadzone(position))
            {
                UIManager.Instance.BlockContainerManager.SetXIcon(CurrentIndex, true);
                UIManager.Instance.PrintUITextByTextIndex(420, false);
                stateMachine.ChangeState(PlayerStateName.DIEMOVE);
                return;
            }

            if (!isLoop)
                currentIndex++;
            else
                CurLoopIndex = curLoopIndex + 1;
            stateMachine.ChangeState(PlayerStateName.CHECK);
        }
    }

    public void Attack(int blockIndex)
    {
        int attackBlockType = blockIndex; // 블록의 인덱스 자체를 공격 타입으로 사용

        //WinEvent += PlayerWinEvent;
        BattleManager.Instance.BattlePhase(position, attackBlockType);

        isAttack = true;
    }

    public void PlayerWinEvent()
    {
        isAttack = false;
        if (!isLoop)
            currentIndex++;
        else
            CurLoopIndex = curLoopIndex + 1;
        //WinEvent -= PlayerWinEvent;
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

    }

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
        var mon = this.transform.GetChild(monsterTypeIndex - 4).gameObject;
        mon.SetActive(true);
        AnimationPlayer.SetTrigger("Attack", mon);
    }


    public void Win()
    {
        EnableTypeMonsterPrefab(4);
        PlayerWinEvent();
        //WinEvent?.Invoke();
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


    private void SetPlayerType()
    {
        int index = DataManagerTest.Instance.LoadedMonsterType.Count;

        for (int i = 0; i < index; i++)
        {
            GameObject typeObj = new GameObject();
            typeObj.transform.SetParent(this.transform);
            typeObj.transform.localPosition = Vector3.zero;
            typeObj.gameObject.name = DataManagerTest.Instance.GetMonsterTypeData(i + 5).TypeName;
        }
    }

    private void SetPlayerPrefab()
    {
        List<GameObject> list = UIManager.Instance.SelectCharacterUIManager.GetSelectedMonsters();
        
        monsterPrefabs = list;

        for (int i = 0; i < monsterPrefabs.Count; i++)
        {
            GameObject monster = Instantiate(monsterPrefabs[i]);
            monster.name = monsterPrefabs[i].name;
            monster.SetActive(true);
            //DontDestroyOnLoad(monster);
            SetPrefabsParent(monster);
            monster.transform.localPosition = Vector3.zero;
            monster.transform.localScale = Vector3.one;
            if(monster.TryGetComponent(out RandomMove ran))
            {
                ran.StopMoving();
            }
        }

        DisableTypeMonsterPrefab();
        EnableTypeMonsterPrefab(4);
    }

    private void SetPrefabsParent(GameObject monster)
    {
        int monsterTypeIndex = DataManagerTest.Instance.GetMonsterData(monster.name).TypeIndex;

        switch (monsterTypeIndex)
        {
            case 1:
                monster.transform.SetParent(this.transform.GetChild(1));
                break;
            case 2:
                monster.transform.SetParent(this.transform.GetChild(2));
                break;
            case 3:
                monster.transform.SetParent(this.transform.GetChild(3));
                break;
        }

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

    public void ResetPlayer()
    {
        //this.gameObject.SetActive(false);
        this.gameObject.SetActive(true);
        EnableTypeMonsterPrefab(4);
        position = stageManager.GetStartPosition();
        transform.position = stageManager.GetPlayerRestPos();
        IsPlaying = false;
        ResetLoopVariable();
        DebugBoxManager.Instance.ClearText();
    }


    public void Die()
    {
        this.gameObject.SetActive(false);
    }

    public MonsterController GetMonsterControllerWithPlayer()
    {
        GameObject monObj = StageManager.Instance.GetMonsterWithPlayerPos(playerPosition);
        MonsterController mon = monObj.GetComponent<MonsterController>();
        return mon;
    }

    public void SetMaxLoopCount(int loopCount) { maxLoopCount = loopCount; }
    public void SetMaxLoopIndex(int index) { maxLoopIndex = index; }
    public void SetLoopBlock(SetLoopBlockUI loopBlock) { this.loopBlock = loopBlock; }
    public void SetIsLoop(bool isloop)
    {
        isLoop = isloop;
    }

    public event Action OnEndLoop;
}
