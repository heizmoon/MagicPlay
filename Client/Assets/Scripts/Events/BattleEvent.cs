using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BattleEvent : MonoBehaviour
{
    int currentBattle;

    public BattleData battleData;
    public EventTanser eventTanser;
    public static BattleEvent instance;
    GameObject skillChooseUI;
    public GameObject stageUI;
    Actor enemy;
    public Text TextBattle;
    public Text TextEnemyName;
    void Awake()
    {
        instance =this;
    }
    void Start()
    {
        this.transform.SetParent(Main.instance.allScreenUI);
        GetComponent<RectTransform>().sizeDelta = Vector2.zero;
		GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
        transform.localScale =Vector3.one;
        Player.instance.playerActor.HpCurrent =Player.instance.playerActor.HpMax;
        Player.instance.playerActor.MpCurrent =0;
        ShowSkillChooseUI();
    }

    // Update is called once per frame
     ///<summary>显示配置技能界面</summary>
    void ShowSkillChooseUI()
    {
        skillChooseUI =Instantiate((GameObject)Resources.Load("Prefabs/UISkillChoose"));
		skillChooseUI.transform.SetParent(Main.instance.allScreenUI);
		skillChooseUI.transform.localScale =Vector3.one;
		skillChooseUI.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
		skillChooseUI.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
        skillChooseUI.GetComponent<UISkillChoose>().confirmBTN.onClick.AddListener(OnClickSureBtn);
			
    }
    ///<summary>点击确认选择按钮</summary>
    public void OnClickSureBtn()
    {
        Debug.Log("执行");
        skillChooseUI.GetComponent<UISkillChoose>().SaveOldSkills();
        skillChooseUI.SetActive(false);
        Destroy(skillChooseUI.gameObject);
        OnBattle();
    }
    void OnBattle()
    {
        if(currentBattle>=battleData.MaxBattle)
        {
            //战斗结束
            OnBattleSuccess();
            return;
        }
        
        CreateEnemy(battleData.monsterList[currentBattle],battleData.level);
        
        ShowStageUI();
        if(UIBattle.Instance)
        {
            Player.instance.playerActor.transform.SetParent(Main.instance.BottomUI);
            Destroy(UIBattle.Instance.gameObject);
        }
        
        currentBattle++;
    }
    void CreateEnemy(int id,int level)
    {
        MonsterTypeData monster = MonsterManager.instance.GetInfo(id);
        
        //从怪物表中创建一个具体的敌人
        enemy =null;
        enemy =Instantiate((GameObject)Resources.Load("Prefabs/Enemy/"+monster.prefab)).GetComponent<Actor>();
        enemy.actorType =ActorType.敌人;
        enemy.level =level;
        enemy.InitEnemy(monster);
    }
    void ShowStageUI()
    {
        stageUI.SetActive(true);
        if(currentBattle==battleData.MaxBattle-1)
        {
            TextBattle.text = string.Format("最后一波");
        }
        else
        {
            TextBattle.text = string.Format("第{0}波",currentBattle+1);
        }
        TextEnemyName.text =string.Format("遭遇<color=#f22223>{0}</color>",enemy.actorName);
        Debug.LogFormat("遭遇<color=#f22223>{0}</color>",enemy.actorName);
        StartCoroutine(WaitForStageUI());
    }
    IEnumerator WaitForStageUI()
    {
        // Debug.LogWarningFormat("startWaitForStageUI");
        yield return new WaitForSeconds(1f);
        // Debug.LogWarningFormat("startInstantiateUIBattle");
        GameObject go =Instantiate((GameObject)Resources.Load("Prefabs/UIBattle"));
		go.transform.SetParent(Main.instance.allScreenUI);
		go.transform.localScale =Vector3.one;
		go.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
		go.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
        UIBattle.Instance.Init(enemy,battleData.scene);
        yield return new WaitForSeconds(1f);
        UIBattle.Instance.BattleBegin();
        stageUI.SetActive(false);

    }
    void OnBattleSuccess()
    {
        OnBattleEnd(1);
    }
    void OnBattleEnd(int result)
    {
        battleData.GetResult(result);
        DestroySelf();
    }
    public void GetBattleResult(int result)
    {
        if(result==1)
        {//战斗胜利
            
            OnBattle();
        }
        else
        {
            OnBattleEnd(2);           
        }
    }
    void DestroySelf()
    {
        Destroy(gameObject);
    }
}
