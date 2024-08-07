using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    private string _monsterName;
    private int _monsterType;
    private int _enemyType;


    private void Awake()
    {
        
    }

    void Start()
    {
        _monsterName = this.transform.gameObject.name;
        _monsterType = DataManger.Inst.GetMonsterData(_monsterName).TypeIndex;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {

        if(other.gameObject.GetComponent<MonsterController>() != null)
        {
            var enemy = other.gameObject.GetComponent<MonsterController>();
            _enemyType = enemy._enemyType;
        }
    }


    protected void Attack()
    {
       
    }
}
