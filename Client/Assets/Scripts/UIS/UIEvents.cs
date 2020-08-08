using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEvents : MonoBehaviour
{
    public static UIEvents instance;
    public Transform content;
    public GameObject information;
    public Text eventName;
    public Text timeCost;
    public Text describe;
    public Text otherCost;
    public Button startButton;
    ///<summary>0=可开始，1=可预约，2=已预约</summary>
    int buttonState;
    ///<summary>事件列表中的事件都在此List中</summary>
    public List<TaskEvent> taskEvents =new List<TaskEvent>();
    TaskEvent task;
    void Awake()
    {
        instance =this;
        startButton.onClick.AddListener(OnStartButton);
    }
    void Start()
    {
        
    }
    ///<summary>在UIEvents中创建一个任务事件</summary>
    ///<param name ="data">任务事件的数据</param>
    ///<param name ="conflict">该事件是否处于冲突状态</param>
    public TaskEvent CreateTaskEvent(TaskEventsData data,bool conflict)
    {
        Debug.LogFormat("UI创建一个任务！{0},状态为{1}",data.eventName,conflict);
        TaskEvent task =Instantiate((GameObject)Resources.Load("Prefabs/TaskEvent")).GetComponent<TaskEvent>();
        taskEvents.Add(task);
        task.conflict =conflict;
        task.CreateTaskEvent(data);
        task.transform.SetParent(content);
        task.transform.localScale =Vector3.one;
        task.transform.localPosition =Vector3.zero;
        task.toggle.group =content.GetComponent<ToggleGroup>();
        task.toggle.onValueChanged.AddListener((isOn)=>OnToggle(task,isOn));
        //如果本事件的id与playerDateEvents中的id一致，那么将本事件加入dateTaskEvents
        if(PlayerPrefs.GetString("playerDateEvents")!="")
        {
            foreach (var item in PlayerPrefs.GetString("playerDateEvents").Split('|'))
            {
                int eid = int.Parse(item.Split(',')[0]);
                if(task.data.id==eid)
                {
                    Player.instance.dateTaskEvents.Add(task);
                    break;
                }
            }
        }
        //如果本事件是自动预约事件，那么将本事件加入dateTaskEvents
        if(data._state==2)
        {
            bool b =false;
            foreach (var item in Player.instance.dateTaskEvents)
            {
                if(item.data.id ==data.id)
                {
                    b=true;
                    break;
                }
            }
            if(!b)
            {
                Player.instance.dateTaskEvents.Add(task);
            }
        }
        EventManager.SaveTaskEventsInList();
        information.SetActive(false);
        return task;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    void OnToggle(TaskEvent task,bool isOn)
    {
        this.task =task;
        information.SetActive(true);
        eventName.text = task.data.eventName;
        
        if(task.data.type==0)
        {
            timeCost.text =string.Format("耗时:{0}天",task.data.timeCost); 
        }
        else
        {
            timeCost.text =string.Format("开启:{0}年{1}月{2}日，耗时:{3}天",task.data._startDate.year,task.data._startDate.month,task.data._startDate.day,task.data.timeCost); 
        }
        string other="";
        if(task.data.prefab=="Abyss"&&Player.instance.rank==0)
        {
            //不显示金钱消耗
        }
        else
        {
            //显示金钱消耗
            if(task.data.goldCost>0)
            {
                other ="金钱消耗:{0}";
            }    
        }
        //显示影响力消耗
        if(task.data.influenceCost>0)
        {
            other +="  影响力消耗:{1}";
        }
        otherCost.text =string.Format(other,task.data.goldCost,task.data.influenceCost);
        
        describe.text =string.Format(task.data.discribe,Player.instance.playerAbyss);
        //点击后原有标记有new的事件将会清空new标识
        if(task.data._state==1)
        {
            task.data._state =0;
            task.ChangeStateText();
        }
        //如果事件有冲突，那么按钮不可点击


        if(task.data.type==0||DateManager.instance.IfToday(task.data._startDate))
        {
            buttonState=0;
            if(task.conflict)
            {
                startButton.GetComponentInChildren<Text>().text ="时间冲突";
                startButton.interactable=false;
            }
            else
            {   
                startButton.GetComponentInChildren<Text>().text ="开始";
                startButton.interactable=true;
                //如果角色影响力或者金钱不达标，则按钮无法点击
                if(Player.instance.gold<task.data.goldCost||Player.instance.influence<task.data.influenceCost)
                {
                    if(task.data.prefab=="Abyss"&&Player.instance.rank==0)
                    {

                    }
                    else
                    {
                        startButton.GetComponentInChildren<Text>().text ="需求不满足";
                        startButton.interactable=false;
                    }
                    
                }  
            }
        }

        else if(task.data._state==0)
        {
            startButton.GetComponentInChildren<Text>().text ="预约";
            buttonState=1;
            startButton.interactable=true;
        }
        else if(task.data._state==2)
        {
            
            startButton.GetComponentInChildren<Text>().text ="取消预约";
            buttonState=2;
            if(task.data.ifDateLock)
            {
                startButton.interactable=false;
            }
            else
            {
                startButton.interactable=true;
            }
            
        }
        
    }
    void OnStartButton()
    {
        //进入事件
        //消耗进入事件所需的金钱和影响力
        if(buttonState==0)
        {
            if(task.data.prefab=="Abyss"&&Player.instance.rank==0)
            {

            }
            else
            {
                Player.instance.gold-=task.data.goldCost;
            }
            Player.instance.influence-=task.data.influenceCost;
            //取消当前选择
            task.toggle.isOn =false;
            LoadEvent(task);
        }
        //预约事件
        else if(buttonState==1)
        {
            //改变事件状态
            task.data._state=2;
            //改变事件文字
            task.ChangeStateText();
            //改变按钮文字
            startButton.GetComponentInChildren<Text>().text ="取消预约";
            buttonState=2;
            //加入预约列表
            Player.instance.dateTaskEvents.Add(task);
            //检测时间冲突
            TryRelieveTimeConfict();
        }
        //取许预约
        else if(buttonState==2)
        {
            task.data._state=0;
            task.conflict =false;
            task.ChangeStateText();
            startButton.GetComponentInChildren<Text>().text ="预约";
            buttonState=1;
            Player.instance.dateTaskEvents.Remove(task);
            TryRelieveTimeConfict();
        }
    }
    //加载事件预设
    public void LoadEvent(TaskEvent task)
    {
        Debug.Log("加载事件");
        //打开loading界面
        Main.instance.StartLoadingUI();
        EffectManager.ClearPool();
        GameObject go = Instantiate((GameObject)Resources.Load("Prefabs/"+task.data.prefab));
        //此处采用tanser是旧的做法中，DestoryBasicUI时，将UIEvents一同Destory掉了，task.data没有保留下来，因此创建了tanser
        //在新的做法中，UIEvents没有被Destory掉，所以其实已经不必创建tanser了
        //目前依然保留了eventTranser留作他用
        //如果加载的事件上没有eventTranser，则创建一个eventTranser，如果有，则直接使用事件上的eventTranser
        EventTanser eventTanser;
        eventTanser=go.GetComponent<EventTanser>();
        if(eventTanser==null)
        {
            eventTanser= go.AddComponent<EventTanser>();
        }
        //设置eventTranser的数据
        eventTanser.data =task.data;
        eventTanser.orginTask =task;


        //时间停止
        DateManager.instance.StopTheWorld();
        Player.instance.playerActor.StopCasting();
        //保存事件列表，以便事件结束后重新创建事件
        Player.instance.dateTaskEvents.Remove(task);
        PlayerPrefs.SetString("playerDateEvents",EventManager.SaveDateEvents());
        //一旦进入事件，时间就变化，并且默认事件为失败
        DateManager.instance.now = DateManager.instance.DateAddtionCompute(DateManager.instance.now,eventTanser.data.timeCost);
        eventTanser.data.doneTime++;
        eventTanser.data._lastDate =DateManager.instance.now;
        eventTanser.data.result =2;
        EventManager.ChangeDoneEvents(eventTanser.data,false);
        //重置角色战斗状态
        Player.instance.playerActor.GetComponent<Actor>().ReLiveActor();
        BuffManager.RemovePlayerActorAllBuff();
        //给角色添加buff
        //给playerActor添加被动buff
        //检测被动技能，如果被动技能有buff，则添加buff
        foreach (var item in Player.instance.GetLearnSkills())
        {
            Ability ability =SkillManager.instance.GetInfo(item);
            int level =0;
            if(!ability.ifActive && ability.buffID!=0 && ability.targetSelf)
            {
                level = Player.instance.GetSkillLevel(item);
                BuffManager.instance.CreateBuffForActor(ability.buffID,level,Player.instance.playerActor);

            }
        }
        //关闭所有其他界面
        Main.instance.DestoryBasicUI();
        Main.instance.HideBasicBanner();
        Main.instance.dateEventStopWorld =false;
    }
    ///<summary>从事件列表中标记过期事件</summary>
    public void TryMarkOverDueEvent()
    {
        //1.找到事件列表中所有type为1的事件
        //2.找到这些事件中startDate<now的事件
        //3.标记这些事件的状态为[已过期]
        if(taskEvents.Count==0)
        {
            return;
        }
        for (int i =0;i<taskEvents.Count;i++)
        {
            if(taskEvents[i].data.type==0)
            {
                continue;
            }
            if(DateManager.ConvertDateToInt(taskEvents[i].data._startDate)-DateManager.ConvertDateToInt(DateManager.instance.now)>=0)
            {
                continue;
            }
            Debug.LogFormat("检测到过期事件：{0}",taskEvents[i].data.eventName);

            RemoveTaskEvent(taskEvents[i]);
            i--;
        }
        EventManager.SaveTaskEventsInList();

    }
    ///<>
    bool FindTimeConflict(TaskEvent task)
    {
        int i =0;
        foreach (var item in taskEvents)
        {
            if(item.data.type ==0)
            {
            item.data._startDate =DateManager.instance.now;
            item.data._endDate =DateManager.instance.DateAddtionCompute(item.data._startDate,item.data.timeCost);
            }
            if(item ==task)
            {
                continue;
            }
            if(item.data.type>0&&item.data._state<2)
            {
                continue;
            }
            if(!DateManager.WhichDateFirst(task.data._endDate,item.data._startDate))
            {
                continue;
                
            }
            if(!DateManager.WhichDateFirst(task.data._startDate,item.data._endDate))
            {
                //如果与其冲突的是一个常驻事件，那么常驻事件显示为冲突，自身不显示为冲突
                //如果冲突的是两个常驻事件，那么都不显示为冲突
                if(task.data.type==0&&item.data.type==0)
                {
                    continue;    
                }
                else if(task.data.type==0&&item.data.type!=0)
                {
                    task.conflict =true;
                    task.ChangeStateText();
                    i++;   
                }
                else if(task.data.type!=0&&item.data.type==0)
                {
                    item.conflict =true;
                    item.ChangeStateText();
                }
                else if(item.data.type!=0&&item.data.type!=0)
                {
                    task.conflict=true;
                    task.ChangeStateText();
                    item.conflict =true;
                    item.ChangeStateText();
                    i++;
                }
                
            }
            

        }
        if(i==0)
        {
            //没有找到冲突
            return false;
        }
        else
        {
            return true;
        }
        
    }
    public void TryRelieveTimeConfict()
    {
        //如果找不到冲突，那么就解除
        foreach (var item in taskEvents)
        {
            if(item.data.type>0&&item.data._state<2)
            {
                continue;
            }
            if(!FindTimeConflict(item))
            {
                item.conflict =false;
                item.ChangeStateText();
            }
            // else
            // {
            //     item.conflict =true;
            //     item.ChangeStateText();
            // }
        }


    }
    public void RemoveTaskEvent(TaskEventsData rdata)
    {
        RemoveTaskEvent(rdata.id);
    }
    public void RemoveTaskEvent(int rid)
    {
        if(taskEvents.Count==0)
        {
            return;
        }
        for (int i =0;i<taskEvents.Count;i++)
        {
            if(taskEvents[i].data.id==rid)
            {
                RemoveTaskEvent(taskEvents[i]);
            }
        }
    }
    public void RemoveTaskEvent(TaskEvent revent)
    {
        EventManager.eventList.Remove(revent.data.id);
        Debug.LogFormat("移除事件：{0}",revent.data.eventName);
        GameObject go =revent.gameObject;
        taskEvents.Remove(revent);
        Destroy(go);

    }
    
}
