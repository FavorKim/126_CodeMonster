using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPlayer : MonoBehaviour
{
    public static void SetTrigger(string pramName, GameObject dest)
    {
        dest.GetComponentInChildren<Animator>().SetTrigger(pramName);
    }
    public static void SetBool(string pramName, GameObject dest, bool isTrue)
    {
        dest.GetComponentInChildren<Animator>().SetBool(pramName, isTrue);
    }
}
