using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICamp : MonoBehaviour
{
    public Button BTNSleep;
    void Start()
    {
        BTNSleep.onClick.AddListener(OnSleep);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnSleep()
    {
        Player.instance.playerActor.AddHp(Mathf.CeilToInt(Player.instance.playerActor.HpMax*0.5f) );
        gameObject.SetActive(false);
        BattleScene.instance.OpenMap();
        Destroy(gameObject);
    }
    public void Init()
    {
        //根据情况决定都显示哪些按钮
    }
}
