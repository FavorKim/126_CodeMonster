using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class MoveTask : Action
{
    public SharedVector3 direction;
    public SharedFloat distance;

    public override TaskStatus OnUpdate()
    {
        Debug.Log($"Moving {direction.Value} by {distance.Value}");
        return TaskStatus.Success;
    }
}
