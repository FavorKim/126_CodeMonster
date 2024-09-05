using UnityEngine;

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

public class CheckState : BaseState<Player>
{
    public CheckState(Player controller) : base(controller) { }

    public override void OnEnterState()
    {
        int blockIndex = Controller.GetCurrentBlockIndex();
        
        if (blockIndex == -1)
        {
            DebugBoxManager.Instance.Log("플레이어 행동 종료.");
            // if(적이 안 죽었다) -> 패배
            if(Controller.AttackedByMonster(out MonsterController mon))
            {
                mon.Attack();
                
            }
            Controller.playerStateMachine.ChangeState(PlayerStateName.IDLE);
            return;
        }

        if (blockIndex <= 4)
        {
            Controller.playerStateMachine.ChangeState(PlayerStateName.MOVE);
        }
        else if (blockIndex <= 7)
        {
            Controller.playerStateMachine.ChangeState(PlayerStateName.ATTACK);
        }
        else if (blockIndex == 8)
        {
            DebugBoxManager.Instance.Log("8번 인덱스. 조건블록");
            //Controller.IsIfUsed = true;
            Controller.playerStateMachine.ChangeState(PlayerStateName.ATTACK);
        }
        else if (blockIndex == 9)
        {
            DebugBoxManager.Instance.Log("9번 인덱스. 반복블록");
            Controller.playerStateMachine.ChangeState(PlayerStateName.Loop);
        }

    }

    public override void OnUpdateState() { }

    public override void OnExitState() { }
}



public class MoveState : BaseState<Player>
{
    public MoveState(Player controller) : base(controller) { }

    Vector2Int direction;

    public override void OnEnterState()
    {
        int blockIndex = Controller.GetCurrentBlockIndex();

        if (Controller.AttackedByMonster(out MonsterController mon))
        {
            mon.Attack();
        }

        direction = GetDirectionFromBlock(blockIndex);
    }

    private Vector2Int GetDirectionFromBlock(int blockIndex)
    {
        switch (blockIndex)
        {
            case 1: return Vector2Int.up;
            case 2: return Vector2Int.down;
            case 3: return Vector2Int.left;
            case 4: return Vector2Int.right;
            default: return Vector2Int.zero;
        }
    }

    public override void OnUpdateState() 
    {
        Controller.Move(direction);
    }

    public override void OnExitState() { }
}



public class AttackState : BaseState<Player>
{
    public AttackState(Player controller) : base(controller) { }

    public override void OnEnterState()
    {
        int blockIndex = Controller.GetCurrentBlockIndex();
        Controller.Attack(blockIndex);
    }

    public override void OnUpdateState() { }

    public override void OnExitState() { }
}

public class LoopState : BaseState<Player>
{
    public LoopState(Player controller) : base(controller) { }
    SetLoopBlockUI loopBlock;
    int curIndex;
    public override void OnEnterState()
    {
        Controller.SetIsLoop(true);
        loopBlock = UIManager.Instance.BlockContainerManager.GetLoopBlockByIndex(Controller.ForceGetCurrentIndex());
        Controller.SetLoopBlock(loopBlock);
        Controller.SetMaxLoopIndex(loopBlock.LoopBlockList.Count);
        Controller.SetMaxLoopCount(loopBlock.LoopCount);
        //DebugBoxManager.Instance.Log($"루프블록의 루프카운트 : {loopBlock.LoopCount}");
        Controller.CurLoopIndex = 0;
        Controller.CurLoopCount = 0;

        Controller.playerStateMachine.ChangeState(PlayerStateName.CHECK);
    }

    public override void OnUpdateState() { }

    public override void OnExitState() { }

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