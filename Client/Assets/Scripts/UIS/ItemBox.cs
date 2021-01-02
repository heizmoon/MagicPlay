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
    GameObject skillMark;
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
    public void Init(AssetsItem item)
    {
        icon.sprite = Resources.Load("Texture/Assets/"+item._icon,typeof(Sprite)) as Sprite;
        itemName.text =item._name;
    }
    public void Init(SkillData item)
    {
        type =1;
        icon.gameObject.SetActive(false);
        SkillCard card= Instantiate((GameObject)Resources.Load("Prefabs/SkillCard")).GetComponent<SkillCard>();
        card.Init(item);
        card.transform.SetParent(Titem);
        card.transform.localPosition =Vector3.zero;
        card.transform.localScale=Vector3.one;
        
        toggle.targetGraphic = card.GetComponent<Image>();
        toggle.graphic = skillMark.GetComponent<Image>();

        // describe =item.describe;
        price =(item.rank+1)*25;
        itemName.text =price.ToString();
        id =item.id;
    }
    public void Init(AbilityData item)
    {
        skillMark.SetActive(false);
        
        type =2;
        icon.sprite = Resources.Load<Sprite>("Texture/Ability/"+item.icon);
        itemName.text =item.price.ToString();
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
        itemName.text ="已购买";
        itemName.color = Color.white;
    }
    public void Reset()
    {
        if(Titem.childCount>0)
        Destroy(Titem.GetChild(0).gameObject);
        skillMark.SetActive(true);
        itemName.color = Color.black;
        toggle.interactable =true;
        toggle.isOn = false;
        icon.gameObject.SetActive(true);
        toggle.targetGraphic =icon;
        price =0;
        id =0;
        type=0;
    }
}
