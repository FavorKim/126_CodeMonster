using System.Collections.Generic;
using UnityEngine;

public class CodeBlockManager : MonoBehaviour
{
    public CodeBlockContainer container;
    public CodeBlockSequenceArea sequenceArea;
    private GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    public void ExecuteAll()
    {
        List<ICommand> commands = new List<ICommand>();

        foreach(Transform child in sequenceArea.transform)
        {
            CodeBlock block = child.GetComponent<CodeBlock>();
            if(block != null)
            {
                commands.Add(block);
            }
        }

        gameManager.ExecuteCommands(commands);
    }
}
