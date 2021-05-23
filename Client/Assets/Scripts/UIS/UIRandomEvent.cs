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
    int rewardCardRank;

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
        rewardCardRank =Configs.instance.GetCardRank(BattleScene.instance.steps);
        
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

        */
        //1:获得随机10-50金钱
        if(effList.Contains(1))
        {
            int r =Random.Range(10,50);
            Player.instance.AddGold(r);
            strs[1]=r.ToString();
        }
        //2:失去随机10-50金钱
        if(effList.Contains(2))
        {
            int r =Random.Range(-10,-50);
            Player.instance.AddGold(r);
            strs[2]=(-r).ToString();
        }
        //3:回复随机10-30生命值
        if(effList.Contains(3))
        {
            int r =Random.Range(10,30);
            Player.instance.playerActor.AddHp(r);
            strs[3]=r.ToString();

        }
        //4:失去随机10-30生命值
        if(effList.Contains(4))
        {
            int r =Random.Range(-10,-30);
            Player.instance.playerActor.AddHp(r);
            strs[4]=(-r).ToString();
        }
        //5：提高随机5-15生命上限
        if(effList.Contains(5))
        {
            int r =Random.Range(5,15);
            Player.instance.playerActor.AddMaxHP(r);
            strs[5]=(r).ToString();
        }
        //6：降低随机5-15生命上限
        if(effList.Contains(6))
        {
            int r =Random.Range(-5,-15);
            Player.instance.playerActor.AddMaxHP(r);
            strs[6]=(-r).ToString();
        }
        //7：获得随机1-3张任意级别本职业技能卡
        if(effList.Contains(7))
        {
            SkillData[] skillDatas =SkillManager.instance.GetRandomSelfSkills(1);
            AddSkillForPlayerActor(skillDatas);
            strs[7]=(skillDatas[0].name).ToString();
        }
        //8：失去随机1张技能卡
        if(effList.Contains(8))
        {
            int r =Random.Range(0,Player.instance.playerActor.UsingSkillsID.Count);
            string _name = SkillManager.instance.GetInfo(Player.instance.playerActor.UsingSkillsID[r],"name");
            Player.instance.playerActor.UsingSkillsID.RemoveAt(r);
            strs[8]=_name;
        }
        //9：获得随机1-3张rank0本职业技能卡
        if(effList.Contains(9))
        {
            // int r =Random.Range(1,4);
            SkillData[] skillDatas = SkillManager.instance.GetRandomSelfSkillsLevelLimit(1,0);
            AddSkillForPlayerActor(skillDatas);
            strs[9]=(skillDatas[0].name).ToString();
        }
        //10：获得随机1-3张rank1本职业技能卡
        if(effList.Contains(10))
        {
            SkillData[] skillDatas = SkillManager.instance.GetRandomSelfSkillsLevelLimit(1,1);
            AddSkillForPlayerActor(skillDatas);
            strs[10]=(skillDatas[0].name).ToString();
        }
        //11：获得随机1-3张rank2本职业技能卡
        if(effList.Contains(11))
        {
            SkillData[] skillDatas = SkillManager.instance.GetRandomSelfSkillsLevelLimit(1,2);
            AddSkillForPlayerActor(skillDatas);
            strs[11]=(skillDatas[0].name).ToString();
        }
        //12：获得随机1-3张rank3本职业技能卡
        if(effList.Contains(12))
        {
            SkillData[] skillDatas =  SkillManager.instance.GetRandomSelfSkillsLevelLimit(1,3);
            AddSkillForPlayerActor(skillDatas);
            strs[12]=(skillDatas[0].name).ToString();
        }
        //13：获得随机1-3张rank4本职业技能卡
        if(effList.Contains(13))
        {
            SkillData[] skillDatas = SkillManager.instance.GetRandomSelfSkillsLevelLimit(1,4);
            AddSkillForPlayerActor(skillDatas);
            strs[13]=(skillDatas[0].name).ToString();
        }
        //14.获得随机1张当前级别本职业技能卡
        if(effList.Contains(14))
        {
            SkillData[] skillDatas = SkillManager.instance.GetRandomSelfSkillsLevelLimit(1,rewardCardRank);
            AddSkillForPlayerActor(skillDatas);
            strs[14]=(skillDatas[0].name).ToString();
        }
        //15：随机获得当前级别的未持有的圣遗物
        if(effList.Contains(15))
        {
            AbilityData[] abilities= AbilityManager.instance.GetRandomAbility(1,rewardCardRank);
            AddAbilityForPlayerActor(abilities);
            strs[15]=(abilities[0].name).ToString();
        }
        //16：获得随机rank0当前未持有圣遗物
        if(effList.Contains(16))
        {
            AbilityData[] abilities= AbilityManager.instance.GetRandomAbility(1,0);
            AddAbilityForPlayerActor(abilities);
            strs[16]=(abilities[0].name).ToString();
        }
        //17：获得随机rank1当前未持有圣遗物
        if(effList.Contains(17))
        {
            AbilityData[] abilities= AbilityManager.instance.GetRandomAbility(1,1);
            AddAbilityForPlayerActor(abilities);
            strs[17]=(abilities[0].name).ToString();
        }
        //18：获得随机rank2当前未持有圣遗物
        if(effList.Contains(18))
        {
            AbilityData[] abilities= AbilityManager.instance.GetRandomAbility(1,2);
            AddAbilityForPlayerActor(abilities);
            strs[18]=(abilities[0].name).ToString();
        }
        //19：获得随机rank3当前未持有圣遗物
        if(effList.Contains(19))
        {
            AbilityData[] abilities= AbilityManager.instance.GetRandomAbility(1,3);
            AddAbilityForPlayerActor(abilities);
            strs[19]=(abilities[0].name).ToString();
        }
        //20:失去角色初始圣遗物
        if(effList.Contains(20))
        {
            Player.instance.playerActor.abilities.RemoveAt(0);
            strs[20]=(AbilityManager.instance.GetInfo(Player.instance.playerActor.character.data.relic).name).ToString();
        }
       /* 21：接下来每场战斗结束后看广告
        22：接下来战斗结束后金钱奖励翻倍，持续1-3场战斗
        23：获得一次重生能力
        28：经验奖励增加50%,持续1-3场战斗
        29：经验奖励减少50%,持续1-3场战斗
        32：后续怪物的强度增加
        33：
        */

        //24：完全恢复
        if(effList.Contains(24))
        {
            Player.instance.playerActor.AddHp(Player.instance.playerActor.HpMax);
            // strs[24]=().ToString();
        }
        //25：失去所有金钱
        if(effList.Contains(25))
        {
            Player.instance.AddGold(-100000);
            // strs[24]=().ToString();
        }
        //26：获得随机诅咒圣遗物（当前未持有）
        if(effList.Contains(26))
        {
            ShopData shopData = ShopManager.instance.GetInfo(9);
            AbilityData[] abilities= AbilityManager.instance.GetRandomAbilityFromSpecialPool(1,shopData._sellRelicList);
            AddAbilityForPlayerActor(abilities);
            strs[19]=(abilities[0].name).ToString();
        }
        //27：获得随机rank4当前未持有圣遗物
        if(effList.Contains(27))
        {
            AbilityData[] abilities= AbilityManager.instance.GetRandomAbility(1,4);
            AddAbilityForPlayerActor(abilities);
            strs[27]=(abilities[0].name).ToString();
        }
        //30：提升一级
        if(effList.Contains(30))
        {
            BattleScene.instance.ifLevelUp = true;
        }
        //31：无法再获得经验值
        if(effList.Contains(31))
        {
            Player.instance.ExpAdditonTimes =100;
            Player.instance.ExpAdditon =0;
        }
        if(effList.Contains(32))
        {
            
        }
        

    }
    void AddSkillForPlayerActor(SkillData[] skillDatas)
    {
        foreach (var item in skillDatas)
        {
            Player.instance.playerActor.UsingSkillsID.Add(item.id);
        }
    }
    void AddAbilityForPlayerActor(AbilityData[] abilities)
    {
        foreach (var item in abilities)
        {
            Player.instance.playerActor.abilities.Add(item.id);
        }
    }
    void OnClose()
    {
        //判断角色是否死亡
        if(Player.instance.playerActor.HpCurrent<=0||Player.instance.playerActor.HpMax<=0)
        {
            BattleScene.instance.ifDeadByBattle = false;
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
