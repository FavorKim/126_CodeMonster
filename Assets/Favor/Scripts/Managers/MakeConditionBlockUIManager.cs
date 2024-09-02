using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public void SetConditionBlock(CodeBlockDrag block, bool trueorFalse)
    {
        CodeBlockDrag dest = trueorFalse ? trueBlock : falseBlock;
        Transform destPos = trueorFalse ? trueBlockPos : falseBlockPos;
        if (dest != null)
            dest.ReturnToPool();
        dest = block;
        dest.transform.SetParent(destPos, false);
        Vector3 newPos = dest.transform.localPosition;
        newPos.z = 0;
        dest.transform.localPosition = newPos;
    }

}
