using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBasicBanner : MonoBehaviour
{
    public static UIBasicBanner instance;
    public Toggle T_BattleCharacter;
    public Toggle T_CardGroup;
    public Toggle T_AbilityGroup;
    public Toggle T_Map;
    public Toggle T_Talent;
    public GameObject talent_new;
    public GameObject F_LevelUp;
    Button BTN_LevelUp;
    Text text_level;
    Text text_attack;
    Text text_defence;
    Text text_HPmax;
    GameObject levleTextObjects;

    public Text textMap;

    Transform T_up;
    Transform T_down;

    Text goldText;
    Text hpText;
    Text expText;
    Image expBar;


    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        goldText =transform.Find("up/goldText").GetComponent<Text>();
        hpText =transform.Find("up/hpText").GetComponent<Text>();
        expText =transform.Find("up/exp/expText").GetComponent<Text>();
        expBar =transform.Find("up/exp/expBar/bar").GetComponent<Image>();

        F_LevelUp =transform.Find("LevelUpFrame").gameObject;
        BTN_LevelUp =transform.Find("LevelUpFrame/Texts/Button").GetComponent<Button>();
        levleTextObjects =transform.Find("LevelUpFrame/Texts").gameObject;
        text_level =transform.Find("LevelUpFrame/Texts/LevleText").GetComponent<Text>();
        text_attack =transform.Find("LevelUpFrame/Texts/AttackText").GetComponent<Text>();
        text_defence =transform.Find("LevelUpFrame/Texts/DefenceText").GetComponent<Text>();
        text_HPmax =transform.Find("LevelUpFrame/Texts/TalentText").GetComponent<Text>();

        F_LevelUp.SetActive(false);
        RegisterButton();
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
    public void RefeashText()
    {
        // goldText.DOText(Player.instance.Gold.ToString(),2f,true,ScrambleMode.None,null);
        goldText.text = Player.instance.Gold.ToString();

        if(Player.instance.playerActor==null)
        {
            return;
        }
        hpText.text = string.Format("{0}/{1}",Player.instance.playerActor.HpCurrent,Player.instance.playerActor.HpMax);
        expText.text ="LV"+ Player.instance.playerActor.level.ToString();
        expBar.fillAmount = BattleScene.instance.exp*1f/CharacterManager.instance.GetLevelData(Player.instance.playerActor.level).exp*1f;

    }
    void RegisterButton()
    {
        T_BattleCharacter.onValueChanged.AddListener(isOn => OpenUIBattleCharacter(T_BattleCharacter.isOn));
        T_CardGroup.onValueChanged.AddListener(isOn => OpenUICardGroup(T_CardGroup.isOn));
        T_AbilityGroup.onValueChanged.AddListener(isOn => OpenUIAbilityGroup(T_AbilityGroup.isOn));
        T_Map.onValueChanged.AddListener(isOn => OpenUIMap(T_Map.isOn));
        T_Talent.onValueChanged.AddListener(isOn => OpenUITalent(T_Talent.isOn));
        BTN_LevelUp.onClick.AddListener(GoOn);
    }
    void OpenUICardGroup(bool IsOn)
    {
        if(IsOn)
        {
            if(UICardGroup.instance)
            {
                UICardGroup.instance.gameObject.SetActive(true);
                UICardGroup.instance.Refeash();
            }
            else
            {
                GameObject g =(GameObject)Instantiate(Resources.Load("Prefabs/UICardGroup"));
                g.transform.SetParent(Main.instance.TopUI);
                g.transform.localPosition =Vector3.zero;
                g.transform.localScale = Vector3.one;
                g.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
                g.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
                UICardGroup.instance.Refeash();

            }
        }
        else
        {
            if(UICardGroup.instance)
            {
                UICardGroup.instance.gameObject.SetActive(false);

            }
        }
    }
    void OpenUIBattleCharacter(bool IsOn)
    {
        if(IsOn)
        {
            if(UIBattleCharacter.instance)
            {
                UIBattleCharacter.instance.gameObject.SetActive(true);
                UIBattleCharacter.instance.Init();
            }
            else
            {
                GameObject g =(GameObject)Instantiate(Resources.Load("Prefabs/UIBattleCharacter"));
                g.transform.SetParent(Main.instance.TopUI);
                g.transform.localPosition =Vector3.zero;
                g.transform.localScale = Vector3.one;
                g.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
                g.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
                UIBattleCharacter.instance.Init();

            }
        }
        else
        {
            if(UIBattleCharacter.instance)
            {
                Player.instance.playerActor.transform.SetParent(Main.instance.BottomUI);
                Player.instance.playerActor.transform.localPosition =Vector3.zero;
                Player.instance.playerActor.transform.localScale =Vector3.one;
                UIBattleCharacter.instance.gameObject.SetActive(false);
            }
        }
    }
    void OpenUIAbilityGroup(bool IsOn)
    {
        if(IsOn)
        {
            if(UIAbilityGroup.instance)
            {
                UIAbilityGroup.instance.gameObject.SetActive(true);
                UIAbilityGroup.instance.Refeash();
            }
            else
            {
                GameObject g =(GameObject)Instantiate(Resources.Load("Prefabs/UIAbilityGroup"));
                g.transform.SetParent(Main.instance.TopUI);
                g.transform.localPosition =Vector3.zero;
                g.transform.localScale = Vector3.one;
                g.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
                g.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
                UIAbilityGroup.instance.Refeash();

            }
        }
        else
        {
            if(UIAbilityGroup.instance)
            {
                UIAbilityGroup.instance.gameObject.SetActive(false);

            }
        }
    }
    void OpenUIMap(bool IsOn)
    {
        if(IsOn)
        {
            Map.instance.gameObject.SetActive(true);
        }
        else
        {
            Map.instance.gameObject.SetActive(false);
        }
    }
    void OpenUITalent(bool IsOn)
    {
        if(IsOn)
        {
            talent_new.SetActive(false);    
        }
        else
        {
            
        }
    }
    public void ShowNewTalent()
    {
        Debug.Log("升级了");
        // talent_new.SetActive(true);
        F_LevelUp.SetActive(true);
        levleTextObjects.SetActive(true);
        text_level.text =string.Format("Lv{0}→Lv{1}",Player.instance.playerActor.level-1,Player.instance.playerActor.level);
        int addAttack =CharacterManager.instance.GetLevelData(Player.instance.playerActor.level-1).addAttack;
        if(addAttack>0)
        {
            text_attack.text =string.Format("攻击力:{0}→{1}",Player.instance.playerActor.basicAttack-1,Player.instance.playerActor.basicAttack);
        }
        else
        text_attack.text ="";
        int addDefence =CharacterManager.instance.GetLevelData(Player.instance.playerActor.level-1).addDefence;
        if(addDefence>0)
        {
            text_defence.text =string.Format("防御力:{0}→{1}",Player.instance.playerActor.basicDefence-addDefence,Player.instance.playerActor.basicDefence);
        }
        else
        text_defence.text ="";
        int addHPMax =CharacterManager.instance.GetLevelData(Player.instance.playerActor.level-1).addHPMax;
        if(addHPMax>0)
        {
            text_HPmax.text =string.Format("最大生命值{0}→{1}",Player.instance.playerActor.HpMax-addHPMax,Player.instance.playerActor.HpMax);
        }
        else
        text_HPmax.text ="";
    }
    void GoOn()
    {
        // F_LevelUp.SetActive(false);
        levleTextObjects.SetActive(false);
        // UIBattleReward.CreateUIBattleReward(1);
    }
}
