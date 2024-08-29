using EnumTypes;
using EventLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLoopBlockUI : MonoBehaviour
{
    public void AddBlock(GameObject newBlock)
    {
        newBlock.transform.SetParent(this.transform, false);
    }
}
