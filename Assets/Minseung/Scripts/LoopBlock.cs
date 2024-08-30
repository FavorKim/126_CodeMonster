using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoopBlock : MonoBehaviour
{
    public int LoopCount {  get; private set; }
    public List<int> SubBlockIndices { get; private set; }

    private int currentLoopIndex = 0;
    private int currentSubBlockIndex = 0;

    public void Start()
    {
        //LoopCount = GetLoopCount(); - 
    }

    public void Initialize(List<int> subBlockIndices)
    {
        // currentLoopIndex = LoopCount;
        this.SubBlockIndices = subBlockIndices;
    }

    public int GetNextBlockIndex()
    {
        if (currentLoopIndex < LoopCount)
        {
            if (currentSubBlockIndex < SubBlockIndices.Count)
            {
                return SubBlockIndices[currentLoopIndex++];
            }
            else
            {
                currentSubBlockIndex = 0;
                currentLoopIndex++;
                return GetNextBlockIndex();
            }
        }

        return -1;
    }
}
