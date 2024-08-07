using UnityEngine;

public class CodeBlockManager : MonoBehaviour
{
    public CodeBlockContainer container;
    public CodeBlockSequenceArea sequenceArea;

    public void ExecuteAll()
    {
        foreach (Transform child in sequenceArea.transform)
        {
            CodeBlock block = child.GetComponent<CodeBlock>();
            if(block != null)
            {
                block.Execute();
            }
        }
    }
}
