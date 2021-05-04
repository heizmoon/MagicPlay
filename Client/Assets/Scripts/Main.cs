using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using Data;
using DG.Tweening;

public class Main : MonoBehaviour {

	// Use this for initialization
	public static Main instance;
	string _lastTime;


	//gameObject
	public GameObject createActorUI =null;
	public GameObject offLineRewardsUI =null;
	public GameObject BattleUI =null;
	public GameObject EventOpenRemindWindow;
	public GameObject UILoading;
	public GameObject dateEventRemindUI;
	public bool dateEventStopWorld;
	public int LockFPS;
	public Player player;
	public Transform BottomUI;
	public Transform MiddleUI;
	public Transform TopUI;
	public Transform allScreenUI;
	public GameObject BasicBanner;
	public Toggle toggleWorld;
	public GameObject storyModeUI;
	///<summary>玩家当前处于哪个挂机状态，0=others,1=practice,2=read,3=work,4=art,-1=UIPlayer</summary>
	public int UIState;

	///<summary>游戏是否已经加载完毕，若没有加载完毕，则关闭游戏的时候，不会执行保存数据（视为没有开启游戏）</summary>
	private bool awakeFinish;
	public int ifNewBird;
	void Awake()
	{
		instance =this;
		//获取上次保存的数据

		player =gameObject.AddComponent<Player>();
		
		//锁定FPS
		Application.targetFrameRate = LockFPS;
		ifNewBird = PlayerPrefs.GetInt("ifNew",0);
	}

	void Start ()
	{
		// 
		// CovertHoursToDate();
		Debug.LogFormat("保存的游戏时间为gameDate:{0}",PlayerPrefs.GetString("gameDate"));
		// PlayerPrefs.SetString("AssetsItems","1001,1,,10,0|1002,2,小家,1,0|1003,1,大法杖,10,0");
		// PlayerPrefs.SetString("playerAssets","1001,1002,1003");


		//初始化
		
		gameObject.AddComponent<DateManager>();
		if(ifNewBird==0)
		{
			NewBird_0();
		}
		else
		{
			StartLoadingUI();
		}
		//加载表格
		
		// if(!ExcelAbility.instance)
        // {
        //     gameObject.AddComponent<ExcelAbility>().LoadInfo("Assets/Data/Ability.xlsx");
        // }

		gameObject.AddComponent<AbilityManager>();
		gameObject.AddComponent<CharacterManager>();
		gameObject.AddComponent<ReformManager>();
		gameObject.AddComponent<SkillManager>();
		gameObject.AddComponent<MonsterManager>();
		gameObject.AddComponent<EventManager>();
		gameObject.AddComponent<SummonManager>();
		gameObject.AddComponent<RandomEventManager>();
		gameObject.AddComponent<ShopManager>();
		gameObject.AddComponent<BuffManager>();
		//判断玩家离线前的状态
		InitBasicListUI();
		//根据离线前的状态恢复UI界面
		//如果已经创建了角色，那么跳过开场剧情
		if(PlayerPrefs.GetInt("createActor")==1)	
		StartCoroutine(WaitForUIInit());
		else//执行开场剧情
		{
			//加载一个特殊的prefab,该prefab上面带有开场剧情，
			//当剧情执行完毕，PlayerPrefs.SetInt("createActor")=1
		StartCoroutine(WaitForUIInit());
			
			//LoadBasicUIs()
		}
	}

	void InitBasicListUI()
	{
		Transform ts =GameObject.Find("BottomToggleList_Main").transform;
		foreach(var item in ts.GetComponentsInChildren<Toggle>())
		{
			item.onValueChanged.AddListener((bool isOn) => OnMarkChanged(item,isOn));
		}
		ts.gameObject.SetActive(false);
	}
	IEnumerator WaitForUIInit()
	{
		yield return new WaitForSeconds(0.5f);
		int offLineTime =getTimeSpan ();
		//加载资产
		// AssetsManager.instance.LoadPlayerAssets();
		
		StartCoroutine(LoadBasicUIs());
		yield return new WaitForSeconds(3f);
		
		//计算离线收益
		if(offLineTime>0)
		{
			offLineRewardsUI.SetActive(true);
			offLineRewardsUI.GetComponent<UIOffLineRewards>().InitUI(player.playerActor.UsingSkillsID[0],offLineTime);
		}


	}
	public IEnumerator LoadBasicUIs()
	{
		Debug.LogFormat("开始加载---");
		// InitUISkillTree();//加载图像数量最多
		//一个一个加载
		yield return new WaitForSeconds(0.5f);
		// InitUIPlayer();
		CharacterManager.instance.CreateCharacters();

		yield return new WaitForSeconds(0.5f);
		SkillManager.instance.SeparateSkillFromLevel();

		yield return new WaitForSeconds(1f);
		
		//等到全部加载完毕，全部隐藏
		
		CloseOhterUIs();
		player.Init();
		// player.playerActor.InitPlayerActor();
		Debug.LogFormat("加载完毕---");
		if(!dateEventStopWorld)
		{
			//游戏时间开始流动
			GetComponent<DateManager>().StartTheWorld();
		}
		awakeFinish =true;
		JudgeWhatsDoing();
	}
	public void ShakeCamera()
	{
		allScreenUI.localPosition =Vector3.zero;
		allScreenUI.DOShakePosition(0.5f,5,10,90,false,true);
	}
	IEnumerator WaitForBattleBegin()
	{
		yield return new  WaitForSeconds(0.5f);
		//UIBattle.Instance.InitUIs();
		
	}
	void JudgeWhatsDoing()
	{
		//如果新手没过，那么开始新手教程
		//直接进入第一场战斗
		if(ifNewBird<7)
		{
			UIChooseCharacter.OnChooseCharacter(0,"Map_00");
			Map.instance.MoveLocal(Map.instance.startPos.GetComponent<MapPoint>().nextPoint[0].GetComponent<MapPoint>());
			
		}
		//--------打开主界面
		else
		InitUIChooseCharacter();
	}
	
