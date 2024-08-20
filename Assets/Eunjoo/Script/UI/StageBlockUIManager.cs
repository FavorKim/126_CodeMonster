using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StageBlockUIManager : Singleton<StageBlockUIManager>
{
    private RectTransform StageBlockUIRectTransform;
    private BoxCollider StageBlockUBoxCollider;

    protected void Awake()
    {
        StageBlockUIRectTransform = GetComponent<RectTransform>();
        StageBlockUBoxCollider = GetComponent<BoxCollider>();
    }

    public void SetStageBlockUISize(int BlockIndexLength)
    {
        StageBlockUIRectTransform.sizeDelta = new Vector2(BlockIndexLength * UIConstants.ATTACK_MOVE_BLOCK_SIZE, UIConstants.ATTACK_MOVE_BLOCK_SIZE);
    }
}