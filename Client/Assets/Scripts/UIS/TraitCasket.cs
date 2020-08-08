using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TraitCasket : MonoBehaviour
{
    public Image background;
    public Image icon;
    public GameObject mask;
    public Text nameText;
    public Text describeText;
    public Text getMethodText;
    TraitData data;
    public void Init(TraitData data,bool ifHas)
    {
        // data = TraitManager.instance.GetInfo(id);
        this.data =data;
        //是否默认显示信息
        if(!data.defaultShow)
        {
            //如果不是默认显示信息
            //判断玩家是否已经拥有此特质，没有的话不显示特质具体信息
            if(!ifHas)
            {
                mask.SetActive(true);
                nameText.text ="???";
                describeText.text ="??????";
                getMethodText.text ="";
                return;
            }
        }
        nameText.text =data.name;
        describeText.text =data.describe;
        icon.sprite = Resources.Load("Texture/Trait/"+data.icon,typeof(Sprite)) as Sprite;
        if(data.showMethod)
        {
            getMethodText.text =data.getDescribe;
        }
        else
        {
            getMethodText.text ="";
        }
        if(ifHas)
        {
            mask.SetActive(false);
            icon.color =Color.white;
        }
        else
        {
            mask.SetActive(true);
        }
    }
    void ModifierColor(bool ifHas)
    {
        if(!ifHas)
        {
            nameText.color =Color.white;
            describeText.color =Color.white;
            getMethodText.color =Color.white;
            background.color =Color.grey;
        }
        else
        {
            nameText.color =Color.black;
            describeText.color =Color.black;
            getMethodText.color =Color.black;
            background.color =Color.white; 
        }
    }

}
