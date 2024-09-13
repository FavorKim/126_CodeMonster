using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldManager : MonoBehaviour
{
    [SerializeField] List<GameObject> monsterPrefabs;
    [SerializeField] float spawnRadius;
    [SerializeField] List<GameObject> fieldMonsterList = new List<GameObject>();
    [SerializeField] List<Vector3> targetPositions;


    private void Start()
    {
        foreach(GameObject prefab in monsterPrefabs)
        {
            Vector3 randomPosition = GetRandomSpawnPosition();
            GameObject monster = Instantiate(prefab, randomPosition, Quaternion.identity);

            RandomMove randomMove = monster.AddComponent<RandomMove>();
            randomMove.SetMoveSpeed(Random.Range(2f, 5f));

            fieldMonsterList.Add(monster);
        }
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float randomX = Random.Range(-spawnRadius, spawnRadius);
        float randomZ = Random.Range(-spawnRadius, spawnRadius);
        return new Vector3(randomX, 0, randomZ);
    }

    public void TeleportMonstersToTargetPositions()
    {
        for(int i = 0; i < fieldMonsterList.Count; i++)
        {
            RandomMove moveComponent = fieldMonsterList[i].GetComponent<RandomMove>();
            if(moveComponent != null)
            {
                moveComponent.TeleportToPosition(targetPositions[i]);
            }
        }
    }

    public void StopAllMonsters()
    {
        foreach(GameObject monster in fieldMonsterList)
        {
            RandomMove moveComponenet = monster.GetComponent<RandomMove>();
            if(moveComponenet != null)
            {
                moveComponenet.StopMoving();
            }
        }
    }

    public void SetSpeedForAllMonsters(float speed)
    {
        foreach(GameObject monster in fieldMonsterList)
        {
            RandomMove moveComponent = monster.GetComponent<RandomMove>();
            if(moveComponent != null)
            {
                moveComponent.SetMoveSpeed(speed);
            }
        }
    }
}
