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
            ThrowCard();
            CheckIfNeedSelectCard();
        }
    }
    ///<summary>卡牌进入弃牌堆</summary>
    public void ThrowCard()
    {
        if(Player.instance.playerActor.handCards.Contains(this))
        Player.instance.playerActor.handCards.Remove(this);

        if(!UIBattle.Instance.usedCardsList.Contains(this))
        UIBattle.Instance.usedCardsList.Add(this);

        StartCoroutine(ThrowCardToUsedPool());
    }
    ///<summary>进入弃牌堆(延迟)</summary>
    IEnumerator ThrowCardToUsedPool()
    {
        transform.SetParent(UIBattle.Instance.t_cardsPool);
        yield return new WaitForSeconds(0.3f);
        
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

        StartCoroutine(ThrowCardToUsedPool());

    }
    //把这张技能卡加入手牌
    public void GiveToHand()
    {
        Player.instance.playerActor.handCards.Add(this);
        // StartCoroutine(ThrowCardToHand());
        transform.SetParent(UIBattle.Instance.t_handCards);
        SetCardPosition();

    }
    IEnumerator ThrowCardToHand()
    {
        transform.SetParent(UIBattle.Instance.t_handCards);
        yield return new WaitForSeconds(0.3f);
        
        
        transform.localPosition =Vector3.zero;
        transform.localScale =Vector3.one;
    }
    void JudgeMP()
    {
        MaskCard(Player.instance.playerActor.MpCurrent<skill.manaCost);    
    }
    void SetCardPosition()
    {
        transform.localScale =Vector3.one;
        float _x =80+(posID-1)*180;
        float _y =posID>4?200:-100;
        GetComponent<RectTransform>().anchoredPosition3D =new Vector3(_x,_y,0);
    }
    public void MaskCard(bool ifmask)
    {
        mask.SetActive(ifmask);
    }
    void CheckIfNeedSelectCard()
    {
        Debug.Log("手牌数量："+Player.instance.playerActor.handCards.Count); 
        if(Player.instance.playerActor.handCards.Count ==0)
        {
            UIBattle.Instance.SelectCard();
        }
    }

}
