using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using Data;

public class Perform : MonoBehaviour
{
    ///<summary>演员列表</summary>
    public List<GameObject> actorList;
    public List<Transform> actorPositions;

    ///<summary>场景</summary>
    public GameObject scene;

    ///<summary>幕-剧情时间轴</summary>
    //播放一段时间轴动画或对话，播放完之后需要等待进一步触发才能继续进行
    public List<TimelineAsset> timeLine;
    public PlayableDirector director;
    public Transform playerActorPostion;

    ///<summary>台词表</summary>
    public string dialogueTable;
    int currentID=1;
    
    DiagueDatabase[] dialogueData;
    ///<summary>演出的触发者,用于通知演出的结果</summary>
    public GameObject trigger;
    public enum TriggerType
    {
        triggerEvent,
        taskEvent
    }
    ///<summary>触发者的类型 0=triggerEvent,1=performEvent,</summary>
    public TriggerType eventType;
    //后续事件
    public GameObject goodContinue;
    public GameObject badContinue;
    //摘要：
    //    进入模式
    public int intoMode;
    public int sceneMode;
    bool startSyncScene;
    void Start()
    {
        DialogueDataSet diaSet = Resources.Load<DialogueDataSet>("DataAssets/Dialogue/"+dialogueTable);
        dialogueData = diaSet.dataArray;
        StartCoroutine(WaitForLoad());
        
        transform.parent.SetParent(Main.instance.allScreenUI);
        transform.parent.GetComponent<RectTransform>().sizeDelta = Vector2.zero;
        transform.parent.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
        transform.parent.localScale =Vector3.one;
        
    }

    void Update()
    {
        if(sceneMode==1&&startSyncScene)
        {
            UIMain.instance.background.localPosition = scene.transform.localPosition;
            UIMain.instance.background.localScale = scene.transform.localScale;

        }
    }
    IEnumerator WaitForLoad()
    {
        if(sceneMode==1)
        {
            UIMain.instance.GetComponent<Animation>().Play("UI_UIMain_dark");
            UIMain.instance.StopActors(true);
            UIMain.instance._actor1.GetComponentInChildren<Actor>().transform.SetParent(actorPositions[0]);
            UIMain.instance._actor2.GetComponentInChildren<Actor>().transform.SetParent(actorPositions[1]);
            UIMain.instance._actor3.GetComponentInChildren<Actor>().transform.SetParent(actorPositions[2]);
            
            if(actorPositions.Count>3&&actorList.Count>0)
            {
                for (int i = 0; i < actorList.Count; i++)
                {
                    GameObject go= Instantiate(actorList[i]);
                    go.transform.SetParent(actorPositions[i+3]);
                    go.transform.localPosition =Vector3.zero;
                    go.transform.localScale =Vector3.one;
                }
            }
        }
        if(playerActorPostion!=null&&sceneMode!=1)
        {
            // if(intoMode ==0)
            // {
            //     Debug.Log(UIBattle.Instance.t_playerPosition.position);
            //     playerActorPostion.position = UIBattle.Instance.t_playerPosition.position;
            //     actorPositions[0].position =UIBattle.Instance.t_enemyPosition.position;
            // }
            Player.instance.playerActor.transform.SetParent(playerActorPostion);
            Player.instance.playerActor.transform.localPosition =Vector3.zero;
            if(intoMode ==0)
            {
                Player.instance.playerActor.transform.localScale =Vector3.one*1.5f;
                UIBattle.Instance.Enemy.transform.SetParent(actorPositions[0]);
                UIBattle.Instance.Enemy.transform.localPosition =Vector3.zero;
                UIBattle.Instance.Enemy.transform.localScale =Vector3.one*1.5f;
            }
            else
            {
                Player.instance.playerActor.transform.localScale =Vector3.one;
            }
            Player.instance.playerActor.GetComponent<Actor>().StopCasting();
            for (int i = 0; i < actorList.Count; i++)
            {
                GameObject go= Instantiate(actorList[i]);
                go.transform.SetParent(actorPositions[i]);
                go.transform.localPosition =Vector3.zero;
                go.transform.localScale =Vector3.one;
            }
            if(intoMode==0)
            {
                Main.instance.IntoStoryMode(intoMode);
                UIBattle.Instance.g_enemyCastingBar.SetActive(false);
                UIBattle.Instance.BTN_coldTip_1.gameObject.SetActive(false);
                UIBattle.Instance.BTN_coldTip_2.gameObject.SetActive(false);
                UIBattle.Instance.BTN_shieldTip_1.gameObject.SetActive(false);
                UIBattle.Instance.BTN_shieldTip_2.gameObject.SetActive(false);
                UIBattle.Instance.BTN_invalidTip_1.gameObject.SetActive(false);
                UIBattle.Instance.BTN_invalidTip_2.gameObject.SetActive(false);
                UIBattle.Instance.t_playerBuffPosition.gameObject.SetActive(false);
                UIBattle.Instance.t_enemyBuffPosition.gameObject.SetActive(false);
                UIBattle.Instance.enemyBarText.gameObject.SetActive(false);
            }
            
        }
        
        director.Stop();
        
        if(intoMode==1&&sceneMode!=1)
        {
            // yield return new WaitForSeconds(3f);
            Main.instance.IntoStoryMode(intoMode);
        }
        if(sceneMode!=1)
        yield return new WaitForSeconds(1.5f);
        else
        {
            if(intoMode==1)
            {
                // yield return new WaitForSeconds(3f);
                yield return new WaitForSeconds(1f);
                Main.instance.IntoStoryMode(intoMode);
                startSyncScene =true;
                actorPositions[0].GetComponentInChildren<Actor>().transform.localPosition =Vector3.zero;
                actorPositions[0].GetComponentInChildren<Actor>().transform.localScale =Vector3.one;
                actorPositions[1].GetComponentInChildren<Actor>().transform.localPosition =Vector3.zero;
                actorPositions[1].GetComponentInChildren<Actor>().transform.localScale =Vector3.one;
                actorPositions[2].GetComponentInChildren<Actor>().transform.localPosition =Vector3.zero;
                actorPositions[2].GetComponentInChildren<Actor>().transform.localScale =Vector3.one;
            }
        }

        director.playableAsset =timeLine[0];
        director.Play();
    }
    public void SwitchTimeLine(int num)
    {
        director.playableAsset =timeLine[num];
        director.Stop();
        director.Play();
    }
    public DiagueDatabase SwitchDialogue()
    {
        DiagueDatabase data = GetInfo(currentID);
        Debug.LogFormat("data.content:{0}",data.content);
        return data;
    }

