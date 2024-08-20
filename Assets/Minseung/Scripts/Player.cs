using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> monsterPrefabs;

    private Vector2Int position;
    private StageManager stageManager;

    private int attackBlockType;
    private bool isAttack;
    private bool isMove;
    private bool isGameOver;
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
        SetPlayerType();
        SetPlayerPrefab();
        InteractEventManager.Instance.startBtn.OnPoke += StartPlayerAction;
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
    }

    private void SetPrefabsParent(GameObject monster)
    {
        int monsterTypeIndex = DataManagerTest.Instance.GetMonsterTypeData(DataManagerTest.Instance.GetMonsterData(monster.name).TypeIndex).TypeIndex;

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
        for (int i = 1; i < 4; i++)
        {
            this.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void EnableTypeMonsterPrefab(int monsterTypeIndex)
    {

        for (int i = 0; i < 4; i++)
        {
            if (i == monsterTypeIndex - 4)
            {
                this.transform.GetChild(i).gameObject.SetActive(true);
                GameObject monster;
                //monster = this.transform.GetChild(i).gameObject.transform.GetChild(0).gameObject;
                //if (monster.activeSelf == false)
                //{
                //    monster.SetActive(true);
                //}
            }
            else
            {
                this.transform.GetChild(i).gameObject.SetActive(false);
            }
        }



    }

    public void Execute(int blockIndex)
    {
        if (blockIndex <= 4)
        {
            if (GameRule.CheckPlayerPosAndMonster(position) == false)//몬스터와 같은 자리인데 움직이려 하는가
            {
                StartCoroutine(Move(GetDirectionFromBlock(blockIndex)));
            }
            else
            {
                //게임오버
                isGameOver = true;
                DebugBoxManager.Instance.Log("Game Over. Monster Attack.");
            }
        }
        else if (blockIndex <= 7)
        {
            attackBlockType = GetAttackTypeFromBlock(blockIndex);
            Attack();
        }
    }

    private IEnumerator Move(Vector2Int direction)
    {
        Vector2Int newPosition = position + direction;
        Vector3 movePos = new Vector3(newPosition.x, 0, newPosition.y);
        isMove = true;

        while (MoveFinsh(transform.position, movePos))
        {
            transform.position = Vector3.Lerp(transform.position, movePos, 0.05f);
            yield return null;
        }

        position = newPosition;

        transform.position = movePos;

        if (GameRule.CheckPlayerPosAndMonster(position) == true)//몬스터와 같은 자리여서 위치를 바꾼다
        {
            //위치 변경
            transform.position = stageManager.GetPlayerPosWithMonsterStage(position);
            DebugBoxManager.Instance.Log("With Monster");

        }
        else if (GameRule.CheckPlayerPosInDeadzone(position))//내 위치가 이동 불가 지역이라 죽는다
        {
            isGameOver = true;
            DebugBoxManager.Instance.Log("Game Over. Wrong Path");
            yield break;
        }

        isMove = false;

    }

    private bool MoveFinsh(Vector3 playerPos, Vector3 targetPos)
    {
        if (Vector3.Distance(targetPos, playerPos) <= 0.1f)
        {
            return false;
        }
        return true;
    }

    private void Attack()
    {
        EnableTypeMonsterPrefab(attackBlockType);
        BattleManager.Instance.BattlePhase(position, attackBlockType);
    }

    public void Win()
    {
        EnableTypeMonsterPrefab(4);
        isAttack = false;
    }

    public void Defeat()
    {
        this.gameObject.SetActive(false);
    }

    public Vector2Int GetCurrentPosition()
    {
        return position;
    }

    public int GetAttackBlockType()
    {
        return attackBlockType;
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

    private IEnumerator PlayerAction()
    {
        int index = 0;
        List<int> indexList = BlockContainerManager.Instance.GetContatinerBlocks();
        if (indexList == null)
        {
            DebugBoxManager.Instance.Log("indexList is Null");
        }

        isAttack = false;
        isMove = false;
        isGameOver = false;

        DebugBoxManager.Instance.Log($"index count : {indexList.Count}, index : {index}");
        while (indexList.Count > index && isGameOver == false)
        {
            //이동중일때 멈춤
            yield return new WaitWhile(() => isMove);
            Execute(indexList[index]);
            index++;
            //공격중일때 멈춤
            yield return new WaitWhile(() => isAttack);
        }
        DebugBoxManager.Instance.Log("Game Over. Stop Player Action");
    }

    public void StartPlayerAction()
    {
        StartCoroutine(PlayerAction());
    }

    public void ResetPlayer()
    {
        EnableTypeMonsterPrefab(4);
        position=stageManager.GetStartPosition();
        transform.position=stageManager.GetPlayerRestPos();
    }
}
