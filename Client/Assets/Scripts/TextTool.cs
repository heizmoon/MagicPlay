using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TextTool : MonoBehaviour
{
    private Text _text;
    public float typeSpeed =0.1f;
    private string words;
    private int currentLength =0;
    private float timer=0;
    bool ifTypeEffect =false;
    void AWake()
    {
        _text =GetComponent<Text>();
        
    }

    // Update is called once per frame
    void Update()
    {
        OnType();
    }
    public void OnStartType(string words)
    {
        if(_text==null)
        {
            _text =GetComponent<Text>();
        }
        _text.text ="";
        this.words =words;
        ifTypeEffect =true;
    }
    void OnType()
    {
        if(!ifTypeEffect)
        {
            return;
        }
        timer+=Time.deltaTime;
        if(timer>typeSpeed)
        {
            timer=0;
            currentLength++;
            _text.text =words.Substring(0,currentLength);
            if(currentLength>=words.Length)
            {
                OnTypeEnd();
            }
        }
        
    }
    public void OnTypeEnd()
    {
        _text.text =words;
        ifTypeEffect =false;
        timer =0;
        currentLength =0;
    }

    public bool IfEnd()
    {
        return !ifTypeEffect;
    }
}
