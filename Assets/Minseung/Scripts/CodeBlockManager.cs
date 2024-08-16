using System.Collections.Generic;
using UnityEngine;

public class CodeBlockManager : MonoBehaviour
{
    private Dictionary<string, CodeBlockData> codeBlocks = new Dictionary<string, CodeBlockData>();
    public static CodeBlockManager Inst { get; private set; }
    List<int> blockIndexList = new List<int>();
    private Player player;

    private void Start()
    {
        codeBlocks = DataManagerTest.Inst.LoadedCodeBlockList;

        player = StageManager.Instance.GetPlayer();
    }

    public void InitCodeBlockSequence()
    {
        blockIndexList.Clear();

        for (int i = 0; i < this.gameObject.transform.childCount; i++)
        {
            int blockIndex = codeBlocks[this.gameObject.transform.GetChild(i).name].BlockIndex;
            blockIndexList.Add(blockIndex);
        }

        if (codeBlocks.Count > 0)
        {
            SetCodeBlockSequence();
        }
    }

    private void SetCodeBlockSequence()
    {
        // blockIndexList에 있는 각 blockIndex를 사용해 Player의 Execute 메서드를 호출
        foreach (var blockIndex in blockIndexList)
        {
            player.Execute(blockIndex);
        }
    }
}
