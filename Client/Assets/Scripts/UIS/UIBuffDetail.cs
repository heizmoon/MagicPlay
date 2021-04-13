using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIBuffDetail : MonoBehaviour
{
    Button background;
    Text describe;
    Text nameText;

    private void Awake() 
    {
        describe = transform.Find("describeText").GetComponent<Text>();
        nameText = transform.Find("nameText").GetComponent<Text>();

        background = transform.Find("Button").GetComponent<Button>();
        background.onClick.AddListener(OnClose);
    }
    private void Start() 
    {
        Time.timeScale=0;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public static void CreateUIBuffDetail(Buff buff)
    {
        UIBuffDetail ui = ((GameObject)Instantiate(Resources.Load("Prefabs/UIBuffDetail"))).GetComponent<UIBuffDetail>();
        ui.transform.SetParent(Main.instance.allScreenUI);
        ui.GetComponent<RectTransform>().anchoredPosition =Vector2.zero;
        ui.GetComponent<RectTransform>().sizeDelta =Vector3.one;
        ui.transform.localScale = Vector3.one;
        ui.transform.localPosition =Vector3.zero;
        ui.nameText.text = buff.buffData.name;
        ui.describe.text = string.Format(buff.buffData.describe,buff.buffData.value,buff.buffData.time,buff.buffData.maxNum,buff.buffData.delay); 
        //{0} = value {1} = time {2} = maxNum {3} = delay
    }
    void OnClose()
    {
        Destroy(gameObject);
        if(!UIBattle.Instance.ifPause)
        Time.timeScale=1;
    }
    //用于界面说明描述
    public static void CreateUIBuffDetail(string strDes,string strName)
    {
        UIBuffDetail ui = ((GameObject)Instantiate(Resources.Load("Prefabs/UIBuffDetail"))).GetComponent<UIBuffDetail>();
        ui.transform.SetParent(Main.instance.allScreenUI);
        ui.GetComponent<RectTransform>().anchoredPosition =Vector2.zero;
        ui.GetComponent<RectTransform>().sizeDelta =Vector3.one;
        ui.transform.localScale = Vector3.one;
        ui.transform.localPosition =Vector3.zero;
        // ui.nameText.text = buff.buffData.name;
        ui.describe.text = strDes;
        ui.nameText.text = strName;
        //{0} = value {1} = time {2} = maxNum {3} = delay
    }

}
