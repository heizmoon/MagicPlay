using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    void Start()
    {
        buttonRemove =transform.Find("ButtonRemove").GetComponent<Button>();
        BTNClose =transform.Find("ButtonClose").GetComponent<Button>();
        BTNClose.onClick.AddListener(CloseUI);
        content=transform.Find("CardList/Viewport/Content");
        Refeash();
    }
    public void Refeash()
    {
        cardList = Player.instance.playerActor.UsingSkillsID;
        // if(temp.SequenceEqual(cardList))
        // {
        //     Debug.Log("不需要重新加载,temp="+temp.Count+"|cardList="+cardList.Count);
        //     return;
        // }
        // DestoryCards();
        //按照数字ID排序
        SortList();
        // foreach (var item in cardList)
        // {
        //     temp.Add(item);   
        // }
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
        price+=25;
        StartCoroutine(WaitForDisableGridLayout());
    }
   void CloseUI()
   {
       Destroy(gameObject);
   }
}
