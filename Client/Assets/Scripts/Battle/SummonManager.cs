using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

public class SummonManager : MonoBehaviour
{
    public  static SummonManager instance;
    SummonDataSet manager;
    List<Summoned> playerSummoneds = new List<Summoned>();
    List<Summoned> enemySummoneds = new List<Summoned>();

    void Awake()
    {
        instance =this;
        manager = Resources.Load<SummonDataSet>("DataAssets/Summon");
        
    }
    void Start()
    {
        
    }
    public void Init()
    {
        playerSummoneds = new List<Summoned>();
        enemySummoneds = new List<Summoned>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CreateSummon(Skill skill)
    {
        for (int i = 0; i < skill.skillData.summonNum; i++)
        {
            CreateSummon(skill,skill.caster);
        }

    }
    public void CreateSummon(Skill skill, Actor master)
    {
        if(playerSummoneds.Count==8)
        {
            playerSummoneds[0].Death();
        }
        if(enemySummoneds.Count==-8)
        {
            enemySummoneds[0].Death();
        }
        SummonData summonData = GetInfo(skill.skillData.summonType);
        Summoned summoned =((GameObject)Instantiate(Resources.Load("Prefabs/Summoned/"+summonData.prefab))).GetComponent<Summoned>();
        summoned.transform.SetParent(master.summonPoint);
        summoned.transform.localPosition =Vector3.zero;
        summoned.transform.localScale =Vector3.one;
        summoned.Init(master,summonData,master.SummonedLifeTimePlus,skill);
        if(master == Player.instance.playerActor)
        {
            playerSummoneds.Add(summoned);
            SummonArray(1);
        }
        else
        {
            enemySummoneds.Add(summoned);
            SummonArray(-1);
        }
    }
    public void DecreaseSummonedNum(Summoned summoned)
    {
        if(summoned.master ==Player.instance.playerActor)
        {
           playerSummoneds.Remove(summoned);
           SummonArray(1);
        }
        else
        {
            enemySummoneds.Remove(summoned);
           SummonArray(-1);
        }
        
    }
    public void SummonArray(int isPlayer)
    {
        //根据当前已经有的召唤物数量，自动进行位置调节
        //1个召唤物时的位置，2个召唤物时的位置……
        List<Summoned> _summoneds =new List<Summoned>();
        if(isPlayer==1)
        {
            _summoneds=playerSummoneds;
        }
        else
        {
            _summoneds =enemySummoneds;
        }
        int SummonedNum =_summoneds.Count;
        if(SummonedNum ==1)
        {
            playerSummoneds[0].transform.localPosition = new Vector3(isPlayer*70,70,0);
        }
        else if(SummonedNum ==2)
        {
            playerSummoneds[0].transform.localPosition = new Vector3(isPlayer*70,70,0);
            playerSummoneds[1].transform.localPosition = new Vector3(isPlayer*-90,70,0);
        }
        else if(SummonedNum ==3)
        {
            playerSummoneds[0].transform.localPosition = new Vector3(isPlayer*70,70,0);
            playerSummoneds[1].transform.localPosition = new Vector3(isPlayer*-90,70,0);
            playerSummoneds[2].transform.localPosition = new Vector3(0,130,0);
        }
        else if(SummonedNum ==4)
        {
            playerSummoneds[0].transform.localPosition = new Vector3(isPlayer*70,70,0);
            playerSummoneds[1].transform.localPosition = new Vector3(isPlayer*-90,70,0);
            playerSummoneds[2].transform.localPosition = new Vector3(isPlayer*30,130,0);
            playerSummoneds[3].transform.localPosition = new Vector3(isPlayer*-45,130,0);
        }
        else if(SummonedNum ==5)
        {
            playerSummoneds[0].transform.localPosition = new Vector3(isPlayer*80,35,0);
            playerSummoneds[1].transform.localPosition = new Vector3(isPlayer*-105,35,0);
            playerSummoneds[2].transform.localPosition = new Vector3(isPlayer*-75,95,0);
            playerSummoneds[3].transform.localPosition = new Vector3(isPlayer*60,95,0);
            playerSummoneds[4].transform.localPosition = new Vector3(0,130,0);
        }
        else if(SummonedNum ==6)
        {
            playerSummoneds[0].transform.localPosition = new Vector3(isPlayer*-105,10,0);
            playerSummoneds[1].transform.localPosition = new Vector3(isPlayer*-75,70,0);
            playerSummoneds[2].transform.localPosition = new Vector3(isPlayer*60,70,0);
            playerSummoneds[3].transform.localPosition = new Vector3(isPlayer*-40,120,0);
            playerSummoneds[4].transform.localPosition = new Vector3(isPlayer*30,120,0);
            playerSummoneds[5].transform.localPosition = new Vector3(isPlayer*80,10,0);
        }
        else if(SummonedNum ==7)
        {
            playerSummoneds[0].transform.localPosition = new Vector3(0,135,0);
            playerSummoneds[1].transform.localPosition = new Vector3(isPlayer*-65,100,0);
            playerSummoneds[2].transform.localPosition = new Vector3(isPlayer*55,100,0);
            playerSummoneds[3].transform.localPosition = new Vector3(isPlayer*80,50,0);
            playerSummoneds[4].transform.localPosition = new Vector3(isPlayer*-100,50,0);
            playerSummoneds[5].transform.localPosition = new Vector3(isPlayer*-85,-10,0);
            playerSummoneds[6].transform.localPosition = new Vector3(isPlayer*70,-10,0);
        }
        else if(SummonedNum ==8)
        {
            playerSummoneds[0].transform.localPosition = new Vector3(isPlayer*-35,130,0);
            playerSummoneds[1].transform.localPosition = new Vector3(isPlayer*40,130,0);
            playerSummoneds[2].transform.localPosition = new Vector3(isPlayer*-80,80,0);
            playerSummoneds[3].transform.localPosition = new Vector3(isPlayer*70,80,0);
            playerSummoneds[4].transform.localPosition = new Vector3(isPlayer*80,30,0);
            playerSummoneds[5].transform.localPosition = new Vector3(isPlayer*-100,30,0);
            playerSummoneds[6].transform.localPosition = new Vector3(isPlayer*-85,-30,0);
            playerSummoneds[7].transform.localPosition = new Vector3(isPlayer*70,-30,0);
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
    public int TakeDamage(int _damage,Actor master)
    {
        if(_damage<1)
        return 0;
        List<Summoned> _list =new List<Summoned>();
        if(master==Player.instance.playerActor)
        {
            _list = playerSummoneds;
        }
        else
        {
            _list = enemySummoneds;
        }
        if(_list.Count==0)
        return 0;
        else
        {
            //找到第一个嘲讽的
            for (int i = 0; i < _list.Count; i++)
            {
                if(_list[i].isTaunt)
                {                    
                    _list[i].TakeDamage(_damage);
                    return _damage;
                }
            }
            //没有嘲讽的召唤物的情况，筛选掉所有虚无的
            for (int i = 0; i < _list.Count; i++)
            {
                if(!_list[i].isVoid)
                {                    
                    _list[i].TakeDamage(_damage);
                    return _damage;
                }
            }
            return 0;
        }
    }
    
}
