using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
    private string _monsterName;
    private int _monsterType;
    private int _enemyType;

    protected Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void Start()
    {
        _monsterName = this.transform.gameObject.name;
        _monsterType = DataManagerTest.Inst.GetMonsterData(_monsterName).TypeIndex;
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

    protected virtual void Hit()
    {
        //�ִϸ��̼� ���

        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

        float startTime = stateInfo.length + 0.5f;

        //�÷��̾�� ������ ���ʹ� �����
        Invoke(nameof(Die), startTime);
    }

    protected void Die()
    {
        this.gameObject.SetActive(false);
    }
}
