using System.Collections.Generic;
using UnityEngine;

public class CodeBlockManager 
{
    //private Dictionary<string, CodeBlockData> codeBlocks = new Dictionary<string, CodeBlockData>();
    //public static CodeBlockManager Inst { get; private set; }
    //private static List<int> blockIndexList = new List<int>();
    //private Player player;

    //private void Start()
    //{
    //    codeBlocks = DataManagerTest.Inst.LoadedCodeBlockList;

    //    player = StageManager.Instance.GetPlayer();
    //}

    public static void StartCodeBlocks()
    {
        ExcuteCodeBlockSequence(BlockContainerManager.Instance.GetContatinerBlocks());
    }

    private static void ExcuteCodeBlockSequence(List<string> sequence)
    {
        var player = StageManager.Instance.GetPlayer();
        // blockIndexList에 있는 각 blockIndex를 사용해 Player의 Execute 메서드를 호출
        foreach (var blockName in sequence)
        {
            //player.Execute(DataManagerTest.Instance.GetCodeBlockData(blockName));
        }
    }
}
