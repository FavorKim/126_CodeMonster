using System;
using System.Collections.Generic;
using UnityEngine;

namespace FavorSample
{
    // 상태 enum
    public enum SampleStates
    {
        Check,
        Attack,
        Move,
        Condition,
    }

    public class SamplePlayer
    {
        public SampleStateMachine StateMachine { get; set; }
    }

    // 상태머신
    public class SampleStateMachine
    {
        SamplePlayer player;
        SampleBaseState<SamplePlayer> curState;
        Dictionary<Enum, SampleBaseState<SamplePlayer>> dict = new Dictionary<Enum, SampleBaseState<SamplePlayer>>();

        // 상태머신 초기화
        public SampleStateMachine(SamplePlayer player)
        {
            this.player = player;
            dict.Add(SampleStates.Move, new SampleMoveState(player));
            dict.Add(SampleStates.Check, new CheckState(player));

            // 액션이 끝날 때 이벤트 구독
            foreach (SampleBaseState<SamplePlayer> states in dict.Values)
            {
                // 이동상태, 공격상태 종료 후 발생할 이벤트를 골라서
                if (states is SampleMoveState)
                    // 체크 상태의 현재 listCount를 증가
                    states.OnActionEnd += (dict[SampleStates.Check] as CheckState).IncreaseListCount;
            }
        }

        // 상태 변화 함수. 조건 상태가 아닐 경우 조건상태에 필요한 조건 블록 변수는 default 매개변수로 선언
        public void ChangeState(Enum state, ConditionCodeBlock block = null)
        {
            SampleBaseState<SamplePlayer> next;
            // 만약 조건 상태로 변경할 경우
            if (state is SampleStates.Condition && block != null)
            {
                // 조건을 초기화
                (dict[state] as ConditionState).InitCondition(block.condition, block.act);
            }
            // 코드블록을 넣어주지 않았다면
            else
            {
                // 에러
            }
            next = dict[state];

            curState.OnExitState();
            curState = next;
            curState.OnEnterState();
        }
    }


    // 조건상태
    public class ConditionState : SampleBaseState<SamplePlayer>
    {
        SamplePlayer owner;
        public ConditionState(SamplePlayer player) : base(player)
        {
            owner = player;
        }

        Func<bool> cond;
        Action act;

        // 조건 초기화 함수
        public void InitCondition(Func<bool> cond, Action act)
        {
            this.cond = cond;
            this.act = act;
        }
        public override void OnExitState()
        {
            throw new System.NotImplementedException();
        }
        public override void OnEnterState()
        {
            ConditionAction();
        }

        private void ConditionAction()
        {
            // 조건에 부합하면 실행
            if (cond.Invoke() == true)
                act.Invoke();

        }
    }

    // 체크 상태
    public class CheckState : SampleBaseState<SamplePlayer>
    {
        SamplePlayer owner;
        public CheckState(SamplePlayer player) : base(player) { owner = player; }

        // 받아올 코드블럭 리스트
        List<int> blocks = new List<int>();

        // 현재 
        int listCount = 0;
        public override void OnEnterState()
        {
            // 코드블럭 세팅
            List<int> newBlocks = UIManager.Instance.BlockContainerManager.GetContatinerBlocks();
            // 블록리스트가 달라지면 (새로 코드를 짜서 실행하면) 리스트 카운트 초기화
            if (blocks != newBlocks)
                listCount = 0;

            blocks = newBlocks;

            switch (blocks[listCount])
            {
                case 0:
                case 1:
                case 2:
                case 3:
                    owner.StateMachine.ChangeState(SampleStates.Move);
                    break;
                case 4:
                case 5:
                case 6:
                    owner.StateMachine.ChangeState(SampleStates.Attack);
                    break;

                case 7:
                    owner.StateMachine.ChangeState(SampleStates.Condition);
                    break;

            }
        }


        public void IncreaseListCount()
        {
            listCount++;
        }


        public override void OnExitState()
        {

        }
    }

    // 이동 상태
    public class SampleMoveState : SampleBaseState<SamplePlayer>
    {
        SamplePlayer owner;
        public event Action OnExitMoveState;
        public SampleMoveState(SamplePlayer owner) : base(owner)
        {
            this.owner = owner;
        }
        public override void OnEnterState()
        {

        }
        public override void OnExitState()
        {
            // 행동 종료시 이벤트 호출
            InvokeOnActionEnd();
        }

    }


    public class ConditionCodeBlock
    {
        public Func<bool> condition { get; private set; }
        public Action act { get; private set; }
        public ConditionCodeBlock(Func<bool> condition, Action act)
        {
            this.condition = condition;
            this.act = act;
        }
    }

    // 플레이어의 상태를 변경하기위해 선언한 클래스
    public class SampleStateChanger
    {
        SamplePlayer player;
        public void ConditionTest(Vector2Int pos)
        {
            // 조건을 설정 (아래와 같이 기존에 있던 함수를 활용할 예정)
            Func<bool> cond = () => StageManager.Instance.CheckMonsterAndPlayerPos(pos);
            // 조건 만족시 수행할 행동을 설정
            Action act = ChangeStateToBattle;
            var block = new ConditionCodeBlock(cond, act);

            // 조건 설정 후 상태 변경
            player.StateMachine.ChangeState(SampleStates.Condition, block);
        }
        public void ChangeStateToBattle()
        {
            // 플레이어를 움직이는 함수
            StageManager.Instance.GetPlayer().playerStateMachine.ChangeState(SampleStates.Move);
        }
    }



    public abstract class SampleBaseState<T> where T : class
    {
        protected T Controller { get; set; }

        // 행동 종료시 호출할 이벤트
        public event Action OnActionEnd;

        // 이벤트 핸들러 호출 함수
        public void InvokeOnActionEnd()
        {
            OnActionEnd?.Invoke();
        }

        public SampleBaseState(T controller)
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
}