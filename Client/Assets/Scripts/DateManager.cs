using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

    public struct Date
    {
        public int year;
        public int month;       
        public int day;
        public float hours;
        public int totalDays;
    }
public class DateManager : MonoBehaviour
{
    public bool ifWorldStop =false;
    public static DateManager instance; 
    public Timer timer;
    public Date now =new Date();
    public static Date firstDay;
    //游戏内时间
    //现实1小时 = 游戏内1个月,//1s =1/120day =0.2hours;
    private int addSkillProficiency =0;//每10秒增加一次技能额外熟练度
    public int createTriggerInterval=5;
    int currentTriggerInterval=0;
    bool ifTriggerInterval =false;
    void Awake()
    {
        instance =this;
        timer = gameObject.AddComponent<Timer>();
        SetFirstDay();
        
    }
    static void SetFirstDay()
    {
        firstDay.year =210;
        firstDay.month =9;
        firstDay.day =1;
        firstDay.hours =0;
        firstDay.totalDays =75840;
        
    }
    public void StartTheWorld()
    {
        if(!timer)
        {
            timer = gameObject.AddComponent<Timer>();
        }
        timer.start(1,0,OnDate,null);
        ifWorldStop=false;
    }
    void OnDate(Timer timer)
    {
        // Debug.Log(now.hours);
        //日期增加，由Timer触发,每秒触发一次，增加0.2小时
        now.hours+=0.2f;
        //练习时使用本类的计数器来进行熟练度结算
        if(UIPractice.instance)
        {
            UIPractice.instance.OnTimerIn(timer);
            //每10秒提升额外熟练度的技能结算时间点
            addSkillProficiency++;
            if(addSkillProficiency==10)
            {
                addSkillProficiency=0;
                Debug.Log("提升技能熟练度");
            }
        }
        //尝试创建触发事件
        //如果现在还不是可以触发的时间，那么时间走
        if(!ifTriggerInterval)
        {
            currentTriggerInterval++;
        }
        //如果到了可以触发的时间，那么让触发时间的条件满足
        if(currentTriggerInterval==createTriggerInterval)
        {
            ifTriggerInterval=true;
        }
        //如果此时玩家挂机状态满足，那么判断时间是否满足
        //如果都满足，那么就尝试触发吧
        if(ifTriggerInterval&&Main.instance.UIState >0)
        {
            StartCoroutine(TryCreateEventOnDate());
        }


        if(now.hours>23.9f)
        {
            now.day++;
            now.hours =0;
            Debug.Log("日期增加了");
            if(UIPractice.instance)
            {
                UIPractice.instance.RefreashDate();
            }
            //每一天刷新人脉中的角色状态
            CharacterManager.instance.RefreashCharacterState();
            {
                if(now.day==31)
                {
                    now.month++;
                    now.day =1;
                    //每个月改变一次资产的折旧度
                    AssetsManager.instance.AssestsDecayLife(1);
                    if(now.month==13)
                    {
                        now.year++;
                        now.month =1;
                    }
                }
            }
            now.totalDays++;
            
            //从表中找到是否有临近的事件，有的话让EventManager把它加到事件列表里
            EventManager.TryCreateEventOnDate();
            //从事件列表中移除已过期的事件
            UIEvents.instance.TryMarkOverDueEvent();
            //寻找产生时间冲突的事件并标记
            UIEvents.instance.TryRelieveTimeConfict();
            //判断是否有预约事件，有的话世界时间停止
            //从预约列表里面找，预约时间是否等于当前时间
            TryFindDateEvents();
        }
    }
    public bool TryFindDateEvents()
    {
        bool f =false;
        foreach (var item in Player.instance.dateTaskEvents)
        {
            if(IfToday(item.data._startDate))
            {
                StopTheWorld();
                item.data._state =3;
                item.ChangeStateText();
                f=true;
                Main.instance.dateEventStopWorld =true;
                Main.instance.ShowDateEventRemind();
                if(UIEvents.instance)
                {
                    UIEvents.instance.information.SetActive(false);
                }
            }
        }
        return f;
    }
    IEnumerator TryCreateEventOnDate()
    {
        yield return new WaitForEndOfFrame();
        EventManager.TryCreateTriggerOnDate();
        ifTriggerInterval=false;
        currentTriggerInterval =0;
        createTriggerInterval+=5;
    }
    public void StopTheWorld()
    {
        //世界暂停
        Debug.Log("THE WORLD !");
        ifWorldStop =true;
        timer.stop();
        if(Player.instance.playerActor)
        {
            Player.instance.playerActor.StopCasting();
        }
    }

