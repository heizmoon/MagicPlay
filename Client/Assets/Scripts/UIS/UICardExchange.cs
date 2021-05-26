using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

using System.Linq;

public class UICardExchange : MonoBehaviour
{
    public Transform content;
    List<int> cardList = new List<int>();
    List<int> temp =new List<int>();
    List<Button> buttons =new List<Button>();
    Button buttonRemove;
    Button BTNClose;
    Button BTNRetry;
    public int price;
    int rewardCardRank =0;
    public List<ItemBox> skillItemBoxes;
    int chooseStep;
    bool hasChoosenCard;
    int needChooseStep =2;
    int chooseID;
    int type =0;
    void Awake()
    {
        buttonRemove =transform.Find("ButtonRemove").GetComponent<Button>();
        BTNClose =transform.Find("ButtonClose").GetComponent<Button>();
        BTNRetry =transform.Find("ButtonRetry").GetComponent<Button>();
        BTNClose.onClick.AddListener(CloseUI);
        BTNRetry.onClick.AddListener(OnRetry);

        content=transform.Find("CardList/Viewport/Content");
    }
    void Start()
    {
        transform.DOPunchScale(new Vector3(0.2f,0.2f,0.2f),0.5f,2,1); 
    }
    void Init(int type)
    {
        this.type = type;
        rewardCardRank =Configs.instance.GetCardRank(BattleScene.instance.steps);
        foreach (var item in skillItemBoxes)
        {
            item.button.onClick.AddListener(delegate () {GetItem(item);});
        }
        cardList = Player.instance.playerActor.UsingSkillsID;
        //按照数字ID排序
        SortList();

        Refreash();
    }
    public void Refreash()
    {
        if(Main.instance.ifNewBird <=7)//创造固定奖励
        {
            SkillData[] Sdatas =new SkillData[3]; 
            Sdatas[0]= SkillManager.instance.GetInfo(169);
            Sdatas[1]= SkillManager.instance.GetInfo(170);
            Sdatas[2]= SkillManager.instance.GetInfo(171);

            for (int i = 0; i < Sdatas.Length; i++)
            {
                skillItemBoxes[i].Reset();
                skillItemBoxes[i].Init(Sdatas[i]);
                skillItemBoxes[i].InReward();
            }
            Main.instance.ifNewBird++;
            BTNRetry.gameObject.SetActive(false);
            return;
        }
        else if(Main.instance.ifNewBird <=14)//创造固定奖励
        {
            SkillData[] Sdatas =new SkillData[3]; 
            Sdatas[0]= SkillManager.instance.GetInfo(200);
            Sdatas[1]= SkillManager.instance.GetInfo(212);
            Sdatas[2]= SkillManager.instance.GetInfo(354);

            for (int i = 0; i < Sdatas.Length; i++)
            {
                skillItemBoxes[i].Reset();
                skillItemBoxes[i].Init(Sdatas[i]);
                skillItemBoxes[i].InReward();
            }
            Main.instance.ifNewBird=15;
            BTNRetry.gameObject.SetActive(false);
            return;
        }
        else
        {
            SkillData[] Sdatas = SkillManager.instance.GetRandomSelfSkillsLevelLimit(3,rewardCardRank);
            for (int i = 0; i < Sdatas.Length; i++)
            {
                skillItemBoxes[i].Reset();
                skillItemBoxes[i].Init(Sdatas[i]);
                skillItemBoxes[i].InReward();
            }
        }   
    }
    void SortList()
    {
        cardList.Sort((x,y)=>x.CompareTo(y));
        Debug.Log(cardList);
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
    void Update()
    {
        
    }
    void OnRetry()
    {
        //播放广告，重置货品
        Refreash();
    }
    void GetItem(ItemBox item)
    {
        chooseID = item.id;
        item.button.GetComponentInChildren<Text>().text ="已选择";
        
        for (int i = 0; i < skillItemBoxes.Count; i++)
        {
            skillItemBoxes[i].button.onClick.RemoveAllListeners();
            if(skillItemBoxes[i]!= item)
            skillItemBoxes[i].CantChoose();
        }
        item.button.onClick.AddListener(delegate () {CancelChoose(item);});

        AddButton();
    }
    void CancelChoose(ItemBox item)
    {
        item.button.onClick.RemoveAllListeners();
        item.button.GetComponentInChildren<Text>().text ="选择";
        foreach (var i in skillItemBoxes)
        {
            i.Reset();
            i.button.onClick.AddListener(delegate () {GetItem(item);});
        }
    }
    void RefeashPrice()
    {
        foreach (var item in buttons)
        {
            item.GetComponentInChildren<Text>().text =price>0?string.Format("{0}",price):"免费";   
        }
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
        Player.instance.playerActor.UsingSkillsID.Add(chooseID);
        //播放动画，两张牌位置互换。然后选择框中的牌悉数消失
        CloseUI();
    }
   void CloseUI()
   {
        if(UIBattle.Instance)
        UIBattle.Instance.OnBattleGoOn();
        if(type>0)
        UIBattleReward.CreateUIBattleReward(type);
        Destroy(gameObject);
   }
   public static void CreateUICardExchange(int type)
   {
        GameObject go =(GameObject)Instantiate(Resources.Load("Prefabs/UICardExchange"));
        go.transform.SetParent(Main.instance.allScreenUI); 
        go.transform.localScale =Vector3.one;
        go.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        go.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
        go.GetComponent<UICardExchange>().Init(type);
   }
    
}
