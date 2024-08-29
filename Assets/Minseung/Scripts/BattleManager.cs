using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class BattleManager : Singleton<BattleManager> 
{
    //플레이어에서 넘어온 공격명령에 대한 배틀페이즈 실행.


    private DataManagerTest dataManager;
    private Player player;
    private StageManager stageManager;
    private GameObject targetMonster;

    private void Awake()
    {
        dataManager = DataManagerTest.Instance;
        stageManager = StageManager.Instance;
       
    }

    public void BattlePhase(Vector2Int playerPosition, int attackBlockType)
    {
        player = stageManager.GetPlayer();

        DebugBoxManager.Instance.Log($"{dataManager.GetMonsterTypeData(attackBlockType).TypeViewName} 타입 공격");
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
                targetMonster = monsterObj;
                UnityEngine.Debug.Log("Attack successful! Monster defeated.");
                UnityEngine.Debug.Log(dataManager.GetMonsterTypeData(attackBlockType).TypeViewName + "으로 공격함");
                //DebugBoxManager.Instance.Log($"{dataManager.GetMonsterTypeData(attackBlockType).TypeViewName} Type Attack");
                // 승리 처리: 플레이어의 승리 메서드 호출
                //-> 플레이어의 공격 애니메이션과 이펙트가 끝나면
                Invoke(nameof(PlayerWin), 2);
            }
            else
            {
                UnityEngine.Debug.Log("Attack failed! Player defeated.");
                // 패배 처리: 플레이어의 패배 메서드 호출
                UIManager.Instance.BlockContainerManager.SetXIcon(player.GetCurrentBlockIndex(), true);

                //player.EnableTypeMonsterPrefab(4);
                Invoke(nameof(PlayerDefeat), 2);
            }
        }
        else
        {
            // 적의 위치와 일치하지 않는 경우, 공격이 빗나간 것으로 간주하여 패배 처리
            //-> 빗나가면 아무일도 없으니 승리 처리로 바꿈
            DebugBoxManager.Instance.Log("공격 빗나감 (같은 자리에 없음)");
            UIManager.Instance.BlockContainerManager.SetXIcon(player.GetCurrentBlockIndex(), true);
            targetMonster = null;
            Invoke(nameof(PlayerWin), 2);
        }




    }

    private void TestTimer(float time)
    {
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        while (stopwatch.Elapsed.TotalSeconds < time)
        {
            // 여기서 시간이 2초가 될 때까지 기다립니다.
            // 이 동안 게임은 멈춘 상태로 있게 됩니다.
        }
    }

    private void PlayerWin()
    {
        player.Win();
        if (targetMonster != null && targetMonster.activeSelf == true) 
        {
            targetMonster.SetActive(false);

        }
    }

    private void PlayerDefeat()
    {
        player.Defeat();
    }
}
