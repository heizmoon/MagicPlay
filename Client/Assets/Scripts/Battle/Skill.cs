using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public int id;
    public string skillName;
    public string discribe;//技能描述
    public string icon;
    public string spellEffect;//施法特效
    public string castEffect;//投射特效
    public float spelllTime;//施法时间
    public bool isChannel;
    public float maxChannelTime;
    public int manaCost;//魔力消耗
    public int manaCostGrow;//魔力消耗成长
    public float manaCostPercent;
    public int damage;//伤害值
    string damageDistribution;
    public int level;//技能等级
    public int hit;//计算后命中
    public int genre;//法术类系
    public int fast;//急速等级
    public int crit;//暴击等级
    public int seep;//穿透等级
    public int hitGrow;//命中成长
    public int fastGrow;//急速成长
    public int critGrow;//暴击成长
    public int seepGrow;//穿透成长
    public int damageGrow;//伤害成长
    public bool targetSelf;//目标是自己还是对方

    public int manaProduce;//魔力生成
    public int manaProduceGrow;//魔力生成成长
    public bool ifActive;//是否为主动技能
    public int buffID;
    public float CD;
    public Actor target;//技能的目标
    public Actor caster;//技能的释放者
    public float orginSpellTime;
    public float damageDelay;
    public string hitEffect;
    public int realManaCost;

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
        Ability ability = SkillManager.instance.GetInfo(_id);
        skillName = ability.name;
        discribe = ability.discribe;
        icon =ability.icon;
        spellEffect =ability.spellEffect;
        castEffect =ability.castEffect;
        spelllTime = ability.spelllTime;
        isChannel =ability.isChannel;
        maxChannelTime =ability.maxChannelTime;
        manaCost = ability.manaCost;
        manaCostGrow =ability.manaCostGrow;
        manaCostPercent =ability.manaCostPercent;
        damage = ability.damage;
        damageDistribution = ability.damageDistribution;
        genre =ability.genre;
        crit = ability.crit;
        hit =ability.hit;
        seep =ability.seep;
        fast =ability.fast;
        damageGrow =ability.damageGrow;
        hitGrow = ability.hitGrow;
        seepGrow = ability.seepGrow;
        critGrow = ability.critGrow;
        fastGrow =ability.fastGrow;
        targetSelf =ability.targetSelf;
        manaProduce =ability.manaProduce;
        manaProduceGrow =ability.manaProduceGrow;
        buffID =ability.buffID;
        CD= ability.CD;
        ifActive =ability.ifActive;
        damageDelay =damageDistribution.Split(',')[0]==""?0: float.Parse(damageDistribution.Split(',')[0]);
        orginSpellTime =spelllTime;
        hitEffect =ability.hitEffect;
        if(!actor)
        {
            caster=Player.instance.playerActor;
        }
        if(actor)
        {
            GetTarget(actor);
        }
        GetLevel();
        ModiferLevel();
        if(actor)
        {
            ModiferCastSpeed();
        }
        realManaCost = Mathf.FloorToInt(caster.MpMax*manaCostPercent+manaCost);
        discribe =string.Format(discribe,Mathf.Abs(damage),Mathf.Abs(realManaCost),Mathf.Abs(manaProduce),Mathf.Abs(crit),Mathf.Abs(hit),Mathf.Abs(seep),Mathf.Abs(fast));//{0}=damage,{1}=manaCost,{2}=manaProduce,{3}=crit;{4}=hit;{5}=seep;{6}=fast
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
    void GetLevel()
    {
        //如果是玩家角色释放的技能，那么技能等级从Player中获取
        if(caster.actorType==0)
        {
            level=Player.instance.GetSkillLevel(id);
        }
        //如果是敌人释放的技能，那么技能等级等于敌人的等级*技能等级成长
        else
        {
            level =caster.level*caster.skillGrow;
        }

    }
    void ModiferLevel()
    {
        damage += damageGrow*level;
        crit += critGrow*level;
        fast += fastGrow*level;
        hit += hitGrow*level; 
        manaCost +=manaCostGrow*level;
        manaProduce +=manaProduceGrow*level;
    }

    //-----------------------------------备注说明-------------------------
    //普通技能执行逻辑：在Actor播放完spell动画后，执行ComputeDamage();
    //根据伤害时间分布，执行WaitForDamage()；
    //ExportDamage()输出伤害到Battle.Instance.ReceiveSkillDamage()进一步计算伤害加成和减免

    //对于isChannel的技能，
    //damageDistribution 每次到伤害产生间隔时输出一次完整的伤害，一旦技能释放中断，则不再输出伤害
    //如果是引导技能，则casting动画播放完毕后，循环播放castEnd动作

    public void ModiferCastSpeed()//根据角色当前的急速，调整施法速度
    {
        
        //从施法者身上得到急速变化值
        float _speed = orginSpellTime;
        float tempFast =0;
        for (int i = 0; i < caster.buffs.Count; i++)
        {
            if(caster.buffs[i].buffData._type ==BuffType.影响急速&& caster.buffs[i].buffData._genreList.Contains(genre))
            {
                tempFast = tempFast+caster.buffs[i].currentValue;
            }
        }
        foreach (var item in caster.buffs)
        {
            if(item.buffData._type == BuffType.百分比影响急速 && item.buffData._genreList.Contains(genre))
            {
                tempFast+=Mathf.CeilToInt(fast*item.currentValue);        
            }
        }
        if(tempFast ==0)
        {
            _speed =orginSpellTime;
        }
        else if(tempFast>0)
        {
            if(fast ==0)
            {
                _speed =(float)(orginSpellTime *(1-System.Math.Tanh(1.1-(1+tempFast)/1)));
            }
            else
            {
                _speed =(float)(orginSpellTime *(1-System.Math.Tanh(1.1-(fast+tempFast)/fast)));
            }
        }
        else
        {
            if(fast ==0)
            {
                _speed =(float)(orginSpellTime *(1-System.Math.Tanh((1+tempFast)/1-1)));

            }
            else
            {
                _speed =(float)(orginSpellTime *(1-System.Math.Tanh((fast+tempFast)/fast-1)));
            }
            
        }
        
        _speed =_speed<0.5f?0.5f:_speed;
        _speed =_speed>orginSpellTime*2?orginSpellTime*2:_speed;
        if(caster.actorType ==ActorType.敌人)
        {
            // Debug.LogWarningFormat("本次{0}的施法时间为：{1}s，急速为：{2}",skillName,_speed,fast+tempFast);
        }
        spelllTime =_speed;
    }
    //解决例如:一个技能在0.5秒造成30%伤害，0.5秒造成30%伤害，0.9秒造成剩余40%伤害 这样的问题
    public void ComputeDamage()
    {
        // damageDistribution ="0.5,1";
        // damageDistribution ="0.5,0.3;0.8,0.3;1,0.2;1.2,0.2";
        string[] distribution = damageDistribution.Split(';');
        for(int i =0;i<distribution.Length;i++)
        {
            float _time = distribution[i]==""?0:float.Parse(distribution[i].Split(',')[0]);
            float _percent = float.Parse(distribution[i].Split(',')[1]);
            StartCoroutine(WaitForDamage(_time,_percent));
        }
    }
    IEnumerator WaitForDamage(float _time,float _percent)
    {
        yield return new WaitForSeconds(_time);
        ExportDamage(_percent);
    }
    void ExportDamage(float _percent)//技能输出的最终伤害→没有计算减免和加成
    {
        int realDamage =Mathf.FloorToInt(damage*_percent);
        Battle.Instance.ReceiveSkillDamage(this,realDamage,false);
    }
    // public void ExportDamage(int damage)
    // {
    //     Battle.Instance.ReceiveSkillDamage(this,damage);
    // }

}
