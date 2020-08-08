using UnityEngine;
using System.Collections;
using System.Data;
using System.IO;
using Excel;
using System.Collections.Generic;
using Data;

public class TraitManager : MonoBehaviour

{
    public static TraitManager instance;
    //(2)根据需要，定义自己的结构        
    public TraitData[] traitDatas;
    IdealData[] idealDatas;
    public TraitData nullData = new TraitData();
    public List<int> failIdeal =new List<int>();
    public List<int> successIdeal =new List<int>();
    int nowIdealID;
    void Awake()
    {   
        instance =this;
        traitDatas = Resources.Load<TraitDataSet>("DataAssets/Trait").dataArray;
        idealDatas = Resources.Load<IdealDataSet>("DataAssets/Ideal").dataArray;
        HandleDynamicInfo();
        GetPastIdeal();
    }
    void HandleDynamicInfo()
    {
        for (int i = 0; i < traitDatas.Length; i++)
        {
            TraitData item =traitDatas[i];
            traitDatas[i] =item;
        }

    }
    void GetPastIdeal()
    {
        string s =PlayerPrefs.GetString("playerIdeals");
        if(s =="")
        {
            return;
        }
        string[] ss = s.Split('|');
        foreach (var item in ss)
        {
            if(int.Parse(item.Split(',')[1])==1)
            {
                successIdeal.Add(int.Parse(item.Split(',')[0]));
                continue;
            }
            if(int.Parse(item.Split(',')[1])==2)
            {
                failIdeal.Add(int.Parse(item.Split(',')[0]));
                continue;
            }
            if(int.Parse(item.Split(',')[1])==3)
            {
                nowIdealID =int.Parse(item.Split(',')[0]);
                continue;
            }
        }
    }
    public string GetInfo(int id ,string content)
    {
        foreach(var item in traitDatas)
        {
            if(item.id==id)
            {
                switch(content)
                {
                    case "name":
                    return item.name;
                    case "describe":
                    
                    return item.describe;
                }     
            }    
        }
        return "";
    }
    public TraitData GetInfo(int id)
    {
       foreach(var item in traitDatas)
        {
            if(item.id==id)
            {
                return item;       
            } 
        }
        return nullData;
    } 
    public TraitData GivePlayerTrait(int id)
    {
        return GetInfo(id);
    }
}
