using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class UIChooseCharacter : MonoBehaviour
{
    public static UIChooseCharacter instance;
    GameObject _char1;
    GameObject _char2;
    GameObject _char3;

    Button _sureBtn;
    int nowCharacter =0;
    float mouseX;
    void Awake()
    {
        instance = this;
        _char1 =transform.Find("background/char1").gameObject;
        _char2 =transform.Find("background/char2").gameObject;
        _char3 =transform.Find("background/char3").gameObject;
        _sureBtn =transform.Find("background/SureButton").gameObject.GetComponent<Button>();
        _sureBtn.onClick.AddListener(OnChooseCharacter);
    }
    void Start()
    {
        ChangeCharacter();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnChooseCharacter()
    {
		// UIBasicBanner.instance.ChangeGoldText();
        string avaterName =CharacterManager.instance.GetInfo(1,"prefab");
        Actor playerActor = Instantiate((GameObject)Resources.Load("Prefabs/"+avaterName)).GetComponent<Actor>();
        Player.instance.playerActor =playerActor;
		playerActor.InitPlayerActor(CharacterManager.instance.GetCharacter(1));
        playerActor.transform.SetParent(Main.instance.BottomUI);
        playerActor.transform.localPosition =Vector3.zero;
        playerActor.transform.localScale =Vector3.one;
        //直接选择角色完毕，进入战斗流程
		BattleScene.instance.ChangeMap("Map_01");
        gameObject.SetActive(false);
        Destroy(gameObject);
    }
    public void OnPointerDown()
    {
        mouseX = Input.mousePosition.x;
    }
    public void OnPointerUp()
    {
        float tempX =Input.mousePosition.x;
        if(mouseX>tempX)
        {
            SwipeRight();
        }
        else if(mouseX<tempX)
        {
            SwipeLeft();
        }
    }
    void ChangeCharacter()
    {
        Debug.Log("当前"+nowCharacter);
        switch (nowCharacter)
        {
            case 0:
            _char1.GetComponent<CanvasGroup>().DOFade(1f,1f);
            _char2.GetComponent<CanvasGroup>().DOFade(0f,1f);
            _char3.GetComponent<CanvasGroup>().DOFade(0f,1f);
            break;
            case 1:
            _char1.GetComponent<CanvasGroup>().DOFade(0f,1f);
            _char2.GetComponent<CanvasGroup>().DOFade(1f,1f);
            _char3.GetComponent<CanvasGroup>().DOFade(0f,1f);
            break;
            case 2:
            _char1.GetComponent<CanvasGroup>().DOFade(0f,1f);
            _char2.GetComponent<CanvasGroup>().DOFade(0f,1f);
            _char3.GetComponent<CanvasGroup>().DOFade(1f,1f);
            break;
        }
    }
    public void SwipeRight()
    {
        if(nowCharacter>0)
        {
            nowCharacter--;
        }
        else
        {
            nowCharacter=2;
        }
        ChangeCharacter();
    }
    public void SwipeLeft()
    {
        if(nowCharacter>1)
        {
            nowCharacter=0;
        }
        else
        {
            nowCharacter++;
        }
        ChangeCharacter();
    }

}
