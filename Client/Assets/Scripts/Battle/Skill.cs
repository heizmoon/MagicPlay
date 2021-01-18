using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public int id;
    public string skillName;
    public string describe;//技能描述
    public string icon;
    public string spellEffect;//施法特效
    public string castEffect;//投射特效
    public float spelllTime;//施法时间
    
    public int manaCost;//魔力消耗
    
    public float manaCostPercent;
    public int damage;//伤害值
    public int basicAttack;//基础攻击力倍数
    public float damagePercent;
    string damageDistribution;
    
    
    public int color;//法术类系
    public int heal;
    public int tempHeal;
    
    public bool targetSelf;//目标是自己还是对方

    public int manaProduce;//魔力生成
    
    public bool ifActive;//是否为主动技能
    public int buffID;
    public float CD;
    public Actor target;//技能的目标
    public Actor caster;//技能的释放者
    public Summoned casterSummon;
    public float orginSpellTime;
    public float damageDelay;
    public string hitEffect;
    public int realManaCost;
    ///<summary>使用后丢弃几张手牌</summary>
    public int usedThrowCard;
    ///<summary>使用后抽卡几张</summary>
    public int usedChooseCard;
    ///<summary>使用后从本场战斗中移除</summary>
    public bool usedToRemove;
    public bool ifCrit;
    public int updateID;
    public bool ifSeep;//是否造成穿透伤害
    public int rank;
    public int summonType;
    public int summonNum;
    public SkillData skillData;
    public SkillCard skillCard;
    public int tempMpCost;
    public int tempDamage;

    //考虑将读取表格和类写在一起？
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void InitSkill(int _id,Actor actor)//根据ID从技能表中读取技能,获取技能释放者
    {
        id =_id;
        skillData = SkillManager.instance.GetInfo(_id);
        if(skillData == null)
        {
            Debug.Log("技能数据为空！");
            return;
        }
        skillName = skillData.name;
        describe = skillData.describe;
        icon =skillData.icon;
        spellEffect =skillData.spellEffect;
        castEffect =skillData.castEffect;
        spelllTime = skillData.spelllTime;
        
        manaCost = skillData.manaCost;
        
        
        damage = skillData.damage;
        basicAttack =skillData.basicAttack;
        damagePercent =skillData.damagePercent;
        damageDistribution = skillData.damageDistribution;
        color =skillData.color;
        heal = skillData.heal;
        
        targetSelf =skillData.targetSelf;
        manaProduce =skillData.manaProduce;
        
        buffID =skillData.buffID;
        CD= skillData.CD;
        ifActive =skillData.ifActive;
        damageDelay =damageDistribution.Split(',')[0]==""?0: float.Parse(damageDistribution.Split(',')[0]);
        orginSpellTime =spelllTime;
        hitEffect =skillData.hitEffect;
        usedToRemove =skillData.usedToRemove;
        usedThrowCard =skillData.usedThrowCard;
        usedChooseCard =skillData.usedChooseCard;
        updateID =skillData.updateID;
        ifSeep = skillData.ifSeep;
        rank =skillData.rank;
        summonNum =skillData.summonNum;
        summonType =skillData.summonType;
        if(!actor)
        {
            caster=Player.instance.playerActor;
        }
        if(actor)
        {
            GetTarget(actor);
        }
        
        // if(actor)
        // {
        //     ModiferCastSpeed();
        // }
        realManaCost = Mathf.FloorToInt(caster.MpMax*manaCostPercent+manaCost);
        tempMpCost =realManaCost;
        tempDamage =damage;
        describe =string.Format(describe,Mathf.Abs(damage),Mathf.Abs(realManaCost),Mathf.Abs(manaProduce));//{0}=damage,{1}=manaCost,{2}=manaProduce,{3}=crit;{4}=hit;{5}=seep;{6}=fast
    }
    public void InitSkill(int _id,Summoned summoned)//根据ID从技能表中读取技能,获取技能释放者
    {
        id =_id;
        skillData = SkillManager.instance.GetInfo(_id);
        if(skillData == null)
        {
            Debug.Log("技能数据为空！");
            return;
        }
        skillName = skillData.name;
        describe = skillData.describe;
        icon =skillData.icon;
        spellEffect =skillData.spellEffect;
        castEffect =skillData.castEffect;
        spelllTime = skillData.spelllTime;
        
        manaCost = skillData.manaCost;
        
        
        damage = skillData.damage;
        basicAttack =skillData.basicAttack;
        damagePercent =skillData.damagePercent;
        damageDistribution = skillData.damageDistribution;
        color =skillData.color;
        heal = skillData.heal;
        
        targetSelf =skillData.targetSelf;
        manaProduce =skillData.manaProduce;
        
        buffID =skillData.buffID;
        CD= skillData.CD;
        ifActive =skillData.ifActive;
        damageDelay =damageDistribution.Split(',')[0]==""?0: float.Parse(damageDistribution.Split(',')[0]);
        orginSpellTime =spelllTime;
        hitEffect =skillData.hitEffect;
        usedToRemove =skillData.usedToRemove;
        usedThrowCard =skillData.usedThrowCard;
        usedChooseCard =skillData.usedChooseCard;
        updateID =skillData.updateID;
        ifSeep = skillData.ifSeep;
        rank =skillData.rank;
        summonNum =skillData.summonNum;
        summonType =skillData.summonType;
        casterSummon = summoned;
        GetTarget(summoned);
        
        // if(actor)
        // {
        //     ModiferCastSpeed();
        // }
        realManaCost = Mathf.FloorToInt(caster.MpMax*manaCostPercent+manaCost);
        describe =string.Format(describe,Mathf.Abs(damage),Mathf.Abs(realManaCost),Mathf.Abs(manaProduce));//{0}=damage,{1}=manaCost,{2}=manaProduce,{3}=crit;{4}=hit;{5}=seep;{6}=fast
    }
    void GetTarget(Actor actor)//获取目标和施法者
    {
        caster =actor;
        if(targetSelf)
        {
            target=actor;
        }
        else
        {
            if(actor.actorType ==0)
            {
                target = Battle.Instance.enemy;
            }
            else
            {
                target = Player.instance.playerActor;
            }
            
        }
    }
    void GetTarget(Summoned summoned)//获取目标和施法者
    {
        caster =summoned.master;
        if(targetSelf)
        {
            target=caster;
        }
        else
        {
            if(caster.actorType ==ActorType.玩家角色)
            {
                target = Battle.Instance.enemy;
            }
            else
            {
                target = Player.instance.playerActor;
            }
            
        }
    }
   


    //-----------------------------------备注说明-------------------------
    //普通技能执行逻辑：在Actor播放完spell动画后，执行ComputeDamage();
    //根据伤害时间分布，执行WaitForDamage()；
    //ExportDamage()输出伤害到Battle.Instance.ReceiveSkillDamage()进一步计算伤害加成和减免

    //对于isChannel的技能，
    //damageDistribution 每次到伤害产生间隔时输出一次完整的伤害，一旦技能释放中断，则不再输出伤害
    //如果是引导技能，则casting动画播放完毕后，循环播放castEnd动作

    
    //解决例如:一个技能在0.5秒造成30%伤害，0.5秒造成30%伤害，0.9秒造成剩余40%伤害 这样的问题
    public void ComputeDamage()
    {
        // damageDistribution ="0.5,1";
        // damageDistribution ="0.5,0.3;0.8,0.3;1,0.2;1.2,0.2";
        string[] distribution = damageDistribution.Split(';');
        float totalTime =0;
        for(int i =0;i<distribution.Length;i++)
        {
            float _time = distribution[i]==""?0:float.Parse(distribution[i].Split(',')[0]);
            float _percent = float.Parse(distribution[i].Split(',')[1]);
            int realDamage = damage+basicAttack*caster.basicAttack+Mathf.FloorToInt(target.HpMax*damagePercent);
            realDamage =Mathf.FloorToInt(realDamage*_percent);
            StartCoroutine(WaitForDamage(_time,realDamage,ifSeep));
            totalTime+=_time;
            // if(id ==105)
            // {
            // Debug.Log("STEP1:狂风攻击的realDamage="+realDamage+",damage="+damage);
            // }
        }
        
    }
    IEnumerator WaitForDamage(float _time,int realDamage,bool ifSeep)
    {
        yield return new WaitForSeconds(_time);
        ExportDamage(realDamage,ifSeep);
    }
    void ExportDamage(int realDamage,bool ifSeep)//技能输出的最终伤害→没有计算减免和加成
    {
        Battle.Instance.ReceiveSkillDamage(this,realDamage,false,ifSeep);
        // if(id ==105)
        // {
        //     Debug.Log("STEP2:狂风攻击的realDamage="+realDamage+",damage="+damage);
        // }
    }
    public void ComputeHeal()
    {
        string[] distribution = damageDistribution.Split(';');
        float totalTime =0;
        for(int i =0;i<distribution.Length;i++)
        {
            float _time = distribution[i]==""?0:float.Parse(distribution[i].Split(',')[0]);
            float _percent = float.Parse(distribution[i].Split(',')[1]);
            StartCoroutine(WaitForHeal(_time,_percent));
            totalTime+=_time;
        }
    }
    IEnumerator WaitForHeal(float _time,float _percent)
    {
        yield return new WaitForSeconds(_time);
        ExportHeal(_percent);
    }
    void ExportHeal(float _percent)
    {
        int realHeal = Mathf.FloorToInt(heal*_percent);
        target.TakeHeal(realHeal);
    }

}
