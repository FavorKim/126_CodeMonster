using System;
using UnityEngine;

public class CodeBlock_Condition : CodeBlock
{
    private Func<bool> condition;
    private CodeBlock blockToExecute;

    // �÷��̾�(�����)�� ���ǰ� ������ �ڵ� ����� �����ϴ� �޼���
    public void SetCondition(Func<bool> condition, CodeBlock block)
    {
        this.condition = condition;
        this.blockToExecute = block;
    }

    // ������ ���ϰ�, ���� ��� ��Ʈ�ʰ� �ൿ�� �����ϵ��� �ϴ� �޼���
    public override void Execute(Player partner)
    {
        if (condition != null && condition())
        {
            blockToExecute.Execute(partner); // ������ ���� ��� ��Ʈ�ʰ� �ൿ�� ����
        }
    }
}
