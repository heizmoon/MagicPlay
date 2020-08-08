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


    

}
