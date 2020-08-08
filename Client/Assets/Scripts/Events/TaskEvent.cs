using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TaskEvent : MonoBehaviour
{
    //这是已经创建出来的，在事件列表中的事件
    public Text nameText;
    public Text stateText;
    public Toggle toggle;
    public TaskEventsData data;
    public bool conflict;
    GameObject ResultUI;
    
    
    public void CreateTaskEvent(TaskEventsData data)
    {
        this.data = data;
        nameText.text =data.eventName;
        ChangeStateText();
        toggle =GetComponent<Toggle>();    
    }
    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeStateText()
    {
        if(conflict)
        {
            stateText.text ="时间冲突";
            // Debug.LogFormat("时间冲突的事件名：{0}",data.eventName);
            return;
        }
        else
        {
            stateText.text ="";
        }
        if(data.type ==1)
        {
            switch(data._state)
            {
                case 0:
                stateText.text ="";
                break;
                case 1:
                stateText.text ="new!";
                break;
                case 2:
                stateText.text ="已预约";
                break;
                case 3:
                data._state=2;
                stateText.text ="马上开始";
                break;
            }
        }
        else
        {
            stateText.text ="";
        }

    }
    public void GetResult(TaskEventsData data)
    {
        this.data =data;
        ShowResultUI();
        
    }
    void ShowResultUI()
    {
        ResultUI  = (GameObject)Instantiate(Resources.Load("Prefabs/UIEventResult"));
        ResultUI.transform.SetParent(Main.instance.allScreenUI);
        ResultUI.GetComponent<RectTransform>().sizeDelta =Vector2.zero;
        ResultUI.GetComponent<RectTransform>().anchoredPosition3D =Vector3.zero;
        ResultUI.transform.localScale =Vector3.one;
        ResultUI.GetComponent<UIEventResult>().BTNok.onClick.AddListener(EndTaskEvent);
        ResultUI.GetComponent<UIEventResult>().data =data;
    }
    void EndTaskEvent()
    {
        ResultUI.GetComponent<UIEventResult>().CloseUI();
        EventManager.TaskEventEnd(data);
    }
    
}
