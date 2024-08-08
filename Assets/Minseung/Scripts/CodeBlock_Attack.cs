public class CodeBlock_Attack : CodeBlock
{
    public string target;

    public override void Execute(Player player)
    {
        player.Attack(target);
    }
}
