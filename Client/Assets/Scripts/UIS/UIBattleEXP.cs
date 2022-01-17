using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

///<summary>用于显示经验获取，金钱获取</summary>
public class UIBattleEXP : MonoBehaviour
{
    GameObject goldFrame;
    GameObject expFrame;
    Image expBar;
    Text goldText;
    Text expText;
    int type;
    Button Btn_go;
    GameObject levelUpFrame;
    Transform levelUPBtnTs;
    Text levelText;
    Text HPMaxText;


    // Start is called before the first frame update
    void Awake()
    {
        goldFrame =transform.Find("background/goldFrame").gameObject;
        expFrame =transform.Find("background/expFrame").gameObject;
        expBar =transform.Find("background/expFrame/back/bar").GetComponent<Image>();
        goldText =transform.Find("background/goldFrame/goldText").GetComponent<Text>();
        expText =transform.Find("background/expFrame/Text").GetComponent<Text>();
        Btn_go = transform.Find("background/ButtonGoOn").gameObject.GetComponent<Button>();
        levelUpFrame =transform.Find("levelUPFrame").gameObject;
        levelUPBtnTs =transform.Find("levelUPFrame/Btn_Trans");
        levelText =transform.Find("levelUPFrame/Texts/LevelText").GetComponent<Text>();
        HPMaxText =transform.Find("levelUPFrame/Texts/HPMaxText").GetComponent<Text>();

        Btn_go.onClick.AddListener(OnButtonGoOn);
        Btn_go.gameObject.SetActive(false);
        levelUpFrame.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void Init(int steps,int type)
    {
        this.type =type;
        goldText.text =string.Format("{0}",steps*Configs.instance.battleLevelGold);

        ShowExpReward();
        UIBasicBanner.instance.RefeashText();
    }
    void ShowExpReward()
    {
        StartCoroutine(WaitForShowLevelUpFrame());
        expText.text = string.Format("等级:Lv{0}",Player.instance.playerActor.level);
        int startExp = BattleScene.instance.exp;
        int maxExp = CharacterManager.instance.GetLevelData(Player.instance.playerActor.level).exp;
        expBar.fillAmount = startExp*1f/maxExp;
        
        int addExp = Configs.instance.everyStepAddEXP;
        addExp=(int)(Player.instance.ExpAdditon*addExp);
        if(Player.instance.ExpAdditon>1)
        {
            Player.instance.ExpAdditonTimes--;
            if(Player.instance.ExpAdditonTimes ==0)
            {
                Player.instance.ExpAdditon =1;
            }
        }

        float endFill =0;
        if(startExp+addExp<maxExp)
        {
            endFill = (startExp+addExp)*1f/maxExp;
            expBar.DOFillAmount(endFill,0.6f);
            BattleScene.instance.exp+= addExp;
        }
        else
        {
            int maxExp2=CharacterManager.instance.GetLevelData(Player.instance.playerActor.level+1).exp;
            endFill = (startExp+addExp-maxExp)*1f/maxExp2;
            Tweener  tweener = expBar.DOFillAmount(1,0.4f);
            tweener.onComplete =delegate() 
            {
                expBar.fillAmount = 0;
			    expBar.DOFillAmount(endFill,0.3f);
                expText.text = string.Format("等级:<color=green>Lv{0}</color>",Player.instance.playerActor.level);
            };
            BattleScene.instance.exp= startExp+addExp-maxExp;
            BattleScene.instance.ifLevelUp = true;
        }
    }
    public static void CreateUIBattleEXP(int type)
    {
        GameObject go =(GameObject)Instantiate(Resources.Load("Prefabs/UIBattleEXP"));
        go.transform.SetParent(Main.instance.allScreenUI);
        go.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        go.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
        go.transform.localScale =Vector3.one;
        go.GetComponent<UIBattleEXP>().Init(BattleScene.instance.steps,type);
    }
    void OnButtonGoOn()
    {
        if(BattleScene.instance.ifLevelUp)//如果升级了，可以再次获得遗物
        UIBattleRewardRelic.CreateUIBattleRewardRelic(0);
        StartCoroutine(WaitForDestory());
    }
    IEnumerator WaitForShowLevelUpFrame()
    {
        yield return new WaitForSeconds(1f);
        Btn_go.gameObject.SetActive(true);
        if(BattleScene.instance.ifLevelUp)
        {
            levelUpFrame.gameObject.SetActive(true);
            Btn_go.transform.SetParent(levelUPBtnTs);
            Btn_go.transform.localPosition =Vector3.zero;
            Animation anim =GetComponent<Animation>();
            anim.Play();
            levelText.text = string.Format("Lv{0} → Lv{1}",Player.instance.playerActor.level,Player.instance.playerActor.level+1);
            HPMaxText.text = string.Format("最大生命值:{0} → {1}",Player.instance.playerActor.HpMax,Player.instance.playerActor.HpMax+CharacterManager.instance.GetLevelData(Player.instance.playerActor.level+1).addHPMax);
        }
    }
    IEnumerator WaitForDestory()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
