using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : MonoBehaviour
{
   

    protected Animator _animator;
    [SerializeField]private int hp;
    int HP
    {
        get { return hp; }
        set
        {
            hp = value;
            if (IsMonsterHPUnderZero())
                Die();
        }
    }
    private int maxHP;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    void Start()
    {
        maxHP = hp;
        InteractEventManager.Instance.RegistOnPokeBtn(PokeButton.RESET, OnReset_ResetHP);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

   

    public void Attack()
    {
        //애니메이션 재생

        //플레이어 죽는 함수 호출 아니면 이벤트 호출

        DebugBoxManager.Instance.Log("적 몬스터 반격!");
    }

    public virtual void Hit()
    {
        //애니메이션 재생

        HP--;
        DebugBoxManager.Instance.Log($"적 몬스터 피격! 적 몬스터 남은 체력 : {hp}");

    }

    public bool IsMonsterHPUnderZero()
    {
        if (hp <= 0)
        {
            return true;
        }

        return false;
    }

    public void Die()
    {
        /*
        //애니메이션 재생
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);//현재 실행중인 애니메이션 얻어옴

        float startTime = stateInfo.length + 0.5f;//애니메이션 재생시간
        

        Invoke(nameof(DisableMonster), startTime);
        */

        DebugBoxManager.Instance.Log("적 몬스터 기절!");
        DisableMonster();
    }

    private void DisableMonster()
    {
        this.gameObject.SetActive(false);
    }

    private void OnReset_ResetHP()
    {
        hp = maxHP;
    }

   
}
