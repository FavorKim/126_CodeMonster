using System.Collections.Generic;
using UnityEngine;

public class CodeBlock_Repeat : CodeBlock
{
    private int repeatCount;
    private List<CodeBlock> blocksToRepeat;

    // 플레이어가 반복 횟수와 반복할 코드 블록들을 설정하는 메서드
    public void SetRepeatParameters(int count, List<CodeBlock> blocks)
    {
        repeatCount = count;
        blocksToRepeat = blocks;
    }

    // 파트너에 명령을 전달하여 반복 실행하는 메서드
    //public override void Execute(Player partner)
    //{
    //    for (int i = 0; i < repeatCount; i++)
    //    {
    //        foreach (var block in blocksToRepeat)
    //        {
    //            block.Execute(partner); // 파트너에게 명령을 전달
    //        }
    //    }
    //}
}