	public void StartLoadBasicUIs()
	{
		StartCoroutine(Main.instance.LoadBasicUIs());
	}
	///显示读条界面，3秒后自动移除读条界面
	public void StartLoadingUI()
	{
		UILoading.SetActive(true);
		UILoading.GetComponent<UILoading>().Reset();
		StartCoroutine(Stoploading());
	}
	IEnumerator Stoploading()
	{
		yield return new WaitForSeconds(3f);
		UILoading.SetActive(false);
		if(UIMain.instance)
		UIMain.instance.IntoMain();
		if(Player.instance.playerActor!=null&&ifNewBird>5)
		{
			Destroy(Player.instance.playerActor.gameObject);
		}
	}
	void NewBird_0()//秘境深处
	{
		ifNewBird++;
		NewBird nb = NewBird.LoadNewBird(0);
		StartCoroutine(StopNewBird_0(nb.gameObject));
	}
	IEnumerator StopNewBird_0(GameObject g)
	{
		yield return new WaitForSeconds(3f);
		g.SetActive(false);
		Destroy(g);
	}
	public void HideBasicBanner()
	{
		BasicBanner.SetActive(false);
	}
	public void ShowBasicBanner()
	{
		BasicBanner.SetActive(true);
	}
	public void DestoryBasicUI()
	{
		UIState =0;
		player.playerActor.transform.SetParent(BottomUI);
		// Destroy(UIPlayer.instance.gameObject);
		SkillManager.ClearPool();
		// Destroy(UIEvents.instance.gameObject);
		
		
		offLineRewardsUI.GetComponent<UIOffLineRewards>().OnReceiveReward(1);
	}
	public void ShowDateEventRemind()
	{
		dateEventRemindUI.SetActive(true);
	}
	public void HideDateEventRemind()
	{
		dateEventRemindUI.SetActive(false);
	}
	//获取从上次下线到这次上线经过了多久时间间隔
	int getTimeSpan()
	{
		_lastTime = PlayerPrefs.GetString ("lastTime");
		Debug.Log ("_lastTime:::"+_lastTime);
		if (_lastTime == "") 
		{
			TimeSpan span =new TimeSpan(0);
			return 0;
			 
		} 
		else 
		{
			DateTime timeLast = DateTime.ParseExact(_lastTime, "yyyy-MM-dd-HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
			DateTime timeNow = DateTime.Now;
			TimeSpan span = timeNow - timeLast;
			if(span.TotalHours>12)
			{
				span =new TimeSpan(12,0,0);
			}
			Debug.Log ("spanTime::::"+span.TotalSeconds);
			return 0;
		}

	}
	// //测试功能，已经不需要
	// public void OnPressAdventure()
	// {

	// 	UIPractice.instance.StopPractice();
	// 	BattleUI.SetActive(true);
	// 	StartCoroutine(WaitForBattleBegin());
	// }
	///<summary>切换主界面底部标签页</summary>
	public void OnMarkChanged(Toggle toggle,bool isOn)
	{
		
		if(!isOn)
		{
			return;
		}
		
		switch(toggle.name)
		{
			
			case "Toggle_Practice":
			CloseOhterUIs();
			// StartPractice();
			break;
			case "Toggle_Player":
			CloseOhterUIs();
			InitUIPlayer();
			break;
			case "Toggle_Events":
			CloseOhterUIs();
			break;	
			case "Toggle_Character":
			CloseOhterUIs();
			break;
		}
		
	}
	///<summary>传入一个技能ID，然后开始练习这个技能</summary>
	///<param name ="skillID">技能ID</param>
	
	
	public void InitUIPlayer()
	{
		UIState =-1;
		if(UIPlayer.instance==null)
		{
			GameObject go =Instantiate((GameObject)Resources.Load("Prefabs/UIPlayer"));
			go.transform.SetParent(MiddleUI);
			go.transform.localScale =Vector3.one;
			go.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
			go.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
			
		}
		else
		{
			UIPlayer.instance.transform.SetParent(MiddleUI);
			UIPlayer.instance.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
			UIPlayer.instance.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
			UIPlayer.instance.RefreashUI();
		}
	}
	public void InitUIChooseCharacter()
	{
		if(UIMain.instance==null)
		{
			GameObject go =Instantiate((GameObject)Resources.Load("Prefabs/UIMain"));
			go.transform.SetParent(allScreenUI);
			go.transform.localScale =Vector3.one;
			go.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
			go.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
			
		}
		else
		{
			UIMain.instance.gameObject.SetActive(true);
		}
		
	}
	public void InitUITrait()
	{
		if(UITrait.instance==null)
		{
			GameObject go =Instantiate((GameObject)Resources.Load("Prefabs/UITrait"));
			go.transform.SetParent(allScreenUI);
			go.transform.localScale =Vector3.one;
			go.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
			go.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
			UITrait.instance.InitUI();
		}
		else
		{
			UITrait.instance.gameObject.SetActive(true);
		}
		
	}
	
	
	public void IntoStoryMode(int mode)
	{
		storyModeUI.SetActive(true);
		Animation anim = storyModeUI.GetComponent<Animation>();
		if(mode ==0)
		{
			anim.clip = anim.GetClip("UI_UIStoryMode_into");
		}
		else
		{
			anim.clip = anim.GetClip("UI_UIStoryMode_fromBlack");
		}
		anim.Play();
	}
	public void LeaveStoryMode()
	{
		Animation anim = storyModeUI.GetComponent<Animation>();
		anim.clip = anim.GetClip("UI_UIStoryMode_leave");
		anim.Play();
		StartCoroutine(HideStoryModeUI());
	}
	IEnumerator HideStoryModeUI()
	{
		yield return new WaitForSeconds(1f);
		storyModeUI.SetActive(false);
	}
	IEnumerator WaitForOverDueEvent()
	{
		yield return new WaitForSeconds(0.5f);
	}
	public void ShowNotEnoughGoldTip()
	{
		GameObject g =(GameObject)Instantiate(Resources.Load("Prefabs/NotEnoughGoldTip"));
		g.transform.SetParent(allScreenUI);
		g.transform.localPosition=Vector3.zero;
		g.transform.localScale=Vector3.one;
		StartCoroutine(WaitForDestoryNotEnoughGoldTip(g));
	}
	IEnumerator WaitForDestoryNotEnoughGoldTip(GameObject g)
	{
		yield return new WaitForSeconds(1f);
		Destroy(g);
	}
	// void RemindEventOpen()
	// {
	// 	EventOpenRemindWindow.SetActive(true);
	// }
	// public void AcceptEventOpenRemind()
	// {
	// 	EventOpenRemindWindow.SetActive(false);
	// 	CloseOhterUIs();
	// 	InitUIEvents();

	// }
	void CloseOhterUIs()
	{
		UIState =0;

		if(UIPlayer.instance)
		{
			UIPlayer.instance.transform.SetParent(BottomUI);
			UIPlayer.instance.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
			UIPlayer.instance.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
		}
		

	}
	


	void OnApplicationQuit()//退出时保存
	{
		if(!awakeFinish)
		{
			return;
		}
		//接下来将数值保存
		//保存游戏内日期
        PlayerPrefs.SetString("gameDate",string.Format("{0}-{1}-{2}-{3}",DateManager.instance.now.year,DateManager.instance.now.month,DateManager.instance.now.day,DateManager.instance.now.hours.ToString("0.0")));//测试用数据
		Debug.LogFormat("游戏时间为gameDate:{0}",PlayerPrefs.GetString("gameDate"));
		//保存玩家角色形象
		PlayerPrefs.SetString("playerAvatar","Girl_01");
		//保存玩家技能等级
		// Debug.LogFormat("playerSkillLevel:{0}",PlayerPrefs.GetString("playerSkillLevel"));
		//保存玩家技能熟练度
		// PlayerPrefs.SetString("playerSkillProficiency",Player.instance.OutputSkillProficiency());
		// Debug.LogFormat("playerSkillProficiency:{0}",PlayerPrefs.GetString("playerSkillProficiency"));
		//保存玩家基础属性
		player.SaveBasicProperty();
		//保存玩家正在做的事
		// if(Player.instance.playerNow=="")
		// {
		// 	Player.instance.playerNow ="P,3";
		// }
		// else if(Player.instance.playerNow.Substring(0,1) =="A")
		// {
		// 	Player.instance.playerNow =Player.instance.playerNow.Remove(0,1);
		// }
		// PlayerPrefs.SetString("nowDoing",Player.instance.playerNow);
		//保存玩家已开启的技能树
		// PlayerPrefs.SetString("skillTrees","0,1");
		//保存玩家拥有的资产列表
		// AssetsManager.instance.SavePlayerAssetsItem();
		//保存玩家拥有的特质列表
		// player.SaveTraitList();
		
		//保存玩家解锁的技能
		Player.instance.SaveUnlockSkill();
		//保存玩家使用的技能列表
		player.SaveUsedBattleSkillListOnQuit();
		//保存角色列表
		//保存退出时间
		_lastTime =System.DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss");
		PlayerPrefs.SetString ("lastTime",_lastTime);
		// Debug.Log (_lastTime);
		PlayerPrefs.SetInt("gold",Player.instance.Gold);
		PlayerPrefs.Save();
	}
}
