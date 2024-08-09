using BehaviorDesigner.Runtime.Tasks;
using UnityEngine;

public abstract class CodeBlock : MonoBehaviour
{
    public abstract Task CreateBehaviorTask();
}
