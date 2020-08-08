using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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
        if(invertTime>=buff.buffData.effectInterval)
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
        ifInit = true;
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
        
    }
    public void OnEffectEnd()
    {
        stop =true;
        int num=0;
        // buffs.RemoveAll(null);
        // Debug.LogWarningFormat("开始移除,共有{0}个",buffs.Count);
        for (int i = 0; i < buffs.Count; i++)
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
                buffs[i].OnBuffEnd();
                // buffs.Remove(buffs[i]);
            }
            
        }
        if(buffs.Count>0)
        {
            BuffManager.RemoveBuffFromActor(buffs[0],buffs[0].target);
        }
        // Debug.LogWarningFormat("移除{0}个,共有{1}个",num,buffs.Count+num);

        EffectManager.TryThrowInPool(effect,true);
        Destroy(this.gameObject);
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
        OnEffectBegin();
    }
    void OnEffectBegin()
    {
        for (int i = 0; i < buffs.Count; i++)
        {
            if(buffs[i]!=null)
            {
                buffs[i].OnBuffBegin();
            }
        }
        if(effect!=null)
        {
            effect.gameObject.SetActive(true);
        }
    }
    
}
