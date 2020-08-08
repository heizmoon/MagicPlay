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
    static public TriggerEvent nowTrigger =null;
    
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

        LoadInfo();
    }

    ///<summary>从表中读取静态数据后，动态处理一些跟日期相关的动态数据</summary>
    void HandleDynamicInfo()
    {
        //--------------trigger
        for (int i = 0; i < triggerData.Count; i++)
        {
            TriggerEventsData td =triggerData[i];
            td._lastDate =GetLastTimeFromTrigger(td.id);
            td.doneTime =GetEventDoneTimesFromTirgger(td.id);
            td.result =GetEventDoneResultFromTrigger(td.id);
            td._earlyDate =GetEarlyDate(td.earlyDate);
            td._lateDate =GetLateDate(td.lateDate);
            triggerData[i] =td;
        }
        //-----------------task
        for (int i = 0; i < taskData.Count; i++)
        {
            TaskEventsData td =taskData[i];
            if(td.type==1)
            {
                td._startDate = DateManager.ConvertStringToDate(td.startDate);
                td._endDate =DateManager.instance.DateAddtionCompute(td._startDate,td.timeCost);
            }
            td._lastDate =GetLastTimeFromTask(td.id);
            

            td._state =GetEventState(td.id);
            td.doneTime =GetEventDoneTimesFromTask(td.id);
            // Debug.LogWarningFormat("事件【{0}】的完成次数为：{1}",td.eventName,td.doneTime);
            td.result =GetEventDoneResultFromTask(td.id);
            if(td.ifDateLock)
            {
                td._state =2;
            }
        }
       
    }
    //加载记录表中信息
    public void LoadInfo()
    {   
        LoadDoneInformation(); 
        TaskEventDataSet ts = Resources.Load<TaskEventDataSet>("DataAssets/TaskEvent");
        foreach (var item in ts.dataArray)
        {
            taskData.Add(item);
        }        
        TriggerEventDataSet gs = Resources.Load<TriggerEventDataSet>("DataAssets/TriggerEvent");
        foreach (var item in gs.dataArray)
        {
            triggerData.Add(item);
        }
        HandleDynamicInfo();
     
    }
    void LoadDoneInformation()
    {
        LoadTriggerDoneInformation();
        LoadTaskDoneInformation();
    }
    void LoadTriggerDoneInformation()
    {
        
        string[] ss = PlayerPrefs.GetString("doneTriggerEvents").Split('|');
        if(ss[0]=="")
        {
            return;
        }
            
        for (int i = 0; i < ss.Length; i++)
        {
            doneTriggerEvents done =new doneTriggerEvents();
            done.id= int.Parse(ss[i].Split(',')[0]);
            done.doneTime =int.Parse(ss[i].Split(',')[1]);
            done.result =int.Parse(ss[i].Split(',')[2]);
            done._lastDate.year =int.Parse(ss[i].Split(',')[3].Split('-')[0]);
            done._lastDate.month =int.Parse(ss[i].Split(',')[3].Split('-')[1]);
            done._lastDate.day =int.Parse(ss[i].Split(',')[3].Split('-')[2]);
            triggerDones.Add(done);
        }

    }
    void LoadTaskDoneInformation()
    {
        string[] ss = PlayerPrefs.GetString("doneTaskEvents").Split('|');
        if(ss[0]=="")
        {
            return;
        }
            
        for (int i = 0; i < ss.Length; i++)
        {
            doneTaskEvents done =new doneTaskEvents();
            done.id= int.Parse(ss[i].Split(',')[0]);
            done.doneTime =int.Parse(ss[i].Split(',')[1]);
            done.result =int.Parse(ss[i].Split(',')[2]);
            done._lastDate.year =int.Parse(ss[i].Split(',')[3].Split('-')[0]);
            done._lastDate.month =int.Parse(ss[i].Split(',')[3].Split('-')[1]);
            done._lastDate.day =int.Parse(ss[i].Split(',')[3].Split('-')[2]);
            taskDones.Add(done);
        }
    }
    public int GetEventDoneTimesFromTirgger(int id)
    {
        //从dones中获取该事件已经进行了几次
        
        if(triggerDones.Count==0)
        {
            return 0;
        }
        foreach (var item in triggerDones)
        {
            if(id ==item.id)
            {
                return item.doneTime;
            }
            
        }
        return 0;    
    }
    public int GetEventDoneTimesFromTask(int id)
    {
        //从dones中获取该事件已经进行了几次
        
        if(taskDones.Count==0)
        {
            return 0;
        }
        foreach (var item in taskDones)
        {
            if(id ==item.id)
            {
                return item.doneTime;
            }
            
        }
        return 0;    
    }
    

    //获取上次进行该事件的日期
    Date GetLastTimeFromTrigger(int id)
    {
        Date date =new Date();
        if(triggerDones.Count==0)
        {
            return date;
        }
        foreach (var item in triggerDones)
        {
            if(id ==item.id)
            {
                date =item._lastDate;
            }    
        }
        return date;
    }
    Date GetLastTimeFromTask(int id)
    {
        Date date =new Date();
        if(taskDones.Count==0)
        {
            return date;
        }
        foreach (var item in taskDones)
        {
            if(id ==item.id)
            {
                date =item._lastDate;
            }    
        }
        return date;
    }
    ///<summary>获取一个事件当前的状态文字</summary>
    public int GetEventState(int id)
    {
        int s =0;
        string[] ss = PlayerPrefs.GetString("taskEventsInList").Split('|');
        if(ss[0]=="")
        {
            return 1;
        }
        foreach (var item in ss)
        {
            if(int.Parse(item.Split(',')[0]) ==id)
            {
                s =int.Parse(item.Split(',')[1]);
            }
        }
        return s;
    }
    Date GetEarlyDate(string s)
    {
        Date date =new Date();
        if(s =="")
        {
            return date;
        }
        else
        {
            return DateManager.ConvertStringToDate(s);
        }
    }
    Date GetLateDate(string s)
    {
        Date date =new Date();
        if(s =="")
        {
            date.year=9999;
            date.month=1;
            date.day=1;
            return date;
        }
        else
        {
            return DateManager.ConvertStringToDate(s);
        }
    }

    public int GetEventDoneResultFromTask(int id)
    {
        if(taskDones.Count==0)
        {
            return 0;
        }
        foreach (var item in taskDones)
        {
            if(id ==item.id)
            {
                return item.result;
            }
            
        }
        return 0;
    }
    public int GetEventDoneResultFromTrigger(int id)
    {
        if(triggerDones.Count==0)
        {
            return 0;
        }
        foreach (var item in triggerDones)
        {
            if(id ==item.id)
            {
                return item.result;
            }
            
        }
        return 0;
    }

    public TaskEventsData GetTaskInfo(int id)
    {
        TaskEventsData task =new TaskEventsData();
        foreach(var item in taskData)
        {
            if(item.id==id)
            {
             return item;       
            } 
        }
        return task;
    }
















    //判断一个事件是否可以被创建出来

    //当时间进行到某时期时，从事件列表中检索该时间点有哪些可创建的事件
    public static void TryCreateEventOnDate()//每120s执行一次，大量逻辑运算可能会造成该帧严重变慢
    {
        Debug.Log("开始判断是否创建任务！");
        foreach (var item in EventManager.instance.taskData)
        {
            //是否事件列表中已经有了该事件
            if(eventList.Contains(item.id))
            {
                continue;
            }
            //事件是世界事件且开始日期小于当前日期
            if(item.type==1&&DateManager.ConvertDateToInt(item._startDate)-DateManager.ConvertDateToInt(DateManager.instance.now)<0)
            {
                continue;
            }
            //事件开始日期距离现在小于12小时
            if(!DateManager.instance.IfWithinOneYear(item._startDate))
            {
                continue;
            }
            //这个事件是否有【次数限制】，并且已经到达限制的次数
            if(item.timeLimit>0&&IfEventTimesLimit(item))
            {
                continue;
            }
            //角色是否满足触发该事件的【级别限制】
            if(Player.instance.rank<item.rankLimit)
            {
                continue;
            }
            //角色是否是【职业限制】中的一种或多种---------此处需要修改剧本
            if(item.professionLimit!=""&& Player.instance.playerProfessions.Intersect(item._professionLimit).ToList().Count<=0)
            {
                continue;
            }
            //角色是否已成功做完该事件的【成功前置事件】
            List<int> failEvents =new List<int>();
            if(item.successEvents!=""&&!item._successEvents.All(t =>GetSuccessEvents(out failEvents).Any(b =>b==t)))
            {
                continue;
            }
            //角色是否已失败做完该事件的【失败前置事件】
            if(item.failEvents!=""&&!item._failEvents.All(t =>failEvents.Any(b =>b==t)))
            {
                continue;
            }
            //对于可以触发多次的事件，上次触发的时间到此时，是否已经大于【触发时间间隔】
            if(item.timeLimit>0&&DateManager.ConvertDateToInt(DateManager.instance.now)-DateManager.ConvertDateToInt(item._lastDate)<DateManager.ConvertDayToIntSeconds(item.timeInterval))
            {
                continue;
            }
            //角色是否拥有【资产要求列表】
            if(item.assetsList!=""&&Player.instance.playerAssests.Intersect(item._assetsList).ToList().Count==0)
            {
                continue;
            }
            //角色是否没有【资产要求列表】
            if(Player.instance.playerAssests.Intersect(item._noAssetslist).ToList().Count>0)
            {
                continue;
            }
            //角色的【资产总价值】大于/小于【资产总价值要求】
            if(item.assetsTotalValueGreater!=0&&Player.instance.gold<item.assetsTotalValueGreater)
            {
                continue;
            }
            if(item.assetsTotalValueLess!=0&&Player.instance.gold>=item.assetsTotalValueLess)
            {
                continue;
            }
            //角色的某几项技能等级大于/小于【技能等级要求】
            if(!AnalysisSkillList(item.skillList))
            {
                continue;
            }
            //角色的【某系技能总等级】大于/小于【技能总等级要求】
            if(!AnalysisSkillTotalLevel(item.skillTotalLevelList))
            {
                continue;
            }
            //角色的深渊层级大于【深渊层级要求】
            if(Player.instance.playerAbyss<item.abyssGreater)
            {
                continue;
            }
            //角色的深渊层级小于【深渊层级要求】
            if(item.abyssLess!=0&&Player.instance.playerAbyss>=item.abyssLess)
            {
                continue;
            }
            //角色的现金大于【现金要求】
            if(item.cashGreater!=0&&Player.instance.gold<item.cashGreater)
            {
                continue;
            }
            //角色的现金小于【现金要求】
            if(item.cashLess!=0&&Player.instance.gold>=item.cashLess)
            {
                continue;
            }
            //经过层层筛选，最终创建了事件
            UIEvents.instance.CreateTaskEvent(item,false);
            eventList.Add(item.id);
            Debug.Log("经过层层筛选，最终创建了事件！");
        }
    }
    static bool IfEventTimesLimit(TaskEventsData data)
    {
        if(data.doneTime>=data.timeLimit)
        {
            return true;
        }
        return false;
    }
    static bool IfEventTimesLimit(TriggerEventsData data)
    {
        if(data.doneTime>=data.timeLimit)
        {
            return true;
        }
        return false;
    }

