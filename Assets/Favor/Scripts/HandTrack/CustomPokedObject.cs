using Oculus.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CustomPokedObject : MonoBehaviour
{
    [SerializeField]PokeInteractable poke;
     
    void Start()
    {
        poke = GetComponent<PokeInteractable>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.LogWarning($"{name} : OnPoke");
        }
    }

    public Action OnPoke;
    public UnityEvent OnHover;
    public UnityEvent OnPokeRelease;


    private void OnEnable()
    {
        poke.WhenStateChanged += OnPokeStateChanged;
    }

    private void OnDisable()
    {
        poke.WhenStateChanged -= OnPokeStateChanged;
    }

    void OnPoke_Debug()
    {
        Debug.LogWarning("OnPokeDebug");
    }

    private void OnPokeStateChanged(InteractableStateChangeArgs args)
    {
        // 포크됐을 때
        if(args.NewState == InteractableState.Select)
        {
            OnPoke.Invoke();
        }

        // 호버 됐을 때
        else if (args.NewState == InteractableState.Hover)
        {
            OnHover.Invoke();
        }

        // 포크 해제됐을 때
        else if(args.PreviousState == InteractableState.Hover && args.NewState != InteractableState.Select)
        {
            OnPokeRelease.Invoke();
        }
    }
}
