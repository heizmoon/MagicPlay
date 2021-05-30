using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIBattleShop : MonoBehaviour
{
    public Text describe;
    public List<ItemBox> abilityItemBoxes;
    public List<ItemBox> skillItemBoxes;

    public Button sureButton;
    public Button cannelButton;
    public GameObject _playerCards;
    List<ItemBox> choosenItemBox =new List<ItemBox>();
    public Button retryButton;
    int totalPrice=0;
    ShopData shopData;
    List<int> _sellCardList;
    List<int> _sellRelicList;
    List<int> cardList = new List<int>();
    public Transform content;
    List<Button> buttons =new List<Button>();
    int chooseID;
    public Button buttonRemove;
    private void Start() 
    {
        // Init();
    }
    public void Init(int realID)
    {
        sureButton.onClick.AddListener(OnButtonBuy);
        cannelButton.onClick.AddListener(OnButtonReturn);
        retryButton.onClick.AddListener(OnRetry);
        sureButton.interactable =false;   
        foreach (var item in abilityItemBoxes)
        {
            // item.toggle.onValueChanged.AddListener(isOn=>OnToggle(item));
            item.button.onClick.AddListener(delegate () {BuyItem(item);});
        } 
        foreach (var item in skillItemBoxes)
        {
            // item.toggle.onValueChanged.AddListener(isOn=>OnToggle(item));
            item.button.onClick.AddListener(delegate () {BuyItem(item);});
        } 
        shopData = ShopManager.instance.GetInfo(realID);
        switch (Player.instance.CharID)
        {
            case 1:
                _sellCardList =shopData._sellCardList;
                _sellRelicList =shopData._sellRelicList;
            break;
            case 2:
                _sellRelicList =shopData._sellRelicList_robort;
                _sellCardList =shopData._sellCardList_robort;
            break;
            case 3:
                _sellRelicList =shopData._sellRelicList_mage;
                _sellCardList =shopData._sellCardList_mage;
            break;
        }

        Refreash();
    }
    
    void Refreash()
    {
        AbilityData[] Adatas = AbilityManager.instance.GetRandomAbilityFromSpecialPool(3,_sellRelicList);

        SkillData[] Sdatas = SkillManager.instance.GetRandomSkillFromSpecialPool(3,_sellCardList);

        // choosenItemBox.Clear();
        
        for (int i = 0; i < Adatas.Length; i++)
        {
            abilityItemBoxes[i].Reset();
            abilityItemBoxes[i].Init(Adatas[i]);
            abilityItemBoxes[i].InShop();

        }
        for (int i = 0; i < Sdatas.Length; i++)
        {
            skillItemBoxes[i].Reset();
            skillItemBoxes[i].Init(Sdatas[i]);
            skillItemBoxes[i].InShop();
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
        abilityItemBoxes[number].priceText.text = "半价："+abilityItemBoxes[number].price;
    }
    void RandomDiscountSkill(int number)
    {
        skillItemBoxes[number].price =Mathf.FloorToInt(skillItemBoxes[number].price)/2;       
        skillItemBoxes[number].priceText.text = "半价："+skillItemBoxes[number].price;
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
            Player.instance.AddGold(-item.price);
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
        UIBasicBanner.instance.RefeashText();
        RefeashBuyButton();
    }
    void ShowCards()
    {
        _playerCards.SetActive(true);
        cardList = Player.instance.playerActor.UsingSkillsID;
        SortList();
    }
    void SortList()
    {
        cardList.Sort((x,y)=>x.CompareTo(y));
        CreateCards();
        StartCoroutine(WaitForDisableGridLayout());
    }
    IEnumerator WaitForDisableGridLayout()
    {
        content.GetComponent<GridLayoutGroup>().enabled = true;
        yield return new WaitForSeconds(0.2f);
        content.GetComponent<GridLayoutGroup>().enabled = false;
        // RefeashPrice();
    }
    void CreateCards()
    {
        foreach (var item in cardList)
        {
            ItemBox itemBox = ((GameObject)Instantiate(Resources.Load("Prefabs/itemBox"))).GetComponent<ItemBox>();
            itemBox.transform.SetParent(content);
            itemBox.transform.localScale = Vector3.one;
            itemBox.Init(SkillManager.instance.GetInfo(item));
            itemBox.HideToggleSelect();
        }
        content.GetComponent<RectTransform>().sizeDelta =new Vector2(0,285*((int)(cardList.Count/3)+1));
        AddButton();
    }
    void AddButton()
    {
        ItemBox[] itemBoxes = content.GetComponentsInChildren<ItemBox>();
        foreach (var item in itemBoxes)
        {
            Button button =Instantiate(buttonRemove);
            button.transform.SetParent(item.transform);
            button.transform.localPosition =new Vector3(0,-140,0);
            button.transform.localScale =Vector3.one;
            button.onClick.AddListener(delegate(){RemoveCard(button);});
            buttons.Add(button);
        }
    }
    void RemoveCard(Button button)
    {
        // if(Player.instance.Gold<price)
        // {
        //     Main.instance.ShowNotEnoughGoldTip();
        //     return;//钱不够
        // }

        ItemBox itemBox =button.GetComponentInParent<ItemBox>();
        Player.instance.playerActor.UsingSkillsID.Remove(itemBox.id);
        // Player.instance.playerActor.UsingSkillsID.Add(chooseID);
        //播放动画，两张牌位置互换。然后选择框中的牌悉数消失
        _playerCards.SetActive(false);
        ClearButton();
    }
    void ClearButton()
    {
        ItemBox[] itemBoxes = content.GetComponentsInChildren<ItemBox>();
        for (int i = itemBoxes.Length-1; i >=0 ; i--)
        {
            Destroy(itemBoxes[i].gameObject); 
        }
        buttons.Clear();
    }
    void BuyItem(ItemBox item)
    {
        if(item.price>Player.instance.Gold)
        {
            Main.instance.ShowNotEnoughGoldTip();
            return;
        }
        item.Disable();
        Player.instance.AddGold(-item.price);
        if(item.type ==1)
        Player.instance.playerActor.UsingSkillsID.Add(item.id);
        else if(item.type ==2)
        Player.instance.playerActor.abilities.Add(item.id);

        ShowCards();
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
