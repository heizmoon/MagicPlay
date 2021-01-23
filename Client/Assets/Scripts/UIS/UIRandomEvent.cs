using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIRandomEvent : MonoBehaviour
{
    Text _describe;
    Text _ch1Des;
    Text _ch2Des;
    Text _ch3Des;
    Text _ch4Des;
    Button _ch1Btn;
    Button _ch2Btn;
    Button _ch3Btn;
    Button _ch4Btn;
    Button _BTNOK;

    Image _image;
    RandomEvent _event;
    List<int> effList;
    string[] strs =new string[26];
    void Awake()
    {
        _describe =transform.Find("background/describe").GetComponent<Text>();
        _ch1Des =transform.Find("background/Ch1/Text").GetComponent<Text>();
        _ch2Des =transform.Find("background/Ch2/Text").GetComponent<Text>();
        _ch3Des =transform.Find("background/Ch3/Text").GetComponent<Text>();
        _ch4Des =transform.Find("background/Ch4/Text").GetComponent<Text>();
        _ch1Btn =transform.Find("background/Ch1").GetComponent<Button>();
        _ch2Btn =transform.Find("background/Ch2").GetComponent<Button>();
        _ch3Btn =transform.Find("background/Ch3").GetComponent<Button>();
        _ch4Btn =transform.Find("background/Ch4").GetComponent<Button>();
        _BTNOK =transform.Find("background/ButtonOK").GetComponent<Button>();
        _image =transform.Find("background/Image").GetComponent<Image>();
        _ch1Btn.onClick.AddListener(delegate {OnChooseEvent(_ch1Btn);});
        _ch2Btn.onClick.AddListener(delegate {OnChooseEvent(_ch2Btn);});
        _ch3Btn.onClick.AddListener(delegate {OnChooseEvent(_ch3Btn);});
        _ch4Btn.onClick.AddListener(delegate {OnChooseEvent(_ch4Btn);});
        _BTNOK.onClick.AddListener(OnClose);

    }
    void Start()
    {
        _BTNOK.gameObject.SetActive(false);
        _event = RandomEventManager.instance.GetRandomEventByRank(0);
        _image.sprite =Resources.Load<Sprite>("Texture/"+_event.image);
        _describe.text = _event.describe;
        _ch1Des.text =_event.des1;
        _ch2Des.text =_event.des2;
        _ch3Des.text =_event.des3;
        _ch4Des.text =_event.des4;

        if(_event.des2=="")
        _ch2Btn.gameObject.SetActive(false);
        if(_event.des3=="")
        _ch3Btn.gameObject.SetActive(false);
        if(_event.des4=="")
        _ch4Btn.gameObject.SetActive(false);
        
    }
    void OnChooseEvent(Button button)
    {
        if(button == _ch1Btn)
        {
            _ch1Btn.gameObject.SetActive(false);
            _ch2Btn.gameObject.SetActive(false);
            _ch3Btn.gameObject.SetActive(false);
            _ch4Btn.gameObject.SetActive(false);
            _BTNOK.gameObject.SetActive(true);

            _image.sprite =Resources.Load<Sprite>("Texture/"+_event.img1);
            _describe.text =_event.res1;
            effList =_event.eList1;
        }
        else if(button ==_ch2Btn)
        {
            _ch1Btn.gameObject.SetActive(false);
            _ch2Btn.gameObject.SetActive(false);
            _ch3Btn.gameObject.SetActive(false);
            _ch4Btn.gameObject.SetActive(false);
            _BTNOK.gameObject.SetActive(true);

            _image.sprite =Resources.Load<Sprite>("Texture/"+_event.img2);
            _describe.text =_event.res2;
            effList =_event.eList2;
        }
        else if(button ==_ch3Btn)
        {
            _ch1Btn.gameObject.SetActive(false);
            _ch2Btn.gameObject.SetActive(false);
            _ch3Btn.gameObject.SetActive(false);
            _ch4Btn.gameObject.SetActive(false);
            _BTNOK.gameObject.SetActive(true);

            _image.sprite =Resources.Load<Sprite>("Texture/"+_event.img3);
            _describe.text =_event.res3;
            effList =_event.eList3;
        }
        else if(button ==_ch4Btn)
        {
            _ch1Btn.gameObject.SetActive(false);
            _ch2Btn.gameObject.SetActive(false);
            _ch3Btn.gameObject.SetActive(false);
            _ch4Btn.gameObject.SetActive(false);
            _BTNOK.gameObject.SetActive(true);

            _image.sprite =Resources.Load<Sprite>("Texture/"+_event.img4);
            _describe.text =_event.res4;
            effList =_event.eList4;
        }
        DoEventEffect();
        if(button == _ch1Btn)
        _describe.text =string.Format(_event.res1,strs);
        else if(button == _ch2Btn)
        _describe.text =string.Format(_event.res2,strs);
        else if(button == _ch3Btn)
        _describe.text =string.Format(_event.res3,strs);
        else if(button == _ch4Btn)
        _describe.text =string.Format(_event.res4,strs);
    } 
    // Update is called once per frame
    void Update()
    {
        
    }
    void DoEventEffect()
    {
        /*
        1:获得随机50-100金钱
        2:失去随机50-100金钱
        3:回复随机25-50生命值
        4:失去随机25-50生命值
        5：提高随机10-20生命上限
        6：降低随机10-20生命上限
        7：获得随机1-3张任意级别本职业技能卡
        8：失去随机1张技能卡
        9：获得随机1-3张rank0本职业技能卡
        10：获得随机1-3张rank1本职业技能卡
        11：获得随机1-3张rank2本职业技能卡
        12：获得随机1-3张rank3本职业技能卡
        13：获得随机1-3张rank4本职业技能卡
        14：获得随机1-3张rank5本职业技能卡
        15：获得随机当前未持有的圣遗物
        16：获得随机宝箱当前未持有圣遗物
        17：获得随机商店当前未持有圣遗物
        18：获得随机精英当前未持有圣遗物
        19：获得随机BOSS当前未持有圣遗物
        20:失去角色初始圣遗物
        21：接下来每场战斗结束后看广告
        22：接下来每场战斗结束后金钱奖励翻倍
        23：获得一次重生能力
        24：完全恢复
        25：失去所有金钱
        26：获得随机诅咒圣遗物（当前未持有）
        */
        if(effList.Contains(1))
        {
            int r =Random.Range(50,100);
            Player.instance.AddGold(r);
            strs[1]=r.ToString();
        }
        if(effList.Contains(2))
        {
            int r =Random.Range(-50,-100);
            Player.instance.AddGold(r);
            strs[2]=(-r).ToString();
        }
        if(effList.Contains(3))
        {
            int r =Random.Range(25,50);
            Player.instance.playerActor.AddHp(r);
            strs[3]=r.ToString();

        }
        if(effList.Contains(4))
        {
            int r =Random.Range(-25,-50);
            Player.instance.playerActor.AddHp(r);
            strs[4]=(-r).ToString();
        }
    }
    void OnClose()
    {
        //判断角色是否死亡
        if(Player.instance.playerActor.HpCurrent<=0||Player.instance.playerActor.HpMax<=0)
        {
            UIBattleFail.CreateUI();
        }
        else
        {
            gameObject.SetActive(false);
            BattleScene.instance.OpenMap();
        }
        Destroy(gameObject);
    }
}
