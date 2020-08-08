using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerEvent : MonoBehaviour
{
    public TriggerEventsData triggerEventsData;
    
    ///<summary>事件生成的位置0=back,1=front</summary>
    public int triggerPos;
    ///<summary>触发事件的点击区域</summary>
    public Button triggerArea;
    ///<summary>触发事件的触发物预设</summary>
    public GameObject triggerPrefab;

    public string triggerPrefabAnimationName;
    ///<summary>触发事件触发后的演出效果</summary>
    public GameObject perform;
    
    Animation anim;
    
    void Start()
    {
        anim = GetComponent<Animation>();
        GameObject go = Instantiate(triggerPrefab);
        go.transform.SetParent(triggerArea.transform);
        go.transform.localPosition =Vector3.zero;
        go.transform.localScale =Vector3.one;
        // StartCoroutine(WaitForAnimator());
        if(triggerPrefabAnimationName!="")
        go.GetComponentInChildren<Animator>().Play(triggerPrefabAnimationName);

    }
    IEnumerator WaitForAnimator()
    {
        yield return new WaitForSeconds(0.1f);
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnTrigger()
    {
        //时间停止时不能触发事件
        if(DateManager.instance.ifWorldStop)
        {
            return;
        }
        Debug.Log("执行triggerEvent");
        DateManager.instance.StopTheWorld();
        triggerArea.interactable =false;
        anim.Stop();
        // Main.instance.IntoStoryMode(0);
        triggerArea.gameObject.SetActive(false);
        Destroy(triggerArea);
        perform.SetActive(true);
        transform.localPosition =Vector3.zero;
    }
    public void GetResult(int result)
    {
        

        //保存触发事件结果
        triggerEventsData.result =result;
        triggerEventsData._lastDate =DateManager.instance.now;
        triggerEventsData.doneTime++;
        //根据结果，发放奖励
        EventManager.MakeEventResult(triggerEventsData);
        
        EventManager.ChangeDoneEvents(triggerEventsData);
        //尝试创建由本triggerEvent引出的TaskEvent
        EventManager.CreateTaskEventFromTrigger(triggerEventsData);


        EventManager.DestoryTrigger();
    }

    public void DestorySelf()
    {
        Destroy(gameObject);
    }
}
