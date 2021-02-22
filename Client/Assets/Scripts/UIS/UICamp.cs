using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICamp : MonoBehaviour
{
    public Button BTNSleep;
    public Button BTNRemove;

    void Start()
    {
        BTNSleep.onClick.AddListener(OnSleep);
        BTNRemove.onClick.AddListener(OnRemoveCard);

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
    }
    void OnRemoveCard()
    {
        GameObject go =(GameObject)Instantiate(Resources.Load("Prefabs/UICardRemove"));
        go.transform.SetParent(Main.instance.allScreenUI); 
		go.transform.localScale =Vector3.one;
        go.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
		go.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;

    }
}
