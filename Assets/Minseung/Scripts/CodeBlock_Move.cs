using UnityEngine;

// 이동 방향을 나타내는 열거형
public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

// CodeBlock_Move 클래스
public class CodeBlock_Move : CodeBlock
{
    public Direction moveDirection;  // 이동 방향

    //public override void Execute(Player player)
    //{
    //    player.Move(moveDirection);  // 이동 명령 전달
    //}
}
