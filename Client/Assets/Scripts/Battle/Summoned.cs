using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Summoned : MonoBehaviour
{
    SummonData summonData;
    [SerializeField]
    float LifeTime =15f;
    [SerializeField]
    int Power =2;
    [SerializeField]
    //每隔多长时间攻击一次
    float attackSpeed =3f; 
    float attackInterval;
    Transform castPoint;
    public Actor master;
    Actor target;
    Skill skill;
    Animator animator;
    List<Buff> buffs =new List<Buff>();
    public SkillCard skillCard;
    void Start()
    {
        //事件订阅
        Player.instance.playerActor.OnUpdateSummonedLifeTime+=OnExtendLifeTime;
        // Player.instance.playerActor.OnUpdateSummonedSpeed+=OnExtendAttackSpeed;
        // Player.instance.playerActor.OnUpdateSummonedPower+=OnExtendPower;
        BuffManager.instance.OnSummonedAddBuff += OnAddBuff;
        Player.instance.playerActor.OnOrderSummonedAttack+=OnOrderAttack;
        animator =GetComponent<Animator>();
        castPoint =transform.Find("Image/castPoint");
        
    }
    public void Init(Actor master,SummonData summonData,float lifeTimePlus,Skill summonSkill)
    {
        this.master =master;
        this.summonData =summonData;
        this.skillCard =summonSkill.skillCard;
        target = master.target;
        LifeTime =summonData.lifeTime+lifeTimePlus;
        Power =summonData.power;
        attackSpeed =summonData.speed;
        
        skill = SkillManager.TryGetFromPool(summonData.skill,this);
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
    void OnAddBuff(Buff buff)
    {
        buffs.Add(buff); 
        buffs.Remove(buff);
        if(buff.buffData._type == BuffType.影响召唤物强度)
        {
            OnExtendPower(Mathf.FloorToInt(buff.buffData.value));
        }
        if(buff.buffData._type == BuffType.影响召唤物攻速)
        {
            OnExtendAttackSpeed(buff.buffData.value);
        }
    }
    void OnRemoveBuff(Buff buff)
    {
        if(buffs.Contains(buff))
        {
            buffs.Remove(buff);
            if(buff.buffData._type == BuffType.影响召唤物强度)
            {
                OnExtendPower(-Mathf.FloorToInt(buff.buffData.value));
            }
            if(buff.buffData._type == BuffType.影响召唤物攻速)
            {
                OnExtendAttackSpeed(-buff.buffData.value);
            }
        }
    }
    void OnExtendLifeTime(int num)
    {
        LifeTime+=num;
    }
    void OnExtendAttackSpeed(float num)
    {
        if(num>0)
        attackSpeed=attackSpeed*(1-num);
        else
        attackSpeed=attackSpeed/(1+num);
    }
    void OnExtendPower(int num)
    {
        Power+=num;
        if(num>0)
        transform.DOScale(1.5f*Vector3.one,0.5f);
        else
        transform.DOScale(Vector3.one,0.5f);

    }
    void OnOrderAttack(int num)
    {
        Attack(num);
    }
    public void Death()
    {
        //解除订阅
        Player.instance.playerActor.OnUpdateSummonedLifeTime-=OnExtendLifeTime;
        // Player.instance.playerActor.OnUpdateSummonedSpeed-=OnExtendAttackSpeed;
        // Player.instance.playerActor.OnUpdateSummonedPower-=OnExtendPower;
        BuffManager.instance.OnSummonedAddBuff -= OnAddBuff;
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
        if(skill.heal==0)
        //附加伤害的攻击
        {
            skill.damage =num;
            skill.ComputeDamage();
        }
        // Battle.Instance.ReceiveSkillDamage(skill,num,false,false);
        else
        {
            skill.heal =num;
            skill.ComputeHeal();
        }
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
