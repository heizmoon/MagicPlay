using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

[System.Serializable]
public class TimeLineEndAssets : PlayableAsset
{
    // Factory method that generates a playable based on this asset

    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        TimeLineEnd timeline = new TimeLineEnd();


        return ScriptPlayable<TimeLineEnd>.Create(graph,timeline);
    }
}
