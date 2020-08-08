using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterItem : MonoBehaviour
{
    Toggle toggle;
    public Text nameText;
    public Text stateText;
    public CharacterData data;
    void Start()
    {
        toggle =GetComponent<Toggle>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CreateItem(CharacterData data)
    {
        this.data =data;
        nameText.text =data.name;
        string state;
        if(data._like<=-60)
        {
            //-60 = 40; -100 = 0;
            
            state = "厌恶";
        }
        else if(data._like<=-20)
        {
            //-20 = 40; -60 =0;
            
            state = "反感";
        }
        else if(data._like<20)
        {
            //20 = 40; -20 =0;
            
            state = "平淡";
        }
        else if(data._like<60)
        {
            //20 = 0; 60 =40;
            
            state = "友好";
        }
        else
        {
            //60 = 0; 100 =40;
            
            state = "爱慕";
        }
        stateText.text =string.Format("{0}:{1}",state,data._like);
        
    }
    public void RefreashData()
    {
       data=CharacterManager.instance.GetInfo(data.id);
       CreateItem(data); 
    }
}
