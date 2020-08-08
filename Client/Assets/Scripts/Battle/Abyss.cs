using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Data;

public class Abyss : MonoBehaviour
{
    public static Abyss instance;
    public int abyssLevel;//深渊的层数
    int currentBattle;//当前的波数
    int MaxBattle;
    int abyssScene;
    public GameObject stageUI;
    GameObject skillChooseUI;
    ///<summary>本层深渊的怪物列表</summary>
    List<int> monsterList =new List<int>();
    Actor enemy;
    public Text TextBattle;
    public Text TextEnemyName;
    EventTanser eventTanser;
    AbyssGroupData abyssGroupData;
    public GameObject successUI;
    public GameObject failUI;
    public GameObject runAwayUI;
    public GameObject deadUI;
    public List<Image> assetsItemIcons;
    public List<Text> assetsItemNames;
    public Text goldRewardText;
    public Text resultText;
    public GameObject rewardsUI;
    public GameObject resultUI;
    DropGroupSet dropGroup;
    
    
    // Start is called before the first frame update
    void Awake()
    {
        instance =this;
        // gameObject.AddComponent<ExcelMonsterGroup>().LoadInfo("Assets/Data/MonsterGroup.xlsx");
        // gameObject.AddComponent<ExcelDropGroup>().LoadInfo("Assets/Data/DropGroup.xlsx");
        dropGroup =Resources.Load<DropGroupSet>("DataAssets/DropGroup");
    }
    void Start()
    {
        eventTanser=gameObject.GetComponent<EventTanser>();
        abyssLevel =Player.instance.playerAbyss;
        this.transform.SetParent(Main.instance.allScreenUI);
        GetComponent<RectTransform>().sizeDelta = Vector2.zero;
		GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
        transform.localScale =Vector3.one;
        //读表
        LoadExcel();
        CreateAbyss();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void LoadExcel()//测试效果
    {
        int leaderId =0;
        foreach (var item in AbyssManager.instance.manager.dataArray)
        {
            if(abyssLevel>=item.startLevel&&abyssLevel<=item.endLevel)
            {
                abyssGroupData =item;
                MaxBattle=abyssGroupData.battles;
                Debug.LogFormat("PlayerPrefs:{0}",PlayerPrefs.GetString("abyssMonsters"));
                //判断是否是第一次进本层（PlayerPrefs中是否有本层数据）
                if(PlayerPrefs.GetString("abyssMonsters") != ""&&int.Parse(PlayerPrefs.GetString("abyssMonsters").Split('|')[0]) ==abyssLevel)
                {
                    //有本层数据
                    //那么怪物列表就直接沿用
                    string[] ss =PlayerPrefs.GetString("abyssMonsters").Split('|')[1].Split(',');
                    foreach (var i in ss)
                    {
                        monsterList.Add(int.Parse(i));
                    }
                    abyssScene =int.Parse(PlayerPrefs.GetString("abyssScene"));
                }
                else
                {
                    //如果没有
                    //随机出怪物
                    int monsterGroupID =ChooseGruop(abyssGroupData.monsterGroups);
                    string monsterDistrubtion = MonsterManager.instance.GetGroupInfo(monsterGroupID,"monsterDistrubtion");
                    GetMonsterList(monsterDistrubtion);
                    leaderId = int.Parse(MonsterManager.instance.GetGroupInfo(monsterGroupID,"leader"));
                    monsterList.Add(leaderId);
                    abyssScene = int.Parse(MonsterManager.instance.GetGroupInfo(monsterGroupID,"scene"));
                }
                break;
            }
        }
        
    }
    ///<summary>随机取得一个XX组</summary>
    int ChooseGruop(string str)
    {
        int groupID =0;
        string[] ss = str.Split('|');
        List<int> GroupsID =new List<int>();
        List<int> GroupsWeight =new List<int>();
        int totalWeight =0;
        foreach (var item in ss)
        {
            GroupsID.Add(int.Parse(item.Split(',')[0]));
            GroupsWeight.Add(int.Parse(item.Split(',')[1])+totalWeight);
            totalWeight += int.Parse(item.Split(',')[1]);
        }
        int r =Random.Range(0,totalWeight);
        if(r<=GroupsWeight[0])
        {
            groupID =GroupsID[0];
        }
        for(int i =1;i<GroupsWeight.Count;i++)
        {
            if (r>GroupsWeight[i-1]&&r<=GroupsWeight[i])
            {
               groupID =GroupsID[i];
               break; 
            }
        }
        return groupID;
    }
    void GetMonsterList(string s)
    {
        for(int i=0;i<MaxBattle-1;i++)
        {
            monsterList.Add(ChooseEnemy(s));
        }
        
    }
    //随机出一个怪物ID
    int ChooseEnemy(string s)
    {
        int monsterId =0;
        int totalWeight =0;
        string[] ss =s.Split('|');
        List<int> monsterIDs =new List<int>();
        List<int> weights =new List<int>();
        foreach (var item in ss)
        {
            monsterIDs.Add(int.Parse(item.Split(',')[0]));
            weights.Add(int.Parse(item.Split(',')[1])+totalWeight);
            totalWeight+=int.Parse(item.Split(',')[1]);
        }
        int r =Random.Range(0,totalWeight);
        if(r<=weights[0])
        {
            monsterId =monsterIDs[0];
        }
        for(int i =1;i<weights.Count;i++)
        {
            if (r>weights[i-1]&&r<=weights[i])
            {
               monsterId =monsterIDs[i];
               break; 
            }
        }
        return monsterId;
    }
    //获取怪物等级
    int GetEnemyLevel()
    {
        if(currentBattle ==MaxBattle-1)
        {
            //Boss战怪物等级 = 深渊组怪物等级基数 + 深渊等级 + 深渊组BOSS等级间隔
            return abyssGroupData.monsterBasicLevel + abyssLevel+abyssGroupData.bossLevelInterval;
        }
        //其他怪物等级 = 深渊组怪物等级基数 + 深渊等级 + 波数*深渊组小怪等级间隔
        return abyssGroupData.monsterBasicLevel + abyssLevel+currentBattle*abyssGroupData.monsterLevelInterval;
    }
    ///<summary>创建深渊</summary>
    void CreateAbyss()
    {
        string s ="";
        foreach (var item in monsterList)
        {
            s+=","+item.ToString();
        }
        s =s.Remove(0,1);
        s =abyssLevel+"|"+s;
        //保存怪物列表到PlayerPrefs,下次进入深渊依然使用此怪物列表
        PlayerPrefs.SetString("abyssMonsters",s);
        Debug.LogFormat("abyssMonsters:{0}",s);
        //保存深渊场景到PlayerPrefs,下次进入深渊依然使用此场景
        PlayerPrefs.SetString("abyssScene",abyssScene.ToString());
        
        Player.instance.playerNow ="A"+Player.instance.playerNow;
        // OnAbyss();
        //初始化角色当前生命值和魔法
        Player.instance.playerActor.HpCurrent =Player.instance.playerActor.HpMax;
        Player.instance.playerActor.MpCurrent =0;
        ShowSkillChooseUI();    
    }
    ///<summary>显示配置技能界面</summary>
    void ShowSkillChooseUI()
    {
        skillChooseUI =Instantiate((GameObject)Resources.Load("Prefabs/UISkillChoose"));
		skillChooseUI.transform.SetParent(Main.instance.allScreenUI);
		skillChooseUI.transform.localScale =Vector3.one;
		skillChooseUI.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
		skillChooseUI.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
        skillChooseUI.GetComponent<UISkillChoose>().confirmBTN.onClick.AddListener(OnClickSureBtn);
			
    }
    void ShowStageUI()
    {
        stageUI.SetActive(true);
        if(currentBattle==MaxBattle-1)
        {
            TextBattle.text = string.Format("最后一波");
        }
        else
        {
            TextBattle.text = string.Format("第{0}波",currentBattle+1);
        }
        TextEnemyName.text =string.Format("遭遇<color=#f22223>{0}</color>",enemy.actorName);
        Debug.LogFormat("遭遇<color=#f22223>{0}</color>",enemy.actorName);
        StartCoroutine(WaitForStageUI());
    }
    IEnumerator WaitForStageUI()
    {
        // Debug.LogWarningFormat("startWaitForStageUI");
        yield return new WaitForSeconds(1f);
        // Debug.LogWarningFormat("startInstantiateUIBattle");
        GameObject go =Instantiate((GameObject)Resources.Load("Prefabs/UIBattle"));
		go.transform.SetParent(Main.instance.allScreenUI);
		go.transform.localScale =Vector3.one;
		go.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
		go.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
        UIBattle.Instance.Init(enemy,abyssScene);
        yield return new WaitForSeconds(1f);
        UIBattle.Instance.BattleBegin();
        stageUI.SetActive(false);

    }
    ///<summary>点击确认选择按钮</summary>
    public void OnClickSureBtn()
    {
        Debug.Log("执行");
        skillChooseUI.GetComponent<UISkillChoose>().SaveOldSkills();
        skillChooseUI.SetActive(false);
        Destroy(skillChooseUI.gameObject);
        OnAbyss();
    }
    void OnAbyss()
    {
        if(currentBattle>=MaxBattle)
        {
            //战斗结束
            OnAbyssSuccess();
            return;
        }
        if(currentBattle%2==0)
        {
            CreateRelic();
            return;
        }
        EndChooseRelic();
    }
    public void EndChooseRelic()
    {
        CreateEnemy(monsterList[currentBattle],GetEnemyLevel());
        
        ShowStageUI();
        if(UIBattle.Instance)
        {
            Player.instance.playerActor.transform.SetParent(Main.instance.BottomUI);
            Destroy(UIBattle.Instance.gameObject);
        }
        currentBattle++;
    }
    void CreateRelic()
    {
        GameObject go =(GameObject)Instantiate(Resources.Load("Prefabs/UIAbyssChooseRelic"));
        go.transform.SetParent(Main.instance.allScreenUI);
        go.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        go.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
        go.transform.localScale =Vector3.one;
        go.GetComponent<UIAbyssChooseRelic>().CreateUIs(abyssGroupData.relicGroupDistribution);

    }
    void CreateEnemy(int id,int level)
    {
        MonsterTypeData monster = MonsterManager.instance.GetInfo(id);
        
        //从怪物表中创建一个具体的敌人
        enemy =null;
        enemy =Instantiate((GameObject)Resources.Load("Prefabs/Enemy/"+monster.prefab)).GetComponent<Actor>();
        enemy.actorType =ActorType.敌人;
        enemy.level =level;
        enemy.InitEnemy(monster);
        
    }
    public void AbyssEnd()
    {
        //清除PlayerPrefs中的怪物表
        PlayerPrefs.SetString("abyssMonsters","");
        
        EventManager.TaskEventEnd(eventTanser.data);
        // Main.instance.StartLoadingUI();
        // //此处应修改为一旦进入事件，时间就变化，并且默认事件为失败-------------------------------------------
        // DateManager.instance.now = DateManager.instance.DateAddtionCompute(DateManager.instance.now,eventTanser.data.timeCost);
        // eventTanser.data.doneTime++;
        // eventTanser.data._lastDate =DateManager.instance.now;
        // EventManager.ChangeDoneEvents(eventTanser.data);
        StartCoroutine(DestroyAbyss());
        // StartCoroutine(Main.instance.LoadBasicUIs());
        // Main.instance.ShowBasicBanner();
        

    }
    IEnumerator DestroyAbyss()
    {
        yield return new WaitForSeconds(2f);
        Destroy(this.gameObject);
    }
    public void GetBattleResult(int result)
    {
        if(result==1)
        {//战斗胜利
            
            OnAbyss();
        }
        else
        {//战斗失败或逃跑
            ShowResultUI(result);
        }
    }
    void ShowResultUI(int result)
    {
        resultUI.SetActive(true);
        Text _text;
        //结算界面根据结果
        switch (result)
        {
            case 0:
            //显示失败时的界面
            failUI.SetActive(true);
            resultText.text ="失败";
            _text = failUI.GetComponent<Text>();
            eventTanser.data.result =2;
            break;
            case 1:
            //显示成功时的界面
            successUI.SetActive(true);
            resultText.text ="成功";
            rewardsUI.SetActive(true);
            Player.instance.playerAbyss++;
            PlayerPrefs.SetInt("abyssLevel",Player.instance.playerAbyss);
            _text = successUI.GetComponent<Text>();
            eventTanser.data.result =1;
            break;
            case 2:
            //显示逃跑时的界面
            runAwayUI.SetActive(true);
            resultText.text ="失败";
            _text = runAwayUI.GetComponent<Text>();
            eventTanser.data.result =2;
            break;
            default:
            _text = runAwayUI.GetComponent<Text>();
            break;
        }
        //复活角色
        Player.instance.playerActor.GetComponent<Actor>().ReLiveActor();
        _text.text =string.Format(_text.text,eventTanser.data.timeCost,abyssLevel);
    }
    void OnAbyssSuccess()
    {
        //计算奖励
        ComputeDrops();
        ShowResultUI(1);
    }
    void ComputeDrops()
    {
        string dropList ="";
        foreach (var item in dropGroup.dataArray)
        {
            if(item.id ==ChooseGruop(abyssGroupData.dropGropDistribution))
            {
               dropList =item.list; 
            }
        }
       if(dropList =="")
       {
           return;
       }
       string[] lists =dropList.Split('|');
       int num =0;
       foreach (var item in lists)
       {
           int i = int.Parse(item.Split(',')[1]);
           int r =Random.Range(0,100);
           if(r<=i)
           {
               //掉落：生成一个assestItem,并将它加入玩家的列表
               AssetsItem assetsItem = AssetsManager.instance.CreateNewAssets(int.Parse(item.Split(',')[0]),abyssLevel);
               AssetsManager.instance.GiveAssetsToPlayer(assetsItem);
               assetsItemIcons[num].sprite = Resources.Load("Texture/Assets/"+assetsItem._icon,typeof(Sprite)) as Sprite;
               assetsItemNames[num].text = assetsItem._name;
               assetsItemIcons[num].transform.parent.gameObject.SetActive(true);
               num++;
               Debug.Log("生成");
           } 
       }
       //金钱奖励
       int goldReward = abyssGroupData.goldReward;
       goldRewardText.text =goldReward.ToString();
       Player.instance.gold+=goldReward;
    }
    
}
