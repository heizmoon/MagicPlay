using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;
using Anima2D;
using System;
public enum BuffType
{
    昏迷 =1,
    持续伤害or治疗=2,
    影响急速 =3,
    影响暴击 =4,
    ///<summary>value =要end的buffID,abilityID=要添加的buffID</summary>
    失去所有护甲后增减buff =5,
    百分比增减受到的伤害 =6,
    百分比增减附加的伤害 =7,
    数值增减受到的伤害 =8,
    数值增减附加的伤害 =9,
    百分比反弹受到的伤害=10,
    数值反弹受到的伤害=11,
    死亡后复活并恢复百分比生命=12,
    吸收一定数量的伤害=13,
    影响召唤物持续时间 =14,
    影响召唤物强度 =15,
    影响召唤物攻速=16,
    影响暴击增益 =17,
    影响闪避 =18,
    数值吸收伤害 =19,
    百分比吸收伤害 =20,
    数值受伤回复 =21,
    影响全局能量消耗=22,
    百分比影响暴击 =23,
    影响补牌数量 =24,
    ///<summary>0=持续时间无限,其余数值为+num</summary>
    影响护甲持续时间 =25,
    效果结束时获得buff =26,
    效果结束时释放技能 =27,
    百分比影响闪避 =28,
    获得护甲后触发技能 =29,
    生命值小于百分之25时触发=30,
    成功格挡后触发技能=31

}
public class BuffManager : MonoBehaviour
{
    public static BuffManager instance;
    static Color[] colors;
    enum BuffColor
    {
        原始颜色 =0,
        燃烧 =1,
        冰冻 =2
    }
    BuffData[] buffDatas;
    public event Action<Buff> OnSummonedAddBuff;//
    public event Action<Buff> OnSummonedRemoveBuff;//
    void Awake()
    {
        instance = this;
        buffDatas = Resources.Load<BuffDataSet>("DataAssets/Buff").dataArray;
        // buffPool =GameObject.Find("BuffPool").transform;
    }
    void Start()
    {
        InitColor();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void InitColor()
    {
        colors =new Color[3];
        colors[0] =new Color(1,1,1);
        colors[1] =new Color(1,0,0);
        colors[2] =new Color(0,0,1);
    }
    ///<summary>给指定的actor增加buff</summary>
    void AddBuff(int id,Actor target)
    {

    }
    ///<summary>直接给Player增加buff</summary>
    void AddBuff(int id)
    {

    }
    //思路：Player中仅记录BUFFID，在各处需要判断时，仅判断是否拥有此BUFFID即可
    //战斗时需要显示BUFF图标，显示BUFF预设效果，然后在数值计算时判断BUFFID
    //有持续时间的buff需要实例化，用自己的timer来判断buff结束
    //哪些BUFF在战斗中会显示icon?
    //buff改变actor颜色的方式

    static void ChangeColor(Actor actor,BuffColor color)
    {
       SpriteMeshInstance[] smi = actor.GetComponentsInChildren<SpriteMeshInstance>();
       for(int i =0;i<smi.Length;i++)
       {
           smi[i].color =colors[(int)color];
       }
    }
    void TryGetBuff()
    {

    }
    void CreateBuff(int id)
    {

    }
    public Buff CreateBuffForActor(int id,Actor target)
    {
        if(target ==null)
        {
            return null;
        }
        Buff buff =new Buff();
        foreach (var item in buffDatas)
        {
            if(item.id ==id)
            {
                buff.buffData =item;
                // Debug.LogWarningFormat("错误检测：{0}",buff.buffData.name);
                buff.level =1;
                buff.currentValue =item.value+item.valueGrow;
                buff.target =target;
                if(buff.buffData.delay>0)
                {
                    StartCoroutine(WaitForAddBuff(buff));
                }
                else
                {
                    AddBuff(buff);
                }
            }
        }
        return buff;
    }
    IEnumerator WaitForAddBuff(Buff buff)
    {
        yield return new WaitForSeconds(buff.buffData.delay);
        AddBuff(buff);
    }
    void AddBuff(Buff buff)
    {
        buff.target.AddBuff(buff);
        buff.CheckChildren();
        // if(buff.buffData.groupType==1)
        // Debug.LogWarningFormat("添加了深渊遗物buff名为：{0}",buff.buffData.name);
    }
    ///<summary>直接移除buff，不触发OnBuffEnd</summary>
    public static void RemoveBuffFromActor(Buff buff,Actor actor)
    {
        actor.buffs.Remove(buff);
        if(buff.buffIcon !=null)
        {
            buff.buffIcon.buffs.Remove(buff);
        }
        // buff.RemoveSlef();
        buff =null;
        // EffectManager.TryThrowInPool(buff.buffData.triggerEffect);
        //EffectManager.TryThrowInPool(buff.buffData.prefab,actor.castPoint);
        
    }
    ///<summary>移除角色身上所有buff</summary>
    public static void RemovePlayerActorAllBuff()
    {
        for (int i = Player.instance.playerActor.buffs.Count-1; i >=0 ; i--)
        {
            BuffManager.RemoveBuffFromActor(Player.instance.playerActor.buffs[i],Player.instance.playerActor);
        }
        Debug.LogWarning("剩余buff数量："+Player.instance.playerActor.buffs.Count);
    }
    ///<summary>移除角色身上所有持续时间大于0的buff</summary>
    public static void RemovePlayerActorTempBuff()
    {
        for (int i = Player.instance.playerActor.buffs.Count-1; i >=0 ; i--)
        {
            if(Player.instance.playerActor.buffs[i].buffData.time!=0)
            BuffManager.RemoveBuffFromActor(Player.instance.playerActor.buffs[i],Player.instance.playerActor);    
        }
    }
    public void OnBuffMax(Buff buff)
    {
        //检查冻结
        // Debug.LogWarningFormat("开始判断冻结");
        // Check1012(buff);
        CheckSpecialBuff(buff,1012,new List<int>(){6},null);
    }
    // void Check1012(Buff buff)
    // {
    //     //如果buff是寒流，且玩家冻结技能等级大于0
    //     if(buff.buffData.id !=6)
    //     {
    //         return;
    //     }
    //     if(buff.target.actorType==ActorType.敌人&&Player.instance.GetSkillLevel(1012)>0)
    //     {
    //         //移除所有寒流，触发技能1012
    //         buff.buffIcon.OnEffectEnd();
    //         Skill skill = SkillManager.TryGetFromPool(1012,Player.instance.playerActor);
    //         Battle.Instance.ReceiveSkillDamage(skill,skill.damage,false);
    //         Debug.LogWarningFormat("{0}造成伤害为：{1}",skill.skillName,skill.damage);
    //     }
    //     else if(buff.target.actorType ==ActorType.玩家角色)
    //     {
    //         var temp =buff.target.target.skills;
    //         if(temp ==null)
    //         {
    //             return;
    //         }
    //         for (int i = 0; i < temp.Length; i++)
    //         {
    //             if(temp[i]!=null&&temp[i].id==1012)
    //             {
    //                 buff.buffIcon.OnEffectEnd();
    //                 Skill skill = SkillManager.TryGetFromPool(1012,Battle.Instance.enemy);
    //                 Battle.Instance.ReceiveSkillDamage(skill,skill.damage,false);
    //                 return;
    //             }
    //         }
    //     }    
    // }
    ///<summary>检查特殊buff</summary>
    public static void CheckSpecialBuff( Buff buff, int needSkill,List<int> needBuff,List<int> refuseBuff)
    {
        //buff必须是什么buff
        if(needBuff!=null && !needBuff.Contains(buff.buffData.id))
        {
            return;
        }
        //buff不能是什么buff
        if(refuseBuff!= null && refuseBuff.Contains(buff.buffData.id))
        {
            return;
        }

        if(buff.target.actorType==ActorType.敌人&&Player.instance.GetSkillLevel(needSkill)>0)
        {
            buff.buffIcon.OnEffectEnd();
            Skill skill = SkillManager.TryGetFromPool(needSkill,Player.instance.playerActor);
            Battle.Instance.ReceiveSkillDamage(skill,skill.damage,false,false);
            Debug.LogWarningFormat("{0}造成伤害为：{1}",skill.skillName,skill.damage);
        }
        else if(buff.target.actorType ==ActorType.玩家角色)
        {
            var temp =buff.target.target.skills;
            if(temp ==null)
            {
                return;
            }
            for (int i = 0; i < temp.Count; i++)
            {
                if(temp[i]!=null&&temp[i].id==needSkill)
                {
                    buff.buffIcon.OnEffectEnd();
                    Skill skill = SkillManager.TryGetFromPool(needSkill,Battle.Instance.enemy);
                    Battle.Instance.ReceiveSkillDamage(skill,skill.damage,false,false);
                    return;
                }
            }
        }

    }
    public static void CheckBuffOnReceiveSkillDamageToTriggerSkillDamage(Actor actor,int genre, BuffType buffType,bool revertTarget)
    {
        for (int i = 0; i < actor.buffs.Count; i++)
            {
                if(actor.buffs[i].buffData._type == buffType && actor.buffs[i].buffData._genreList.Contains(genre))
                {
                    //显示特效
                    Transform e = EffectManager.TryGetFromPool(actor.buffs[i].buffData.triggerEffect);
                    if(e!=null)
                    {
                        e.SetParent(actor.target.hitPoint);
                        e.localPosition =Vector3.zero;
                        e.localScale =Vector3.one;
                    }
                    
                    Skill skill = SkillManager.TryGetFromPool(actor.buffs[i].buffData.abilityID,actor);
                    if(revertTarget)
                    {
                        skill.target =skill.target.target;   
                    }
                    Battle.Instance.ReceiveSkillDamage(skill,skill.damage,true,actor.buffs[i].buffData.delay,false);
                    if(revertTarget)
                    {
                        skill.target =skill.target.target;
                    }
                }
            }
    }
    ///<summary>检查是否有特殊类型的buff用于触发技能</summary>
    public static void Check_SpecialTypeBuff_ToTriggerSkill(Actor actor,BuffType buffType)
    {
        
        for (int i = 0; i < actor.buffs.Count; i++)
            {
                if(actor.buffs[i].buffData._type == buffType)
                {
                    //显示特效
                    Transform e = EffectManager.TryGetFromPool(actor.buffs[i].buffData.triggerEffect);
                    if(e!=null)
                    {
                        e.SetParent(actor.target.hitPoint);
                        e.localPosition =Vector3.zero;
                        e.localScale =Vector3.one;
                    }
                    
                    Skill skill = SkillManager.TryGetFromPool(actor.buffs[i].buffData.abilityID,actor);
                    Debug.LogWarning("触发技能"+skill.skillName);
                    skill.caster.BeginSpell(skill);
                    // Battle.Instance.ReceiveSkillDamage(skill,skill.damage,true,actor.buffs[i].buffData.delay,false);
                    
                }
            }
    }
    public static void Check_SpecialTypeBuff_ToSetBuff(Actor actor,BuffType buffType)
    {
        
        for (int i = 0; i < actor.buffs.Count; i++)
            {
                if(actor.buffs[i].buffData._type == buffType)
                {
                    //显示特效
                    Transform e = EffectManager.TryGetFromPool(actor.buffs[i].buffData.triggerEffect);
                    if(e!=null)
                    {
                        e.SetParent(actor.target.hitPoint);
                        e.localPosition =Vector3.zero;
                        e.localScale =Vector3.one;
                    }
                    if(actor.buffs[i].buffData.abilityID!=0)
                    BuffManager.instance.CreateBuffForActor(actor.buffs[i].buffData.abilityID,actor);
                    if(actor.buffs[i].buffData.value!=0)
                    BuffManager.EndBuffFromActor(BuffManager.FindBuff((int)actor.buffs[i].buffData.value,actor),actor);
                }
            }
    }
    public void SummonedAddBuff(Buff buff)
    {
        if(OnSummonedAddBuff!=null)
        OnSummonedAddBuff(buff);
    }
        public void SummonedRemoveBuff(Buff buff)
    {
        if(OnSummonedRemoveBuff!=null)
        OnSummonedRemoveBuff(buff);
    }
    public void TryModiferBuffIconNum(int id,int num,Actor target)
    {

        BuffIcon buffIcon =  UIBattle.Instance.CheckBuffIcon(FindBuff(id,target));
        if(buffIcon)
        {
            buffIcon.textBuffNum.text = num.ToString();
        }
    }
    public static Buff FindBuff(int id,Actor target)
    {
        foreach (var item in target.buffs)
        {
            if(item.buffData.id == id)
            {
                return item;
            }
        }
        return null;
    }
    public static void EndBuffFromActor(Buff buff,Actor actor)
    {
        if(buff!=null)
        buff.buffIcon.OnEffectEnd();
    }
}
