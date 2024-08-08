using System;

public class CodeBlock_Condition : CodeBlock
{
    private Func<bool> condition;
    private CodeBlock blockToExecute;

    public CodeBlock_Condition(Func<bool> condition, CodeBlock blockToExecute)
    {
        this.condition = condition;
        this.blockToExecute = blockToExecute;
    }

    public override void Execute(Player player)
    {
        if (condition())
        {
            blockToExecute.Execute(player);
        }
    }
}
