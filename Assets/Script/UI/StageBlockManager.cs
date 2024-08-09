using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageBlockManager : Singleton<StageBlockManager>
{
    public RectTransform StageBlockUIRectTransform;
    public BoxCollider StageBlockUBoxCollider;

    [Header("CodeBlockPrefab")]
    public GameObject[] StageCodeBlockArr;

    private void Awake()
    {
        StageBlockUIRectTransform = GetComponent<RectTransform>();
        StageBlockUBoxCollider = GetComponent<BoxCollider>();
    }
    public void SetStageBlockUISize(int InventoryNum)
    {
        StageBlockUIRectTransform.sizeDelta = new Vector2(InventoryNum * 60, 60);
        StageBlockUBoxCollider.size = new Vector2(InventoryNum * 60, 60);
    }

    public void SetStageBlock(int[] BlockIndexList)
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
