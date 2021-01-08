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
    List<ItemBox> choosenItemBox =new List<ItemBox>();
    public Button retryButton;
    int totalPrice=0;
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
        foreach (var item in skillItemBoxes)
        {
            item.toggle.onValueChanged.AddListener(isOn=>OnToggle(item));
        } 
        Refreash();
    }
    void Refreash()
    {
        AbilityData[] Adatas = AbilityManager.instance.GetRandomAbility(3,Player.instance.playerActor.abilities);
        SkillData[] Sdatas = SkillManager.instance.GetRandomSelfSkills(3);
        choosenItemBox.Clear();
        
        for (int i = 0; i < Adatas.Length; i++)
        {
            abilityItemBoxes[i].Reset();
            abilityItemBoxes[i].Init(Adatas[i]);
        }
        for (int i = 0; i < Sdatas.Length; i++)
        {
            skillItemBoxes[i].Reset();
            skillItemBoxes[i].Init(Sdatas[i]);
        }
        //随机1个能力打折
        RandomDiscountAbility(Random.Range(0,3));
        //随机1个技能卡打折
        RandomDiscountSkill(Random.Range(0,3));
        totalPrice =0;

    }
    void DiscountItem()
    {
        //根据角色拥有的折扣buff，降低所有物品的价格
    }
    void RandomDiscountAbility(int number)
    {   
        abilityItemBoxes[number].price =Mathf.FloorToInt(abilityItemBoxes[number].price)/2;      
        abilityItemBoxes[number].itemName.text = "半价："+abilityItemBoxes[number].price;
    }
    void RandomDiscountSkill(int number)
    {
        skillItemBoxes[number].price =Mathf.FloorToInt(skillItemBoxes[number].price)/2;       
        skillItemBoxes[number].itemName.text = "半价："+skillItemBoxes[number].price;
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
        if(item.toggle.isOn)
        {
            choosenItemBox.Add(item);
            totalPrice+=item.price;
        }
        else
        {
            choosenItemBox.Remove(item);
            totalPrice-=item.price;
        }
        // describe.text = item.describe;
        
        RefeashBuyButton();
    }
    void RefeashBuyButton()
    {
        sureButton.GetComponentInChildren<Text>().text =totalPrice.ToString();
        if(Player.instance.Gold>=totalPrice)
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
        foreach (var item in choosenItemBox)
        {
            item.Disable();
            Player.instance.Gold-=item.price;
            if(item.type ==1)
            Player.instance.playerActor.UsingSkillsID.Add(item.id);
            else if(item.type ==2)
            Player.instance.playerActor.abilities.Add(item.id);
        }
        for(int i =choosenItemBox.Count-1;i>=0 ;i--)
        {
            choosenItemBox[i].toggle.isOn =false;
        }
        totalPrice  = 0;
        //++++++++++++++++++++++++++++++++++++++++++++++++++++++++++金币UI显示变化
        UIBasicBanner.instance.ChangeGoldText();
        RefeashBuyButton();
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
        Refreash();
        totalPrice  = 0;

    }
}
