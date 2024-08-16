using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour 
{
    //�÷��̾�� �Ѿ�� ���ݸ�ɿ� ���� ��Ʋ������ ����.

    public static BattleManager Instance { get; private set; }

    private DataManagerTest dataManager;
    private Player player;
    private StageManager stageManager;

    private void Awake()
    {
        dataManager = DataManagerTest.Inst;
        stageManager = StageManager.Instance;
        player = stageManager.GetPlayer();
    }

    public void BattlePhase(Vector2Int playerPosition, int attackBlockType)
    {
        // ���� ��ġ�� ��� ���� ������ �ִ� ���� ����Ʈ�� ������
        List<Vector2Int> monsterPositions = stageManager.GetMonsterSpawnList();

        // �÷��̾��� ��ġ�� ���� ��ġ�� ���Ͽ� ���� ��ġ�� �ִ��� Ȯ��
        for (int i = 0; i < monsterPositions.Count; i++)
        {
            if (GameRule.ComparePosition(playerPosition, monsterPositions[i]))
            {
                string monsterName = stageManager.GetMonsterNameAtIndex(i);
                Monster monster = DataManagerTest.Inst.GetMonsterData(monsterName);
                MonsterType monsterType = DataManagerTest.Inst.GetMonsterTypeData(monster.TypeIndex);

                // ���� ����� Ÿ�԰� ������ ���� ��
                if (GameRule.CompareType(attackBlockType, monsterType.Weakness))
                {
                    Debug.Log("Attack successful! Monster defeated.");
                    // �¸� ó��: �÷��̾��� �¸� �޼��� ȣ��
                    StageManager.Instance.GetPlayer().Win();
                }
                else
                {
                    Debug.Log("Attack failed! Player defeated.");
                    // �й� ó��: �÷��̾��� �й� �޼��� ȣ��
                    StageManager.Instance.GetPlayer().Defeat();
                }
                return;
            }
        }

        // ���� ��ġ�� ��ġ���� �ʴ� ���, ������ ������ ������ �����Ͽ� �й� ó��
        Debug.Log("Attack missed! Player defeated.");
        StageManager.Instance.GetPlayer().Defeat();
    }
}
