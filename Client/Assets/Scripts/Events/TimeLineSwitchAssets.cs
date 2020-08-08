using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

[System.Serializable]
public class TimeLineSwitchAssets : PlayableAsset
{
    // Factory method that generates a playable based on this asset
    public int timeLineID;
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        TimeLineSwitch timeline = new TimeLineSwitch();
        timeline.timeLineID =timeLineID;

        return ScriptPlayable<TimeLineSwitch>.Create(graph,timeline);
    }
}
