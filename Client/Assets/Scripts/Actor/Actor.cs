using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
public enum ActorType
{
    玩家角色 =0,
    敌人 =1,
    NPC =2
}
public enum AnimState
{
    //角色当前的动作状态 
    idle =0,
    casting=1,
    dizzy=2,
    dead =3,
    dodge =4
}
public class Actor : MonoBehaviour
{
    // Start is called before the first frame update
    public string actorName;
    public int HpMax;//最大HP
	public int MpMax;
	public int HpCurrent;//当前HP
    public int armor;//护甲
    public float MpCurrent;
    public float Crit;
    public float dodge;
    public Transform spellPoint;
    public Transform castPoint;
    public Transform hitPoint;
    public Transform summonPoint;

	public List<int> UsingSkillsID;//角色携带的卡牌列表
    [HideInInspector]
    ///<summary>0=玩家角色；1=敌人；2=NPC</summary>
    public ActorType actorType;
    
    public List<int> abilities =new List<int>();
    [HideInInspector]
    public List<Skill> skills;//读取后的技能列表
    [HideInInspector]
    public List<Buff> buffs =new List<Buff>();
    [HideInInspector]
    public Animator animator;//角色身上的Animator
    HPBar castingbar;//角色的施法条
    HPBar hpBar;
    HPBar mpBar;
    public BattleText bt;

    public AnimState animState ;

    ///<summary>角色将要改变成的动作状态 0=idle,1=spell,2=casting,3=castEnd,4=dizzy,5=dead</summary>
    int NextState;
    // Timer timer;//角色泛用计时器
    Transform pool;
    // int TempSkillNumber =0;
    ///<summary>用于敌人和NPC的数值成长判断</summary>
    public int level =1;
    public Actor target;
    float autoReduceHpInterval=5;
    public const float autoReduceMpInterval=0.2f;
    // float currentAutoReduceHPTime;
    float currentAutoReduceMPTime;
    [HideInInspector]
    public float autoReduceHPAmount =0;
    [HideInInspector]
    public float autoReduceMPAmount =0;
    public float constArmorDecayTime =2;
    public float ArmorAutoDecayTime =0;
    bool ifArmorAutoDecay =true;

    public int basicAttack;
    public int basicDefence;
    public float initialMP=0;
    public int cardMpReduce =0;
    public float SummonedLifeTimePlus=0;

    [HideInInspector]
    public Character character;

    [HideInInspector]
    public MonsterTypeData monsterData;
    public int dealCardsNumber =3;//发牌数
    public int startBattleDealCardsNumber =4;//初次发牌数
    public int autoDealCardsMinValue =1;
    int state;
    int tempState =-1;
    int behaviour;
    ///<summary>手牌列表</summary>
    public List<SkillCard> handCards;

#region 已经无用的属性
//---------------已经无用--------------
    [HideInInspector]
    ///<summary>公共冷却时间</summary>
    public float commonCD;
//-----------------------------------   
#endregion 
    
    ///<summary>提升召唤物伤害事件，可传入伤害值</summary>
    public event Action<int> OnUpdateSummonedLifeTime;//

    ///<summary>命令召唤物立即进行一次攻击,可传入附加的伤害</summary>
    public event Action<int> OnOrderSummonedAttack;//

    
    //怪物AI相关
    Skill wanaSkill;


    void Start()
    {
        
    }
    #region  初始化角色
    public void InitActor()
    {
        pool = GameObject.Find("Pool").transform;
        animator =gameObject.GetComponent<Animator>();
        if(animator ==null)
        {
            animator =GetComponentInChildren<Animator>();
        }
        bt = gameObject.GetComponentInChildren<BattleText>();
        Transform btParent = transform.Find("TextPoint/BattleText");
        GameObject g = (GameObject)Instantiate(Resources.Load("Prefabs/battleText"));
        g.transform.SetParent(btParent);
        g.transform.localPosition =Vector3.zero;
        g.transform.localScale =Vector3.one;

        // timer =gameObject.GetComponent<Timer>();
        summonPoint =transform.Find("SummonPoint");
    }
    public void InitPlayerActor(Character character)
    {
        this.character =character;
        HpMax =character.HPMax;
        MpMax =character.MPMax;
        HpCurrent =HpMax;
        autoReduceMPAmount =character.reMP;
        Crit = character.crit;
        SetBasicAttack();
        // UsingSkillsID =data.skills;
        string[] str =character.data.skills.Split(',');
        UsingSkillsID =new List<int>();
        for (int i = 0; i < str.Length; i++)
        {
            UsingSkillsID.Add(int.Parse(str[i]));
        }
        if(character.data.relic>0)
        abilities.Add(character.data.relic);
    }
    public void InitEnemy(MonsterTypeData data)
    {
        monsterData =data;
        actorName =data.monsterName;
        HpMax =data.hp;
        HpCurrent =HpMax;
        SetBasicAttack();
        Crit =data.crit;
        UsingSkillsID =new List<int>();
        UsingSkillsID = data.m_attackSkills1.Union(data.m_attackSkills2).ToList(); 
        UsingSkillsID = UsingSkillsID.Union(data.m_attackSkills3).ToList(); 
        UsingSkillsID = UsingSkillsID.Union(data.m_defendSkills1).ToList(); 
        UsingSkillsID = UsingSkillsID.Union(data.m_defendSkills2).ToList(); 
        UsingSkillsID = UsingSkillsID.Union(data.m_defendSkills3).ToList(); 
        UsingSkillsID = UsingSkillsID.Union(data.m_buffSkills1).ToList(); 
        UsingSkillsID = UsingSkillsID.Union(data.m_buffSkills2).ToList(); 
        UsingSkillsID = UsingSkillsID.Union(data.m_buffSkills3).ToList(); 
        UsingSkillsID = UsingSkillsID.Union(data.m_nerfSkills1).ToList(); 
        UsingSkillsID = UsingSkillsID.Union(data.m_nerfSkills2).ToList(); 
        UsingSkillsID = UsingSkillsID.Union(data.m_nerfSkills3).ToList(); 
    
    }
    public void InitMagic()//初始化技能数值
    {
        
        if(actorType ==ActorType.NPC)
        {
            // return;
        }
        //1.读取角色技能列表
        skills = new List<Skill>();
        for(int i =0;i<UsingSkillsID.Count;i++)
        {
            Skill sk =SkillManager.CreateSkillForActor(UsingSkillsID[i],this);
            skills.Add(sk);
        }
    }
    #endregion

