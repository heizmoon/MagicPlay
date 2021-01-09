using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleScene : MonoBehaviour
{
    public static BattleScene instance;
    public int steps{get;set;}//行走步数，用于决定难度等
    public int beatEnemyNumber{get;set;}
    public int beatBossNumber{get;set;}
    int currentMonsterId;
    int currentSceneId;
    bool isBoss;

    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
        
    }
    public void Init()
    {
        //选择角色结束后，playerActer = 选定的character
        
        //如果是第一次游戏：新手流程
        
        
    }
    public void ShowRandomAbilityUI()
    {
        GameObject go =Instantiate((GameObject)Resources.Load("Prefabs/UIChooseAbility"));
        go.transform.SetParent(Main.instance.allScreenUI);
		go.transform.localScale =Vector3.one;
		go.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
		go.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
        //使用随机出的3个能力
        AbilityManager.instance.GetRandomAbility(3,Player.instance.playerActor.abilities);
    }
    public void InitBattle(int monsterID,int sceneID,bool isBoss)
	{
        //备份当前战斗信息；
        currentMonsterId=monsterID;
        currentSceneId = sceneID;
        this.isBoss =isBoss;

		Actor enemy =CreateEnemy(monsterID,1);
		GameObject go =Instantiate((GameObject)Resources.Load("Prefabs/UIBattle"));
		go.transform.SetParent(Main.instance.allScreenUI);
		go.transform.localScale =Vector3.one;
		go.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
		go.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
        UIBattle.Instance.Init(enemy,sceneID,isBoss);
        
        UIBattle.Instance.BattleBegin();
	}
    public void ReturnToBattle()
    {
        InitBattle(currentMonsterId,currentSceneId,isBoss);
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
	public void InitShop()
	{
		//商店不会出售玩家身上已经拥有的能力
		//从所有能力中随机出N个能力，排除List
        
        GameObject go  = Instantiate((GameObject)Resources.Load("Prefabs/UIBattleShop"));
        go.transform.SetParent(Main.instance.middleUI);
		go.transform.localScale =Vector3.one;
		go.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
		go.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
        // go.GetComponent<UIBattleShop>().Init(datas);
	}
    public void InitCamp()
	{
        GameObject go  = Instantiate((GameObject)Resources.Load("Prefabs/UICamp"));
        go.transform.SetParent(Main.instance.middleUI);
		go.transform.localScale =Vector3.one;
		go.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
		go.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
        go.GetComponent<UICamp>().Init();
	}
    public void InitTreasure()
	{
        GameObject go  = Instantiate((GameObject)Resources.Load("Prefabs/UITreasure"));
        go.transform.SetParent(Main.instance.middleUI);
		go.transform.localScale =Vector3.one;
		go.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
		go.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
        // Debug.Log("生成一个宝箱");
	}
    public void OpenMap()
    {
        if(Map.instance!=null)
        Map.instance.gameObject.SetActive(true);
        Map.instance.Refresh();
    }
    public void ChangeMap(string mapName)
    {
        if(Map.instance!=null)
        {
            Map.instance.DestoryMap();
        }
        GameObject go  = Instantiate((GameObject)Resources.Load("Prefabs/Maps/"+mapName));
        go.transform.SetParent(Main.instance.middleUI);
		go.transform.localScale =Vector3.one;
		go.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
		go.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
        go.GetComponent<Map>().InitMap();
    }
    public void BattleEnd(bool isBoss)
    {
        beatEnemyNumber++;

        //如果不是BOSS结束，打开当前地图，
        //如果是BOSS结束，尝试打开新地图，
        //如果没有新地图，则整个场景完结
        if(!isBoss)
        {
            OpenMap();
        }
        else
        {
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
        Destroy(gameObject);
        Main.instance.StartLoadingUI();
    }
    
}
