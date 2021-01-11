using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

public class SummonManager : MonoBehaviour
{
    public  static SummonManager instance;
    SummonDataSet manager;
    List<Summoned> playerSummoneds = new List<Summoned>();
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
        summoned.Init(master,summonData,master.SummonedLifeTimePlus);
        if(master == Player.instance.playerActor)
        {
            playerSummoneds.Add(summoned);
        }
        SummonArray();

    }
    public void DecreaseSummonedNum(Summoned summoned)
    {
        if(summoned.master ==Player.instance.playerActor)
        {
           playerSummoneds.Remove(summoned);
        }
        SummonArray();
    }
    public void SummonArray()
    {
        //根据当前已经有的召唤物数量，自动进行位置调节
        //1个召唤物时的位置，2个召唤物时的位置……
        int PlayerSummonedNum =playerSummoneds.Count;
        if(PlayerSummonedNum ==1)
        {
            playerSummoneds[0].transform.localPosition = new Vector3(70,70,0);
        }
        else if(PlayerSummonedNum ==2)
        {
            playerSummoneds[0].transform.localPosition = new Vector3(70,70,0);
            playerSummoneds[1].transform.localPosition = new Vector3(-90,70,0);
        }
        else if(PlayerSummonedNum ==3)
        {
            playerSummoneds[0].transform.localPosition = new Vector3(70,70,0);
            playerSummoneds[1].transform.localPosition = new Vector3(-90,70,0);
            playerSummoneds[2].transform.localPosition = new Vector3(0,130,0);
        }
        else if(PlayerSummonedNum ==4)
        {
            playerSummoneds[0].transform.localPosition = new Vector3(70,70,0);
            playerSummoneds[1].transform.localPosition = new Vector3(-90,70,0);
            playerSummoneds[2].transform.localPosition = new Vector3(30,130,0);
            playerSummoneds[3].transform.localPosition = new Vector3(-45,130,0);
        }
        else if(PlayerSummonedNum ==5)
        {
            playerSummoneds[0].transform.localPosition = new Vector3(80,35,0);
            playerSummoneds[1].transform.localPosition = new Vector3(-105,35,0);
            playerSummoneds[2].transform.localPosition = new Vector3(-75,95,0);
            playerSummoneds[3].transform.localPosition = new Vector3(60,95,0);
            playerSummoneds[4].transform.localPosition = new Vector3(0,130,0);
        }
        else if(PlayerSummonedNum ==6)
        {
            playerSummoneds[0].transform.localPosition = new Vector3(-105,10,0);
            playerSummoneds[1].transform.localPosition = new Vector3(-75,70,0);
            playerSummoneds[2].transform.localPosition = new Vector3(60,70,0);
            playerSummoneds[3].transform.localPosition = new Vector3(-40,120,0);
            playerSummoneds[4].transform.localPosition = new Vector3(30,120,0);
            playerSummoneds[5].transform.localPosition = new Vector3(80,10,0);
        }
        else if(PlayerSummonedNum ==7)
        {
            playerSummoneds[0].transform.localPosition = new Vector3(0,135,0);
            playerSummoneds[1].transform.localPosition = new Vector3(-65,100,0);
            playerSummoneds[2].transform.localPosition = new Vector3(55,100,0);
            playerSummoneds[3].transform.localPosition = new Vector3(80,50,0);
            playerSummoneds[4].transform.localPosition = new Vector3(-100,50,0);
            playerSummoneds[5].transform.localPosition = new Vector3(-85,-10,0);
            playerSummoneds[6].transform.localPosition = new Vector3(70,-10,0);
        }
        else if(PlayerSummonedNum ==8)
        {
            playerSummoneds[0].transform.localPosition = new Vector3(-35,130,0);
            playerSummoneds[1].transform.localPosition = new Vector3(40,130,0);
            playerSummoneds[2].transform.localPosition = new Vector3(-80,80,0);
            playerSummoneds[3].transform.localPosition = new Vector3(70,80,0);
            playerSummoneds[4].transform.localPosition = new Vector3(80,30,0);
            playerSummoneds[5].transform.localPosition = new Vector3(-100,30,0);
            playerSummoneds[6].transform.localPosition = new Vector3(-85,-30,0);
            playerSummoneds[7].transform.localPosition = new Vector3(70,-30,0);
        }
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
