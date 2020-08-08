using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TypeEventTool : MonoBehaviour
{
    //后续事件
    public GameObject goodContinue;
    public GameObject badContinue;
    void Start()
    {
        
    }

    // Update is called once per frame
    public void GetResult(int result)
    {
        Main.instance.StartLoadingUI();
        DestorySelf();
        // if(result ==1)
        // {
        //     if(goodContinue!=null)
        //     {
        //         GameObject go = Instantiate(goodContinue);
        //         go.transform.SetParent(Main.instance.allScreenUI);
        //         // TransEvent(go);
        //         Main.instance.StartLoadingUI();
        //         DestorySelf();
        //     }
        //     else
        //     {
        //         // SendResultToEvent(result);
        //     }
        // }
        // if(result ==2)
        // {
        // if(badContinue!=null)
        //     {
        //         GameObject go = Instantiate(badContinue);
        //         go.transform.SetParent(Main.instance.allScreenUI);
        //         // TransEvent(go);
                
        //     }
        //     else
        //     {
        //         // SendResultToEvent(result);
        //     }
        // }
    }
    // void TransEvent(GameObject go)
    // {
    //     EventTanser et = GetComponent<EventTanser>();
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
    // }
    // void SendResultToEvent(int result)
    // {
    //     Debug.LogFormat("返回事件结果");

    //     EventTanser et = GetComponent<EventTanser>();
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
    // }
    void DestorySelf()
    {
        Destroy(gameObject);
    }
}
