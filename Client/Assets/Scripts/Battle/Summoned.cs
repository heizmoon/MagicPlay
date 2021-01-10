using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summoned : MonoBehaviour
{
    SummonData summonData;
    float LifeTime =15f;
    int Power =2;
    //每隔多长时间攻击一次
    float attackSpeed =3f; 
    float attackInterval;
    Transform castPoint;
    public Actor master;
    Actor target;
    Skill skill;
    Animator animator;
    void Start()
    {
        //事件订阅
        Player.instance.playerActor.OnUpdateSummonedAttack+=OnExtendLifeTime;
        Player.instance.playerActor.OnUpdateSummonedSpeed+=OnExtendAttackSpeed;
        Player.instance.playerActor.OnUpdateSummonedDamage+=OnExtendPower;
        Player.instance.playerActor.OnOrderSummonedAttack+=OnOrderAttack;
        animator =GetComponent<Animator>();
        castPoint =transform.Find("Image/castPoint");
        
    }
    public void Init(Actor master,SummonData summonData)
    {
        this.master =master;
        this.summonData =summonData;
        target = master.target;
        LifeTime =summonData.lifeTime;
        Power =summonData.power;
        attackSpeed =summonData.speed;
        
        skill = SkillManager.TryGetFromPool(summonData.skill,master);
        if(skill.targetSelf)
        {
            skill.target =master;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(LifeTime<=0)
        {
            Death();
        }
        else
        {
            LifeTime-=Time.deltaTime;
            if(attackSpeed<=0)
            {
                return;
            }
            attackInterval+=Time.deltaTime;
            if(attackInterval>=attackSpeed)
            {
                attackInterval =0;
                Attack();
            }
        }
    }
    void OnExtendLifeTime(int num)
    {
        LifeTime+=num;
    }
    void OnExtendAttackSpeed(float num)
    {
        attackSpeed*=num;
    }
    void OnExtendPower(int num)
    {
        Power+=num;

    }
    void OnOrderAttack(int num)
    {
        Attack(num);
    }
    public void Death()
    {
        //解除订阅
        Player.instance.playerActor.OnUpdateSummonedAttack-=OnExtendLifeTime;
        Player.instance.playerActor.OnUpdateSummonedSpeed-=OnExtendAttackSpeed;
        Player.instance.playerActor.OnUpdateSummonedDamage-=OnExtendPower;
        Player.instance.playerActor.OnOrderSummonedAttack-=OnOrderAttack;
        SummonManager.instance.DecreaseSummonedNum(this);
        Destroy(gameObject);
        
    }
    void Attack()
    {
        //攻击动画，攻击特效
        animator.SetTrigger("Attack");
        CreateCastEffect(skill);
        StartCoroutine(WaitForAttack(skill.CD,Power));
    }
    void Attack(int num)
    {
        //攻击动画，攻击特效
        animator.SetTrigger("Attack");
        CreateCastEffect(skill);
        // Debug.Log("圣剑攻击！");
        StartCoroutine(WaitForAttack(skill.CD,Power+num));
    }
    IEnumerator WaitForAttack(float time,int num)
    {
        yield return new WaitForSeconds(time);
        //附加伤害的攻击
        Battle.Instance.ReceiveSkillDamage(skill,num,false,false);
    }
    void CreateCastEffect(Skill skill)
    {
        if(skill.castEffect=="")
        {
            return;
        }
        
        
        Transform f = EffectManager.TryGetFromPool(skill.castEffect);
        if(f!=null)
        {
            f.SetParent(castPoint);
            f.gameObject.SetActive(true);
            f.localPosition =Vector3.zero;
            // f.localScale =Vector3.one;
            if(target!=null)
            {
                
                EffectManager.CastEffect(f,target.hitPoint,skill.damageDelay,skill.hitEffect);
                
            }
            else
            {
                
            }

            return;
        }

    }
}
