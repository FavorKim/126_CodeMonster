using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager
{
    //플레이어에서 넘어온 공격명령에 대한 배틀페이즈 실행.

    public static BattleManager Instance { get; private set; }

    public void BattlePhase()
    {
        //if (GameRule.ComparePosition())
        //{
        //    if (GameRule.CompareType())
        //    {
        //        Debug.Log("공격 성공");
        //    }
        //    else
        //    {
        //        Debug.Log("공격 실패");
        //    }
        //}
        //else
        //{
        //    Debug.Log("공격 실패");
        //}
    }
}
