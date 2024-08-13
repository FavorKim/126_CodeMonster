using Oculus.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomGrabObject : MonoBehaviour
{
    GrabInteractable grab;
    void Start()
    {
        grab = GetComponent<GrabInteractable>();
    }

    public event Action OnGrab;
    public event Action OnRelease;

    private void OnEnable()
    {
        grab.WhenStateChanged += OnGrabStateChanged;
        OnGrab += OnGrab_ChangeColor;
        OnRelease += OnRelease_ChangeColor;
    }

    private void OnDisable()
    {
        OnRelease -= OnRelease_ChangeColor;
        OnGrab -= OnGrab_ChangeColor;
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

    private void OnGrab_ChangeColor()
    {
        Material mat = GetComponent<MeshRenderer>().material;
        mat = new Material(mat);
        mat.color = Color.red;
    }

    private void OnRelease_ChangeColor()
    {
        Material mat = GetComponent<MeshRenderer>().material;
        mat = new Material(mat);
        mat.color = Color.white;
    }
}
