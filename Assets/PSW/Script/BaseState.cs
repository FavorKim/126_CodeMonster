using System.Runtime.CompilerServices;
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
            // 행동이 종료됐는데도 적과 같이 있으면
            if(StageManager.Instance.CheckMonsterAndPlayerPos(Controller.playerPosition))
            {
                MonsterController mon = Controller.GetMonsterControllerWithPlayer();
                if(!mon.IsMonsterHPUnderZero())
                {
                    mon.Attack();
                    Controller.Die();
                    UIManager.Instance.PrintUITextByTextIndex(411, false);
                    return;
                }
            }
            // 행동 종료 후 적을 모두 쓰러트렸으면
            if(MonsterObjPoolManger.Instance.CheckEnemyAllDead() == true)
            {
                if (UIManager.Instance.SelectStageNum != 5 && UIManager.Instance.SelectChapterNum !=4000)
                {
                    // 클리어 UI 출력
                    UIManager.Instance.PrintPraise();
                }
                else
                {
                    GameManager.Instance.StartLoading(GameManager.Instance.StartCollectScene);
                }
            }
            else
            {
                UIManager.Instance.PrintUITextByTextIndex(400, false);
            }
            
            Controller.playerStateMachine.ChangeState(PlayerStateName.IDLE);
            return;
        }
        
        if (!Controller.isLoop)
        {
            UIManager.Instance.BlockContainerManager.SetBlockMaterial(Controller.CurrentIndex, MaterialType.OUTLINE_CODEBLOCK_MATERIAL);
            if (Controller.CurrentIndex > 0)
            {
                UIManager.Instance.BlockContainerManager.SetBlockMaterial(Controller.CurrentIndex - 1, MaterialType.USE_CODEBLOCK_MATERIAL);
            }
        }
        else
        {
            SetLoopBlockUI loopBlock = UIManager.Instance.BlockContainerManager.GetLoopBlockByIndex(Controller.ForceGetCurrentIndex());
            loopBlock.SetBlockMaterial(Controller.CurrentIndex, MaterialType.OUTLINE_CODEBLOCK_MATERIAL);

            if (Controller.CurrentIndex > 0)
            {
                loopBlock.SetBlockMaterial(Controller.CurrentIndex - 1, MaterialType.USE_CODEBLOCK_MATERIAL);
            }
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
            DebugBoxManager.Instance.Log($"현재 인덱스 : {Controller.CurrentIndex}");
            //Controller.IsIfUsed = true;
            DebugBoxManager.Instance.Log($"isLoop : {Controller.isLoop}");
            if (Controller.isLoop == false)
            {
                SetConditionBlockUI cond = UIManager.Instance.BlockContainerManager.GetCodeBlockByIndex(Controller.CurrentIndex).GetConditionBlockUI();
                if (cond != null)
                {
                    DebugBoxManager.Instance.Log("컨디션블록 불러옴");
                    cond.EnableConditionBlockListImage();
                    Controller.playerStateMachine.ChangeState(PlayerStateName.ATTACK);
                }
                else
                {
                    DebugBoxManager.Instance.Log("컨디션 블록 널");
                }
            }
            else
            {
                SetLoopBlockUI loop = UIManager.Instance.BlockContainerManager.GetCodeBlockByIndex(Controller.ForceGetCurrentIndex()).GetLoopBlockUI();
                SetConditionBlockUI cond = loop.GetConditionByIndex(Controller.CurrentIndex);
                if(cond != null)
                {
                    DebugBoxManager.Instance.Log("컨디션블록 불러옴");
                    
                    cond.EnableConditionBlockListImage();
                    Controller.playerStateMachine.ChangeState(PlayerStateName.ATTACK);
                }
                else
                {
                    DebugBoxManager.Instance.Log("컨디션 블록 널 (loop)");

                }

            }
        }
        else if (blockIndex == 9)
        {
            DebugBoxManager.Instance.Log("9번 인덱스. 반복블록");
            SetLoopBlockUI loop = UIManager.Instance.BlockContainerManager.GetCodeBlockByIndex((int)Controller.CurrentIndex).GetLoopBlockUI();
            DebugBoxManager.Instance.Log($"loop 차일드 카운트 : {loop.CountLoopBlockListBox()}");
            loop.EnableLoopBlockImage();
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

        if (StageManager.Instance.CheckMonsterAndPlayerPos(Controller.playerPosition))
        {
            MonsterController mon = Controller.GetMonsterControllerWithPlayer();
            mon.Attack();
            Controller.Die();
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
        DebugBoxManager.Instance.Log($"현재 공격 블록의 인덱스 : {blockIndex}");
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