    public bool IfToday(Date d)
    {
        //判断今天是否是传入的那一天
        if(d.year==now.year&&d.month==now.month&&d.day==now.day)
        {
            return true;
        }
        return false;
    }
    public static int ConvertDateToInt(Date date)
    {
        int seconds =0;
        if(date.day==0)
        {
            return seconds;
        }
        int days = GetTotalDays(date);
        seconds =Mathf.FloorToInt(days*12/0.2f);
        return seconds;
    }
    ///<summary>将现实时间（秒）转化为游戏内小时</summary>
    public static float ConvertIntToGameHours(int seconds)
    {
        float hours = seconds*0.2f;
        return hours;
    }
    ///<summary>将现实世界(TimeSpan)转化位游戏内的小时</summary>
    public static float ConvertTimeSpanToDate(TimeSpan span)
    { 
        float hours =ConvertIntToGameHours((int)span.TotalSeconds);
        return hours;
    }
    ///<summary>将游戏世界中的天(持续时长)转化为现实中的秒Int</summary>
    public static int ConvertDayToIntSeconds(int day)
    {

        int seconds = Mathf.FloorToInt(day*12/0.2f);
        return seconds;

    }
    public bool IfExistTaskEvent(TimeSpan span)
    {

        return false;
    }
    public static Date ConvertStringToDate(string s)
    {
        Date date =new Date();
        if(s=="")
        {
            return date;
        }
        int y =int.Parse(s.Split('-')[0]);
        int m =int.Parse(s.Split('-')[1]);
        int d =int.Parse(s.Split('-')[2]);
        if(s.Split('-').Length>3)
        {
            float h =float.Parse(s.Split('-')[3]);
            date.hours =h;
        }
        date.day=d;
        date.month =m;
        date.year =y;
        return date;

    }
    ///<summary>判断一个日期距离今天是否在一年以内</summary>
    public  bool IfWithinOneYear(Date dt)
    {
        if(dt.day==0)
        {
            return true;
        }
        if(ConvertDateToInt(dt)-ConvertDateToInt(now)>12*3600)
        {
            return false;
        }
        else
        {
            return true;
        }
        
    }
    ///<summary>日期与0-1-1相比一共多少天</summary>
    public static int GetTotalDays(Date date)
    {
        Date dt =new Date();
        dt.year  =date.year;
        dt.month =date.month-1;
        dt.day =date.day-1;
        int days =dt.month*30+dt.year*360+dt.day;
        // date.totalDays =days-firstDay.totalDays;
        return days;
    }
    ///<summary>调整当前时间，并返回从上次下线到现在共经过了多少秒(int)</summary>
    public int AdjustNow(TimeSpan span)
    {
        
        string s = PlayerPrefs.GetString("gameDate");
        //计算方式 如果存在上次下线日期：
        //now =上次下线日期 + 时间间隔（秒）转为为day
        if(s!="")
        {
            Date lastDate = ConvertStringToDate(s);
            float lastHours = GetTotalHours(lastDate);
            float hours = ConvertTimeSpanToDate(span)+lastHours;
            now = ConvertHoursToDate(hours);
            now =EventManager.TryFindDateInDateEvents();
            hours =GetTotalHours(now);
            int realTimeSpan = Mathf.FloorToInt(hours-lastHours)*5;
            Debug.LogFormat("现在时间为：{0}-{1}-{2}-{3}",now.year,now.month,now.day,now.hours);
            return realTimeSpan;
        }
        //如果不存在上次下线日期
        else
        {
            now =firstDay;
            return 0;
        }
        
    
    }
    ///<summary> 判断是否存有预约事件到期</summary>
    public void AdjustDateAboutEvent()
    {
        
    }
    ///<summary>将游戏中的小时转化为Date</summary>
    public Date ConvertHoursToDate(float hours)
    {
        Date dt =new Date();
        if(hours<=0)
        {
            return dt;
        }
        dt.hours =hours%24;
        Debug.LogFormat("date.hours:{0},输入的hours ={1}",dt.hours,hours);
        int days =Mathf.FloorToInt(hours/24);
        dt.day=days%30 +1;
        int month =Mathf.FloorToInt(days/30);
        dt.month =month%12 +1;
        int year =Mathf.FloorToInt(month/12);
        dt.year =year;
        
        dt.totalDays =GetTotalDays(dt);
        
        return dt;
    }
    public float GetTotalHours(Date date)
    {
        //0-1-1-0  1-1-2-0  24
        //0-1-1-0  1-2-1-0  
        Date dt =new Date();
        dt.year  =date.year;
        dt.month =date.month-1;
        dt.day =date.day-1;
        dt.hours =date.hours;
        float hours =dt.month*30*24+dt.year*360*24+dt.day*24+dt.hours;
        // date.totalDays =days-firstDay.totalDays;

        return hours;
    }
    ///<summary>一个日期加上一个天数,取得一个新的日期</summary>
    public Date DateAddtionCompute(Date date,int day)
    {
        Date dt =new Date();
        //首先将date转化为小时
        float hours = GetTotalHours(date);
        //把小时相加
        hours+=day*24;
        //把小时转化为date
        dt =ConvertHoursToDate(hours);
        return dt;
    }
    ///<summary>给定的Date1和Date2比大小，true表示[>=]</summary>
    public static bool WhichDateFirst(Date d1,Date d2)
    {
        //将两个Date转为为day
        int r = GetTotalDays(d1)-GetTotalDays(d2);
        if(r>=0)
        {
            return true;
        }
        return false;
    }

}
