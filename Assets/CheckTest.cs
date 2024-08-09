using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner;
using BehaviorDesigner.Runtime.Tasks;
[TaskCategory("Test")]
public class CheckTest : Conditional
{
    public override TaskStatus OnUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }

    
}
