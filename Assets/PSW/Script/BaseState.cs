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
        else if (blockIndex >= 8)
        {
            Controller.playerStateMachine.ChangeState(PlayerStateName.LOOP);
        }
    }

    public override void OnUpdateState() { }

    public override void OnExitState() { }
}



public class MoveState : BaseState<Player>
{
    public MoveState(Player controller) : base(controller) { }

    public override void OnEnterState()
    {
        int blockIndex = Controller.GetCurrentBlockIndex();
        Vector2Int direction = GetDirectionFromBlock(blockIndex);
        Controller.Move(direction);
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

    public override void OnUpdateState() { }

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
    private LoopBlock currentLoopBlock;
    private int currentLoopIndex;
    private int currentSubBlockIndex;

    public LoopState(Player controller) : base(controller) { }

    public override void OnEnterState()
    {
        currentLoopIndex = 0;
        currentSubBlockIndex = 0;

        int blockIndex = Controller.GetCurrentBlockIndex();
        LoopBlock loopBlock = DataManagerTest.Instance.GetLoopBlockData(blockIndex);

        if (currentLoopBlock != null)
        {
            ExecuteNextSubBlock();
        }
    }

    public override void OnUpdateState()
    {
        if(currentLoopBlock == null || currentLoopIndex >= currentLoopBlock.LoopCount)
        {
            Controller.playerStateMachine.ChangeState(PlayerStateName.CHECK);
            return;
        }

        if(!Controller.isAttack && !Controller.isGameOver)
        {
            ExecuteNextSubBlock();
        }
    }

    private void ExecuteNextSubBlock()
    {
        if(currentSubBlockIndex >= currentLoopBlock.SubBlockIndices.Count)
        {
            currentSubBlockIndex = 0;
            currentLoopIndex++;
        }

        if(currentLoopIndex < currentLoopBlock.LoopCount)
        {
            int subBlockIndex = currentLoopBlock.SubBlockIndices[currentLoopIndex];
            currentSubBlockIndex++;

            Controller.Execute(subBlockIndex);
        }
    }

    public override void OnExitState()
    {
        currentLoopBlock = null;
    }
}

public class ConditionState : BaseState<Player>
{
    private ConditionalBlock currentConditionalBlock;

    public ConditionState(Player controller) : base(controller) { }

    public override void OnEnterState()
    {
        int blockInedx = Controller.GetCurrentBlockIndex();
        currentConditionalBlock = DataManagerTest.Instance.GetConditionalBlockData(blockInedx);

        if(currentConditionalBlock != null)
        {
            ExecuteConditionalBlock();
        }
    }

    private void ExecuteConditionalBlock()
    {
        if (currentConditionalBlock.Condition())
        {
            Controller.Execute(currentConditionalBlock.TrueBlockIndex);
        }
        else
        {
            Controller.Execute(currentConditionalBlock.FalseBlockIndex);
        }

        Controller.playerStateMachine.ChangeState(PlayerStateName.CHECK);
    }

    public override void OnUpdateState() { }

    public override void OnExitState()
    {
        currentConditionalBlock = null;
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