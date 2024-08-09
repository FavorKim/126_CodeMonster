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
    public int BlockIndexLength;

    public BlockContainerManager BlockContainerManager;
    public StageBlockManager StageBlockManager;

    void Start()
    {
        BlockContainerManager = BlockContainerUI.GetComponent<BlockContainerManager>();
        StageBlockManager = StageBlockUI.GetComponent<StageBlockManager>();

        BlockIndexLength = BlockIndexList.Length;
        BlockContainerManager.Instance.SetBlockContainerUISize(BlockContainerLength);
        StageBlockManager.Instance.SetStageBlockUISize(BlockIndexLength);
        StageBlockManager.Instance.SetStageBlock(BlockIndexList);
    }
}
