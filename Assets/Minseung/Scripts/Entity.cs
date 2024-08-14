using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour
{
    protected virtual void Move(Vector2Int newPosition) { }
    protected virtual void Attack() { }
}
