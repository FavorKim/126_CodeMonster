using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPlayer : MonoBehaviour
{
    public static void SetTrigger(string paramName, GameObject dest)
    {
        Animator anim = dest.GetComponentInChildren<Animator>();
        if (anim != null)
            anim.SetTrigger(paramName);
    }
    public static void SetBool(string paramName, GameObject dest, bool isTrue)
    {
        Animator anim = dest.GetComponentInChildren<Animator>();
        if (anim != null)
            anim.SetBool(paramName,isTrue);
    }
}
