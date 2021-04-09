using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

[System.Serializable]
public class TimeLineActorControllerAssets : PlayableAsset
{
    
    public string str;
    public ExposedReference<GameObject> go;
    // Factory method that generates a playable based on this asset
    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        TimeLineActorController test = new  TimeLineActorController();
        
        test.go = this.go.Resolve(graph.GetResolver());
        test.str =str;
        return ScriptPlayable<TimeLineActorController>.Create(graph,test);
    }
}
