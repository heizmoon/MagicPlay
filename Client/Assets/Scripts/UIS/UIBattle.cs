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
    //public UISkillButton[] BTNSkill;//技能1按钮
    public HPBar EnemyHP;
    public HPBar PlayerHP;
    Timer timer;
    public GameObject battleOver;
    public GameObject statisticLine;
    public Button BTN_BattleOverGoOn;
    public Text enemyBarText;
    public Text battleResult;
    public Transform t_playerPosition;
    public Transform t_enemyPosition;
    public Transform t_playerBuffPosition;
    public Transform t_enemyBuffPosition;
    public Transform t_buffPool;
    public Transform t_cardsPool;
    public Transform t_handCards;
    public GameObject g_settingPannel;

    [HideInInspector]
    ///<summary>牌堆列表</summary>
    public List<SkillCard> cardsList;
    [HideInInspector]
    ///<summary>弃牌堆列表</summary>
    public List<SkillCard> usedCardsList;
    [HideInInspector]
    ///<summary>移除牌堆列表</summary>
    public List<SkillCard> removeCarsList;

    int result;
    public bool isBattleOver =true;
    Animation anim;
    bool isBoss;

    Dictionary<int,bool> cardPos =new Dictionary<int, bool>();
    void Awake()
    {
        Instance = this;
        timer =gameObject.GetComponent<Timer>();
        anim =gameObject.GetComponent<Animation>();
    }
    public void OnPressSetting()
    {
        //调出设置界面
        //将游戏速度调整到0.001
        Time.timeScale =0.01f;
        g_settingPannel.SetActive(true);
    }
    public void OnQuitSetting()
    {
        Time.timeScale =1f;
        g_settingPannel.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    ///<summary>//初始化UI</summary>
    public void Init(Actor enemy,int scene,bool isBoss)
    {
        //1.显示敌我双方角色
        //2.显示敌我双方的生命值，魔法值
        //3.显示我方技能列表
        Enemy =enemy;
        InitBattle();
        ShowActors();
        ShowPropertys();
        InitSkillCards();
        CreateScene(scene);
        this.isBoss = isBoss;
        //清理手牌
        cardPos.Clear();
        for (int i = 0; i < 6; i++)
        {
            cardPos.Add(i,false);
        }
        
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

        Enemy.transform.SetParent(t_enemyPosition);
        Enemy.transform.localPosition =Vector3.zero;
        Enemy.transform.localScale =Vector3.one;
        playerActor.transform.SetParent(t_playerPosition);
        playerActor.transform.localPosition =Vector3.zero;
        playerActor.transform.localScale =Vector3.one;
        playerActor.target =Enemy;
        Enemy.target =playerActor;
        BackUpActor();//备份角色数据
        playerActor.InitActor();
        // playerActor.InitPlayerActor();
        playerActor.GetActorSpellBar();
        playerActor.InitMagic();
        Enemy.InitActor();
        Enemy.GetActorSpellBar();
        SetEnemyBarText(0);
        Enemy.InitMagic();

    }
    void BackUpActor()
    {
        ActorBackUp backUp = gameObject.AddComponent<ActorBackUp>();
        backUp.BackUp(playerActor);
    }
    void ShowPropertys()
    {
        EnemyHP.initHpBar(Enemy.HpCurrent,Enemy.HpMax);
        
        PlayerHP.initHpBar(playerActor.HpCurrent,playerActor.HpMax);
        // Debug.LogFormat("角色当前生命值为：{0}",playerActor.HpCurrent);
        
    }
    //抽4张卡
    void InitSkillCards()
    {
        //创建玩家所有的技能卡
        for (int i = 0; i < playerActor.skills.Count; i++)
        {
            GameObject go = Instantiate((GameObject)Resources.Load("Prefabs/SkillCard"));
            SkillCard skillCard = go.GetComponent<SkillCard>();
            go.transform.SetParent(t_cardsPool);
            skillCard.Init(playerActor.skills[i]);
            cardsList.Add(skillCard);
        }
        DealCards();
    }
    
    public void SetEnemyBarText(int state)
    {
        Debug.LogWarning("敌人的状态是"+state);
        switch(state)
        {
            case 0:
            enemyBarText.text ="";
            break;
            case 1:
            enemyBarText.text ="敌人准备<color=#f22223>攻击</color>";
            break;
            case 2:
            enemyBarText.text ="敌人准备<color=#f22223>防御</color>";
            break;
            case 3:
            enemyBarText.text ="敌人准备<color=#f22223>强化</color>";
            break;
            case 4:
            enemyBarText.text ="敌人准备<color=#f22223>削弱</color>";
            break;
        }
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
        
    }
    public void CheckButtonMP()
    {
        // BTNSkill[0].ChangeMpBar();
        // BTNSkill[1].ChangeMpBar();
        // BTNSkill[2].ChangeMpBar();
        // BTNSkill[3].ChangeMpBar();

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
        // BTNSkill[0].ContrlButton(false);
        // BTNSkill[1].ContrlButton(false);
        // BTNSkill[2].ContrlButton(false);
        // BTNSkill[3].ContrlButton(false);
        StartCoroutine(WaitForShowBattleOver());
    }
    IEnumerator WaitForShowBattleOver()
    {
        yield return new WaitForSeconds(1f);
        battleOver.SetActive(true);
        //显示结算
        // Battle.Instance.ShowStatisticDamage(0);
        // Battle.Instance.ShowStatisticDamage(1);
        //选择能力奖励
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
        RecoverActor();//还原角色备份
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
        BattleScene.instance.BattleEnd(isBoss);
        CreateRelic();
        Destroy(this.gameObject);
    }
    void RecoverActor()
    {
        GetComponent<ActorBackUp>().Recover(playerActor);
    }

    public BuffIcon CreateBuffIcon(Buff buff,bool ifHasIcon)
    {
        //查找是否已经拥有bufficon
        if(buff.target.actorType==ActorType.玩家角色)
        {
            if(t_playerBuffPosition.childCount>0)
            {
                foreach (var item in t_playerBuffPosition.GetComponentsInChildren<BuffIcon>())
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
            if(t_enemyBuffPosition.childCount>0)
            {
                foreach (var item in t_enemyBuffPosition.GetComponentsInChildren<BuffIcon>())
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
                buffIcon.transform.SetParent(t_playerBuffPosition);
                buffIcon.transform.localScale =Vector3.one;
                buffIcon.transform.localPosition =Vector3.zero;

            }
            else
            {
                buffIcon.transform.SetParent(t_enemyBuffPosition);
                buffIcon.transform.localScale =Vector3.one;
                buffIcon.transform.localPosition =Vector3.zero;

            }
        }
        else
        {
            buffIcon.transform.SetParent(t_buffPool);
            buffIcon.transform.localScale =Vector3.one;
            buffIcon.transform.localPosition =Vector3.zero;
        }
        
        return buffIcon;
    }
    //抽齐4张手牌(发牌)
    public void DealCards()
    {
        for (int i = 0; i < playerActor.dealCardsNumber ; i++)
        {
            if(cardsList.Count<1)
            Reshuffle();//洗牌
            if(cardsList.Count<1)
            {
                //--------洗牌也没牌了，那就不抽了
                Debug.Log("没牌了");
                return;
            }
            

            int r = Random.Range(0,cardsList.Count);
            if(playerActor.handCards.Count<8)
            StartCoroutine(IESelectCard(cardsList[r],i));
            else
            cardsList[r].ThrowCard();

            cardsList.RemoveAt(r);
            Debug.Log("本次抽到的是第 "+r +" 张");
        }
    }
    IEnumerator IESelectCard(SkillCard skillCard,int delayNumber)
    {
        yield return new WaitForSeconds(0.5f+delayNumber*0.2f);
        skillCard.posID =GetCardPos();
        skillCard.GiveToHand();
        AddCardPos(skillCard.posID);
        // Debug.Log("posID ="+skillCard.posID);
    }
    //用于给技能卡定位
    int GetCardPos()
    {
        Debug.LogWarning(cardPos.Count);
        
        foreach (var item in cardPos)
        {
            // Debug.LogWarning("GetCardPos.cardPos ="+item.Key+",posValue="+item.Value);
            if(!item.Value)
            { 
                return item.Key;
            }
        }
        return 0;
        
    }
    void AddCardPos(int num)
    {
        cardPos[num] = true;
        // Debug.Log("cardPos["+num+"]="+cardPos[num]);
    }
    public void RemoveCardPos(int num)
    {
        cardPos[num] = false;
    }
    ///<summary>将所有手牌丢入弃牌堆</summary>
    public void ThrowAllHandCardsToPool()
    {
        foreach (var item in playerActor.handCards)
        {
            cardPos[item.posID] =false;
            item.ThrowCard();
        }
        DealCards();
    }
    ///<summary>将一定数量的手牌丢入弃牌堆</summary>
    public void ThrowHandCardsToPool(int num)
    {
        for (int i = 0; i < num; i++)
        {
            //如果已经没有手牌，则停止，并抽卡
            if(playerActor.handCards.Count==0)
            {
                DealCards();
                return;
            }
            //随机一张手牌；丢入弃牌堆
            SkillCard skillCard =RandomSelectACard();
            cardPos[skillCard.posID] =false;
            skillCard.ThrowCard();
        }
        SkillCard.CheckIfNeedSelectCard();
    }
    SkillCard RandomSelectACard()
    {
        int r = Random.Range(0,playerActor.handCards.Count);
        return playerActor.handCards[r];
    }
    ///<summary>临时创建一张牌</summary>
    public SkillCard CreateNewCardTemp(int id)
    {
        Skill skill = SkillManager.TryGetFromPool(id,playerActor);
        skill.InitSkill(id,playerActor);
        GameObject go = Instantiate((GameObject)Resources.Load("Prefabs/SkillCard"));
        SkillCard skillCard = go.GetComponent<SkillCard>();
        go.transform.SetParent(t_cardsPool);
        skillCard.Init(skill);
        // cardsList.Add(skillCard);
        return skillCard;
    }
    public void CreateNewCardAndGiveToHand(int id)
    {
        if(playerActor.handCards.Count<8)
        StartCoroutine(IESelectCard(CreateNewCardTemp(id),0));
        else
        CreateNewCardTemp(id).ThrowCard();
    }

    //从卡堆中抽N张手牌
    public void SelectCard(int number)
    {
        for (int i = 0; i < number ; i++)
        {
            if(cardsList.Count<1)
            Reshuffle();//洗牌
            if(cardsList.Count<1)
            return;//洗牌也还是没牌，那就不抽了

            int r = Random.Range(0,cardsList.Count);
            Debug.Log("r="+r);
            if(playerActor.handCards.Count<8)
            StartCoroutine(IESelectCard(cardsList[r],i));
            else
            cardsList[r].ThrowCard();

            cardsList.RemoveAt(r);
            Debug.Log("本次抽到的是第 "+r +" 张");
        }
    }
    //将弃牌堆中的所有牌加入牌堆
    public void Reshuffle()
    {
        for (int i = 0; i < usedCardsList.Count; i++)
        {
            cardsList.Add(usedCardsList[i]);
        }
        usedCardsList.Clear();
    }
    //技能牌因手牌过多而无法加入手牌

    void CreateRelic()
    {
        GameObject go =(GameObject)Instantiate(Resources.Load("Prefabs/UIAbyssChooseRelic"));
        go.transform.SetParent(Main.instance.allScreenUI);
        go.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        go.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
        go.transform.localScale =Vector3.one;
        go.GetComponent<UIAbyssChooseRelic>().CreateUIs(3);

    }

}
