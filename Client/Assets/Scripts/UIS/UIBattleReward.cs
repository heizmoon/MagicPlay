using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIBattleReward : MonoBehaviour
{
    // 战斗结束后获取奖励的界面
    GameObject relicFrame;
    GameObject skillFrame;
    GameObject goldFrame;
    GameObject expFrame;
    Image expBar;
    int steps;//物品随机等级
    bool isRelic;
    Text goldText;
    Text expText;
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
        skillFrame = transform.Find("cardFrame").gameObject;
        goldFrame = transform.Find("goldFrame").gameObject;
        expFrame = transform.Find("expFrame").gameObject;
        goldText = transform.Find("goldFrame/goldText").gameObject.GetComponent<Text>();
        expText = transform.Find("expFrame/Text").gameObject.GetComponent<Text>();
        expBar = transform.Find("expFrame/back/bar").gameObject.GetComponent<Image>();
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
    public void Init(int steps,bool isRelic)
    {
        this.steps =steps;
        this.isRelic =isRelic;
        goldText.text =string.Format("{0}",steps*Configs.instance.battleLevelGold);
        if(!isRelic)
        {
            relicFrame.SetActive(false);
            
            // needChooseStep =1;
        }
        else
        {
            // needChooseStep =2;
            skillFrame.SetActive(false);
            goldFrame.SetActive(false);
            expFrame.SetActive(false);
        }
        foreach (var item in skillItemBoxes)
        {
            item.button.onClick.AddListener(delegate () {GetItem(item);});
        }
        // needChooseStep =1;
        foreach (var item in abilityItemBoxes)
        {
            item.button.onClick.AddListener(delegate () {GetItem(item);});
        }
        rewardCardRank =Configs.instance.GetCardRank(steps); 
        Refreash();
        ShowExpReward();
    }
    void Refreash()
    {
        // if(isBoss&&!hasChoosenRelic)
        // {
            
        // }
        if(Main.instance.ifNewBird <=6)
        {
            SkillData[] Sdatas =new SkillData[3]; 
            Sdatas[0]= SkillManager.instance.GetInfo(169);
            Sdatas[1]= SkillManager.instance.GetInfo(170);
            Sdatas[2]= SkillManager.instance.GetInfo(171);

            for (int i = 0; i < Sdatas.Length; i++)
            {
                skillItemBoxes[i].Reset();
                skillItemBoxes[i].Init(Sdatas[i]);
                skillItemBoxes[i].InReward();
            }
            Main.instance.ifNewBird++;
            Btn_retry.gameObject.SetActive(false);
            return;
        }
        if(Main.instance.ifNewBird ==13)
        {
            SkillData[] Sdatas =new SkillData[3]; 
            Sdatas[0]= SkillManager.instance.GetInfo(173);
            Sdatas[1]= SkillManager.instance.GetInfo(174);
            Sdatas[2]= SkillManager.instance.GetInfo(175);

            for (int i = 0; i < Sdatas.Length; i++)
            {
                skillItemBoxes[i].Reset();
                skillItemBoxes[i].Init(Sdatas[i]);
                skillItemBoxes[i].InReward();
            }
            Main.instance.ifNewBird++;
            Btn_retry.gameObject.SetActive(false);
            return;
        }
        if(Main.instance.ifNewBird ==14)
        {
            AbilityData[] Adatas =new AbilityData[3]; 
            Adatas[0]= AbilityManager.instance.GetInfo(23);
            Adatas[1]= AbilityManager.instance.GetInfo(26);
            Adatas[2]= AbilityManager.instance.GetInfo(28);

            for (int i = 0; i < Adatas.Length; i++)
            {
                abilityItemBoxes[i].Reset();
                abilityItemBoxes[i].Init(Adatas[i]);
                abilityItemBoxes[i].InReward();
            }
            Main.instance.ifNewBird++;
            Btn_retry.gameObject.SetActive(false);
            return;
        }

        if(!isRelic)
        {
            SkillData[] Sdatas = SkillManager.instance.GetRandomSelfSkillsLevelLimit(3,rewardCardRank);
            for (int i = 0; i < Sdatas.Length; i++)
            {
                skillItemBoxes[i].Reset();
                skillItemBoxes[i].Init(Sdatas[i]);
                skillItemBoxes[i].InReward();
            }
        }
        else
        {
            AbilityData[] Adatas = AbilityManager.instance.GetRandomAbility(3,rewardCardRank);
            for (int i = 0; i < Adatas.Length; i++)
            {
                abilityItemBoxes[i].Reset();
                abilityItemBoxes[i].Init(Adatas[i]);
                abilityItemBoxes[i].InReward();
            }
        }
        
        // if(!hasChoosenCard)
        // {
        //     
        // }
    }
    void ShowExpReward()
    {
        if(isRelic)
        {
            return;
        }
        expText.text = string.Format("等级:Lv{0}",Player.instance.playerActor.level);
        int startExp = BattleScene.instance.exp;
        int maxExp = CharacterManager.instance.GetLevelData(Player.instance.playerActor.level).exp;
        expBar.fillAmount = startExp*1f/maxExp;
        int addExp = Configs.instance.everyStepAddEXP;
        float endFill =0;
        if(startExp+addExp<maxExp)
        {
            endFill = (startExp+addExp)*1f/maxExp;
            expBar.DOFillAmount(endFill,0.6f);
        }
        else
        {
            int maxExp2=CharacterManager.instance.GetLevelData(Player.instance.playerActor.level+1).exp;
            endFill = (startExp+addExp-maxExp)*1f/maxExp2;
            Tweener  tweener = expBar.DOFillAmount(1,0.4f);
            tweener.onComplete =delegate() 
            {
                expBar.fillAmount = 0;
			    expBar.DOFillAmount(endFill,0.3f);
                expText.text = string.Format("等级:<color=green>Lv{0}</color>",Player.instance.playerActor.level);
            };

        }
        BattleScene.instance.exp+= Configs.instance.everyStepAddEXP;
        if(BattleScene.instance.exp>=CharacterManager.instance.GetLevelData(Player.instance.playerActor.level).exp)
        {
            BattleScene.instance.ifLevelUp = true;
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
            AbilityData ability = AbilityManager.instance.GetInfo(item.id);
            Player.instance.playerActor.basicAttack+=ability.attack;
            Player.instance.playerActor.basicDefence+=ability.defence;
            Player.instance.playerActor.HpMax+=ability.hpMax;
            Player.instance.playerActor.MpMax+=ability.mpMax;
            Player.instance.playerActor.Crit+=ability.crit;
            float reMp =ability.reMp/5;
            int temp = (int)(reMp*100);
            reMp =temp/100f;
            Player.instance.playerActor.autoReduceMPAmount+= reMp;

            for (int i = 0; i < abilityItemBoxes.Count; i++)
            {
                abilityItemBoxes[i].Disable();
            }
        }
        // chooseStep++;
        // if(chooseStep==needChooseStep)
        // {
        //     OnButtonReturn();
        // }
        OnButtonReturn();

    }
    void OnButtonReturn()
    {
        gameObject.SetActive(false);
        Player.instance.AddGold(steps*Configs.instance.battleLevelGold);
        // UIBasicBanner.instance.ChangeGoldText();
        // BattleScene.instance.OpenMap();
        // if(UIBattle.Instance)
        // UIBattle.Instance.OnBattleGoOn();
        if(isRelic)
        UIBasicBanner.instance.F_LevelUp.SetActive(false);
        Destroy(gameObject);
    }
    void OnRetry()
    {
        //播放广告，重置货品
        Refreash();
    }
}
