using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UITreasure : MonoBehaviour
{
    public GameObject Gframe;
    public Button BTNOpen;
    public Button BTNOK;
    public Button BTNReTry;
    public Button BTNReturn;
    public ItemBox item;
    int type;
    int id;
    void Start()
    {
        BTNOK.onClick.AddListener(OnOK);
        BTNOpen.onClick.AddListener(OnOpen);
        BTNReturn.onClick.AddListener(OnReturn);
        BTNReTry.onClick.AddListener(OnRetry);
        Init();
        Gframe.SetActive(false);
        

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Init()
    {
        //根据情况随机出宝物
        int level = BattleScene.instance.steps;
        //先确定随机出道具还是技能卡
        if(Random.Range(0,5)>3)
        {
            //随机出技能卡
            SkillData[] datas =SkillManager.instance.GetRandomSelfSkillsLevelLimit(1,0);
            item.Init(datas[0]);
            type =0;
            id = datas[0].id;
        }
        else
        {
            //随机出道具
            AbilityData[] datas = AbilityManager.instance.GetRandomAbilityFromLevel(1,0);
            item.Init(datas[0]);
            type =1;
            id = datas[0].id;
        }
        item.HideToggleSelect();

    }
    void OnOpen()
    {
        Gframe.SetActive(true);
    }
    void OnOK()
    {
        gameObject.SetActive(false);
        if(type==0)
        {
            Player.instance.playerActor.UsingSkillsID.Add(id);
        }
        if(type==1)
        {
            Player.instance.playerActor.abilities.Add(id);
        }
        BattleScene.instance.OpenMap();
        Destroy(gameObject);
    }
    void OnRetry()
    {
        item.Reset();
        Gframe.SetActive(false);
        Init();
    }
    void OnReturn()
    {
        gameObject.SetActive(false);
        BattleScene.instance.OpenMap();
        Destroy(gameObject);
    }
    
}
