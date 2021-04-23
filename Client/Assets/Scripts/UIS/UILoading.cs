using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UILoading : MonoBehaviour
{
    Text toolTipText;
    Image bar;
    void Awake()
    {
        bar = transform.Find("Bar/BarImage").GetComponent<Image>();
        toolTipText =transform.Find("ToolTipText").GetComponent<Text>();
    }
    public void Reset()
    {
        bar.fillAmount =0;
        bar.DOFillAmount(1,2.8f);
        int r  = Random.Range(0,Configs.instance.toolTips.Count);
        toolTipText.text =Configs.instance.toolTips[r];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
