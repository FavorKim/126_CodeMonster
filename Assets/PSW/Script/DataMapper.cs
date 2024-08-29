using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster
{
    public int ID { get; set; }
    public string MonsterName { get; set; }
    public string MonsterViewName { get; set; }
    public int TypeIndex { get; set; }
    public string Description { get; set; }
}

public class CodeBlockData
{
    public int BlockIndex { get; set; }
    public string BlockName { get; set; }
    public string ViewName { get; set; }
    public string Description { get; set; }
}

public class MoveBlock
{
    public int BlockIndex { get; set; }
    public int MoveDirection { get; set; }
}

public class AttackBlock
{
    public int BlockIndex { get; set; }
    public int AttackType { get; set; }
}

public class LoopBlock
{
    public int BlockIndex { get; set; } // 반복 블록의 인덱스
    public int LoopCount { get; set; } // 반복 횟수
    public List<int> SubBlockIndices { get; set; }  // 반복 블록 안에 포함된 블록들의 인덱스 목록
}

public class ConditionalBlock
{
    public int BlockIndex { get; set; } // 조건 블록의 인덱스
    public Func<bool> Condition { get; set; } // 조건부를 평가하는 함수
    public int ActionBlockIndex { get; set; } // 조건이 참일 때 실행할 블록(여기에 공격블록과 이동블록의 인덱스를 넣는다.)
}

    public class StageMap
{
    public int StageIndex { get; set; }
    public Vector2Int StageSize { get; set; }
    public List<int> ArrayInfo = new List<int>();
    public List<string> BlockNameList = new List<string>();
    public int BlockContainerLength { get; set; }
    public List<string> MonsterNameList = new List<string>();
    public List<Vector2Int> MonsterSpawnPosList = new List<Vector2Int>();
    public Vector2Int PlayerSpawnPos { get; set; }
}

public class MonsterType
{
    public int TypeIndex { get; set; }
    public string TypeName { get; set; }
    public string TypeViewName { get; set; }
    public int Weakness {  get; set; }
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
    public string TypeName { get; set; }
}

public class PlayerData
{
    public string PlayerName { get; set; }
    public List<string> StartMonsterNameList = new List<string>();
}
