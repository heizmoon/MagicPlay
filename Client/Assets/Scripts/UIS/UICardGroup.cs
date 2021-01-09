using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.Linq;

public class UICardGroup : MonoBehaviour
{
    public static UICardGroup instance;
    public Transform content;
    List<int> cardList = new List<int>();
    List<int> temp =new List<int>();
    void Awake()
    {
        if(instance ==null)
        instance = this;
    }
    public void Refeash()
    {
        cardList = Player.instance.playerActor.UsingSkillsID;
        if(temp.SequenceEqual(cardList))
        {
            Debug.Log("不需要重新加载,temp="+temp.Count+"|cardList="+cardList.Count);
            return;
        }
        DestoryCards();
        //按照数字ID排序
        SortList();
        foreach (var item in cardList)
        {
            temp.Add(item);   
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
        yield return new WaitForSeconds(0.5f);
        content.GetComponent<GridLayoutGroup>().enabled = false;
    }
    void Update()
    {
        
    }
    void CreateCards()
    {
        foreach (var item in cardList)
        {
            // SkillCard skillCard = ((GameObject)Instantiate(Resources.Load("Prefabs/SkillCard"))).GetComponent<SkillCard>();
            // skillCard.transform.SetParent(content);
            // skillCard.transform.localScale = Vector3.one;
            // skillCard.Init(SkillManager.instance.GetInfo(item));
            ItemBox itemBox = ((GameObject)Instantiate(Resources.Load("Prefabs/itemBox"))).GetComponent<ItemBox>();
            itemBox.transform.SetParent(content);
            itemBox.transform.localScale = Vector3.one;
            itemBox.Init(SkillManager.instance.GetInfo(item));
            itemBox.HideToggleSelect();
        }
        content.GetComponent<RectTransform>().sizeDelta =new Vector2(0,256*((int)(cardList.Count/4)+1));
    }
    void DestoryCards()
    {
        for (int i = content.childCount-1; i >= 0; i--)
        {
            Destroy(content.GetChild(i).gameObject);
        }
    }
}
