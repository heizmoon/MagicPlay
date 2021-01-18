using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    Text textSkillDescribe;
    Text textSkillCost;
    public bool canShow;
    
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
    }
    public void Init(Skill skill)
    {
        this.skill =skill;
        skill.skillCard =this;
        textSkillName.text =skill.skillName;
        RefeashCardShow();
        if(skill.color ==0)
        {
            backgroud.sprite = Resources.Load<Sprite>("Texture/UI/UI_Card_Attack");
        }
        else if(skill.color ==1)
        {
            backgroud.sprite = Resources.Load<Sprite>("Texture/UI/UI_Card_Defence");
        }
        else if(skill.color ==2)
        {
            backgroud.sprite = Resources.Load<Sprite>("Texture/UI/UI_Card_Equipment");
        }
        else if(skill.color ==3)
        {
            backgroud.sprite = Resources.Load<Sprite>("Texture/UI/UI_Card_Special");
        }
        icon.sprite = Resources.Load<Sprite>("Texture/Skills"+skill.icon);
        if(skill.rank <2)
        {
            rank.sprite = Resources.Load<Sprite>("Texture/UI/UI_CardRank_Normal");
        }
        else if(skill.rank <4)
        {
            rank.sprite = Resources.Load<Sprite>("Texture/UI/UI_CardRank_Good");
        }
        else if(skill.rank <6)
        {
            rank.sprite = Resources.Load<Sprite>("Texture/UI/UI_CardRank_Epic");
        }
        else
        {
            rank.sprite = Resources.Load<Sprite>("Texture/UI/UI_CardRank_Legend");
        }
        
        
        
    }
    public void Init(SkillData skillData)
    {
        textSkillName.text =skillData.name;
        textSkillDescribe.text =string.Format(skillData.describe,skillData.damage,skillData.manaCost,skillData.manaProduce);
        textSkillCost.text =skillData.manaCost.ToString();
        if(skillData.color ==0)
        {
            backgroud.sprite = Resources.Load<Sprite>("Texture/UI/UI_Card_Attack");
        }
        else if(skillData.color ==1)
        {
            backgroud.sprite = Resources.Load<Sprite>("Texture/UI/UI_Card_Defence");
        }
        else if(skillData.color ==2)
        {
            backgroud.sprite = Resources.Load<Sprite>("Texture/UI/UI_Card_Equipment");
        }
        else if(skillData.color ==3)
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

        textSkillDescribe.text =skill.describe;

        if(Player.instance.playerActor.basicAttack>0||skill.damage>skill.skillData.damage)
        skill.describe =string.Format(skill.skillData.describe,"<color=cyan>"+Mathf.Abs(skill.damage)+"</color>",Mathf.Abs(skill.realManaCost),Mathf.Abs(skill.manaProduce));//{0}=damage,{1}=manaCost,{2}=manaProduce,{3}=crit;{4}=hit;{5}=seep;{6}=fast
        else if(Player.instance.playerActor.basicAttack==0||skill.damage==skill.skillData.damage)
        skill.describe =string.Format(skill.skillData.describe,"<color=white>"+Mathf.Abs(skill.damage)+"</color>",Mathf.Abs(skill.realManaCost),Mathf.Abs(skill.manaProduce));
        else
        skill.describe =string.Format(skill.skillData.describe,"<color=red>"+Mathf.Abs(skill.damage)+"</color>",Mathf.Abs(skill.realManaCost),Mathf.Abs(skill.manaProduce));



        textSkillCost.text =skill.realManaCost.ToString();
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
        if(Player.instance.playerActor.WanaSpell(skill))
        {
            //移除的技能移除
            if(skill.usedToRemove)
            RemoveCard();
            else
            ThrowCard();
            
            //弃牌的技能弃牌:弃牌数量不包含自身
            if(skill.usedThrowCard>0)
            {
                int throwNum = UIBattle.Instance.ThrowHandCardsToPool(skill.usedThrowCard);
                Debug.Log("弃牌数="+throwNum);
                if(throwNum>0)
                {
                    //执行每当弃牌时就XX的事件
                    if(skill.id == 107)//技能--质能转换
                    {
                        skill.target.AddMp(throwNum);//每弃一张牌回复1点能量
                    }
                    if(skill.id == 110)//技能--破釜沉舟
                    {
                        //每弃一张牌获得4点护甲
                        skill.target.armor+= 4*(throwNum+1);
                        skill.target.RefeashArmorAutoDecayTime();
                    }
                }
            }
            if(Player.instance.playerActor.abilities.Contains(4))
            {
                Player.instance.playerActor.AddMp(1);
            }
            CheckIfNeedSelectCard();
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
        MaskCard(Player.instance.playerActor.MpCurrent<skill.realManaCost);    
    }
    IEnumerator SetCardPosition(float time)
    {
        yield return new WaitForSeconds(time);
        transform.SetParent(UIBattle.Instance.t_handCards);
        transform.localScale =Vector3.one;
        float _x =posID<4?80+(posID)*180:80+(posID-4)*180;
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
    public void LegacyCard()
    {
        if(skill.skillData.ELCDamage!=0)
        {
            IncreaseDamage(skill.skillData.ELCDamage);
        }
        if(skill.skillData.ELCMP!=0)
        {
            ReduceMPCost(skill.skillData.ELCMP);
        }
        if(skill.skillData.ELCHeal!=0)
        {
            IncreaseHeal(skill.skillData.ELCHeal);
        }
        RefeashCardShow();
    }
    //1.全局降低/增加卡牌能量消耗
    //生效时机：第一次使用，新卡牌出现，效果结束时
    //2.降低/增加 某类卡牌消耗
    //
    //3.降低/增加 自身的消耗
    public void ReduceMPCost(int num)
    {
        skill.tempMpCost -=num;
        if(skill.tempMpCost<0)
        skill.realManaCost =0;
        else
        skill.realManaCost =skill.tempMpCost;
    }
    public void IncreaseDamage(int num)
    {
        // Debug.Log("增加了攻击");
        skill.tempDamage +=num;
        if(skill.tempDamage<0)
        skill.damage =0;
        else
        skill.damage =skill.tempDamage;
    }
    public void IncreaseHeal(int num)
    {
        skill.tempHeal +=num;
        if(skill.tempHeal<0)
        skill.heal =0;
        else
        skill.heal =skill.tempHeal;
    }

}
