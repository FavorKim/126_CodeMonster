using UnityEngine;

// �̵� ������ ��Ÿ���� ������
public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

// CodeBlock_Move Ŭ����
public class CodeBlock_Move : CodeBlock
{
    public Direction moveDirection;  // �̵� ����

    //public override void Execute(Player player)
    //{
    //    player.Move(moveDirection);  // �̵� ��� ����
    //}
}
