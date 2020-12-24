using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UITreasure : MonoBehaviour
{
    public GameObject Gframe;
    public Button BTNOpen;
    public Button BTNOK;
    public Button BTNReTry;
    public Button BTNReturn;
    void Start()
    {
        BTNOK.onClick.AddListener(OnOK);
        BTNOpen.onClick.AddListener(OnOpen);
        BTNReturn.onClick.AddListener(OnReturn);
        BTNReTry.onClick.AddListener(OnRetry);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Init()
    {
        //根据情况随机出宝物
    }
    void OnOpen()
    {
        Gframe.SetActive(true);
    }
    void OnOK()
    {
        gameObject.SetActive(false);
        BattleScene.instance.OpenMap();
        Destroy(gameObject);
    }
    void OnRetry()
    {

    }
    void OnReturn()
    {
        gameObject.SetActive(false);
        BattleScene.instance.OpenMap();
        Destroy(gameObject);
    }
}
