using BehaviorDesigner.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SharedAA : SharedVariable<CustomAA>
{
    public static implicit operator SharedAA(CustomAA value) { return new SharedAA { Value = value }; }
}
[System.Serializable]
public class CustomAA
{
    public test Value;
}