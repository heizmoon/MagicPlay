using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Data;

public class AssetsManager : MonoBehaviour
{
    public static AssetsManager instance;
    ///<summary>玩家资产列表</summary>
    List<int> playerAssetsItems =new List<int>();
    ///<summary>所有资产的集合</summary>
    public List<AssetsItem> items =new List<AssetsItem>();
    Transform ts;
    AssetsItemDataSet manager;
    void Awake()
    {
        instance =this;
        ts = GameObject.Find("AssetsItemsPool").transform;
        manager = Resources.Load<AssetsItemDataSet>("DataAssets/AssetsItem");
    }
    public void LoadPlayerAssets()
    {
        if(PlayerPrefs.GetString("playerAssets")=="")
        {
            return;
        }
        //从保存的玩家资产占有数据中获取玩家都拥有那些资产（uid）
        foreach (var item in PlayerPrefs.GetString("playerAssets").Split(','))
        {
            playerAssetsItems.Add(int.Parse(item));
        }
        //从保存的资产数据中获取每项资产的生成数据
        string[] assetsItemsProperty;
        if(PlayerPrefs.GetString("AssetsItems") =="")
        {
            // string sbug = "1001,1,,10,0|1002,2,我的小家,1,0|1003,1,大法杖,5,0";//测试数据
            // assetsItemsProperty=sbug.Split('|');
            return;
        }
        else
        {
            assetsItemsProperty =PlayerPrefs.GetString("AssetsItems").Split('|');

        }
        //接下来实例化并根据生成数据创建出这些资产
        for(int i =0;i<assetsItemsProperty.Length;i++)
        {
           //分割字符串，获取每一项资产的生成数据
            string[] itemProperty =assetsItemsProperty[i].Split(',');
            //如果玩家拥有这件资产
            if(playerAssetsItems.Contains(int.Parse(itemProperty[0])))
            {
                //实例化一个资产
                AssetsItem ait =Instantiate((GameObject)Resources.Load("Prefabs/AssetsItem")).GetComponentInChildren<AssetsItem>();
                //创建这个资产
                ait.CreateAssets(int.Parse(itemProperty[0]),int.Parse(itemProperty[1]),itemProperty[2],int.Parse(itemProperty[3]),int.Parse(itemProperty[4]));
                ait.transform.SetParent(ts);
                items.Add(ait);
            }
        }
        AutoEquipAssets();
    }
    void AutoEquipAssets()//自动给玩家装备上之前装备的资产
    {
       
       if(PlayerPrefs.GetString("equipAssets")=="")
       {
           return;
       }
       string[] ss = PlayerPrefs.GetString("equipAssets").Split(',');
        for(int i =0;i<ss.Length;i++)
        {
            foreach (var item in items)
            {
                if(item.uid==int.Parse(ss[i]))
                {
                    item.equip =true;
                    Player.instance.PlayerEquipAssets(item);
                    item.ChangeItemState();
                }
            }
        }
    }
    ///<summary>根据uid来从池中获取出一个资产</summary>
    public AssetsItem TryGetAssetsFromPool(int uid)
    {
        AssetsItem at =null;
        foreach (var item in items)
        {
            if(item.uid ==uid)
            {
                at =item;
            }
        }
        return at;
    }
    ///<summary>根据类型来获取一个资产列表</summary>
    public List<AssetsItem> TryGetAssetsOfType(int type)
    {
        List<AssetsItem> at =new List<AssetsItem>();//此处可能产生大量垃圾？
        foreach (var item in items)
        {
            if(item._type ==type)
            {
                at.Add(item);
            }
        }
        return at;
    }
    ///<summary>把一个资产放入资产池当中</summary>
    public void PutAssetsIntoPool(Transform at)
    {
        at.SetParent(ts);
        at.localPosition =Vector3.zero;
        at.localScale =Vector3.one;
    }

    public void SaveAssetsItem()
    {
        string s ="";
        foreach (var item in items)
        {
            s+= "|";
            s+= item.uid;
            s+= ",";
            s+= item._id;
            s+= ",";
            s+= item._name;
            s+= ",";
            s+= item.level;
            s+= ",";
            s+= item.freashTime;
        }
        s = s.Remove(0,1);
        PlayerPrefs.SetString("AssetsItems",s);
        Debug.Log(s);    
    }
    public void SavePlayerAssetsItem()
    {
        if(playerAssetsItems.Count==0)
        {
            return;
        }
        string s ="";
        foreach (var item in playerAssetsItems)
        {
            s = s +","+item;
        }
        s =s.Remove(0,1);
        PlayerPrefs.SetString("playerAssets",s);
    }
    public AssetsItem CreateNewAssets(int id,int level)
    {
        AssetsItem ait =Instantiate((GameObject)Resources.Load("Prefabs/AssetsItem")).GetComponentInChildren<AssetsItem>();
        int uid =GetNewUid();
        ait.CreateAssets(uid,id,"",level,Mathf.FloorToInt(DateManager.instance.now.totalDays/30));
        ait.transform.SetParent(ts);
        items.Add(ait);
        SaveAssetsItem();
        return ait;
    }
    ///<summary>获取最新的UID</summary>
    int GetNewUid()
    {
        List<int> list =new List<int>();
        foreach (var item in items)
        {
            list.Add(item.uid);
        }
        if(list.Count==0)
        {
            return 1000;
        }
        return list.Max()+1;
    }
    public void AssestsDecayLife(int month)
    {
        foreach (var item in items)
        {
            item.DecayLife(month);
        }
    }
    ///<summary>把一个资产加入玩家的资产列表</summary>
    public void GiveAssetsToPlayer(AssetsItem item)
    {
        playerAssetsItems.Add(item.uid);
    }

    public string GetInfo(int id ,string content)
    {
        foreach(var item in manager.dataArray)
        {
            if(item.id==id)
            {
                switch(content)
                {
                    case "name":
                    // Debug.LogFormat("内容:{0}",item.name);
                    return item.name;
                    case "describe":
                    // Debug.LogFormat("内容:{0}",item.describe);
                    break;
                    case "type":
                    return item.type.ToString();
                    case "equipType":
                    return item.equipType.ToString();
                    case "icon":
                    return item.icon;
                    case "life":
                    return item.life.ToString();
                }     
            }    
        }
        return "";
    }
    public AssetsItemData GetInfo(int id)
    {
       foreach(var item in manager.dataArray)
        {
            if(item.id==id)
            {
                return item;       
            } 
        }
        return null;
    } 
}
