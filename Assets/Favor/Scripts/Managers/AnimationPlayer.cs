using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPlayer : MonoBehaviour
{
    public static void SetTrigger(string pramName, GameObject dest)
    {
        dest.GetComponentInChildren<Animator>().SetTrigger(pramName);
    }
}
