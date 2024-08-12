using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class DataManagerTest : MonoBehaviour
{
    private string filePath;
    public Dictionary<string, Monster> LoadedMonsterList { get; private set; }
    public Dictionary<string, CodeBlockData> LoadedCodeBlockList { get; private set; }
    public Dictionary<int, MonsterType> LoadedMonsterType { get; private set; }
    public Dictionary<int, StageMap> LoadedStageMap { get; private set; }
    public Dictionary<int, UIText> LoadedText { get; private set; }
    public Dictionary<string, TextType> LoadedTextType { get; private set; }

    private readonly string _dataRootPath = "C:/Users/KGA/Desktop/PizzaDataTable";

    public static DataManagerTest Inst { get; private set; }

    private void Awake()
    {
        filePath = Application.persistentDataPath + "/playerData.json";
        Inst = this;
        ReadAllDataOnAwake();
    }

    private void ReadAllDataOnAwake()
    {
        LoadedMonsterList = LoadDataTable(nameof(Monster), ParseMonster, m => m.MonsterName);
        LoadedCodeBlockList = LoadDataTable(nameof(CodeBlockData), ParseCodeBlockData, cb => cb.BlockName);
        LoadedMonsterType = LoadDataTable(nameof(MonsterType), ParseMonsterType, mt => mt.TypeIndex);
        LoadedStageMap = LoadDataTable(nameof(StageMap), ParseStageMap, sm => sm.StageIndex);
        LoadedText = LoadDataTable(nameof(UIText), ParseUIText, ut => ut.TextIndex);
        LoadedTextType = LoadDataTable(nameof(TextType), ParseTextType, tt => tt.TypeName);
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

    private Monster ParseMonster(XElement data)
    {
        return new Monster
        {
            ID = int.Parse(data.Attribute(nameof(Monster.ID)).Value),
            MonsterName = data.Attribute(nameof(Monster.MonsterName)).Value,
            ViewName = data.Attribute(nameof(Monster.ViewName)).Value,
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

    private MonsterType ParseMonsterType(XElement data)
    {
        return new MonsterType
        {
            TypeIndex = int.Parse(data.Attribute(nameof(MonsterType.TypeIndex)).Value),
            TypeName = data.Attribute(nameof(MonsterType.TypeName)).Value,
            Viewname = data.Attribute(nameof(MonsterType.Viewname)).Value
        };
    }

    private StageMap ParseStageMap(XElement data)
    {
        var tempStageMap = new StageMap
        {
            StageIndex = int.Parse(data.Attribute(nameof(StageMap.StageIndex)).Value),
            StageXSize = int.Parse(data.Attribute(nameof(StageMap.StageXSize)).Value),
            StageYSize = int.Parse(data.Attribute(nameof(StageMap.StageYSize)).Value),
            BlockContainerLength = int.Parse(data.Attribute(nameof(StageMap.BlockContainerLength)).Value)
        };

        SetDataList(out tempStageMap.ArrayInfo, data, "ArrayInfo");
        SetDataList(out tempStageMap.BlockIndexList, data, "BlockIndexList");
        SetDataList(out tempStageMap.MonsterIDList, data, "MonsterIDList");
        SetDataList(out tempStageMap.MonsterSpawnPosXList, data, "MonsterSpawnPosXList");
        SetDataList(out tempStageMap.MonsterSpawnPosYList, data, "MonsterSpawnPosYList");

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

    private void SetDataList<T>(out List<T> usingList, XElement data, string listName)
    {
        string ListStr = data.Attribute(listName).Value;
        if (!string.IsNullOrEmpty(ListStr))
        {
            ListStr = ListStr.Replace("{", string.Empty);
            ListStr = ListStr.Replace("}", string.Empty);

            var names = ListStr.Split(',');

            var list = new List<T>();

            if (names.Length > 0)
            {
                foreach (var name in names)
                {
                    T value = (T)Convert.ChangeType(name, typeof(T));
                    list.Add(value);
                }
            }
            usingList = list;
        }
        else
        {
            usingList = null;
        }
    }

    public Monster GetMonsterData(string dataName)
    {
        if (LoadedMonsterList.Count == 0
            || !LoadedMonsterList.ContainsKey(dataName))
            return null;

        return LoadedMonsterList[dataName];
    }

    public CodeBlockData GetCodeBlockData(string dataClassName)
    {
        if (LoadedCodeBlockList.Count == 0
            || !LoadedCodeBlockList.ContainsKey(dataClassName))
            return null;

        return LoadedCodeBlockList[dataClassName];
    }

    public StageMap GetStageMapData(int dataIndex)
    {
        if (LoadedStageMap.Count == 0
            || !LoadedStageMap.ContainsKey(dataIndex))
            return null;

        return LoadedStageMap[dataIndex];
    }

    public MonsterType GetTypeData(int dataIndex)
    {
        if (LoadedMonsterType.Count == 0
            || !LoadedMonsterType.ContainsKey(dataIndex))
            return null;

        return LoadedMonsterType[dataIndex];
    }

    public UIText GetTextMapData(int dataIndex)
    {
        if (LoadedText.Count == 0
            || !LoadedText.ContainsKey(dataIndex))
            return null;

        return LoadedText[dataIndex];
    }

    public TextType GetTextTypeData(string dataClassName)
    {
        if (LoadedTextType.Count == 0
            || !LoadedTextType.ContainsKey(dataClassName))
            return null;

        return LoadedTextType[dataClassName];
    }
}