    public DiagueDatabase GetInfo(int id)
    {
        
       foreach(var item in dialogueData)
        {
            if(item.id==id)
            {
            return item;       
            } 
        }
        DiagueDatabase end =new DiagueDatabase();
        end.choose1="";
        end.choose2 ="";

        return end;
    }
    public void OnChoose(int id)
    {
        DiagueDatabase data = GetInfo(currentID);
        if(id ==0)
        {
            currentID =data.nextID;
        }
        if(id ==1)
        {
            currentID =data.next1;
        }
        else if(id ==2)
        {
            currentID =data.next2;
        }
        Debug.LogFormat("下一个id:{0}",currentID);
    }
    public void OnPerformEnd()
    {
        //playerActor归位
        if(sceneMode==1)
        {
            actorPositions[0].GetComponentInChildren<Actor>().transform.SetParent(UIMain.instance._actor1.transform);
            actorPositions[1].GetComponentInChildren<Actor>().transform.SetParent(UIMain.instance._actor2.transform);
            actorPositions[2].GetComponentInChildren<Actor>().transform.SetParent(UIMain.instance._actor3.transform);
            UIMain.instance._actor1.transform.GetChild(0).localPosition =Vector3.zero;
            UIMain.instance._actor1.transform.GetChild(0).localScale =Vector3.one;
            UIMain.instance._actor2.transform.GetChild(0).localPosition =Vector3.zero;
            UIMain.instance._actor2.transform.GetChild(0).localScale =Vector3.one;
            UIMain.instance._actor3.transform.GetChild(0).localPosition =Vector3.zero;
            UIMain.instance._actor3.transform.GetChild(0).localScale =Vector3.one;
            UIMain.instance.background.localScale =Vector3.one;
            UIMain.instance.StopActors(false);
            UIMain.instance.transform.Find("ActiveUIs/block").gameObject.SetActive(false);
        }
        if(playerActorPostion!=null)
        {
            if(intoMode!=0)
            {
                Player.instance.playerActor.transform.SetParent(Main.instance.BottomUI);
            }
            else
            {
                Player.instance.playerActor.transform.SetParent(UIBattle.Instance.t_playerPosition);
                Player.instance.playerActor.transform.localPosition =Vector3.zero;
                Player.instance.playerActor.transform.localScale =Vector3.one;
                UIBattle.Instance.Enemy.transform.SetParent(UIBattle.Instance.t_enemyPosition);
                UIBattle.Instance.Enemy.transform.localPosition =Vector3.zero;
                UIBattle.Instance.Enemy.transform.localScale =Vector3.one;
            }
            
        }
        
        
        //结束时判断结果
        int result =0;
        if(currentID==-1)
        {
            Debug.Log("好结局");
            result =1;
            if(goodContinue!=null)
            {
                GameObject go = Instantiate(goodContinue);
                go.transform.SetParent(Main.instance.allScreenUI);
                TransEvent(go);
                Main.instance.StartLoadingUI();
                DestorySelf();
            }
            else
            {
                SendResultToEvent(result);
            }
        }
        else if(currentID ==-2)
        {
            Debug.Log("坏结局");
            result =2;
            if(badContinue!=null)
            {
                GameObject go = Instantiate(badContinue);
                go.transform.SetParent(Main.instance.allScreenUI);
                TransEvent(go);
                Main.instance.StartLoadingUI();
                DestorySelf();
            }
            else
            {
                SendResultToEvent(result);
            }
        }    
    }
    void SendResultToEvent(int result)
    {
        //如果没有后续事件，则直接执行结果
        //如果演出是由触发事件引出的，那么要将演出的结果传递给触发事件
        //如果演出是由任务事件引出的，那么要将演出的结果传递给任务事件
        // if(eventType==TriggerType.triggerEvent)
        // {
        //     if(trigger ==null)
        //     {
        //         Debug.LogFormat("演出结束，没有返回结果的事件");
        //         DestorySelf();
        //         return;
        //     }
        //     trigger.GetComponent<TriggerEvent>().GetResult(result);
        //     DestorySelf();
        //     return;
        // }
        // if(eventType ==TriggerType.taskEvent)
        // {
        //     //寻找eventTranser
        //     EventTanser et = transform.parent.GetComponent<EventTanser>();
        //     if(et!=null)
        //     {
        //         et.data.result =result;
        //         et.orginTask.GetResult(et.data);
        //     }
        //     else
        //     {
        //         Debug.LogFormat("演出结束，没有返回结果的事件");
        //     }
        //     DestorySelf();
        //     return;
        // }
        DestorySelf();
    }
    public void DestorySelf()
    {
        Main.instance.LeaveStoryMode();
        if(intoMode==0)
        {
            UIBattle.Instance.g_enemyCastingBar.SetActive(true);
            UIBattle.Instance.BTN_shieldTip_1.gameObject.SetActive(true);
            UIBattle.Instance.BTN_shieldTip_2.gameObject.SetActive(true);
            // UIBattle.Instance.BTN_coldTip_1.gameObject.SetActive(true);
            // UIBattle.Instance.BTN_coldTip_2.gameObject.SetActive(true);
            UIBattle.Instance.t_playerBuffPosition.gameObject.SetActive(true);
            UIBattle.Instance.t_enemyBuffPosition.gameObject.SetActive(true);
            UIBattle.Instance.enemyBarText.gameObject.SetActive(true);
        
            UIBattle.Instance.isBattleOver = false;
            UIBattle.Instance.Enemy.RunAI();
            UIBattle.Instance.StartBattle();
        
        }
        if(Main.instance.ifNewBird ==19)
        {
            Main.instance.ifNewBird++;
            PlayerPrefs.SetInt("ifNew",20);
            UIBattle.Instance.OnBattleGoOn();
            BattleScene.instance.BattleSceneOver();
            Main.instance.InitUIChooseCharacter();
        }
        Destroy(gameObject.transform.parent.gameObject);
    }

    //如果自身本来就挂有eventTranser那么就会把transer传递给后续物体
    //判断后续物体身上是否带有transer,如果有，那么后续物体的transer=this.transer
    //如果没有，在后续物体上创建一个transer，然后让后续物体的transer=this.transer
    void TransEvent(GameObject go)
    {
    //     EventTanser et = transform.parent.GetComponent<EventTanser>();
    //     if(et!=null)
    //     {
    //         EventTanser gt =  go.GetComponent<EventTanser>();
    //         if(gt!=null)
    //         {
    //             gt.data = et.data;
    //             gt.orginTask =et.orginTask;
    //         }
    //         else
    //         {
    //             gt = go.AddComponent<EventTanser>();
    //             gt.data = et.data;
    //             gt.orginTask =et.orginTask;
    //         }
    //     }
    //     else
    //     {
    //         Debug.LogErrorFormat("无法传递事件");
    //     }
    }
    public static void LoadPerform(string performName)
    {
        Instantiate(Resources.Load("Prefabs/Perform/"+performName));
    }
}
