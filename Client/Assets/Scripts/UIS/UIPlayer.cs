using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIPlayer : MonoBehaviour
{
    public static UIPlayer instance;
    public Transform actorFrame;
    List<Text> propertyText =new List<Text>();
    // public Button BTNAssets;
    public Text describe;
    
    void Awake()
    {
        instance =this;
    }
    void Start()
    {
    //    BTNAssets.onClick.AddListener(OnOpenAssetsUI);
        if(propertyText.Count==0)
        {
            foreach(var item in actorFrame.GetComponentsInChildren<Text>())
            {
                propertyText.Add(item);
                
            }
        }
        RefreashUI();
    }

    void Update()
    {
        
    }
    public void RefreashUI()
    {
        //获取属性显示文字
        propertyText[0].text =string.Format("生命:{0}+{1}",Player.instance.basicHp,Player.instance.hp); 
        propertyText[1].text =string.Format("魔力:{0}+{1}",Player.instance.basicMp,Player.instance.mp); 
        propertyText[2].text =string.Format("闪避:{0}+{1}",Player.instance.basicDodge,Player.instance.dodge);
        propertyText[3].text =string.Format("韧性:{0}+{1}",Player.instance.basicTough,Player.instance.tough);
        //如果正在浏览UIPlyer,则把角色显示在actorFrame
		if(Main.instance.UIState==-1)
        {
            actorFrame.gameObject.SetActive(true);
            Transform playerActor = Player.instance.playerActor.transform;
            playerActor.SetParent(actorFrame);
            playerActor.localPosition =Vector3.zero;
            playerActor.localScale =Vector3.one*1.5f;
        }
        string marry ="";
        string gender ="";
        string work ="";
        if(Player.instance.marriaged)
        {
            marry ="已婚";
        }
        else
        {
            marry ="未婚";
        }
        if(Player.instance.sex ==0)
        {
            gender ="♀";
        }
        else
        {
            gender ="♂";
        }
        switch(Player.instance.rank)
        {
            case 0:
            work ="魔法学徒";
            break;

        }
        describe.text =string.Format("{0}岁 {1} {2} {3}",Player.instance.age,gender,marry,work);
        
    }
    public void OnOpenAssetsUI()
    {
        HideAvatar();
        Main.instance.InitUIPlayerAssets();
    }
    public void OnOpenTraitUI()
    {
        HideAvatar();
        Main.instance.InitUITrait();
    }
    void HideAvatar()
    {
        actorFrame.gameObject.SetActive(false);
    }
}
