using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCard : MonoBehaviour
{
    public Skill skill;
    public Text textSkillName;
    public Image icon;
    public GameObject mask;
    public Button button;
    float innerTime =0;
    public int posID;
    void Awake()
    {
        button =GetComponent<Button>();
        button.onClick.AddListener(UseSkillCard);
    }
    public void Init(Skill skill)
    {
        this.skill =skill;
        textSkillName.text =skill.skillName;
    }

    void Update()
    {
        innerTime+=Time.deltaTime;
        if(innerTime>=Player.instance.playerActor.autoReduceMPAmount)
        {
            JudgeMP();
            innerTime =0;
        }
        
    }
    void UseSkillCard()
    {
        if(Player.instance.playerActor.WanaSpell(skill))
        {
            //移除的技能移除
            if(skill.usedToRemove)
            RemoveCard();
            else
            ThrowCard();
            //抽卡的技能抽卡
            if(skill.usedChooseCard>0)
            UIBattle.Instance.SelectSomeCards(skill.usedChooseCard);
            //弃牌的技能弃牌:弃牌数量不包含自身
            if(skill.usedThrowCard>0)
            UIBattle.Instance.ThrowHandCardsToPool(skill.usedThrowCard);
            

            CheckIfNeedSelectCard();
        }
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
        MaskCard(Player.instance.playerActor.MpCurrent<skill.manaCost);    
    }
    IEnumerator SetCardPosition(float time)
    {
        yield return new WaitForSeconds(time);
        transform.SetParent(UIBattle.Instance.t_handCards);
        transform.localScale =Vector3.one;
        float _x =posID<4?80+(posID)*180:80+(posID-4)*180;
        float _y =posID>3?-320:-100;
        GetComponent<RectTransform>().anchoredPosition3D =new Vector3(_x,_y,0);
    }
    public void MaskCard(bool ifmask)
    {
        mask.SetActive(ifmask);
    }
    public static void CheckIfNeedSelectCard()
    {
        Debug.Log("手牌数量："+Player.instance.playerActor.handCards.Count); 
        if(Player.instance.playerActor.handCards.Count ==0)
        {
            UIBattle.Instance.DealCards();
        }
    }

}
