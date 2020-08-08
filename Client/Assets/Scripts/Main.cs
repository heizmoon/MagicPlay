using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using Data;

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
	public Transform middleUI;
	public Transform allScreenUI;
	public GameObject BasicBanner;
	public Toggle toggleWorld;
	public GameObject storyModeUI;
	///<summary>玩家当前处于哪个挂机状态，0=others,1=practice,2=read,3=work,4=art,-1=UIPlayer</summary>
	public int UIState;

	///<summary>游戏是否已经加载完毕，若没有加载完毕，则关闭游戏的时候，不会执行保存数据（视为没有开启游戏）</summary>
	private bool awakeFinish;
	void Awake()
	{
		instance =this;
		//获取上次保存的数据

		player =gameObject.AddComponent<Player>();
		
		//锁定FPS
		Application.targetFrameRate = LockFPS;
	}

	void Start ()
	{
		// 
		// CovertHoursToDate();
		PlayerPrefs.SetString("playerAvatar","Girl_01");
		PlayerPrefs.SetString("skillTrees","0,1");
		Debug.LogFormat("保存的游戏时间为gameDate:{0}",PlayerPrefs.GetString("gameDate"));
		// PlayerPrefs.SetString("AssetsItems","1001,1,,10,0|1002,2,小家,1,0|1003,1,大法杖,10,0");
		// PlayerPrefs.SetString("playerAssets","1001,1002,1003");


		//初始化
		player.Init();
		gameObject.AddComponent<DateManager>();
		StartLoadingUI();
		//加载表格
		
		// if(!ExcelAbility.instance)
        // {
        //     gameObject.AddComponent<ExcelAbility>().LoadInfo("Assets/Data/Ability.xlsx");
        // }

		
		gameObject.AddComponent<AssetsManager>();
		gameObject.AddComponent<SkillManager>();
		gameObject.AddComponent<MonsterManager>();
		gameObject.AddComponent<EventManager>();
		gameObject.AddComponent<AbyssManager>();
		gameObject.AddComponent<TraitManager>();
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
		Transform ts =GameObject.Find("BottomToggleList").transform;
		foreach(var item in ts.GetComponentsInChildren<Toggle>())
		{
			item.onValueChanged.AddListener((bool isOn) => OnMarkChanged(item,isOn));
		}
	}
	IEnumerator WaitForUIInit()
	{
		yield return new WaitForSeconds(0.5f);
		int offLineTime =getTimeSpan ();
		//加载资产
		AssetsManager.instance.LoadPlayerAssets();
		
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
		InitUISkillTree();//加载图像数量最多
		//一个一个加载
		yield return new WaitForSeconds(0.4f);
		InitUIPlayer();
		yield return new WaitForSeconds(0.2f);
		
		//等到全部加载完毕，全部隐藏
		
		
		CloseOhterUIs();
		JudgeWhatsDoing();
		player.playerActor.InitPlayerActor();
		Debug.LogFormat("加载完毕---");
		if(!dateEventStopWorld)
		{
			//游戏时间开始流动
			GetComponent<DateManager>().StartTheWorld();
		}
		awakeFinish =true;
	}
	void Update () 
	{
	}
	IEnumerator WaitForBattleBegin()
	{
		yield return new  WaitForSeconds(0.5f);
		//UIBattle.Instance.InitUIs();
		
	}
	void JudgeWhatsDoing()
	{
		//根据玩家正在做的事件进行判断
		string[] ss;
		if(PlayerPrefs.GetString("nowDoing")=="")
		{
			ss =new string [] {"P","3"};
		}
		else
		{
			ss=PlayerPrefs.GetString("nowDoing").Split(',');
		}
		
		Debug.LogFormat("nowDoing:{0}",PlayerPrefs.GetString("nowDoing"));
		int[] skills =new int[1]{int.Parse(ss[1])};
		toggleWorld.isOn =true;
		if(ss[0] =="")
		{
			return;
		}
		switch(ss[0])
		{
			case "P":
			Debug.Log("正在练习技能"); 
			player.playerActor.SetSkillList(skills);
			StartPractice(skills[0]);
			break;
			case "A":
			//正在进行冒险
			break;
			case "L":
			//正在学习
			break;
		}
			
		
	}
	public void startLoadBasicUIs()
	{
		StartCoroutine(Main.instance.LoadBasicUIs());
	}
	public void StartLoadingUI()
	{
		UILoading.SetActive(true);
		StartCoroutine(Stoploading());
	}
	IEnumerator Stoploading()
	{
		yield return new WaitForSeconds(3f);
		UILoading.SetActive(false);
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
		Destroy(UIPractice.instance.gameObject);
		SkillManager.ClearPool();
		Destroy(UISkillTree.instance.gameObject);
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
			case "Toggle_SkillTree":
			CloseOhterUIs();
			InitUISkillTree();
			break;
			case "Toggle_Practice":
			CloseOhterUIs();
			StartPractice();
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
	public void StartPractice(params int[] skillID)
	{
		if(UIPractice.instance==null)
		{
			GameObject go =Instantiate((GameObject)Resources.Load("Prefabs/UIPractice"));
			go.transform.SetParent(middleUI);
			go.transform.localScale =Vector3.one;
			go.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
			go.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
			// go.transform.localPosition =Vector3.zero;
			go.GetComponent<UIPractice>().StartPractice(skillID);
		}
		else
		{
			UIPractice.instance.transform.SetParent(middleUI);
			UIPractice.instance.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
			UIPractice.instance.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
			UIPractice.instance.StartPractice(skillID);
		}
		UIState =1;
	}
	void InitUISkillTree()
	{
		if(UISkillTree.instance==null)
		{
			GameObject go =Instantiate((GameObject)Resources.Load("Prefabs/UISkillTree"));
			go.transform.SetParent(middleUI);
			go.transform.localScale =Vector3.one;
			go.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
			go.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
		}
		else
		{
			UISkillTree.instance.transform.SetParent(middleUI);
			UISkillTree.instance.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
			UISkillTree.instance.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
		}
		UIState =0;
	}
	public void InitUIPlayer()
	{
		UIState =-1;
		if(UIPlayer.instance==null)
		{
			GameObject go =Instantiate((GameObject)Resources.Load("Prefabs/UIPlayer"));
			go.transform.SetParent(middleUI);
			go.transform.localScale =Vector3.one;
			go.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
			go.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
			
		}
		else
		{
			UIPlayer.instance.transform.SetParent(middleUI);
			UIPlayer.instance.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
			UIPlayer.instance.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
			UIPlayer.instance.RefreashUI();
		}
	}
	public void InitUIPlayerAssets()
	{
		if(UIPlayerAssets.instance==null)
		{
			GameObject go =Instantiate((GameObject)Resources.Load("Prefabs/UIPlayerAssets"));
			go.transform.SetParent(allScreenUI);
			go.transform.localScale =Vector3.one;
			go.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
			go.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
			UIPlayerAssets.instance.InitUI();
		}
		else
		{
			UIPlayerAssets.instance.gameObject.SetActive(true);
			UIPlayerAssets.instance.InitList();
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
		if(UIPractice.instance)
		{
			UIPractice.instance.HidePracitce();
			UIPractice.instance.transform.SetParent(BottomUI);
			UIPractice.instance.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
			UIPractice.instance.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
		}

		if(UISkillTree.instance)
		{
			UISkillTree.instance.transform.SetParent(BottomUI);
			UISkillTree.instance.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
			UISkillTree.instance.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
		}
		
		if(UIPlayer.instance)
		{
			UIPlayer.instance.transform.SetParent(BottomUI);
			UIPlayer.instance.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
			UIPlayer.instance.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
		}
		
		
		
		

		// if(UILoading.activeSelf)
		// {
		// 	UILoading.SetActive(false);
		// }
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
		PlayerPrefs.SetString("playerSkillLevel",Player.instance.OutputSkillLevel());
		// Debug.LogFormat("playerSkillLevel:{0}",PlayerPrefs.GetString("playerSkillLevel"));
		//保存玩家技能熟练度
		PlayerPrefs.SetString("playerSkillProficiency",Player.instance.OutputSkillProficiency());
		// Debug.LogFormat("playerSkillProficiency:{0}",PlayerPrefs.GetString("playerSkillProficiency"));
		//保存玩家基础属性
		player.SaveBasicProperty();
		//保存玩家正在做的事
		if(Player.instance.playerNow=="")
		{
			Player.instance.playerNow ="P,3";
		}
		else if(Player.instance.playerNow.Substring(0,1) =="A")
		{
			Player.instance.playerNow =Player.instance.playerNow.Remove(0,1);
		}
		PlayerPrefs.SetString("nowDoing",Player.instance.playerNow);
		//保存玩家已开启的技能树
		PlayerPrefs.SetString("skillTrees","0,1");
		//保存玩家拥有的资产列表
		AssetsManager.instance.SavePlayerAssetsItem();
		//保存玩家拥有的特质列表
		player.SaveTraitList();
		
		//保存玩家解锁的技能
		Player.instance.SaveUnlockSkill();
		//保存玩家使用的技能列表
		player.SaveUsedBattleSkillListOnQuit();
		//保存角色列表
		//保存退出时间
		_lastTime =System.DateTime.Now.ToString("yyyy-MM-dd-HH:mm:ss");
		PlayerPrefs.SetString ("lastTime",_lastTime);
		// Debug.Log (_lastTime);
		PlayerPrefs.SetInt("gold",Player.instance.gold);
		PlayerPrefs.Save();
	}
}
