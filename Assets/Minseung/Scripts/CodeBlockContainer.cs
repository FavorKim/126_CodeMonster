using System.Collections.Generic;
using UnityEngine;

public class CodeBlockContainer : MonoBehaviour
{
    private List<CodeBlock> blocks = new List<CodeBlock>();

    public void AddBlock(CodeBlock block)
    {
        blocks.Add(block);
    }

    public void RemoveBlock(CodeBlock block)
    {
        blocks.Remove(block);
    }

    public List<CodeBlock> GetBlocks()
    {
        return blocks;
    }

    public void UpdateBlockOrder(List<CodeBlock> newOrder)
    {
        blocks = newOrder;
    }
}
