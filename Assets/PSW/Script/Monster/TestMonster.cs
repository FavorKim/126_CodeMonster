using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

public class TestMonster : MonoBehaviour
{
    public bool IsValue;
    SharedBool _isPlayer;
    public BehaviorTree _tree;

    void Awake()
    {
        _tree = GetComponent<BehaviorTree>();
        _isPlayer = (SharedBool)_tree.GetVariable("IsPlayer");
        _isPlayer.Value = IsValue;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
