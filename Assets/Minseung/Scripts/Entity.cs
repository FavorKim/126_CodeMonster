using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;

public enum EntityType
{
    Player, Enemy
}

public class Entity : MonoBehaviour
{
    public Element element;
    
    public EntityType entityType;
    private StageManager stageManager;

    public bool IsValue;
    protected SharedBool _isPlayer;
    public BehaviorTree _tree;

    protected virtual void Awake()
    {
        _tree = GetComponent<BehaviorTree>();
        _isPlayer = (SharedBool)_tree.GetVariable("IsPlayer");
        _isPlayer.Value = IsValue;
        if (_isPlayer.Value == true)
        {
            _tree.StartWhenEnabled = true;
        }
    }

    //public void SetStageManager(StageManager manager)
    //{
    //    stageManager = manager;

    //    if (stageManager != null && entityType == EntityType.Player)
    //    {
    //        Initialize(stageManager.GetGrid(), stageManager.GetStartPosition());
    //    }
    //}

    //public void Initialize(int[,] grid, Vector2Int startPosition)
    //{
    //    position = startPosition;
    //    transform.position = new Vector3(position.x, transform.position.y, position.y);
    //}

    


    

    

    public void Attack(Entity targetEntity, Element attackElement)
    {
        //if (EntityRules.CanAttack(this, targetEntity, attackElement))
        //{
        //    targetEntity.Defeat();
        //    Debug.Log("Attack successful!");
        //}
        //else
        //{
        //    Defeat();
        //}
    }

    public void Defeat()
    {
        if (entityType == EntityType.Player)
        {
            Debug.Log(gameObject.name + " has been defeated!");
        }
        else
        {
            Debug.Log("Enemy defeated!");
            gameObject.SetActive(false);
        }
    }

    
}
