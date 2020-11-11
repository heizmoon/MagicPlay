using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Relic : MonoBehaviour
{
    public Text textName;
    public Text textDescribe;
    
    public SkillData data;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CreateRelic(SkillData skillData)
    {
        data =skillData;
        textName.text =data.name;
        textDescribe.text =data.describe;
    }
}
