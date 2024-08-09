using UnityEngine;
using System.Collections.Generic;

public class CodeBlockSequenceArea : MonoBehaviour
{
    public CodeBlockContainer container;

    public void OnDrop(GameObject droppedObject)
    {
        CodeBlock block = droppedObject.GetComponent<CodeBlock>();

        if (block != null)
        {
            container.AddBlock(block);
            droppedObject.transform.SetParent(this.transform);
            UpdateContainerOrder();
        }
    }

    public void UpdateContainerOrder()
    {
        List<CodeBlock> newOrder = new List<CodeBlock>();

        foreach (Transform child in transform)
        {
            CodeBlock block = child.GetComponent<CodeBlock>();
            if (block != null)
            {
                newOrder.Add(block);
            }
        }

        container.UpdateBlockOrder(newOrder);
    }

    private void LateUpdate()
    {
        UpdateContainerOrder();
    }
}
