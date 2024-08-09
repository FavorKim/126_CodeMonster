using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner;
using BehaviorDesigner.Runtime.Tasks;
[TaskCategory("Test")]
public class test : Action
{
    // Start is called before the first frame update
    

    // Update is called once per frame
    public override TaskStatus OnUpdate()
    {
        aaa();
        return TaskStatus.Success;
    }

    public void aaa()
    {
        Debug.Log("asdasd");
    }
}
