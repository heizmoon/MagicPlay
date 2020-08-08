using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIBattle : MonoBehaviour
{
    // Start is called before the first frame update
    public static UIBattle Instance;

    public Actor playerActor;
    public Actor Enemy;
    ///<summary>战斗场景</summary>
    public GameObject scene;
    public UISkillButton[] BTNSkill;//技能1按钮
    public HPBar EnemyHP;
    public HPBar PlayerHP;
    Timer timer;
    public GameObject battleOver;
    public GameObject statisticLine;
    public Button BTN_BattleOverGoOn;
    public Text battleResult;
    public Transform playerPosition;
    public Transform enemyPosition;
    public Transform playerBuffPosition;
    public Transform enemyBuffPosition;
    public Transform buffPool;

    int result;
    public bool isBattleOver =true;
    Animation anim;
    void Awake()
    {
        Instance = this;
        timer =gameObject.GetComponent<Timer>();
        anim =gameObject.GetComponent<Animation>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    ///<summary>//初始化UI</summary>
    public void Init(Actor enemy,int scene)
    {
        //1.显示敌我双方角色
        //2.显示敌我双方的生命值，魔法值
        //3.显示我方技能列表
        Enemy =enemy;
        InitBattle();
        ShowActors();
        ShowPropertys();
        InitButtons();
        CreateScene(scene);
        
    }
    void InitBattle()
    {
        playerActor =Player.instance.playerActor;
        if(!Battle.Instance)
        {
            Battle b = gameObject.AddComponent<Battle>();
            b.InitBattle(Enemy);
            return;
        }
        Battle.Instance.InitBattle(Enemy);
    }
    void ShowActors()//1.初始化敌我双方角色
    {

        Enemy.transform.SetParent(enemyPosition);
        Enemy.transform.localPosition =Vector3.zero;
        Enemy.transform.localScale =Vector3.one;
        playerActor.transform.SetParent(playerPosition);
        playerActor.transform.localPosition =Vector3.zero;
        playerActor.transform.localScale =Vector3.one;
        playerActor.target =Enemy;
        Enemy.target =playerActor;
        playerActor.InitActor();
        playerActor.InitPlayerActor();
        playerActor.GetActorSpellBar();
        playerActor.InitMagic();
        Enemy.InitActor();
        Enemy.GetActorSpellBar();
        Enemy.InitMagic();

    }
    void ShowPropertys()
    {
        EnemyHP.initHpBar(Enemy.HpCurrent,Enemy.HpMax);
        
        PlayerHP.initHpBar(playerActor.HpCurrent,playerActor.HpMax);
        // Debug.LogFormat("角色当前生命值为：{0}",playerActor.HpCurrent);
        
    }
    void InitButtons()
    {
        //根据PlayerActor的UsingSkills来修改按钮
        for(int i =0;i< playerActor.UsingSkillsID.Length;i++)
        {
            int id =playerActor.UsingSkillsID[i];
            if(id ==0)
            BTNSkill[i].gameObject.SetActive(false);
            else
            {
                BTNSkill[i].InitButton(i);
            }
        }
    }
    
    private void OnTimer(Timer timer)
    {
        
    }
    void CreateScene(int scene)
    {
        //根据id加载场景

        //根据场景效果，给角色附加buff
    }
    public void BattleBegin()
    { 
        
        //主角入场，怪物出场
        anim.Play();
        playerActor.animator.Play("run");        
        StartCoroutine(WaitForBattleReady());
    }
    IEnumerator WaitForBattleReady()
    {
        yield return new WaitForSeconds(1.25f);
        playerActor.animator.Play("idle");
        isBattleOver =false;
        Enemy.RunAI();
        playerActor.RunAI();
    }
    public void UseSkill(int id)
    {
        // playerActor.WanaSpell(playerActor.GetSkills(id));
        //触发公共CD，所有技能0.5秒内禁用
        CommonCD();
    }
    ///<summary>使用技能后产生的公共CD，不能在此期间点击其他技能按钮</summary>
    public void CommonCD()
    {
        BTNSkill[0].CommonCD();
        BTNSkill[1].CommonCD();
        BTNSkill[2].CommonCD();
        BTNSkill[3].CommonCD();
        // timer.start(0.5f,OnBTNTimer);
    }
    public void CheckButtonMP()
    {
        BTNSkill[0].ChangeMpBar();
        BTNSkill[1].ChangeMpBar();
        BTNSkill[2].ChangeMpBar();
        BTNSkill[3].ChangeMpBar();

    }
    public void BattleEnd(Actor actor)
    {
        //获取谁死了，然后判定谁胜利
        isBattleOver =true;
        if(actor ==Enemy)
        {
            battleResult.text ="胜利";
            result =1;

        }
        else
        {
            battleResult.text ="失败";
            result =0;
        }
        BTNSkill[0].ContrlButton(false);
        BTNSkill[1].ContrlButton(false);
        BTNSkill[2].ContrlButton(false);
        BTNSkill[3].ContrlButton(false);
        StartCoroutine(WaitForShowBattleOver());
    }
    IEnumerator WaitForShowBattleOver()
    {
        yield return new WaitForSeconds(1f);
        battleOver.SetActive(true);
        //显示结算
        Battle.Instance.ShowStatisticDamage(0);
        Battle.Instance.ShowStatisticDamage(1);
    }
    ///<summary>创建一条结算数据信息</summary>
    ///<param name ="type">区分是造成伤害还是受到伤害</param>
    public void CreateStatisticLine(Skill skill,int damage,string percent,int lineNumber,int type)
    {
        GameObject go = GameObject.Instantiate(statisticLine);
        go.transform.SetParent(statisticLine.transform.parent);
        go.SetActive(true);
        if(type ==0)
        {
            go.transform.localPosition =new Vector3(-310,140-lineNumber*40,0);
        }
        else if(type ==1)
        {
            go.transform.localPosition =new Vector3(50,140-lineNumber*40,0);

        }
        
        go.transform.localScale =new Vector3(1,1,1);
        if(lineNumber ==7)
        {
            go.GetComponent<UIStatisticLine>().SetText("其他",damage.ToString(),percent);
        }
        else
        {
        go.GetComponent<UIStatisticLine>().SetText(skill.skillName,damage.ToString(),percent);

        }
            
    }
    public void OnBattleGoOn()
    {
        //移除所有临时buff
        BuffManager.RemovePlayerActorTempBuff();
        EffectManager.TryThrowInPool(playerActor.castPoint);
        EffectManager.TryThrowInPool(playerActor.spellPoint);
        EffectManager.TryThrowInPool(playerActor.hitPoint);
        playerActor.target =null;
        gameObject.SetActive(false);
        SkillManager.ClearPool();
        //通知main，以下为测试效果
        if(Abyss.instance!=null)
        {
            Abyss.instance.GetBattleResult(result);
        }
        if(BattleEvent.instance!=null)
        {
            BattleEvent.instance.GetBattleResult(result);
        }
        Player.instance.playerActor.transform.SetParent(Main.instance.BottomUI);
        Player.instance.playerActor.transform.localPosition =Vector3.zero;
        Destroy(this.gameObject);
    }

    public BuffIcon CreateBuffIcon(Buff buff,bool ifHasIcon)
    {
        //查找是否已经拥有bufficon
        if(buff.target.actorType==ActorType.玩家角色)
        {
            if(playerBuffPosition.childCount>0)
            {
                foreach (var item in playerBuffPosition.GetComponentsInChildren<BuffIcon>())
                {
                    if(item.buffID ==buff.buffData.id)
                    {
                        //已经有了
                        item.buffs.Add(buff);
                        return item;
                    }
                }
            }
        }
        else
        {
            if(enemyBuffPosition.childCount>0)
            {
                foreach (var item in enemyBuffPosition.GetComponentsInChildren<BuffIcon>())
                {
                    if(item.buffID ==buff.buffData.id)
                    {
                        //已经有了
                        item.buffs.Add(buff);
                        return item;
                    }
                }
            }
        }
        //没有的情况，创建一个prefab
        BuffIcon buffIcon = ((GameObject)Instantiate(Resources.Load("Prefabs/BuffIcon"))).GetComponent<BuffIcon>();
        buffIcon.buffs.Add(buff);
        if(ifHasIcon)
        {
            if(buff.target.actorType==ActorType.玩家角色)
            {
                buffIcon.transform.SetParent(playerBuffPosition);
                buffIcon.transform.localScale =Vector3.one;
            }
            else
            {
                buffIcon.transform.SetParent(enemyBuffPosition);
                buffIcon.transform.localScale =Vector3.one;
            }
        }
        else
        {
            buffIcon.transform.SetParent(buffPool);
            buffIcon.transform.localScale =Vector3.one;
            buffIcon.transform.localPosition =Vector3.zero;
        }
        
        return buffIcon;
    }
}