//获取玩家已经完成了的事件列表,每次都重新计算，可能会影响效率----------------
    static List<int> GetSuccessEvents(out List<int> failList)
    {
        // failEvents ="";
        string[] ss = PlayerPrefs.GetString("doneTaskEvents").Split('|');
        List<int> successList =new List<int>();
        failList =new List<int>();
        if(ss[0]=="")
        {
            return successList;
        }
        foreach (var item in ss)
        {
            if(item.Split(',')[2]=="1")
            {
                //成功事件
                successList.Add(int.Parse(item.Split(',')[0]));
            }
            else if(item.Split(',')[2]=="2")
            {
                //失败事件
                // failEvents+=",";
                // failEvents +=item.Split(',')[0];
                failList.Add(int.Parse(item.Split(',')[0]));
            }
        }
        return successList;
    }
    //判断是否满足技能列表需求
    static bool AnalysisSkillList(string s)
    {
        if(s =="")
        {
            return true;
        }
        string [] ss =s.Split('|');
        int id =0;
        int level =0;
        foreach (var item in ss)
        {
            id = int.Parse(item.Split(',')[0]) ;
            level = int.Parse(item.Split(',')[2]) ;
            //如果要求是>
            if(item.Split(',')[1] ==">")
            {
                if(Player.instance.GetSkillLevel(id)<level)
                {
                    return false;
                }
            }
            else
            {
                if(Player.instance.GetSkillLevel(id)>=level)
                {
                    return false;
                }
            }
        }
        return true;
    }
    static bool AnalysisSkillTotalLevel(string s)
    {
        if(s =="")
        {
            return true;
        }
        string [] ss =s.Split('|');
        int genre ;
        int level ;
        foreach (var item in ss)
        {
            genre = int.Parse(item.Split(',')[0]) ;
            level = int.Parse(item.Split(',')[2]) ;
            //如果要求是>
            if(item.Split(',')[1] ==">")
            {
                if(SkillManager.instance.totalLevel[genre]<level)
                {
                    return false;
                }
            }
            else
            {
                if(SkillManager.instance.totalLevel[genre]>=level)
                {
                    return false;
                }
            }
        }
        return true;
    }
    public static void SaveEventState()
    {
        //存储格式：事件id,进行次数，完成状况，上次完成日期|事件id,进行次数，完成状况，上次完成日期
        // PlayerPrefs.SetString("doneTaskEvents","1,1,0,210-1-1,0|2,10,1,210-2-2,1");
        if(EventManager.instance.taskDones.Count==0)
        {
            return;
        }
        string s ="";
        foreach (var item in EventManager.instance.taskDones)
        {
            s = s+"|"+item.id+","+item.doneTime+","+item.result+","+item._lastDate.year+"-"+item._lastDate.month+"-"+item._lastDate.day;
            // Debug.LogWarningFormat("此时S={0}",s);
        }
        s =s.Remove(0,1);
        PlayerPrefs.SetString("doneTaskEvents",s);
        Debug.LogWarningFormat("保存的完成事件列表为：{0}",s);
    }
    ///<summary>新增一项完成的事件</summary>
    public static void AddDoneEvents(TaskEventsData data)
    {
        doneTaskEvents done =new doneTaskEvents();
        done.id =data.id;
        done.result =data.result;
        done.doneTime =data.doneTime;
        done._lastDate =data._lastDate;
        EventManager.instance.taskDones.Add(done);
        // Debug.LogWarningFormat("新增一个完成事件{0}",done.id);
    }
    public static void AddDoneEvents(TriggerEventsData data)
    {
        doneTriggerEvents done =new doneTriggerEvents();
        done.id =data.id;
        done.result =data.result;
        done.doneTime =data.doneTime;
        done._lastDate =data._lastDate;
        EventManager.instance.triggerDones.Add(done);
    }
    ///<summary>修改事件的完成情况</summary>
    ///<param name ="ifTimeLimit">true=达到最大次数时删除事件，false=不删除</param>
    public static void ChangeDoneEvents(TaskEventsData data,bool ifTimeLimit)
    {
        
        int num =0;
        for (int i = 0; i < EventManager.instance.taskDones.Count; i++)
        {
            if(EventManager.instance.taskDones[i].id == data.id)
            {
                doneTaskEvents done =EventManager.instance.taskDones[i];
                done.result =data.result;
                done.doneTime =data.doneTime;
                done._lastDate =data._lastDate;
                EventManager.instance.taskDones[i] =done;
                num++;
            }
        }
        if(num==0)
        {
            AddDoneEvents(data);
        }
        if(!ifTimeLimit)
        {
            return;
        }
        //如果事件达到最大次数限制，那么在列表中移除该事件
        if(data.timeLimit>0&&data.doneTime>=data.timeLimit)
        {
            UIEvents.instance.RemoveTaskEvent(data);
        }
        Debug.LogWarningFormat("改变完成事件{0},次数为{1},结果为{2}",data.id,data.doneTime,data.result);
    }
    public static void ChangeDoneEvents(TriggerEventsData data)
    {
        int num =0;
        for (int i = 0; i < EventManager.instance.triggerDones.Count; i++)
        {
            if(EventManager.instance.triggerDones[i].id == data.id)
            {
                doneTriggerEvents done =EventManager.instance.triggerDones[i];
                done.result =data.result;
                done.doneTime =data.doneTime;
                done._lastDate =data._lastDate;
                num++;
            }
        }
        if(num==0)
        {
            AddDoneEvents(data);
        }
    }
    //UIEvnets在初始化时，直接读取已存储的事件列表，而不再通过OnDate去筛选创建事件
    public static void CreateTaskEvents()
    {

        string[] ss= PlayerPrefs.GetString("taskEventsInList").Split('|');
        if(ss[0]=="")
        {
            return;
        }
        foreach (var item in ss)
        {
            TaskEventsData data = EventManager.instance.GetTaskInfo(int.Parse(item.Split(',')[0]));
            data._state =int.Parse(item.Split(',')[1]);
            bool conflict =(item.Split(',')[2]=="1"?true:false);
            UIEvents.instance.CreateTaskEvent(data,conflict);
            eventList.Add(data.id);
        }
        
    }
    public static string SaveDateEvents()
    {
        string s ="";
        if(Player.instance.dateTaskEvents.Count==0)
        {
            return s;
        }
        foreach (var item in Player.instance.dateTaskEvents)
        {
            s+="|";
            s+= item.data.id+","+item.data._startDate.year+"-"+item.data._startDate.month+"-"+item.data._startDate.day;
            
        }
        s =s.Remove(0,1);
        Debug.LogFormat("保存的预约事件列表为：{0}",s);

        return s;
    }
    ///<summary>将事件列表中的事件和其状态转化为string,以便保存到playerprefs</summary>
    public static string SaveTaskEventsInList()
    {
        string s ="";
        if(UIEvents.instance.taskEvents.Count==0)
        {
            return s;
        }
        foreach (var item in UIEvents.instance.taskEvents)
        {
            s+="|";
            s+= item.data.id+","+item.data._state+","+ (item.conflict?"1":"0");
            
        }
        s =s.Remove(0,1);
        return s;
    }
    ///<summary>从已预约的事件中，找到一个日期</summary>
    public static Date TryFindDateInDateEvents()
    {
        string[] ss= PlayerPrefs.GetString("playerDateEvents").Split('|');
        if(ss[0]=="")
        {
            return DateManager.instance.now;
        }
        List<float> dates =new List<float>();
        foreach (var item in ss)
        {
            //事件为已预约事件
            Date d = DateManager.ConvertStringToDate(item.Split(',')[1]);
            dates.Add(DateManager.instance.GetTotalHours(d));    
        }
        dates.Add(DateManager.instance.GetTotalHours(DateManager.instance.now));
        //比较所有已预约事件和now中最小的那个时间
        float min = dates.Min();
        Date date =DateManager.instance.ConvertHoursToDate(min);
        return date;
    }
    static int GetTriggerResult(int id)
    {
        return EventManager.instance.GetEventDoneResultFromTrigger(id);
    }
    public static void TryCreateTriggerOnDate()
    {
        if(nowTrigger)
        {
            return;
        }
        foreach (var item in EventManager.instance.triggerData)
        {
            //这个事件触发的场景是否包含当前挂机场景
            string[] scenes =item.scene.Split(',');
            if(!scenes.Contains(Main.instance.UIState.ToString()))
            {
                continue;
            }
            //这个事件是否有【成功前置触发要求】,并且前置触发是否成功
            if(item.successTrigger>0&&GetTriggerResult(item.successTrigger)!=1)
            {
                continue;
            }
            if(item.failTrigger>0&&GetTriggerResult(item.failTrigger)!=2)
            {
                continue;
            }
            //这个事件是否有【前置完成触发要求】，且前置触发是否完成（无论成功与失败）
            if(item.doneTrigger>0&&GetTriggerResult(item.doneTrigger)==0)
            {
                continue;
            }
            //当前日期大于最早开始日期
            if(DateManager.WhichDateFirst(item._earlyDate,DateManager.instance.now))
            {
                continue;
            }
            if(DateManager.WhichDateFirst(DateManager.instance.now,item._lateDate))
            {
                continue;
            }
            //这个事件是否有【次数限制】，并且已经到达限制的次数
            if(item.timeLimit>0&&IfEventTimesLimit(item))
            {
                continue;
            }
            //角色是否满足触发该事件的【级别限制】
            if(Player.instance.rank<item.rankLimit)
            {
                continue;
            }
            //角色是否是【职业限制】中的一种或多种---------此处需要修改剧本
            if(item.professionLimit!=""&& Player.instance.playerProfessions.Intersect(item._professionLimit).ToList().Count<=0)
            {
                continue;
            }
            //角色是否已成功做完该事件的【成功前置事件】
            List<int> failEvents =new List<int>();
            if(item.successEvents!=""&&!item._successEvents.All(t =>GetSuccessEvents(out failEvents).Any(b =>b==t)))
            {
                continue;
            }
            //角色是否已失败做完该事件的【失败前置事件】
            if(item.failEvents!=""&&!item._failEvents.All(t =>failEvents.Any(b =>b==t)))
            {
                continue;
            }
            //对于可以触发多次的事件，上次触发的时间到此时，是否已经大于【触发时间间隔】
            if(item.timeLimit>0&&DateManager.ConvertDateToInt(DateManager.instance.now)-DateManager.ConvertDateToInt(item._lastDate)<DateManager.ConvertDayToIntSeconds(item.timeInterval))
            {
                continue;
            }
            //角色是否拥有【资产要求列表】
            if(item.assetsList!=""&&Player.instance.playerAssests.Intersect(item._assetsList).ToList().Count==0)
            {
                continue;
            }
            //角色是否没有【资产要求列表】
            if(Player.instance.playerAssests.Intersect(item._noAssetslist).ToList().Count>0)
            {
                continue;
            }
            //角色的【资产总价值】大于/小于【资产总价值要求】
            if(item.assetsTotalValueGreater!=0&&Player.instance.gold<item.assetsTotalValueGreater)
            {
                continue;
            }
            if(item.assetsTotalValueLess!=0&&Player.instance.gold>=item.assetsTotalValueLess)
            {
                continue;
            }
            //角色的某几项技能等级大于/小于【技能等级要求】
            if(!AnalysisSkillList(item.skillList))
            {
                continue;
            }
            //角色的【某系技能总等级】大于/小于【技能总等级要求】
            if(!AnalysisSkillTotalLevel(item.skillTotalLevelList))
            {
                continue;
            }
            //角色的深渊层级大于【深渊层级要求】
            if(Player.instance.playerAbyss<item.abyssGreater)
            {
                continue;
            }
            //角色的深渊层级小于【深渊层级要求】
            if(item.abyssLess!=0&&Player.instance.playerAbyss>=item.abyssLess)
            {
                continue;
            }
            //角色的现金大于【现金要求】
            if(item.cashGreater!=0&&Player.instance.gold<item.cashGreater)
            {
                continue;
            }
            //角色的现金小于【现金要求】
            if(item.cashLess!=0&&Player.instance.gold>=item.cashLess)
            {
                continue;
            }

            //经过层层筛选，最终创建了事件
            CreateTriggerEvent(item);
            Debug.Log("经过层层筛选，最终创建了触发事件！");
        }

    }
    public static void CreateTriggerEvent(TriggerEventsData data)
    {
        switch(Main.instance.UIState)
        {
            case 1:
            nowTrigger = UIPractice.instance.CreateTriggerEvent(data);
            break;
        }
        
        
    }
    public static void DestoryTrigger()
    {
        nowTrigger.DestorySelf();
        nowTrigger = null;
        DateManager.instance.StartTheWorld();
        UIPractice.instance.ContinueMagic();
        
    }
    ///<summary>由触发事件创建一个任务事件,若返回值为true，则立即开始事件</summary>
    public static bool CreateTaskEventFromTrigger(TriggerEventsData data)
    {
        TaskEventsData item =new TaskEventsData();
        int num =0;
        if(data.successTask!=0)
        {
            item =EventManager.instance.GetTaskInfo(data.successTask);
            num++;
        }
        if(data.failTask!=0)
        {
            item =EventManager.instance.GetTaskInfo(data.failTask);
            num++;
        }
        if(data.doneTask!=0)
        {
            item =EventManager.instance.GetTaskInfo(data.doneTask);
            num++;
        }
        if(num==0)
        {
           return false; 
        }
        item._startDate =DateManager.instance.DateAddtionCompute(DateManager.instance.now,int.Parse(item.startDate));
        TaskEvent task = UIEvents.instance.CreateTaskEvent(item,false);
        eventList.Add(item.id);          
        Main.instance.NewEventRemind();
        //如果开始日期小于1，那么立即开始；此处对于预约事件可能具有破坏性作用
        if(int.Parse(item.startDate)<1)
        {
            //立即开始事件
            UIEvents.instance.LoadEvent(task);
            return true;
        }
        return false;
    }
    
    ///<summary>事件结束时调用，以结算事件结果</summary>
    public static void MakeEventResult(TaskEventsData data)
    {
        //事件成功时
        if(data.result==1)
        {
            //增加金钱
            Player.instance.gold+=data.SGold;
            Debug.LogWarningFormat("金钱增加了{0}",data.SGold);
            //增加好感度
            for (int i = 0; i < data._SlikeCharacters.Count; i++)
            {
                var d = CharacterManager.instance.GetInfo(data._SlikeCharacters[i]);
                d._like+=data._SlikeCharacterAdds[i];
                CharacterManager.instance.ChangeLikeInDatabase(data._SlikeCharacters[i],data._SlikeCharacterAdds[i]);
            }
            //增加声望
            //增加影响力
            Player.instance.influence+=data.SInfluence;
            //获取情报
            int a =0;
            foreach (var item in data._SInfoLevel)
            {
                var d = CharacterManager.instance.GetInfo(item);
                d._infoLevel++;
                CharacterManager.instance.ChangeInfoLevelInDatabase(item);
                a++;
            }
            
            if(a>0)
            {
                UICharacter.instance.CreateCharacters();
            }
            //获取特质
            foreach (var item in data._STrait)
            {
                Player.instance.TryAddTrait(item);
            }
            //解锁技能
            foreach (var item in data._SUnlockSkill)
            {
                Player.instance.UnlockSkill(item);
            }
        }
        //失败时
        else if(data.result==2)
        {
            //增加金钱
            Player.instance.gold+=data.FGold;
            //增加好感度
            for (int i = 0; i < data._FlikeCharacters.Count; i++)
            {
                var d = CharacterManager.instance.GetInfo(data._FlikeCharacters[i]);
                d._like+=data._FlikeCharacterAdds[i];
                CharacterManager.instance.ChangeLikeInDatabase(data._SlikeCharacters[i],data._SlikeCharacterAdds[i]);
            }
            //增加声望
            //增加影响力
            Player.instance.influence+=data.FInfluence;
            //获取情报
            int a =0;
            foreach (var item in data._FInfoLevel)
            {
                var d = CharacterManager.instance.GetInfo(item);
                d._infoLevel++;
                CharacterManager.instance.ChangeInfoLevelInDatabase(item);
                a++;
            }
            if(a>0)
            {
                UICharacter.instance.CreateCharacters();
            }
            //获取特质
            foreach (var item in data._FTrait)
            {
                Player.instance.TryAddTrait(item);
            }
            //解锁技能
            foreach (var item in data._FUnlockSkill)
            {
                Player.instance.UnlockSkill(item);
            }    
        }
    }
    public static void MakeEventResult(TriggerEventsData data)
    {
        //事件成功时
        if(data.result==1)
        {
            //增加金钱
            Player.instance.gold+=data.SGold;
            //增加好感度
            for (int i = 0; i < data._SlikeCharacters.Count; i++)
            {
                var d = CharacterManager.instance.GetInfo(data._SlikeCharacters[i]);
                d._like+=data._SlikeCharacterAdds[i];
                CharacterManager.instance.ChangeLikeInDatabase(data._SlikeCharacters[i],data._SlikeCharacterAdds[i]);
            }
            //增加声望
            //增加影响力
            Player.instance.influence+=data.SInfluence;
            //获取情报
            int a =0;
            foreach (var item in data._SInfoLevel)
            {
                var d = CharacterManager.instance.GetInfo(item);
                d._infoLevel++;
                CharacterManager.instance.ChangeInfoLevelInDatabase(item);
                a++;
            }
            if(a>0)
            {
                UICharacter.instance.CreateCharacters();
            }
            //获取特质
            foreach (var item in data._STrait)
            {
                Player.instance.TryAddTrait(item);
            }
        }
        //失败时
        else if(data.result==2)
        {
            //增加金钱
            Player.instance.gold+=data.FGold;
            //增加好感度
            for (int i = 0; i < data._FlikeCharacters.Count; i++)
            {
                var d = CharacterManager.instance.GetInfo(data._FlikeCharacters[i]);
                d._like+=data._FlikeCharacterAdds[i];
                CharacterManager.instance.ChangeLikeInDatabase(data._SlikeCharacters[i],data._SlikeCharacterAdds[i]);
            }
            //增加声望
            //增加影响力
            Player.instance.influence+=data.FInfluence;
            //获取情报
            int a =0;
            foreach (var item in data._FInfoLevel)
            {
                var d = CharacterManager.instance.GetInfo(item);
                d._infoLevel++;
                CharacterManager.instance.ChangeInfoLevelInDatabase(item);

            }
            if(a>0)
            {
                UICharacter.instance.CreateCharacters();
            }
            //获取特质
            foreach (var item in data._FTrait)
            {
                Player.instance.TryAddTrait(item);
            }
        }
    }
    public static void TaskEventEnd(TaskEventsData data)
    {
        Main.instance.StartLoadingUI();
        // data.doneTime++;
        data._lastDate =DateManager.instance.now;
        BuffManager.RemovePlayerActorAllBuff();
        EffectManager.ClearPool();
        EventManager.ChangeDoneEvents(data,true);
        EventManager.MakeEventResult(data);
        EventManager.SaveEventState();
        EventManager.SaveTaskEventsInList();
        Main.instance.ShowBasicBanner();
        Main.instance.startLoadBasicUIs();
    }
     

}
