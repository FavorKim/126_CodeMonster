using System.Collections.Generic;
using UnityEngine;

public class CodeBlock_Repeat : CodeBlock
{
    private int repeatCount;
    private List<CodeBlock> blocksToRepeat;

    // �÷��̾ �ݺ� Ƚ���� �ݺ��� �ڵ� ��ϵ��� �����ϴ� �޼���
    public void SetRepeatParameters(int count, List<CodeBlock> blocks)
    {
        repeatCount = count;
        blocksToRepeat = blocks;
    }

    // ��Ʈ�ʿ� ����� �����Ͽ� �ݺ� �����ϴ� �޼���
    //public override void Execute(Player partner)
    //{
    //    for (int i = 0; i < repeatCount; i++)
    //    {
    //        foreach (var block in blocksToRepeat)
    //        {
    //            block.Execute(partner); // ��Ʈ�ʿ��� ����� ����
    //        }
    //    }
    //}
}
