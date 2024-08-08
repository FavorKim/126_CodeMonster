using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using System.IO;
using UnityEditor.EditorTools;

public class DataManger : MonoBehaviour
{
    private string filePath;
    public Dictionary<string, Monster> LoadedMonsterList { get; private set; }
    public Dictionary<string, CodeBlock> LoadedCodeBlockList { get; private set; }
    public Dictionary<int, MonsterType> LoadedMonsterType { get; private set; }
    public Dictionary<int, StageMap> LoadedStageMap { get; private set; }
    public Dictionary<int, UIText> LoadedText { get; private set; }
    public Dictionary<string, TextType> LoadedTextType { get; private set; }

    private readonly string _dataRootPath = "C:/Users/KGA/Desktop/PizzaDataTable";//읽는거 실패시 \\을 /로
    private readonly string _dataRootPathInHome = "C:/Users/qkr38/Downloads/PizzaDataTable";
    public static DataManger Inst { get; private set; }

    private void Awake()
    {
        filePath = Application.persistentDataPath + "/playerData.json";
        Inst = this;
        ReadAllDataOnAwake();
    }

    private void ReadAllDataOnAwake()
    {

        ReadData(nameof(Monster));
        ReadData(nameof(CodeBlock));
        ReadData(nameof(MonsterType));
        ReadData(nameof(StageMap));
        ReadData(nameof(UIText));
        ReadData(nameof(TextType));

        //if (File.Exists(filePath))
        //{
        //    // 파일에서 JSON 문자열 읽기
        //    string jsonData = File.ReadAllText(filePath);

        //    // JSON 문자열을 객체로 변환
        //    Player data = new Player();
        //    data = LoadData();
        //    //data.Name = jsonData.Attribute(nameof(data.Name)).Value;
        //    //data.StartPizzaRecipe = data.Attribute(nameof(data.StartPizzaRecipe)).Value;
        //    //data.StartMoney = int.Parse(data.Attribute(nameof(data.StartMoney)).Value);
        //    //SetDataList(out data.StartToppingResorceList, data, "StartToppingResorceList");


        //}
        //else
        //{
        //    ReadData(nameof(Player));

        //}
    }

