using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.RuleTile.TilingRuleOutput;

public abstract class BaseState<T>  where T : class
{
    protected T Controller { get; set; }

    public BaseState(T controller)
    {
        this.Controller = controller;
    }

    public abstract void OnEnterState();
    public virtual void OnUpdateState()
    {

    }
    public virtual void OnFixedUpdateState()
    {

    }
    public abstract void OnExitState();
}

public class Idle : BaseState<Player>
{
    public Idle(Player controller) : base(controller)
    {
    }

    public override void OnEnterState()
    {

    }

    public override void OnUpdateState()
    {

    }

    public override void OnExitState()
    {

    }
}
public class PlayerAction : BaseState<Player>
{
    Vector2Int position;
    private int attackBlockType;
    private int index;
    List<int> indexList;
    private bool isAttack = false;
    public PlayerAction(Player controller) : base(controller)
    {

    }

    public override void OnEnterState()
    {
        position = Controller.playerPosition;
        Controller.isPlaying = true;
        index = 0;
        indexList = BlockContainerManager.Instance.GetContatinerBlocks();
        Controller.WinEvent += PlayerWinEvent;
        isAttack = false;
    }

    public override void OnUpdateState()
    {
        BlockAction();
    }

    public override void OnExitState()
    {
        Controller.isPlaying = false;
        Controller.WinEvent -= PlayerWinEvent;
    }
    private void BlockAction()
    {
        if (indexList == null || index >= indexList.Count)
        {
            DebugBoxManager.Instance.Log("플레이어 행동종료.");
            Controller.playerStateMachine.ChangeState(PlayerStateName.IDLE);
            return;
        }

        if (isAttack) return;

        BlockContainerManager.Instance.SetBlockMaterial(index, MaterialType.OUTLINE_CODEBLOCK_MATERIAL);
        if(index > 0)
        {
            BlockContainerManager.Instance.SetBlockMaterial(index - 1, MaterialType.USE_CODEBLOCK_MATERIAL);
        }

        Execute(indexList[index]);
    }
    public void Execute(int blockIndex)
    {
        if (blockIndex <= 4)
        {
            if (!GameRule.CheckPlayerPosAndMonster(position))//몬스터와 같은 자리인데 움직이려 하는가
            {
                Move(GetDirectionFromBlock(blockIndex));
            }
            else
            {
                DebugBoxManager.Instance.Log("몬스터가 있는데 이동함. 게임오버");
                Controller.playerStateMachine.ChangeState(PlayerStateName.DIEMOVE);
            }
        }
        else if (blockIndex <= 7)
        {
            attackBlockType = GetAttackTypeFromBlock(blockIndex);
            Attack();
        }
        else if (blockIndex >= 8)
        {
            LoopBlock loopBlock = DataManagerTest.Instance.GetLoopBlockData(blockIndex);
            if (loopBlock != null)
            {
                Controller.StartCoroutine(ExecuteLoopBlock(loopBlock));
            }
        }
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
    private void Move(Vector2Int direction)
    {
        Vector2Int newPosition = position + direction;
        Vector3 movePos = new Vector3(newPosition.x, 0, newPosition.y);


        Controller.gameObject.transform.position = Vector3.Lerp(Controller.gameObject.transform.position, movePos, 0.05f);

        if(MoveFinsh(Controller.gameObject.transform.position, movePos))
        {
            index++;             
            position = newPosition;
            Controller.gameObject.transform.position = movePos;

            if (GameRule.CheckPlayerPosAndMonster(position))//몬스터와 같은 자리여서 위치를 바꾼다
            {
                Controller.gameObject.transform.position = StageManager.Instance.GetPlayerPosWithMonsterStage(position);
                DebugBoxManager.Instance.Log("몬스터와 같은 자리에 위치함");

            }
            else if (GameRule.CheckPlayerPosInDeadzone(position))//내 위치가 이동 불가 지역이라 죽는다
            {
                DebugBoxManager.Instance.Log("잘못된 경로. 게임오버");
                Controller.playerStateMachine.ChangeState(PlayerStateName.DIEMOVE);
            }
        }
    }

    private bool MoveFinsh(Vector3 playerPos, Vector3 targetPos)
    {
        return Vector3.Distance(targetPos, playerPos) > 0.1f;
    }

    private void Attack()
    {
        Controller.EnableTypeMonsterPrefab(attackBlockType);
        BattleManager.Instance.BattlePhase(position, attackBlockType);
        isAttack = true;
    }

    private IEnumerator ExecuteLoopBlock(LoopBlock loopBlock)
    {
        for(int i = 0; i < loopBlock.LoopCount; i++)
        {
            foreach(int subBlockIndex in loopBlock.SubBlockIndices)
            {
                Execute(subBlockIndex);
                yield return new WaitWhile(() => isAttack || Controller.isGameOver);
            }
        }
        index++;
        BlockAction();
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

    public void PlayerWinEvent()
    {
        index++;
        isAttack = false;
        BlockAction();
    }

}
public class DIEMOVE : BaseState<Player>
{
    public DIEMOVE(Player controller) : base(controller)
    {
    }

    public override void OnEnterState()
    {

    }

    public override void OnUpdateState()
    {

    }

    public override void OnExitState()
    {

    }
}
public class DIEHIT : BaseState<Player>
{
    float time = 0;
    public DIEHIT(Player controller) : base(controller)
    {
    }

    public override void OnEnterState()
    {
        time = 0;
    }

    public override void OnUpdateState()
    {
        time += Time.deltaTime;
        if(time > 2)
        {
            Controller.Die();
        }
    }

    public override void OnExitState()
    {

    }
}
public class HINTACCENT : BaseState<Player>
{
    public HINTACCENT(Player controller) : base(controller)
    {
    }

    public override void OnEnterState()
    {

    }

    public override void OnUpdateState()
    {

    }

    public override void OnExitState()
    {

    }
}