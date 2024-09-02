using System.Collections.Generic;
using UnityEngine;

public class ConditionalBlock : MonoBehaviour
{
    public int TrueBlockIndex { get; private set; }
    public int FalseBlockIndex { get; private set; }

    private int selectedAttribute;
    private DropdownManager dropdownManager;

    private void Start()
    {
        dropdownManager = FindAnyObjectByType<DropdownManager>();
        selectedAttribute = dropdownManager.GetSelectedConditionAttribute();
    }

    public void Initialize(int trueBlockIndex, int falseBlockIndex)
    {
        this.TrueBlockIndex = trueBlockIndex;
        this.FalseBlockIndex = falseBlockIndex;
    }

    public int EvaluateCondition()
    {
        Player player = StageManager.Instance.GetPlayer();

        Vector2Int playerPosition = player.GetCurrentPosition();

        int randomIndex = Random.Range(0, 1);

        GameObject bushMonster = StageManager.Instance.GetMonsterInBush(playerPosition, randomIndex);

        if(bushMonster != null )
        {
            string bushMonsterName = bushMonster.name;

            Monster monsterData = DataManagerTest.Instance.GetMonsterData(bushMonsterName);

            if(monsterData != null && monsterData.TypeIndex == selectedAttribute)
            {
                return TrueBlockIndex;
            }
        }

        return FalseBlockIndex;
    }
}
