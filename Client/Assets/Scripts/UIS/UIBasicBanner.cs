using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
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


    public Text textMap;

    Transform T_up;
    Transform T_down;

    Text goldText;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        goldText =transform.Find("up/goldText").GetComponent<Text>();
        F_LevelUp =transform.Find("LevelUpFrame").gameObject;
        BTN_LevelUp =transform.Find("LevelUpFrame/Button").GetComponent<Button>();
        text_level =transform.Find("LevelUpFrame/LevleText").GetComponent<Text>();
        text_attack =transform.Find("LevelUpFrame/AttackText").GetComponent<Text>();
        text_defence =transform.Find("LevelUpFrame/DefenceText").GetComponent<Text>();

        F_LevelUp.SetActive(false);
        RegisterButton();
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeGoldText()
    {
        // goldText.DOText(Player.instance.Gold.ToString(),2f,true,ScrambleMode.None,null);
        goldText.text = Player.instance.Gold.ToString();
    }
    void RegisterButton()
    {
        T_BattleCharacter.onValueChanged.AddListener(isOn => OpenUIBattleCharacter(T_BattleCharacter.isOn));
        T_CardGroup.onValueChanged.AddListener(isOn => OpenUICardGroup(T_CardGroup.isOn));
        T_AbilityGroup.onValueChanged.AddListener(isOn => OpenUIAbilityGroup(T_AbilityGroup.isOn));
        T_Map.onValueChanged.AddListener(isOn => OpenUIMap(T_Map.isOn));
        T_Talent.onValueChanged.AddListener(isOn => OpenUITalent(T_Talent.isOn));
        BTN_LevelUp.onClick.AddListener(CloseLevelUpFrame);
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
        talent_new.SetActive(true);
        F_LevelUp.SetActive(true);
        text_level.text =string.Format("Lv{0}→Lv{1}",Player.instance.playerActor.level-1,Player.instance.playerActor.level);
        text_attack.text =string.Format("攻击力:{0}→{1}",Player.instance.playerActor.basicAttack-1,Player.instance.playerActor.basicAttack);
        text_defence.text =string.Format("防御力:{0}→{1}",Player.instance.playerActor.basicDefence-1,Player.instance.playerActor.basicDefence);

    }
    void CloseLevelUpFrame()
    {
        F_LevelUp.SetActive(false);
    }
}
