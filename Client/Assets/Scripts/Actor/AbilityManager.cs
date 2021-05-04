using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;
using System.Linq;

public class AbilityManager : MonoBehaviour
{
    public static AbilityManager instance;
    AbilityDataSet manager;
    Dictionary<int,List<int>> levelAbility; 
    ///<summary>流派，级别，技能列表</summary>
    Dictionary<int,List<int>[]> buildAbilityDic =new Dictionary<int, List<int>[]>();
    
    //已解锁的道具集合
    //不同等级的道具集合
    //在随机时从不同等级的道具集合中随机，并判断随机到的道具是否已经解锁
    public List<int> initialUnlockAbility=new List<int>();
    //已经解锁的列表
    //不会随机到角色身上已经有的道具
    List<int> allAbilityList =new List<int>();

    void Awake()
    {
        instance =this;
        manager = Resources.Load<AbilityDataSet>("DataAssets/Ability");
        
    }
    public void SeparateSkillFromLevel()
    {
        StartCoroutine(IESeparateSkillFromLevel());
    }
    IEnumerator IESeparateSkillFromLevel()
    {
        yield return new WaitForSeconds(2f);
        GetAllAbility();
        GetInitialAbility();
        for (int i = 0; i < Configs.instance.buildNumber+1; i++)//所有流派
        {
            List<int>[] list =new List<int>[5];
            list[0] = new List<int>();
            list[1] = new List<int>();
            list[2] = new List<int>();
            list[3] = new List<int>();
            list[4] = new List<int>();
            foreach (var item in manager.dataArray)//所有技能循环
            {
                foreach (var buildID in item._buildList)
                {
                    if(buildID ==i)//角色技能列表循环
                    {
                        list[item.rank].Add(item.id);
                        Debug.Log("收录--流派["+i+"]的["+item.rank+"]级别【遗物】列表：id="+item.id+","+item.name);
                    }
                }    
            }
            buildAbilityDic.Add(i,list);
        }
        //
        
    }
    void GetInitialAbility()
    {
        foreach (var item in manager.dataArray)
        {
            if(item.initialUnlock)
            {
                initialUnlockAbility.Add(item.id);
            }
        }
    }
    void Start()
    {
        SeparateSkillFromLevel();
    }
    void GetAllAbility()
    {
        foreach (var item in manager.dataArray)//所有技能循环
        {
            allAbilityList.Add(item.id);
        }
    }
    public AbilityData GetInfo(int id)
    {
       foreach(var item in manager.dataArray)
        {
            if(item.id==id)
            {
                return item;       
            } 
        }
        return null;
    }
    public string GetInfo(int id,string str)
    {
        AbilityData data =GetInfo(id);
        switch(str)
        {
            case "describe":
            return data.describe;
            case "name":
            return data.name;
            case "icon":
            return data.icon;
        }
        return "";
    }
    public List<int> GetBuildList(int id)
    {
        AbilityData data =GetInfo(id);
        return data._buildList;
    }
    ///<summary>随机N个rank级的能力</summary>
    public AbilityData[] GetRandomAbility(int N,int rank)
    {
        if(N<1)
        return null;
        AbilityData[] datas = new AbilityData[N];
        List<int> _buildList = Player.instance.playerActor.character.data._buildList;
        // 随机出本次要从那个流派中抽牌
        int[] buildIDs =SkillManager.GetRandomWeight(_buildList,SkillManager.CheckPlayerSkillBuild(),N);
        List<int> temp =new List<int>();

        for (int i = 0; i < N; i++)
        {
            List<int>[] _list = buildAbilityDic[buildIDs[i]];
            List<int> list =_list[rank];
            list.RemoveAll(it => Player.instance.playerActor.abilities.Contains(it));
            list = list.Intersect(Player.instance.unlockAbility).ToList();
            if(list.Count<3)
            Debug.LogError("流派【"+ buildIDs[i]+"】 rank 【"+rank+"】的遗物数量不足！");

            int r =UnityEngine.Random.Range(0,list.Count);
            int randomTimes =0;
            while (temp.Contains(r)&&randomTimes<4)
            {
                r =UnityEngine.Random.Range(0,list.Count);
                randomTimes++;
            }
            temp.Add(r);
            datas[i] =GetInfo(list[r]);

            Debug.Log("流派为"+buildIDs[i]+",从"+list.Count+"张牌中随机到了"+rank+"级技能："+datas[i].name);
            
        }
        return datas;
    }
    public AbilityData[] GetRandomAbilityFromSpecialPool(int N,List<int> list)
    {
       AbilityData[] abilityDatas =new AbilityData[N];
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
            abilityDatas[i] =GetInfo(list[r]);
        }
        return abilityDatas;
    }
///<summary>随机N个X级能力，不包含玩家已拥有的能力，能力必须已经解锁</summary>
public AbilityData[] GetRandomAbilityFromLevel(int number,int type)
{
    List<int> list =new List<int>();
        AbilityData[] datas = new AbilityData[number];
        List<int> ids =Player.instance.playerActor.abilities;
        List<int> unlocks = Player.instance.unlockAbility;
        //排除传入的ids中已有的id
        if(ids.Count>0)
        {
            foreach (var outer in ids)
            {
                foreach (var inner in levelAbility[type])
                {
                    if(outer != inner&&unlocks.Contains(inner))
                    {
                        list.Add(inner);
                    }
                }
            }
        }
        else
        {
            foreach (var inner in levelAbility[type])
                {
                    if(unlocks.Contains(inner))
                    list.Add(inner);
                }
        }
        
        for (int i = 0; i < number; i++)
        {
            int r = Random.Range(0,list.Count);
            List<int> temp =new List<int>();
            while(temp.Contains(r))
            {
                r = Random.Range(0,list.Count);
            }
            temp.Add(r);
            Debug.Log("I="+i+",number="+number+",r="+r+",list.count="+list.Count);
            datas[i] =GetInfo(list[r]);
            
        }

        return datas;
    }
    ///<summary>从所有未解锁的遗物中随机N个</summary>
    public AbilityData[] GetRandomAbilityFromLockAbility(int N)
    {
        AbilityData[] abilities = new AbilityData[N];
        if(N<1)
        return null;
        List<int> list =allAbilityList;
        list = list.Except(Player.instance.unlockAbility).ToList();//获取当前所有未解锁的技能
        if(list.Count==0)
        return null;
        if(N>list.Count)
        N=list.Count;
        for(int i =0;i<N;i++)
        {
            int r =UnityEngine.Random.Range(1,list.Count);
            int randomTimes =0;
            while (list.Contains(r)&&randomTimes<4)
            {
                r =UnityEngine.Random.Range(1,list.Count);
                randomTimes++;
            }
            list.Add(r);
            abilities[i] =GetInfo(list[r]);
        }
        return abilities;
    }

}

