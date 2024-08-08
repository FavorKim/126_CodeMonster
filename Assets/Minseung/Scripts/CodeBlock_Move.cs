using UnityEngine;

public class CodeBlock_Move : CodeBlock
{
    public Vector3 direction;
    public float distance;

    public override void Execute(Player player)
    {
        player.Move(direction, distance);
    }
}
