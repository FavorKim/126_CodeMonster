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
        if(this.trueBlock != null)
        {
            this.trueBlock.ReturnToPool();
        }
        this.trueBlock = block;

        this.trueBlock.transform.SetParent(trueBlockPos, false);
        Vector3 newPos = trueBlock.transform.localPosition;
        newPos.z = 0;
        this.trueBlock.transform.localPosition = newPos;
    }
    public void SetFalseBlock(CodeBlockDrag block)
    {
        if(this.falseBlock != null)
        {
            this.falseBlock.ReturnToPool();
        }
        this.falseBlock = block;
        this.falseBlock.transform.SetParent(falseBlockPos, false);
        Vector3 newPos = falseBlock.transform.localPosition;
        newPos.z = 0;
        this.falseBlock.transform.localPosition = newPos;
    }
}
