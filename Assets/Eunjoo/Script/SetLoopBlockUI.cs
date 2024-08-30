using EnumTypes;
using EventLibrary;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLoopBlockUI : MonoBehaviour
{
    [SerializeField] GameObject LoopBlockListBox;
    [SerializeField] GameObject LoopBlockListImage;

    public void EnableLoopBlockImage()
    {
        LoopBlockListImage.SetActive(true);
    }

    public void AddBlock(GameObject newBlock)
    {
        newBlock.transform.SetParent(LoopBlockListBox.transform, false);
        newBlock.transform.localScale = Vector3.one;
    }
}
