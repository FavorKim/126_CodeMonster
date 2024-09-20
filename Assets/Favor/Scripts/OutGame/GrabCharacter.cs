using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabCharacter : MonoBehaviour
{
    CustomGrabObject grab;

    SelectCharacterUIManager characterUI;

    private void OnEnable()
    {
        if(grab == null)
        {
            grab = GetComponent<CustomGrabObject>();
        }
        if (grab != null)
        {
            grab.OnRelease?.AddListener(SelectMonster);
        }
    }

    private void OnDisable()
    {
        if(grab!= null)
        {
            grab.OnRelease?.RemoveListener(SelectMonster);
        }
    }
    public void InitGrab(CustomGrabObject grab)
    {
        this.grab = grab;
    }

    private void SelectMonster()
    {
        if (characterUI != null) 
        {
            characterUI.AddMonster(this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SelectCharacterUI"))
        {
            if(other.transform.parent.TryGetComponent(out SelectCharacterUIManager charUI))
            {
                characterUI = charUI;
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
                characterUI = null;
            }
            
        }
    }
}
