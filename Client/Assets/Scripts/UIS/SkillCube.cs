using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public  enum SceneType
{
    练习场=0,
    图书馆=1,
    剧院=2,

}
public class SkillCube : MonoBehaviour
{
    public Image icon;
    public Text skillName;
    public Text skillLevel;
    public int level;
    public int skillID;
    public List<int> needLevel;
    public List<SkillCube> needSkill;
    public int rank;//本技能的层级
    public Toggle toggle;
    public GameObject mask;
    int genre;
    

    public SceneType mscene;
    ///<summary>技能的状态：0，可学习，尚未学习；1，可学习已学习；2，不可学习，已知技能；3，不可见，未知技能</summary>
    public int skillState;

    //逻辑：
    //1.点击一个cube，该技能变为选中状态，移除其他cube的选中状态，屏幕下方显示该技能的信息   -->ok
    //2.初始化时，技能根据needsSkill/level 和 rank 判断当前技能为哪种skillState   -->ok
    //3.如果技能的needLevel大于0，且有needSkill，判断needSkill的level是否大于等于needLevel   -->ok
    //4.如果技能rank大于1，判断玩家当前系总等级是否大于rank所需    -->ok
    //5.如果【3】和【4】任意不满足条件，则显示为不可学习状态，mask保留    -->ok
    //6.暂时忽略未知技能
    //7.如果满足【3】和【4】的条件，判断技能当前等级是否>0，如果>0，则为已学习状态，隐藏mask，否则为可学习状态   -->ok
    //8.当有技能升级时，刷新所有的cube状态显示    -->ok
    //9.


    void Start()
    {
        toggle =GetComponentInChildren<Toggle>();
        toggle.onValueChanged.AddListener(OnBTNClick);
        StartCoroutine(WaitForInit());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator  WaitForInit()
    {
        yield return new WaitForSeconds(1f);
        InitSkillCube();
    }
    public void InitSkillCube()
    {
        //通过一个id来构建一个技能格子，获取技能的名称，图标，玩家该技能当前的等级
        //设置技能图标
        icon.sprite = Resources.Load("Texture/Skills/"+SkillManager.instance.GetInfo(skillID,"icon"),typeof(Sprite)) as Sprite;
        // Debug.LogFormat("{0}",SkillManager.instance.GetInfo(skillID,"icon"));
        //设置技能名字
        skillName.text =SkillManager.instance.GetInfo(skillID,"name");
        //设置技能等级
        level=Player.instance.GetSkillLevel(skillID);
        genre =int.Parse(SkillManager.instance.GetInfo(skillID,"genre")); 
        //公共技能如何判定？
        //genre=9：元素魔法；10：混沌魔法
        //如果等于9，那么在[0-3]里面都加入
        //如果等于10，那么在[4-7]里面都加入
        if(genre == 9)
        {
            UISkillTree.instance.skillCubes[0].Add(this);
            UISkillTree.instance.skillCubes[1].Add(this);
            UISkillTree.instance.skillCubes[2].Add(this);
            UISkillTree.instance.skillCubes[3].Add(this);
        }
        else if(genre ==10)
        {
            UISkillTree.instance.skillCubes[4].Add(this);
            UISkillTree.instance.skillCubes[5].Add(this);
            UISkillTree.instance.skillCubes[6].Add(this);
            UISkillTree.instance.skillCubes[7].Add(this);
        }
        else
        {
            UISkillTree.instance.skillCubes[genre].Add(this);
        }

        StartCoroutine(WaitForJudge());

    }
    IEnumerator WaitForJudge()
    {
        yield return new WaitForSeconds(0.1f);
        bool b3= Player.instance.ifSkillUnlock(skillID);
        bool b1 =false,b2 =false;
        
        if(needLevel.Count==0)
        {
            b1 =true;
        }
        //是否满足技能所需等级
        else
        {
            b1 =true;
            for(int i =0;i<needLevel.Count;i++)
            {
                if(needSkill[i].level<needLevel[i])
                {
                    b1 =false;
                }
            }
        }
        
        //是否满足总等级 大于 rank
        if(genre>8)
        {
            genre =UISkillTree.instance.nowPage;
        }
        if(SkillManager.instance.totalLevel[genre]>=UISkillTree.instance.levelRankTable[genre][rank-1])
        {
            b2 =true;
        }
        //如果都满足
        if(b1&&b2)
        {
            if(level>0)//这是一个已经学过的技能
            {  
                skillLevel.text =level.ToString();
                mask.SetActive(false);
                toggle.image.color =new Color(1,1,0);
                skillState =1;
            }
            else//这是一个可以学的技能
            {
                skillState =0;
                mask.SetActive(false);
                toggle.image.color =new Color(1,0,0);
            }
            
        }
        else
        {
            //这是一个不可学习的技能
            skillState =2;
        }
        if(!b3)
        {
            skillState =2;
        }
    }
    void OnBTNClick(bool isOn)
    {
        RefreashUI();
        UISkillTree.instance.ShowInformation(this);
        

    }
    public void RefreashUI()
    {
        //刷新技能等级，刷新可学习状态
        level=Player.instance.GetSkillLevel(skillID);
        if(this.gameObject.activeSelf)
        StartCoroutine(WaitForJudge());
    }
}