    // Update is called once per frame


    private void Update() 
    {
        if(UIBattle.Instance!=null&&!UIBattle.Instance.isBattleOver&& animState!= AnimState.dead)
        {
            
            // currentAutoReduceHPTime+=Time.deltaTime;
            currentAutoReduceMPTime+=Time.deltaTime;
            // if(currentAutoReduceHPTime>=autoReduceHpInterval)
            // {
            //     AutoReduceHP();
            // }
            if(currentAutoReduceMPTime>=autoReduceMpInterval)
            {
                AutoReduceMP();
            }
        }
        if(ifArmorAutoDecay&&armor>0)
        {
            ArmorAutoDecayTime+=Time.deltaTime;
            if(ArmorAutoDecayTime>=constArmorDecayTime)
            {
                armor =0;
                ArmorAutoDecayTime =0;
            }
        }
    }
    // void AutoReduceHP()
    // {
    //     AddHp(Mathf.FloorToInt(autoReduceHPAmount));
    //     currentAutoReduceHPTime =0;
    // }
    ///<summary>true=开启护甲衰减，false=关闭护甲衰减</summary>
    public void ChangeArmorAutoDecay(bool b)
    {
         ifArmorAutoDecay =b;
         ArmorAutoDecayTime =constArmorDecayTime;
    }
    public void RefeashArmorAutoDecayTime()
    {
        ArmorAutoDecayTime =0;
    }
    void AutoReduceMP()
    {
        AddMp(autoReduceMPAmount);
        currentAutoReduceMPTime =0;
    }
    public void AddMaxHP(int number)
    {
        HpMax+=number;
    }
    public void AddMaxMP(int number)
    {
        MpMax+=number;

    }
    public void AddBasicAttack(int number)
    {
        basicAttack+=number;
    }
    
    public void SetBasicAttack()
    {
        if(actorType ==ActorType.玩家角色)
        {
            basicAttack = character.attack;
            // basicDefence = character.attack;
        }
        else if(actorType ==ActorType.敌人)
        {
            basicAttack = monsterData.attack;

        }
    }
    public void AddArmor(int number)
    {
        armor +=number;
        if(armor<0)
        armor =0;
    }

    //在牌堆中增加一张牌(本局游戏永久)
    public void AddSkillCard(SkillData data)
    {
        UsingSkillsID.Add(data.id);
    }
    
    //复制数据用于战后回复
    //哪些数据在战后会恢复为原始？怎样标记
    //战斗开始前，先备份原始数据，当在战斗中发生永久属性变化时，同时操纵备份数据。
    //战斗结束后，将备份数据还原到当前数据
    
