using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerAssets : MonoBehaviour
{
    public static UIPlayerAssets instance;
    public Transform listAssets;
    public Transform tab;
    public Transform infomation;
    public Text itemName;
    public Text itemDescribe;
    public Text itemLife;
    public Image itemIcon;
    AssetsItem assetsItem;//当前选中的assetsitem
    public Button BTNSell;
    public Button BTNEquip;
    public Button BTNClose;
    public GameObject changeName;
    public Text newName;
    int nowPage =0;
    public Text goldText;
    public Text totalText;
    
    void Awake()
    {
        instance =this;
    }
    void Start()
    {

    }
    public void InitUI()
    {
        //1.获取标签页toggle
        //2.根据当前所在标签页加载玩家拥有的assetsItem
        foreach(var item in tab.GetComponentsInChildren<Toggle>()) 
        {
            item.onValueChanged.AddListener((bool isOn) =>OnTabChanged(item,isOn));
        }
        BTNClose.onClick.AddListener(OnClickClose);
    }   
    void OnTabChanged(Toggle toggle,bool isOn)
    {
        if(!isOn)
        {
            return;
        }
        
        switch (toggle.name)
        {
            case "Toggle_equip":
            nowPage =0;
            break;
            case "Toggle_house":
            nowPage =1;
            break;
            case "Toggle_pet":
            nowPage =2;
            break;
            case "Toggle_land":
            nowPage =3;
            break;
        }
        InitList();
    }
    public void InitList()
    {
        List<AssetsItem> at =new List<AssetsItem>();
        //把现有的item拿走
        PutAwayCurrentItems();
        //获取需要的item列表
        at = AssetsManager.instance.TryGetAssetsOfType(nowPage);
        //把需要的item放进来
        PutAssetsItemsInList(at);
        //调整List的高度
        SetUIListHeight();
        infomation.gameObject.SetActive(false);
        goldText.text =string.Format(goldText.text,Player.instance.gold);
        // totalText.text =string.Format(totalText.text);
    }
    void PutAssetsItemsInList(List<AssetsItem> at)
    {
        foreach (var item in at)
        {
            item.transform.SetParent(listAssets);
            item.transform.localPosition =Vector3.zero;
            item.transform.localScale =Vector3.one;
            item.GetComponent<Toggle>().group =listAssets.GetComponent<ToggleGroup>();
            item.GetComponent<Toggle>().onValueChanged.AddListener((bool isOn)=>OnItemChoosen(item,isOn));
        }
    }
    void PutAwayCurrentItems()
    {
        foreach (var item in listAssets.GetComponentsInChildren<AssetsItem>())
        {
            AssetsManager.instance.PutAssetsIntoPool(item.transform);
            item.GetComponent<Toggle>().group =null;
            item.GetComponent<Toggle>().onValueChanged.RemoveAllListeners();
        }
    }
    void OnItemChoosen(AssetsItem assetsItem,bool isOn)
    {
        if(!isOn)
        {
            return;
        }
        this.assetsItem =assetsItem;
        infomation.gameObject.SetActive(true);
        itemName.text = assetsItem._name;
        itemDescribe.text =assetsItem._describe;
        if(assetsItem._life==-1)
        {
            itemLife.text =string.Format("预期使用寿命：无限");
        }
        else
        {
            itemLife.text =string.Format("预期使用寿命：{0}个月",assetsItem._life);
        }
        itemIcon.sprite = Resources.Load("Texture/Assets/"+assetsItem._icon,typeof(Sprite)) as Sprite;
        BTNEquip.gameObject.SetActive(true);
        string  st ="";
        switch(assetsItem._type)
        {
            case 0:
            st ="穿戴";
            break;
            case 1:
            st ="设为家";
            break;
            case 2:
            st ="骑乘";
            break;
            case 3:
            BTNEquip.gameObject.SetActive(false);
            break;
        }
        BTNEquip.GetComponentInChildren<Text>().text =st;
        BTNSell.GetComponentInChildren<Text>().text =string.Format("售卖:{0}",assetsItem._value);
    }
    public void OnClickEquip()
    {
        if(assetsItem.equip)
        {
            return;
        }
        assetsItem.equip =true;
        assetsItem.ChangeItemState();
        //找到所有其他于本资产类型相同的资产，把equip设置为false
        foreach (var item in listAssets.GetComponentsInChildren<AssetsItem>())
        {
            if(item!=assetsItem&&item._equipType == assetsItem._equipType)
            {
                if(item.equip)
                {
                    item.equip =false;
                    item.ChangeItemState();
                    Player.instance.PlayerUnEquipAssets(item);
                }
            }
        }
        Player.instance.PlayerEquipAssets(assetsItem);
    }
    public void OnClickSell()
    {
        assetsItem.TrySellItem();
    }
    public void OnClickClose()
    {
        gameObject.SetActive(false);
        UIPlayer.instance.RefreashUI();
    }
    public void OnChangeNameButtonClick()
    {
        changeName.SetActive(true);

    }
    public void OnChangeName()
    {
        if(newName.text =="")
        {
            changeName.SetActive(false);
            return;
        }
        assetsItem._name = newName.text;
        assetsItem.nameText.text =newName.text;
        itemName.text =newName.text;
        AssetsManager.instance.SaveAssetsItem();
        changeName.SetActive(false);

    }
    public void OnChangeNameCancel()
    {
        changeName.SetActive(false);
    }
    void SetUIListHeight()
    {
        if(listAssets.childCount<12)
        {
            listAssets.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 600);    
        }
        else
        {
            listAssets.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 200+listAssets.childCount*50);
            // listAssets.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0,0,0);
        }
    }
}
