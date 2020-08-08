using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

// A behaviour that is attached to a playable
public class TimeLineDialogue : PlayableBehaviour
{
    // Called when the owning graph starts playing
    public PlayableDirector director ;
    public Button choose1BTN;
    public Button choose2BTN;
    public Button nextButton;
    public Text contentText;
    public Text nameText;
    bool contrl;

    public override void OnPlayableCreate(Playable playable)
    {
        
        director =Transform.FindObjectOfType<PlayableDirector>().GetComponent<PlayableDirector>();
        Debug.LogFormat("driector:{0}",director.name);
    }
    public override void OnGraphStart(Playable playable)
    {
        
    }

    // Called when the owning graph stops playing
    public override void OnGraphStop(Playable playable)
    {

    }

    // Called when the state of the playable is set to Play
    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        Debug.LogFormat("OnBehaviourPlay{0}",contrl);
        if(contrl)
        {
            contrl =false;
            return;
        }
        director.Pause();
        DiagueDatabase data = director.GetComponent<Perform>().SwitchDialogue();
        Debug.LogFormat("要说的话是：{0}",data.content);
        if(data.content!="")
        {
            contentText.gameObject.SetActive(true);
            // contentText.text =data.content;
            contentText.GetComponent<TextTool>().OnStartType(data.content);
        }
        nameText.gameObject.SetActive(true);
        if(data.maskName =="")
        {
            nameText.text = CharacterManager.instance.GetInfo(data.character,"name");
        }
        else
        {
            nameText.text =data.maskName;
        }
        if(data.choose1!="")
        {
            choose1BTN.gameObject.SetActive(true);
            Text _text = choose1BTN.GetComponentInChildren<Text>();
            _text.GetComponent<TextTool>().OnStartType(data.choose1);
            choose1BTN.onClick.RemoveAllListeners();
            choose1BTN.onClick.AddListener(OnButton1);
        }
        if(data.choose2!="")
        {
            choose2BTN.gameObject.SetActive(true);
            Text _text = choose2BTN.GetComponentInChildren<Text>();
            _text.GetComponent<TextTool>().OnStartType(data.choose2);
            choose2BTN.onClick.RemoveAllListeners();
            choose2BTN.onClick.AddListener(OnButton2);
        }
        
        if(data.choose1==""&&data.choose2=="")
        {
            Debug.Log("没有选项！");
            nextButton.gameObject.SetActive(true);
            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(OnNextButton);
        }
    }

    // Called when the state of the playable is set to Paused
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        
    }

    // Called each frame while the state is set to Play
    public override void PrepareFrame(Playable playable, FrameData info)
    {
        
    }
    public void OnNextButton()
    {
        if(!contentText.GetComponent<TextTool>().IfEnd())
        {
            contentText.GetComponent<TextTool>().OnTypeEnd();
            return;
        }
        else
        {
            choose1BTN.onClick.RemoveAllListeners();
            choose2BTN.onClick.RemoveAllListeners();
            nextButton.onClick.RemoveAllListeners();
            director.Play();
            contrl =true;
            director.GetComponent<Perform>().OnChoose(0);
        }

    }
    public void OnButton1()
    {
        if(!choose1BTN.GetComponentInChildren<TextTool>().IfEnd())
        {
            choose1BTN.GetComponentInChildren<TextTool>().OnTypeEnd();
            return;
        }
        else
        {
            choose1BTN.onClick.RemoveAllListeners();
            choose2BTN.onClick.RemoveAllListeners();
            nextButton.onClick.RemoveAllListeners();
            director.Play();
            contrl =true;
            director.GetComponent<Perform>().OnChoose(1);
        }        
    }
    public void OnButton2()
    {
        if(!choose2BTN.GetComponentInChildren<TextTool>().IfEnd())
        {
            choose2BTN.GetComponentInChildren<TextTool>().OnTypeEnd();
            return;
        }
        else
        {
        choose1BTN.onClick.RemoveAllListeners();
        choose2BTN.onClick.RemoveAllListeners();
        nextButton.onClick.RemoveAllListeners();
        director.Play();
        contrl =true;
        director.GetComponent<Perform>().OnChoose(2);
        }  
    }
}
