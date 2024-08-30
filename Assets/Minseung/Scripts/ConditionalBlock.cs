using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConditionalBlock : MonoBehaviour
{
    public int TrueBlockIndex { get; private set; }
    public int FalseBlockIndex { get; private set; }

    private int selectedAttribute;
    private DropdownManager dropdownManager;

    private void Start()
    {
        dropdownManager = FindAnyObjectByType<DropdownManager>();
        selectedAttribute = dropdownManager.GetSelectedConditionAttribute();
    }

    public void Initialize(int trueBlockIndex, int falseBlockIndex)
    {
        this.TrueBlockIndex = trueBlockIndex;
        this.FalseBlockIndex = falseBlockIndex;
    }

    public bool EvaluateCondition(int currentAttribute)
    {
        return currentAttribute == selectedAttribute;
    }
}
