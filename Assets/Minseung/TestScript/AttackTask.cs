using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public class AttackTask : Action
{
    public SharedString target;

    public override TaskStatus OnUpdate()
    {
        Debug.Log($"Attacking {target.Value}");
        return TaskStatus.Success;
    }
}
