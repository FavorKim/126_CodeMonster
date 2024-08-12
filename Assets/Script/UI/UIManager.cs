using UnityEngine;

public static class UIConstants
{
    public const float RegularBoxSize = 60f;
    public const float ConditionalCodeBoxSize = 90f;
}


public class UIManager : Singleton<UIManager>
{
    [Header("UI List")]
    public GameObject BlockContainerUI;
    public GameObject StageBlockUI;


    [Header("BlockContainer UI")]
    public int BlockContainerLength;
    private bool PlusContainerUI = false;

    [Header("StageBlockList UI")]
    public int[] BlockIndexList;
    private int BlockIndexLength;
    private int PlusBlockListUISizeNum = 0;

    private BlockContainerManager BlockContainerManager;
    private StageBlockManager StageBlockManager;

    void Start()
    {
        BlockContainerManager = BlockContainerUI.GetComponent<BlockContainerManager>();
        StageBlockManager = StageBlockUI.GetComponent<StageBlockManager>();

        BlockIndexLength = BlockIndexList.Length;

        foreach(int index in BlockIndexList)
        {
            if(index == 7 || index ==8)
            {
                PlusBlockListUISizeNum += 1;
                PlusContainerUI = true;
            }
        }

        BlockContainerManager.Instance.SetBlockContainerUISize(BlockContainerLength, PlusContainerUI);
        StageBlockManager.Instance.SetStageBlockUISize(BlockIndexLength + PlusBlockListUISizeNum);
    }
}
