using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;
using System;
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
    SkillDataSet manager;
    //每一系的总等级
    //0-8:水，火，风，土，心灵，能量，物质,时空，真理
    public int[] totalLevel=new int[]{0,0,0,0,0,0,0,0,0};
    public List<int> unlockSkills =new List<int>();
    // public Dictionary<int,List<int>> rankSkills =new Dictionary<int, List<int>>();
    // struct CharSkillList
    // {
    //    public int rank;
    //    public List<int> skills;
    // }
    ///<summary>职业，级别，技能列表</summary>
    Dictionary<int,List<int>[]> charSkillDic =new Dictionary<int, List<int>[]>();
    //装备牌技能列表
    List<int> equipCardsList =new List<int>();
    ///<summary>（buildID,List(skillid)[type]）</summary>
    Dictionary<int,List<int>[]> typeSkillDic =new Dictionary<int, List<int>[]>();

    void Awake()
    {
        instance =this;
        ts = GameObject.Find("SkillPool").transform;
        manager = Resources.Load<SkillDataSet>("DataAssets/Skill");
    }
    void Start()
    {
    }
        //将各个角色的技能按照级别分列表储存
    public void SeparateSkillFromLevel()
    {
        StartCoroutine(IESeparateSkillFromLevel());
    }
    IEnumerator IESeparateSkillFromLevel()
    {
        yield return new WaitForSeconds(2f);
        for (int i = 0; i < CharacterManager.instance.characters.Count; i++)//所有职业循环
        {
            List<int>[] list =new List<int>[5];
            list[0] = new List<int>();
            list[1] = new List<int>();
            list[2] = new List<int>();
            list[3] = new List<int>();
            list[4] = new List<int>();
            foreach (var item in manager.dataArray)//所有技能循环
            {
                if(CharacterManager.instance.characters[i].allSkillsList.Contains(item.id) )//角色技能列表循环
                {
                    list[item.rank].Add(item.id);
                    // Debug.Log("item.rank="+item.rank+",item.id="+item.id);
                }
            }
            charSkillDic.Add(i,list);
        }
        Debug.Log("charSkillDic[0].count="+charSkillDic[0].Length);
        Debug.Log("charSkillDic[0][0].count="+charSkillDic[0][0].Count);
        //
        IESeparateSkillFromType();

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
        return CreateSkillForActor(id,actor);
    }
    public static Skill CreateSkillForActor(int id,Actor actor)
    {
        Skill skill =null;
        if(id ==0)
        {
            return skill;
        }
        skill =((GameObject)Instantiate(Resources.Load("Prefabs/Skill"))).GetComponent<Skill>();
        skill.transform.SetParent(ts);
        skill.InitSkill(id,actor);
        return skill;
    }
    public static Skill TryGetFromPool(int id,Summoned summoned)
    {
        Skill skill =null;
        if(id ==0)
        {
            return skill;
        }
        skill =((GameObject)Instantiate(Resources.Load("Prefabs/Skill"))).GetComponent<Skill>();
        skill.transform.SetParent(ts);
        skill.InitSkill(id,summoned);
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
    public SkillData GetInfo(int id)
    {
        
        foreach (var item in manager.dataArray)
        {
            if(item.id == id)
            {
                return item;
            }
        }
        Debug.LogWarning("没有随到合适的技能");
        return null;
    }
    
    public delegate List<int> Mydegete();
    public delegate List<int> skillDelegate(int i);
    public List<int> GetLockedPassiveSkills()
    {
        return null;
    }
    public List<int> GetUnlockPassiveSkills()
    {
        return null;
    }
    public List<int> GetLockedActiveSkills()
    {
        return null;
    }
    public List<int> GetUnlockActiveSkills()
    {
        return null;
    }
    public List<int> GetActerNumberSkills(int actorNumber)
    {

        return null;
    }
    //所有职业的类型牌List
    void IESeparateSkillFromType()
    {
        for (int i = 0; i < 9; i++)//所有流派
        {
            List<int>[] list =new List<int>[5];
            list[0] = new List<int>();
            list[1] = new List<int>();
            list[2] = new List<int>();
            list[3] = new List<int>();
            // list[4] = new List<int>();
            foreach (var item in manager.dataArray)//所有技能循环
            {
                // if(CharacterManager.instance.characters[i].allSkillsList.Contains(item.id) )//角色技能列表循环
                if(item.buildID ==i)//角色技能列表循环
                {
                    list[item.type].Add(item.id);
                    Debug.Log("收录--流派["+i+"]的"+item.type+"系牌列表：item.id="+item.id+","+item.name);
                }
            }
            typeSkillDic.Add(i,list);
        }
    }
    public int GetRandomEquipCard()
    {
        int r =UnityEngine.Random.Range(1,equipCardsList.Count);
        return r;
    }
    public int GetRandomSkillByType(int charID,int type)
    {
        int r =UnityEngine.Random.Range(0,typeSkillDic[charID][type].Count);
        r=typeSkillDic[charID][type][r];
        return r;
    }
    // public SkillData[] GetRandomSkills(int N,Mydegete mydegete)
    // {
    //     SkillData[] skillDatas =new SkillData[N];
    //     List<int> list = mydegete.Invoke();
    //     if(N<1)
    //     return null;
    //     for(int i =0;i<list.Count;i++)
    //     {
    //         int r =UnityEngine.Random.Range(0,list.Count);
    //         skillDatas[i] =GetInfo(r);
    //     }
    //     return skillDatas;
    // }
    ///<summary>随机获得N个不重复的本职业技能</summary>
    public SkillData[] GetRandomSelfSkills(int N)
    {
        SkillData[] skillDatas =new SkillData[N];
        List<int> list = Player.instance.playerActor.character.allSkillsList;
        if(N<1)
        return null;
        List<int> temp =new List<int>();
        for(int i =0;i<N;i++)
        {
            int r =UnityEngine.Random.Range(1,list.Count);
            while (temp.Contains(r))
            {
                r =UnityEngine.Random.Range(1,list.Count);
            }
            temp.Add(r);
            skillDatas[i] =GetInfo(list[r]);
            Debug.Log("随机到的技能ID是："+skillDatas[i].id+",技能名是:"+skillDatas[i].name);
        }
        return skillDatas;
    }
    //随机获得N个不重复的本职业级别A的技能
    public SkillData[] GetRandomSelfSkillsLevelLimit(int N,int rank)
    {
        if(N<1)
        return null;
       SkillData[] skillDatas =new SkillData[N];
        // 首先先获取本职业都有哪些流派
        List<int> _buildList = Player.instance.playerActor.character.data._buildList;
        // 随机出本次要从那个流派中抽牌
        int[] buildIDs =GetRandomWeight(_buildList,CheckPlayerSkillBuild(),N);
        List<int> temp =new List<int>();

        for (int i = 0; i < N; i++)
        {
            List<int>[] _list = typeSkillDic[buildIDs[i]];
            List<int> list =_list[rank];
    
            int r =UnityEngine.Random.Range(0,list.Count);
            int randomTimes =0;
            while (temp.Contains(r)&&randomTimes<4)
            {
                r =UnityEngine.Random.Range(0,list.Count);
                randomTimes++;
            }
            temp.Add(r);
            skillDatas[i] =GetInfo(list[r]);
            Debug.Log("流派为"+buildIDs[i]+",从"+list.Count+"张牌中随机到了"+rank+"级技能："+skillDatas[i].name);
            
        }
        
        return skillDatas;
    }
    int[] GetRandomWeight(List<int> _buildList,Dictionary<int,int> playerBuildDic,int N)
    {
        List<int> _list =new List<int>();
        foreach (var item in _buildList)
        {
            if(playerBuildDic.ContainsKey(item))
            _list.Add(1+playerBuildDic[item]);
            else
            _list.Add(1);
        }
        List<int> weightList =new List<int>();
        int totalWeight =0;
        foreach (var item in _list)
        {
            weightList.Add(item+totalWeight);
            totalWeight+=item;
        }
        int[] buildIDs = new int[N];
        //__buildList = 0,1,2,3; weightList =4,5,6,10
        for (int j = 0; j < N; j++)
        {
            int r = UnityEngine.Random.Range(0,totalWeight+1);
            if(r<weightList[0])
            {
                buildIDs[j] = _buildList[0];
                Debug.Log("r="+r+",buildIDs["+j+"]= _buildList["+0+"]="+_buildList[0]);
            }
            else
            {
                for(int i =1;i<weightList.Count;i++)
                {
                    if (r>weightList[i-1]&&r<=weightList[i])
                    {
                        buildIDs[j] = _buildList[i];
                        Debug.Log("r="+r+",buildIDs["+j+"]= _buildList["+i+"]="+_buildList[i]);
                    }
                }
            }    
        }
        Debug.LogError("流派随机出现问题！");
        return buildIDs;

    }
    ///<summary>从特定的池子中随机出N个不重复的技能</summary>
    public SkillData[] GetRandomSkillFromSpecialPool(int N,List<int> list)
    {
       SkillData[] skillDatas =new SkillData[N];
        // List<int> list = Player.instance.playerActor.character.allSkillsList;
        if(N<1)
        return null;
        List<int> temp =new List<int>();
        for(int i =0;i<N;i++)
        {
            int r =UnityEngine.Random.Range(1,list.Count);
            while (temp.Contains(r))
            {
                r =UnityEngine.Random.Range(1,list.Count);
            }
            temp.Add(r);
            skillDatas[i] =GetInfo(list[r]);
        }
        return skillDatas;
    }
    // public Ability[] GetRandomUnlockPassiveSkills(int N)
    // {
    //     Ability[] abilities =new Ability[N];
    //     if(N<1)
    //     return null;
    //     for(int i =0;i<unlockSkills.Count;i++)
    //     {
    //         int r =UnityEngine.Random.Range(0,unlockSkills.Count-1);
    //         abilities[i] =GetInfo(r);
    //     }
    //     return abilities;
    // }
  /*获取技能卡的规则：
  根据当前拥有的牌，选出最适合组成BUILD的牌
  计算出当前牌的Build值
  例如 
    牌  Build值
    A   1
    B   1
    C   2
    1:2,2:1
    3:2:1:..
    核心牌
    每张牌从哪个流派列表中随机
    list 12345 weight 
    → 
  */
    public Dictionary<int,int> CheckPlayerSkillBuild()
    {
        Dictionary<int,int> playerBuildDic =new Dictionary<int, int>();
        for (int i = 0; i < Player.instance.playerActor.UsingSkillsID.Count; i++)
        {
            int _key =int.Parse (GetInfo(Player.instance.playerActor.UsingSkillsID[i],"buildID"));
            if(_key==0)
            continue;
            if(playerBuildDic.ContainsKey(_key))
            playerBuildDic[_key] += Configs.instance.cardWeightAddition;
            else
            playerBuildDic.Add(_key,Configs.instance.cardWeightAddition);
        }
        return playerBuildDic;
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
                    case "describe":
                    // Debug.LogFormat("内容:{0}",item.describe);
                    break;
                    case "type":
                    return item.type.ToString();
                    case "icon":
                    return item.icon;
                    case "ifActive":
                    // Debug.LogFormat("内容:{0}",item.name);
                    return item.ifActive.ToString();
                    case "rank":
                    return item.rank.ToString();
                    case "buildID":
                    return item.buildID.ToString();
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
        if(needGenres!=null &&!needGenres.Contains(skill.type))
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
        
        var tempskills =skill.caster.skills;
        if(tempskills==null)
        {
            return;
        }
        for (int i = 0; i < tempskills.Count; i++)
        {
            if(tempskills[i]!=null&&tempskills[i].id ==skillID)
            {
                Skill sk = TryGetFromPool(skillID,skill.caster);
                SkillManager.instance.SimulateCastSkill(sk,sk.damage,false,true);
                return;
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
    // public static void CheckSkillOnHitToAddBuff(Skill skill,int skillID, int buffID,List<int> refuseSkills,List<int> needGenres,List<int> refuseBuffs)
    public static void CheckSkillOnHitToAddBuff(Skill skill,int skillID, int buffID)

    {
        // Debug.LogWarningFormat("{0}经过第0次判断:不能触发的技能{1}，需要的派系{2}",skillID,refuseSkills[0],needGenres[0]);
        //不能触发的技能列表
        // if(refuseSkills!= null&&refuseSkills.Contains(skill.id))
        // {
        //     return;
        // }
        //必须是X系技能
        // if(needGenres!=null &&!needGenres.Contains(skill.color))
        // {
        //     return;
        // }
        //目标身上有某些Buff时不能添加
        // if(refuseBuffs != null)
        // {
        //     for (int i = 0; i < skill.target.buffs.Count; i++)
        //     {
        //         for (int j = 0; j < refuseBuffs.Count; j++)
        //         {
        //             if(skill.target.buffs[i].buffData.id == refuseBuffs[j])
        //             {
        //                 return;
        //             }
        //         }
                
        //     }
        // }
        
        var tempskills =skill.caster.skills;
        if(tempskills==null)
        {
            return;
        }
        for (int i = 0; i < tempskills.Count; i++)
        {
            if(tempskills[i]!=null&&tempskills[i].id ==skillID)
            {
                BuffManager.instance.CreateBuffForActor(buffID,skill.target);
                return;
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
        if(needGenres!=null &&!needGenres.Contains(skill.type))
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
        
        var tempskills =skill.target.skills;
        if(tempskills==null)
        {
            return 0;
        }
        for (int i = 0; i < tempskills.Count; i++)
        {
            if(tempskills[i]!=null&&tempskills[i].id ==skillID)
            {
                Skill sk = TryGetFromPool(skillID,skill.caster);
                // Battle.Instance.ReceiveSkillDamage(sk,sk.damage,false);
                return sk.damage;
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
        Battle.Instance.ReceiveSkillDamage(skill,damage,ifRebound,skill.orginSpellTime,skill.ifSeep);
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
            BuffManager.instance.CreateBuffForActor(buffID,skill.target);
        }
        //给自己添加层数的点燃
        for (int i = 0; i < num; i++)
        {
            BuffManager.instance.CreateBuffForActor(buffID,skill.caster);  
        }
        Debug.LogWarningFormat("----------引火烧身附加目标点燃{0}层，自身{1}层--------",num*2,num);

    }
    
    
}
