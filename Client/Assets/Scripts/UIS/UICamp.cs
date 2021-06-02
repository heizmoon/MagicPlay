using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICamp : MonoBehaviour
{
    public Button BTNSleep;
    public Button BTNRemove;
    public Button BTNGift;
    int giftType;

    void Start()
    {
        BTNSleep.onClick.AddListener(OnSleep);
        BTNRemove.onClick.AddListener(OnRemoveCard);
        BTNGift.onClick.AddListener(OnGift);
        if(Main.instance.ifNewBird<18)
        {
            
        }
        BTNRemove.gameObject.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnSleep()
    {
        Player.instance.playerActor.AddHp(Mathf.CeilToInt(Player.instance.playerActor.HpMax*Configs.instance.campRestoreHP) );
        gameObject.SetActive(false);
        BattleScene.instance.OpenMap();
        Destroy(gameObject);
    }
    public void Init()
    {
        //根据情况决定都显示哪些按钮
        // if(Player.instance.CharID !=3)
        // {
        //     BTNGift.gameObject.SetActive(false);
        // }
        RandomGift();
    }
    void OnRemoveCard()
    {
        GameObject go =(GameObject)Instantiate(Resources.Load("Prefabs/UICardRemove"));
        go.transform.SetParent(Main.instance.allScreenUI); 
		go.transform.localScale =Vector3.one;
        go.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
		go.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;

    }
    void OnGift()
    {
        switch(giftType)
        {
            case 0:
            Player.instance.playerActor.basicDefence++;
            break;
            case 1:
            Player.instance.playerActor.AddMaxMP(1);
            break;
            case 2:
            Player.instance.playerActor.AddMaxHP(5);
            break;
            case 3:
            Player.instance.playerActor.Crit+=5;
            break;
        }
        gameObject.SetActive(false);
        BattleScene.instance.OpenMap();
        Destroy(gameObject);
    }
    void RandomGift()
    {
        //随机一个基础属性奖励
        //1.防御力+1；2.能量上限+1；3.生命上限+5；4.暴击率+5%；
        int r =Random.Range(0,4);
        switch(r)
        {
            case 0:
            BTNGift.GetComponentInChildren<Text>().text="防御力+1";
            break;
            case 1:
            BTNGift.GetComponentInChildren<Text>().text="能量上限+1";
            break;
            case 2:
            BTNGift.GetComponentInChildren<Text>().text="生命上限+5";
            break;
            case 3:
            BTNGift.GetComponentInChildren<Text>().text="暴击率+5%";
            break;
        }
        giftType =r;
    }
}
