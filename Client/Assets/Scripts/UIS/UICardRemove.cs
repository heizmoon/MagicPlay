using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

using System.Linq;

public class UICardRemove : MonoBehaviour
{
    public Transform content;
    List<int> cardList = new List<int>();
    List<int> temp =new List<int>();
    List<Button> buttons =new List<Button>();
    Button buttonRemove;
    Button BTNClose;
    public int price;
    int rewardCardRank =0;
    int chooseStep;
    bool hasChoosenCard;
    int needChooseStep =2;
    int chooseID;
    void Start()
    {
        buttonRemove =transform.Find("ButtonRemove").GetComponent<Button>();
        BTNClose =transform.Find("ButtonClose").GetComponent<Button>();
        BTNClose.onClick.AddListener(CloseUI);

        content=transform.Find("CardList/Viewport/Content");
    
        transform.DOPunchScale(new Vector3(0.2f,0.2f,0.2f),0.5f,2,1);
        
        cardList = Player.instance.playerActor.UsingSkillsID;
        //按照数字ID排序
        CreateCards();
        SortList();
    }
    void SortList()
    {
        cardList.Sort((x,y)=>x.CompareTo(y));
        Debug.Log(cardList);
        StartCoroutine(WaitForDisableGridLayout());
    }
    IEnumerator WaitForDisableGridLayout()
    {
        content.GetComponent<GridLayoutGroup>().enabled = true;
        yield return new WaitForSeconds(0.2f);
        content.GetComponent<GridLayoutGroup>().enabled = false;
        RefeashPrice();
    }
    void Update()
    {
        
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
            Button button =Instantiate(buttonRemove);
            button.transform.SetParent(itemBox.transform);
            button.transform.localPosition =new Vector3(0,-140,0);
            button.transform.localScale =Vector3.one;
            button.onClick.AddListener(delegate(){RemoveCard(button);});
            buttons.Add(button);
        }
        content.GetComponent<RectTransform>().sizeDelta =new Vector2(0,285*((int)(cardList.Count/3)+1));
    }
    // void AddButton()
    // {
    //     ItemBox[] itemBoxes = content.GetComponentsInChildren<ItemBox>();
    //     foreach (var item in itemBoxes)
    //     {
    //         Button button =Instantiate(buttonRemove);
    //         button.transform.SetParent(item.transform);
    //         button.transform.localPosition =new Vector3(0,-140,0);
    //         button.transform.localScale =Vector3.one;
    //         button.onClick.AddListener(delegate(){RemoveCard(button);});
    //         buttons.Add(button);
    //     }
    // }
    void RemoveCard(Button button)
    {
        if(Player.instance.Gold<price)
        {
            Main.instance.ShowNotEnoughGoldTip();
            return;//钱不够
        }
        ItemBox itemBox =button.GetComponentInParent<ItemBox>();
        Player.instance.playerActor.UsingSkillsID.Remove(itemBox.id);
        buttons.Remove(button);
        DestroyImmediate(itemBox.gameObject);
        Player.instance.AddGold(-price);
        price+=Configs.instance.removeCardGold;
        StartCoroutine(WaitForDisableGridLayout());
    }
   void CloseUI()
   {
       Destroy(gameObject);
   }
    
}
