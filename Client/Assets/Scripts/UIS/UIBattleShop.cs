using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBattleShop : MonoBehaviour
{
    public Text describe;
    public List<ItemBox> abilityItemBoxes;
    public List<ItemBox> skillItemBoxes;

    public Button sureButton;
    public Button cannelButton;
    ItemBox choosenItemBox;
    public Button retryButton;
    private void Start() 
    {
        Init();
    }
    public void Init()
    {
        sureButton.onClick.AddListener(OnButtonBuy);
        cannelButton.onClick.AddListener(OnButtonReturn);
        retryButton.onClick.AddListener(OnRetry);
        sureButton.interactable =false;   
        foreach (var item in abilityItemBoxes)
        {
            item.toggle.onValueChanged.AddListener(isOn=>OnToggle(item));
        } 
    }
    void Refreash()
    {
        AbilityData[] Adatas = AbilityManager.instance.GetRandomAbility(3,Player.instance.playerActor.abilities);
        SkillData[] Sdatas = SkillManager.instance.GetRandomSkills(3);
        for (int i = 0; i < Adatas.Length; i++)
        {
            abilityItemBoxes[i].Init(Adatas[i]);
        }
        for (int i = 0; i < Sdatas.Length; i++)
        {
            skillItemBoxes[i].Init(Sdatas[i]);
        }
        //随机1个能力打折
        RandomDiscountAbility(Random.Range(0,3));
        //随机1个技能卡打折
        RandomDiscountSkill(Random.Range(0,3));

    }
    void DiscountItem()
    {
        //根据角色拥有的折扣buff，降低所有物品的价格
    }
    void RandomDiscountAbility(int number)
    {        
        abilityItemBoxes[number].itemName.text = "半价："+Mathf.FloorToInt(abilityItemBoxes[number].price)/2;
    }
    void RandomDiscountSkill(int number)
    {        
        skillItemBoxes[number].itemName.text = "半价："+Mathf.FloorToInt(skillItemBoxes[number].price)/2;
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
    public void RandomUpdateSkill(int id)
    {
        if(id == 0)
        {
            //显示为空
            return;
        }
        skillItemBoxes[2].Init(SkillManager.instance.GetInfo(id));
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
    void OnRetry()
    {
        //播放广告，重置货品
    }
}
