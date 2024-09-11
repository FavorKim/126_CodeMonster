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
    void Start()
    {
        grab = GetComponent<HandGrabInteractable>();
    }

    public UnityEvent OnGrab;
    public UnityEvent OnRelease;

    private void OnEnable()
    {
        grab.WhenStateChanged += OnGrabStateChanged;
    }
  

    private void OnDisable()
    {
        grab.WhenStateChanged -= OnGrabStateChanged;
    }

    private void OnGrabStateChanged(InteractableStateChangeArgs args)
    {
        // Grab됐을 때
        if (args.NewState == InteractableState.Select)
        {
            OnGrab.Invoke();
        }

        // Release 됐을 때
        else if (args.PreviousState == InteractableState.Select && args.NewState != InteractableState.Select)
        {
            OnRelease.Invoke();
        }
    }
}
