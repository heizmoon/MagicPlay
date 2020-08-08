using UnityEngine;
using System.Collections;
using System.Data;
using System.IO;
using Excel;
using System.Collections.Generic;
using Data;

public class RelicManager : MonoBehaviour

{
    public static RelicManager instance;
    //(2)根据需要，定义自己的结构        
    

    RelicData[] data;
    RelicGroupData[] groupDatas;
    public RelicData nullData = new RelicData();

    void Awake()
    {   
        instance =this;
        data = Resources.Load<RelicDataSet>("DataAssets/Relic").dataArray;
        groupDatas =Resources.Load<RelicGroupDataSet>("DataAssets/RelicGroup").dataArray;
    }

    
    public string GetInfo(int id ,string content)
    {
        foreach(var item in data)
        {
            if(item.id==id)
            {
                switch(content)
                {
                    case "describe":
                    
                    return item.describe;
                    case "group":
                    return item.group.ToString();
                    case "icon":
                    return item.icon;
                }     
            }    
        }
        return "";
    }
    public RelicData GetRelic(int id)
    {
       foreach(var item in data)
        {
            if(item.id==id)
            {
                return item;       
            }
        }
        return nullData;
    }
    public List<int> GetRelicGroup(int id)
    {
        List<int> list =new List<int>();
        foreach (var item in groupDatas)
        {
            if(item.id ==id)
            {
               string[] slist = item.list.Split(',');
               foreach (var s in slist)
               {
                   list.Add(int.Parse(s));
               }
               return list;
            }
        }
        return list;
    }
}
