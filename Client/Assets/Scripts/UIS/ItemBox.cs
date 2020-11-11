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
    void Awake()
    {
        toggle =GetComponent<Toggle>();
    }
    void Start()
    {
        
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
        icon.sprite = Resources.Load("Texture/Skills/"+item.icon,typeof(Sprite)) as Sprite;
        itemName.text ="500";
        describe =item.describe;
        price =500;
        id =item.id;
    }
    public void Init(AbilityData item)
    {
        icon.sprite = Resources.Load("Texture/Ability/"+item.icon,typeof(Sprite)) as Sprite;
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
        icon.color =Color.gray;
        toggle.interactable =false;
        itemName.text ="已购买";
        itemName.color = Color.white;
    }
}
