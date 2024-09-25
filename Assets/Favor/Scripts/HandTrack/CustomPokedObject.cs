using Oculus.Interaction;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CustomPokedObject : MonoBehaviour
{
    [SerializeField] PokeInteractable poke;
    [SerializeField] Image Img_Btn;
    [SerializeField] TMP_Text Txt_Btn;






    public UnityEvent OnPoke;
    public UnityEvent OnHover;
    public UnityEvent OnPokeRelease;


    private void Awake()
    {
        poke = GetComponent<PokeInteractable>();

        poke.WhenStateChanged += OnPokeStateChanged;
        Img_Btn = GetComponentInChildren<Image>();
        Txt_Btn = GetComponentInChildren<TMP_Text>();

    }


    private void OnApplicationQuit()
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
        if (args.NewState == InteractableState.Select)
        {
            OnPoke.Invoke();
        }

        // 호버 됐을 때
        else if (args.NewState == InteractableState.Hover)
        {
            OnHover.Invoke();
        }

        // 포크 해제됐을 때
        else if (args.PreviousState == InteractableState.Hover && (args.NewState != InteractableState.Select || args.NewState != InteractableState.Hover))
        {
            OnPokeRelease.Invoke();
        }
    }

    public void EnablePokeBtn()
    {
        if (Img_Btn == null)
            Img_Btn = GetComponentInChildren<Image>();
        Img_Btn.color = new Color(0, 0, 0, 0.8f);
        if (Txt_Btn == null)
            Txt_Btn = GetComponentInChildren<TMP_Text>();
        Txt_Btn.color = new Color(1, 1, 1, 1f);
        poke.enabled = true;
    }
    public void DisablePokeBtn()
    {
        if (Txt_Btn == null)
            Txt_Btn = GetComponentInChildren<TMP_Text>();
        Txt_Btn.color = new Color(1, 1, 1, 0.1f);
        if (Img_Btn == null)
            Img_Btn = GetComponentInChildren<Image>();
        Img_Btn.color = new Color(0, 0, 0, 0.1f);
        poke.enabled = false;

    }
}
