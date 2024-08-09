using UnityEngine;



public class UIManager : Singleton<UIManager>
{
    [Header("UI List")]
    public GameObject BlockContainerUI;
    public GameObject StageBlockUI;


    [Header("BlockContainer UI")]
    public int BlockContainerLength;

    [Header("StageBlockList UI")]
    public int[] BlockIndexList;
    private int BlockIndexLength;

    private BlockContainerManager BlockContainerManager;
    private StageBlockManager StageBlockManager;

    void Start()
    {
        BlockContainerManager = BlockContainerUI.GetComponent<BlockContainerManager>();
        StageBlockManager = StageBlockUI.GetComponent<StageBlockManager>();

        BlockIndexLength = BlockIndexList.Length;
        BlockContainerManager.Instance.SetBlockContainerUISize(BlockContainerLength);
        StageBlockManager.Instance.SetStageUI(BlockIndexLength, BlockIndexList);
    }
}
