using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Vector2Int position;
    private StageManager stageManager;

    private int attackBlockType;
    private bool isAttack;
    private bool isMove;
    private bool isGameOver;
    public Player(StageManager stageManager)
    {
        this.stageManager = stageManager;
        position = stageManager.GetStartPosition();
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
                Debug.Log("Game Over");
            }
        }
        else if(blockIndex <= 7)
        {
            attackBlockType = GetAttackTypeFromBlock(blockIndex);
            Attack();
        }
    }

    private IEnumerator Move(Vector2Int direction)
    {
        Vector2Int newPosition = position + direction;
        Vector3 movePos = new Vector3(newPosition.x, 0, newPosition.y);
        int[,] grid = stageManager.GetGrid();
        isMove = true;
        while (MoveFinsh(transform.position,movePos))
        {
            transform.position = Vector3.Lerp(transform.position, movePos, 2);
            yield return null;
        }
        position = newPosition;
        if (GameRule.CheckPlayerPosAndMonster(position) == true)//몬스터와 같은 자리여서 위치를 바꾼다
        {
            //위치 변경
            transform.position = stageManager.GetPlayerPosWithMonsterStage(position);
        }
        else if(GameRule.CheckPlayerPosInDeadzone(position))//내 위치가 이동 불가 지역이라 죽는다
        {
            isGameOver = true;
            Debug.Log("Game Over");
            yield break;
        }

        isMove = false;

    }

    private bool MoveFinsh(Vector3 playerPos,Vector3 targetPos)
    {
        if(Vector3.Distance(targetPos, playerPos) <= 0.1f)
        {
            return true;
        }
        return false;
    }

    private void Attack()
    {
        BattleManager.Instance.BattlePhase(position, attackBlockType);
    }

    public void Win()
    {

    }

    public void Defeat()
    {

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
        // AttackBlock의 타입에 따른 처리 (1: Grass, 2: Water, 3: Fire)
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

        isAttack = false;
        isMove = false;
        isGameOver = false;

        while (indexList.Count < index && isGameOver)
        {
            //이동중일때 멈춤
            yield return new WaitWhile(() => isMove);
            Execute(indexList[index]);
            index++;
            //공격중일때 멈춤
            yield return new WaitWhile(() => isAttack);
        }

    }

    private void StartPlayerAction()
    {
        StartCoroutine(PlayerAction());
    }
}
