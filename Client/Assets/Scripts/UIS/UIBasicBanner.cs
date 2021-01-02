using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class UIBasicBanner : MonoBehaviour
{
    public static UIBasicBanner instance;
    Text goldText;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        goldText =transform.Find("up/goldText").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeGoldText()
    {
        // goldText.DOText(Player.instance.Gold.ToString(),2f,true,ScrambleMode.None,null);
        goldText.text = Player.instance.Gold.ToString();
    }
}
