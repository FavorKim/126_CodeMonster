using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    private List<string> _playerMonsterNameList = new List<string>();

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        _playerMonsterNameList = DataManagerTest.instance.GetPlayerData("Player").StartMonsterNameList;
    }


    public void AddMonsterInPlayerList(string monsterName)//플레이어 보유 몬스터에 몬스터를 추가
    {
        var name = DataManagerTest.instance.RemoveTextAfterParenthesis(monsterName);

        if (CheckMonsterInPlayerList(name) == false)
        {
            _playerMonsterNameList.Add(name);
        }
        else
        {
            return;
        }
    }

    public bool CheckMonsterInPlayerList(string monsterName)//플레이어 보유 몬스터에 몬스터가 있는지 검사
    {
        var name = DataManagerTest.instance.RemoveTextAfterParenthesis(monsterName);

        if (_playerMonsterNameList.Contains(name))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public string GetMonsterNameInPlayerList(string monsterName)//플레이어 보유 목록에서 보유한 몬스터의 이름을 반환 단 ( 가 있으면 ( 뒤는 삭제후 반환
    {
        var name = DataManagerTest.instance.RemoveTextAfterParenthesis(monsterName);

        if (CheckMonsterInPlayerList(name))
        {
            return name;
        }
        else
        {
            return string.Empty;
        }
    }

    public List<int> GetMonsterTypeInIndex(int stageIndex)
    {
        List<int> list = DataManagerTest.instance.GetStageMapData(stageIndex).MonsterIDList;

        if (DataManagerTest.instance.GetStageMapData(stageIndex).BushMonsterIDList != null)
        {
            foreach (var item in DataManagerTest.instance.GetStageMapData(stageIndex).BushMonsterIDList)
            {

                var replaceString = item.Replace("(", "").Replace(")", "");

                var elements = replaceString.Split('/');

                for (int i = 0; i < elements.Length; i++)
                {
                    var element = elements[i];
                    list.Add(int.Parse(element));
                }
            }
        }

        List<int> monsterTypeIndexList = new List<int>();
        foreach (var item in list)
        {
            int monsterTypeIndex = DataManagerTest.instance.GetMonsterData(item).TypeIndex;

            if (monsterTypeIndexList.Contains(monsterTypeIndex) == false)
            {
                monsterTypeIndexList.Add(monsterTypeIndex);
            }
        }

        return monsterTypeIndexList;
    }
}
