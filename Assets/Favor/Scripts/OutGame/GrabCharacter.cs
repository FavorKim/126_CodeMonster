using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabCharacter : MonoBehaviour
{
    CustomGrabObject grab;

    bool isTriggered = false;


    private void OnEnable()
    {
        
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;

        SphereCollider col = GetComponent<SphereCollider>();
        col.isTrigger = true;

        if(grab == null)
        {
            grab = GetComponent<CustomGrabObject>();
        }
        if (grab != null)
        {
            grab.OnGrab.AddListener(UnselectMonster);
            grab.OnRelease.AddListener(SelectMonster);
        }
    }
    

    private void OnDisable()
    {
        if(grab!= null)
        {
            grab.OnRelease?.RemoveListener(SelectMonster);
            grab.OnGrab.RemoveListener(UnselectMonster);
        }
    }
    public void InitGrab()
    {
        if (grab == null)
        {
            grab = GetComponent<CustomGrabObject>();
            grab.InitOnStateChanged();
            grab.OnGrab.AddListener(UnselectMonster);
            grab.OnRelease.AddListener(SelectMonster);
        }
    }

    private void SelectMonster()
    {
        if (isTriggered)
            UIManager.Instance.SelectCharacterUIManager.AddMonster(this.gameObject);
    }

    private void UnselectMonster()
    {
        UIManager.Instance.SelectCharacterUIManager.RemoveMonster(this.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SelectCharacterUI"))
        {
            if(other.transform.parent.TryGetComponent(out SelectCharacterUIManager charUI))
            {
                isTriggered = true;
            }
            else
            {
                Debug.LogError("SelectCharacterUI NULL!");
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("SelectCharacterUI"))
        {
            if (other.transform.parent.TryGetComponent(out SelectCharacterUIManager charUI))
            {
                isTriggered = false;
            }
        }
    }
}
