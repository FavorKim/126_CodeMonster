using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CustomGrabObject : MonoBehaviour
{
    [SerializeField] HandGrabInteractable grab;
    bool isGrabbing;


    public UnityEvent OnGrab;
    public UnityEvent OnRelease;

    private void OnEnable()
    {
        if (grab == null)
            grab = GetComponent<HandGrabInteractable>();
        if (grab != null)
        {
            grab.WhenStateChanged += OnGrabStateChanged;
        }
    }

    public void InitOnStateChanged()
    {
        if (grab == null)
        {
            grab = GetComponent<HandGrabInteractable>();
            grab.WhenStateChanged += OnGrabStateChanged;
        }
    }


    private void OnDisable()
    {
        if (grab != null)
            grab.WhenStateChanged -= OnGrabStateChanged;
    }

    public void InitHandGrabInteractable()
    {
        grab = GetComponent<HandGrabInteractable>();
        this.grab.WhenStateChanged += OnGrabStateChanged;
    }

    private void OnGrabStateChanged(InteractableStateChangeArgs args)
    {
        // Grab됐을 때
        if (args.NewState == InteractableState.Select)
        {
            if(OnGrab == null)
            {
                DebugBoxManager.Instance.Log($"{gameObject.name}의 On Grab이 NULL");
            }
            OnGrab?.Invoke();
        }

        // Release 됐을 때
        else if (args.PreviousState == InteractableState.Select && args.NewState != InteractableState.Select)
        {
            if (OnRelease == null)
            {
                DebugBoxManager.Instance.Log($"{gameObject.name}의 OnRelease가 NULL");
            }
            OnRelease?.Invoke();
        }
    }
}
