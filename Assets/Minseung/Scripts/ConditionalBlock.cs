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

    public int EvaluateCondiiton(Player player)
    {
        Vector2Int playerPosition = player.GetCurrentPosition();

        List<GameObject> bushMonsters = StageManager.Instance.GetBushMonsterWithPlayerPos(playerPosition);

        if(bushMonsters == null || bushMonsters.Count == 0)
        {
            return FalseBlockIndex;
        }

        foreach(var bushMonster in bushMonsters)
        {
            
        }
    }
}
