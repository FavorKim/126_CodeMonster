using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectingFood : MonoBehaviour
{
    Vector3 originPos;

    private void Start()
    {
        originPos = transform.position;
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
        transform.position = originPos;
        transform.rotation = Quaternion.identity;
    }
}
