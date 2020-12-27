using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;
using Anima2D;
public enum BuffType
{
    昏迷 =1,
    持续伤害or治疗=2,
    影响急速 =3,
    影响暴击 =4,
    影响命中 =5,
    百分比增减受到的伤害 =6,
    百分比增减附加的伤害 =7,
    数值增减受到的伤害 =8,
    数值增减附加的伤害 =9,
    百分比反弹受到的伤害=10,
    数值反弹受到的伤害=11,
    死亡后复活并恢复百分比生命=12,
    吸收一定数量的伤害=13,
    影响韧性 =14,
    影响抗性 =15,
    影响穿透=16,
    影响暴击增益 =17,
    影响闪避 =18,
    数值吸收伤害 =19,
    百分比吸收伤害 =20,
    数值受伤回复 =21,
    百分比影响急速=22,
    百分比影响暴击 =23,
    百分比影响命中 =24,
    百分比影响韧性 =25,
    百分比影响抗性 =26,
    百分比影响穿透 =27,
    百分比影响闪避 =28

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
        if(buff.buffData.groupType==1)
        Debug.LogWarningFormat("添加了深渊遗物buff名为：{0}",buff.buffData.name);
    }
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
    public static void RemovePlayerActorAllBuff()
    {
        for (int i = Player.instance.playerActor.buffs.Count-1; i >=0 ; i--)
        {
            BuffManager.RemoveBuffFromActor(Player.instance.playerActor.buffs[i],Player.instance.playerActor);
        }
    }
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
    
}
