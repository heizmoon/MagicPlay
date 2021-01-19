using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

public class AbilityManager : MonoBehaviour
{
    public static AbilityManager instance;
    AbilityDataSet manager;
    Dictionary<int,List<int>> levelAbility; 
    
    //已解锁的道具集合
    //不同等级的道具集合
    //在随机时从不同等级的道具集合中随机，并判断随机到的道具是否已经解锁
    //不会随机到角色身上已经有的道具

    void Awake()
    {
        instance =this;
        manager = Resources.Load<AbilityDataSet>("DataAssets/Ability");
        
    }
    void Start()
    {
        levelAbility =new Dictionary<int, List<int>>();
        //分类别
        List<int> level0 = new List<int>();
        List<int> level1 = new List<int>();
        List<int> level2 = new List<int>();


        foreach (var item in manager.dataArray)
        {
            if(item.level ==0)
            {
                level0.Add(item.id);
            }
            else if(item.level ==1)
            {
                level1.Add(item.id);
            }
            else if(item.level ==2)
            {
                level2.Add(item.id);
            }
        }
        levelAbility.Add(0,level0);
        levelAbility.Add(1,level1);
        levelAbility.Add(2,level2);

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
    ///<summary>随机N个能力，不包含列表中的能力</summary>
    public AbilityData[] GetRandomAbility(int number,List<int> ids)
    {
        List<int> list =new List<int>();
        AbilityData[] datas = new AbilityData[number];
        //排除传入的ids中已有的id
        if(ids.Count>0)
        {
            foreach (var outer in ids)
            {
                foreach (var inner in manager.dataArray)
                {
                    if(outer != inner.id)
                    {
                        list.Add(inner.id);
                    }
                }
            }
        }
        else
        {
            foreach (var inner in manager.dataArray)
                {
                    list.Add(inner.id);
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
///<summary>随机N个X级能力，不包含玩家已拥有的能力，能力必须已经解锁</summary>
public AbilityData[] GetRandomAbilityFromLevel(int number,int level)
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
                foreach (var inner in levelAbility[level])
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
            foreach (var inner in levelAbility[level])
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

}

