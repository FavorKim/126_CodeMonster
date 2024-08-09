using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;

public class CodeBlock_Repeat : CodeBlock
{
    public int repeatCount;
    public List<CodeBlock> blocksToRepeat;

    public override Task CreateBehaviorTask()
    {
        var sequence = new Sequence();

        for (int i = 0; i < repeatCount; i++)
        {
            foreach (var block in blocksToRepeat)
            {
                Task task = block.CreateBehaviorTask();
                if (task != null)
                {
                    sequence.AddChild(task, sequence.Children.Count);
                }
            }
        }

        return sequence;
    }
}
