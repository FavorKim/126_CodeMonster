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
    private int PlusUISizeNum = 0;

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
                Debug.Log(PlusUISizeNum);
                PlusUISizeNum += 1;
            }
        }

        BlockContainerManager.Instance.SetBlockContainerUISize(BlockContainerLength);
        StageBlockManager.Instance.SetStageBlockUISize(BlockIndexLength + PlusUISizeNum);
    }
}
