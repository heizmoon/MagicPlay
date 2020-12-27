using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

public class SummonManager : MonoBehaviour
{
    public  static SummonManager instance;
    SummonDataSet manager;
    void Awake()
    {
        instance =this;
        manager = Resources.Load<SummonDataSet>("DataAssets/Summon");
        
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CreateSummon(int id, int num,Actor master)
    {
        for (int i = 0; i < num; i++)
        {
            CreateSummon(id,master);
        }

    }
    public void CreateSummon(int id, Actor master)
    {
        SummonData summonData = GetInfo(id);
        Summoned summoned =((GameObject)Instantiate(Resources.Load("Prefabs/Summoned/"+summonData.prefab))).GetComponent<Summoned>();
        summoned.transform.SetParent(Player.instance.playerActor.summonPoint);
        summoned.transform.localPosition =Vector3.zero;
        summoned.transform.localScale =Vector3.one;
        summoned.Init(master,summonData);

    }
    public SummonData GetInfo(int id)
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
