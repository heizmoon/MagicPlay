using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UIMain : MonoBehaviour
{
    public static UIMain instance;
    public Transform background;
    Vector3 orginPoint;
    public Vector2 moveSpeed;
    GameObject _terminal;
    GameObject _market;
    GameObject _spring;
    public Button _actor1;
    public Button _actor2;
    public Button _actor3;
    Animation _anim;
    Text _charName;
    Button _buttonGo;
    Button _buttonCancel;
    Button _buttonSwitchLeft;
    Button _buttonSwitchRight;
    Button _buttonOpenShop;
    Button _buttonOpenMatong;
    Button _buttonCrystal;


    public Button _buttonCloseShop;
    Button _buttonShopBuy;

    int charID;
    Text _crystalText;
    GameObject _ShopPanel;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        background=transform.Find("background");
        _ShopPanel=transform.Find("ActiveUIs/ShopPannel").gameObject;
        _ShopPanel.SetActive(false);
        _actor1 =transform.Find("background/actor_1").GetComponent<Button>();     
        _actor1.onClick.AddListener(delegate{OnPressActor(_actor1);});
        _actor2 =transform.Find("background/actor_2").GetComponent<Button>();     
        _actor2.onClick.AddListener(delegate{OnPressActor(_actor2);});
        _actor3 =transform.Find("background/actor_3").GetComponent<Button>();     
        _actor3.onClick.AddListener(delegate{OnPressActor(_actor3);});
        _anim =transform.Find("ActiveUIs").GetComponent<Animation>();
        _buttonGo =transform.Find("ActiveUIs/ButtonGo").GetComponent<Button>();
        _buttonGo.onClick.AddListener(OnChooseChar);
        _buttonGo.gameObject.SetActive(false);
        _buttonSwitchLeft =transform.Find("ActiveUIs/SwitchLeft").GetComponent<Button>();
        _buttonSwitchLeft.onClick.AddListener(SwipeLeft);
        _buttonSwitchLeft.gameObject.SetActive(false);
        _buttonSwitchRight =transform.Find("ActiveUIs/SwitchRight").GetComponent<Button>();
        _buttonSwitchRight.onClick.AddListener(SwipeRight);
        _buttonSwitchRight.gameObject.SetActive(false);
        _charName =transform.Find("ActiveUIs/CharName").GetComponent<Text>();
        _charName.gameObject.SetActive(false);
        _crystalText = transform.Find("ActiveUIs/upFrame/TextCrystal").GetComponent<Text>();
        _buttonOpenShop =transform.Find("background/Button_shop").GetComponent<Button>(); 
        _buttonOpenShop.onClick.AddListener(OnOpenShop);
        _buttonOpenMatong =transform.Find("background/Button_matong").GetComponent<Button>(); 
        _buttonOpenMatong.onClick.AddListener(OnOpenMatong);
        _buttonCloseShop =transform.Find("ActiveUIs/ShopPannel/block").GetComponent<Button>(); 
        _buttonCloseShop.onClick.AddListener(OnCloseShop);
        _buttonShopBuy =transform.Find("ActiveUIs/ShopPannel/ButtonBuy").GetComponent<Button>(); 
        _buttonShopBuy.onClick.AddListener(OnOpenBox);
        _buttonCancel = transform.Find("ActiveUIs/ButtonCancel").GetComponent<Button>();
        _buttonCancel.onClick.AddListener(OnCancelChooseChar);
        _buttonCancel.gameObject.SetActive(false);
        _buttonCrystal = transform.Find("ActiveUIs/upFrame/ButtonCrystal").GetComponent<Button>();
        _buttonCrystal.onClick.AddListener(OnTapCrystal);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Init()//初始化：有哪些地图元素可用，哪些还不可用
    {
        //MainFunction{2,0,1}   功能1：可用，功能2：不可用，功能3：可建造
        //功能：终端，黑市，泉水
        string[] mainFunction = PlayerPrefs.GetString("MainFunction").Split(',');
        if(int.Parse(mainFunction[0])==0)
        {
            _terminal.SetActive(false);
        }
        if(int.Parse(mainFunction[1])==0)
        {
            _market.SetActive(false);
        }
        if(int.Parse(mainFunction[2])==0)
        {
            _spring.SetActive(false);
        }
    }
    public void IntoMain()
    {
        GetComponent<Animation>().Play("UI_UIMain_into");
    }
    public void DragMap()
    {
        
        Vector3 currentPoint  =Input.mousePosition;
        Vector3 moveDir = currentPoint - orginPoint;
        moveDir = new Vector3(moveDir.x, moveDir.y, 0);
        if (moveDir == Vector3.zero)
        {
            return;
        }
        
        // moveDir.x = moveDir.x / Screen.width*moveSpeed.x;
        // moveDir.y = moveDir.y / Screen.height*moveSpeed.y;
        moveDir.x = moveDir.x /moveSpeed.x;
        moveDir.y = moveDir.y /moveSpeed.y;
        background.Translate(moveDir);
        if(background.localPosition.x>420)
        {
            background.localPosition =new Vector3(420,background.localPosition.y,0);
        }
        if(background.localPosition.x<-420)
        {
            background.localPosition =new Vector3(-420,background.localPosition.y,0);
        }
        if(background.localPosition.y>350)
        {
            background.localPosition =new Vector3(background.localPosition.x,350,0);
        }
        if(background.localPosition.y<-350)
        {
            background.localPosition =new Vector3(background.localPosition.x,-350,0);
        }
        if (orginPoint != currentPoint)
        {
            orginPoint = currentPoint;
        }
    }
    public void BeginDrag()
    {
        orginPoint = new Vector3(Input.mousePosition.x,Input.mousePosition.y,0);
        Debug.Log("orginPoint="+orginPoint);
        
    }
    public void Refeash()
    {
        _crystalText.text =Player.instance.Crystal.ToString();
        //更新事件显示相关
    }
    void OnPressActor(Button button)
    {
        /*
        如果角色当前身上有时间，那么优先触发事件
        否则进入角色选择界面
        */
        Debug.Log("触发");
        TriggerChooseActor(button.transform);
        _actor1.GetComponent<MovingActor>().Stop();
        _actor2.GetComponent<MovingActor>().Stop();
        _actor3.GetComponent<MovingActor>().Stop();    
    }
    public void StopActors(bool ifStop)
    {
        if(ifStop)
        {
            _actor1.GetComponent<MovingActor>().Stop();
            _actor2.GetComponent<MovingActor>().Stop();
            _actor3.GetComponent<MovingActor>().Stop();
        }
        else
        {
            _actor1.GetComponent<MovingActor>().Resume();
            _actor2.GetComponent<MovingActor>().Resume();
            _actor3.GetComponent<MovingActor>().Resume();
        }
    }
    void TriggerChooseActor(Transform _transform)
    {
        switch(_transform.name)
        {
            case "actor_1":
            charID =1;
            break;
            case "actor_2":
            charID =2;
            break;
            case "actor_3":
            charID =3;
            break;
        }
        PanCameraToObject(_transform);
        FocusActor();
        _charName.text =CharacterManager.instance.GetInfo(charID,"name");     
    }
    void PanCameraToObject(Transform _transform)
    {
        background.DOLocalMove(-_transform.localPosition,0.5f,false);
    }
    void FocusActor()
    {
        _anim.Play("UI_Main_intoChooseChar");
    }
    void SwitchActor()
    {
        Button button =_actor1;
        switch(charID)
        {
            case 1:
            button = _actor1;
            break;
            case 2:
            button = _actor2;
            break;
            case 3:
            button = _actor3;
            break;
        }
        TriggerChooseActor(button.transform);
    }
    void OnChooseChar()
    {
        string mapName ="Map_01";
        if(Configs.instance.useTestMapName!="")
        {
            mapName=Configs.instance.useTestMapName;
        }
        OnChooseCharacter(charID,mapName);
    }
    public static void OnChooseCharacter(int charID,string mapName)
    {
		// UIBasicBanner.instance.ChangeGoldText();
        string avaterName =CharacterManager.instance.GetInfo(charID,"prefab");
        Actor playerActor = Instantiate((GameObject)Resources.Load("Prefabs/"+avaterName)).GetComponent<Actor>();
        playerActor.actorType =ActorType.玩家角色;
        Player.instance.playerActor =playerActor;
		playerActor.InitPlayerActor(CharacterManager.instance.GetCharacter(charID));
        Player.instance.CharID =charID;
        playerActor.transform.SetParent(Main.instance.BottomUI);
        playerActor.transform.localPosition =Vector3.zero;
        playerActor.transform.localScale =Vector3.one;
        //直接选择角色完毕，进入战斗流程
		BattleScene.instance.ChangeMap(mapName);
        //获得初始金币
        Player.instance.AddGold(Configs.instance.initGold);
        if(UIMain.instance)
        {
            UIMain.instance.gameObject.SetActive(false);
            Destroy(UIMain.instance.gameObject);
        }
        
    }
    void OnTapCrystal()
    {
        //点击水晶图标，提示水晶可以用于...
        Debug.Log("点击水晶图标，提示水晶可以用于...");
    }
    void OnCancelChooseChar()
    {
        _anim.Play("UI_Main_leaveChooseChar");
    }
    public void SwipeRight()
    {
        if(charID>1)
        {
            charID--;
        }
        else
        {
            charID=3;
        }
        SwitchActor();
    }
    public void SwipeLeft()
    {
        if(charID>2)
        {
            charID=1;
        }
        else
        {
            charID++;
        }
        SwitchActor();
    }
    #region
    void OnOpenShop()
    {
        _ShopPanel.SetActive(true);
    } 
    void OnCloseShop()
    {
        _ShopPanel.SetActive(false);
    }
    void OnOpenBox()
    {
        //箱子中能开出哪些东西？
        Player.instance.AddCrystal(10);
        UnlockSkill();
    }
    public void UnlockSkill()
    {
        //从所有没有解锁的技能中随机一个出来
        SkillData[] skillDatas= SkillManager.instance.GetRandomSkillFromLockSkill(1);
        if(skillDatas==null)
        {
            Debug.LogError("所有技能都已经被解锁");
            return;
        }
        for (int i = 0; i < skillDatas.Length; i++)
        {
            Player.instance.UnlockSkill(skillDatas[i].id);
        }
    }
    public void UnlockAbility()
    {
        AbilityData[] abilities= AbilityManager.instance.GetRandomAbilityFromLockAbility(1);
        if(abilities==null)
        {
            Debug.LogError("所有遗物都已经被解锁");
            return;
        }
        for (int i = 0; i < abilities.Length; i++)
        {
            Player.instance.UnlockAbility(abilities[i].id);
        }
    }
    #endregion
    void OnOpenMatong()
    {
        Perform.LoadPerform("matong");
    }
}
