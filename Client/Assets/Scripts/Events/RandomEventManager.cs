using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

public class RandomEventManager : MonoBehaviour
{
    public  static RandomEventManager instance;
    RandomEventDataSet manager;
    void Awake()
    {
        instance =this;
        manager = Resources.Load<RandomEventDataSet>("DataAssets/RandomEvent");    
    }

    public RandomEvent GetRandomEventByRank(int rank)
    {
        bool b =true;
        while(b)
        {
            int r = Random.Range(1,manager.dataArray.Length);
            RandomEvent random =GetInfo(r);
            if(random.rank ==rank)
            {
                return random;
            } 
        }
        return new RandomEvent();
        
    }
    public RandomEvent GetInfo(int id)
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
