using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class UIAbilityGroup : MonoBehaviour
{
    public static UIAbilityGroup instance;
    public Transform content;
    List<int> abilityList = new List<int>();
    List<int> temp =new List<int>();
    void Awake()
    {
        if(instance ==null)
        instance = this;
    }
    public void Refeash()
    {
        abilityList = Player.instance.playerActor.abilities;
        if(abilityList.Count>0&&temp.SequenceEqual(abilityList))
        {
            return;
        }
        DestoryCards();
        //按照数字ID排序
        SortList();
        foreach (var item in abilityList)
        {
            temp.Add(item);
        }
    }
    void SortList()
    {
        abilityList.Sort((x,y)=>x.CompareTo(y));
        Debug.Log(abilityList);
        CreateItems();
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
    void CreateItems()
    {
        foreach (var item in abilityList)
        {
            ItemBox itemBox = ((GameObject)Instantiate(Resources.Load("Prefabs/itemBox"))).GetComponent<ItemBox>();
            itemBox.transform.SetParent(content);
            itemBox.transform.localScale = Vector3.one;
            itemBox.Init(AbilityManager.instance.GetInfo(item));
            itemBox.HideToggleSelect();
        }
        content.GetComponent<RectTransform>().sizeDelta =new Vector2(0,205*((int)(abilityList.Count/4)+1));
    }
    void DestoryCards()
    {
        for (int i = content.childCount-1; i >= 0; i--)
        {
            Destroy(content.GetChild(i).gameObject);
        }
    }
}
