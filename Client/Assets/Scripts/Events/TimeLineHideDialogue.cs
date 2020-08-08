using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

// A behaviour that is attached to a playable
public class TimeLineHideDialogue : PlayableBehaviour
{
    // Called when the owning graph starts playing
    
    
    public Button choose1BTN;
    public Button choose2BTN;
    public Button nextButton;
    public Text contentText;
    public Text nameText;
    
    public override void OnPlayableCreate(Playable playable)
    {
        
        
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
        contentText.gameObject.SetActive(false);
        nameText.gameObject.SetActive(false);
        choose1BTN.gameObject.SetActive(false);
        choose2BTN.gameObject.SetActive(false);
        nextButton.gameObject.SetActive(false);
    }

    // Called when the state of the playable is set to Paused
    public override void OnBehaviourPause(Playable playable, FrameData info)
    {
        
    }

    // Called each frame while the state is set to Play
    public override void PrepareFrame(Playable playable, FrameData info)
    {
        
    }
}
