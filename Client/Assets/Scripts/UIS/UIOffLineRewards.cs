using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIOffLineRewards : MonoBehaviour
{
    int skillId;
    int num;
    public Text TextOffLineTime;
    public Text TextOffLineReward;
    public Button BTNGetReward;
    public Button BTNAd;
    bool enable =true;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnReceiveReward(int i)
    {
        if(!enable)
        {
            return;
        }
        if(i!=1)
        {
            //插播广告
        }
        // UIPractice.instance.AddSkillProficiency(skillId,num*i);
        gameObject.SetActive(false);
        enable =false;
    }
    ///<summary>初始化离线奖励窗口</summary>
    ///<param name ="seconds">离线时长：秒</param>
    ///<param name ="_id">正在练习的技能id</param>
    public void InitUI(int _id,int seconds)
    {
        enable =true;
        this.skillId =_id;
        num =seconds;
        int _h,_m,_s =0;
        _h =Mathf.FloorToInt(seconds/3600f);
        _m =(seconds/60)%60;
        _s =seconds%60;
        TextOffLineTime.text =string.Format("<color=#f22223>{0}</color>小时<color=#f22223>{1}</color>分<color=#f22223>{2}</color>秒",_h,_m,_s);
        string sName =SkillManager.instance.GetInfo(_id,"name");
        TextOffLineReward.text =string.Format("<color=#f22223>{0}</color>点<color=#f22223>{1}</color>熟练度",num,sName);
    }
    void OnApplicationQuit()
    {
        //如果退出游戏时，还没有领取离线奖励，那么自动领取1倍离线奖励
        if(enable)
        {
            OnReceiveReward(1);
        }
    }
}
