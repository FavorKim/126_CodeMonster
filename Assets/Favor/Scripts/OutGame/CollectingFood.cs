using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectingFood : MonoBehaviour
{
    Vector3 originPos;
    [SerializeField]bool isCorrect;
    CustomGrabObject grab;

    void SetIsCorrect()
    {
        int curStageIndex = UIManager.Instance.SelectChapterNum + UIManager.Instance.SelectStageNum;
        curStageIndex /= 1000;
        string correct = string.Empty;
        switch (curStageIndex)
        {
            case 1:
                correct = "Banana";
                break;
            case 2:
                correct = "Fish";
                break;
            case 3:
                correct = "Tomato";
                break;
        }
        if (gameObject.name == correct)
        {
            isCorrect = true;
        }
        else
        {
            isCorrect = false;
        }
    }

    private void Start()
    {
        originPos = transform.position;
    }
    private void OnEnable()
    {
        SetIsCorrect();
        if(grab == null)
        {
            grab = GetComponent<CustomGrabObject>();
            grab.OnGrab.AddListener(OnGrabIsCorrect);
        }
    }
    private void OnApplicationQuit()
    {
        if (grab != null)
        {
            grab.OnGrab.RemoveAllListeners();
        }
    }

    public void OnGrabIsCorrect()
    {
        if (isCorrect)
        {
            UIManager.Instance.PrintOnGrabFood();
        }
        else
        {
            UIManager.Instance.PrintCollectStage();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FeedArea"))
        {
            if (isCorrect)
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
