using Oculus.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomPokedObject : MonoBehaviour
{
    PokeInteractable poke;
    void Start()
    {
        poke = GetComponent<PokeInteractable>();
    }

    public event Action OnPoke;
    public event Action OnHover;
    public event Action OnPokeRelease;


    private void OnEnable()
    {
        poke.WhenStateChanged += OnPokeStateChanged;
    }

    private void OnDisable()
    {
        poke.WhenStateChanged -= OnPokeStateChanged;
    }

    private void OnPokeStateChanged(InteractableStateChangeArgs args)
    {
        // ��ũ���� ��
        if(args.NewState == InteractableState.Select)
        {
            OnPoke.Invoke();
        }

        // ȣ�� ���� ��
        else if (args.NewState == InteractableState.Hover)
        {
            OnHover.Invoke();
        }

        // ��ũ �������� ��
        else if(args.PreviousState == InteractableState.Hover && args.NewState != InteractableState.Select)
        {
            OnPokeRelease.Invoke();
        }
    }
}
