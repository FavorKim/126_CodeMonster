using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StageBlockManager : Singleton<StageBlockManager>
{
    private RectTransform StageBlockUIRectTransform;
    private BoxCollider StageBlockUBoxCollider;

    private void Awake()
    {
        StageBlockUIRectTransform = GetComponent<RectTransform>();
        StageBlockUBoxCollider = GetComponent<BoxCollider>();
    }

    public void SetStageBlockUISize(int BlockIndexLength)
    {
        StageBlockUIRectTransform.sizeDelta = new Vector2(BlockIndexLength * UIConstants.RegularBoxSize, UIConstants.RegularBoxSize);
        StageBlockUBoxCollider.size = new Vector2(BlockIndexLength * UIConstants.RegularBoxSize, UIConstants.RegularBoxSize);
    }
}