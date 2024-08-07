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
        //기획과 협의 필요
        //플레이어가 공격실패시 바로 오버인지
        // 적이 공격하고 나서 오버인지
        if(other.gameObject.GetComponent<MonsterController>() != null)
        {
            var enemy = other.gameObject.GetComponent<MonsterController>();
        }
    }


    protected void Attack()
    {
        //기획과 협의 필요
        //플레이어가 공격실패시 바로 오버인지
        // 적이 공격하고 나서 오버인지
    }
}
