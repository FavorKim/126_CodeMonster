using System.Xml.Linq;
using UnityEngine;

public class CodeBlock_Attack : CodeBlock
{
    public Element attackElement;  // ������ �Ӽ�
    public Vector2Int partnerPosition;  // ��Ʈ���� ���� ��ġ
    public Enemy enemy;  // ���� ����� ��

    // ��Ʈ���� ��ġ�� ���� ��� ����
    public void Initialize(Vector2Int partnerPosition, Enemy enemy)
    {
        this.partnerPosition = partnerPosition;
        this.enemy = enemy;
    }

    //public override void Execute(Player partner)
    //{
    //    // ��Ʈ�ʰ� ���� ����
    //    partner.Attack(enemy, attackElement);
    //}
}
