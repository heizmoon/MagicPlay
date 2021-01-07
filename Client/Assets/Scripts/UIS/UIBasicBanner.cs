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

    Text goldText;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        goldText =transform.Find("up/goldText").GetComponent<Text>();
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
                g.transform.SetParent(Main.instance.middleUI);
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
                g.transform.SetParent(Main.instance.middleUI);
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
}
