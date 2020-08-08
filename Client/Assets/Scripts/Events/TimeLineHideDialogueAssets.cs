using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

[System.Serializable]
public class TimeLineHideDialogueAssets : PlayableAsset
{
    // Factory method that generates a playable based on this asset
    public ExposedReference<Button> choose1BTN;
    public ExposedReference<Button> choose2BTN;
    public ExposedReference<Button> nextButton;
    public ExposedReference<Text> contentText;    
    public ExposedReference<Text> nameText;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        TimeLineHideDialogue timeline = new TimeLineHideDialogue();
        timeline.choose1BTN =choose1BTN.Resolve(graph.GetResolver());
        timeline.choose2BTN =choose2BTN.Resolve(graph.GetResolver());
        timeline.nextButton =nextButton.Resolve(graph.GetResolver());
        timeline.contentText =contentText.Resolve(graph.GetResolver());
        timeline.nameText =nameText.Resolve(graph.GetResolver());



        return ScriptPlayable<TimeLineHideDialogue>.Create(graph,timeline);
    }
}
