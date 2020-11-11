using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Data;

public class ReformManager : MonoBehaviour
{
    public static ReformManager instance;
    ReformDataSet manager;
    void Awake()
    {
        instance =this;
        manager = Resources.Load<ReformDataSet>("DataAssets/Reform");
        
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
                    
                }     
            }    
        }
        return "";
    }
    public ReformData GetInfo(int id)
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
    public string GetInfo(int id,int result)
    {
        ReformData reformData = GetInfo(id);
        switch(result)
        {
            case 0:
            return reformData.fail;
            case 1:
            return reformData.common;
            case 2:
            return reformData.perfect;
            
        }
        return "";
    }
    public int GetResult(ReformData reformData,int reformTimes)
    {
        float x = Random.Range(0,1f);
        if(x<=reformData.percentP)
        return 2;
        else if(x>reformData.percentP&&x<=reformData.percentC)
        return 1;
        else
        return 0;
    }
    ///<summary>随机获取N个改造箱</summary>
    public ReformData[] RandomCharacters(int N)
    {
        if(N<1)
        {
            return null;
        }
        ReformData[] datas =new ReformData[N];
        int temp =100;
        for(int i =0;i<N;i++)
        {
            int code = Random.Range(0,manager.dataArray.Length-i);
            if(code == temp)
            {
                i--;
            }
            else
            {
                datas[i] = GetInfo(code);
            }
        }
        return datas;
    }
}
