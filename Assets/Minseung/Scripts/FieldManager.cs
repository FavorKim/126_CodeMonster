using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public enum SoltType
{
    ID,
    Name
}

public class FieldManager : MonoBehaviour
{
    [SerializeField] List<GameObject> monsterPrefabs;
    [SerializeField] float spawnRadius;
    [SerializeField] List<GameObject> fieldMonsterList = new List<GameObject>();
    [SerializeField] List<Vector3> targetPositions;


    private void Start()
    {
        //DataManagerTest.Instance.LoadedMonsterList
        //foreach (GameObject gobj in)

        foreach (GameObject prefab in monsterPrefabs)
        {
            Vector3 randomPosition = GetRandomSpawnPosition();
            GameObject monster = Instantiate(prefab, randomPosition, Quaternion.identity);

            RandomMove randomMove = monster.AddComponent<RandomMove>();
            Rigidbody rb = monster.AddComponent<Rigidbody>();
            rb.isKinematic = true;
            SphereCollider collider = monster.AddComponent<SphereCollider>();
            Grabbable grab = monster.AddComponent<Grabbable>();
            HandGrabInteractable hand = monster.AddComponent<HandGrabInteractable>();
            hand.InjectRigidbody(rb);
            CustomGrabObject customGrabObject = monster.AddComponent<CustomGrabObject>();
            customGrabObject.InitHandGrabInteractable(hand);
            randomMove.SetMoveSpeed(Random.Range(2f, 5f));

            fieldMonsterList.Add(monster);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            TeleportMonstersToTargetPositions();
            StopAllMonsters();
        }
    }
    public void OnPokeCharacterSelect()
    {
        TeleportMonstersToTargetPositions();
        StopAllMonsters();
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float randomX = Random.Range(-spawnRadius, spawnRadius);
        float randomZ = Random.Range(-spawnRadius, spawnRadius);
        return new Vector3(randomX, 0, randomZ);
    }

    public void TeleportMonstersToTargetPositions()
    {
        for (int i = 0; i < fieldMonsterList.Count; i++)
        {
            RandomMove moveComponent = fieldMonsterList[i].GetComponent<RandomMove>();
            if (moveComponent != null)
            {
                moveComponent.TeleportToPosition(targetPositions[i]);
            }
        }
    }

    public void StopAllMonsters()
    {
        foreach (GameObject monster in fieldMonsterList)
        {
            RandomMove moveComponenet = monster.GetComponent<RandomMove>();
            if (moveComponenet != null)
            {
                moveComponenet.StopMoving();
            }
        }
    }

    public void SetSpeedForAllMonsters(float speed)
    {
        foreach (GameObject monster in fieldMonsterList)
        {
            RandomMove moveComponent = monster.GetComponent<RandomMove>();
            if (moveComponent != null)
            {
                moveComponent.SetMoveSpeed(speed);
            }
        }
    }

    public List<GameObject> SoltingMonster(List<GameObject> monsters, SoltType type, bool UP = false)//기본이내림차순임
    {
        var sortedMonsters = new List<GameObject>();

        if (UP == false)
        {
            sortedMonsters = OrderByDescendingSoltingMonster(monsters, type);
        }
        else
        {
            sortedMonsters = OrderBySoltingMonster(monsters, type);
        }


        return sortedMonsters;
    }

    public List<GameObject> OrderBySoltingMonster(List<GameObject> monsters, SoltType type)//기본이내림차순임
    {
        var sortedMonsters = new List<GameObject>();



        switch (type)
        {
            case SoltType.Name:
                sortedMonsters = monsters.OrderBy(p => DataManagerTest.Instance.GetMonsterData(p.name).ViewName).ToList();
                break;
            case SoltType.ID:
                sortedMonsters = monsters.OrderBy(p => DataManagerTest.Instance.GetMonsterData(p.name).ID).ToList();
                break;
        }



        return sortedMonsters;
    }

    public List<GameObject> OrderByDescendingSoltingMonster(List<GameObject> monsters, SoltType type)//기본이내림차순임
    {
        var sortedMonsters = new List<GameObject>();


        switch (type)
        {
            case SoltType.Name:
                sortedMonsters = monsters.OrderByDescending(p => DataManagerTest.Instance.GetMonsterData(p.name).ViewName).ToList();
                break;
            case SoltType.ID:
                sortedMonsters = monsters.OrderByDescending(p => DataManagerTest.Instance.GetMonsterData(p.name).ID).ToList();
                break;
        }


        return sortedMonsters;
    }


}
