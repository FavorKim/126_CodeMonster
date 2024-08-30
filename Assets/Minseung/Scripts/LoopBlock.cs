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
    private DropdownManager dropdownManager;

    public void Start()
    {
        dropdownManager = FindAnyObjectByType<DropdownManager>();
        LoopCount = dropdownManager.GetSelectedLoopCount();
    }

    public void Initialize(List<int> subBlockIndices)
    {
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
