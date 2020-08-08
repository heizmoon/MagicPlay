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
    public void ReceiveSkillDamage(Skill skill,int damage,bool ifRebound,float delay)
    {
        StartCoroutine(WaitForReceiveSkillDamage(skill,damage,ifRebound,delay));
    }
    IEnumerator WaitForReceiveSkillDamage(Skill skill,int damage,bool ifRebound,float delay)
    {
        yield return new WaitForSeconds(delay);
        ReceiveSkillDamage(skill, damage,ifRebound);
    }
    ///<summray>一般性技能伤害</summary>
    public void ReceiveSkillDamage(Skill skill,int damage,bool ifRebound)
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
            if(ComputeCrit(skill))
            {
                damage=(damage+ComputeCritDamageAdd(skill))*2;
                crit =true;
                //通知施法者已经暴击,暴击伤害为damage(未经削减的)
                skill.caster.OnSkillHasCrit(skill,damage);
            }
            damage = Mathf.CeilToInt(damage*ComputeResDamage(skill));
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
            ExportDamage(damage,skill.target,crit,skill.genre,ifRebound);
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
    
    ///<summary>buff伤害</summary>
    //持续伤害可暴击，不受急速影响，独立判断命中，独立判断减伤
    ///<summray>用于反弹伤害</summary>
    public void ReceiveSkillDamage(int damage,Actor target,int genre)
    {
        ExportDamage(damage,target,false,genre,true);
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
        int tempd = SkillManager.CheckSkillOnComputeToGiveAdd(skill,1013,new List<int>(){7},false,new List<int>(){1012},new List<int>(){0,1,2,3,4,5,6,7,8},null,SkillBuffType.伤害);
        if(tempd>0)
        {
            Debug.LogWarningFormat("{0}技能的碎冰附加伤害为{1}",skill.skillName,tempd);
        }
        damage +=tempd;
        tempd =0;
        //燃烧：如果目标身上有点燃效果，技能1111等级大于0，则除点燃外所有火焰魔法伤害增加
        tempd = SkillManager.CheckSkillOnComputeToGiveAdd(skill,1111,new List<int>(){4},false,new List<int>(){1110},new List<int>(){1},null,SkillBuffType.伤害);
        if(tempd>0)
        {
            Debug.LogWarningFormat("{0}技能的燃烧附加伤害为{1}",skill.skillName,tempd);
        }
        damage +=tempd;
        foreach (var item in skill.caster.buffs)
        {
            if(item.buffData._type == BuffType.数值增减附加的伤害 && item.buffData._genreList.Contains(skill.genre))
            {
                damage+=Mathf.CeilToInt(item.currentValue);
            }
        }
        foreach (var item in skill.caster.buffs)
        {
            if(item.buffData._type == BuffType.百分比增减附加的伤害 && item.buffData._genreList.Contains(skill.genre))
            {
                damage*=Mathf.CeilToInt(1+item.currentValue);
            }
        }
        


        return damage;
        
    }
    int ComputeDamageBuffStepTwo(Skill skill,int damage)
    {
        
        foreach (var item in skill.target.buffs)
        {
            if(item.buffData._type == BuffType.数值增减受到的伤害 && item.buffData._genreList.Contains(skill.genre))
            {
                damage+=Mathf.CeilToInt(item.currentValue);
            }
        }
        foreach (var item in skill.target.buffs)
        {
            if(item.buffData._type == BuffType.百分比增减受到的伤害 && item.buffData._genreList.Contains(skill.genre))
            {
                damage+=Mathf.CeilToInt(item.currentValue);
            }
        }
        return damage;
        
    }
    void ExportDamage(int damage,Actor target,bool crit,int genre,bool ifRebound)//计算后传递伤害给目标
    {
        target.TakeDamage(damage,crit,genre,ifRebound);
        Debug.LogFormat("目标是：{0}，伤害为：{1}",target.name,damage);
    }
    bool ComputeHit(Skill skill)//判断是否命中
    {
        if(skill.targetSelf)
        {
            return true;
        }
        float percent;
        int p =ComputeSkillsHit(skill)-ComputeTargetsDodge(skill);
        if(p==1)
        {
            percent =90;
        }
        else if(p==0)
        {
            percent =90;
        }
        else if(p>1)
        {
            percent =(Mathf.Log(p,constanceHitT)+90);
        }
        else
        {
            float i =Mathf.Log(-p,constanceHitB);
            percent =90-(i);
        }
        
        if(Random.Range(0,100)<percent)
        {
            // Debug.Log("命中了");
            return true;
        }
        else
        {
            // Debug.LogWarningFormat("没命中:{0}",percent);
            return false;
        }
    }
    bool ComputeCrit(Skill skill)//判断是否暴击
    {   
        int p =ComputeSkillsCirt(skill)-ComputeTargetsTough(skill);
        if(p<1)
        {
            return false;
        }
        float percent = (Mathf.Log(p,constanceCrit));
        if(Random.Range(0,100)<percent)
        {
            // Debug.LogWarningFormat("暴击了");
            return true;
        }
        else
        {
            return false;
        }
    }  
    int ComputeTargetsDodge(Skill skill)//计算目标的闪避
    {
        int _dodge =skill.target.dodge;
        foreach (var item in skill.target.buffs)
        {
            if(item.buffData._type == BuffType.影响闪避 && item.buffData._genreList.Contains(skill.genre))
            {
                _dodge+=Mathf.CeilToInt(item.currentValue);
            }
        }
        foreach (var item in skill.caster.buffs)
        {
            if(item.buffData._type == BuffType.百分比影响闪避 && item.buffData._genreList.Contains(skill.genre))
            {
                if(_dodge>0)
                {
                    _dodge=Mathf.CeilToInt(_dodge*(1+item.currentValue));
                }
                else if(_dodge<0)
                {
                    _dodge+=Mathf.CeilToInt(-_dodge*item.currentValue);
                }
            }
        }
        return _dodge;
    }
    int ComputeSkillsHit(Skill skill)//计算法术命中
    {
        int _hit =skill.hit;
        //判断技能的caster是否拥有增加技能命中的buff
        foreach (var item in skill.caster.buffs)
        {
            if(item.buffData._type == BuffType.影响命中 && item.buffData._genreList.Contains(skill.genre))
            {
                _hit+=Mathf.CeilToInt(item.currentValue);
            }
        }
        foreach (var item in skill.caster.buffs)
        {
            if(item.buffData._type == BuffType.百分比影响命中 && item.buffData._genreList.Contains(skill.genre))
            {
                if(_hit>0)
                {
                    _hit=Mathf.CeilToInt(_hit*(1+item.currentValue));
                }
                else if(_hit<0)
                {
                    _hit+=Mathf.CeilToInt(-_hit*item.currentValue);
                }
            }
        }
         
        
        return _hit;
    }
    int ComputeSkillsCirt(Skill skill)//计算法术暴击
    {
        int _crit =skill.crit;
        foreach (var item in skill.caster.buffs)
        {
            if(item.buffData._type == BuffType.影响暴击 && item.buffData._genreList.Contains(skill.genre))
            {
                _crit+=Mathf.CeilToInt(item.currentValue);
            }
        }
        foreach (var item in skill.caster.buffs)
        {
            if(item.buffData._type == BuffType.百分比影响暴击 && item.buffData._genreList.Contains(skill.genre))
            {
                if(_crit>0)
                {
                    _crit=Mathf.CeilToInt(_crit*(1+item.currentValue));
                }
                else if(_crit<0)
                {
                    _crit+=Mathf.CeilToInt(-_crit*item.currentValue);
                }
            }
        }
        //死亡霜寒
        _crit += SkillManager.CheckSkillOnComputeToGiveAdd(skill,1014,new List<int>(){7},false,new List<int>(){1012},new List<int>(){0,1,2,3,4,5,6,7,8},null,SkillBuffType.暴击); 
        //凯撒技艺
        _crit += SkillManager.CheckSkillOnComputeToGiveAdd(skill,1113,new List<int>(){4},false,new List<int>(){1110},new List<int>(){0,1,2,3,4,5,6,7,8},null,SkillBuffType.暴击);
        return _crit;
    }
    int ComputeTargetsTough(Skill skill)//计算目标韧性
    {
        int _tough =1;
        foreach (var item in skill.target.buffs)
        {
            if(item.buffData._type == BuffType.影响韧性 && item.buffData._genreList.Contains(skill.genre))
            {
                _tough+=Mathf.CeilToInt(item.currentValue);
            }
        }
        foreach (var item in skill.caster.buffs)
        {
            if(item.buffData._type == BuffType.百分比影响韧性 && item.buffData._genreList.Contains(skill.genre))
            {
                if(_tough>0)
                {
                    _tough=Mathf.CeilToInt(_tough*(1+item.currentValue));
                }
                else if(_tough<0)
                {
                    _tough+=Mathf.CeilToInt(-_tough*item.currentValue);
                }
            }
        }
        return _tough;
    }
    int ComputeTargetsResistance(Skill skill)//计算目标抗性
    {
        //1.判断技能类别
        //2.获取目标该类别抗性
        int _resistance =skill.target.resistance[skill.genre];
        foreach (var item in skill.target.buffs)
        {
            if(item.buffData._type == BuffType.影响抗性 && item.buffData._genreList.Contains(skill.genre))
            {
                _resistance+=Mathf.CeilToInt(item.currentValue);
            }
        }
        foreach (var item in skill.caster.buffs)
        {
            if(item.buffData._type == BuffType.百分比影响抗性 && item.buffData._genreList.Contains(skill.genre))
            {
                if(_resistance>0)
                {
                    _resistance=Mathf.CeilToInt(_resistance*(1+item.currentValue));
                }
                else if(_resistance<0)
                {
                    _resistance+=Mathf.CeilToInt(-_resistance*item.currentValue);
                }
            }
        }

        return _resistance;
    }
    int ComputeSkillsSeep(Skill skill)//计算法术穿透
    {
        int _seep =skill.seep;
        foreach (var item in skill.caster.buffs)
        {
            if(item.buffData._type == BuffType.影响穿透 && item.buffData._genreList.Contains(skill.genre))
            {
                _seep+=Mathf.CeilToInt(item.currentValue);
            }
        }
        foreach (var item in skill.caster.buffs)
        {
            if(item.buffData._type == BuffType.百分比影响穿透 && item.buffData._genreList.Contains(skill.genre))
            {
                if(_seep>0)
                {
                    _seep=Mathf.CeilToInt(_seep*(1+item.currentValue));
                }
                else if(_seep<0)
                {
                    _seep+=Mathf.CeilToInt(-_seep*item.currentValue);
                }
            }
        }
        return _seep;
    }
    int ComputeCritDamageAdd(Skill skill)//计算暴击增益效果
    {
        int _add =1;
        // for(int i=0;i<skill.caster.buffs.Count;i++)
        // {
        // }
        foreach (var item in skill.caster.buffs)
        {
            if(item.buffData._type == BuffType.影响暴击增益 && item.buffData._genreList.Contains(skill.genre))
            {
                _add+=Mathf.CeilToInt(item.currentValue);
            }
        }

        return _add;
    }
    ///<summary>输入穿透与抗性，取得实际伤害的百分比</summray>
    float ComputeResDamage(Skill skill)
    {
        if(skill.targetSelf)
        {
            return 1f;
        }
        float p =ComputeSkillsSeep(skill)-ComputeTargetsResistance(skill);
        if(p>=-1)
        {
            return 1f;
        }
        float percent =(100-Mathf.Log(-p,constanceResis))/100;
        if(percent<=0)
        {
            return 0;
        }
        return percent;
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
