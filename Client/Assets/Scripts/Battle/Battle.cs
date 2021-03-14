using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Battle : MonoBehaviour
{
    //战场类
    //战场类用于处理一切关于主角和一个敌方目标战斗的场景
    //包括冒险 和 普通练习施法类技能
    //在这里同时也会计算一切伤害，效果，然后传递给目标
    static float constanceHitB =1.15f;
    static float constanceHitT =4f;
    static float constanceCrit =1.15f;
    static float constanceResis =1.15f;
    Dictionary<Skill,int> damageStatistic =new Dictionary<Skill, int>();//角色输出伤害
    Dictionary<Skill,int> hurtStatistic =new Dictionary<Skill, int>();//角色承受伤害

    public static Battle Instance;
    public  Actor enemy;
    public Actor playerActor;
    void Awake()
    {
        Instance =this;
 
    }

    void Start()
    {
  
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void InitBattle(Actor actor)//传入敌人
    {                
        enemy =actor;
        playerActor = Player.instance.playerActor;
        
    }
    public void ReceiveSkillDamage(Skill skill,int damage,bool ifRebound,float delay,bool ifSeep)
    {
        StartCoroutine(WaitForReceiveSkillDamage(skill,damage,ifRebound,delay,ifSeep));
    }
    IEnumerator WaitForReceiveSkillDamage(Skill skill,int damage,bool ifRebound,float delay,bool ifSeep)
    {
        yield return new WaitForSeconds(delay);
        ReceiveSkillDamage(skill, damage,ifRebound,ifSeep);
    }
    
    ///<summray>一般性技能伤害</summary>
    public void ReceiveSkillDamage(Skill skill,int damage,bool ifRebound,bool ifSeep)
    {
        if(skill.target.animState==AnimState.dead||skill.caster.animState==AnimState.dead)
        {
            return;
        }
        bool crit =false;
        if(ComputeHit(skill))
        {
            damage = ComputeDamageBuffStepOne(skill,damage);
            skill.caster.OnSkillHasHit(true,skill);
            float critRate = playerActor.Crit+skill.exCrit;//某些技能拥有额外暴击率加成
            if(critRate>=Random.Range(0,101))
            {
                damage=Mathf.CeilToInt(damage*1.5f);
                crit =true;
                //通知施法者已经暴击,暴击伤害为damage(未经削减的)
                skill.caster.OnSkillHasCrit(skill,damage);
            }
            damage = ComputeDamageBuffStepTwo(skill,damage);
            if(!skill.targetSelf && damage<=0)
            {
                //通知施法者技能被抵抗
                skill.caster.OnSkillHasResistance();
                //通知目标技能被抵抗
                skill.target.OnHitResistance();
                return;
            }
            Statistic(skill,damage);//伤害统计
            ExportDamage(damage,skill.target,crit,skill.color,ifRebound,ifSeep,skill);
            if(ifRebound)
            {
                // Debug.LogWarningFormat("{0}反弹伤害：{1}点,目标是：{2}",skill.skillName,damage,skill.target.name);
            }
        }
        else
        {
            skill.caster.OnSkillHasHit(false,skill);
            skill.target.OnHitMiss();
        }
    }
    public void NoDamageSkillHitTarget(Skill skill)
    {
        if(ComputeHit(skill))
        {
            skill.caster.OnSkillHasHit(true,skill);
        }
        else
        {
            skill.caster.OnSkillHasHit(false,skill);
            skill.target.OnHitMiss();
        }

    }
    ///<summary>buff伤害</summary>
    //持续伤害可暴击，不受急速影响，独立判断命中，独立判断减伤
    ///<summray>用于反弹伤害</summary>
    public void ReceiveSkillDamage(int damage,Actor target,int genre,bool ifSeep)
    {
        Skill skill =null;
        ExportDamage(damage,target,false,genre,true,ifSeep,skill);
    }
    //第一步：技能输出伤害 = （技能原本伤害+buff数值附加伤害）*（1+buff百分比附加）
    //第二步：如果暴击 技能输出伤害 = (技能输出伤害 + 暴击数值附加)* （2+暴击伤害百分比附加） 
    //第三步：抗性减免后伤害 = 技能输出伤害*抗性减免百分比
    //第四步：承受伤害 = (抗性减免后伤害+buff数值减免伤害)*(1+buff百分比减免伤害)
    int ComputeDamageBuffStepOne(Skill skill,int damage)
    {
        //特殊被动技能附加的伤害
        //如果身上有冻结效果，技能1013等级大于0，则所有伤害增加
        //碎冰
        // int tempd = SkillManager.CheckSkillOnComputeToGiveAdd(skill,1013,new List<int>(){7},false,new List<int>(){1012},new List<int>(){0,1,2,3,4,5,6,7,8},null,SkillBuffType.伤害);
        // if(tempd>0)
        // {
        //     Debug.LogWarningFormat("{0}技能的碎冰附加伤害为{1}",skill.skillName,tempd);
        // }
        // damage +=tempd;
        // tempd =0;
        // //燃烧：如果目标身上有点燃效果，技能1111等级大于0，则除点燃外所有火焰魔法伤害增加
        // tempd = SkillManager.CheckSkillOnComputeToGiveAdd(skill,1111,new List<int>(){4},false,new List<int>(){1110},new List<int>(){1},null,SkillBuffType.伤害);
        // if(tempd>0)
        // {
        //     Debug.LogWarningFormat("{0}技能的燃烧附加伤害为{1}",skill.skillName,tempd);
        // }
        // damage +=tempd;
        foreach (var item in skill.caster.buffs)
        {
            if(item.buffData._type == BuffType.数值增减附加的伤害 && item.buffData._genreList.Contains(skill.color))
            {
                damage+=Mathf.CeilToInt(item.currentValue);
            }
        }
        float damageChangeValue =0;
        foreach (var item in skill.caster.buffs)
        {
            if(item.buffData._type == BuffType.百分比增减附加的伤害)
            {
                damageChangeValue+=item.currentValue;
            }
        }
        // Debug.LogWarning("原始伤害为："+damage);
        damage=Mathf.RoundToInt(damage*(1+damageChangeValue));
        // Debug.LogWarning("伤害改变了："+damageChangeValue+"，改变后为："+damage);

        
        return damage;
        
    }
    int ComputeDamageBuffStepTwo(Skill skill,int damage)
    {
        
        // foreach (var item in skill.target.buffs)
        // {
        //     if(item.buffData._type == BuffType.数值增减受到的伤害)
        //     {
        //         damage+=Mathf.CeilToInt(item.currentValue);
        //     }
        // }
        // foreach (var item in skill.target.buffs)
        // {
        //     if(item.buffData._type == BuffType.百分比增减受到的伤害)
        //     {
        //         damage+=Mathf.CeilToInt(item.currentValue);
        //     }
        // }
        //计算寒冷层数
        int temp =damage;
        damage-=skill.caster.coldNum;
        skill.caster.AddCold(-temp);
        return damage;
        
    }
    void ExportDamage(int damage,Actor target,bool crit,int genre,bool ifRebound,bool ifSeep,Skill skill)//计算后传递伤害给目标
    {
        target.TakeDamage(damage,crit,genre,ifRebound,ifSeep,skill);
        // Debug.LogFormat("目标是：{0}，伤害为：{1}",target.name,damage);
    }
    bool ComputeHit(Skill skill)//判断是否命中
    {
        if(skill.targetSelf)
        {
            return true;
        }
        float r =Random.Range(0,100);
        if(skill.target.dodge>r)
        {
            return false;
        }
        // if(skill.target.animState ==AnimState.dodge)
        // {
        //     return false;
        // }
        else
        return true;
    }
    bool ComputeCrit(Skill skill)//判断是否暴击
    {   
        
        return skill.ifCrit;
        
    }  
    
    public void Statistic(Skill skill,int damage)//传入一次伤害以及伤害来源
    {
        if(damage<=0)
        {
            return;
        }
        if(skill.target.animState==AnimState.dead)
        {
            return;
        }
        if(skill.target ==enemy)
        {
            if(damageStatistic.ContainsKey(skill))
            {
                damageStatistic[skill] +=damage; 
            }
            else
            {
                damageStatistic.Add(skill,damage);
            }
        }
        else
        {
            if(hurtStatistic.ContainsKey(skill))
            {
                hurtStatistic[skill] +=damage;
            }
            else
            {
                hurtStatistic.Add(skill,damage);
            }
        }
        if(skill.target !=playerActor)
        {
            Debug.LogWarningFormat("{0}技能输出伤害{1}点",skill.skillName,damage);
        }
    }
    ///<summary>显示战斗结算信息</summary>
    public void ShowStatisticDamage(int type)
    {
        int lineNumber =0;
        float totalDamage =0;
        float otherDamage =0;
        Dictionary<Skill,int> statisticDic;
        if(type ==0)
        {
            statisticDic =damageStatistic;
        }
        else if(type ==1)
        {
            statisticDic =hurtStatistic;

        }
        else
        {
            statisticDic =damageStatistic;
        }
        
        foreach(var item in statisticDic)
        {
            totalDamage += item.Value;
        }
        Dictionary<Skill,int> sortedDic =statisticDic.OrderByDescending(o =>o.Value).ToDictionary(p => p.Key, o => o.Value);
        foreach(var item in sortedDic)
        {
            string percent = (item.Value/totalDamage).ToString("0.00%");
            if(lineNumber<8)
            {
                if(lineNumber==7)
                {
                    int Fdamage =Mathf.CeilToInt(totalDamage-otherDamage);
                    percent = (Fdamage/totalDamage).ToString("0.00%");
                    UIBattle.Instance.CreateStatisticLine(item.Key,Fdamage,percent,lineNumber,type);
                }
                else
                {
                    UIBattle.Instance.CreateStatisticLine(item.Key,item.Value,percent,lineNumber,type);
                    lineNumber++;
                    otherDamage += item.Value;
                }
                Debug.LogFormat("创建一条！！！");
            }
            else
            {
                return;
            }    
        }
    }
    ///<summary></summary>
    public void OnTakeDamage()
    {

    }
    public void OnShieldHasOverDamage()
    {

    }
    public void OnShieldAbsorbDamage()
    {

    }

}
