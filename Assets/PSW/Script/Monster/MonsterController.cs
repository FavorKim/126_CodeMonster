using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
   

    protected Animator _animator;
    private int hp;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void Start()
    {
        //hp=DataManagerTest.Instance.GetMonsterData(this.gameObject.name).HP;
        hp = 3;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   

    public void Attack()
    {
       //애니메이션 재생

        //플레이어 죽는 함수 호출 아니면 이벤트 호출

        
    }

    public virtual void Hit()
    {
        //애니메이션 재생

        hp--;

    }

    public bool CheckMonsterHPUnderZero()
    {
        if (hp <= 0)
        {
            return true;
        }

        return false;
    }

    public void Die()
    {
        //애니메이션 재생
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);//현재 실행중인 애니메이션 얻어옴

        float startTime = stateInfo.length + 0.5f;//애니메이션 재생시간

        Invoke(nameof(DisableMonster), startTime);

    }

    private void DisableMonster()
    {
        this.gameObject.SetActive(false);
    }

   
}
