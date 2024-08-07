using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryList : MonoBehaviour
{
    public GameObject CodeBlock;

    public void SetCodeBlock(string Type, string num)
    {
        Debug.Log(Type + num);
        CodeBlock.SetActive(true);  
    }
}
