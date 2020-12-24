using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBattleShop : MonoBehaviour
{
    public Text describe;
    public List<ItemBox> itemBoxes;
    public Button sureButton;
    public Button cannelButton;
    ItemBox choosenItemBox;
    public void Init(AbilityData[] datas)
    {
        foreach (var item in itemBoxes)
        {
            item.toggle.onValueChanged.AddListener(isOn=>OnToggle(item));
        }
        for (int i = 0; i < datas.Length; i++)
        {
            itemBoxes[i].Init(datas[i]);
        }
        //随机1个能力打折
        RandomDiscountAbility(Random.Range(0,4));
        //1个随机技能
        
        //1个技能升级
        // ShowUpdateSkill(RandomUpdateSkill());
        sureButton.onClick.AddListener(OnButtonBuy);
        cannelButton.onClick.AddListener(OnButtonReturn);
        sureButton.interactable =false;
    }

    void RandomDiscountAbility(int number)
    {        
        itemBoxes[number].itemName.text = "半价："+Mathf.FloorToInt(itemBoxes[number].price)/2;
    }
    //决定升级哪个技能
    int RandomUpdateSkill()
    {
        //首先获取角色有哪些技能还未升级
        List<Skill> skills =new List<Skill>();
        foreach (var item in Player.instance.playerActor.skills)
        {
            if(item.updateID!=-1)
            {
                //说明这个技能可以升级
                skills.Add(item);
            }
        }
        if(skills.Count<1)
        {
            return 0;
        }
        int r = Random.Range(0,skills.Count);
        r =skills[r].updateID;
        return r;
    }
    public void ShowUpdateSkill(int id)
    {
        if(id == 0)
        {
            //显示为空
            return;
        }
        itemBoxes[5].Init(SkillManager.instance.GetInfo(id));
    }
    public void OnToggle(ItemBox item)
    {
        if(!item.toggle.isOn)
        {
            return;
        }
        describe.text = item.describe;
        choosenItemBox =item;
        RefeashBuyButton(item.price);
    }
    void RefeashBuyButton(int price)
    {
        if(Player.instance.Gold>=price)
        {
            sureButton.interactable =true;
        }
        else
        {
            sureButton.interactable =false;
        }
    }
    void OnButtonBuy()
    {
        choosenItemBox.Disable();
        Player.instance.Gold-=choosenItemBox.price;
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++金币UI显示变化
        Player.instance.playerActor.abilities.Add(choosenItemBox.id);
        sureButton.interactable =false;
    }
    void OnButtonReturn()
    {
        gameObject.SetActive(false);
        Debug.Log("关闭商店");
        BattleScene.instance.OpenMap();
        Destroy(gameObject);
    }
}
