using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Action WinEvent;
    [SerializeField]
    private List<GameObject> monsterPrefabs;

    private Vector2Int position;
    private StageManager stageManager;

    private StateMachin<Player> stateMachin;

    
    private bool isAttack;
    private bool isMove;
    public bool isGameOver;
    private bool isDie;
    public bool isPlaying = false;

    public Vector2Int playerPosition { get { return position; } }
    public StateMachin<Player> playerStateMachine { get { return stateMachin; } } 
    public void InitPlayer(StageManager stageManager)
    {
        this.stageManager = stageManager;
        position = stageManager.GetStartPosition();
        
    }

    private void Awake()
    {
    }
    public void Start()
    {
        InitPlayer(StageManager.Instance);
        InitStateMachine();
        SetPlayerType();
        SetPlayerPrefab();
        InteractEventManager.Instance.RegistOnClickStartBtn(StartPlayerAction);
        InteractEventManager.Instance.RegistOnClickRestartBtn(ResetPlayer);
    }

    private void Update()
    {
        stateMachin.UpdateState();
    }
    public void InitStateMachine()
    {
        stateMachin = new StateMachin<Player>(PlayerStateName.IDLE, new Idle(this));
        stateMachin.AddState(PlayerStateName.ACTION, new PlayerAction(this));
        stateMachin.AddState(PlayerStateName.DIEMOVE, new DIEMOVE(this));
        stateMachin.AddState(PlayerStateName.DIEHIT, new DIEHIT(this));
        stateMachin.AddState(PlayerStateName.HINTACCENT, new HINTACCENT(this));
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
        for (int i = 0; i < monsterPrefabs.Count; i++)
        {
            GameObject monster = Instantiate(monsterPrefabs[i]);
            monster.name = monsterPrefabs[i].name;
            monster.SetActive(true);
            SetPrefabsParent(monster);
            monster.transform.localPosition = Vector3.zero;

        }

        DisableTypeMonsterPrefab();
        EnableTypeMonsterPrefab(4);
    }

    private void SetPrefabsParent(GameObject monster)
    {
        int monsterTypeIndex = DataManagerTest.Instance.GetMonsterData(monster.name).TypeIndex;

        switch (monsterTypeIndex)
        {
            case 5:
                monster.transform.SetParent(this.transform.GetChild(1));
                break;
            case 6:
                monster.transform.SetParent(this.transform.GetChild(2));
                break;
            case 7:
                monster.transform.SetParent(this.transform.GetChild(3));
                break;
        }

    }
    //private void EnableTypeMonster(int monsterTypeIndex)
    //{
    //    switch (monsterTypeIndex)
    //    {
    //        case 5:
    //            EnableTypeMonsterPrefab(monsterTypeIndex);
    //            break;
    //        case 6:
    //            transform.GetChild(1).gameObject.SetActive(true);
    //            break;
    //        case 7:
    //            transform.GetChild(2).gameObject.SetActive(true);
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
        isAttack = false;
    }

    public void Defeat()
    {
        EnableTypeMonsterPrefab(4);
        stateMachin.ChangeState(PlayerStateName.DIEHIT);
        
        //isGameOver = true;
        //isAttack = false;
        //isDie = true;
    }

    public Vector2Int GetCurrentPosition()
    {
        return position;
    }

    
    

    

    public void StartPlayerAction()
    {
        if (!isPlaying)
            stateMachin.ChangeState(PlayerStateName.ACTION);
            //StartCoroutine(PlayerAction());
    }

    public void ResetPlayer()
    {
        this.gameObject.SetActive(false);
        this.gameObject.SetActive(true);
        EnableTypeMonsterPrefab(4);
        position = stageManager.GetStartPosition();
        transform.position = stageManager.GetPlayerRestPos();
        isDie = false;
        isAttack = false;
        isMove = false;
        isGameOver = false;
        isPlaying = false;

        DebugBoxManager.Instance.ClearText();
    }

    public void Die()
    {
        this.gameObject.SetActive(false);
    }
}
