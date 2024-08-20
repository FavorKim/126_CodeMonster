using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : Singleton<BattleManager> 
{
    //플레이어에서 넘어온 공격명령에 대한 배틀페이즈 실행.


    private DataManagerTest dataManager;
    private Player player;
    private StageManager stageManager;

    private void Awake()
    {
        dataManager = DataManagerTest.Instance;
        stageManager = StageManager.Instance;
        player = stageManager.GetPlayer();
    }

    public void BattlePhase(Vector2Int playerPosition, int attackBlockType)
    {


        DebugBoxManager.Instance.Log($"{dataManager.GetMonsterTypeData(attackBlockType).TypeViewName} Type Attack");
        // 플레이어의 위치와 적의 위치를 비교하여 같은 위치에 있는지 확인

        if (stageManager.CheckMonsterAndPlayerPos(playerPosition))
        {
            GameObject monsterObj= stageManager.GetMonsterWithPlayerPos(playerPosition);
            
            string monsterName = monsterObj.name;
            Monster monster = DataManagerTest.Instance.GetMonsterData(monsterName);
            MonsterType monsterType = DataManagerTest.Instance.GetMonsterTypeData(monster.TypeIndex);

            // 공격 블록의 타입과 몬스터의 약점 비교
            if (GameRule.CompareType(attackBlockType, monsterType.TypeIndex))
            {
                Debug.Log("Attack successful! Monster defeated.");
                Debug.Log(dataManager.GetMonsterTypeData(attackBlockType).TypeViewName + "으로 공격함");
                DebugBoxManager.Instance.Log($"{dataManager.GetMonsterTypeData(attackBlockType).TypeViewName} Type Attack");
                // 승리 처리: 플레이어의 승리 메서드 호출
                //-> 플레이어의 공격 애니메이션과 이펙트가 끝나면
                StageManager.Instance.GetPlayer().Win();
                monsterObj.SetActive(false);
            }
            else
            {
                Debug.Log("Attack failed! Player defeated.");
                // 패배 처리: 플레이어의 패배 메서드 호출

                player.EnableTypeMonsterPrefab(4);
                StageManager.Instance.GetPlayer().Defeat();
            }
        }
        else
        {
            // 적의 위치와 일치하지 않는 경우, 공격이 빗나간 것으로 간주하여 패배 처리
            //-> 빗나가면 아무일도 없으니 승리 처리로 바꿈
            Debug.Log("Attack missed!");
            StageManager.Instance.GetPlayer().Win();
        }




    }

    private IEnumerator TestTimer()
    {
        float time = 0;
        float checkTime = 2;
        while (time <= checkTime) 
        {
            time += Time.deltaTime;
            yield return null;
        }
    }
}
