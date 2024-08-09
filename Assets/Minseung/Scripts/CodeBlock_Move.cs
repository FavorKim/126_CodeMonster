using UnityEngine;

public class CodeBlock_Move : CodeBlock
{
    public Vector3 direction;
    public float distance;

    public override void Execute(Player partner)
    {
        partner.Move(direction, distance);
    }
}
