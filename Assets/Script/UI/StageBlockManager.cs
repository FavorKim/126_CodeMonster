using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class StageBlockManager : Singleton<StageBlockManager>
{
    private RectTransform StageBlockUIRectTransform;
    private BoxCollider StageBlockUBoxCollider;
    [Header("CodeBlockPrefab")]
    public GameObject[] StageCodeBlockArr;
    private void Awake()
    {
        StageBlockUIRectTransform = GetComponent<RectTransform>();
        StageBlockUBoxCollider = GetComponent<BoxCollider>();
    }
    public void SetStageUI(int BlockIndexLength, int[] BlockIndexList)
    {
        SetStageBlockUISize(BlockIndexLength);
        SetStageBlock(BlockIndexList);
    }
    private void SetStageBlockUISize(int BlockIndexLength)
    {
        StageBlockUIRectTransform.sizeDelta = new Vector2(BlockIndexLength * 60, 60);
        StageBlockUBoxCollider.size = new Vector2(BlockIndexLength * 60, 60);
    }
    private void SetStageBlock(int[] BlockIndexList)
    {
        // BlockIndexList에 따라 코드 블럭들을 생성하여 UI 하위로 추가합니다.
        for (int i = 0; i < BlockIndexList.Length; i++)
        {
            int index = BlockIndexList[i];
            if (index >= 0 && index < StageCodeBlockArr.Length)
            {
                GameObject codeBlockPrefab = StageCodeBlockArr[index];
                Instantiate(codeBlockPrefab, StageBlockUIRectTransform);
            }
            else
            {
                Debug.LogWarning($"BlockIndexList의 인덱스 {index}가 StageCodeBlockArr의 범위를 벗어났습니다.");
            }
        }
    }
}