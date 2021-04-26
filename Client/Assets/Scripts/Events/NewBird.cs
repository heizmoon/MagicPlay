using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBird : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] steps;
    Animator _animator;
    Button button;
    float currentTime =0;
    float delayTime;
    public int type;
    int nowStep=0;
    bool CanContrl;
    void Start()
    {
        if(type ==0)
        {
            Init();
        }
        
    }
    void Init()
    {
        Time.timeScale=0;
        button = GetComponent<Button>();
        _animator = GetComponent<Animator>();
        _animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        delayTime =1f;
        currentTime = Time.realtimeSinceStartup;
        button.onClick.AddListener(GoOn);
    }
    // Update is called once per frame
    void Update()
    {
        if(type ==0 && CanContrl == false)
        {
            if(Time.realtimeSinceStartup>=delayTime+currentTime)
            {
                CanContrl =true;
            }
        }
        
    }
    
    void GoOn()
    {
        if(!CanContrl)
        {
            return;
        }
        if(steps.Length<=nowStep+1)
        {
            LeaveNewBird();
        }
        else
        {
            steps[nowStep].SetActive(false);
            nowStep++;
            steps[nowStep].SetActive(true);
            CanContrl=false;
        }
    }
    void LeaveNewBird()
    {
        Time.timeScale =1f;
        UIBattle.Instance.ResumeBattle();    
        UIBuffDetail g = Main.instance.allScreenUI.GetComponentInChildren<UIBuffDetail>();
        NewBird b = UIBattle.Instance.enemyBarText.GetComponentInChildren<NewBird>();
        if(g!=null)
        {
            Destroy(g.gameObject);
        }
        if(b!=null&&b.name=="NewBird_13(Clone)")
        {
            Destroy(b.gameObject);
        }
        Destroy(gameObject);
    }
    public static NewBird LoadNewBird(int i)
    {
        GameObject go = Instantiate((GameObject)Resources.Load("Prefabs/NewBird/NewBird_"+i));
        go.transform.SetParent(Main.instance.allScreenUI);
        go.transform.localPosition =Vector3.zero;
        go.transform.localScale =Vector3.one;
        go.GetComponent<RectTransform>().sizeDelta =Vector2.one;
        go.GetComponent<RectTransform>().anchoredPosition =Vector2.zero;
        return go.GetComponent<NewBird>();
    }
    /*
    记录：     预设          内容                                                            触发位置                       Main.instance.ifNewBird
            newBird0      秘境深处                                                            Main                               =0    ++     1
               --         发4张固定的手牌                                                    UIBattle.StartBattle                =1     ++     2
            newBird2      当能量值达到要求，便可以使用技能牌                                   UIBattle.Update                     =2     ++     3
            newBird3      第一次加护甲新手引导                                                Actor.AddArmor                      =3     ++     4
            newBird4      第一次补牌                                                         SkillCard.CheckIfNeedSelectCard     <=5    ++     5 or 6
            newBird5      第一次攻击行为，引导防御                                            Actor.GetSpecialSkill               <=5    ++     5 or 6
               --         模拟给玩家创建初始牌组                                              UIBattle.OnBattleGoOn               <=7    ++     7 or 8
               --         创造固定奖励                                                       UIBattleReward.Refreash             <=7    ++     7 or 8
            newBird8      点击屏幕暂停提示                                                   UIBattle.StartBattle                 =8     ++    9
            newBird9      buff点击提示                                                      UIBattle.IENewBird_8                 =9     ++    10
            newBird10     BUFF新手提醒对策说明                                               BuffIcon.ShowDetail                 <=13    ++    11 or 14
            newBird11     敌人第一次强化,点击可以查看敌人即将施放的技能详情                     Actor.GetSpecialSkill               <=11    =12    12
            newBird12     点开查看技能的文字说明，对策提醒                                    UIBattle.ShowEnemyBehavier           <=13    ++     13 or 14
            newBird13     点击技能条提示                                                     UIBattle.IENewBird_11               -      -      - 
               --         创造固定奖励                                                       UIBattleReward.Refreash             <=14   =15    15
               --         创造固定奖励                                                       UIBattleReward.Refreash             =15    ++     16
            perform_NewBird  剧情-开战                                                       UIBattle.BattleBegin                =16    =17    17
               --         什么都不做                                                        UIBattle.WaitForBattleReady          =17    -      -
            newBird17     蓝色施法条不会被打断                                                  Actor.GetSpecialSkill             <=17    =18    18
            perform_NewBird2  剧情-终结                                                      Actor.EnemyState                     =18    ++     19
               --         新手引导结束                                                       Perform.DestorySelf                  =19     ++     20

            

    */

}
