using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleScene : MonoBehaviour
{
    public static BattleScene instance;
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
        //如果不是第一次游戏：3选1初始能力
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
		Actor enemy =CreateEnemy(monsterID,1);
		GameObject go =Instantiate((GameObject)Resources.Load("Prefabs/UIBattle"));
		go.transform.SetParent(Main.instance.allScreenUI);
		go.transform.localScale =Vector3.one;
		go.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
		go.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
        UIBattle.Instance.Init(enemy,sceneID,isBoss);
        
        UIBattle.Instance.BattleBegin();
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
        go.GetComponent<UITreasure>().Init();
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
        //如果不是BOSS结束，打开当前地图，
        //如果是BOSS结束，尝试打开新地图，
        //如果没有新地图，则整个场景完结
        if(!isBoss)
        {
            OpenMap();
        }
        else
        {
            if(Map.instance.nextMap!="")
            ChangeMap(Map.instance.nextMap);
            else
            BattleSceneOver();
        }
    }
    public void BattleSceneOver()
    {
        //1.显示结算
        //+++++++++++++++++++++++++++++++++++
        //2.摧毁自身
        Destroy(gameObject);
    }
}
