using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StageBlockManager : Singleton<StageBlockManager>
{
    private RectTransform StageBlockUIRectTransform;
    private BoxCollider StageBlockUBoxCollider;

    protected override void Awake()
    {
        base.Awake();
        StageBlockUIRectTransform = GetComponent<RectTransform>();
        StageBlockUBoxCollider = GetComponent<BoxCollider>();
    }

    public void SetStageBlockUISize(int BlockIndexLength)
    {
        StageBlockUIRectTransform.sizeDelta = new Vector2(BlockIndexLength * UIConstants.RegularUISize, UIConstants.RegularUISize);
        //StageBlockUBoxCollider.size = new Vector2(BlockIndexLength * UIConstants.RegularUISize, UIConstants.RegularUISize);
    }
}