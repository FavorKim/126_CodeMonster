using EnumTypes;
using EventLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBlockUIManager : Singleton<MoveBlockUIManager>
{
    private RectTransform MoveBlockUIRectTransform;
    private BoxCollider MoveBlockUBoxCollider;

    protected void Awake()
    {
        MoveBlockUIRectTransform = GetComponent<RectTransform>();
        MoveBlockUBoxCollider = GetComponent<BoxCollider>();
    }

    public void SetMoveBlockUISize(int BlockIndexLength)
    {
        MoveBlockUIRectTransform.sizeDelta = new Vector2(UIConstants.ATTACK_MOVE_BLOCK_SIZE, BlockIndexLength * UIConstants.ATTACK_MOVE_BLOCK_SIZE);
    }
}

