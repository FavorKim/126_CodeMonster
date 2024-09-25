using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectingFood : MonoBehaviour
{
    Transform originPos;

    private void Start()
    {
        transform.position = originPos.position;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FeedArea"))
        {
            CollectManager.Instance.IsCapturing = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("FeedArea"))
        {
            CollectManager.Instance.IsCapturing = false;
        }
    }
    public void ResetPosition()
    {
        transform.position = originPos.position;
    }
}
