using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

public class ShopManager : MonoBehaviour
{
    public  static ShopManager instance;
    ShopDataSet manager;
    void Awake()
    {
        instance =this;
        manager = Resources.Load<ShopDataSet>("DataAssets/Shop");    
    }

    // public ShopData GetShop(int id)
    // {
        
    //     return new RandomEvent();
        
    // }
    public ShopData GetInfo(int id)
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
}
