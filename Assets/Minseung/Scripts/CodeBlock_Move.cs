using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class CodeBlock_Move : CodeBlock
{
    public Vector3 direction;
    public float distance;

    public override Task CreateBehaviorTask()
    {
        var task = new MoveTask(); 
        task.direction = direction;
        task.distance = distance;
        return task;
    }
}
