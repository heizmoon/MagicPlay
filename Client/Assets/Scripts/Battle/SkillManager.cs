using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;
///<summary>被动技能加成类型</summary>
public enum SkillBuffType
{
    伤害,
    命中,
    急速,
    暴击,
    暴击加成,
    穿透
}

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;
    static Transform ts;
    AbilityDataSet manager;
    //每一系的总等级
    //0-8:水，火，风，土，心灵，能量，物质,时空，真理
    public int[] totalLevel=new int[]{0,0,0,0,0,0,0,0,0};
    void Awake()
    {
        instance =this;
        ts = GameObject.Find("SkillPool").transform;
        manager = Resources.Load<AbilityDataSet>("DataAssets/Ability");
    }
    public static Skill TryGetFromPool(int id,Actor actor)
    {
        Skill skill =null;
        if(id ==0)
        {
            return skill;
        }
        Skill[] skills = ts.GetComponentsInChildren<Skill>();
        foreach (var item in skills)
        {
            if(item.id ==id&&item.caster==actor)
            {
                skill =item;
                return skill;
            }
        }
        skill =((GameObject)Instantiate(Resources.Load("Prefabs/Skill"))).GetComponent<Skill>();
        skill.transform.SetParent(ts);
        skill.InitSkill(id,actor);
        return skill;
    }
    public static void ClearPool()
    {
        Skill[] skills = ts.GetComponentsInChildren<Skill>();
        int temp =skills.Length;
        for (int i =temp-1 ; i >=0 ; i--)
        {
            // Debug.LogWarningFormat("Destory:{0}",skills[i].skillName);
            DestroyImmediate(skills[i].gameObject);
        }
        // Debug.LogWarningFormat("!!!ClearSkillPool:剩余数目{0}",ts.childCount);
    }
    ///<summary>获取技能</summary>
    public  Ability GetInfo(int id)
    {
        foreach (var item in manager.dataArray)
        {
            if(item.id == id)
            {
                return item;
            }
        }
        return null;
    }
    public string GetInfo(int id ,string content)
    {
        foreach(var item in manager.dataArray)
        {
            if(item.id==id)
            {
                switch(content)
                {
                    case "name":
                    // Debug.LogFormat("内容:{0}",item.name);
                    return item.name;
                    case "discribe":
                    // Debug.LogFormat("内容:{0}",item.discribe);
                    break;
                    case "genre":
                    return item.genre.ToString();
                    case "icon":
                    return item.icon;
                    case "ifActive":
                    // Debug.LogFormat("内容:{0}",item.name);
                    return item.ifActive.ToString();
                    
                }     
            }    
        }
        return "ok";
    }
    ///<summary>检查一个技能是否能够触发另一个技能</summary>
    ///<param name ="skill">传入的技能</param>
    ///<param name ="skillID">想要触发的新技能ID</param>
    ///<param name ="refuseSkills">如果命中的技能是这些之一则不能触发</param>
    ///<param name ="needGenres">命中的技能必须是这些派系之一</param>
    ///<param name ="refuseBuffs">目标身上不能携带这些buff</param>
    ///<param name ="needBuffs">目标身上必须携带这些buff</param>
    public static void CheckSkillToTriggerNewSkill(Skill skill,int skillID, List<int> refuseSkills,List<int> needGenres,List<int> refuseBuffs,List<int> needBuffs,bool AndOr)
    {
        //不能触发的技能列表
        if(refuseSkills!= null&&refuseSkills.Contains(skill.id))
        {
            return;
        }
        //必须是X系技能
        if(needGenres!=null &&!needGenres.Contains(skill.genre))
        {
            return;
        }
        //目标身上有某些Buff时不能添加
        if(refuseBuffs != null)
        {
            for (int i = 0; i < skill.target.buffs.Count; i++)
            {
                for (int j = 0; j < refuseBuffs.Count; j++)
                {
                    if(skill.target.buffs[i].buffData.id == refuseBuffs[j])
                    {
                        return;
                    }
                }
                
            }
        }
        //目标身上必须要有这些buff之一
        if(needBuffs != null)
        {
            int num =0;
            for (int i = 0; i < skill.target.buffs.Count; i++)
            {
                for (int j = 0; j < needBuffs.Count; j++)
                {
                    if(skill.target.buffs[i].buffData.id == needBuffs[j])
                    {
                        num++;
                    }
                }    
            }
            if(AndOr)
            {
                if(num !=needBuffs.Count)
                {
                    return;
                }
            }
            else
            {
                if(num ==0)
                {
                    return;
                }
            }
        }
        if(skill.target.actorType==ActorType.敌人)
        {
            //检测技能等级
            int level = Player.instance.GetSkillLevel(skillID);
            if(level>0)
            {
                Skill sk = TryGetFromPool(skillID,skill.caster);
                SkillManager.instance.SimulateCastSkill(sk,sk.damage,false,true);
            }
        }
        if(skill.target.actorType ==ActorType.玩家角色)
        {
            var tempskills =skill.target.skills;
            if(tempskills==null)
            {
                return;
            }
            for (int i = 0; i < tempskills.Length; i++)
            {
                if(tempskills[i]!=null&&tempskills[i].id ==skillID)
                {
                    Skill sk = TryGetFromPool(skillID,skill.caster);
                    SkillManager.instance.SimulateCastSkill(sk,sk.damage,false,true);
                    return;
                }
            }
        }
    }
    ///<summary>当技能命中时，检查是否可以触发某个特定技能的buff</summary>
    ///<param name ="skill">命中的技能</param>
    ///<param name ="skillID">要检测是否触发的技能ID</param>
    ///<param name ="buffID">技能触发后会添加的buffID</param>
    ///<param name ="refuseSkills">如果命中的技能是这些之一则不能触发</param>
    ///<param name ="needGenres">命中的技能必须是这些派系之一</param>
    ///<param name ="refuseBuffs">目标身上不能携带这些buff</param>
    public static void CheckSkillOnHitToAddBuff(Skill skill,int skillID, int buffID,List<int> refuseSkills,List<int> needGenres,List<int> refuseBuffs)
    {
        // Debug.LogWarningFormat("{0}经过第0次判断:不能触发的技能{1}，需要的派系{2}",skillID,refuseSkills[0],needGenres[0]);
        //不能触发的技能列表
        if(refuseSkills!= null&&refuseSkills.Contains(skill.id))
        {
            return;
        }
        //必须是X系技能
        if(needGenres!=null &&!needGenres.Contains(skill.genre))
        {
            return;
        }
        //目标身上有某些Buff时不能添加
        if(refuseBuffs != null)
        {
            for (int i = 0; i < skill.target.buffs.Count; i++)
            {
                for (int j = 0; j < refuseBuffs.Count; j++)
                {
                    if(skill.target.buffs[i].buffData.id == refuseBuffs[j])
                    {
                        return;
                    }
                }
                
            }
        }
        if(skill.target.actorType==ActorType.敌人)
        {
            //检测技能等级
            int level = Player.instance.GetSkillLevel(skillID);
            if(level>0)
            {
                //给技能目标添加效果
                Buff buff = BuffManager.instance.CreateBuffForActor(buffID,level,skill.target);
                Debug.LogWarningFormat("{0}技能附加buff:{1}",skill.skillName,buff.buffData.name);
            }

        }
        if(skill.target.actorType ==ActorType.玩家角色)
        {
            var tempskills =skill.target.skills;
            if(tempskills==null)
            {
                return;
            }
            for (int i = 0; i < tempskills.Length; i++)
            {
                if(tempskills[i]!=null&&tempskills[i].id ==skillID)
                {
                    BuffManager.instance.CreateBuffForActor(buffID,tempskills[i].level,skill.target);
                    return;
                }
            }
        }
        
    }
    ///<summary>在计算伤害时，检查某个技能是否可以获得另一个被动技能的加成</summary>
    ///<param name ="skill">想要获得加成的技能</param>
    ///<param name ="skillID">被动技能ID</param>
    ///<param name ="needBuff">目标身上必须有这些BUFF</param>
    ///<param name ="refuseSkills">如果技能是这些之一则不能获得加成</param>
    ///<param name ="needGenres">技能必须是这些派系之一</param>
    ///<param name ="refuseBuffs">目标身上不能携带这些buff</param>
    ///<param name ="skillBuffType">被动加成的类型</param>
    public static int CheckSkillOnComputeToGiveAdd(Skill skill,int skillID, List<int> needBuff,bool AndOr,List<int> refuseSkills,List<int> needGenres,List<int> refuseBuffs,SkillBuffType skillBuffType)
    {
        
        //不能触发的技能列表
        if(refuseSkills!= null&&refuseSkills.Contains(skill.id))
        {
            return 0;
        }
        //必须是X系技能
        if(needGenres!=null &&!needGenres.Contains(skill.genre))
        {
            return 0;
        }
        //目标身上有某些Buff时不能添加
        if(refuseBuffs != null)
        {
            for (int i = 0; i < skill.target.buffs.Count; i++)
            {
                for (int j = 0; j < refuseBuffs.Count; j++)
                {
                    if(skill.target.buffs[i].buffData.id == refuseBuffs[j])
                    {
                        return 0;
                    }
                }    
            }
        }
        //目标身上必须有某些buff
        if(needBuff != null)
        {   
            int num =0;
            for (int i = 0; i < skill.target.buffs.Count; i++)
            {
                for (int j = 0; j < needBuff.Count; j++)
                {
                    if(skill.target.buffs[i].buffData.id == needBuff[j])
                    {
                        num++;
                    }
                }    
            }
            if(AndOr)
            {
                if(num !=needBuff.Count)
                {
                    return 0;
                }
            }
            else
            {
                if(num ==0)
                {
                    return 0;
                }
            }
            
        }
        if(skill.target.actorType==ActorType.敌人)
        {
            //检测技能等级
            int level = Player.instance.GetSkillLevel(skillID);
            if(level>0)
            {
               Skill sk = TryGetFromPool(skillID,skill.caster);
            //    Debug.LogWarningFormat("-------{0}:",sk.skillName);
            //    Battle.Instance.ReceiveSkillDamage(sk,sk.damage,false);
                switch(skillBuffType)
                {
                    case SkillBuffType.伤害:
                    return sk.damage;
                    case SkillBuffType.命中:
                    return sk.hit;
                    case SkillBuffType.急速:
                    return sk.fast;
                    case SkillBuffType.暴击:
                    return sk.crit;
                    case SkillBuffType.穿透:
                    return sk.seep;
                    case SkillBuffType.暴击加成:
                    return sk.damage;
                }
                
            }
        }
        if(skill.target.actorType ==ActorType.玩家角色)
        {
            var tempskills =skill.target.skills;
            if(tempskills==null)
            {
                return 0;
            }
            for (int i = 0; i < tempskills.Length; i++)
            {
                if(tempskills[i]!=null&&tempskills[i].id ==skillID)
                {
                    Skill sk = TryGetFromPool(skillID,skill.caster);
                    // Battle.Instance.ReceiveSkillDamage(sk,sk.damage,false);
                    return sk.damage;
                }
            }
        }
        return 0;
    }
    ///<summary>用于模拟触发一次额外技能</summary>
    public  void SimulateCastSkill(Skill skill,int damage,bool ifRebound,bool ifShow)
    {
        if(ifShow)
        {
           StartCoroutine(WaitForSimulateEffect(skill.orginSpellTime,skill.hitEffect,skill.target));
        }
        Battle.Instance.ReceiveSkillDamage(skill,damage,ifRebound,skill.orginSpellTime);
    }
    IEnumerator WaitForSimulateEffect(float delay,string effectName,Actor actor)
    {
        yield return new WaitForSeconds(delay);
        Transform e =EffectManager.TryGetFromPool(effectName);
        if(e!=null)
        {
            e.SetParent(actor.hitPoint);
            e.localPosition =Vector3.zero;
            e.localScale =Vector3.one;
            e.gameObject.SetActive(true);
        }
    }
    ///<summary>技能——引火烧身</summary>
    public void Check1117(Skill skill)
    {
        int buffID =4;
        if(skill.id!=1117)
        {
            return;
        }
        //检查目标身上的点燃层数   
        int num =0;
        for (int i = 0; i < skill.target.buffs.Count; i++)
        {
            
            if(skill.target.buffs[i].buffData.id == buffID)
            {
                num++;
            }
               
        }
        //给目标添加层数*2的点燃
        for (int i = 0; i < num*2; i++)
        {
            BuffManager.instance.CreateBuffForActor(buffID,skill.level,skill.target);
        }
        //给自己添加层数的点燃
        for (int i = 0; i < num; i++)
        {
            BuffManager.instance.CreateBuffForActor(buffID,skill.level,skill.caster);  
        }
        Debug.LogWarningFormat("----------引火烧身附加目标点燃{0}层，自身{1}层--------",num*2,num);

    }
    
}
