using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class DataManagerTest : MonoBehaviour
{
    private string filePath;
    public Dictionary<string, Monster> LoadedMonsterList { get; private set; }
    public Dictionary<string, CodeBlockData> LoadedCodeBlockList { get; private set; }
    public Dictionary<int, MoveBlock> LoadedMoveBlockList { get; private set; }
    public Dictionary<int, AttackBlock> LoadedAttackBlockList { get; private set; }
    public Dictionary<int, MonsterType> LoadedMonsterType { get; private set; }
    public Dictionary<int, StageMap> LoadedStageMap { get; private set; }
    public Dictionary<int, UIText> LoadedText { get; private set; }
    public Dictionary<string, TextType> LoadedTextType { get; private set; }
    public Dictionary<string, PlayerData> LoadedPlayerData { get; private set; }

    private readonly string _dataRootPath = "Application.streamingAssetsPath";

    public static DataManagerTest Inst { get; private set; }

    private void Awake()
    {
        filePath = Application.persistentDataPath + "/playerData.json";
        Inst = this;
        ReadAllDataOnAwake();
    }

    #region 데이터테이블 로드
    private void ReadAllDataOnAwake()
    {
        LoadedMonsterList = LoadDataTable(nameof(Monster), ParseMonster, m => m.MonsterName);
        LoadedCodeBlockList = LoadDataTable(nameof(CodeBlockData), ParseCodeBlockData, cb => cb.BlockName);
        LoadedMoveBlockList = LoadDataTable(nameof(MoveBlock), ParseMoveBlock, mb => mb.BlockIndex);
        LoadedAttackBlockList = LoadDataTable(nameof(AttackBlock), ParseAttackBlock, ab => ab.BlockIndex);
        LoadedMonsterType = LoadDataTable(nameof(MonsterType), ParseMonsterType, mt => mt.TypeIndex);
        LoadedStageMap = LoadDataTable(nameof(StageMap), ParseStageMap, sm => sm.StageIndex);
        LoadedText = LoadDataTable(nameof(UIText), ParseUIText, ut => ut.TextIndex);
        LoadedTextType = LoadDataTable(nameof(TextType), ParseTextType, tt => tt.TypeName);
        LoadedPlayerData = LoadDataTable(nameof(PlayerData), ParsePlayerData, p => p.PlayerName);
    }
    
    private Dictionary<TKey, TValue> LoadDataTable<TKey, TValue>(string fileName, Func<XElement, TValue> parseElement, Func<TValue, TKey> getKey)
    {
        var dataTable = new Dictionary<TKey, TValue>();

        XDocument doc = XDocument.Load($"{_dataRootPath}/{fileName}.xml");
        var dataElements = doc.Descendants("data");

        foreach (var data in dataElements)
        {
            TValue value = parseElement(data);
            TKey key = getKey(value);
            dataTable.Add(key, value);
        }

        return dataTable;
    }
    #endregion

    #region 데이터 파싱
    private Monster ParseMonster(XElement data)
    {
        return new Monster
        {
            ID = int.Parse(data.Attribute(nameof(Monster.ID)).Value),
            MonsterName = data.Attribute(nameof(Monster.MonsterName)).Value,
            MonsterViewName = data.Attribute(nameof(Monster.MonsterViewName)).Value,
            Description = data.Attribute(nameof(Monster.Description)).Value,
            TypeIndex = int.Parse(data.Attribute(nameof(Monster.TypeIndex)).Value)
        };
    }

    private CodeBlockData ParseCodeBlockData(XElement data)
    {
        return new CodeBlockData
        {
            BlockIndex = int.Parse(data.Attribute(nameof(CodeBlockData.BlockIndex)).Value),
            BlockName = data.Attribute(nameof(CodeBlockData.BlockName)).Value,
            ViewName = data.Attribute(nameof(CodeBlockData.ViewName)).Value,
            Description = data.Attribute(nameof(CodeBlockData.Description)).Value
        };
    }

    private MoveBlock ParseMoveBlock(XElement data)
    {
        return new MoveBlock
        {
            BlockIndex = int.Parse(data.Attribute(nameof(MoveBlock.BlockIndex)).Value),
            MoveDirection = int.Parse(data.Attribute(nameof(MoveBlock.MoveDirection)).Value)
        };
    }

    private AttackBlock ParseAttackBlock(XElement data)
    {
        return new AttackBlock
        {
            BlockIndex = int.Parse(data.Attribute(nameof(AttackBlock.BlockIndex)).Value),
            AttackType = int.Parse(data.Attribute(nameof(AttackBlock.AttackType)).Value)
        };
    }

    private MonsterType ParseMonsterType(XElement data)
    {
        return new MonsterType
        {
            TypeIndex = int.Parse(data.Attribute(nameof(MonsterType.TypeIndex)).Value),
            TypeName = data.Attribute(nameof(MonsterType.TypeName)).Value,
            TypeViewname = data.Attribute(nameof(MonsterType.TypeViewname)).Value,
            Weakness = int.Parse(data.Attribute(nameof(MonsterType.Weakness)).Value)
        };
    }

    private StageMap ParseStageMap(XElement data)
    {
        var tempStageMap = new StageMap
        {
            StageIndex = int.Parse(data.Attribute(nameof(StageMap.StageIndex)).Value),
            StageSize = new Vector2Int(
                int.Parse(data.Attribute("StageXSize").Value),
                int.Parse(data.Attribute("StageYSize").Value)
            ),
            BlockContainerLength = int.Parse(data.Attribute(nameof(StageMap.BlockContainerLength)).Value),
            PlayerSpawnPos = ParseVector2Int(data.Attribute("PlayerSpawnPos").Value)
        };

        SetDataList(out tempStageMap.ArrayInfo, data, "ArrayInfo");
        SetDataList(out tempStageMap.BlockNameList, data, "BlockNameList");
        SetDataList(out tempStageMap.MonsterNameList, data, "MonsterNameList");
        SetDataList(out tempStageMap.MonsterSpawnPosList, data, "MonsterSpawnPosList", ParseVector2Int);

        return tempStageMap;
    }

    private UIText ParseUIText(XElement data)
    {
        return new UIText
        {
            TextIndex = int.Parse(data.Attribute(nameof(UIText.TextIndex)).Value),
            TextTypeIndex = int.Parse(data.Attribute(nameof(UIText.TextTypeIndex)).Value),
            Description = data.Attribute(nameof(UIText.Description)).Value
        };
    }

    private TextType ParseTextType(XElement data)
    {
        return new TextType
        {
            TextTypeIndex = int.Parse(data.Attribute(nameof(TextType.TextTypeIndex)).Value),
            TypeName = data.Attribute(nameof(TextType.TypeName)).Value
        };
    }
    private PlayerData ParsePlayerData(XElement data)
    {
        var tempPlayerData = new PlayerData
        {
            PlayerName = data.Attribute(nameof(PlayerData.PlayerName)).Value
        };

        SetDataList(out tempPlayerData.StartMonsterNameList, data, "StartMonsterNameList");
        
        return tempPlayerData;
    }

    private Vector2Int ParseVector2Int(string value)
    {
        var values = value.Replace("(", "").Replace(")", "").Split(',');
        return new Vector2Int(int.Parse(values[0]), int.Parse(values[1]));
    }

    #endregion

    #region 데이터 세팅
    private void SetDataList<T>(out List<T> usingList, XElement data, string listName, Func<string, T> parseElement = null)
    {
        string ListStr = data.Attribute(listName)?.Value;
        if (!string.IsNullOrEmpty(ListStr))
        {
            ListStr = ListStr.Replace("{", "").Replace("}", "");

            var elements = ListStr.Split(',');

            var list = new List<T>();

            foreach (var element in elements)
            {
                T value = parseElement != null ? parseElement(element) : (T)Convert.ChangeType(element, typeof(T));
                list.Add(value);
            }
            usingList = list;
        }
        else
        {
            usingList = null;
        }
    }
    #endregion 

    #region 데이터 불러오기
    public Monster GetMonsterData(string dataName)
    {
        if (LoadedMonsterList.Count == 0 || !LoadedMonsterList.ContainsKey(dataName))
            return null;

        return LoadedMonsterList[dataName];
    }

    public CodeBlockData GetCodeBlockData(string dataClassName)
    {
        if (LoadedCodeBlockList.Count == 0 || !LoadedCodeBlockList.ContainsKey(dataClassName))
            return null;

        return LoadedCodeBlockList[dataClassName];
    }

    public MoveBlock GetMoveBlockData(int blockIndex)
    {
        if (LoadedMoveBlockList.Count == 0 || !LoadedMoveBlockList.ContainsKey(blockIndex))
            return null;

        return LoadedMoveBlockList[blockIndex];
    }

    public AttackBlock GetAttackBlockData(int blockIndex)
    {
        if (LoadedAttackBlockList.Count == 0 || !LoadedAttackBlockList.ContainsKey(blockIndex))
            return null;

        return LoadedAttackBlockList[blockIndex];
    }

    public StageMap GetStageMapData(int dataIndex)
    {
        if (LoadedStageMap.Count == 0 || !LoadedStageMap.ContainsKey(dataIndex))
            return null;

        return LoadedStageMap[dataIndex];
    }

    public MonsterType GetMonsterTypeData(int dataIndex)
    {
        if (LoadedMonsterType.Count == 0 || !LoadedMonsterType.ContainsKey(dataIndex))
            return null;

        return LoadedMonsterType[dataIndex];
    }

    public UIText GetTextMapData(int dataIndex)
    {
        if (LoadedText.Count == 0 || !LoadedText.ContainsKey(dataIndex))
            return null;

        return LoadedText[dataIndex];
    }

    public TextType GetTextTypeData(string dataClassName)
    {
        if (LoadedTextType.Count == 0 || !LoadedTextType.ContainsKey(dataClassName))
            return null;

        return LoadedTextType[dataClassName];
    }
    public PlayerData GetPlayerData(string dataClassName)
    {
        if (LoadedPlayerData.Count == 0 || !LoadedPlayerData.ContainsKey(dataClassName))
            return null;

        return LoadedPlayerData[dataClassName];
    }
    #endregion
}
