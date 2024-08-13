using System.Collections.Generic;
using UnityEngine;

public class CodeBlockManager : MonoBehaviour
{
    public CodeBlockContainer container;
    public CodeBlockSequenceArea sequenceArea;
    private GameManager gameManager;
    private Dictionary<string, CodeBlockData> codeBlocks = new Dictionary<string, CodeBlockData>();
    public static CodeBlockManager Inst { get; private set; }

    private void Awake()
    {
    }
    private void Start()
    {
        codeBlocks = DataManagerTest.Inst.LoadedCodeBlockList;
    }
    public List<int> ExecuteAll()
    {
        List<int> blockIndexList = new List<int>();

        for(int i = 0; i < this.gameObject.transform.childCount; i++) 
        {
            int blockIndex = codeBlocks[this.gameObject.transform.GetChild(i).name].BlockIndex;
            blockIndexList.Add(blockIndex);
        }

        return blockIndexList;

    }
}
