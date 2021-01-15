using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkillTree : MonoBehaviour
{
    public static UISkillTree instance;
    //需要实现的：
    //1.切换标签页
    //2.
    //3.点击一个技能，会显示该技能的详情
    //4.点击技能信息中的【训练】按钮，角色开始学习这个技能
    //5.根据情况判断【训练】按钮中的文字是否为“训练”
    //6.创建技能树时，根据玩家已经拥有的技能树来创建
    public Image icon;
    public Text skillLevel;
    public Text skillName;
    public Text skillDescribe;
    public Text skillHit;
    public Text skillSeep;
    public Text skillCrit;
    public Text skillFast;
    public Text skillSpellTime;
    public GameObject information;
    public List<List<SkillCube>> skillCubes =new List<List<SkillCube>>();
    Skill skill;
    int skillID;
    public Button buttonLean;
    public int nowPage =0;
    public int[][] levelRankTable =new int[9][];
    SkillCube cube;
    public RectTransform tab;
    public List<GameObject> pages =new List<GameObject>();
    public Transform pool;
    public Transform page;
    void Awake()
    {
        instance =this;    
    }

    void Start()//需要优化pages的显示
    {
        // for(int i =0;i<page.childCount;i++)
        // {
        //    pages.Add(page.GetChild(i).gameObject);
        // }
        //初始化技能树
        string[] ss = PlayerPrefs.GetString("skillTrees").Split(',');
        if(ss[0]=="")
        {
            Debug.Log("skillTree为空,玩家未开启任何技能树");
            return;
        }
        
        for(int i =0;i<ss.Length;i++)
        {
            ToggleMagic tg = Instantiate((GameObject)Resources.Load("Prefabs/ToggleMagic")).GetComponent<ToggleMagic>();
            tg.tabName =string.Format("{0}魔法",GetGenreString(int.Parse(ss[i])));
            tg.genre =int.Parse(ss[i]);
            tg.transform.SetParent(tab.transform);
            
            tg.toggle.group =tab.GetComponent<ToggleGroup>();
            tg.GetComponent<RectTransform>().anchoredPosition3D =new Vector3(i*150-300,-100,0);
            tg.transform.localScale =Vector3.one;
            tab.sizeDelta =new Vector2((i+1)*150,100);
            Transform pg = Instantiate((GameObject)Resources.Load("Prefabs/Page_"+i)).GetComponent<Transform>();
            pg.SetParent(page);
            pg.localScale =Vector3.one;
            pg.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
            pg.GetComponent<RectTransform>().anchoredPosition3D =Vector3.zero;
            pages.Add(pg.gameObject);
            pg.gameObject.SetActive(false);
            tg.toggle.onValueChanged.AddListener((bool isOn)=>OnChangeTab(isOn,tg));
        }
        // pages[0].SetActive(true);
        InitTable();
        skill =gameObject.AddComponent<Skill>();
        buttonLean.onClick.AddListener(OnButtonClick);
        StartCoroutine(WaitForCompute());
    }
    void InitTable()
    {
        levelRankTable[0] = new int[7]{0,10,100,200,500,1000,2000};
        levelRankTable[1] = new int[7]{0,10,100,200,500,1000,2000};
        levelRankTable[2] = new int[7]{0,10,100,200,500,1000,2000};
        levelRankTable[3] = new int[7]{0,10,100,200,500,1000,2000};
        levelRankTable[4] = new int[7]{0,100,500,1000,2000,3000,5000};
        levelRankTable[5] = new int[7]{0,100,500,1000,2000,3000,5000};
        levelRankTable[6] = new int[7]{0,100,500,1000,2000,3000,5000};
        levelRankTable[7] = new int[7]{0,100,500,1000,2000,3000,5000};
        levelRankTable[8] = new int[7]{0,1000,3000,5000,10000,20000,50000};
        
        for(int i =0;i<9;i++)
        {
            skillCubes.Add(new List<SkillCube>());
        }
    }
    IEnumerator WaitForCompute()
    {
        yield return new WaitForSeconds(1.2f);
        ComputeTotalLevel();

    }

    ///<summary>点击后显示某一个技能的信息</summary>
    ///<param name="id">传入技能id</param>
    public void ShowInformation(SkillCube cb)
    {
        cube =cb;
        information.SetActive(true);
        skillID =cb.skillID;
        Actor actor = null;
        skill.InitSkill(skillID,actor);
        icon.sprite = Resources.Load("Texture/Skills/"+SkillManager.instance.GetInfo(skillID,"icon"),typeof(Sprite)) as Sprite;
        //设置技能名字
        skillName.text =skill.skillName;
        //设置技能等级
        int level=Player.instance.GetSkillLevel(skillID);
        if(level>0)
        {  
            skillLevel.text =string.Format("等级：{0}",level);
            skillLevel.color =Color.white;
        }
        else if(cb.skillState ==0)
        {
            skillLevel.text = string.Format("可学习");
            skillLevel.color =Color.green;
        }
        else if(cb.skillState==2)
        {
            //需要{0}系魔法总等级到达{1},GetGenreString(skill.genre),GetLevelRankTable(skill.genre)[cb.rank-1]
            string st =string.Format("完成特定事件以解锁技能");
            //需要显示还需满足什么条件
            if(cb.needLevel.Count>0)
            {
                skillLevel.text = string.Format("{0},以及前置魔法达到等级要求",st);
            }
            else
            {
                skillLevel.text =st;
            }
            skillLevel.color =Color.red;
            
        }
        
        if(!skill.ifActive)
        {
            skillSpellTime.text =string.Format("被动技能");
        }
        else
        {
            skillSpellTime.text =string.Format("施法时间：{0}",skill.spelllTime);
        }
        skillDescribe.text =skill.describe;
        //如果选中的技能是正在练习的技能
        // if(UIPractice.instance.skillID==skillID)
        // {
        //     buttonLean.GetComponentInChildren<Text>().text ="学习中";
        //     buttonLean.interactable=true;
            
        //     return;         
        // }
        //根据技能状态，改变按钮状态
        if(cb.skillState==0)
        {
            buttonLean.GetComponentInChildren<Text>().text ="学习";
            buttonLean.interactable=true;
        }
        else if(cb.skillState==1)
        {
            buttonLean.GetComponentInChildren<Text>().text ="练习";
            buttonLean.interactable=true;
        }
        else if(cb.skillState==2)
        {
            buttonLean.GetComponentInChildren<Text>().text ="学习";
            buttonLean.interactable=false;
        }

    }   
    public void OnButtonClick()
    {
        //判断技能类型
        //1.主动技能，进入UIPractice进行练习
        if(cube.mscene==0)
        {
            // Main.instance.StartPractice(skillID);
        }
        //2.被动技能，进入UIReading进行学习

        //其他
        //设置按钮为【练习中】状态
        buttonLean.GetComponentInChildren<Text>().text ="学习中";
        skillLevel.text =string.Format("等级：{0}",Player.instance.GetSkillLevel(skillID));
        skillLevel.color =Color.white;
        
    }
    // SkillCube GetSkillCube(int id)
    // {
    //     //通过ID来找到特定的SkillCube
    //     foreach(var item in fireSkills)
    //     {
    //         if(item.skillID == id)
    //         {
    //             return item;
    //         }
            
    //     }
    //     return null;
    // }
    public void RefreashUI(int genre)
    {

        if(genre==9)
        {
            foreach(var item in skillCubes[0])
            {
                if(item.transform.parent.parent.parent.parent.parent.gameObject.activeSelf)
                {
                    item.RefreashUI();
                }
            }
            foreach(var item in skillCubes[1])
            {
                if(item.transform.parent.parent.parent.parent.parent.gameObject.activeSelf)
                {
                    item.RefreashUI();
                }
            }
            foreach(var item in skillCubes[2])
            {
                if(item.transform.parent.parent.parent.parent.parent.gameObject.activeSelf)
                {
                    item.RefreashUI();
                }
            }
            foreach(var item in skillCubes[3])
            {
                if(item.transform.parent.parent.parent.parent.parent.gameObject.activeSelf)
                {
                    item.RefreashUI();
                }
            }
            return;
        }

        foreach(var item in skillCubes[genre])
        {
            if(item.transform.parent.parent.parent.parent.parent.gameObject.activeSelf)
            {
                item.RefreashUI();
            }
        }
    }
    int ComputeTotalLevel(int genre)//计算某一系的总等级
    {
        int total =0;
        foreach(var item in skillCubes[genre])
        {
           total += Player.instance.GetSkillLevel(item.skillID);
        }
        return total;
    }
    //计算每一系的总等级
    void ComputeTotalLevel()
    {
        for(int i=0;i<skillCubes.Count;i++)
        {
            SkillManager.instance.totalLevel[i]=(ComputeTotalLevel(i)); 
        }
    }
    ///<summary>根据int的值获取魔法种类名称</summary>
    string GetGenreString(int genre)
    {
        if(genre>8)
        {
            genre = nowPage;
        }
        switch(genre)
        {
            case 0:
            return "水";
            case 1:
            return "火";
            case 2:
            return "风";
            case 3:
            return "土";
            case 4:
            return "心灵";
            case 5:
            return "能量";
            case 6:
            return "物质";
            case 7:
            return "时空";
            case 8:
            return "真理";
        }
        return "";
    }
    int[] GetLevelRankTable(int i)
    {
        if(i>8)
        {
            i=nowPage;
        }
        return levelRankTable[i];
    }
    void OnChangeTab(bool isOn,ToggleMagic tg)
    {
        if(!isOn)
        {
            return;
        }
        foreach(var item in pages)
        {
            item.SetActive(false);
            if(int.Parse(item.name.Split('(')[0].Split('_')[1])==tg.genre)
            {
                item.SetActive(true);
                nowPage = tg.genre;
            }
        }
    }
}