    private void ReadData(string name)
    {
        switch (name)
        {
            case nameof(Monster):
                ReadMonsterTable(name);
                break;
            case nameof(CodeBlock):
                ReadCodeBlockTable(name);
                break;
            case nameof(MonsterType):
                ReadMonsterTypeTable(name);
                break;
            case nameof(StageMap):
                ReadStageMapTable(name);
                break;
            case nameof(UIText):
                ReadTextTable(name);
                break;
            case nameof(TextType):
                ReadTextTypeTable(name);
                break;
        }
    }

    
    private void ReadMonsterTable(string name)
    {
        LoadedMonsterList = new Dictionary<string, Monster>();

        XDocument doc = XDocument.Load($"{_dataRootPath}/{name}.xml");

        var dataElements = doc.Descendants("data");

        foreach (var data in dataElements)
        {
            var tempMonster = new Monster();
            tempMonster.ID = int.Parse(data.Attribute(nameof(tempMonster.ID)).Value);
            tempMonster.MonsterName = data.Attribute(nameof(tempMonster.MonsterName)).Value;
            tempMonster.ViewName = data.Attribute(nameof(tempMonster.ViewName)).Value;
            tempMonster.Description = data.Attribute(nameof(tempMonster.Description)).Value;
            tempMonster.TypeIndex = int.Parse(data.Attribute(nameof(tempMonster.TypeIndex)).Value);

            LoadedMonsterList.Add(tempMonster.MonsterName, tempMonster);
        }
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
                foreach (var pizzaName in names)
                {
                    T value = (T)Convert.ChangeType(pizzaName, typeof(T));
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
    
    private void ReadCodeBlockTable(string name)
    {
        LoadedCodeBlockList = new Dictionary<string, CodeBlock>();

        XDocument doc = XDocument.Load($"{_dataRootPath}/{name}.xml");

        var dataElements = doc.Descendants("data");

        foreach (var data in dataElements)
        {
            var tempCodeBlock = new CodeBlock();
            tempCodeBlock.BlockIndex = int.Parse(data.Attribute(nameof(tempCodeBlock.BlockIndex)).Value);
            tempCodeBlock.BlockName = data.Attribute(nameof(tempCodeBlock.BlockName)).Value;
            tempCodeBlock.Description = data.Attribute(nameof(tempCodeBlock.Description)).Value;
            tempCodeBlock.ViewName = data.Attribute(nameof(tempCodeBlock.ViewName)).Value;

            LoadedCodeBlockList.Add(tempCodeBlock.BlockName, tempCodeBlock);
        }
    }

    private void ReadStageMapTable(string name)
    {
        LoadedStageMap = new Dictionary<int, StageMap>();

        XDocument doc = XDocument.Load($"{_dataRootPath}/{name}.xml");

        var dataElements = doc.Descendants("data");

        foreach (var data in dataElements)
        {
            var tempStageMap = new StageMap();
            tempStageMap.StageIndex = int.Parse(data.Attribute(nameof(tempStageMap.StageIndex)).Value);
            tempStageMap.StageXSize = int.Parse(data.Attribute(nameof(tempStageMap.StageXSize)).Value);
            tempStageMap.StageYSize = int.Parse(data.Attribute(nameof(tempStageMap.StageYSize)).Value);
            tempStageMap.BlockContainerLength = int.Parse(data.Attribute(nameof(tempStageMap.BlockContainerLength)).Value);
            SetDataList(out tempStageMap.ArrayInfo, data, "ArrayInfo");
            SetDataList(out tempStageMap.BlockIndexList, data, "BlockIndexList");
            SetDataList(out tempStageMap.MonsterIDList, data, "MonsterIDList");
            SetDataList(out tempStageMap.MonsterSpawnPosXList, data, "MonsterSpawnPosXList");
            SetDataList(out tempStageMap.MonsterSpawnPosYList, data, "MonsterSpawnPosYList");
            LoadedStageMap.Add(tempStageMap.StageIndex, tempStageMap);
        }




    }
    private void ReadMonsterTypeTable(string name)
    {
        LoadedMonsterType = new Dictionary<int, MonsterType>();

        XDocument doc = XDocument.Load($"{_dataRootPath}/{name}.xml");

        var dataElements = doc.Descendants("data");

        foreach (var data in dataElements)
        {
            var tempType = new MonsterType();
            tempType.TypeIndex = int.Parse(data.Attribute(nameof(tempType.TypeIndex)).Value);
            tempType.TypeName = data.Attribute(nameof(tempType.TypeName)).Value;
            tempType.Viewname = data.Attribute(nameof(tempType.Viewname)).Value;
            LoadedMonsterType.Add(tempType.TypeIndex, tempType);
        }




    }
    private void ReadTextTable(string name)
    {
        LoadedText = new Dictionary<int, UIText>();

        XDocument doc = XDocument.Load($"{_dataRootPath}/{name}.xml");

        var dataElements = doc.Descendants("data");

        foreach (var data in dataElements)
        {
            var tempText = new UIText();
            tempText.TextIndex = int.Parse(data.Attribute(nameof(tempText.TextIndex)).Value);
            tempText.TextTypeIndex = int.Parse(data.Attribute(nameof(tempText.TextTypeIndex)).Value);
            tempText.Description = data.Attribute(nameof(tempText.Description)).Value;
            LoadedText.Add(tempText.TextIndex, tempText);
        }




    }
    private void ReadTextTypeTable(string name)
    {
        LoadedTextType = new Dictionary<string, TextType>();

        XDocument doc = XDocument.Load($"{_dataRootPath}/{name}.xml");

        var dataElements = doc.Descendants("data");

        foreach (var data in dataElements)
        {
            var tempTextType = new TextType();
            tempTextType.TextTypeIndex = int.Parse(data.Attribute(nameof(tempTextType.TextTypeIndex)).Value);
            tempTextType.TypeName = data.Attribute(nameof(tempTextType.TypeName)).Value;
            LoadedTextType.Add(tempTextType.TypeName, tempTextType);
        }




    }
    public Monster GetMonsterData(string dataName)
    {
        if (LoadedMonsterList.Count == 0
            || !LoadedMonsterList.ContainsKey(dataName))
            return null;

        //딕셔너리는 찾아주는게 빠르다
        return LoadedMonsterList[dataName];
    }
    public CodeBlock GetCodeBlockData(string dataClassName)
    {
        if (LoadedCodeBlockList.Count == 0
            || !LoadedCodeBlockList.ContainsKey(dataClassName))
            return null;

        //딕셔너리는 찾아주는게 빠르다
        return LoadedCodeBlockList[dataClassName];
    }

    public StageMap GetStageMapData(int dataIndex)
    {
        if (LoadedStageMap.Count == 0
            || !LoadedStageMap.ContainsKey(dataIndex))
            return null;

        //딕셔너리는 찾아주는게 빠르다
        return LoadedStageMap[dataIndex];
    }
    public MonsterType GetTypeData(int dataIndex)
    {
        if (LoadedMonsterType.Count == 0
            || !LoadedMonsterType.ContainsKey(dataIndex))
            return null;

        //딕셔너리는 찾아주는게 빠르다
        return LoadedMonsterType[dataIndex];
    }
    public UIText GetTextMapData(int dataIndex)
    {
        if (LoadedText.Count == 0
            || !LoadedText.ContainsKey(dataIndex))
            return null;

        //딕셔너리는 찾아주는게 빠르다
        return LoadedText[dataIndex];
    }
    public TextType GetTextTypeData(string dataClassName)
    {
        if (LoadedTextType.Count == 0
            || !LoadedTextType.ContainsKey(dataClassName))
            return null;

        //딕셔너리는 찾아주는게 빠르다
        return LoadedTextType[dataClassName];
    }
    //private void LoadFile()
    //{
    //    // 파일 경로 설정
    //    filePath = Application.persistentDataPath + "/playerData.json";

    //    // 데이터 생성
    //    Player data = new Player();

    //    // 데이터 저장
    //    SaveData();

    //}

    //public void SaveData()
    //{
    //    Player data = new Player();
    //    data.Name = PlayerController.Instance.Player.Name;
    //    data.StartPizzaRecipe = PlayerController.Instance.PizaaRecipe;
    //    data.StartToppingResorceList = PlayerController.Instance.PizaaToppingResorce;
    //    data.StartMoney = PlayerController.Instance.PlayerMoney;
    //    data.ToppingResorceCountList = PoolManger.Instance.ReturnToppingResorceCount();
    //    // 객체를 JSON 문자열로 변환
    //    string jsonData = JsonUtility.ToJson(data, true);

    //    // JSON 문자열을 파일에 저장
    //    File.WriteAllText(filePath, jsonData);

    //}
    //public IEnumerator SaveDataCo()
    //{
    //    Player data = new Player();
    //    data.Name = PlayerController.Instance.Player.Name;
    //    data.StartPizzaRecipe = PlayerController.Instance.PizaaRecipe;
    //    data.StartToppingResorceList = PlayerController.Instance.PizaaToppingResorce;
    //    data.StartMoney = PlayerController.Instance.PlayerMoney;
    //    data.ToppingResorceCountList = PoolManger.Instance.ReturnToppingResorceCount();
    //    // 객체를 JSON 문자열로 변환
    //    string jsonData = JsonUtility.ToJson(data, true);

    //    // JSON 문자열을 파일에 저장
    //    File.WriteAllText(filePath, jsonData);
    //    yield return null;

    //}

    //Player LoadData()
    //{
    //    // 파일이 존재하는지 확인
    //    if (File.Exists(filePath))
    //    {
    //        // 파일에서 JSON 문자열 읽기
    //        string jsonData = File.ReadAllText(filePath);
    //        LoadedPlayer = new Dictionary<string, Player>();
    //        // JSON 문자열을 객체로 변환
    //        Player data = JsonUtility.FromJson<Player>(jsonData);
    //        LoadedPlayer.Add(data.Name, data);
    //        Debug.Log("Data loaded from " + filePath);
    //        return data;
    //    }
    //    else
    //    {
    //        Debug.LogWarning("No data file found at " + filePath);
    //        return null;
    //    }
    //}
    //public bool IsFileCheck()
    //{
    //    if (File.Exists(filePath))
    //    {
    //        return true;
    //    }
    //    else
    //    {
    //        return false;
    //    }
    //}
}
