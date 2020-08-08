using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITrait : MonoBehaviour
{
    public static UITrait instance;
    public Transform content;
    void Awake()
    {
        instance =this;
    }
    public void InitUI()
    {
        CreateTraitCaskets();
    }

    void CreateTraitCaskets()
    {
        //首先将所有Trait数据分为2组，一组是玩家已经拥有，一组是尚未拥有
        //先创建所有已拥有，再创建所有未拥有
        var hasTraits =new List<TraitData>();
        var notHasTrait=new List<TraitData>();
        foreach (var item in TraitManager.instance.traitDatas)
        {
            if(Player.instance.IfHasTrait(item.id))
            {
                hasTraits.Add(item);
            }
            else
            {
                notHasTrait.Add(item);
            }
        }
        foreach (var item in hasTraits)
        {
            Transform ts =((GameObject)Instantiate(Resources.Load("Prefabs/TraitCasket"))).transform;
            ts.SetParent(content);
            ts.localScale =Vector3.one;
            ts.GetComponent<TraitCasket>().Init(item,true);
            // content.GetComponent<RectTransform>().sizeDelta += new Vector2(0,300);
        }
        foreach (var item in notHasTrait)
        {
            Transform ts =((GameObject)Instantiate(Resources.Load("Prefabs/TraitCasket"))).transform;
            ts.SetParent(content);
            ts.localScale =Vector3.one;
            ts.GetComponent<TraitCasket>().Init(item,false);
        }
        SetUIListHeight();
    }
    void SetUIListHeight()
    {
        if(content.childCount<10)
        {
            content.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 1100);
        }
        else
        {
            content.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 250+content.childCount*110);
        }
    }
    public void OnClose()
    {
        Destroy(gameObject);
        UIPlayer.instance.RefreashUI();
    }
}