    //敌人AI
    #region
    public void RunAI()
    {
        //玩家角色不执行AI
        if(actorType ==ActorType.玩家角色)
        {
            return;
        }
        //判断当前是否会切换阶段
        state = EnemyState();
        //状态切换了，加buff或者减buff
        if(tempState!=state)
        {
            SetStateBuff();
            tempState = state;
        }
        //随机出一项当前要进行的行为
        behaviour =GetBehaviour(state);
        Debug.LogWarning("behaviour="+behaviour);
        float speed =3f;
        switch (state)
        {
            case 1:
            speed =monsterData.speed1;
            break;
            case 2:
            speed =monsterData.speed2;
            break;
            case 3:
            speed =monsterData.speed3;
            break;
        }
        
        if(behaviour<4)
        {
            castingbar.changeHPBar(speed);
            wanaSkill = GetSpecialSkill(state,behaviour);
            UIBattle.Instance.SetEnemyBarText(behaviour,wanaSkill.damage);
        }
        else//休息2秒然后再次判断
        {
            StartCoroutine(IEEnemySleep());
            UIBattle.Instance.SetEnemyBarText();
        }
    
    }
    void SetStateBuff()
    {
        switch(state)
        {
            case 1:
            if(monsterData.m_addBuffList1.Count ==0)
            {
                return;
            }
            foreach (var item in monsterData.m_addBuffList1)
            {
                BuffManager.instance.CreateBuffForActor(item,this);    
            }
            break;
            case 2:
            if(monsterData.m_removeBuffList2.Count >0)
            {
                foreach (var item in monsterData.m_removeBuffList2)
                {
                    BuffManager.EndBuffFromActor(BuffManager.FindBuff(item,this),this);
                }
            }
            if(monsterData.m_addBuffList2.Count >0)
            {
                foreach (var item in monsterData.m_addBuffList2)
                {
                    BuffManager.instance.CreateBuffForActor(item,this);
                }
            }
            break;
            case 3:
            if(monsterData.m_removeBuffList3.Count >0)
            {
                foreach (var item in monsterData.m_removeBuffList3)
                {
                    BuffManager.EndBuffFromActor(BuffManager.FindBuff(item,this),this);
                }
            }
            if(monsterData.m_addBuffList3.Count >0)
            {
                foreach (var item in monsterData.m_addBuffList3)
                {
                    BuffManager.instance.CreateBuffForActor(item,this);
                }
            }
            break;
        }
    }
    IEnumerator IEEnemySleep()
    {
        yield return new WaitForSeconds(2f);
        RunAI();
    }
    void OnBehaviourComplete(object sender, EventArgs e)
    {
        //决策条读完了
        // Debug.LogWarning("决策条读完了!");
        BarEventArgs eventArgs = e as BarEventArgs;
        if(eventArgs.IFComplete)
        //从当前行为中随机出一个要释放的技能
        WanaSpell(wanaSkill);
        else
        Debug.LogWarning("决策条被打断!");

    }
    int EnemyState()
    {
        int state =1;
        // Debug.LogWarning("判断阶段"+monsterData.switchCondition2);
        if(monsterData.switchCondition1==1)
        {
            if(HpCurrent>=Mathf.FloorToInt(HpMax/2))
            state  =1;
        }
        if(monsterData.switchCondition2==1)
        {
            if(HpCurrent<=Mathf.FloorToInt(HpMax/2))
            state  =2;
        }
        if(monsterData.switchCondition2==2)
        {
            if(HpCurrent<=Mathf.FloorToInt(HpMax*0.75f))
            state  =2;
        }
        if(monsterData.switchCondition2==3)
        {
            if(UIBattle.Instance.BattleTime>=10)
            state  =2;
        }
        if(monsterData.switchCondition2==4)
        {
            if(UIBattle.Instance.BattleTime>=20)
            state  =2;
        }
        if(monsterData.switchCondition3==1)
        {
            if(HpCurrent<=Mathf.FloorToInt(HpMax/2))
            state  =3;
        }
        if(monsterData.switchCondition3==2)
        {
            if(HpCurrent<=Mathf.FloorToInt(HpMax*0.25f))
            state  =3;
        }
        return state;
    }
    int GetBehaviour(int state)
    {
        switch(state)
        {
            case 1: return  1+GetRandomFromList(monsterData.m_aitype1);
            case 2: return  1+GetRandomFromList(monsterData.m_aitype2);
            case 3: return  1+GetRandomFromList(monsterData.m_aitype3); 
        }
        return 1;
    }
    Skill GetSpecialSkill(int state,int behaviour)
    {
        int skillId =0;
        switch(state)
        {
            case 1:
                switch(behaviour)
                {
                    case 1:
                        skillId = monsterData.m_attackSkills1[GetRandomFromIntList(monsterData.m_weightAttackSkills1)];
                    break;
                    case 2:
                        skillId = monsterData.m_defendSkills1[GetRandomFromIntList(monsterData.m_weightDefendSkills1)];
                    break;
                    case 3:
                        skillId = monsterData.m_buffSkills1[GetRandomFromIntList(monsterData.m_weightBuffSkills1)];    
                    break;
                    case 4:
                        skillId = monsterData.m_nerfSkills1[GetRandomFromIntList(monsterData.m_weightNerfSkills1)];
                    break;
                }
            break;

            case 2:
                switch(behaviour)
                {
                    case 1:
                        skillId = monsterData.m_attackSkills2[GetRandomFromIntList(monsterData.m_weightAttackSkills2)];
                    break;
                    case 2:
                        skillId = monsterData.m_defendSkills2[GetRandomFromIntList(monsterData.m_weightDefendSkills2)];
                    break;
                    case 3:
                        skillId = monsterData.m_buffSkills2[GetRandomFromIntList(monsterData.m_weightBuffSkills2)];    
                    break;
                    case 4:
                        skillId = monsterData.m_nerfSkills2[GetRandomFromIntList(monsterData.m_weightNerfSkills2)];
                    break;
                }

            break;

            case 3: 
                switch(behaviour)
                {
                    case 1:
                        skillId = monsterData.m_attackSkills3[GetRandomFromIntList(monsterData.m_weightAttackSkills3)];
                    break;
                    case 2:
                        skillId = monsterData.m_defendSkills3[GetRandomFromIntList(monsterData.m_weightDefendSkills3)];
                    break;
                    case 3:
                        skillId = monsterData.m_buffSkills3[GetRandomFromIntList(monsterData.m_weightBuffSkills3)];    
                    break;
                    case 4:
                        skillId = monsterData.m_nerfSkills3[GetRandomFromIntList(monsterData.m_weightNerfSkills3)];
                    break;
                } 

            break;

        }
        foreach (var item in skills)
        {
            if(item.id == skillId)
            {
                return item;
            }
        }
        return skills[0];

    }
    ///<summary>从一个List<int>中随机获取一个位置数</summray>
    int GetRandomFromIntList(List<int> listW)
    {
        if(listW.Count <2)
        {
            return 0;
        }

        int max =0;
        List<int> weights =new List<int>();
        //5,10,0,5
        //5,15,15,20
        //r=18
        // 15<=r<=15 15<=r<=20
        foreach (var item in listW)
        {
            weights.Add(max+item);
            max+=item;
        }
        // Debug.LogWarning("MAX="+max);
        int r = UnityEngine.Random.Range(0,max+1);
        // Debug.LogWarning("随机到"+r);
        if(r<weights[0])
        {
            return 0;
        }
        for(int i =1;i<weights.Count;i++)
        {
            if (r>=weights[i-1]&&r<=weights[i])
            {
                // Debug.LogWarning("顺序是"+i);
                return i;
            }
        }
        Debug.LogWarning("出现奇怪的问题");
        return 0;
    }
    int GetRandomFromList(List<int> listW)
    {
        List<int> listww =new List<int>();
        for (int i = 0; i < listW.Count; i++)
        {
            if(listW[i] != 0)
            {
                listww.Add(i);
            }
        }
        int max =0;
        List<int> weights =new List<int>();
        foreach (var item in listww)
        {
            weights.Add(max+item);
            max+=item;
        }
        int r = UnityEngine.Random.Range(0,max+1);
        Debug.LogWarning("behaviour:r="+r+",weights[0]="+weights[0]);
        // if(weights[0]==0&&r==0)
        // {
        //     return listww[0];
        // }
        if(r<=weights[0])
        {
            return listww[0];
        }
        for(int i =1;i<weights.Count;i++)
        {
            if (r>weights[i-1]&&r<=weights[i])
            {
                Debug.LogWarning("顺序是"+i+"结果是："+listww[i]);
                return listww[i];
            }
        }
        return 0;
    }
    public void AutoAttack()
    {
        // WanaSpell(skills[TempSkillNumber]);
        // if(TempSkillNumber ==UsingSkillsID.Length-1)
        // {
        //     TempSkillNumber = 0;
        //     return;
        // }
        // TempSkillNumber++;
        if(skills[0]!=null)
        WanaSpell(skills[0]);
    }
    #endregion


