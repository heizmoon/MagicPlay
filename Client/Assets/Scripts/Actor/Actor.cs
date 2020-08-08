using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    dead =3
}
public class Actor : MonoBehaviour
{
    // Start is called before the first frame update
    public string actorName;
    public int HpMax;//最大HP
	public int MpMax;
	public int HpCurrent;//当前HP
    public int MpCurrent;
    public Transform spellPoint;
    public Transform castPoint;
    public Transform hitPoint;
	public int[] UsingSkillsID;//角色的技能列表（需要填写的）
    ///<summary>0=玩家角色；1=敌人；2=NPC</summary>
    public ActorType actorType;
    public int dodge;//闪避
    public int tough;//韧性
    public List<int> resistance =new List<int>();//0~7:水，火，风，土，心灵，物质，能量，时空 (真理魔法无法抵抗) 

    public Skill[] skills=new Skill[4];//读取后的技能列表 ，其中第一个为自动释放的技能
    public List<Buff> buffs =new List<Buff>();

    public Animator animator;//角色身上的Animator
    HPBar castingbar;//角色的施法条
    HPBar hpBar;
    HPBar mpBar;
    public BattleText bt;

    public AnimState animState ;
    ///<summary>角色将要改变成的动作状态 0=idle,1=spell,2=casting,3=castEnd,4=dizzy,5=dead</summary>
    int NextState;
    Timer timer;//角色泛用计时器
    Transform pool;
    // int TempSkillNumber =0;
    ///<summary>用于敌人和NPC的数值成长判断</summary>
    public int level;
    public int skillGrow;
    public Actor target;
    float autoReduceHpInterval=5;
    float autoReduceMpInterval=5;
    float currentAutoReduceHPTime;
    float currentAutoReduceMPTime;
    private Skill channelSkill;

