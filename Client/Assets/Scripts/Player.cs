using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour 
{

	public static Player instance;
	public int Gold{get;set;}
	public int Transistor{get;set;}
	
	//各种技能的等级，存储方式：技能ID，对应等级
	private Dictionary<int,int> skillLevel =new Dictionary<int, int>();
	private Dictionary<int,int> buffSkillLevel =new Dictionary<int, int>();

	private Dictionary<int,int> skillProficiency =new Dictionary<int, int>();//技能熟练度
	//角色身上所具有的性格特质
	public List<int> traitList =new List<int>();
	
	//角色身上所具有的爱好
	//角色的装备列表？？
	//计算角色的抗性，韧性，闪避
	public List<int> buffList =new List<int>();
	List<int> usedBattleSkillList =new List<int>();
	public string playerName ="妮口小璐茶";
	
	///<summary>玩家的级别</summary>
	public int rank;
	///<summary>由资产加成，不会受到理想影响的部分</summary>
	public Actor playerActor;
	public string playerNow="";//玩家正在做的事，进入各个界面时会发生改变#无用
	public List<int> playerAssests =new List<int>();//玩家拥有的资产
	public List<AssetsItem> playerEquipItems =new List<AssetsItem>(); //玩家装备的资产
	public int playerAbyss =1;//玩家进入深渊的层数
	public List<int> playerProfessions =new List<int>();//玩家获得的职业称号
	public List<int> unlockSkills =new List<int>();
	//玩家已经解锁了哪些角色
	//玩家目前拥有多少晶体
	//玩家拥有哪些改造箱
	//玩家拥有哪些护符  护符 = 可以带进关卡，或者在关卡外也可以发挥效果的道具
	//玩家解锁了哪些遗物（能力/道具） 此处的遗物用ability  
	public List<int> unlockAbility =new List<int>();

	//玩家打通了多少关卡
	//玩家开启了哪些场景
	void Awake()
	{
		instance =this;
		
		// playerActor =GameObject.Find("Girl_01").GetComponent<Actor>();
	}

	public void Init()
	{
		string avaterName =CharacterManager.instance.GetInfo(1,"prefab");
		// if(PlayerPrefs.GetString("playerAvatar")=="")
		// {
		// 	avaterName ="Girl_01";
		// }
		// else
		// {
		// 	avaterName =PlayerPrefs.GetString("playerAvatar");
		// }
		playerActor = Instantiate((GameObject)Resources.Load("Prefabs/"+avaterName)).GetComponent<Actor>();
		playerActor.InitPlayerActor(CharacterManager.instance.GetCharacter(1));
		// playerAbyss = PlayerPrefs.GetInt("abyssLevel")==0?1:PlayerPrefs.GetInt("abyssLevel");
		// Gold = PlayerPrefs.GetInt("gold");
		Gold =100;//========================================================临时数值
		UIBasicBanner.instance.ChangeGoldText();
		// InputBasicProperty();
		// LoadTraitList();
		GetUnlockSkills();
		GetUnlockAbility();

		// InputSkillLevel();
		// InputSkillProficiency();
		// ModifierTraitProperty();
		// GetUsedBattleSkillListOnOpen();
	}
	// Update is called once per frame
	void Update () 
	{
		
	}
	// void initSkillProficiency()//测试用
	// {
	// 	skillProficiency.Add(1,0);
	// 	skillProficiency.Add(2,0);
	// 	skillProficiency.Add(3,0);
	// 	skillProficiency.Add(4,0);
	// 	skillLevel.Add(1,1);
	// 	skillLevel.Add(2,1);
	// 	skillLevel.Add(3,1);
		
	// }
	void InputBasicProperty()
	{
		
		//从PlayerPerfs中读取基础属性值
		if(LoadBasicProperty())
		{
			return;
		}
		//如果没有数值则初始化
		
	}
	public int GetSkillLevel(int key)
	{
		if(skillLevel.ContainsKey(key))
		{
			return skillLevel[key];
		}
		return 0;

	}
	///<summary>使某个技能的等级提升</summary>
	///<param name ="key">技能的id</param>
	///<param name ="level">提升的等级,负数为降级</param>
	public void AddSkillLevel(int key ,int level)
	{
		if(skillLevel[key]+level<0)
		{
			skillLevel[key] =0;
		}
		else
		{
			skillLevel[key] += level;
		}
		//增加属性的技能
		if(key==1002)//元素聚集
		{
			int level1004 =GetSkillLevel(1004);
			SkillAddBasicProperty(SkillManager.instance.GetInfo(1002).damage,"mp",0);
			SkillAddBasicProperty(level1004*SkillManager.instance.GetInfo(1004).damage,"mp",0);
		}
		if(key ==1004)//元素召唤
		{
			int level1002 =GetSkillLevel(1002);
			SkillAddBasicProperty(level1002*SkillManager.instance.GetInfo(1004).damage,"mp",0);
		}
		if(key ==1116)//炽热刺激
		{
			int level1116 =GetSkillLevel(1116);
			// SkillAddBasicProperty(SkillManager.instance.GetInfo(1116).seep,"resis",1);
		}
		//刷新技能显示等级
		if(UISkillTree.instance)
		{
			int genre =int.Parse(SkillManager.instance.GetInfo(key,"genre")); 
			UISkillTree.instance.RefreashUI(genre);
		}
	}
	
	public int GetSkillProficiency(int key)
	{
		return skillProficiency[key];
	}
	///<summary>设置一个技能的熟练度</summary>
	///<prarm name ="key">技能的id</param>
	///<prarm name ="num">增加的熟练度数值</param>
	///<prarm name ="newCurrent">增加后该技能的熟练度值</param>
	///<prarm name ="newMax">增加后该技能的最大熟练度值</param>
	public void SetSkillProficiency(int key,int num,out int newCurrent, out int newMax)
	{
		//如果还没有这个技能，那么新建这个技能
		if(!skillProficiency.ContainsKey(key))
		{
			skillProficiency.Add(key,0);
			skillLevel.Add(key,1);
		}
		//当前熟练度增加，增加值为num
		newCurrent = skillProficiency[key]+num; 
		//获取本技能当前等级的最大熟练度值
		newMax = GetSkillLevelProficiencyMax(key,GetSkillLevel(key));
		//如果当前熟练度增加之后，大于熟练度最大值，那么当前熟练度值=当前熟练度值-当前等级最大值，然后技能升级
		// bool ifLevelUp =false;
		while(newCurrent>newMax)
		{
			newCurrent-=newMax;
			AddSkillLevel(key,1);
			newMax = GetSkillLevelProficiencyMax(key,GetSkillLevel(key));
		}

		skillProficiency[key] =newCurrent;
		// Debug.LogFormat("-----{0}",skillProficiency[key]);
		
	}
	public List<int> GetLearnSkills()
	{
		List<int> list =new List<int>();
		// for (int i = 0; i < skillLevel.Count; i++)
		// {
		// 	list.Add(skillLevel[i]);	
		// }
		foreach (var item in skillLevel.Keys)
		{
			list.Add(item);
			// Debug.Log(item);
		}
		return list;
	}
	public int GetDodge()
	{
		//角色原始闪避率+技能提供的闪避+装备提供的闪避+性格特质提供的闪避
		return 0;
	}
	public int GetTough()
	{
		return 0;
	}
	public int GetRisistance(int genre)
	{
		return 0;
	}
	/// <summary> 获取某个技能某个等级时的最大熟练度 </summary>
    /// <param name="id">技能id</param>
    /// <param name="level">技能等级</param>
	int GetSkillLevelProficiencyMax(int id,int level)
	{
		//先获取技能派系
		int genre =int.Parse(SkillManager.instance.GetInfo(id,"genre"));
		//判断技能等级区间
		if(level>1000)
		{
			return 3600;
		}
		else if(level>100)
		{
			return 1000;
		}
		else if(level>10)
		{
			return 120;
		}
		else if(level>1)
		{
			return 30;
		}
		else
		{
			return 10;
		}
		
	}
	public string OutputSkillLevel()
	{
		//技能等级去掉装备buff
		TryUnEquipLevelBuffFromAssets();
		string mlevel="";
		foreach(var item in skillLevel)
		{
			mlevel+=string.Format("|{0},{1}",item.Key,item.Value); 
		} 
		return mlevel;
	}
	public string OutputSkillProficiency()
	{
		string mProficeincy ="";
		foreach(var item in skillProficiency)
		{
			mProficeincy+=string.Format("|{0},{1}",item.Key,item.Value);
		}
		return mProficeincy;
	}
	void InputSkillLevel()
	{
		string strLevel = PlayerPrefs.GetString("playerSkillLevel");
		string[] strs =strLevel.Split('|');
		for(int i =1;i<strs.Length;i++)
		{
			skillLevel.Add(int.Parse(strs[i].Split(',')[0]),int.Parse(strs[i].Split(',')[1])); 
		}
	}
	void InputSkillProficiency()
	{
		string strPro = PlayerPrefs.GetString("playerSkillProficiency");
		string[] strs =strPro.Split('|');
		for(int i =1;i<strs.Length;i++)
		{
			skillProficiency.Add(int.Parse(strs[i].Split(',')[0]),int.Parse(strs[i].Split(',')[1])); 
		}
	}

	///<summary>装备某个资产</summary>
	public void PlayerEquipAssets(AssetsItem assetsItem)
	{
		playerEquipItems.Add(assetsItem);
		//角色的属性会加上
		
		//角色的技能等级会加上
		if(assetsItem._skillBufferLevel>0)
		{
			AddSkillLevel(assetsItem._skillBuffer,assetsItem._skillBufferLevel);
			TryAddLevelFromAssets(assetsItem._skillBuffer,assetsItem._skillBufferLevel);
		}
		SavePlayerEquipAssets();
		// playerActor.InitPlayerActor();
	}

	///<summary>卸下某个资产</summary>
	public void PlayerUnEquipAssets(AssetsItem assetsItem)
	{
		playerEquipItems.Remove(assetsItem);
		//角色属性下降
		
		//角色等级下降
		if(assetsItem._skillBufferLevel>0)
		{
			AddSkillLevel(assetsItem._skillBuffer,-assetsItem._skillBufferLevel);
			TryAddLevelFromAssets(assetsItem._skillBuffer,-assetsItem._skillBufferLevel);
		}
		// playerActor.InitPlayerActor();
	}
	void TryAddLevelFromAssets(int skillid,int level)
	{
		//如果还没有这个技能，那么新建这个技能
		if(!buffSkillLevel.ContainsKey(skillid))
		{
			buffSkillLevel.Add(skillid,level);
		}
		else
		{
			buffSkillLevel[skillid]+=level;
			// //最终技能附加等级不能小于0
			// if(buffSkillLevel[skillid]<=0)
			// {
			// 	buffSkillLevel[skillid]=0;
			// }
		}
	}
	void TryUnEquipLevelBuffFromAssets()
	{
		// for (int i = 0; i < buffSkillLevel.Count; i++)
		// {
			
		// }
		foreach (var item in buffSkillLevel.Keys)
		{
			if(skillLevel.ContainsKey(item))
			{
				skillLevel[item]-=buffSkillLevel[item];
			}
		}
	}
	///<summary>保存资产列表</summary>
	void SavePlayerEquipAssets()
	{
		string s="";
		foreach (var item in playerEquipItems)
		{
			s+=",";
			s+= item.uid;
		}
		s =s.Remove(0,1);
		PlayerPrefs.SetString("equipAssets",s);
		Debug.Log(s);
	}
	public void AddBuff(int id)
	{
		buffList.Add(id);
	}
	public void RemoveBuff(int id)
	{
		buffList.Remove(id);
	}
	public void TryAddTrait(int id)
	{
		if(IfHasTrait(id))
		{
			return;
		}
		else
		{
			traitList.Add(id);
			TryAddBuffFromTrait(id);
		}
	}
	void TryAddBuffFromTrait(int id)
	{
		var trait = TraitManager.instance.GetInfo(id);
		foreach (var item in trait._buffList)
		{
			AddBuff(item);
		}
		
	}
	void TyrRmoveBuffFromTrait(int id)
	{
		var trait = TraitManager.instance.GetInfo(id);
		foreach (var item in trait._buffList)
		{
			RemoveBuff(item);
		}
	}
	public bool IfHasTrait(int id)
	{
		if(traitList.Contains(id))
		{
			return true;
		}
		return false;
	}
	///<summary>保存战斗携带的技能列表</summary>
	public void SaveUsedBattleSkillList(List<int> list)
	{
		usedBattleSkillList = list;
	}
	public List<int> GetUsedBattleSkillList()
	{
		return usedBattleSkillList;
	}
	public void SaveUsedBattleSkillListOnQuit()
	{
		if(usedBattleSkillList.Count==0)
		{
			return;
		}
		string s ="";
		foreach (var item in usedBattleSkillList)
		{
			s=s+","+item;
		}
		s =s.Remove(0,1);
		PlayerPrefs.SetString("usedBattleSkills",s);
		Debug.LogFormat("保存旧技能列表：{0}",s);
	}
	void GetUsedBattleSkillListOnOpen()
	{
		string s =PlayerPrefs.GetString("usedBattleSkills");
		if(s =="")
		{
			return;
		}
		Debug.LogFormat("保存旧技能列表：{0}",s);
		string[] ss =s.Split(',');
		for (int i = 0; i < ss.Length; i++)
		{
			usedBattleSkillList.Add(int.Parse(ss[i]));
		}

	}
	public void SkillAddBasicMp(int value)
	{
		
		if(UIPlayer.instance)
		{
			UIPlayer.instance.RefreashUI();
		}
	}
	///<summary>property:hp,mp,dodge,tough,resis</summary>
	public void SkillAddBasicProperty(int value,string property,int genre)
	{
		switch(property)
		{
			case "hP":
			// basicHp += value;
			break;
			case "mp":
			// basicMp += value;
			break;
			case "dodge":
			// basicDodge += value;
			break;
			case "tough":
			// basicTough += value;
			break;
			case "reisi":
			// basicResistance[genre] += value;
			break;
		}
		if(UIPlayer.instance)
		{
			UIPlayer.instance.RefreashUI();
		}
	}
	public void SaveBasicProperty()
	{
		// string s =basicHp+","+basicMp+","+basicDodge+","+basicTough;
		// foreach (var item in basicResistance)
		// {
		// 	s=s+","+item;
		// }
		
		// PlayerPrefs.SetString("basicProperty",s);
	}
	bool LoadBasicProperty()
	{
		string s =PlayerPrefs.GetString("basicProperty");
		if(s=="")
		{
			return false;
		}
		else
		{
			

			return true;
		}
	}
	///<summary>获取技能是否已经解锁</summary>
	public bool ifSkillUnlock(int id)
	{
		return unlockSkills.Contains(id);
	}
	public void UnlockSkill(int id)
	{
		if(unlockSkills.Contains(id))
		{
			return;
		}
		unlockSkills.Add(id);
	}
	public void SaveUnlockSkill()
	{
		if(unlockSkills.Count<1)
		{
			return;
		}
		string unlock ="";
		foreach (var item in unlockSkills)
		{
			unlock=unlock+","+item;
		}
		unlock = unlock.Remove(0,1);
		PlayerPrefs.SetString("unlockSkills",unlock);
	}
	void GetUnlockSkills()
	{
		string[] unlock =PlayerPrefs.GetString("unlockSkills").Split(',');
		if(unlock[0] =="")
		{
			unlockSkills =new List<int>{1001,1002,1003,1021};//---------------------------初始解锁的
			return;
		}
		foreach (var item in unlock)
		{
			unlockSkills.Add(int.Parse(item));
		}
	}
	public void SaveUnlockAbility()
	{
		if(unlockAbility.Count<1)
		{
			return;
		}
		string unlock ="";
		foreach (var item in unlockAbility)
		{
			unlock=unlock+","+item;
		}
		unlock = unlock.Remove(0,1);
		PlayerPrefs.SetString("unlockAbility",unlock);
	}
	void GetUnlockAbility()
	{
		string[] unlock =PlayerPrefs.GetString("unlockAbility").Split(',');
		if(unlock[0] =="")
		{
			unlockAbility =new List<int>{1,2,3,4,5,6,7,8,9,10};//---------------------------初始解锁的
			return;
		}
		foreach (var item in unlock)
		{
			unlockAbility.Add(int.Parse(item));
		}
	}
	void LoadTraitList()
	{
		string[] trait =PlayerPrefs.GetString("trait").Split(',');
		if(trait[0] =="")
		{
			traitList =new List<int>();
			return;
		}
		foreach (var item in trait)
		{
			traitList.Add(int.Parse(item));
		}
	}
	public void SaveTraitList()
	{
		string trait ="";
		foreach (var item in traitList)
		{
			trait=trait+","+item;
		}
		trait = trait.Remove(0,1);
		PlayerPrefs.SetString("trait",trait);
	}
	void ModifierTraitProperty()
	{
		if(traitList.Count<1)
		return;
		foreach (var item in traitList)
		{
			if(TraitManager.instance.GetInfo(item).buffList!="")
			{
				string[] s =TraitManager.instance.GetInfo(item).buffList.Split(',');
				for (int i = 0; i < s.Length; i++)
				{
					AddBuff(int.Parse(s[i]));
				}
			}
		}
	}
	public void AddGold(int num)
	{
		if(Gold+num<=0)
		{
			Gold =0;
		}
		else
		Gold+=num;
	}
}