    public Skill GetSkills(int id)
    {
        return skills[id+1];
    }
    public void GetActorSpellBar()
    {
        //获取角色施法条
        if(actorType==0)
        {   
            // if(Player.instance.playerNow!=""&&Player.instance.playerNow.Substring(0,1) =="A")
            if(UIBattle.Instance!=null)
            {
                hpBar = GameObject.Find("HP_Player").GetComponent<HPBar>();
                mpBar = GameObject.Find("MP_Player").GetComponent<HPBar>();
                mpBar.initHpBar((int)MpCurrent,MpMax);
                hpBar.BindHPBar(this);    
            }
            // castingbar= GameObject.Find("CastingBar_Player").GetComponent<HPBar>();
        }
        else
        {
            castingbar =GameObject.Find("CastingBar_Enemy").GetComponent<HPBar>();
            hpBar = GameObject.Find("HP_Enemy").GetComponent<HPBar>();
            hpBar.BindHPBar(this);  
            // mpBar = GameObject.Find("MP_Enemy").GetComponent<HPBar>();
            // mpBar.initHpBar((int)MpCurrent,MpMax);
            // Debug.Log("HPBAR");
            castingbar.BindHPBar(this);
            castingbar.onBarEvent+= OnBehaviourComplete;
        }        
    }
    // void GetTarget()
    // {
    //     if(target ==null)
    //     {
    //         if(actorType ==ActorType.敌人)
    //         {
    //             target =Battle.Instance.playerActor;
    //         }
    //         else
    //         {
    //             target =Battle.Instance.enemy;
    //         }
    //     }
    // }
    public bool WanaSpell(Skill skill)//想要施法一个法术，判断是否可以施放该法术
    {
        // Debug.LogWarning("wanaSpell");
        // Debug.LogWarning("animState="+(int)animState);

        //角色自身状态不为dizzy，dead
        if(animState ==AnimState.dead)
        {
            
            return false;
        }
        if(animState ==AnimState.dizzy)
        {
            
            return false;
        }
        #region 宝珠相关,已无用
        //如果需要消耗宝珠，先检查宝珠的数量是否足够
        // if(skill.needBallAmount>0)
        // {
        //     if(skill.dontNeedColor)
        //     {
        //         if(!BallManager.instance.CheckBallNumber(skill.needBallAmount))
        //         {
        //             return false;
        //         }
        //     }
        //     else
        //     {
        //         if(!BallManager.instance.CheckBallNumber(skill.needBallColor,skill.needBallAmount))
        //         {
        //             return false;
        //         }
        //     }
        // }
        #endregion
        if(skill.realManaCost>MpCurrent)//法力值不足
        {
            // Debug.LogFormat("魔法值不足,需要{0},当前为{1}",skill.realManaCost,MpCurrent);
            return false;
        }
        if(actorType ==ActorType.玩家角色)
        StopCasting();
        //如果目标已经死了
        if(target!=null&&target.animState ==AnimState.dead)
        {
            return false;
        }
        
        // StartCoroutine(WaitForBeginSpell(skill));
        animator.Play("attack");
        BeginSpell(skill);
        RunAI();
        // BeginSpell(skill);
        // RunAI();
        return true;
        
    }
    IEnumerator WaitForBeginSpell(Skill skill)
    {
        yield return new WaitForEndOfFrame();
        // Debug.LogWarning("WaitForBeginSpell");
        // Debug.LogWarning("animState="+(int)animState);
        if(animState!=AnimState.dead&&animState!=AnimState.dizzy)
        {
            animator.Play("attack");
            BeginSpell(skill);
            RunAI();
        }
        
    }
    public void BeginSpell(Skill skill)//开始施放X技能,需要传入一个技能
    {
        // if(Main.instance.UIState>0&&!DateManager.instance.timer.enabled)
        // {
        //     StopCasting();
        //     return;
        // }
        //1.切换Animator到1，开始循环播放施法动作spell
        // ChangeAnimatorInteger(1);
        
        //消耗MP，数值为技能所需
        AddMp(-skill.realManaCost);
        #region 宝珠相关，以及其他废弃部分
        // int costAmount =0;
        // if(skill.needBallAmount>0)
        // {
        //     if(skill.dontNeedColor)
        //     {
                
        //     }
        //     else
        //     {
        //         costAmount =BallManager.instance.CostBalls(skill.needBallColor);
        //     }
        // }
        // BallManager.instance.CreateNewBall(skill.amount,(BallColor)skill.color);
        // costAmount+=BallManager.instance.CostBalls();
        // Debug.Log("costAmout:"+costAmount);
        
        //检查消耗宝珠能力

        // if(Main.instance.UIState <=0)
        // {
        //     //调整施法时间
        //     // skill.ModiferCastSpeed();
        //     //2.消耗MP，数值为技能所需
            
            
        //     //3.施法条开始自行运动
        //     // castingbar.changeHPBar(skill.spelllTime+0f);
        // }
        // else
        // {
        //     timer.start(skill.spelllTime,OnPracticeTimerComplete);
        // }
        //4.设置角色状态为casting
        // animState=AnimState.casting;
        #endregion
        //5.创建施法特效
        // CreateSpellEffect(skill);
        CreateCastEffect(skill);
        //执行技能释放完毕事件
        OnSkillSpellFinish(skill);
        //6.绑定技能和施法条
        // if(castingbar)
        // castingbar.BindHPBar(skill);

    }
    
    bool CheckAbility(Skill skill,int costAmount)
    {
        if(costAmount>=3)
        {
            // if(abilities.Contains(101))
            {
                return true;
            }
        }
        return false;

    }
    void CreateSpellEffect(Skill skill)
    {
        if(skill.spellEffect=="")
        {
            return;
        }
        Transform e = EffectManager.TryGetFromPool(skill.spellEffect);
        if(e!=null)
        {
            e.SetParent(spellPoint);
            e.localPosition =Vector3.zero;
            e.localScale =Vector3.one;
            // e.gameObject.SetActive(true);
            return;
        }
        
        // GameObject go =(GameObject)Instantiate(Resources.Load("Prefabs/Effects/"+skill.spellEffect));
        // go.transform.SetParent(spellPoint);
        // go.transform.localPosition =Vector3.zero;
        // go.transform.localScale =Vector3.one;
    }
    public void StopCasting()//打断施法
    {
        if((int)animState<2)
        {
            //切换动作到idle
            //设置角色状态为idle
            // animator.Play("idle");
            animState =AnimState.idle;
            ChangeAnimatorInteger(0);
        }
        // if(target!=null&&target.animState ==AnimState.dead)
        // {
        //     animator.Play("idle");
        //     // animator.SetInteger("anim",0);
        // }
    //    if(this.isActiveAndEnabled)
    //    animator.Play("idle");
       //施法条停止运动
       if(castingbar)
       {
            castingbar.stopChanging(true);
            UIBattle.Instance.SetEnemyBarText(0,0);
       }
        
       
    //    if((int)animState<2)
    //    {
           
    //    }
        //移除施法特效
       EffectManager.TryThrowInPool(spellPoint);
       EffectManager.ClearChannelEffect(castPoint);
       
    }
    #region 动画控制部分
    void AnimationState(int i)//从关键帧获取当前播放到哪个动画片段,并根据逻辑切换到下一个动画
    {
        if(animState ==AnimState.dead)
        {
            animator.SetInteger("anim",5);
            return;
        }
        if(animState ==AnimState.dizzy)
        {
            animator.SetInteger("anim",4);
            return;
        }
        // Debug.LogFormat("播放完成动作{0}",i);
        switch(i)
        {
            case 0:
            //idle动作播放完毕
            break;
            case 1:
            //spell动作播放完毕,接下来循环播放casting
            NextState=2;
            break;
            case 3:
            //castEnd动作播放完毕
            

            NextState =0;
            if((int)animState<2)
            {
                animState =AnimState.idle; 
            }
            
            // StartCoroutine(NextTurn());

            break;
        }
        if(i!=NextState)
        {
            ChangeAnimatorInteger(NextState);
        }
    }
    public void ChangeAnimatorInteger(int i)
    {
        if(animator==null)
        {
            return;
        }
        if(animState == AnimState.dead)
        {
            animator.SetInteger("anim",5);
            // animator.Play("dead",0,0f);
        }
        if(animState == AnimState.dizzy)
        {
            animator.SetInteger("anim",4);
        }
        animator.SetInteger("anim",i);
        // if(this != Player.instance.playerActor)
        // Debug.LogWarningFormat("当前animator为：{0}",animator.GetInteger("anim"));
    }
      //执行下一轮动作逻辑
    // IEnumerator NextTurn()
    // {
    //     yield return new WaitForFixedUpdate();

