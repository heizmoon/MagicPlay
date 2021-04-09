using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class UIBattle : MonoBehaviour
{
    // Start is called before the first frame update
    public static UIBattle Instance;

    public Actor playerActor;
    public Actor Enemy;
    ///<summary>战斗场景</summary>
    public GameObject scene;
    public HPBar EnemyHP;
    public HPBar PlayerHP;
    Timer timer;
    public GameObject battleOver;
    public GameObject statisticLine;
    public Button BTN_BattleOverGoOn;
    public Button BTN_shieldTip_1;
    public Button BTN_shieldTip_2;
    public Button BTN_coldTip_1;
    public Button BTN_coldTip_2;

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

    public Button btn_pause;
    public Button btn_play;
    public bool ifPause;

    ///<summary>牌堆列表</summary>
    public List<SkillCard> cardsList;
    ///<summary>弃牌堆列表</summary>
    public List<SkillCard> usedCardsList;
    [HideInInspector]
    ///<summary>移除牌堆列表</summary>
    public List<SkillCard> removeCarsList;

    ///<summary>当前拥有的所有卡</summary>
    public List<SkillCard> allCards;
    int result;
    public bool isBattleOver =true;
    Animation anim;
    bool isBoss;
    [SerializeField]
    Dictionary<int,bool> cardPos =new Dictionary<int, bool>();
    bool battleStart;
    public float BattleTime =0;
    bool ifFirstDeal =true;
    public event Action<int,int> OnUseCardAction;
    public event Action<int> OnLegacyCardAction;
    public event Action<int> OnThrowCardAction;
    public event Action<int> OnDealCardAction;
    Dictionary<int,int> useCardTimes;
    int legacyCardTimes;
    int dealCardTimes;


    void Awake()
    {
        Instance = this;
        timer =gameObject.GetComponent<Timer>();
        anim =gameObject.GetComponent<Animation>();
        btn_pause.onClick.AddListener(PauseBattle);
        btn_play.onClick.AddListener(ResumeBattle);
        BTN_shieldTip_1.onClick.AddListener(OnShieldTips);
        BTN_shieldTip_2.onClick.AddListener(OnShieldTips);
        BTN_coldTip_1.onClick.AddListener(OnColdTips);
        BTN_coldTip_2.onClick.AddListener(OnColdTips);
    }
    public void OnPressSetting()
    {
        //调出设置界面
        //将游戏速度调整到0.001
        Time.timeScale =0.001f;
        g_settingPannel.SetActive(true);
    }
    public void OnQuitSetting()
    {
        Time.timeScale =1f;
        g_settingPannel.SetActive(false);

    }
    public void OnQuitBattle()
    {
        Time.timeScale =1f;
        StartCoroutine(WaitForShowBattleOver(0,2f));
    }
    // Update is called once per frame
    void Update()
    {
        if(battleStart)
        {
            BattleTime+=Time.deltaTime;
        }
        if(Main.instance.ifNewBird==0)//新手引导1
        {
            if(playerActor.MpCurrent>3.3f)
            {
                Main.instance.ifNewBird++;
                NewBird.LoadNewBird(0);
            }
        }
        
    }
    ///<summary>//初始化UI</summary>
    public void Init(Actor enemy,int scene,bool isBoss)
    {
        cardPos.Clear();
        for (int i = 0; i < 8; i++)
        {
            cardPos.Add(i,false);
        }
        useCardTimes=new Dictionary<int, int>();        //1.显示敌我双方角色
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
        SetEnemyBarText(0,0);
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
        playerActor.MpCurrent = playerActor.initialMP;
        // Debug.LogFormat("角色当前生命值为：{0}",playerActor.HpCurrent);
        
    }
    void StartBattle()//--------------包含新手引导部分-----------------------------
    {
        battleStart=true;
        //开战物品生效
        StartCoroutine(IEWaitForOpenAbility());
        Shuffle();//洗牌
        if(Main.instance.ifNewBird==0)
        {
            //关闭可以点击屏幕暂停的功能！
            //手动发牌，3张攻击

            CreateNewCardAndGiveToHand(2,0);
            CreateNewCardAndGiveToHand(2,1);
            CreateNewCardAndGiveToHand(2,2);

            //怪物初始行为：发呆
            //当能量超过1张攻击牌的消耗时，暂停游戏，引导玩家使用一张攻击牌。-----------newbird00

            //当怪物受到1张攻击牌伤害后，怪物开始不停防御
            //当怪物第一次成功释放防御后，暂停游戏，告诉玩家护甲值可以抵挡攻击，并且护甲值会随时间自动消失-----------newbird01

            //当玩家手牌为1张时，模拟发牌，并暂停游戏，告诉玩家当手牌小于等于1张的时候，会自动补牌-----------newbird02
            //补牌会补充2张格挡 1张攻击

            //补牌结束后，怪物正好开始攻击，攻击即将结束时，提醒玩家使用格挡 来阻挡怪物的攻击？？？可以不提醒

            //后续补牌变为正常
        }
        if(Main.instance.ifNewBird==1)
        {
            //开启可以点击屏幕暂停的功能
            //怪物行为：发呆 buff:护甲持续时间无限，并初始拥有20层护甲
            //暂停，提醒玩家点击屏幕中心可以暂停战斗
            //提醒玩家观察怪物BUFF
            //当玩家关闭buff提示后，告诉玩家暂停状态时点击卡牌，可以查看卡牌效果详情
            //玩家拥有穿透伤害的牌，打断的牌
            //战斗结束后升级 给玩家防御流的遗物
            //使用后能量消耗降低，并复制一张
            //
        }
        if(Main.instance.ifNewBird==2)
        {
            //战斗开始前先播剧情
            //当怪物生命值到50%时再播剧情
            //结束战斗
            DealCards();//发牌
        }
        else
        DealCards();//发牌
    }
    IEnumerator IEWaitForOpenAbility()
    {
        yield return new WaitForSeconds(0.1f);
        // if(playerActor.abilities.Contains(5))//充能护符
        // {
        //     playerActor.AddMp(100);
        // }
        // if(playerActor.abilities.Contains(7))//樱桃罐头
        // {
        //     BuffManager.instance.CreateBuffForActor(1012,playerActor);    
        // }
        // if(playerActor.abilities.Contains(9))//震撼登场
        // {
        //     BuffManager.instance.CreateBuffForActor(1011,Enemy);    
        // }
        for (int i = 0; i < playerActor.abilities.Count; i++)
        {
            AbilityData ability = AbilityManager.instance.GetInfo(playerActor.abilities[i]);
            Actor target;
            if(ability.targetSelf)
            target = playerActor; 
            else
            target =Enemy;
            for (int j = 0; j < ability.buffNum; j++)
            {
                BuffManager.instance.CreateBuffForActor(ability.buffID,target);   
            }
        }
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
            allCards.Add(skillCard);
        }   
    }
    void PauseBattle()
    {
        if(Main.instance.ifNewBird<=3)
        {
            return;
        }
        ifPause = true;
        btn_play.gameObject.SetActive(true);
        btn_pause.gameObject.SetActive(false);
        Time.timeScale = 0f;
    }
    void ResumeBattle()
    {
        ifPause = false;
        btn_play.gameObject.SetActive(false);
        btn_pause.gameObject.SetActive(true);
        Time.timeScale = 1f;
    }
    public void SetEnemyBarText(int state,int damage)
    {
        // Debug.LogWarning("敌人的状态是"+state);
        switch(state)
        {
            case 0:
            enemyBarText.text ="";
            break;
            case 1:
            enemyBarText.text =string.Format("准备<color=#f22223>攻击:{0}</color>",damage+Enemy.basicAttack);
            break;
            case 2:
            enemyBarText.text ="准备<color=#f22223>防御</color>";
            break;
            case 3:
            enemyBarText.text ="准备<color=#f22223>强化</color>";
            break;
            case 4:
            enemyBarText.text ="准备<color=#f22223>削弱</color>";
            break;
        }
    }
    public void SetEnemyBarText()
    {
        enemyBarText.text ="正在发呆…";
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
        StartBattle();       
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
        StartCoroutine(WaitForShowBattleOver(result,2f));
    }
    IEnumerator WaitForShowBattleOver(int result,float delay)
    {
        yield return new WaitForSeconds(delay);
        // battleOver.SetActive(true);
        //显示结算
        // Battle.Instance.ShowStatisticDamage(0);
        // Battle.Instance.ShowStatisticDamage(1);
        ifPause = true;
        BuffManager.RemovePlayerActorAllBuff();//------------移除所有buff
        BuffManager.RemoveActorAllBuff(Enemy);
        playerActor.cardMpReduce =0;
        playerActor.SummonedLifeTimePlus =0;
        playerActor.ChannelSkill(-999);
        playerActor.AddCold(-999);
        playerActor.handCards =new List<SkillCard>();
        Player.instance.playerActor.transform.SetParent(Main.instance.BottomUI);
        Player.instance.playerActor.transform.localPosition =Vector3.zero;
        playerActor.target =null;
        playerActor.ClearSummon();
        playerActor.armor =0;
        RecoverActor();//还原角色备份
        Enemy.gameObject.SetActive(false);
        
        if(result==1)
        {
            //胜利了
            //如果不是最终BOSS，选择能力奖励
            //怎样判断是否是最终BOSS？
            if(BattleScene.instance.beatBossNumber>1&&isBoss)
            {
                //最终BOSS，跳过
            }
            else
            {
                if(!Configs.instance.ifChangMode)
                ShowReward();
                else
                ShowChangeCard();
            }
            
            BattleScene.instance.BattleEnd(isBoss);
        }
        else
        {
            //失败了
            //显示失败UI
            UIBattleFail.CreateUI();
        }
        // BuffManager.RemovePlayerActorTempBuff();
        OnBattleGoOn();

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
        
        EffectManager.TryThrowInPool(playerActor.castPoint);
        EffectManager.TryThrowInPool(playerActor.spellPoint);
        EffectManager.TryThrowInPool(playerActor.hitPoint);
        gameObject.SetActive(false);
        SkillManager.ClearPool();
        
        //通知main，以下为测试效果
        // if(Abyss.instance!=null)
        // {
        //     Abyss.instance.GetBattleResult(result);
        // }
        // if(BattleEvent.instance!=null)
        // {
        //     BattleEvent.instance.GetBattleResult(result);
        // }
        if(Main.instance.ifNewBird==3)
        {
            playerActor.UsingSkillsID.Add(2);
            playerActor.UsingSkillsID.Add(2);
            playerActor.UsingSkillsID.Add(2);
            playerActor.UsingSkillsID.Add(102);
            playerActor.UsingSkillsID.Add(102);

        }
        
        
        Destroy(this.gameObject);
    }
    void RecoverActor()
    {
        Debug.LogWarning("还原备份");
        GetComponent<ActorBackUp>().Recover(playerActor);
    }

    public BuffIcon CreateBuffIcon(Buff buff,bool ifHasIcon)
    {
        BuffIcon buffIcon = null;
        if(buff.buffData.maxNum !=-1)
        {
            buffIcon=CheckBuffIcon(buff);
        }
        if(buffIcon)
        {
            buffIcon.buffs.Add(buff);
            return buffIcon;
        }
        else
        {
        //没有的情况，创建一个prefab
        buffIcon = ((GameObject)Instantiate(Resources.Load("Prefabs/BuffIcon"))).GetComponent<BuffIcon>();
        buffIcon.buffs.Add(buff);
        // if(buff.buffData.id==1003)
        // {
        //     Debug.Log("buffIcon.buffs[0]="+buffIcon.buffs[0].buffData.name);
        // }
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

    }
    public BuffIcon CheckBuffIcon(Buff buff)
    {
        if(buff.buffData.maxNum>=0)
        {
            //查找是否已经拥有bufficon
            if(buff.buffData.icon=="")
            {
                if(t_buffPool.childCount>0)
                {
                    foreach (var item in t_buffPool.GetComponentsInChildren<BuffIcon>())
                    {
                        if(item.buffID ==buff.buffData.id)
                        {
                            //已经有了
                            return item;
                        }
                    }
                }
            }
            if(buff.target.actorType==ActorType.玩家角色)
            {
                if(t_playerBuffPosition.childCount>0)
                {
                    foreach (var item in t_playerBuffPosition.GetComponentsInChildren<BuffIcon>())
                    {
                        if(item.buffID ==buff.buffData.id)
                        {
                            //已经有了
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
                            return item;
                        }
                    }
                }
            }
        }
        return null;
    }
    public void OnUseCard(SkillCard skillCard)
    {
        int type = skillCard.skill.skillData.type;
        if(useCardTimes.ContainsKey(type))
        {
            useCardTimes[type]++;
        }
        else
        {
            useCardTimes.Add(type,1);
        }

        if(OnUseCardAction!=null)
        {
            OnUseCardAction(type,useCardTimes[type]);
        } 
    }
    public void OnLegacyCard()
    {
        legacyCardTimes++;
        if(OnLegacyCardAction!=null)
        {
            OnLegacyCardAction(legacyCardTimes);
        } 
    }
    public void OnThorwCard(int num)
    {
        if(OnThrowCardAction!=null)
        {
            OnThrowCardAction(num);
        } 
    }
    ///<summary>抽齐4张手牌(补牌)</summary>
    public void DealCards()
    {
        
        if(OnDealCardAction!=null)
        {
            dealCardTimes++;
            Debug.Log("第"+dealCardTimes+"次补牌");
            OnDealCardAction(dealCardTimes);
        }
        //检测当补牌时就触发的buff，道具
        // if(playerActor.abilities.Contains(7))//樱桃罐头
        // {
        //     Skill skill =SkillManager.TryGetFromPool(26,playerActor);
        //     skill.ComputeHeal();
        // }
        // if(playerActor.abilities.Contains(6))//凸透镜
        // {
        //     Skill skill =SkillManager.TryGetFromPool(27,playerActor);
        //     skill.ComputeDamage();
        // }
        //检查补牌前是否有卡牌被【遗留】
        if(playerActor.handCards.Count>0)
        {
            for (int i = 0; i < playerActor.handCards.Count; i++)
            {
                playerActor.handCards[i].LegacyCard();
            }
        }
        int maxNum = playerActor.dealCardsNumber;
        if(ifFirstDeal)
        {
            maxNum =playerActor.startBattleDealCardsNumber;
            ifFirstDeal =false;
        }
        if(maxNum>8)
        maxNum=8;//补牌数量上限为8
        for (int i = 0; i < maxNum ; i++)
        {
            if(cardsList.Count<1)
            ReloadCards();//洗牌
            if(cardsList.Count<1)
            {
                //--------洗牌也没牌了，那就不抽了
                // Debug.LogError("没牌了!");
                return;
            }
            // int r = Random.Range(0,cardsList.Count);
            if(playerActor.handCards.Count<8)
            SelectCard(cardsList[0],i);
            else
            cardsList[0].ThrowCard();

            cardsList.RemoveAt(0);
            // Debug.Log("本次抽到的是第 "+0 +" 张");
        }
    }
    ///<summary>抽到某张牌,并在延迟时间后将其加入手牌中</summary>
    void SelectCard(SkillCard skillCard,int delayNumber)
    {
        skillCard.posID =GetCardPos();
        AddCardPos(skillCard.posID);
        skillCard.GiveToHand(0.5f+delayNumber*0.2f);

        // yield return new WaitForSeconds();
        
        // Debug.Log("posID ="+skillCard.posID);
    }
    //用于给技能卡定位
    int GetCardPos()
    {
        // Debug.LogWarning(cardPos.Count);
        
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
    public int ThrowHandCardsToPool(int num)
    {
        int throwNum =0;
        Debug.Log("将 "+num +" 张手牌丢入弃牌堆");
        for (int i = 0; i < num; i++)
        {
            //如果已经没有手牌，则停止，并抽卡
            if(playerActor.handCards.Count==0)
            {
                DealCards();
                return i;
            }
            //随机一张手牌；丢入弃牌堆
            SkillCard skillCard =RandomSelectACard();
            cardPos[skillCard.posID] =false;
            skillCard.ThrowCard();
            throwNum++;
        }
        SkillCard.CheckIfNeedSelectCard();
        return throwNum;//----------------此处可能遗留有顺序问题
    }
    SkillCard RandomSelectACard()
    {
        int r =UnityEngine.Random.Range(0,playerActor.handCards.Count);
        return playerActor.handCards[r];
    }
    ///<summary>临时创建一张特殊ID的牌</summary>
    public SkillCard CreateNewCardTemp(int id)
    {
        Skill skill = SkillManager.CreateSkillForActor(id,playerActor);
        GameObject go = Instantiate((GameObject)Resources.Load("Prefabs/SkillCard"));
        SkillCard skillCard = go.GetComponent<SkillCard>();
        go.transform.SetParent(t_cardsPool);
        skillCard.Init(skill);
        skill.realManaCost -=Player.instance.playerActor.cardMpReduce;
        skillCard.RefeashCardShow();
        allCards.Add(skillCard);
        skillCard.CheckBuffCardWhileCreateCard();
        return skillCard;
    }
    public void CreateNewCardAndGiveToHand(int id,int delayNumber)
    {
        if(playerActor.handCards.Count<8)
        SelectCard(CreateNewCardTemp(id),delayNumber);
        else
        CreateNewCardTemp(id).ThrowCard();
    }

    //从卡堆中抽N张手牌
    public void SelectSomeCards(int number)
    {
        Debug.Log("从牌堆中抽 "+number +" 张牌");
        for (int i = 0; i < number ; i++)
        {
            if(cardsList.Count<1)
            ReloadCards();//重装牌堆
            if(cardsList.Count<1)
            {
                Debug.LogError("洗牌后依然没牌，无法抽牌了！");
                return;//洗牌也还是没牌，那就不抽了
            }

            // int r = Random.Range(0,cardsList.Count);
            // Debug.Log("r="+r);
            if(playerActor.handCards.Count<8)
            SelectCard(cardsList[0],i);
            else
            cardsList[0].ThrowCard();

            cardsList.RemoveAt(0);
            // 
        }
    }
    //将弃牌堆中的所有牌加入牌堆:重装牌堆
    public void ReloadCards()
    {
        Debug.Log("将弃牌堆中的所有牌加入牌堆");
        for (int i = 0; i < usedCardsList.Count; i++)
        {
            cardsList.Add(usedCardsList[i]);
        }
        usedCardsList.Clear();
        Shuffle();
    }
    //洗牌,运用洗牌算法，将牌堆洗牌
    void  Shuffle()
    {
        Debug.Log("洗牌~");
        for (int i = 0; i < cardsList.Count; i++)
        {
            int r =UnityEngine.Random.Range(i,cardsList.Count);
            SkillCard temp = cardsList[r];
            cardsList[r] =cardsList[i];
            cardsList[i] = temp;
        }
    }
    //技能牌因手牌过多而无法加入手牌

    void ShowReward()
    {
        GameObject go =(GameObject)Instantiate(Resources.Load("Prefabs/UIBattleReward"));
        go.transform.SetParent(Main.instance.allScreenUI);
        go.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        go.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
        go.transform.localScale =Vector3.one;
        go.GetComponent<UIBattleReward>().Init(BattleScene.instance.steps,false);
        
    }
    void ShowChangeCard()
    {
        GameObject go =(GameObject)Instantiate(Resources.Load("Prefabs/UICardExchange"));
        go.transform.SetParent(Main.instance.allScreenUI); 
		go.transform.localScale =Vector3.one;
        go.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
		go.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
    }

    void OnShieldTips()
    {
        UIBuffDetail.CreateUIBuffDetail("每1点护甲可以减少1点伤害,但无法减少穿透伤害");
    }
    void OnColdTips()
    {
        UIBuffDetail.CreateUIBuffDetail("每1点寒冷可以减少1点造成的伤害,最多10点");
    }
    public void ReduceAllCardCost(int num)
    {
        for (int i = 0; i < allCards.Count; i++)
        {
            allCards[i].skill.ReduceMPCost(num);
            allCards[i].RefeashCardShow();

        }
    }
    public void NewBird_02()
    {
        StartCoroutine(IENewBird_02());
    }
    IEnumerator IENewBird_02()
    {
        UIBattle.Instance.CreateNewCardAndGiveToHand(102,0);
        UIBattle.Instance.CreateNewCardAndGiveToHand(102,1);
        UIBattle.Instance.CreateNewCardAndGiveToHand(2,2);
        yield return new WaitForSeconds(1f);
        Main.instance.ifNewBird++;
        NewBird.LoadNewBird(2);
        
    }
}
