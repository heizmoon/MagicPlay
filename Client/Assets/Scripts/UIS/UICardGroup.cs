using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        if(cardList.Count>0&&temp.SequenceEqual(cardList))
        {
            return;
        }
        DestoryCards();
        cardList = Player.instance.playerActor.UsingSkillsID;
        //按照数字ID排序
        SortList();
        temp =cardList;
    }
    void SortList()
    {
        cardList.Sort((x,y)=>x.CompareTo(y));
        Debug.Log(cardList);
        CreateCards();
    }
    void Update()
    {
        
    }
    void CreateCards()
    {
        foreach (var item in cardList)
        {
            SkillCard skillCard = ((GameObject)Instantiate(Resources.Load("Prefabs/SkillCard"))).GetComponent<SkillCard>();
            skillCard.transform.SetParent(content);
            skillCard.transform.localScale = Vector3.one;
            skillCard.Init(SkillManager.instance.GetInfo(item));
        }
    }
    void DestoryCards()
    {
        for (int i = content.childCount-1; i >= 0; i--)
        {
            Destroy(content.GetChild(i).gameObject);
        }
    }
}
