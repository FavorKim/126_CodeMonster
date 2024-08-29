using EnumTypes;
using EventLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeLoopBlockContainerManager : BlockContainerManager
{
    List<MaterialChanger> loopMaterialChangers = new List<MaterialChanger>();

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
        List<int> baseList = base.GetContatinerBlocks();

        return baseList;
    }

    public override void SetBlockMaterial(int index, MaterialType type)
    {
        loopMaterialChangers[index].ChangeMaterial(type);
    }

    public override int CountCodeBlockDragComponents()
    {
        int count = base.CountCodeBlockDragComponents();

        return count;
    }
}
