using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    public int HpMax;
    public int HpCurrent;
    public Image ImgMax;
    public Image ImgCurrent;
    public Text HpText;
    public bool colorful;
    public bool CastingBar;
    public bool LikeBar;
    Skill skill;

    private int changeValue;//每次变化的值
    Timer timer;
    private Actor actor;//此条的拥有者
    void Awake()
    {
        timer =gameObject.AddComponent<Timer>();
        timer.autoDestory =false;
        timer.autoStart =false;
        timer.interval=0.1f;
    }
    void Start()
    {
        setHpBarNormal();
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
    
    }
    public void BindHPBar(Actor actor)//绑定HP条与角色
    {
        this.actor=actor;
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
    public void changeHPBar(int current,int max)
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
            HpText.text =string.Format("{0}/{1}",HpCurrent,HpMax);
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
    /// <summary> 改变血条 </summary>
    /// <param name="time">每0.05秒执行一次变化，共执行time秒</param>
    public void changeHPBar(float time)//每0.05秒执行一次变化，共执行time秒
    {
        changeValue =100/Mathf.CeilToInt(time/0.16f);
        // Debug.LogFormat("动作条开始了,间隔为{0}",changeValue);
        timer.start(0.16f,Mathf.CeilToInt(time/0.16f),onTimerInterval,onTimerComplete);        
    }
    public void stopChanging(bool immediately)
    // immediately=true：立即停止血条变化，数值停留在当前值；
    // immediately=false:数值停留在变化的最终值；
    {
        if(immediately)
        {

        }
        timer.stop();
        HpCurrent =0;
        setHpBarNormal();
        timer.onCompleteEvent =null;
        timer.onIntervalEvent =null;
    }
    void onTimerInterval(Timer timer)
    {
        HpCurrent+=changeValue;
        setHpBarNormal();
    }
    void onTimerComplete(Timer timer)
    {
        // Debug.LogFormat("动作条到了");
        timer.gameObject.GetComponent<Skill>();
        HpCurrent =100;
        setHpBarNormal();
        StartCoroutine(WaitForResetBar());
        
    }
    IEnumerator WaitForResetBar()
    {
        yield return new WaitForSeconds(0.1f);
        HpCurrent =0;
        setHpBarNormal();
        actor.OnTimerComplete(skill);
    }
}
