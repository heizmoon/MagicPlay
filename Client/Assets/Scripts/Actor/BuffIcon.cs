using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class BuffIcon : MonoBehaviour
{
    public List<Buff> buffs =new List<Buff>();
    public int buffID;
    public int buffNum;
    Buff buff;
    float lastTime;
    public Image iconHide;
    public Image iconShow;
    public Text textBuffNum;
    float fillNum;
    float currentTime=0;
    float invertTime=0;
    bool stop =true;
    Transform effect;
    Transform triggerEffect;
    string triggerEffectString;
    bool ifInit;
    Button button;
    public static event Action<int,string,int,ActorType> OnBuffAction;

    private void Awake() 
    {
        button =GetComponent<Button>();
        button.onClick.AddListener(ShowDetail);
    }
    void Start()
    {
        if(!ifInit)
        {
            InintBuffIcon();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(stop)
        {
            return;
        }
        currentTime+=Time.deltaTime;
        invertTime+=Time.deltaTime;
        iconShow.fillAmount -=fillNum*Time.deltaTime;
        if(buff.buffData.effectInterval>0&&invertTime>=buff.buffData.effectInterval)
        {
            OnEffectInterval();
        }
        if(currentTime>=lastTime)
        {
            OnEffectEnd();
        }
    }
    void InintBuffIcon()
    {
        ifInit = true;
        buff = buffs[0];
        buffID =buff.buffData.id;
        lastTime =buff.buffData.time;
        if(lastTime ==0)
        {
            fillNum =0;
            lastTime=9999;
        }
        else
        {
            fillNum = 1/lastTime;
        }
        if(buff.buffData.icon!="")
        {
            iconHide.sprite = Resources.Load("Texture/Skills/"+buff.buffData.icon,typeof(Sprite)) as Sprite;
            iconShow.sprite = Resources.Load("Texture/Skills/"+buff.buffData.icon,typeof(Sprite)) as Sprite;
        }
        if(buff.buffData.prefab!="")
        {
            effect =EffectManager.TryGetFromPos(buff.buffData.prefab,buff.target.hitPoint);
            if(effect ==null)
            {
                effect = EffectManager.TryGetFromPool(buff.buffData.prefab);
            }
            effect.SetParent(buff.target.hitPoint);
            effect.localPosition =Vector3.zero;
            effect.localScale =Vector3.one;
            effect.gameObject.SetActive(false);
        }
        triggerEffectString=buff.buffData.triggerEffect;
        
    }
    void OnEffectInterval()
    {
        invertTime = 0;
        Buff tempBuff =null;
        for (int i = 0; i < buffs.Count; i++)
        {
            if(buffs[i]!=null)
            {
                if(tempBuff!=null&&tempBuff.buffData.id!=buffs[i].buffData.id)
                {
                    buffs[i].OnBuffInterval();
                    tempBuff =buffs[i];

                }
                else if(tempBuff ==null)
                {
                    buffs[i].OnBuffInterval();
                    tempBuff =buffs[i];
                }
            }
        }
        if(triggerEffectString!="")
        {
            triggerEffect =EffectManager.TryGetFromPool(triggerEffectString);
            triggerEffect.SetParent(buff.target.hitPoint);
            triggerEffect.localPosition =Vector3.zero;
            triggerEffect.localScale =Vector3.one;
            triggerEffect.gameObject.SetActive(true);
        }
        if(OnBuffAction!=null)
        {
            OnBuffAction(buffID,"interval",buffNum,buff.target.actorType);
        }
    }
    public void OnEffectEnd()
    {
        stop =true;
        int num=0;
        
        // if(buffID==2002)
        // {
        //     Debug.LogWarning("移除buff2002");
        // }
        if(buffs.Count<=0)
        {
            return;
        }
        for (int i = buffs.Count-1; i >=0;i--)
        {
            num++;
            if(buffs[i] ==null)
            {
                buffs.Remove(buffs[i]);
                continue;
            }
            if(buffs[i]!=null)
            {   
                if(buffs[i].childrenBuffs!=null)
                {
                    for (int j = 0; j < buffs[i].childrenBuffs.Count; j++)
                    {
                        if(buffs[i].childrenBuffs[j]!=null&&buffs[i].childrenBuffs[j].buffIcon!=null)
                        buffs[i].childrenBuffs[j].buffIcon.OnEffectEnd();   
                    }
                }
                // if(buffID==15)
                // {
                //     Debug.LogWarning("end了几次？"+buffs.Count);
                // }
                buffs[i].OnBuffEnd();

                // buffs.Remove(buffs[i]);
            }
            
        }
        // if(buffs.Count>0)
        // {
        //     BuffManager.RemoveBuffFromActor(buffs[0],buffs[0].target);
        // }
        // Debug.LogWarningFormat("移除{0}个,共有{1}个",num,buffs.Count+num);

        EffectManager.TryThrowInPool(effect,true);
        if(OnBuffAction!=null)
        {
            OnBuffAction(buffID,"end",buffNum-num,buff.target.actorType);
        }
        Destroy(this.gameObject);
    }
    public void OnEffectReduce(int num)
    {
        if(OnBuffAction!=null)
        OnBuffAction(buffID,"end",buffNum-num,buff.target.actorType);
    }
    public void ResetTime()
    {
        if(!ifInit)
        {
            InintBuffIcon();
        }
        currentTime =0;
        invertTime =0;
        if(buffNum==1)
        {
            textBuffNum.text ="";
        }
        else
        {
            textBuffNum.text =buffNum.ToString();
        }
        
        stop =false;
        iconShow.fillAmount =1;
        // OnEffectBegin();//--问题所在：重置时间的时候并不需要Begin所有BUFF
    }
    public void OnEffectBegin(Buff currentBuff)
    {

        // for (int i = 0; i < buffs.Count; i++)
        // {
        //     if(buffs[i]!=null)
        //     {
        //         //如果是修改数值的BUFF，不应该再重启一次，如果是触发伤害的BUFF，可以重启？
        //         buffs[i].OnBuffBegin();//???所有相同BUFF都重启一次？
        //     }
        // }
        currentBuff.OnBuffBegin();
        if(effect!=null)
        {
            effect.gameObject.SetActive(true);
        }
        if(OnBuffAction!=null)
        {
            OnBuffAction(buffID,"begin",buffNum,buff.target.actorType);
        }
    }
    void ShowDetail()
    {
        //点击后在屏幕中间显示一个buff的说明文字，再次点击任意区域，说明文字消失
        //仅在时间停止时点击有效
        // if(Time.timeScale ==0)
        // {
            
        // }
        UIBuffDetail.CreateUIBuffDetail(buff);
        
    }
    
}
