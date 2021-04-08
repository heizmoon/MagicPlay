using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SkillCard : MonoBehaviour
{
    public Skill skill;
    Text textSkillName;
    Image icon;
    Image rank;
    Image backgroud;
    public GameObject mask;
    public Button button;
    float innerTime =0;
    public int posID;
    public Text textSkillDescribe;
    Text textSkillCost;
    public bool canShow;
    public GameObject _hightLight;
    bool _enable;
    void Awake()
    {
        button =GetComponent<Button>();
        backgroud =GetComponent<Image>();

        button.onClick.AddListener(UseSkillCard);

        textSkillName =transform.Find("cardName").GetComponent<Text>();
        textSkillDescribe =transform.Find("cardDescribe").GetComponent<Text>();
        textSkillCost =transform.Find("cardCost").GetComponent<Text>();
        icon = transform.Find("icon").GetComponent<Image>();
        rank =transform.Find("rank").GetComponent<Image>();
        // _hightLight = transform.Find("HighLight").gameObject;
    }
    public void Init(Skill skill)
    {
        this.skill =skill;
        skill.skillCard =this;
        textSkillName.text =skill.skillName;
        RefeashCardShow();
        if(skill.type ==0)
        {
            backgroud.sprite = Resources.Load<Sprite>("Texture/UI/UI_Card_Attack");
        }
        else if(skill.type ==1)
        {
            backgroud.sprite = Resources.Load<Sprite>("Texture/UI/UI_Card_Defence");
        }
        else if(skill.type ==2)
        {
            backgroud.sprite = Resources.Load<Sprite>("Texture/UI/UI_Card_Equipment");
        }
        else if(skill.type ==3)
        {
            backgroud.sprite = Resources.Load<Sprite>("Texture/UI/UI_Card_Special");
        }
        icon.sprite = Resources.Load<Sprite>("Texture/Skills"+skill.icon);
        if(skill.rank <1)
        {
            rank.sprite = Resources.Load<Sprite>("Texture/UI/UI_CardRank_Normal");
        }
        else if(skill.rank <2)
        {
            rank.sprite = Resources.Load<Sprite>("Texture/UI/UI_CardRank_Good");
        }
        else if(skill.rank <3)
        {
            rank.sprite = Resources.Load<Sprite>("Texture/UI/UI_CardRank_Epic");
        }
        else
        {
            rank.sprite = Resources.Load<Sprite>("Texture/UI/UI_CardRank_Legend");
        }
        if(skill.skillData.checkBuff>0)
        {
            BuffIcon.OnBuffAction+=CheckBuffCard;
        }
        
        
    }
    public void Init(SkillData skillData)
    {
        textSkillName.text =skillData.name;
        textSkillDescribe.text =string.Format(skillData.describe,skillData.damage+Player.instance.playerActor.basicAttack,
                                skillData.manaCost,skillData.manaProduce,skillData.addArmor+Player.instance.playerActor.basicDefence);
        textSkillCost.text =skillData.manaCost.ToString();
        if(skillData.type ==0)
        {
            backgroud.sprite = Resources.Load<Sprite>("Texture/UI/UI_Card_Attack");
        }
        else if(skillData.type ==1)
        {
            backgroud.sprite = Resources.Load<Sprite>("Texture/UI/UI_Card_Defence");
        }
        else if(skillData.type ==2)
        {
            backgroud.sprite = Resources.Load<Sprite>("Texture/UI/UI_Card_Equipment");
        }
        else if(skillData.type ==3)
        {
            backgroud.sprite = Resources.Load<Sprite>("Texture/UI/UI_Card_Special");
        }
        icon.sprite = Resources.Load<Sprite>("Texture/Skills"+skillData.icon);
        if(skillData.rank <2)
        {
            rank.sprite = Resources.Load<Sprite>("Texture/UI/UI_CardRank_Normal");
        }
        else if(skillData.rank <4)
        {
            rank.sprite = Resources.Load<Sprite>("Texture/UI/UI_CardRank_Good");
        }
        else if(skillData.rank <6)
        {
            rank.sprite = Resources.Load<Sprite>("Texture/UI/UI_CardRank_Epic");
        }
        else
        {
            rank.sprite = Resources.Load<Sprite>("Texture/UI/UI_CardRank_Legend");
        }
        canShow = true;
    }
    public void RefeashCardShow()
    {
        if(skill.damage>skill.skillData.damage)
        skill.describe =string.Format(skill.skillData.describe,"<color=cyan>"+Mathf.Abs((skill.damage+Player.instance.playerActor.basicAttack))+"</color>",Mathf.Abs(skill.realManaCost)+skill.skillData.keepManaCost,Mathf.Abs(skill.manaProduce),Mathf.Abs(skill.addArmor+Player.instance.playerActor.basicDefence));//{0}=damage,{1}=manaCost,{2}=manaProduce,{3}=addArmor;{4}=hit;{5}=seep;{6}=fast
        else if(skill.damage==skill.skillData.damage)
        {
            // Debug.Log("技能显示恢复正常");
            skill.describe =string.Format(skill.skillData.describe,"<color=white>"+Mathf.Abs((skill.damage+Player.instance.playerActor.basicAttack))+"</color>",Mathf.Abs(skill.realManaCost)+skill.skillData.keepManaCost,Mathf.Abs(skill.manaProduce),Mathf.Abs(skill.addArmor+Player.instance.playerActor.basicDefence));
        }
        else
        skill.describe =string.Format(skill.skillData.describe,"<color=red>"+Mathf.Abs((skill.damage+Player.instance.playerActor.basicAttack))+"</color>",Mathf.Abs(skill.realManaCost)+skill.skillData.keepManaCost,Mathf.Abs(skill.manaProduce),Mathf.Abs(skill.addArmor+Player.instance.playerActor.basicDefence));
        if(textSkillDescribe!=null)
        textSkillDescribe.text =skill.describe;

        if(textSkillCost==null)
        {
            return;
        }
        textSkillCost.text =(skill.realManaCost+skill.skillData.keepManaCost).ToString();
        if(skill.realManaCost==skill.skillData.manaCost)
        textSkillCost.color = Color.white;
        else if(skill.realManaCost>skill.skillData.manaCost)
        textSkillCost.color = Color.red;
        else
        textSkillCost.color = Color.green;
    }

    void Update()
    {
        if(UIBattle.Instance==null||UIBattle.Instance.ifPause)
        {
            return;
        }
        innerTime+=Time.deltaTime;
        if(innerTime>=Player.instance.playerActor.autoReduceMPAmount)
        {
            JudgeMP();
            innerTime =0;
        }
        
    }
    void UseSkillCard()
    {
        if(canShow)
        {
            ExploreSkillCard();
            return;
        }
        if(Time.timeScale==0)
        {
            ExploreSkillCard();
            return;
        }
        if(UIBattle.Instance.ifPause)
        {
            ExploreSkillCard();
            return;
        }
        if(Player.instance.playerActor.WanaSpell(skill))
        {
            //已经使用，不可再次点击
            if(!_enable)
            {
                return;
            }
            _enable = false;
            UIBattle.Instance.OnUseCard(this);
            //移除的技能移除
            if(skill.usedToRemove)
            RemoveCard();
            else
            ThrowCard();
            
            //弃牌的技能弃牌:弃牌数量不包含自身
            CardThrowCard(skill);
            //创建卡牌的牌
            CardCreateCard(skill);
            //抽卡的技能抽卡
            if(skill.usedChooseCard>0)
            UIBattle.Instance.SelectSomeCards(skill.usedChooseCard);
            CheckIfNeedSelectCard();
        }
    }
    public static void CardCreateCard(Skill _skill)
    {
        if(_skill.skillData.createCardNum>0)
            {
            Debug.LogWarning("创建卡牌"+_skill.skillName);
                for (int i = 0; i < _skill.skillData.createCardNum; i++)
                {
                    if(_skill.skillData.createCardID==0)
                    {
                        int r =SkillManager.instance.GetRandomSkillByType(_skill.skillData.createCardChar,_skill.skillData.createCardType);
                        Debug.LogWarning("随机到卡牌id为"+r);
                        UIBattle.Instance.CreateNewCardAndGiveToHand(r);
                    }
                    else
                    {
                        UIBattle.Instance.CreateNewCardAndGiveToHand(_skill.skillData.createCardID);
                    } 
                }
                    
            }
    }
    public static void CardThrowCard(Skill _skill)
    {
        if(_skill.usedThrowCard>0)
            {
                int throwNum = UIBattle.Instance.ThrowHandCardsToPool(_skill.usedThrowCard);
                Debug.Log("弃牌数="+throwNum);
                if(throwNum>0)
                {
                    //执行每当弃牌时就XX的事件
                    UIBattle.Instance.OnThorwCard(throwNum);
                    if(_skill.id == 107)//技能--质能转换
                    {
                        _skill.target.AddMp(throwNum);//每弃一张牌回复1点能量
                    }
                    if(_skill.id == 110)//技能--破釜沉舟
                    {
                        //每弃一张牌获得4点护甲
                        _skill.target.armor+= 4*(throwNum+1);
                        _skill.target.RefeashArmorAutoDecayTime();
                    }
                }
            }
    }
    IEnumerator IESpecialAddBuff(int num)
    {
        yield return new WaitForEndOfFrame();
        // BuffManager.instance.CreateBuffForActor(skill.buffID,skill.target);//每弃一张牌获得4点护甲
        //修改BuffIcon显示层数
        BuffManager.instance.TryModiferBuffIconNum(skill.buffID,num+1,skill.target);
    }
    // IEnumerator WaitForUseCard()
    // {
    //     yield return new WaitForSeconds(0.1f);
        
    // }
    void ExploreSkillCard()
    {
        //放大卡牌看
        UICardDetail cardDetail = UICardDetail.CreateUI();
        cardDetail.Init(this);
        canShow =false;

    }
    ///<summary>卡牌进入弃牌堆</summary>
    public void ThrowCard()
    {
        Debug.Log(skill.skillName+"进入弃牌堆");
        if(Player.instance.playerActor.handCards.Contains(this))
        Player.instance.playerActor.handCards.Remove(this);

        if(!UIBattle.Instance.usedCardsList.Contains(this))
        UIBattle.Instance.usedCardsList.Add(this);

        UIBattle.Instance.RemoveCardPos(this.posID);
        StartCoroutine(IEThrowCardToUsedPool());
    }
    ///<summary>进入弃牌堆(延迟)</summary>
    IEnumerator IEThrowCardToUsedPool()
    {
        
        yield return new WaitForSeconds(0.3f);
        transform.SetParent(UIBattle.Instance.t_cardsPool);
        transform.localPosition =Vector3.zero;
        transform.localScale =Vector3.one;
    }

    ///<summary>卡牌从本场移除</summary>
    public void RemoveCard()
    {
        if(Player.instance.playerActor.handCards.Contains(this))
        Player.instance.playerActor.handCards.Remove(this);

        if(UIBattle.Instance.usedCardsList.Contains(this))
        UIBattle.Instance.usedCardsList.Remove(this);

        UIBattle.Instance.removeCarsList.Add(this);
        
        UIBattle.Instance.RemoveCardPos(this.posID);
        StartCoroutine(IEThrowCardToUsedPool());

    }
    //把这张技能卡加入手牌
    public void GiveToHand(float delay)
    {
        Player.instance.playerActor.handCards.Add(this);
        // StartCoroutine(ThrowCardToHand());
        RefeashCardShow();
        StartCoroutine(SetCardPosition(delay));
        _enable = true;

    }
    // IEnumerator IEThrowCardToHand()
    // {
    //     yield return new WaitForSeconds(0.3f);
        
    //     transform.SetParent(UIBattle.Instance.t_handCards);
    //     transform.localPosition =Vector3.zero;
    //     transform.localScale =Vector3.one;
    // }
    void JudgeMP()
    {
        MaskCard(Player.instance.playerActor.MpCurrent<skill.realManaCost+skill.skillData.keepManaCost);    
    }
    IEnumerator SetCardPosition(float time)
    {
        yield return new WaitForSeconds(time);
        transform.SetParent(UIBattle.Instance.t_handCards);
        transform.localScale =Vector3.one;
        float _x =posID<4?80+(posID)*175:80+(posID-4)*175;
        float _y =posID>3?-375:-120;
        GetComponent<RectTransform>().anchoredPosition3D =new Vector3(_x,_y,0);
    }
    public void MaskCard(bool ifmask)
    {
        mask.SetActive(ifmask);
    }
    ///<summary>检查手牌数量是否小于自动补牌阈值，小于则自动补牌</summary>
    public static void CheckIfNeedSelectCard()
    {
        Debug.Log("手牌数量："+Player.instance.playerActor.handCards.Count); 
        if(Player.instance.playerActor.handCards.Count <=Player.instance.playerActor.autoDealCardsMinValue)
        {
            UIBattle.Instance.DealCards();
        }
    }
    public void LegacyCard()//遗留效果
    {
        int temp =0;
        if(skill.skillData.ELCDamage!=0)
        {
            skill.IncreaseDamage(skill.skillData.ELCDamage);
            temp++;
        }
        if(skill.skillData.ELCMP!=0)
        {
            skill.ReduceMPCost(skill.skillData.ELCMP);
            temp++;
        }
        if(skill.skillData.ELCHeal!=0)
        {
            skill.IncreaseHeal(skill.skillData.ELCHeal);
            temp++;
        }
        if(temp>0)
        {
            Debug.Log("有牌被遗留了！");
            UIBattle.Instance.OnLegacyCard();
        }
        RefeashCardShow();
    }
    //1.全局降低/增加卡牌能量消耗
    //生效时机：第一次使用，新卡牌出现，效果结束时
    //2.降低/增加 某类卡牌消耗
    //
    //3.降低/增加 自身的消耗
    void CheckBuffCard(int buffID,string buffState,int buffNum,ActorType actorType)
    {
        
        //buffID是否为要检查的ID
        if (buffID!=skill.skillData.checkBuff)
        {
            return;
        }
        //buff要检查的目标是否符合要求
        if(skill.skillData.checkSelf&&actorType!=skill.caster.actorType)
        {
            return;
        }
        if(!skill.skillData.checkSelf&&actorType==skill.caster.actorType)
        {
            return;
        }
        if(buffState =="begin")//收到添加了新buff事件，检查添加后层数是否满足
        {
            //buff的数量是否达到要求
            if(buffNum<skill.skillData.buffNumLimit)
            {
                return;
            }
            skill.CheckBuffUpdate(true);
            HighLightCard(true);
        }
        if(buffState =="end")//收到buff结束事件，检查结束后层数是否满足
        {
            if(buffNum>=skill.skillData.buffNumLimit)
            {
                return;
            }
            skill.CheckBuffUpdate(false);
            HighLightCard(false);
        }
        RefeashCardShow();
    }
    public void CheckBuffCardWhileCreateCard()
    {
        if(skill.skillData.checkBuff==0)
        {
            return;
        }
        int num =0;
        if(skill.skillData.checkSelf)
        {
            if(skill.caster.buffs.Count==0)
            {
                return;
            }
            for (int i = 0; i < skill.caster.buffs.Count; i++)
            {
                if(skill.caster.buffs[i].buffData.id==skill.skillData.checkBuff)
                {
                    num++;
                }
            }
        }
        else
        {
            if(skill.caster.target.buffs.Count==0)
            {
                return;
            }
            for (int i = 0; i < skill.caster.target.buffs.Count; i++)
            {
                if(skill.caster.target.buffs[i].buffData.id==skill.skillData.checkBuff)
                {
                    num++;
                }
            }
        }
        if(num>=skill.skillData.buffNumLimit)
        {
            skill.CheckBuffUpdate(true);
            HighLightCard(true);
            RefeashCardShow();
        }
    }
    void HighLightCard(bool ifShow)//技能卡高亮显示
    {
        if(_hightLight)
        _hightLight.SetActive(ifShow);
    }

}
