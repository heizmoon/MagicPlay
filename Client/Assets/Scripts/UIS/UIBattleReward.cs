using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIBattleReward : MonoBehaviour
{
    // 战斗结束后获取奖励的界面
    GameObject relicFrame;
    int steps;//物品随机等级
    bool isBoss;
    Text goldText;
    public List<ItemBox> abilityItemBoxes;
    public List<ItemBox> skillItemBoxes;
    Button Btn_return;
    Button Btn_retry;
    bool hasChoosenCard;
    bool hasChoosenRelic;
    int chooseStep =0;
    int needChooseStep =0;
    int rewardCardRank =0;

    void Awake()
    {
        relicFrame = transform.Find("RelicFrame").gameObject;
        goldText = transform.Find("goldText").gameObject.GetComponent<Text>();
        Btn_retry = transform.Find("ButtonRetry").gameObject.GetComponent<Button>();
        Btn_return = transform.Find("ButtonReturn").gameObject.GetComponent<Button>();

        Btn_retry.onClick.AddListener(OnRetry);
        Btn_return.onClick.AddListener(OnButtonReturn);
    }
    void Start()
    {
        transform.DOPunchScale(new Vector3(0.2f,0.2f,0.2f),0.5f,2,1);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void Init(int steps,bool isBoss)
    {
        this.steps =steps;
        this.isBoss =isBoss;
        goldText.text =string.Format("{0}",steps*Configs.instance.battleLevelGold);
        if(!isBoss)
        {
            relicFrame.SetActive(false);
            needChooseStep =1;
        }
        if(isBoss)
        {
            needChooseStep =2;
            foreach (var item in abilityItemBoxes)
            {
                item.button.onClick.AddListener(delegate () {GetItem(item);});
            } 
        }
        foreach (var item in skillItemBoxes)
        {
            item.button.onClick.AddListener(delegate () {GetItem(item);});
        }
        rewardCardRank =Configs.instance.GetCardRank(steps); 
        Refreash();
    }
    void Refreash()
    {
        if(isBoss&&!hasChoosenRelic)
        {
            AbilityData[] Adatas = AbilityManager.instance.GetRandomAbilityFromLevel(3,0);
            for (int i = 0; i < Adatas.Length; i++)
            {
                abilityItemBoxes[i].Reset();
                abilityItemBoxes[i].Init(Adatas[i]);
                abilityItemBoxes[i].InReward();
            }
        }
        if(!hasChoosenCard)
        {
            SkillData[] Sdatas = SkillManager.instance.GetRandomSelfSkillsLevelLimit(3,rewardCardRank);
            for (int i = 0; i < Sdatas.Length; i++)
            {
                skillItemBoxes[i].Reset();
                skillItemBoxes[i].Init(Sdatas[i]);
                skillItemBoxes[i].InReward();
            }
        }
    }
    void GetItem(ItemBox item)
    {
        if(item.type ==1)
        {
            hasChoosenCard =true;
            Player.instance.playerActor.UsingSkillsID.Add(item.id);
            for (int i = 0; i < skillItemBoxes.Count; i++)
            {
                skillItemBoxes[i].Disable();
            }
        }
        else if(item.type ==2)
        {
            hasChoosenRelic =true;
            Player.instance.playerActor.abilities.Add(item.id);
            for (int i = 0; i < abilityItemBoxes.Count; i++)
            {
                abilityItemBoxes[i].Disable();
            }
        }
        chooseStep++;
        if(chooseStep==needChooseStep)
        {
            OnButtonReturn();
        }
    }
    void OnButtonReturn()
    {
        gameObject.SetActive(false);
        Player.instance.AddGold(steps*Configs.instance.battleLevelGold);
        // UIBasicBanner.instance.ChangeGoldText();
        // BattleScene.instance.OpenMap();
        UIBattle.Instance.OnBattleGoOn();
        Destroy(gameObject);
    }
    void OnRetry()
    {
        //播放广告，重置货品
        Refreash();
    }
}
