using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPractice : MonoBehaviour
{
    public static UIPractice instance;
    public bool enable;
    public Text dateText;
    public Text proficiencyText;
    public Text skillNameText;
    public int skillID;//正在练习的技能ID
    public Actor playerActor;
    // public Actor targetActor;
    public HPBar proficiencyBar;
    public Transform playerPosition;
    public Transform backPosition;
    public Transform frontPosition;
    public Transform targetPoint;
    void Awake()
    {
        instance = this;
        if(Battle.Instance==null)
        {
            Battle b =gameObject.AddComponent<Battle>();
        }
    }
    void Start()
    {


    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StartPractice(params int[] id)
    {
        gameObject.SetActive(true);
        playerPosition.gameObject.SetActive(true);
        playerActor =Player.instance.playerActor;
        playerActor.transform.SetParent(playerPosition);
        playerActor.transform.localPosition =Vector3.zero;
        playerActor.transform.localScale =Vector3.one;
        
        enable=true;
        if(id.Length>0)
        {
            skillID =id[0];
        }
        else
        {
            skillID =playerActor.UsingSkillsID[0];
        }
        int[] skills =new int[1]{skillID};
        playerActor.SetSkillList(skills);
        AddSkillProficiency(skillID,0);
        StartCoroutine(WaitForStart());

    }
    //隐藏式练级，当界面位于后方时，隐藏角色
    public void HidePracitce()
    {
        //角色停止动作
        playerActor.StopCasting();
        //移除角色身上的特效
        EffectManager.TryThrowInPool(playerActor.castPoint);
        EffectManager.TryThrowInPool(playerActor.hitPoint);
        enable =false;
        playerPosition.gameObject.SetActive(false);
        // EffectManager.ClearPool();
    }
    IEnumerator WaitForStart()
    {
        playerActor.InitActor();
        yield return new WaitForSeconds(0.3f);
        ContinueMagic();
        
    }
    public void ContinueMagic()
    {
        if(!Main.instance.dateEventStopWorld)
        {
            playerActor.animator.Play("idle");
            playerActor.InitMagic();
            playerActor.RunAI();
            // gameObject.GetComponent<Timer>().start(1,0,OnTimerIn,null);
            Player.instance.playerNow =string.Format("P,{0}",skillID);
            playerActor.GetActorSpellBar();
            skillNameText.text =string.Format("正在练习<color=#f22223>{0}</color>",SkillManager.instance.GetInfo(skillID,"name"));
        }
        else
        {
            skillNameText.text =string.Format("正在<color=#f22223>等待</color>");
        }    
    }
    public void StopPractice()
    {
        playerActor.StopCasting();
        enable =false;
        // gameObject.SetActive(false);
        // Destroy(gameObject);
    }
    public void UpdateSkillProficiency(int newCurrent,int newMax)
    {
      proficiencyBar.changeHPBar(newCurrent,newMax);
      proficiencyText.text =string.Format("LV{0}",Player.instance.GetSkillLevel(skillID));
    }
    public void OnTimerIn(Timer timer)
    {
        int newCurrent =0;
        int newMax =0;
        Player.instance.SetSkillProficiency(skillID,1,out newCurrent,out newMax);
        UpdateSkillProficiency(newCurrent,newMax);
    }
    ///<summary>增加技能熟练度</summary>
    ///<param name ="key">技能id</param>
    ///<param name ="num">增加的值</param>
    public void AddSkillProficiency(int key,int num)
    {
        int newCurrent =0;
        int newMax =0;
        // skillID =key;
        Player.instance.SetSkillProficiency(key,num,out newCurrent,out newMax);
        UpdateSkillProficiency(newCurrent,newMax);   
    }


    
}
