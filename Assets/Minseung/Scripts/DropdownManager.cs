using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DropdownManager : Singleton<DropdownManager>
{
    [SerializeField] private TMP_Dropdown loopDropdown;
    [SerializeField] private TMP_Dropdown conditionDropdown;

    public int GetSelectedLoopCount()
    {
        return loopDropdown.value + 2;
    }

    public int GetSelectedConditionAttribute()
    {
        switch (conditionDropdown.value)
        {
            case 0: return 5;
            case 1: return 6;
            case 2: return 7;
            default: return 0;
        }
    }

    public void InitializeDropdowns()
    {
        loopDropdown.ClearOptions();
        List<string> loopOptions = new List<string> { "2회", "3회", "4회", "5회", "6회" };
        loopDropdown.AddOptions(loopOptions);

        conditionDropdown.ClearOptions();
        List<string> conditionOptions = new List<string> { "풀", "물", "불" };
        conditionDropdown.AddOptions(conditionOptions);
    }
}
