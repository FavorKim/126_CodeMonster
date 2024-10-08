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
        if (!TryGetComponent(out GrabCharacter character))
        {
            InteractEventManager.Instance.RegistOnPokeBtn(PokeButton.RESET, OnReset_ResetHP);
        }
    }



   

    public void Attack()
    {
        //애니메이션 재생

        //플레이어 죽는 함수 호출 아니면 이벤트 호출

    }

    public virtual void Hit()
    {
        //애니메이션 재생

        AnimationPlayer.SetTrigger("Hit", gameObject);
        HP--;

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
        AnimationPlayer.SetBool("isDead", gameObject, true);
        GameManager.Instance.StartInvokeCallBack(DisableMonster, 2.0f);
    }

    public void DisableMonster()
    {
        this.gameObject.SetActive(false);
    }

    private void OnReset_ResetHP()
    {
        hp = maxHP;
        
        AnimationPlayer.SetBool("isDead", gameObject, false);

    }


}
