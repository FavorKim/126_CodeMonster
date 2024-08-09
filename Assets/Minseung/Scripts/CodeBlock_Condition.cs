using System;
using UnityEngine;

public class CodeBlock_Condition : CodeBlock
{
    private Func<bool> condition;
    private CodeBlock blockToExecute;

    // 플레이어(사용자)가 조건과 실행할 코드 블록을 설정하는 메서드
    public void SetCondition(Func<bool> condition, CodeBlock block)
    {
        this.condition = condition;
        this.blockToExecute = block;
    }

    // 조건을 평가하고, 참일 경우 파트너가 행동을 수행하도록 하는 메서드
    public override void Execute(Player partner)
    {
        if (condition != null && condition())
        {
            blockToExecute.Execute(partner); // 조건이 참일 경우 파트너가 행동을 수행
        }
    }
}
