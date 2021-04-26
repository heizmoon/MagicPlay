using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BarEventArgs:EventArgs
{
    public bool IFComplete;
}
public class HPBar : MonoBehaviour
{
    public int HpMax;
    public float HpCurrent;
    public Image ImgMax;
    public Image ImgCurrent;
    public Text HpText;
    public bool colorful;
    public bool CastingBar;
    public bool LikeBar;
    public bool hasShadow;

    Skill skill;
    BarEventArgs barEventTrue;
    BarEventArgs barEventFalse;

    public Text armorText;
    public Image armorImage;

    float timeInterval;
    float timeCurrentInterval;
    float intervalPercent;

    private float changeValue;
    float changeInterval;
    Timer timer;
    private Actor actor;//此条的拥有者
    public bool playerActorMPBar;
    public bool showArmor;
    float mpchangeInterval =0;

    public event EventHandler onBarEvent;

    Image shadowImage;
    Image minImage;

    bool isChanging;
    GameObject coldFrame;
    Text coldNumber;
      
    void Awake()
    {
        // timer =gameObject.AddComponent<Timer>();
        // timer.autoDestory =false;
        // timer.autoStart =false;
        // timer.interval=0.1f;
        if(showArmor)
        {
            armorText.transform.parent.gameObject.SetActive(true);
            coldFrame =transform.Find("ColdFrame").gameObject;
            coldNumber =transform.Find("ColdFrame/ColdNumber").GetComponent<Text>();

        }
        if(hasShadow)
        {
            shadowImage =transform.Find("Shadow").GetComponent<Image>();
        }
        if(playerActorMPBar)
        {
            minImage =transform.Find("Min").GetComponent<Image>();
        }

        // if(!hasShadow)
        // {
        //     shadowImage.gameObject.SetActive(false);
        // }
        
    }
    void Start()
    {
        setHpBarNormal();
        barEventTrue =new BarEventArgs();
        barEventTrue.IFComplete =true;
        barEventFalse =new BarEventArgs();
        barEventFalse.IFComplete =false;
    }

