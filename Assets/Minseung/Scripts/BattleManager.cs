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
        player = stageManager.GetPlayer();
        // 플레이어의 위치와 적의 위치를 비교하여 같은 위치에 있는지 확인

        if (stageManager.CheckMonsterAndPlayerPos(playerPosition))
        {

            GameObject monsterObj;
            // 부시라면
            if (stageManager.CheckBushAndPlayerPos(playerPosition))
            {
                int rand = UnityEngine.Random.Range(0, 1);
                // 조건문을 사용했을 경우
                if (attackBlockType == 8)
                {
                    // 랜덤으로 몬스터 생성
                    monsterObj = stageManager.GetMonsterInBush(playerPosition, rand);
                    GameObject pref = MonsterObjPoolManger.Instance.GetMonsterPrefab(monsterObj.name);
                    pref.SetActive(true);
                    GameObject bush = stageManager.GetMonsterWithPlayerPos(playerPosition).transform.GetChild(0).gameObject;
                    pref.transform.position = bush.transform.position;
                    pref.transform.position = new Vector3(pref.transform.position.x, player.transform.position.y, pref.transform.position.z);
                    bush.SetActive(false);
                    bush.transform.parent.gameObject.SetActive(false);
                    // 리셋 시 초기화를 위한 이벤트 구독
                    InteractEventManager.Instance.RegistOnPokeBtn(PokeButton.PAUSE, () => { pref.SetActive(false); bush.gameObject.SetActive(true); });
                    InteractEventManager.Instance.RegistOnPokeBtn(PokeButton.RESTART, () => { pref.SetActive(false); bush.gameObject.SetActive(true); });
                    InteractEventManager.Instance.RegistOnPokeBtn(PokeButton.BACTTOMAIN, () => { pref.SetActive(false); bush.gameObject.SetActive(true); });

                }
                else
                {
                    UIManager.Instance.BlockContainerManager.SetXIcon(player.CurrentIndex, true);

                    //monsterObj = null;
                    return;
                }
            }
            else
            {
                monsterObj = stageManager.GetMonsterWithPlayerPos(playerPosition);
            }

            string monsterName = monsterObj.name;
            Monster monster = DataManagerTest.Instance.GetMonsterData(monsterName);
            if(monster == null)
            {
            }
            MonsterType monsterType = DataManagerTest.Instance.GetMonsterTypeData(monster.TypeIndex + 4);
            if (monsterObj == null)
            {
            }
            targetMonsterController = monsterObj.GetComponent<MonsterController>();
            if (targetMonsterController == null)
            {
            }


            // 조건문을 사용할 경우 등장한 조건문에 맞는 공격블록으로 인덱스 변경
            if (attackBlockType == 8)
            {
                if (player.isLoop == false)
                {
                    attackBlockType = UIManager.Instance.BlockContainerManager.GetConditionBlockByIndex(player.CurrentIndex).EvaluateCondition(monster);
                }
                else
                {
                    int curIndex = player.ForceGetCurrentIndex();
                    SetLoopBlockUI loop = UIManager.Instance.BlockContainerManager.GetLoopBlockByIndex(curIndex);
                    if(loop == null)
                    {
                    }
                    ConditionBlock cond = loop.GetConditionBlockByIndex(player.CurrentIndex);
                    if(cond == null)
                    {
                    }
                    attackBlockType = cond.EvaluateCondition(monster);
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

                int stageBlockIndex = StageManager.instance.ChangePosToKeyValue(playerPosition);
                Transform stageBlock = StageManager.instance.GetStageBlockPosition(stageBlockIndex);
                FXManager.Instance.PlayFXAtPosition(stageBlock, (FXType)(attackBlockType - 4));
                targetMonsterController.Hit();
                AnimationPlayer.SetTrigger("Hit" ,targetMonster);

                // 승리 처리: 플레이어의 승리 메서드 호출
                //-> 플레이어의 공격 애니메이션과 이펙트가 끝나면
                Invoke(nameof(PlayerWin), 2);
            }
            else
            {
                UnityEngine.Debug.Log("Attack failed!");

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


    }

    private void PlayerDefeat()
    {
        player.Defeat();
    }
}
