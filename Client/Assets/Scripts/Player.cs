using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour 
{

	public static Player instance;
	public int Gold{get;set;}
	public int Power{get;set;}
	public int Crystal{get;set;}
	public int CharID =0;
	
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
	
	public Actor playerActor;
	public string playerNow="";//玩家正在做的事，进入各个界面时会发生改变#无用
	public List<int> playerAssests =new List<int>();//玩家拥有的资产
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
	///<summary>获取已经解锁的技能,遗物,各种玩家进度,数值</summary>
	public void Init()
	{
		// if(PlayerPrefs.GetString("playerAvatar")=="")
		// {
		// 	avaterName ="Girl_01";
		// }
		// else
		// {
		// 	avaterName =PlayerPrefs.GetString("playerAvatar");
		// }
		
		// playerAbyss = PlayerPrefs.GetInt("abyssLevel")==0?1:PlayerPrefs.GetInt("abyssLevel");
		// Gold = PlayerPrefs.GetInt("gold");
		//========================================================临时数值
		// InputBasicProperty();
		// LoadTraitList();
		Crystal = PlayerPrefs.GetInt("Crystal");
		if(UIMain.instance)
		UIMain.instance.Refeash();
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



	
	

	public void AddBuff(int id)
	{
		buffList.Add(id);
	}
	public void RemoveBuff(int id)
	{
		buffList.Remove(id);
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
		SaveUnlockSkill();
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
		// Debug.LogError("已解锁技能:"+unlock);
	}
	void GetUnlockSkills()
	{
		string[] unlock =PlayerPrefs.GetString("unlockSkills").Split(',');
		if(unlock[0] =="")
		{
			unlockSkills =SkillManager.instance.initialUnlockSkill;//---------------------------初始解锁的
			return;
		}
		foreach (var item in unlock)
		{
			unlockSkills.Add(int.Parse(item));
		}
	}
	public void UnlockAbility(int id)
	{
		if(unlockAbility.Contains(id))
		{
			return;
		}
		unlockAbility.Add(id);
		SaveUnlockAbility();
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
			unlockAbility =AbilityManager.instance.initialUnlockAbility;//---------------------------初始解锁的
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
		if(UIBasicBanner.instance)
		UIBasicBanner.instance.RefeashText();
	}
	public void AddCrystal(int num)
	{
		if(Crystal+num<=0)
		{
			Crystal =0;
		}
		else
		Crystal+=num;
		PlayerPrefs.SetInt("Crystal",Crystal);
		if(UIMain.instance)
		UIMain.instance.Refeash();
	}
}

