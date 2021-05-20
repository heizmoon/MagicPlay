using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleScene : MonoBehaviour
{
    public static BattleScene instance;
    public int steps;//行走步数，用于决定难度等
    public int beatEnemyNumber{get;set;}
    public int beatBossNumber{get;set;}
    int currentMonsterId;
    int currentSceneId;
    int battleType;
    public bool ifLevelUp;
    public int talentPoint;
    public int exp;
    public bool ifDeadByBattle;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
        Reset();
    }
    public void Reset()
    {
        //选择角色结束后，playerActer = 选定的character
        if(SummonManager.instance!=null)
        SummonManager.instance.Init();
        //如果是第一次游戏：新手流程
        steps =0;
        beatEnemyNumber =0;
        beatBossNumber=0;
        currentMonsterId=0;
        currentSceneId=0;
        battleType =0;
        talentPoint =0;
        exp =0;
        Player.instance.ExpAdditon =1;
        Player.instance.ExpAdditonTimes =0;
        Player.instance.GoldAdditon =1;
        Player.instance.GoldAdditonTimes =0;    

    }
    // public void ShowRandomAbilityUI()
    // {
    //     GameObject go =Instantiate((GameObject)Resources.Load("Prefabs/UIChooseAbility"));
    //     go.transform.SetParent(Main.instance.allScreenUI);
	// 	go.transform.localScale =Vector3.one;
	// 	go.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
	// 	go.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
    //     //使用随机出的3个能力
    //     AbilityManager.instance.GetRandomAbility(3,Player.instance.playerActor.abilities);
    // }
    public void InitBattle(int monsterID,int sceneID,int battleType)
	{
        //备份当前战斗信息；
        currentMonsterId=monsterID;
        currentSceneId = sceneID;
        this.battleType = battleType;

		Actor enemy =CreateEnemy(monsterID,1);
		GameObject go =Instantiate((GameObject)Resources.Load("Prefabs/UIBattle"));
		go.transform.SetParent(Main.instance.allScreenUI);
		go.transform.localScale =Vector3.one;
		go.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
		go.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
        UIBattle.Instance.Init(enemy, sceneID, battleType);
        
        UIBattle.Instance.BattleBegin();
	}
    public void ReturnToBattle()
    {
        InitBattle(currentMonsterId,currentSceneId,battleType);
    }
	Actor CreateEnemy(int id,int level)
    {
        MonsterTypeData monster = MonsterManager.instance.GetInfo(id);
        Actor enemy;
        //从怪物表中创建一个具体的敌人
        enemy =null;
        enemy =Instantiate((GameObject)Resources.Load("Prefabs/Enemy/"+monster.prefab)).GetComponent<Actor>();
        enemy.actorType =ActorType.敌人;
        enemy.level =level;
        enemy.InitEnemy(monster);
        return enemy;
    }
	//---------------------------------
	public void InitShop(int realID)
	{
		//商店不会出售玩家身上已经拥有的能力
		//从所有能力中随机出N个能力，排除List
        
        GameObject go  = Instantiate((GameObject)Resources.Load("Prefabs/UIBattleShop"));
        go.transform.SetParent(Main.instance.MiddleUI);
		go.transform.localScale =Vector3.one;
		go.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
		go.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
        go.GetComponent<UIBattleShop>().Init(realID);
        UIBasicBanner.instance.textMap.text ="商店";

	}
    public void InitCamp()
	{
        GameObject go  = Instantiate((GameObject)Resources.Load("Prefabs/UICamp"));
        go.transform.SetParent(Main.instance.MiddleUI);
		go.transform.localScale =Vector3.one;
		go.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
		go.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
        go.GetComponent<UICamp>().Init();
        UIBasicBanner.instance.textMap.text ="营地";

	}
    public void InitTreasure()
	{
        GameObject go  = Instantiate((GameObject)Resources.Load("Prefabs/UITreasure"));
        go.transform.SetParent(Main.instance.MiddleUI);
		go.transform.localScale =Vector3.one;
		go.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
		go.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
        // Debug.Log("生成一个宝箱");
        UIBasicBanner.instance.textMap.text ="宝箱";

	}
    public void InitRandomEvent()
	{
        GameObject go  = Instantiate((GameObject)Resources.Load("Prefabs/UIRandomEvent"));
        go.transform.SetParent(Main.instance.MiddleUI);
		go.transform.localScale =Vector3.one;
		go.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
		go.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
        UIBasicBanner.instance.textMap.text ="事件";

	}
    public void OpenMap()
    {
        if(Map.instance!=null)
        Map.instance.gameObject.SetActive(true);
        Map.instance.Refresh();
        UIBasicBanner.instance.textMap.text ="地图";
    }
    public void ChangeMap(string mapName)
    {
        if(Map.instance!=null)
        {
            Map.instance.DestoryMap();
        }
        GameObject go  = Instantiate((GameObject)Resources.Load("Prefabs/Maps/"+mapName));
        go.transform.SetParent(Main.instance.MiddleUI);
		go.transform.localScale =Vector3.one;
        go.transform.localPosition =Vector3.zero;
		// go.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
		go.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
        go.GetComponent<Map>().InitMap();
        UIBasicBanner.instance.textMap.text ="地图";
    }
    public void BattleEnd(int type)
    {
        beatEnemyNumber++;

        //如果不是BOSS结束，打开当前地图，
        //如果是BOSS结束，尝试打开新地图，
        //如果没有新地图，则整个场景完结
        if(type !=2)
        {
            OpenMap();
        }
        else
        {
            Player.instance.playerActor.HpCurrent=Player.instance.playerActor.HpMax;
            beatBossNumber++;
            if(Map.instance.nextMap!="")
            ChangeMap(Map.instance.nextMap);
            else
            UIBattleFail.CreateUI().ShowStatisticUI();//1.显示结算
        }
    }
    public void BattleSceneOver()
    {
        //2.摧毁自身
        // Destroy(gameObject);
        Main.instance.StartLoadingUI();
        Reset();
    }
    
}
