using EnumTypes;
using EventLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeLoopBlockContainerManager : BlockContainerManager
{

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            GetMakeLoopBlocksName();
        }
    }
    public override void AddBlock(GameObject newBlock)
    {
        base.AddBlock(newBlock);
    }

    public override void ResetBlockContainer()
    {
        base.ResetBlockContainer();
    }

    public override List<int> GetContatinerBlocks()
    {
        return base.GetContatinerBlocks();
    }

    public override void SetBlockMaterial(int index, MaterialType type)
    {
        base.SetBlockMaterial(index, type);
    }

    public override int CountCodeBlockDragComponents()
    {
        return base.CountCodeBlockDragComponents();
    }

    public void GetMakeLoopBlocksName()
    {
        if (this.transform.childCount <= 0)
        {
            return;
        }

        for (int i = 0; i < this.transform.childCount; i++)
        {
            BlockName codeBlockDrag = transform.GetChild(i).GetComponent<CodeBlockDrag>().BlockName;
            UIManager.Instance.LoopBlockList.Add(codeBlockDrag);
        }
    }

    public void LoseMakeLoopBlocksName()
    {
        ResetBlockContainer();
        UIManager.Instance.LoopBlockList = null;
    }
}