    void Start()
    {
        
        
    }
    public void InitActor()
    {
        pool = GameObject.Find("Pool").transform;
        animator =gameObject.GetComponent<Animator>();
        if(animator ==null)
        {
            animator =GetComponentInChildren<Animator>();
        }
        bt = gameObject.GetComponentInChildren<BattleText>();
        timer =gameObject.GetComponent<Timer>();
    }
    public void InitPlayerActor()
    {
        HpMax =Player.instance.hp+Player.instance.basicHp;
        MpMax =Player.instance.mp+Player.instance.basicMp;
        dodge =Player.instance.dodge+Player.instance.basicDodge;
        tough =Player.instance.tough+Player.instance.basicTough;
        for (int i = 0; i < resistance.Count; i++)
        {
            resistance[i] =Player.instance.resistance[i];
        }
        
    }
    public void InitEnemy(MonsterTypeData data)
    {
        actorName =data.monsterName;
        HpMax =data.hp+data.hpGrow*level;
        HpCurrent =HpMax;
        MpMax =data.mp+data.mpGrow*level;
        MpCurrent =MpMax;
        dodge =data.dodge+data.dodgeGrow*level;
        tough =data.tough+data.toughGrow*level;
        resistance.Add(data.resistance0+data.resistanceGrow*level);
        resistance.Add(data.resistance1+data.resistanceGrow*level);
        resistance.Add(data.resistance2+data.resistanceGrow*level);
        resistance.Add(data.resistance3+data.resistanceGrow*level);
        resistance.Add(data.resistance4+data.resistanceGrow*level);
        resistance.Add(data.resistance5+data.resistanceGrow*level);
        resistance.Add(data.resistance6+data.resistanceGrow*level);
        resistance.Add(data.resistance7+data.resistanceGrow*level);
        skillGrow =data.skillGrow;
        for(int i =0;i<data.skills.Split(',').Length;i++)
        {
            UsingSkillsID[i] =int.Parse(data.skills.Split(',')[i]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(UIBattle.Instance!=null&& !UIBattle.Instance.isBattleOver&& animState!= AnimState.dead)
        {
            currentAutoReduceHPTime+=Time.deltaTime;
            if(currentAutoReduceHPTime>=autoReduceHpInterval)
            {
                AutoReduceHP();
            }
            if(currentAutoReduceMPTime>=autoReduceMpInterval)
            {
                AutoReduceMP();
            }
        }
    }

    void AutoReduceHP()
    {
        AddHp(1);
        currentAutoReduceHPTime =0;
    }
    void AutoReduceMP()
    {
        AddMp(1);
        currentAutoReduceMPTime =0;

    }
    public void SetSkillList(int[] skills)
    {
        UsingSkillsID =skills;
    }
    public void InitMagic()//初始化技能数值
    {
        
        if(actorType ==ActorType.NPC)
        {
            // return;
        }
        //1.读取角色技能列表
        
        for(int i =0;i<UsingSkillsID.Length;i++)
        {
            Skill sk =SkillManager.TryGetFromPool(UsingSkillsID[i],this);
            skills[i] =sk;
            // if(UIPractice.instance !=null)
            // {
            //     Debug.LogWarningFormat("练习状态下新建技能：{0}",sk.skillName);
            // }
            // else if(sk!=null)
            // {
            //     Debug.LogWarningFormat("新建技能：{0}，目标是{1}",sk.skillName,sk.target.name);

            // }
            // else
            // {
            //     Debug.LogWarningFormat("没有创建技能：{0}",UsingSkillsID[i]);
            // }
        }        
    }
    public void RunAI()
    {
        //开始自动攻击
        AutoAttack();
        //AI类别1：无限使用技能1
        //AI类别2：每隔X秒，使用一次技能2
        //AI类别3：当魔力满时，使用一次技能2
        //AI类别4：当生命低于X时，使用技能Y
    }
    public Skill GetSkills(int id)
    {
        return skills[id];
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
                mpBar.initHpBar(MpCurrent,MpMax);
                    
            }

            castingbar= GameObject.Find("CastingBar_Player").GetComponent<HPBar>();
            
        }
        else
        {
            castingbar =GameObject.Find("CastingBar_Enemy").GetComponent<HPBar>();
            hpBar = GameObject.Find("HP_Enemy").GetComponent<HPBar>();
            mpBar = GameObject.Find("MP_Enemy").GetComponent<HPBar>();
            mpBar.initHpBar(MpCurrent,MpMax);
            // Debug.Log("HPBAR");
        }
        
        castingbar.BindHPBar(this);
            
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
        //角色自身状态不为dizzy，dead
        if(animState ==AnimState.dead)
        {
            
            return false;
        }
        if(animState ==AnimState.dizzy)
        {
            
            return false;
        }
        // if((int)animState>1)
        // {
        //     Debug.Log("当前角色状态无法施法");
        //     return false;
        // }
        if(UIPractice.instance.enable ==true)
        {

        }
        if(UIPractice.instance.enable ==false&&skill.realManaCost>MpCurrent)//法力值不足
        {
            // Debug.LogFormat("魔法值不足,需要{0},当前为{1}",skill.realManaCost,MpCurrent);
            return false;
        }
        StopCasting();
        //如果目标已经死了
        if(target!=null&&target.animState ==AnimState.dead)
        {
            return false;
        }
        StartCoroutine(WaitForBeginSpell(skill));
        return true;
        
    }
    IEnumerator WaitForBeginSpell(Skill skill)
    {
        yield return new WaitForSeconds(0.3f);
        
        if(animState!=AnimState.dead&&animState!=AnimState.dizzy)
        BeginSpell(skill);
    }
    void BeginSpell(Skill skill)//开始施放X技能,需要传入一个技能
    {
        if(Main.instance.UIState>0&&!DateManager.instance.timer.enabled)
        {
            StopCasting();
            return;
        }
        //1.切换Animator到1，开始循环播放施法动作spell
        ChangeAnimatorInteger(1);
        //如果不在练习状态
        // if(Player.instance.playerNow!=""&&Player.instance.playerNow.Substring(0,1) =="A")
        if(Main.instance.UIState <=0)
        {
            //调整施法时间
            skill.ModiferCastSpeed();
            //2.消耗MP，数值为技能所需
            
            AddMp(-skill.realManaCost);
            
            //3.施法条开始自行运动
            castingbar.changeHPBar(skill.spelllTime+0f);
        }
        else
        {
            timer.start(skill.spelllTime,OnPracticeTimerComplete);
        }
        //4.设置角色状态为casting
        animState=AnimState.casting;
        //5.创建施法特效
        CreateSpellEffect(skill);
        //6.绑定技能和施法条
        if(castingbar)
        castingbar.BindHPBar(skill);
        //如果是引导类技能
        if(skill.isChannel)
        {
            channelSkill =skill;
        }
        else
        {
            channelSkill =null;
        }
    }
    //引导类技能，从可扩展性考虑，spell→casting→ channeling(循环)→ castEnd
    //每次channeling，需要进行一次施法判定，如果当前状态不能施法，则停止
    //castEnd动作来代替channel动作
    //那么每当castEnd动作播放完毕，就进行一次判定
    
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
        castingbar.stopChanging(true);
       
