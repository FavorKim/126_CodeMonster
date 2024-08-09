using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster
{
    public int ID { get; set; }
    public string MonsterName { get; set; }
    public string ViewName { get; set; }
    public string Description { get; set; }
    public int TypeIndex {  get; set; }
}

public class CodeBlockData
{
    public int BlockIndex {  get; set; }
    public string BlockName { get; set; }
    public string ViewName { get; set; }
    public string Description { get; set; }
}

public class StageMap
{
    public int StageIndex { get; set; }
    public int StageXSize { get; set; }
    public int StageYSize { get; set; }
    public List<int> ArrayInfo = new List<int>();
    public List<int> BlockIndexList = new List<int>();
    public int BlockContainerLength {  get; set; }
    public List<int> MonsterIDList = new List<int>();
    public List<int>MonsterSpawnPosXList = new List<int>();
    public List<int> MonsterSpawnPosYList = new List<int>();
}

public class MonsterType
{
    public int TypeIndex { get; set; }
    public string TypeName { get; set; }
    public string Viewname {  get; set; }
}

public class UIText
{
    public int TextIndex { get; set; }
    public int TextTypeIndex { get; set; }
    public string Description { get; set; }
}

public class TextType
{
    public int TextTypeIndex { get; set; }
    public string TypeName {  get; set; }

}
