using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

public class AbilityManager : MonoBehaviour
{
    public static AbilityManager instance;
    AbilityDataSet manager;
    void Awake()
    {
        instance =this;
        manager = Resources.Load<AbilityDataSet>("DataAssets/Ability");
        
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

}