    public void initHpBar(int Current,int Max)
    {
        if(CastingBar)
        {
            HpMax =100;
            HpCurrent =0;
        }
        else
        {
            HpCurrent =Current;
            HpMax =Max;
        }

        setHpBarNormal();
    }
    void Update()
    {
        if(isChanging)
        {
            if(timeCurrentInterval>=timeInterval)
            {
                ImgCurrent.fillAmount+=changeInterval;
                timeCurrentInterval=0;
                HpCurrent+= changeValue;
                if(actor.ifProtectSpell)
                ImgCurrent.color = Color.blue;
                else
                ImgCurrent.color = Color.yellow;
                if(ImgCurrent.fillAmount>=1)
                {
                    isChanging = false;
                    
                    HpCurrent =HpMax;
                    setHpBarNormal();
                    onChangeEnd();
                    StartCoroutine(WaitForResetBar());
                }
            }
            timeCurrentInterval+=Time.deltaTime;
        }
        if(playerActorMPBar)
        {
            // mpchangeInterval+= Time.deltaTime;
            // if(mpchangeInterval>Player.instance.playerActor.autoReduceMPAmount)
            // {
            //     changeHPBar(Player.instance.playerActor.MpCurrent,Player.instance.playerActor.MpMax);
            //     mpchangeInterval =0;
            // }
            changeHPBar(Player.instance.playerActor.MpCurrent,Player.instance.playerActor.MpMax);

        }
        if(showArmor&&actor)
        {
            armorText.text = actor.armor+"";
            if(actor.armor>0)
            armorImage.fillAmount = (actor.constArmorDecayTime-actor.ArmorAutoDecayTime)/actor.constArmorDecayTime;
            else
            armorImage.fillAmount = 0;
        }
        if(hasShadow)
        {

            StartCoroutine(MoveShadow());
            if(shadowImage.fillAmount<ImgCurrent.fillAmount)
            {
                shadowImage.fillAmount = ImgCurrent.fillAmount;
            }
        }
    }
    IEnumerator MoveShadow()
    {
        while(shadowImage.fillAmount>ImgCurrent.fillAmount)
        {
            shadowImage.fillAmount-=0.001f;
            yield return null;
        }
    }
    public void BindHPBar(Actor actor)//绑定HP条与角色
    {
        this.actor=actor;
         if(showArmor)
        ChangeCold();
    }
    public void BindHPBar(Skill skill)//绑定施法条与技能
    {
        this.skill =skill;
    }
    /// <summary> 改变血条 </summary>
    /// <param name="num">改变的数值</param>
    /// <param name="way">true → 改变血条值到num；false → 血条值减少num</param>
    public void changeHPBar(int num,bool way)//way：true → 改变血条值到num；false → 血条值减少num
    {
        if(way)
        {
            HpCurrent=num;         
        }
        else
        {
            HpCurrent-=num;
        }
        setHpBarNormal();
    }
    /// <summary> 改变血条 </summary>
    /// <param name="current">当前值</param>
    /// <param name="way">最大值</param>
    public void changeHPBar(float current,int max)
    {
        HpMax =max;
        HpCurrent =current;
        setHpBarNormal();
    }
    void setHpBarNormal()
    {
        if(HpMax<HpCurrent)
        {
           HpCurrent=HpMax;
        }
        if(!LikeBar)
        {
            HpText.text =string.Format("{0}/{1}",Mathf.FloorToInt(HpCurrent),HpMax);
        }
        float percent =((float)HpCurrent)/((float)HpMax);
        ImgCurrent.fillAmount=percent;
        if(colorful)
        {
            float r,g,b ;
            if(percent>0.67f)
            {
                r=0.33f/(percent-0.33f);
                g=1f;
                b=0f;
            }
            else if(percent>0.33f)
            {
                r=1f;
                g=(percent-0.33f)*3;
                b=0f;
            }
            else
            {
                r=(percent)*3f;
                g=0f;
                b=0f;
            }

            ImgCurrent.color =new Color(r,g,b,1);
        }
    }
    /// <summary> 改变施法条 </summary>
    /// <param name="time">每0.16秒执行一次变化，共执行time秒</param>
    public void changeHPBar(float time)//每0.05秒执行一次变化，共执行time秒
    {
        HpCurrent =0;
        HpMax = (int)time*100;
        changeInterval = 0.16f/time;
        // Debug.LogFormat("动作条开始了,间隔为{0}",changeValue);
        // timer.start(0.16f,Mathf.CeilToInt(time/0.16f),onTimerInterval,onTimerComplete); 
        timeInterval = 0.16f;
        changeValue =HpMax*changeInterval;
        isChanging=true;
        if(actor.ifProtectSpell)
        ImgCurrent.color = Color.blue;
        else
        ImgCurrent.color = Color.yellow;

    }
    public void DelayHPBar(float time)
    {
        if(!isChanging)
        {
            return;
        }
        HpMax+=(int)time*100;
        // changeValue =HpMax*changeInterval;
        changeInterval =changeValue/HpMax;
        ImgCurrent.fillAmount = HpCurrent/HpMax;
    }
    public void stopChanging(bool immediately)
    // immediately=true：立即停止血条变化，数值停留在当前值；
    // immediately=false:数值停留在变化的最终值；
    {
        if(immediately)
        {

        }
        // Debug.LogError("动作条被打断了");
        // timer.stop();
        isChanging=false;
        timeCurrentInterval=0;
        onChangeStop();
        HpCurrent =0;
        setHpBarNormal();
        
        // timer.onCompleteEvent =null;
        // timer.onIntervalEvent =null;
    }
    // void onTimerInterval(Timer timer)
    // {
    //     HpCurrent+=changeValue;
    //     setHpBarNormal();
    // }
    void onChangeEnd()
    {
        onBarEvent(this,barEventTrue);
    }
    void onChangeStop()
    {
        onBarEvent(this,barEventFalse);
    }
    // void onTimerComplete(Timer timer)
    // {
    //     // Debug.LogError("动作条到了");
        
    //     // timer.gameObject.GetComponent<Skill>();
    //     HpCurrent =100;
    //     setHpBarNormal();
    //     onChangeEnd();
    //     StartCoroutine(WaitForResetBar());
        
    // }
    
    IEnumerator WaitForResetBar()
    {
        yield return new WaitForSeconds(0.1f);
        HpCurrent =0;
        setHpBarNormal();
        // actor.OnTimerComplete(skill);
    }
    public void ChangeMin()
    {
        minImage.fillAmount = Player.instance.playerActor.manaMin/Player.instance.playerActor.MpMax;
    }
    public void ChangeCold()
    {
        if(actor.coldNum<=0)
        {
            coldFrame.SetActive(false);
            return;
        }
        coldFrame.SetActive(true);
        coldNumber.text = actor.coldNum.ToString();
        
    }

}
