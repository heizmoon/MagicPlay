using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Data;


///<summary>做过的TriggerEvent</summary>
public struct doneTriggerEvents
{
    public int id;
    public int doneTime;
    ///<summary>事件的结果 0=未进行，1=成功，2=失败</summary>
    public int result;
    public Date _lastDate;
}
public struct doneTaskEvents
{
    public int id;
    public int doneTime;
    public int result;
    public Date _lastDate;

}
public class EventManager : MonoBehaviour
{
    //用来创建，管理事件

    static EventManager instance;
    static public List<int> eventList =new List<int>();
    
    // static public TaskEvent nowTask =null;
    public List<TaskEventsData> taskData;
    public List<doneTaskEvents> taskDones =new List<doneTaskEvents>();

    public List<TriggerEventsData> triggerData;
    public List<doneTriggerEvents> triggerDones =new List<doneTriggerEvents>();
    void Awake()
    {   
        instance =this;
        taskData = new List<TaskEventsData>();
        triggerData = new List<TriggerEventsData>();
        
    }

   

     

}
