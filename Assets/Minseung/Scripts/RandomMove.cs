using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3.0f;
    [SerializeField] private float changeDirectionTime = 2.0f;
    private Vector3 moveDirection;
    private float timeSinceLastDirectionChange;
    private bool isMoving = true;
    [SerializeField] float distance;
    private void Start()
    {
        SetRandomDirection();
    }

    private void Update()
    {
        if (isMoving)
        {
            distance = Vector3.Distance(GameManager.Instance.PlayerPrefab.transform.position, transform.position);
            if (distance > 3 || distance < -3)
            {
                Vector3 tempdir = (GameManager.Instance.PlayerPrefab.transform.position - transform.position).normalized;
                moveDirection = new Vector3(tempdir.x, 0, tempdir.z);
            }
            else
            {
                timeSinceLastDirectionChange += Time.deltaTime;
                if (timeSinceLastDirectionChange >= changeDirectionTime)
                {
                    SetRandomDirection();
                    timeSinceLastDirectionChange = 0f;
                }
            }
            transform.Translate(moveDirection * moveSpeed * Time.deltaTime);
        }
    }

    private void SetRandomDirection()
    {
        float randomX = Random.Range(-1f, 1f);
        float randomZ = Random.Range(-1f, 1f);
        moveDirection = new Vector3(randomX, 0, randomZ).normalized;
    }

    public void StartMoving()
    {
        isMoving = true;
        SetRandomDirection();
    }

    public void StopMoving()
    {
        isMoving = false;
    }

    public void SetMoveSpeed(float speed)
    {
        moveSpeed = speed;
    }

    public void TeleportToPosition(Transform target)
    {
        transform.position = target.position;
        transform.localRotation = Quaternion.identity;
    }
}
