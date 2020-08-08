using UnityEngine;
using System.Collections;
using System.Data;
using System.IO;
using Excel;
using System.Collections.Generic;
using Data;


public class MonsterManager : MonoBehaviour

{
    public static MonsterManager instance;
    
    public MonsterTypeData[] data;
    MonsterGroupData[] groupDatas;
    public MonsterTypeData infoTd = new MonsterTypeData();

    void Awake()
    {   
        instance =this;
        data = Resources.Load<MonsterTypeSet>("DataAssets/MonsterType").dataArray;
        groupDatas = Resources.Load<MonsterGroupSet>("DataAssets/MonsterGroup").dataArray;

    }
    
    public string GetGroupInfo(int id ,string content)
    {
        foreach(var item in groupDatas)
        {
            if(item.id==id)
            {
                switch(content)
                {
                    case "monsterDistrubtion":
                    return item.monsterDistrubtion;
                    case "leader":
                    return item.leader.ToString();
                    case "scene":
                    return item.scene.ToString();
                }
            }
        }
        return "";
    }

    public MonsterTypeData GetInfo(int id)
    {
        MonsterTypeData task =new MonsterTypeData();
        foreach(var item in data)
        {
            if(item.id==id)
            {
             return item;       
            } 
        }
        return task;
    }


}