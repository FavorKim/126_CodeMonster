using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class CodeBlock_Attack : CodeBlock
{
    public string target;

    public override Task CreateBehaviorTask()
    {
        var task = new AttackTask(); 
        task.target = target;
        return task;
    }
}
