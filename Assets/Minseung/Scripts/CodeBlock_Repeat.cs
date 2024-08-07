using System.Collections.Generic;

public class CodeBlock_Repeat : CodeBlock
{
    private int repeatCount;
    private List<CodeBlock> blocksToRepeat;

    public CodeBlock_Repeat(int repeatCount, List<CodeBlock> blocksToRepeat)
    {
        this.repeatCount = repeatCount;
        this.blocksToRepeat = blocksToRepeat;
    }

    public override void Execute()
    {
        for (int i = 0; i < repeatCount; i++)
        {
            foreach (var block in blocksToRepeat)
            {
                block.Execute();
            }
        }
    }
}
