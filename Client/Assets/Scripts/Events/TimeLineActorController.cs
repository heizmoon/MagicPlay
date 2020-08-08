using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

// A behaviour that is attached to a playable
///<summary>控制一个角色做出某种动作</summary>
public class TimeLineActorController : PlayableBehaviour
{
    
    public string str;
    public GameObject go;
    // Called when the owning graph starts playing
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
        Animator[] anims =go.GetComponentsInChildren<Animator>();
        foreach (var item in anims)
        {
            if(item.gameObject != go)
            {
                item.Play(str);
            }
        }
        
        // Debug.LogFormat(go.name);
        
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