    //    RunAI();
    // }
    #endregion
  
    
    public void OnTimerComplete(Skill skill)
    {
        // timer.stop();
        //播放cast动作
        animState =AnimState.casting;
        ChangeAnimatorInteger(3);
        //移除施法特效
        EffectManager.TryThrowInPool(spellPoint);
        //添加投射物特效
        CreateCastEffect(skill);
        //如果不在练习状态
        // if(Player.instance.playerNow!=""&&Player.instance.playerNow.Substring(0,1) =="A")
        // {
        // }
        
        // skill.ComputeDamage();
        //执行技能释放完毕事件
        
        OnSkillSpellFinish(skill);
    }
    
    void CreateCastEffect(Skill skill)
    {
        if(skill.castEffect=="")
        {
            return;
        }
        
        
        Transform f = EffectManager.TryGetFromPool(skill.castEffect);
        if(f!=null)
        {
            f.SetParent(castPoint);
            f.gameObject.SetActive(true);
            f.localPosition =Vector3.zero;
            // f.localScale =Vector3.one;
            if(target!=null)
            {
                
                EffectManager.CastEffect(f,target.hitPoint,skill.damageDelay,skill.hitEffect);
                
            }
            else
            {
                
                // else
                // {
                //     // EffectManager.CastEffect(f,UIPractice.instance.targetPoint,skill.damageDelay,skill.hitEffect);
                // }
            }
            // if(skill.caster.actorType==0)
            // {
            //     f.localScale =Vector3.one;
            // }
            // else
            // {
            //     f.localScale =new Vector3(-1,1,1);
            // }
            // 
            return;
        }
        // GameObject go =(GameObject)Instantiate(Resources.Load("Prefabs/Effects/"+skill.castEffect));
        // go.transform.SetParent(castPoint);
        // go.transform.localPosition =Vector3.zero;
        // if(skill.caster.actorType==0)
        //     {
        //         go.transform.localScale =Vector3.one;
        //     }
        //     else
        //     {
        //         go.transform.localScale =new Vector3(-1,1,1);
        //     }
        
        // go.SetActive(true);
    }
    public int AddHp(int num)
    {
        // if(this.actorType == ActorType.玩家角色)
        // Debug.LogWarningFormat("{0}的生命值变化{1},最终为{2}",this.name,num,HpCurrent);
        int changeNum =0;
        if(HpCurrent+num>=HpMax)
        {
            changeNum = HpMax-HpCurrent;
            HpCurrent =HpMax;
        }
        else if(HpCurrent+num <= 0)
        {
            changeNum = -HpCurrent;
            HpCurrent = 0;
        }
        else
        {
            changeNum = num;
            HpCurrent =HpCurrent+num;
        }
        if(hpBar!=null)
        {
            hpBar.changeHPBar(HpCurrent,true);
        }
        // if(this.actorType == ActorType.敌人)
        if(num>0)
        Debug.LogWarningFormat("{0}的生命值变化{1},最终为{2}",this.name,num,HpCurrent);

        return changeNum;
    }

    #region --------------------------技能命中，暴击，受到伤害等等角色反应,BUFF相关，复活--------------------
    public void OnSkillHasCrit(Skill skill, int damage)//通知角色技能暴击，暴击伤害为damage
    {
        //执行当技能产生暴击时就xxx这类效果
        //暴击检查点燃Buff 和 燃爆 等级,满足条件造成燃爆伤害
        // SkillManager.CheckSkillToTriggerNewSkill(skill,1112,null,new List<int>(){1},null,new List<int>(){4},false);
        //添加点燃buff
        // if(skills.
        SkillManager.CheckSkillOnHitToAddBuff(skill,1110,4,new List<int>(){1110},null,null);
    }
    public void OnSkillHasHit(bool ifHit ,Skill skill)
    {
        if(ifHit)
        {
            if(skill.id==29)
            {
                Debug.LogWarning("该打晕了");
            }
            //执行当技能命中目标时就xxx这类效果
            if(skill.buffID>0&&!skill.targetSelf)
            {
                BuffManager.instance.CreateBuffForActor(skill.buffID,skill.target);
            }
            // Check1110(skill);
            //火系技能添加点燃buff
            // SkillManager.CheckSkillOnHitToAddBuff(skill,1110,4,new List<int>(){1110},new List<int>(){1},null);
            //冻结
            SkillManager.CheckSkillOnHitToAddBuff(skill,1011,6,new List<int>(){1112},new List<int>(){0},new List<int>(){7});
            //引火烧身
            SkillManager.instance.Check1117(skill);
            
            // Check1011(skill);
            Main.instance.ShakeCamera();
        }
        else
        {
            //执行当技能未命中目标时就xxx这类效果
        }
    }
    
