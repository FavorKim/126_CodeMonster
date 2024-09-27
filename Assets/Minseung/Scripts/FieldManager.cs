using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
//using static UnityEditor.Experimental.GraphView.GraphView;

public enum SoltType
{
    ID,
    Name
}

public class FieldManager : Singleton<FieldManager>
{
    [SerializeField] List<GameObject> monsterPrefabs;
    [SerializeField] float spawnRadius;
    [SerializeField] List<GameObject> fieldMonsterList = new List<GameObject>();
    [SerializeField] List<Transform> targetPositions;

  
    protected override void Start()
    {
        base.Start();
        OnStartOutGameUI();
    }

    public void OnStartOutGameUI()
    {
        foreach (GameObject prefab in monsterPrefabs)
        {
            InstantiateMonster(prefab);
        }
    }

    private void InstantiateMonster(GameObject prefab)
    {
        if (GameManager.Instance.CheckMonsterInPlayerList(prefab.name))
        {
            if (CheckIsContainedInFieldMonsterList(prefab.name) == false)
            {
                Vector3 randomPosition = GetRandomSpawnPosition();
                GameObject monster = Instantiate(prefab, randomPosition, Quaternion.identity);
                InitMonster(monster);
                monster.transform.localScale = Vector3.one;
            }
        }
    }

    private bool CheckIsContainedInFieldMonsterList(string name)
    {
        name = DataManagerTest.instance.RemoveTextAfterParenthesis(name);
        foreach(GameObject monster in fieldMonsterList)
        {
            string monName = DataManagerTest.Instance.RemoveTextAfterParenthesis(monster.name);
            if (monName == name)
                return true;
        }
        return false;
    }
 

    private void InitMonster(GameObject monster)
    {
        RandomMove randomMove = monster.AddComponent<RandomMove>();
        Rigidbody rb = monster.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
        SphereCollider collider = monster.AddComponent<SphereCollider>();
        collider.isTrigger = true;
        Grabbable grab = monster.AddComponent<Grabbable>();
        HandGrabInteractable hand = monster.AddComponent<HandGrabInteractable>();
        hand.InjectRigidbody(rb);
        hand.InjectOptionalPointableElement(grab);
        CustomGrabObject customGrabObject = monster.GetComponent<CustomGrabObject>();
        customGrabObject.InitHandGrabInteractable();
        randomMove.SetMoveSpeed(Random.Range(0.5f, 1.5f));
        monster.AddComponent<RectTransform>();
        GrabCharacter GC = monster.AddComponent<GrabCharacter>();
        GC.InitGrab();

        fieldMonsterList.Add(monster);
    }
    public void OnPokeCharacterSelect()
    {
        EnableAllMonsters();
        TeleportMonstersToTargetPositions();
        StopAllMonsters();
    }

    private Vector3 GetRandomSpawnPosition()
    {
        float randomX = Random.Range(-spawnRadius, spawnRadius);
        float randomZ = Random.Range(-spawnRadius, spawnRadius);
        return new Vector3(randomX+3, -1.0f, randomZ+3.0f);
    }

    public void TeleportMonstersToTargetPositions()
    {
        Vector3 scale = new Vector3(0.2f, 0.2f, 0.2f);
        for (int i = 0; i < fieldMonsterList.Count; i++)
        {
            RandomMove moveComponent = fieldMonsterList[i].GetComponent<RandomMove>();
            if (moveComponent != null)
            {
                moveComponent.TeleportToPosition(targetPositions[i]);
                moveComponent.transform.localScale = scale;
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
    public void MoveAllMonsters()
    {
        foreach (GameObject monster in fieldMonsterList)
        {
            RandomMove moveComponenet = monster.GetComponent<RandomMove>();
            if (moveComponenet != null)
            {
                moveComponenet.StartMoving();
            }
        }
    }

    public void DisableAllMonsters()
    {
        foreach (GameObject monster in fieldMonsterList)
        {
            monster.SetActive(false);
            monster.transform.SetParent(null);
        }
        StopAllMonsters();
        UIManager.Instance.SelectCharacterUIManager.RemoveAllMonsters();
    }
    public void EnableAllMonsters()
    {
        foreach (GameObject monster in fieldMonsterList)
        {
            monster.SetActive(true);
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
