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
    public List<Buff> childrenBuffs;
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
            BuffManager.instance.CreateBuffForActor(item,target);
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
        if(buffData._type ==BuffType.昏迷)
        {
            //昏迷
            target.animState =AnimState.dizzy;
            target.StopCasting();
            target.ChangeAnimatorInteger(4);
        }
        if(buffData._type == BuffType.数值吸收伤害)
        {
            target.armor+=Mathf.FloorToInt(buffData.value);
        }
        if(buffData._type == BuffType.影响闪避)
        {
            target.dodge+=Mathf.FloorToInt(buffData.value);
        }
        if(buffData._type == BuffType.影响全局能量消耗)
        {
            Debug.Log("影响全局能量消耗");
            UIBattle.Instance.playerActor.cardMpReduce+=Mathf.FloorToInt(buffData.value);
            UIBattle.Instance.ReduceAllCardCost(Mathf.FloorToInt(buffData.value));
        }
        if(buffData._type == BuffType.影响召唤物强度)
        {
            BuffManager.instance.SummonedAddBuff(this);
        }
        if(buffData._type == BuffType.影响召唤物攻速)
        {
            BuffManager.instance.SummonedAddBuff(this);
        }
        if(buffData._type == BuffType.影响召唤物持续时间)
        {
            Debug.Log("影响召唤物持续时间");
            target.SummonedLifeTimePlus+=buffData.value;
            target.InvokeSummonedLifeTimeUpdate(Mathf.FloorToInt(buffData.value));
        }

    }
    public void OnBuffEnd()
    {
        //执行当buff结束后XX的事件
        if(buffData._type ==BuffType.昏迷)
        {
            //解除昏迷
            if(target.animState!=AnimState.dead)
            {
                target.animState = AnimState.idle;
                target.ChangeAnimatorInteger(0);
                target.RunAI();
            }
        }
        //如果身上没有id为xxx的buff（护甲持续时间变永久），那么护甲类buff时间到之后将会移除护甲
        
        if(buffData._type == BuffType.数值吸收伤害)
        {
            target.AddArmor(-Mathf.FloorToInt(buffData.value));
        }

        if(buffData._type == BuffType.影响闪避)
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
        if(buffData._type == BuffType.影响全局能量消耗)
        {
            UIBattle.Instance.playerActor.cardMpReduce-=Mathf.FloorToInt(buffData.value);
            UIBattle.Instance.ReduceAllCardCost(Mathf.FloorToInt(-buffData.value));
        }
        if(buffData._type == BuffType.影响召唤物强度)
        {
            BuffManager.instance.SummonedRemoveBuff(this);
        }
        if(buffData._type == BuffType.影响召唤物攻速)
        {
            BuffManager.instance.SummonedRemoveBuff(this);
        }
        if(buffData._type == BuffType.影响召唤物持续时间)
        {
            target.SummonedLifeTimePlus-=buffData.value;
            target.InvokeSummonedLifeTimeUpdate(-Mathf.FloorToInt(buffData.value));
        }


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

        //移除自己
        BuffManager.RemoveBuffFromActor(this,target);
    }
    public void OnBuffInterval()
    {
        int num =0;
        if(buffData._type ==BuffType.持续伤害or治疗)
        {
            //检测是否有相同ID的buff，如果有，合并计算
            for (int i = 0; i < target.buffs.Count; i++)
            {
                if(target.buffs[i].buffData.id ==buffData.id)
                {
                    num++;    
                }
            }
            Skill skill = SkillManager.TryGetFromPool(buffData.abilityID,target.target); 
            
            // Battle.Instance.ReceiveSkillDamage(Mathf.CeilToInt(currentValue),target,buffData._genreList[0]);
            Battle.Instance.ReceiveSkillDamage(skill,skill.damage*num,false,false);
            
        }
        
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
