using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;
using Anima2D;
using System;
public enum BuffType
{
    昏迷 =1,
    间隔触发技能=2,
    影响攻击力 =3,
    影响耐力 =4,
    ///<summary>value =要end的buffID,abilityID=要添加的buffID</summary>
    失去所有护甲后增减buff =5,
    百分比增减受到的伤害 =6,
    百分比增减附加的伤害 =7,
    数值增减受到的伤害 =8,
    数值增减附加的伤害 =9,
    百分比反弹受到的伤害=10,
    数值反弹受到的伤害=11,
    死亡后复活并恢复百分比生命=12,
    百分比影响护甲值=13,
    影响召唤物持续时间 =14,
    影响召唤物强度 =15,
    影响召唤物攻速=16,
    影响暴击增益 =17,
    影响闪避 =18,
    数值吸收伤害 =19,
    百分比吸收伤害 =20,
    数值受伤回复 =21,
    影响全局能量消耗=22,
    影响暴击率 =23,
    影响补牌数量 =24,
    ///<summary>0=持续时间无限,其余数值为+num</summary>
    影响护甲持续时间 =25,
    效果结束时获得buff =26,
    效果结束时释放技能 =27,
    引导能量 =28,
    获得护甲后触发技能 =29,
    生命值小于百分之25时触发=30,
    成功格挡后触发技能=31,
    时间间隔触发卡牌效果=32,
    冰冷效果=33,
    对目标造成伤害后触发技能 =34,
    受到伤害后触发技能=35,
    影响能量回复速度=36,
    技能暴击后触发技能=37,
    被攻击触发技能 =38,
    移除指定ID的BUFF=39,
    BUFF叠加到最大层触发技能=40,
    每出X张Y牌时自己触发技能=41,
    弃牌大于X时自己触发技能=42,
    每X次补牌时自己触发技能=43,
    每X次遗留时自己触发技能 =44,
    改变能量上限=45,
    每造成X次伤害触发技能=46,
    每受到X次伤害触发技能 =47,
    造成的总伤害高于x触发技能 =48,
    受到的总伤害高于x触发技能 =49,
    造成的单次伤害高于x触发技能 =50,
    受到的单次伤害高于x触发技能=51,
    获得的总护甲层数大于x触发技能 =52,
    当前护甲层数大于x获取BUFF =53,
    获得保护状态 =54,
    改变生命上限 =55,
    下一个技能无效 =56,
    改变怪物阶段 =57,
    改变角色形象 =58

    
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
    public event Action<int> OnBuffRemoveCastSkill;
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
    ///<summary>用于BUFF触发移除另一个BUFF</summary>
    public void BuffRemoveBuff(Buff buff)
    {
        StartCoroutine(IEWaitForRemoveBuff(buff));
    }
    IEnumerator IEWaitForRemoveBuff(Buff buff)
    {
        yield return new WaitForSeconds(0.1f);
        Debug.Log("开始移除BUFF:"+buff.buffData.abilityID);
        if(buff.buffData.abilityID ==33)
        {
            int i = buff.target.AddCold(-(int)buff.buffData.value);
            OnBuffRemoveCastSkill(i);    
        }
        else
        {
            Buff m_buff =FindBuff(buff.buffData.abilityID,buff.target);
            if(m_buff !=null)
            {
                if(m_buff.buffData.removeType ==0)
                {
                    m_buff.buffIcon.OnEffectEnd();
                }
                else
                {
                    int num =(int)buff.buffData.value;
                    int i =0;
                    for (int j = buff.target.buffs.Count-1; j >=0 ; j--)
                    {
                        // Debug.Log("BUFF层数"+buff.target.buffs.Count);
                        Buff _buff =buff.target.buffs[j];
                        if(_buff.buffData.id == buff.buffData.abilityID)
                        {
                            if(_buff!=null)
                            {
                                Debug.Log("移除一层"+_buff.buffData.name);
                                _buff.OnBuffEnd();
                                // Debug.Log("移除后剩余BUFF层数"+buff.target.buffs.Count);
                                i++;
                                if(_buff.buffIcon.buffs.Count==0)
                                {
                                    _buff.buffIcon.OnEffectEnd();
                                }
                                if(i == num)
                                {
                                    Debug.Log("一共移除了"+num+"层"+_buff.buffData.name);
                                    OnBuffRemoveCastSkill(i);
                                    break;
                                }
                                else if(i<num&&j==0)
                                {
                                    Debug.Log("一共移除了"+i+"层"+_buff.buffData.name);
                                    OnBuffRemoveCastSkill(i);
                                    break;
                                }
                            }
                        }    
                    }
                }
            }
        }    
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
                buff.effectInterval =item.effectInterval;
                if(buff.buffData.delay>0)
                {
                    StartCoroutine(WaitForAddBuff(buff));
                    return buff;
                }
                else
                {
                    AddBuff(buff);
                    return buff;
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
        //--------------------------------------------此处需要移除BUFF绑定的所有事件！！！！！！！！
        // buff.RemoveAllEvent();
        Debug.Log("彻底移除BUFF："+buff.buffData.name);           
        buff =null;
        // EffectManager.TryThrowInPool(buff.buffData.triggerEffect);
        //EffectManager.TryThrowInPool(buff.buffData.prefab,actor.castPoint);
        
    }
    ///<summary>移除角色身上所有buff</summary>
    public static void RemovePlayerActorAllBuff()
    {
        // for (int i = Player.instance.playerActor.buffs.Count-1; i >=0 ; i--)
        // {
        //     BuffManager.RemoveBuffFromActor(Player.instance.playerActor.buffs[i],Player.instance.playerActor);
        //     // Player.instance.playerActor.buffs[i].OnBuffEnd();
        // }
        while (Player.instance.playerActor.buffs.Count>0)
        {
            Player.instance.playerActor.buffs[0].OnBuffEnd();
        }
        Player.instance.playerActor.buffs.Clear();
        Debug.LogWarning("RemovePlayerActorAllBuff::剩余buff数量："+Player.instance.playerActor.buffs.Count);
    }
    ///<summary>移除角色身上所有的buff</summary>
    public static void RemoveActorAllBuff(Actor actor)
    {
        // for (int i = actor.buffs.Count-1; i >=0 ; i--)
        // {
        //     // if(Player.instance.playerActor.buffs[i].buffData.time!=0)
        //     BuffManager.RemoveBuffFromActor(actor.buffs[i],actor);
        //     // actor.buffs[i].OnBuffEnd();
        // }
        while (actor.buffs.Count>0)
        {
            actor.buffs[0].OnBuffEnd();
        }
        actor.buffs.Clear();
    }
    
    // public void OnBuffMax(Buff buff)
    // {
    //     //检查冻结
    //     // Debug.LogWarningFormat("开始判断冻结");
    //     // Check1012(buff);
    //     CheckSpecialBuff(buff,1012,new List<int>(){6},null);
    // }
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

        if(buff.target.actorType==ActorType.敌人)
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
                    Transform e = EffectManager.TryGetFromPool(actor.buffs[i].buffData.triggerEffect,actor.target);
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
                    Transform e = EffectManager.TryGetFromPool(actor.buffs[i].buffData.triggerEffect,actor.target);
                    if(e!=null)
                    {
                        e.SetParent(actor.target.hitPoint);
                        e.localPosition =Vector3.zero;
                        e.localScale =Vector3.one;
                    }
                    
                    Skill skill;
                    if(actor.buffs[i].buffData.value==0)
                    skill = SkillManager.TryGetFromPool(actor.buffs[i].buffData.abilityID,actor);
                    else
                    skill = SkillManager.TryGetFromPool(actor.buffs[i].buffData.abilityID,actor.target);
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
                    Transform e = EffectManager.TryGetFromPool(actor.buffs[i].buffData.triggerEffect,actor.target);
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
    public void BuffChangeActor(Buff buff)
    {
        
        string perfab =buff.buffData.triggerEffect;
        Transform target =buff.target.transform;
        string actorType ="";
        if(buff.target.actorType==ActorType.玩家角色)
        {
            actorType ="Player";
        }
        else
        {
            actorType ="Enemy";
        }
        if(Main.instance.BottomUI.Find(actorType).childCount>0)
        //如果已经处于变身状态，那么先变回去
        {
            Buff _buff =null;
            foreach (var item in buff.target.buffs)
            {
                if(item.buffData._type==BuffType.改变角色形象)
                _buff =item;
            }
            BuffManager.EndBuffFromActor(_buff,buff.target);
            
            BuffChangeActor(buff);
            return;
        }
        Transform _bone =target.Find("bone");
        Vector3 targetPosition =_bone.localPosition;
        _bone.SetParent(Main.instance.BottomUI.Find(actorType));
        Transform _body =target.Find("body");
        _body.SetParent(Main.instance.BottomUI.Find(actorType));
        _bone.localPosition =targetPosition;
        GameObject g = Instantiate((GameObject)(Resources.Load("Prefabs/"+perfab)));
        Transform bone =g.transform.Find("bone");
        Transform body = g.transform.Find("body");
        Vector3 tempScale = bone.localScale;
        Vector3 tempPosition = bone.localPosition;
        bone.SetParent(target);
        body.SetParent(target);
        bone.localPosition =tempPosition;
        bone.localScale =tempScale;
        // UnityEditorInternal.ComponentUtility.CopyComponent(g.GetComponent<Animator>());
        // UnityEditorInternal.ComponentUtility.PasteComponentValues(target.GetComponent<Animator>());
        target.GetComponent<Animator>().runtimeAnimatorController =g.GetComponent<Animator>().runtimeAnimatorController;
        DestroyImmediate(g);
        
    }
    IEnumerator WaitForChangeActor(Buff buff)
    {
        yield return new WaitForSeconds(0.1f);
        
    }
    public void BuffResumeActor(Actor _actor)
    {
        string actorType ="";
        if(_actor.actorType==ActorType.玩家角色)
        {
            actorType ="Player";
        }
        else
        {
            actorType ="Enemy";
        }
        if(Main.instance.BottomUI.Find(actorType+"/bone")==null)
        {
            return;
        }
        DestroyImmediate(_actor.transform.Find("body").gameObject);
        DestroyImmediate(_actor.transform.Find("bone").gameObject);
        
        Transform _bone =Main.instance.BottomUI.Find(actorType+"/bone");
        Vector3 orginPosition =_bone.localPosition;
        _bone.SetParent(_actor.transform);
        Transform _body =Main.instance.BottomUI.Find(actorType+"/body");
        _body.SetParent(_actor.transform);
        _bone.localPosition =orginPosition;

        _actor.GetComponent<Animator>().runtimeAnimatorController  = _actor.animatorController;
        
    }

}
