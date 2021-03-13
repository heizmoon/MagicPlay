using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Buff
{
    // Start is called before the first frame update
    public BuffData buffData;
    public int level;
    public float currentValue;
    public Actor target;
    public List<Buff> childrenBuffs =new List<Buff>();
    public BuffIcon buffIcon;

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CheckChildren()
    {
        // Debug.LogWarningFormat("检查childrenbuff：{0}",buffData.name);
        foreach (var item in buffData._childrenBuff)
        {
            childrenBuffs.Add(BuffManager.instance.CreateBuffForActor(item,target));
            Debug.LogFormat("尝试添加buffID：{0}",item);
        }
    }
    public void CheckBuffIcon()
    {
        
        if(UIBattle.Instance!=null)
        {
            if(buffData.icon=="")
            {
                buffIcon = UIBattle.Instance.CreateBuffIcon(this,false);
            }
            else
            {
                buffIcon = UIBattle.Instance.CreateBuffIcon(this,true);
            }   
        }    
    }
    public void OnBuffBegin()
    {
        //执行当buff开始就xx的事件
        switch (buffData._type)
        {
            case BuffType.昏迷:
            {
                target.animState =AnimState.dizzy;
                target.StopCasting();
                target.ChangeAnimatorInteger(4);
            }
            break;
            case BuffType.影响攻击力:
            target.basicAttack+=(int)buffData.value;
            Debug.LogWarning("增加了攻击力："+(int)buffData.value);
            break;
            case BuffType.影响防御力:
            target.basicDefence+=(int)buffData.value;
            break;
            case BuffType.数值吸收伤害:
                target.armor+=Mathf.FloorToInt(buffData.value);
                target.RefeashArmorAutoDecayTime();
                // 如果身上有获得护甲后触发的buff，那么此时触发
                if(buffData.value==0)
                BuffManager.Check_SpecialTypeBuff_ToTriggerSkill(target,BuffType.获得护甲后触发技能);
                else
                BuffManager.Check_SpecialTypeBuff_ToTriggerSkill(target.target,BuffType.获得护甲后触发技能);
            break;
            case BuffType.影响闪避:
                target.dodge+=buffData.value;
            break;
            case BuffType.影响暴击率:
                target.Crit+=buffData.value;
            break;
            case BuffType.百分比影响护甲值:
                target.armor+=(int)(buffData.value*target.armor);
            break;
            case BuffType.影响全局能量消耗:
            {
                UIBattle.Instance.playerActor.cardMpReduce+=Mathf.FloorToInt(buffData.value);
                UIBattle.Instance.ReduceAllCardCost(Mathf.FloorToInt(buffData.value));
            }
            break;
            case BuffType.影响召唤物强度:
                BuffManager.instance.SummonedAddBuff(this);
            break;
            case BuffType.影响召唤物攻速:
                BuffManager.instance.SummonedAddBuff(this);
            break;
            case BuffType.影响召唤物持续时间:
            {
                target.SummonedLifeTimePlus+=buffData.value;
                target.InvokeSummonedLifeTimeUpdate(Mathf.FloorToInt(buffData.value));
            }
            break;
            case BuffType.影响补牌数量:
                target.dealCardsNumber+=Mathf.FloorToInt(buffData.value);
            break;
            case BuffType.引导能量:
                target.ChannelSkill(buffData.value);
                target.OnChannelSkillManaOut+=ChannelEnd;
            break;
            case BuffType.影响护甲持续时间:
                if(buffData.value ==0)
                {
                    target.ChangeArmorAutoDecay(false);
                }
                else
                {
                    target.constArmorDecayTime+=buffData.value;
                }
            break;
            default:
            break;
            case BuffType.冰冷效果:
                target.AddCold(Mathf.FloorToInt(buffData.value));
            break;
            case BuffType.对目标造成伤害后触发技能:
                target.target.OnTakeDamageAndReduceHP+=DamageTriggerSkill;
            break;
            case BuffType.受到伤害后触发技能:
                target.OnTakeDamageAndReduceHP+=DamageTriggerSkill;
            break;
            
        }
        

    }
    public void OnBuffEnd()
    {
        //执行当buff结束后XX的事件
        switch (buffData._type)
        {
            case BuffType.昏迷:
            //解除昏迷
            if(target.animState!=AnimState.dead)
            {
                target.animState = AnimState.idle;
                target.ChangeAnimatorInteger(0);
                target.RunAI();
            }
            break;
            case BuffType.影响攻击力:
            target.basicAttack-=(int)buffData.value;
            break;
            case BuffType.影响防御力:
            target.basicDefence-=(int)buffData.value;
            break;
            case BuffType.数值吸收伤害:
            //如果身上没有id为xxx的buff（护甲持续时间变永久），那么护甲类buff时间到之后将会移除护甲
                // target.AddArmor(-Mathf.FloorToInt(buffData.value));
                //不再根据单独的buff移除护甲，而是统一时间移除护甲
            break;
            case BuffType.影响闪避:
            {
                if(target.dodge-Mathf.FloorToInt(buffData.value) > 0)
                {
                    target.dodge-=Mathf.FloorToInt(buffData.value);
                }
                else
                {
                    target.dodge =0;
                }
            }
            break;
            case BuffType.影响暴击率:
                target.Crit-=buffData.value;
            break;
            case BuffType.影响全局能量消耗:
            {
                UIBattle.Instance.playerActor.cardMpReduce-=Mathf.FloorToInt(buffData.value);
                UIBattle.Instance.ReduceAllCardCost(Mathf.FloorToInt(-buffData.value));
            }
            break;
            case BuffType.影响召唤物强度:
                BuffManager.instance.SummonedRemoveBuff(this);
            break;
            case BuffType.影响召唤物攻速:
                BuffManager.instance.SummonedRemoveBuff(this);
            break;
            case BuffType.影响召唤物持续时间:
            {
                target.SummonedLifeTimePlus-=buffData.value;
                target.InvokeSummonedLifeTimeUpdate(-Mathf.FloorToInt(buffData.value));
            }
            break;
            case BuffType.影响补牌数量:
                target.dealCardsNumber-=Mathf.FloorToInt(buffData.value);
            break;
            case BuffType.影响护甲持续时间:
                if(buffData.value ==0)
                {
                    target.ChangeArmorAutoDecay(true);
                }
                else
                {
                    target.constArmorDecayTime-=buffData.value;
                }
            break;
            case BuffType.效果结束时获得buff:
                if(buffData.value >0)
                {
                    BuffManager.instance.CreateBuffForActor((int)buffData.value,target);
                }
                
            break;
            case BuffType.效果结束时释放技能:
                if(buffData.abilityID >0)
                {
                    Skill skill;
                    if(buffData.value==0)
                    skill = SkillManager.TryGetFromPool(buffData.abilityID,target);
                    else
                    skill = SkillManager.TryGetFromPool(buffData.abilityID,target.target);
                    skill.caster.OnSkillSpellFinish(skill);
                }
                
            break;
            case BuffType.引导能量:
                target.ChannelSkill(-buffData.value);
                target.OnChannelSkillManaOut-=ChannelEnd;
            break;
            case BuffType.对目标造成伤害后触发技能:
                target.target.OnTakeDamageAndReduceHP-=DamageTriggerSkill;
            break;
            case BuffType.受到伤害后触发技能:
                target.OnTakeDamageAndReduceHP-=DamageTriggerSkill;
            break;
            
        }
        
        // if(buffData.id==15)
        // {
        //     Debug.LogWarning("移除了几次？");
        // }
        int num =1;
        if(buffData.removeType ==1)//只移除自己
        {

        }
        else  if(buffData.removeType ==0)//移除所有同类buff
        {
            for (int i = 0; i < target.buffs.Count; i++)
            {
            if(target.buffs[i]!=this && target.buffs[i].buffData.id ==buffData.id)
            {
                BuffManager.RemoveBuffFromActor(target.buffs[i],target);
                num++;    
            }
        }
        
        }
        
        //引发技能
        //若技能的数值与层数相关，则使用num

        //移除自己----------??
        BuffManager.RemoveBuffFromActor(this,target);
    }
    public void OnBuffInterval()
    {

        int num =0;
        if(buffData._type ==BuffType.间隔触发技能)
        {
            //检测是否有相同ID的buff，如果有，合并计算

            for (int i = 0; i < target.buffs.Count; i++)
            {
                if(target.buffs[i].buffData.id ==buffData.id)
                {
                    num++;    
                }
            }
            Skill skill;
            if(buffData.value==0)
            skill = SkillManager.TryGetFromPool(buffData.abilityID,target);
            else
            skill = SkillManager.TryGetFromPool(buffData.abilityID,target.target);
            skill.damage *=num;
            skill.heal *=num;
            skill.caster.OnSkillSpellFinish(skill);
                // skill.ComputeDamage();
            // Battle.Instance.ReceiveSkillDamage(Mathf.CeilToInt(currentValue),target,buffData._genreList[0]);
            // Battle.Instance.ReceiveSkillDamage(skill,skill.damage*num,false,false);
                // skill.ComputeHeal();
            if(buffData.removeType==2)//每次触发移除一层
            {
                OnBuffEnd();    
            }

        }
        //合并计算后，如何跳过所有同类BUFF

        if(buffData._type ==BuffType.时间间隔触发卡牌效果)
        {
            Debug.LogWarning("触发间隔时间卡牌效果");
            Skill skill;
            if(buffData.value==0)
            skill = SkillManager.TryGetFromPool(buffData.abilityID,target);
            else
            skill = SkillManager.TryGetFromPool(buffData.abilityID,target.target);
            SkillCard.CardThrowCard(skill);
            SkillCard.CardCreateCard(skill);
            if(skill.usedChooseCard>0)
            UIBattle.Instance.SelectSomeCards(skill.usedChooseCard);
        }
        
    }
    public void ChannelEnd()
    {
        buffIcon.OnEffectEnd();
    }
    void DamageTriggerSkill(int num)
    {
        Skill skill;
        if(buffData.value==0)
        skill = SkillManager.TryGetFromPool(buffData.abilityID,target);
        else
        skill = SkillManager.TryGetFromPool(buffData.abilityID,target.target);
        skill.caster.OnSkillSpellFinish(skill);
    }
    public void RemoveSlef()
    {
        buffData =null;
        if(buffIcon!=null)
        {
            for (int i = 0; i < buffIcon.buffs.Count; i++)
            {
                if(buffIcon.buffs[i]==this)
                {
                    buffIcon.buffs.Remove(this);
                    i--;
                }
            }
            buffIcon =null;
        }
        
        if(childrenBuffs!=null)
        {
            for (int i = 0; i < childrenBuffs.Count; i++)
            {
                if(childrenBuffs[i]!=null)
                {
                    BuffManager.RemoveBuffFromActor(childrenBuffs[i],childrenBuffs[i].target);
                    childrenBuffs.Remove(childrenBuffs[i]);
                    i--;
                }
            }
            childrenBuffs =null;
        }
        
        target =null;
    }

}
