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
    private MonsterController targetMonsterController;

    private void Awake()
    {
        dataManager = DataManagerTest.Instance;
        stageManager = StageManager.Instance;

    }

    public void BattlePhase(Vector2Int playerPosition, int attackBlockType)
    {
        //DebugBoxManager.Instance.Log("배틀페이즈로 들어옴");
        player = stageManager.GetPlayer();
        //DebugBoxManager.Instance.Log($"{dataManager.GetMonsterTypeData(attackBlockType).TypeViewName} 타입 공격");
        // 플레이어의 위치와 적의 위치를 비교하여 같은 위치에 있는지 확인

        if (stageManager.CheckMonsterAndPlayerPos(playerPosition))
        {
            //DebugBoxManager.Instance.Log("몬스터와 같은 공간");

            GameObject monsterObj;
            // 부시라면
            if (stageManager.CheckBushAndPlayerPos(playerPosition))
            {
                //DebugBoxManager.Instance.Log("부시에 있음");
                int rand = UnityEngine.Random.Range(0, 1);
                // 조건문을 사용했을 경우
                if (attackBlockType == 8)
                {
                    // 랜덤으로 몬스터 생성
                    monsterObj = stageManager.GetMonsterInBush(playerPosition, rand);
                    DebugBoxManager.Instance.Log($"{monsterObj.name}");
                    GameObject pref = MonsterObjPoolManger.Instance.GetMonsterPrefab(monsterObj.name);
                    pref.SetActive(true);
                    GameObject bush = stageManager.GetMonsterWithPlayerPos(playerPosition).transform.GetChild(0).gameObject;
                    pref.transform.position = bush.transform.position;
                    pref.transform.position = new Vector3(pref.transform.position.x, player.transform.position.y, pref.transform.position.z);
                    bush.SetActive(false);
                    bush.transform.parent.gameObject.SetActive(false);
                    // 리셋 시 초기화를 위한 이벤트 구독
                    InteractEventManager.Instance.RegistOnPokeBtn(PokeButton.RESET, () => { pref.SetActive(false); bush.gameObject.SetActive(true); });
                    InteractEventManager.Instance.RegistOnPokeBtn(PokeButton.RESTART, () => { pref.SetActive(false); bush.gameObject.SetActive(true); });

                    //DebugBoxManager.Instance.Log("조건 + 부시");
                }
                else
                {
                    DebugBoxManager.Instance.Log("부쉬에서 조건문을 사용하지 않음. 패배");
                    UIManager.Instance.BlockContainerManager.SetXIcon(player.CurrentIndex, true);

                    //monsterObj = null;
                    return;
                }
            }
            else
            {
                //DebugBoxManager.Instance.Log("부시가 아님");
                monsterObj = stageManager.GetMonsterWithPlayerPos(playerPosition);
            }

            string monsterName = monsterObj.name;
            DebugBoxManager.Instance.Log(monsterName);
            Monster monster = DataManagerTest.Instance.GetMonsterData(monsterName);
            if(monster == null)
            {
                DebugBoxManager.Instance.Log("monster 널");
            }
            MonsterType monsterType = DataManagerTest.Instance.GetMonsterTypeData(monster.TypeIndex + 4);
            if (monsterObj == null)
            {
                DebugBoxManager.Instance.Log("몬스터오브젝 널");
            }
            targetMonsterController = monsterObj.GetComponent<MonsterController>();
            if (targetMonsterController == null)
            {
                DebugBoxManager.Instance.Log("타겟몬스터 널");
            }

            //DebugBoxManager.Instance.Log($"플레이어 {monsterName}몬스터의 속성은 {monsterType.TypeIndex}번 인덱스");

            // 조건문을 사용할 경우 등장한 조건문에 맞는 공격블록으로 인덱스 변경
            if (attackBlockType == 8)
            {
                if (player.isLoop == false)
                {
                    attackBlockType = UIManager.Instance.BlockContainerManager.GetConditionBlockByIndex(player.CurrentIndex).EvaluateCondition(monster);
                    DebugBoxManager.Instance.Log($"{attackBlockType}번 인덱스 공격 (불 물 풀)");
                }
                else
                {
                    int curIndex = player.ForceGetCurrentIndex();
                    SetLoopBlockUI loop = UIManager.Instance.BlockContainerManager.GetLoopBlockByIndex(curIndex);
                    if(loop == null)
                    {
                        DebugBoxManager.instance.Log($"루프 중 조건 공격 중 loop null");
                    }
                    ConditionBlock cond = loop.GetConditionBlockByIndex(player.CurrentIndex);
                    if(cond == null)
                    {
                        DebugBoxManager.instance.Log($"루프 중 조건 공격 중 cond null");
                    }
                    attackBlockType = cond.EvaluateCondition(monster);
                    DebugBoxManager.instance.Log($"{attackBlockType}번 인덱스 공격 (반복중)");
                }
            }

            player.EnableTypeMonsterPrefab(attackBlockType);

            UnityEngine.Debug.LogWarning($"공격블록 타입 : {attackBlockType}");
            UnityEngine.Debug.LogWarning($"적 몬스터 타입 : {monsterType.TypeIndex}");
            // 공격 블록의 타입과 몬스터의 약점 비교
            if (GameRule.CompareType(attackBlockType, monsterType.TypeIndex))
            {
                targetMonster = monsterObj;
                UnityEngine.Debug.Log("Attack successful! Monster defeated.");
                UnityEngine.Debug.Log(dataManager.GetMonsterTypeData(attackBlockType).TypeViewName + "으로 공격함");
                DebugBoxManager.Instance.Log("공격성공");
                int stageBlockIndex = StageManager.instance.ChangePosToKeyValue(playerPosition);
                Transform stageBlock = StageManager.instance.GetStageBlockPosition(stageBlockIndex);
                FXManager.Instance.PlayFXAtPosition(stageBlock, (FXType)(attackBlockType - 4));


                //DebugBoxManager.Instance.Log($"{dataManager.GetMonsterTypeData(attackBlockType).TypeViewName} Type Attack");
                // 승리 처리: 플레이어의 승리 메서드 호출
                //-> 플레이어의 공격 애니메이션과 이펙트가 끝나면
                Invoke(nameof(PlayerWin), 2);
            }
            else
            {
                UnityEngine.Debug.Log("Attack failed!");
                DebugBoxManager.Instance.Log("공격실패");
                UIManager.Instance.PrintUITextByTextIndex(410, false);
                // 패배 처리: 플레이어의 패배 메서드 호출
                UIManager.Instance.BlockContainerManager.SetXIcon(player.ForceGetCurrentIndex(), true);

                //player.EnableTypeMonsterPrefab(4);
                //Invoke(nameof(PlayerDefeat), 2);
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
        targetMonsterController.Hit();

        player.Win();


    }

    private void PlayerDefeat()
    {
        player.Defeat();
    }
}
