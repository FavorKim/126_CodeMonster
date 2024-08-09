using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using System;

public class CodeBlock_Condition : CodeBlock
{
    public Func<bool> condition;
    public CodeBlock blockToExecute;

    public override Task CreateBehaviorTask()
    {
        var conditionalTask = new ConditionalTask();  // 일반 생성자 사용
        conditionalTask.condition = condition;

        var sequence = new Sequence();
        sequence.AddChild(conditionalTask, 0);

        if (blockToExecute != null)
        {
            Task task = blockToExecute.CreateBehaviorTask();
            if (task != null)
            {
                sequence.AddChild(task, 1);  // 인덱스를 명시적으로 지정하여 추가
            }
        }

        return sequence;
    }
}

public class ConditionalTask : Conditional
{
    public Func<bool> condition;

    public override TaskStatus OnUpdate()
    {
        return condition() ? TaskStatus.Success : TaskStatus.Failure;
    }
}
