using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionalBlock : MonoBehaviour
{
    public int TrueBlockIndex { get; private set; }
    public int FalseBlockIndex { get; private set; }
    private Func<Vector2Int, bool> condition;

    private void Start()
    {
        
    }

    public void Initialize(Func<bool> condition, int trueBlockIndex, int falseBlockIndex)
    {
        this.condition = condition;
        this.TrueBlockIndex = trueBlockIndex;
        this.FalseBlockIndex = falseBlockIndex;
    }

    public int EvaluateCondition()
    {
        if (condition != null && condition.Invoke())
        {
            return TrueBlockIndex;
        }
        else
        {
            return FalseBlockIndex;
        }
    }
}