    //    if((int)animState<2)
    //    {
           
    //    }
        //移除施法特效
       EffectManager.TryThrowInPool(spellPoint);
       EffectManager.ClearChannelEffect(castPoint);
       if(UIPractice.instance.enable)
       {
           timer.stop();
       }
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
            if(channelSkill!=null)
            {
                //判定是否能够再次播放castEnd
                if(UIPractice.instance.enable)
                {
                    OnTimerComplete(channelSkill);
                    break;
                }
                else if(channelSkill.realManaCost<=MpCurrent)
                {
                    AddMp(-channelSkill.realManaCost);
                    OnTimerComplete(channelSkill);
                    break;
                }

            }

            NextState =0;
            if((int)animState<2)
            {
                animState =AnimState.idle; 
            }
            
            StartCoroutine(NextTurn());

            break;
        }
        if(i!=NextState)
        {
            ChangeAnimatorInteger(NextState);
        }
    }
    //执行下一轮动作逻辑
    IEnumerator NextTurn()
    {
        yield return new WaitForFixedUpdate();

       RunAI();
    }
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
        if(UIPractice.instance==null)
        {
            skill.ComputeDamage();
        }
        //执行技能释放完毕事件
        
        OnSkillSpellFinish(skill);
    }
    public void ChangeAnimatorInteger(int i)
    {
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
    void CreateCastEffect(Skill skill)
    {
        if(skill.castEffect=="")
        {
            return;
        }
         //如果引导类技能已经有了特效，不再再次创建特效
        if(skill.isChannel)
        {
            for (int i = 0; i < castPoint.childCount; i++)
            {
                Debug.LogWarning(castPoint.GetChild(i).name);
                if(castPoint.GetChild(i).name == skill.castEffect+"(Clone)")
                {
                    
                    return;
                }
            }    
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
                if(skill.isChannel)
                {
                    f.localScale =Vector3.one;
                    EffectManager.CastEffect(f);
                }
                else
                {
                    EffectManager.CastEffect(f,target.hitPoint,skill.damageDelay,skill.hitEffect);
                }
            }
            else
            {
                if(skill.isChannel)
                {
                    f.localScale =Vector3.one;
                    EffectManager.CastEffect(f);
                }
                else
                {
                    EffectManager.CastEffect(f,UIPractice.instance.targetPoint,skill.damageDelay,skill.hitEffect);
                }
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
    public void AddHp(int num)
    {
        // if(this.actorType == ActorType.玩家角色)
        // Debug.LogWarningFormat("{0}的生命值变化{1},最终为{2}",this.name,num,HpCurrent);
        
        if(HpCurrent+num>=HpMax)
        {
            HpCurrent =HpMax;
        }
        else if(HpCurrent+num <= 0)
        {
            HpCurrent = 0;
        }
        else
        {
            HpCurrent =HpCurrent+num;
        }
        if(hpBar!=null)
        {
            hpBar.changeHPBar(HpCurrent,true);
        }
        // if(this.actorType == ActorType.敌人)
        if(num>0)
        Debug.LogWarningFormat("{0}的生命值变化{1},最终为{2}",this.name,num,HpCurrent);
    }
    public void OnSkillHasCrit(Skill skill, int damage)//通知角色技能暴击，暴击伤害为damage
    {
        //执行当技能产生暴击时就xxx这类效果
        //暴击检查点燃Buff 和 燃爆 等级,满足条件造成燃爆伤害
        SkillManager.CheckSkillToTriggerNewSkill(skill,1112,null,new List<int>(){1},null,new List<int>(){4},false);
    }
    public void OnSkillHasHit(bool ifHit ,Skill skill)
    {
        if(ifHit)
        {
            //执行当技能命中目标时就xxx这类效果
            if(skill.buffID>0&&!skill.targetSelf)
            {
                BuffManager.instance.CreateBuffForActor(skill.buffID,skill.level,skill.target);
            }
            // Check1110(skill);
            //火系技能添加点燃buff
            SkillManager.CheckSkillOnHitToAddBuff(skill,1110,4,new List<int>(){1110},new List<int>(){1},null);
            //冻结
            SkillManager.CheckSkillOnHitToAddBuff(skill,1011,6,new List<int>(){1112},new List<int>(){0},new List<int>(){7});
            //引火烧身
            SkillManager.instance.Check1117(skill);
            
            // Check1011(skill);

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
    public void TakeDamage(int num,bool crit,int genre,bool ifRebound)//传入技能伤害,是否暴击,是否为反弹伤害
    {
        //如果角色已经死亡，则不会再承受伤害
        if(animState ==AnimState.dead)
        {
            target.StopCasting();
            return;
        }
        
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
                    Battle.Instance.ReceiveSkillDamage(skill,skill.damage,true);
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
                            Battle.Instance.ReceiveSkillDamage(skill,Mathf.CeilToInt(buffs[i].currentValue),true);
                            skill.target =this;
                        }
                    }
                    else
                    {
                        Battle.Instance.ReceiveSkillDamage(Mathf.CeilToInt(buffs[i].currentValue),target,genre);
                    }
                    
                    // Debug.LogFormat("百分比反弹buff名为：{0},效果类型为：{1},影响派系为：{2}",buffs[i].buffData.name,buffs[i].buffData._type,buffs[i].buffData.genreList);
                }
            }
        }
        ///<summary>是否吸收伤害</summary>
        bool ifAbsorb =false;
        int tempNum =0;
        //治疗不会被吸收
        if(num>=0)
        {
        //吸收伤害在受到伤害之前执行
            for (int i = 0; i < buffs.Count; i++)
            {
                if(buffs[i].buffData._type == BuffType.数值吸收伤害 && buffs[i].buffData._genreList.Contains(genre))
                {
                    //显示特效
                    Transform e = EffectManager.TryGetFromPool(buffs[i].buffData.triggerEffect);
                    if(e!=null)
                        {
                            e.SetParent(target.hitPoint);
                            e.localPosition =Vector3.zero;
                            e.localScale =Vector3.one;
                        }
                    //伤害减少
                    //护盾被打爆
                    if(num>=buffs[i].currentValue)
                    {
                        num-=Mathf.CeilToInt(buffs[i].currentValue);
                        tempNum =Mathf.CeilToInt(buffs[i].currentValue);
                        Debug.LogFormat("由于{2}的buff效果，吸收了{0}点伤害,护盾剩余值为{1}",buffs[i].currentValue,0,buffs[i].buffData.name);
                        // item.currentValue =0;
                        buffs[i].buffIcon.OnEffectEnd();
                        //执行护盾被打爆事件

                    }
                    //护盾没被打爆
                    else
                    {
                        buffs[i].currentValue-=num;
                        tempNum =num;
                        Debug.LogFormat("由于{2}的buff效果，吸收了{0}点伤害,,护盾剩余值为{1}",num,buffs[i].currentValue,buffs[i].buffData.name);
                        num =0;
                    }
                    ifAbsorb =true;
                }
            }
            
            for (int i = 0; i < buffs.Count; i++)
            
            {
                if(buffs[i].buffData._type == BuffType.百分比吸收伤害 && buffs[i].buffData._genreList.Contains(genre))
                {
                    //显示特效
                    Transform e = EffectManager.TryGetFromPool(buffs[i].buffData.triggerEffect);
                    if(e!=null)
                        {
                            e.SetParent(target.hitPoint);
                            e.localPosition =Vector3.zero;
                            e.localScale =Vector3.one;
                        }
                    //伤害减少
                    
                    num = Mathf.CeilToInt( num*(1-buffs[i].currentValue));
                    
                }
            }
        }
        //执行{受到伤害时就xxx这类效果}
        
        //执行 {受到暴击时就xxx}
        if(crit)
        {

        }

        //2.扣除生命值，判断生命值是否归零
        //先改变生命再判断
        int tempHP = HpCurrent;
        AddHp(-num);
        bool ifDie =false;;
        //3.伤害文字
        if(ifAbsorb)
        {
            if(tempNum!=0)
            {
                bt.SetText("吸收"+tempNum);
            }
        }
        else
        {
            bt.SetText(num,crit);
        }
        
        Debug.LogFormat("角色：{0}的当前生命值：{1}",name,HpCurrent);
            
        //如果是掉血效果
        if(num>0)
        {
            //执行当角色生命值低于50%，就xxx这类效果
            if(tempHP>0.5*HpMax&&HpCurrent<0.5*HpMax)
            {

            }
            //执行当角色生命值低于25%，就xxx这类效果
            if(tempHP>0.5*HpMax&&HpCurrent<0.5*HpMax)
            {

            }
            
        }
        //如果是加血效果
        else
        {
            
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
        
    }
    public void OnSkillSpellFinish(Skill skill)
    {
        //技能释放完毕事件
        if(skill.buffID>0&&skill.targetSelf)
        {
            BuffManager.instance.CreateBuffForActor(skill.buffID,skill.level,skill.target);
        }
        //如果在练习状态
        if(UIPractice.instance.enable)
        {
            //提升技能熟练度

        }else
        {
            //魔力回复
            skill.caster.AddMp(skill.manaProduce);
        }    
    }
    public void AddMp(int num)
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
            mpBar.changeHPBar(MpCurrent,true);
        }
        if(this.actorType ==ActorType.玩家角色)
        {
            UIBattle.Instance.CheckButtonMP();
        }
    }
    public void OnHitMiss()
    {
        //显示文字 未命中
        bt.SetText("未命中");
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
        mpBar.initHpBar(MpCurrent,MpMax);
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
        mpBar.initHpBar(MpCurrent,MpMax);
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
        else if(buff.buffData.maxNum!=0&&buffNum<buff.buffData.maxNum)
        {
            buffs.Add(buff);
            tempList.Add(buff);
            buffNum++;
        }
        //已叠加到最大层
        else if(buff.buffData.maxNum!=0&&buffNum==buff.buffData.maxNum)
        {
            //去掉一层，再重新添加一层
            
            BuffManager.RemoveBuffFromActor(tempList[0],this);
            tempList.Remove(tempList[0]);
            buffs.Add(buff);
            tempList.Add(buff);
            ifMax =true;
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
    
}
