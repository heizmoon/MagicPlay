using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleMagic : MonoBehaviour
{
    public GameObject skillPage;//对应的技能页
    public string tabName;//标签名
    public Text textName;
    public Image icon;
    public Toggle toggle;
    public int genre;
    void Start()
    {
        textName.text =tabName;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
