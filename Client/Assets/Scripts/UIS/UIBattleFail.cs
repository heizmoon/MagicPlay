using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIBattleFail : MonoBehaviour
{
    Button btn_over;
    Button btn_again;
    GameObject statisticUI;
    Button btn_trueOver;
    Text text_cardNumber;
    Text text_abilityNumber;
    Text text_enemyNumber;
    Text text_stepNumber;
    Text text_bossNumber;
    Text text_score;



    void Awake()
    {
        btn_again = transform.Find("Btn_relive").gameObject.GetComponent<Button>();
        btn_over = transform.Find("Btn_gameOver").gameObject.GetComponent<Button>();
        btn_trueOver = transform.Find("StatisticUI/Btn_trueOver").gameObject.GetComponent<Button>();

        btn_over.onClick.AddListener(OnOver);
        btn_again.onClick.AddListener(OnAgain);
        btn_trueOver.onClick.AddListener(OnTrueOver);

        text_cardNumber =transform.Find("StatisticUI/cardNumber").gameObject.GetComponent<Text>();
        text_abilityNumber =transform.Find("StatisticUI/abilityNumber").gameObject.GetComponent<Text>();
        text_enemyNumber =transform.Find("StatisticUI/enemyNumber").gameObject.GetComponent<Text>();
        text_stepNumber =transform.Find("StatisticUI/stepNumber").gameObject.GetComponent<Text>();
        text_bossNumber =transform.Find("StatisticUI/bossNumber").gameObject.GetComponent<Text>();
        text_score =transform.Find("StatisticUI/score").gameObject.GetComponent<Text>();

        statisticUI =transform.Find("StatisticUI").gameObject;
        statisticUI.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnOver()
    {
        //游戏结束
        ShowStatisticUI();

    }
    void OnAgain()
    {
        //看广告复活
        Player.instance.playerActor.ReLiveActor();

        if(UIBattle.Instance)
        {
            UIBattle.Instance.OnBattleGoOn();
            BattleScene.instance.ReturnToBattle();
        }
        else
        {
            BattleScene.instance.OpenMap();

        }
        
        CloseUI();
    }
    void OnTrueOver()
    {
        CloseUI();
        UIBattle.Instance.OnBattleGoOn();
        BattleScene.instance.BattleSceneOver();
    }
    void CloseUI()
    {
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
    public void ShowStatisticUI()
    {
        //显示统计界面
        statisticUI.SetActive(true);
        //统计内容：获得的卡牌数量，获得的道具数量，战胜的敌人数量，走过的步数，换算的分数
        text_cardNumber.text = string.Format("获得卡牌数:{0}",Player.instance.playerActor.UsingSkillsID.Count-Player.instance.playerActor.character.data.skills.Split(',').Length);//初始卡牌數量
        text_abilityNumber.text = string.Format("获得道具数:{0}",Player.instance.playerActor.abilities.Count);
        text_enemyNumber.text = string.Format("击败敌人数:{0}",BattleScene.instance.beatEnemyNumber);
        text_stepNumber.text = string.Format("走过的步数:{0}",BattleScene.instance.steps);
        text_bossNumber.text = string.Format("战胜Boss数:{0}",BattleScene.instance.beatBossNumber);
        int score = BattleScene.instance.steps*100+Player.instance.playerActor.abilities.Count*200+BattleScene.instance.beatEnemyNumber*150+BattleScene.instance.beatBossNumber*1000;
        text_score.text = string.Format("最终分数:{0}",score);
    }
    public static UIBattleFail CreateUI()
    {
        Transform t =((GameObject)Instantiate(Resources.Load("Prefabs/UIBattleFail"))).transform;
        t.SetParent(Main.instance.allScreenUI);
        t.localScale =Vector3.one;
        t.GetComponent<RectTransform>().sizeDelta =Vector3.one;
        t.GetComponent<RectTransform>().anchoredPosition =Vector2.zero;
        return t.GetComponent<UIBattleFail>();
    }
}