    public void OnSkillHasResistance()
    {
        //执行当技能被抵抗时就xxx这类效果
    }
    public int TakeDamage(int num,bool crit,int genre,bool ifRebound,bool ifSeep,Skill TakenSkill)//传入技能伤害,是否暴击,是否为反弹伤害 【增加是否为穿透伤害】【返回最终造成的伤害数值】
    {
        //如果角色已经死亡，则不会再承受伤害
        if(animState ==AnimState.dead)
        {
            target.StopCasting();
            return 0;
        }
        //基础防御力减免
        if(num-basicDefence>0)
        num-=basicDefence;
        else
        num = 0;

        //反弹伤害不能被反弹
        //治疗不能被反弹
        if(!ifRebound&&num>0)
        {
            // Debug.Log("可以反弹伤害");
            //如果拥有数值反弹或百分比反弹伤害的buff
            for (int i = 0; i < buffs.Count; i++)
            {
                if(buffs[i].buffData._type == BuffType.数值反弹受到的伤害 && buffs[i].buffData._genreList.Contains(genre))
                {
                    // Debug.LogFormat("数值反弹buff名为：{0},效果类型为：{1},影响派系为：{2}",buffs[i].buffData.name,buffs[i].buffData._type,buffs[i].buffData.genreList);
                    //显示特效
                    Transform e = EffectManager.TryGetFromPool(buffs[i].buffData.triggerEffect);
                    if(e!=null)
                    {
                        e.SetParent(target.hitPoint);
                        e.localPosition =Vector3.zero;
                        e.localScale =Vector3.one;
                    }
                    //伤害目标:输出一次伤害，反弹伤害属性与原技能属性相同，不会未命中，不会暴击，不受任何加成【作废】
                    // Battle.Instance.ReceiveSkillDamage(Mathf.CeilToInt(buffs[i].currentValue),target,genre);
                    //伤害目标：使用关联技能来造成伤害
                    Skill skill = SkillManager.TryGetFromPool(buffs[i].buffData.abilityID,this);
                    if(skill.targetSelf)
                    {
                        skill.target =target;
                    }
                    Battle.Instance.ReceiveSkillDamage(skill,skill.damage,true,false);
                    skill.target =this;    
                }
            }
            for (int i = 0; i < buffs.Count; i++)
            {
                if(buffs[i].buffData._type == BuffType.百分比反弹受到的伤害 && buffs[i].buffData._genreList.Contains(genre))
                {
                    //显示特效
                    Transform e = EffectManager.TryGetFromPool(buffs[i].buffData.triggerEffect);
                    if(e!=null)
                    {
                        e.SetParent(target.hitPoint);
                        e.localPosition =Vector3.zero;
                        e.localScale =Vector3.one;
                    }
                    //伤害目标:输出一次伤害，反弹伤害属性与原技能属性相同，不会未命中，不会暴击，不受任何加成
                    Skill skill = SkillManager.TryGetFromPool(buffs[i].buffData.abilityID,this);
                    if(skill!=null)
                    {
                        if(skill.targetSelf)
                        {
                            skill.target =target;
                            Battle.Instance.ReceiveSkillDamage(skill,Mathf.CeilToInt(buffs[i].currentValue),true,false);
                            skill.target =this;
                        }
                    }
                    else
                    {
                        Battle.Instance.ReceiveSkillDamage(Mathf.CeilToInt(buffs[i].currentValue),target,genre,false);
                    }
                    
                    // Debug.LogFormat("百分比反弹buff名为：{0},效果类型为：{1},影响派系为：{2}",buffs[i].buffData.name,buffs[i].buffData._type,buffs[i].buffData.genreList);
                }
            }
        }
        #region 吸收伤害相关
        ///<summary>是否吸收伤害</summary>
        bool ifAbsorb =false;
        int tempNum =0;
        
        //如果不是穿透伤害
        if(num>=0&&!ifSeep)
        {
        //吸收伤害在受到伤害之前执行
        #region 旧版吸收伤害
            // for (int i = 0; i < buffs.Count; i++)
            // {
            //     if(buffs[i].buffData._type == BuffType.数值吸收伤害 && buffs[i].buffData._genreList.Contains(genre))
            //     {
            //         //显示特效
            //         Transform e = EffectManager.TryGetFromPool(buffs[i].buffData.triggerEffect);
            //         if(e!=null)
            //             {
            //                 e.SetParent(target.hitPoint);
            //                 e.localPosition =Vector3.zero;
            //                 e.localScale =Vector3.one;
            //             }
            //         //伤害减少
            //         //护盾被打爆
            //         if(num>=buffs[i].currentValue)
            //         {
            //             num-=Mathf.CeilToInt(buffs[i].currentValue);
            //             tempNum =Mathf.CeilToInt(buffs[i].currentValue);
            //             Debug.LogFormat("由于{2}的buff效果，吸收了{0}点伤害,护盾剩余值为{1}",buffs[i].currentValue,0,buffs[i].buffData.name);
            //             // item.currentValue =0;
            //             buffs[i].buffIcon.OnEffectEnd();
            //             //执行护盾被打爆事件

            //         }
            //         //护盾没被打爆
            //         else
            //         {
            //             buffs[i].currentValue-=num;
            //             tempNum =num;
            //             Debug.LogFormat("由于{2}的buff效果，吸收了{0}点伤害,,护盾剩余值为{1}",num,buffs[i].currentValue,buffs[i].buffData.name);
            //             num =0;
            //         }
            //         ifAbsorb =true;
            //     }
            // }
            
            // for (int i = 0; i < buffs.Count; i++)
            
            // {
            //     if(buffs[i].buffData._type == BuffType.百分比吸收伤害 && buffs[i].buffData._genreList.Contains(genre))
            //     {
            //         //显示特效
            //         Transform e = EffectManager.TryGetFromPool(buffs[i].buffData.triggerEffect);
            //         if(e!=null)
            //             {
            //                 e.SetParent(target.hitPoint);
            //                 e.localPosition =Vector3.zero;
            //                 e.localScale =Vector3.one;
            //             }
            //         //伤害减少
                    
            //         num = Mathf.CeilToInt( num*(1-buffs[i].currentValue));
                    
            //     }
            // }
            #endregion
            
            //法力护盾：减少能量值来吸收伤害，当能量值空时，不再吸收伤害,法力护盾吸收的优先级低于普通护甲


            //改变角色当前护甲数值
            //如何计算当前角色拥有的所有护甲值？
            //1.增加护甲的buff将直接增加护甲值
            //2.buff时间到或层数改变时，将直接修改护甲值
        if(armor>0)
        ifAbsorb =true;
            // tempNum =Math.Abs(num-armor);
            if(armor>=num)
            {
                tempNum =num;
                AddArmor(-num);
                num = 0;
            }
            else
            {
                tempNum =armor;
                num-=armor;
                armor =0;
            }
            if(armor == 0)//可执行破盾时就xxx这类效果
            {
                //当护甲归零时，移除护甲BUFF-----------------获得护甲类的BUFF不再显示icon，所以不必手动移除
                // for (int i = buffs.Count-1; i >=0 ; i--)
                // {
                //     if(buffs[i].buffData._type == BuffType.吸收一定数量的伤害)
                //     {
                //         BuffManager.RemoveBuffFromActor(buffs[i],this);
                //     }
                // }
                BuffManager.Check_SpecialTypeBuff_ToSetBuff(this,BuffType.失去所有护甲后增减buff);
                
            }
        }
        #endregion
        //执行{受到伤害时就xxx这类效果}
        
        //执行 {受到暴击时就xxx}
        if(crit)
        {

        }

        //2.扣除生命值，判断生命值是否归零
        //先改变生命再判断
        int tempHP = HpCurrent;
        AddHp(-num);
        bool ifDie =false;
        //3.伤害文字
        
        if(ifAbsorb)
        {
            if(num<=0)
            bt.SetText("吸收"+tempNum);
            else
            bt.SetText(-num+"("+tempNum+"吸收)");
            //格挡成功事件
            BuffManager.Check_SpecialTypeBuff_ToTriggerSkill(this,BuffType.成功格挡后触发技能);
        }
        else
        {
            bt.SetText(num,crit);
        }
        
        Debug.LogFormat("角色：{0}的当前生命值：{1}",name,HpCurrent);
        if(!ifAbsorb&&num<=0)
        {
            bt.SetText("0");
            return 0;
        }
        
        //如果最终判断掉血了
        if(num>0)
        {
            //执行当角色生命值低于50%，就xxx这类效果
            if(tempHP>0.5*HpMax&&HpCurrent<=0.5*HpMax)
            {
                // if(actorType ==ActorType.敌人)
                // {
                //     if(monsterData.switchCondition2==1)
                //     {

                //     }
                // }
            }
            //执行当角色生命值低于25%，就xxx这类效果
            if(tempHP>0.25*HpMax&&HpCurrent<=0.25*HpMax)
            {
                BuffManager.Check_SpecialTypeBuff_ToTriggerSkill(this,BuffType.生命值小于百分之25时触发);
            }
            
        }
        
        
        //如果要死了
        if(HpCurrent==0)
        {
        //执行当角色生命值为0时，就xxx这类效果
        bool ifRelive =false;
        for (int i = 0; i < buffs.Count; i++)
        {
            Debug.LogWarningFormat("---开始查找复活buff---{0}",buffs[i].buffData.id);

            if(buffs[i].buffData._type == BuffType.死亡后复活并恢复百分比生命)
            {
                //显示特效
                Transform e = EffectManager.TryGetFromPool(buffs[i].buffData.triggerEffect);
                if(e!=null)
                {
                    e.SetParent(hitPoint);
                    e.localPosition =Vector3.zero;
                    e.localScale =Vector3.one;
                }
                Debug.LogWarningFormat("---找到复活buff---");
                ifRelive = true;
                StartCoroutine(WaitForRelive (Mathf.CeilToInt(HpMax*buffs[i].currentValue)));
                if(buffs[i].buffIcon!=null)
                {
                    buffs[i].buffIcon.OnEffectEnd();
                }
                else
                {
                    buffs[i].OnBuffEnd();
                }
                target.StopCasting();
                animState = AnimState.dead;
                UIBattle.Instance.playerActor.StopCasting();
                UIBattle.Instance.Enemy.StopCasting();
                ChangeAnimatorInteger(5);
                break;//一次只消耗一种复活
            }
        }
        //如果没复活能力
        if(!ifRelive)
        {
            ifDie =true;
            // HpCurrent =0;
            // hpBar.changeHPBar(HpCurrent,true);
            if(TakenSkill.skillName =="圣剑攻击")
            {
                Skill _skill = TakenSkill.casterSummon.skillCard.skill;
                //移除该技能对应的技能卡，然后创造一张该技能卡的升级卡给玩家
                UIBattle.Instance.gameObject.GetComponent<ActorBackUp>().UsingSkillsID.Remove(_skill.id);
                Debug.Log("移除的技能："+_skill.id);
                UIBattle.Instance.gameObject.GetComponent<ActorBackUp>().UsingSkillsID.Add(_skill.updateID);
                Debug.Log("添加的技能："+_skill.updateID);
            }
            if(TakenSkill.id ==109)//斩杀，恢复10点生命值
            {
                TakenSkill.caster.AddHp(Configs.instance.executeRestoreHP);
            }
            if(Player.instance.playerActor.abilities.Contains(11))//勇气手环
            {
                Skill tempSkill =SkillManager.TryGetFromPool(40,Player.instance.playerActor);
                Player.instance.playerActor.OnSkillSpellFinish(tempSkill);
            }
            
            Die();
        }    
        }
        //受伤回血效果
        //没死才能受伤回血
        if(num>0&&!ifDie)
        {
           //如果拥有数值受伤回血的buff
            BuffManager.CheckBuffOnReceiveSkillDamageToTriggerSkillDamage(this,genre,BuffType.数值受伤回复,false);
        }
        return num;
        
    }
    public int TakeHeal(int num)
    {
       if(animState ==AnimState.dead)
        {
            target.StopCasting();
            return 0;
        }
        //如果身上有减少治疗的buff，那么治疗降低
        // if(buffs[i].buffData.id)
        int realNum = -AddHp(num);
        OnTakeHeal(num);
        bt.SetText(realNum,false);
        return num;

    }
    public void OnTakeHeal(int num)//受到治疗的效果
    {

    }
    public void OnSkillSpellFinish(Skill skill)
    {
        //技能释放完毕事件
        //有伤害的技能输出伤害
        if(skill.damage!=0)
        skill.ComputeDamage();
        //有治疗的技能产生治疗
        //........
        if(skill.heal!=0)
        skill.ComputeHeal();
        //有buff的技能加buff
        if(skill.buffID>0&&skill.targetSelf)
        {
            BuffManager.instance.CreateBuffForActor(skill.buffID,skill.target);
            Debug.LogWarning("添加的buffid="+skill.buffID);
        }
        //抽卡的技能抽卡
        if(skill.usedChooseCard>0)
        UIBattle.Instance.SelectSomeCards(skill.usedChooseCard);
        //魔力回复
        skill.caster.AddMp(skill.manaProduce);
        //召唤类技能进行召唤
        if(skill.summonNum>0)
        {
            SummonManager.instance.CreateSummon(skill);
        }
        //如果有圣剑buff，那么命令圣剑攻击
        for (int i = 0; i < buffs.Count; i++)
        {
            if(buffs[i].buffData.id == 1001)
            {
                if(OnOrderSummonedAttack!=null)
                OnOrderSummonedAttack.Invoke(0);
            }
        }
        if(abilities.Contains(1))
        {
            if(skill.color ==2)
            {
                autoReduceMPAmount+=0.2f;
            }
        }
        if(abilities.Contains(4))
        {
            AddMp(1);
        }
         //每次使用改变自身伤害的技能
        if(skill.skillData.EUSDamage!=0)
        {
            skill.skillCard.IncreaseDamage(skill.skillData.EUSDamage);
        }
        //每次使用改变自身消耗的技能
        if(skill.skillData.EUSMP!= 0)
        {
            skill.skillCard.ReduceMPCost(skill.skillData.EUSMP);
        }
        //每次使用改变自身治疗量的技能
        if(skill.skillData.EUSHeal!= 0)
        {
            skill.skillCard.IncreaseHeal(skill.skillData.EUSHeal);
        }
           
    }
    public void AddMp(float num)
    {
        if(MpCurrent+num>=MpMax)
        {
            MpCurrent =MpMax;

        }
        else if(MpCurrent+num<=0)
        {
            MpCurrent =0;
        }
        else
        {
            MpCurrent+=num;
        }
        if(mpBar!=null)
        {
            mpBar.changeHPBar((int)MpCurrent,true);
        }
        if(this.actorType ==ActorType.玩家角色)
        {
            UIBattle.Instance.CheckButtonMP();
        }
    }
    public void OnHitMiss()
    {
        //显示文字 未命中
        bt.SetText("闪避");
        //执行当角色闪避攻击时就xxx这类效果
    }
    public void OnHitResistance()
    {
        //显示文字 抵抗
        bt.SetText("抵抗");
        //执行当角色抵抗技能时就xxx这类效果
    }
    void Die()
    {
        //1.执行当角色死亡时就xxx这类效果（深渊随机buff：当角色死亡时会发生一次爆炸，造成XXX点伤害，如果杀死敌人，则视为战斗胜利）
        //2.告诉UIBattle,播放死亡相关的UI
        // StopCasting();
        Debug.LogWarningFormat("死了！！！！");
        animState = AnimState.dead;
        // animator.Play("dead");
        UIBattle.Instance.playerActor.StopCasting();
        UIBattle.Instance.Enemy.StopCasting();
        if(actorType == ActorType.敌人)
        {
            castingbar.gameObject.SetActive(false);
           
        }
        // animator.SetInteger("anim",5);
        ChangeAnimatorInteger(5);
        // animator.Play("dead");
        EffectManager.TryThrowInPool(spellPoint);
        UIBattle.Instance.BattleEnd(this);
           
    }
    public void ReLiveActor()
    {
        //复活
        animState =AnimState.idle;
        // animator.SetInteger("anim",0);
        ChangeAnimatorInteger(1);
        HpCurrent =HpMax;
        MpCurrent = 0;
        if(hpBar)
        hpBar.initHpBar(HpCurrent,HpMax);
        if(mpBar)
        mpBar.initHpBar((int)MpCurrent,MpMax);
    }
    IEnumerator WaitForRelive(int hp)
    {
        yield return new WaitForSeconds(2.5f);
        ReLiveActor(hp);
        target.RunAI();
        RunAI();
    }
    public void ReLiveActor(int hp)
    {
        //复活
        animState =AnimState.idle;
        // animator.SetInteger("anim",0);
        animator.Play("idle");
        HpCurrent =hp;
        MpCurrent = 0;
        if(hpBar)
        hpBar.initHpBar(HpCurrent,HpMax);
        if(mpBar)
        mpBar.initHpBar((int)MpCurrent,MpMax);
    }
    void OnPracticeTimerComplete(Timer timer)
    {
        OnTimerComplete(skills[0]);
    }
    public void AddBuff(Buff buff)
    {
        // if(!buffs.Contains(buff))
        //Buff叠加机制：相同id的buff，如果已达最大层数，只会刷新持续时间
        
        int buffNum =0;
        bool ifMax=false;
        // Buff tempBUff =null;
        List<Buff> tempList =new List<Buff>();
        foreach (var item in buffs)
        {
            //相同id的buff
            if(item.buffData.id == buff.buffData.id)
            {
                buffNum++;
                // tempBUff =item;
                tempList.Add(item);
                
            }
        }
        
        //maxNum =0 代表可以叠无限层
        if(buff.buffData.maxNum==0)
        {
            buffs.Add(buff);
            tempList.Add(buff);
            buffNum++;
        }
        //未叠加到最大层
        else if(buff.buffData.maxNum>0&&buffNum<buff.buffData.maxNum)
        {
            buffs.Add(buff);
            tempList.Add(buff);
            buffNum++;
        }
        //已叠加到最大层
        else if(buff.buffData.maxNum>0&&buffNum==buff.buffData.maxNum)
        {
            //去掉一层，再重新添加一层
            
            BuffManager.RemoveBuffFromActor(tempList[0],this);
            tempList.Remove(tempList[0]);
            buffs.Add(buff);
            tempList.Add(buff);
            ifMax =true;
        }
        else//不能叠层，当有新的相同IDbuff时，会额外生成一个buff图标
        {
            buffs.Add(buff);
            tempList.Add(buff);
            buffNum =1;
            if(buff.buffIcon==null)
            {
                buff.CheckBuffIcon();
                
            }
            buff.buffIcon.buffNum=buffNum;
            buff.buffIcon.ResetTime();
            return;
        }

        if(UIBattle.Instance ==null)
        {
            return;
        }
        
        //重置持续时间
        for (int i = 0; i < tempList.Count; i++)
        {
            // tempList[i].buffNum++;
            if(tempList[i].buffIcon==null)
            {
                tempList[i].CheckBuffIcon();
                
            }
            tempList[i].buffIcon.buffNum=buffNum;
            tempList[i].buffIcon.ResetTime();    
        }
        if(ifMax)
        {
            //执行当达到最大层数后就xxx的事件
            BuffManager.instance.OnBuffMax(buff);
        }
    }
    #endregion
    public void ClearSummon()
    {
        for (int i = summonPoint.childCount-1; i >=0 ; i--)
        {
            summonPoint.GetChild(i).GetComponent<Summoned>().Death();
            // Destroy(summonPoint.GetChild(i).gameObject);
        }
    }

    public void InvokeSummonedLifeTimeUpdate(int num)
    {
        if(OnUpdateSummonedLifeTime!=null)
        OnUpdateSummonedLifeTime(num);
    }
    
}
