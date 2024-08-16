using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : Singleton<BattleManager> 
{
    //플레이어에서 넘어온 공격명령에 대한 배틀페이즈 실행.

    //public static BattleManager Instance { get; private set; }

    private DataManagerTest dataManager;
    private Player player;
    private StageManager stageManager;

    protected override void Awake()
    {
        base.Awake();
        dataManager = DataManagerTest.Instance;
        stageManager = StageManager.Instance;
        player = stageManager.GetPlayer();
    }

    public void BattlePhase(Vector2Int playerPosition, int attackBlockType)
    {
        // 적의 위치를 얻기 위해 적들이 있는 스폰 리스트를 가져옴
        List<Vector2Int> monsterPositions = stageManager.GetMonsterSpawnList();

        // 플레이어의 위치와 적의 위치를 비교하여 같은 위치에 있는지 확인
        for (int i = 0; i < monsterPositions.Count; i++)
        {
            if (GameRule.ComparePosition(playerPosition, monsterPositions[i]))
            {
                string monsterName = stageManager.GetMonsterNameAtIndex(i);
                Monster monster = DataManagerTest.Instance.GetMonsterData(monsterName);
                MonsterType monsterType = DataManagerTest.Instance.GetMonsterTypeData(monster.TypeIndex);

                // 공격 블록의 타입과 몬스터의 약점 비교
                if (GameRule.CompareType(attackBlockType, monsterType.Weakness))
                {
                    Debug.Log("Attack successful! Monster defeated.");
                    // 승리 처리: 플레이어의 승리 메서드 호출
                    StageManager.Instance.GetPlayer().Win();
                }
                else
                {
                    Debug.Log("Attack failed! Player defeated.");
                    // 패배 처리: 플레이어의 패배 메서드 호출
                    StageManager.Instance.GetPlayer().Defeat();
                }
                return;
            }
        }

        // 적의 위치와 일치하지 않는 경우, 공격이 빗나간 것으로 간주하여 패배 처리
        Debug.Log("Attack missed! Player defeated.");
        StageManager.Instance.GetPlayer().Defeat();
    }
}
