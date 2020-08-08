using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Data;

public class AbyssManager : MonoBehaviour
{
    public static AbyssManager instance;
    
    public AbyssGroupDataSet manager;
    void Awake()
    {
        instance =this;
        
        manager = Resources.Load<AbyssGroupDataSet>("DataAssets/AbyssGroup");
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
                    return item.groupName;
                    case "discribe":
                    // Debug.LogFormat("内容:{0}",item.discribe);
                    break;
                    case "reward":
                    return item.eventDistribution.ToString();
                    case "icon":
                    return item.icon;
                    case "background":
                    return item.background;
                }
            }    
        }
        return "ok";
    }

    public AbyssGroupData GetInfo(int id)
    {
        AbyssGroupData task =new AbyssGroupData();
        foreach(var item in manager.dataArray)
        {
            if(item.id==id)
            {
             return item;       
            } 
        }
        return task;
    }
}
