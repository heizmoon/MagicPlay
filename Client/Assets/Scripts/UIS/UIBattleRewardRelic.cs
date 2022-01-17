using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

///<summary>仅负责获得获得遗物，不负责换牌</summary>
public class UIBattleRewardRelic : MonoBehaviour
{
    // 战斗结束后获取奖励的界面
    GameObject relicFrame;
    // GameObject skillFrame;
    
    int steps;//物品随机等级
    ///<summary>0=仅获得遗物，1=获得经验与金钱，如果升级，则获得遗物，2=获得经验与金钱，且获得遗物，如果升级，则再次获得遗物</summary>
    int type;
   
    public List<ItemBox> abilityItemBoxes;
    // public List<ItemBox> skillItemBoxes;
    Button Btn_return;
    Button Btn_retry;
    // bool hasChoosenCard;
    // bool hasChoosenRelic;
    int chooseStep =0;
    int needChooseStep =0;
    int rewardCardRank =0;

    void Awake()
    {
        relicFrame = transform.Find("RelicFrame").gameObject;
        // skillFrame = transform.Find("cardFrame").gameObject;
        // goldFrame = transform.Find("goldFrame").gameObject;
        // expFrame = transform.Find("expFrame").gameObject;
        // goldText = transform.Find("goldFrame/goldText").gameObject.GetComponent<Text>();
        // expText = transform.Find("expFrame/Text").gameObject.GetComponent<Text>();
        // expBar = transform.Find("expFrame/back/bar").gameObject.GetComponent<Image>();
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
    public void Init(int steps,int type)
    {
        this.steps =steps;
        this.type = type;

        if(this.type==0)
        {
            //仅获得遗物，适用于事件三选一
        }
        else if(this.type==1)
        {
            //获得金钱和经验值，如果升级，则获得遗物，适用与普通战斗
            needChooseStep =1;
        }
        else if(this.type==2)
        {
            //获得金钱，经验值，遗物，如果升级，则再次获得遗物，适用于BOSS战
            needChooseStep =2;
        }
        // 
        // if(!Configs.instance.ifChangMode)
        // {
        //     if(type==0)
        //     {
        //         relicFrame.SetActive(false);
        //         Player.instance.AddGold(steps * Configs.instance.battleLevelGold);
        //     }
        //     else
        //     {
        //         skillFrame.SetActive(false);
        //         goldFrame.SetActive(false);
        //         expFrame.SetActive(false);
        //     }
        //     if(type!=2)
        //     {
        //         needChooseStep =1;
        //     }
        //     else
        //     {
        //         needChooseStep =2;
        //     }
        // }
        // else//换牌模式
        // {   
        // }
        // foreach (var item in skillItemBoxes)
        // {
        //     item.button.onClick.AddListener(delegate () {GetItem(item);});
        // }
        // needChooseStep =1;
        foreach (var item in abilityItemBoxes)
        {
            item.button.onClick.AddListener(delegate () {GetItem(item);});
        }
        rewardCardRank =Configs.instance.GetCardRank(steps); 
        Refreash();

       
    }
    void Refreash()
    {
        AbilityData[] Adatas;
        if(Main.instance.ifNewBird==15)//创造固定奖励
        {
            Adatas =new AbilityData[3]; 
            Adatas[0]= AbilityManager.instance.GetInfo(23);
            Adatas[1]= AbilityManager.instance.GetInfo(26);
            Adatas[2]= AbilityManager.instance.GetInfo(28);

            for (int i = 0; i < Adatas.Length; i++)
            {
                abilityItemBoxes[i].Reset();
                abilityItemBoxes[i].Init(Adatas[i]);
                abilityItemBoxes[i].InReward();
            }
            Main.instance.ifNewBird=16;
            Btn_retry.gameObject.SetActive(false);
            Btn_return.gameObject.SetActive(false);
            return;
        }
        Adatas = AbilityManager.instance.GetRandomAbility(3,rewardCardRank);
        for (int i = 0; i < Adatas.Length; i++)
        {
            abilityItemBoxes[i].Reset();
            abilityItemBoxes[i].Init(Adatas[i]);
            abilityItemBoxes[i].InReward();
        }
        // if(!Configs.instance.ifChangMode)
        // {
        //     if(type==0)
        //     {
        //         SkillData[] Sdatas = SkillManager.instance.GetRandomSelfSkillsLevelLimit(3,rewardCardRank);
        //         for (int i = 0; i < Sdatas.Length; i++)
        //         {
        //             skillItemBoxes[i].Reset();
        //             skillItemBoxes[i].Init(Sdatas[i]);
        //             skillItemBoxes[i].InReward();
        //         }
        //     }
        //     else if(type ==2&&chooseStep==0)
        //     {
        //         SkillData[] Sdatas = SkillManager.instance.GetRandomSelfSkillsLevelLimit(3,rewardCardRank);
        //         for (int i = 0; i < Sdatas.Length; i++)
        //         {
        //             skillItemBoxes[i].Reset();
        //             skillItemBoxes[i].Init(Sdatas[i]);
        //             skillItemBoxes[i].InReward();
        //         }
        //     }
        //     else
        //     {
        //         AbilityData[] Adatas = AbilityManager.instance.GetRandomAbility(3,rewardCardRank);
        //         for (int i = 0; i < Adatas.Length; i++)
        //         {
        //             abilityItemBoxes[i].Reset();
        //             abilityItemBoxes[i].Init(Adatas[i]);
        //             abilityItemBoxes[i].InReward();
        //         }
        //     }
        // }
    }
    
    void GetItem(ItemBox item)
    {
        // if(!Configs.instance.ifChangMode)
        // {
        //     if(item.type ==1)
        //     {
        //         // hasChoosenCard =true;
        //         Player.instance.playerActor.UsingSkillsID.Add(item.id);
        //         for (int i = 0; i < skillItemBoxes.Count; i++)
        //         {
        //             skillItemBoxes[i].Disable();
        //         }
        //     }
        //     else if(item.type ==2)
        //     {
        //         // hasChoosenRelic =true;
        //         Player.instance.playerActor.abilities.Add(item.id);
        //         AbilityManager.instance.EquipRelic(item.id);
        //         for (int i = 0; i < abilityItemBoxes.Count; i++)
        //         {
        //             abilityItemBoxes[i].Disable();
        //         }
        //     }
        //     chooseStep++;
        //     if(chooseStep==needChooseStep)
        //     {
        //         OnButtonReturn();
        //     }
        //     else
        //     {
        //         Refreash();
        //     }
        // }
        
        Player.instance.playerActor.abilities.Add(item.id);
        AbilityManager.instance.EquipRelic(item.id);
        for (int i = 0; i < abilityItemBoxes.Count; i++)
        {
            abilityItemBoxes[i].Disable();
        }
        OnButtonReturn();
    }
    void OnButtonReturn()
    {
        // UIBasicBanner.instance.ChangeGoldText();
        // BattleScene.instance.OpenMap();
        if(UIBattle.Instance)
        UIBattle.Instance.OnBattleGoOn();
        if(type>0)
        UIBattleEXP.CreateUIBattleEXP(type);
        StartCoroutine(WaitForDestory());

    }
    void OnRetry()
    {
        //播放广告，重置货品
        Refreash();
    }
    IEnumerator WaitForDestory()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
    public static void CreateUIBattleRewardRelic(int type)
    {
        GameObject go =(GameObject)Instantiate(Resources.Load("Prefabs/UIBattleRewardRelic"));
        go.transform.SetParent(Main.instance.allScreenUI);
        go.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        go.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
        go.transform.localScale =Vector3.one;
        go.GetComponent<UIBattleRewardRelic>().Init(BattleScene.instance.steps,type);
    }
}
