using UnityEngine;
using System.Collections;

public enum EntityType
{
    Player, Enemy
}

public class Entity : MonoBehaviour
{
    public Element element;
    public Vector2Int position;
    public EntityType entityType;
    private StageManager stageManager;

    private void OnEnable()
    {
        //GameInitializer.OnStageManagerSet += SetStageManager;
    }

    private void OnDisable()
    {
       // GameInitializer.OnStageManagerSet -= SetStageManager;
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

    public void Move(Direction direction)
    {
        if (entityType != EntityType.Player) return;

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

    public void Attack(Entity targetEntity, Element attackElement)
    {
        if (EntityRules.CanAttack(this, targetEntity, attackElement))
        {
            targetEntity.Defeat();
            Debug.Log("Attack successful!");
        }
        else
        {
            Defeat();
        }
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
}
