using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeConditionBlockUIManager : Singleton<MakeConditionBlockUIManager>
{
    [SerializeField] Transform trueBlockPos;
    [SerializeField] Transform falseBlockPos;
    public CodeBlockDrag trueBlock
    {
        get; private set;
    }

    public CodeBlockDrag falseBlock
    {
        get;
        private set;
    }

    public void SetTrueBlock(CodeBlockDrag block)
    {
        this.trueBlock = block;
        trueBlock.transform.parent = trueBlockPos;
        trueBlock.transform.localPosition = Vector3.zero;
    }
    public void SetFalseBlock(CodeBlockDrag block)
    {
        this.falseBlock = block;
        falseBlock.transform.parent = falseBlockPos;
        falseBlock.transform.localPosition = Vector3.zero;
    }
}
