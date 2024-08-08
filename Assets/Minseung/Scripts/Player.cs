using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public void Move(Vector3 direction, float distance)
    {
        transform.Translate(direction.normalized * distance);
    }

    public void Attack(string target)
    {
        Debug.Log(gameObject.name + " attacks " + target);
    }
}
