using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemBox : MonoBehaviour
{
    public Image icon;
    public Text itemName;
    public Text textChange;
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
    public void Init(Ability item)
    {
        icon.sprite = Resources.Load("Texture/Skills/"+item.icon,typeof(Sprite)) as Sprite;
        itemName.text =item.name;
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
    
}
