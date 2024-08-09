using BehaviorDesigner.Runtime;
using UnityEngine;

public class Partner : MonoBehaviour
{
    public BehaviorTree behaviorTree;

    public void SetBehaviorTree(BehaviorTree tree)
    {
        behaviorTree = tree;
    }

    public void ExecuteBehavior()
    {
        behaviorTree.EnableBehavior();
    }
}
