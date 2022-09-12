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
