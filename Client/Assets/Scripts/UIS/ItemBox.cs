using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemBox : MonoBehaviour
{
    public Image icon;
    public Text itemName;
    public Text textChange;
    public Toggle toggle;
    public string describe;
    public int id;
    public int price;
    Transform Titem;
    public int type;
    public int contentType;
    GameObject skillMark;
    public Button button;
    public Text priceText;
    public bool inshop;
    public bool inReward;
    public Text skillText;
    void Awake()
    {
        toggle =GetComponent<Toggle>();
        Titem=transform.Find("Item");
        skillMark = transform.Find("SkillMark").gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Init()
    {
        
    }
    public void Init(SkillData item)
    {
        type =1;
        contentType =1;
        icon.gameObject.SetActive(false);
        SkillCard card= Instantiate((GameObject)Resources.Load("Prefabs/SkillCard")).GetComponent<SkillCard>();
        card.Init(item);
        skillText=card.textSkillDescribe;
        card.transform.SetParent(Titem);
        card.transform.localPosition =Vector3.zero;
        card.transform.localScale=Vector3.one;
        card.SwitchCard();
        
        // toggle.targetGraphic = card.GetComponent<Image>();
        // toggle.graphic = skillMark.GetComponent<Image>();

        // describe =item.describe;
        price =(item.rank+1)*Configs.instance.priceRankGold;
        itemName.text ="";
        id =item.id;
    }
    public void Init(AbilityData item)
    {
        // if(skillMark)
        // skillMark.SetActive(false);
        
        type =2;
        contentType =2;
        icon.sprite = Resources.Load<Sprite>("Texture/Ability/"+item.icon);
        itemName.text =item.name;
        textChange.text = item.describe;
        textChange.gameObject.SetActive(false);
        describe =item.describe;
        price =item.price;
        id =item.id;
    }
    public void Init(TraitData item)
    {
        icon.sprite = Resources.Load("Texture/Trait/"+item.icon,typeof(Sprite)) as Sprite;
        itemName.text =item.name;
    }
    public void Init(CharacterData item,int num)
    {
        icon.sprite = Resources.Load("Texture/Character/"+item.portrait,typeof(Sprite)) as Sprite;
        itemName.text =item.name;
        if(num>0)
        {
            textChange.text =string.Format("+{0}",num);
        }
        else
        {
            textChange.text =string.Format("{0}",num);
        }

    }
    public void Init(CharacterData item)
    {
        icon.sprite = Resources.Load("Texture/Character/"+item.portrait,typeof(Sprite)) as Sprite;
        itemName.text =item.name;
    }
    public void Init(GuildData item,int num)
    {
        icon.sprite = Resources.Load("Texture/Guild/"+item.icon,typeof(Sprite)) as Sprite;
        itemName.text =item.name;
        if(num>0)
        {
            textChange.text =string.Format("+{0}",num);
        }
        else
        {
            textChange.text =string.Format("{0}",num);
        }
    }
    public void Disable()
    {
        skillMark.SetActive(false);
        if(Titem.childCount>0)
        Destroy(Titem.GetChild(0).gameObject);
        icon.gameObject.SetActive(false);
        toggle.interactable =false;
        if(inshop)
        itemName.text ="已购买";
        else if(inReward)
        itemName.text ="";
        itemName.color = Color.white;
        button.gameObject.SetActive(false);
    }
    public void CantChoose()
    {
        skillMark.SetActive(true);
        toggle.interactable =false;
        button.gameObject.SetActive(false);
    }
    public void Reset()
    {
        // try
        // {
        //     toggle.onValueChanged.RemoveListener(OpenDetail);
        // }
        // catch (System.Exception)
        // {
            
        //     throw;
        // }
        if(Titem.childCount>0)
        Destroy(Titem.GetChild(0).gameObject);
        // itemName.color = Color.black;
        toggle.interactable =true;

        // if(toggle.graphic)
        // {
        //     toggle.isOn = false;
        //     Debug.LogWarning("正确的刷新");
        // }
        // else
        // {
        //     Debug.LogWarning("错误的刷新");

        //     skillMark.SetActive(true);
        // }

        icon.gameObject.SetActive(true);
        // toggle.targetGraphic =icon;
        price =0;
        id =0;
        type=0;
    }
    public void ShowDetail()
    {
        //显示优先级为最高
        //出现一个黑色遮罩
        //显示放大的itemBox
        //如果是卡牌，不显示文字描述
        //如果是能力，显示文字描述
        //点击黑色遮罩区域，会退出显示
        //词条会有额外说明
        
    }
    public void HideToggleSelect()
    {
        if(toggle.graphic)
        toggle.graphic.gameObject.SetActive(false);
        toggle.graphic =null;
        skillMark.SetActive(false);
        toggle.onValueChanged.RemoveAllListeners();
        toggle.onValueChanged.AddListener(isOn => OpenDetail(isOn));
        // itemName.gameObject.SetActive(false);
        textChange.gameObject.SetActive(false);
    }
    public void InShop()
    {
        inshop =true;
        if(toggle.graphic)
        toggle.graphic.gameObject.SetActive(false);
        toggle.graphic =null;
        skillMark.SetActive(false);
        toggle.onValueChanged.RemoveAllListeners();
        toggle.onValueChanged.AddListener(isOn => OpenDetail(isOn));
        itemName.gameObject.SetActive(true);
        textChange.gameObject.SetActive(false);
        button.gameObject.SetActive(true);
        priceText.gameObject.SetActive(true);
        priceText.text =string.Format("{0}",price);
        textChange.gameObject.SetActive(false);
    }
    public void InReward()
    {
        inReward =true;
        if(toggle.graphic)
        toggle.graphic.gameObject.SetActive(false);
        toggle.graphic =null;
        skillMark.SetActive(false);
        toggle.onValueChanged.RemoveAllListeners();
        toggle.onValueChanged.AddListener(isOn => OpenDetail(isOn));
        itemName.gameObject.SetActive(true);
        textChange.gameObject.SetActive(false);
        button.gameObject.SetActive(true);
        priceText.gameObject.SetActive(true);
        priceText.text =string.Format("选择");
        textChange.gameObject.SetActive(false);
    }
    void OpenDetail(bool isOn)
    {
        // Debug.LogWarning("点到了");
        UICardDetail.CreateUI().Init(this);
        if(type==2)
        {
            textChange.gameObject.SetActive(true);
        }
        button.gameObject.SetActive(false);
    }

}
