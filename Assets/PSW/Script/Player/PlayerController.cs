using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

public class PlayerController : Entity
{
    public Vector2Int position;
    public SharedVector2Int sharedPos;
    List<int> blockList = new List<int>();
    public void CheckBlock()
    {
        blockList = CodeBlockManager.Inst.ExecuteAll();
    }
    public void SetPlayerStartPos()
    {
        position = new Vector2Int(StageManager.Inst.GetPlayerSpawnPosX(), StageManager.Inst.GetPlayerSpawnPosY());
        SetSharedPlayerPos();
    }

    public void SetSharedPlayerPos()
    {
        sharedPos = (SharedVector2Int)_tree.GetVariable("playerPos");
        sharedPos.Value = position;
    }
    public void Move(Direction direction)
    {
       

        Vector2Int newPosition = position;

        switch (direction)
        {
            case Direction.Up:
                newPosition.y += 1;
                break;
            case Direction.Down:
                newPosition.y -= 1;
                break;
            case Direction.Left:
                newPosition.x -= 1;
                break;
            case Direction.Right:
                newPosition.x += 1;
                break;
        }


        if (EntityRules.CanMove(newPosition))
        {
            StartCoroutine(MoveToPosition(new Vector3(newPosition.x, transform.position.y, newPosition.y)));
            position = newPosition;
        }
    }
    private IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        float duration = 0.5f;
        float elapsed = 0f;
        Vector3 startingPosition = transform.position;

        while (elapsed < duration)
        {
            transform.position = Vector3.Lerp(startingPosition, targetPosition, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = targetPosition;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void Move()
    {
        //코드블럭 담당자와 협의 필요
    }
}
