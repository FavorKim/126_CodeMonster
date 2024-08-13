using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGrabObject : MonoBehaviour
{
    [SerializeField] HandGrabInteractable grab;
    void Start()
    {
        grab = GetComponent<HandGrabInteractable>();
    }

    public event Action OnGrab;
    public event Action OnRelease;

    private void OnEnable()
    {
        grab.WhenStateChanged += OnGrabStateChanged;
        OnGrab += OnGrab_ChangeSize;
        OnRelease += OnRelease_ChangeSize;
    }

    private void OnDisable()
    {
        OnRelease -= OnRelease_ChangeSize;
        OnGrab -= OnGrab_ChangeSize;
        grab.WhenStateChanged -= OnGrabStateChanged;
    }

    private void OnGrabStateChanged(InteractableStateChangeArgs args)
    {
        // GrabµÆÀ» ¶§
        if (args.NewState == InteractableState.Select)
        {
            OnGrab.Invoke();
        }

        // Release µÆÀ» ¶§
        else if (args.PreviousState == InteractableState.Select && args.NewState != InteractableState.Select)
        {
            OnRelease.Invoke();
        }
    }

    private void OnGrab_ChangeSize()
    {
        transform.localScale = transform.localScale * 1.5f;
    }

    private void OnRelease_ChangeSize()
    {
        transform.localScale = transform.localScale / 1.5f;
    }
}
