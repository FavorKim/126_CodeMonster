using System.Xml.Linq;
using UnityEngine;

public class CodeBlock_Attack : CodeBlock
{
    public Element attackElement;  // 공격의 속성
    public Vector2Int partnerPosition;  // 파트너의 현재 위치
    public Enemy enemy;  // 공격 대상인 적

    // 파트너의 위치와 공격 대상 설정
    public void Initialize(Vector2Int partnerPosition, Enemy enemy)
    {
        this.partnerPosition = partnerPosition;
        this.enemy = enemy;
    }

    //public override void Execute(Player partner)
    //{
    //    // 파트너가 적을 공격
    //    partner.Attack(enemy, attackElement);
    //}
}
