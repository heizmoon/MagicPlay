using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Relic : MonoBehaviour
{
    public Text textName;
    public Text textDescribe;
    
    public RelicData data;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CreateRelic(RelicData relicData)
    {
        data =relicData;
        textName.text =data.name;
        textDescribe.text =data.describe;
    }
}
