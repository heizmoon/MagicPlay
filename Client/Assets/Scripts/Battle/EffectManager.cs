using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectManager : MonoBehaviour
{
    //特效在播放完毕后自动进入pool中待再次使用
    //从pool中选择合适的特效使用
    public static Transform pool;
    public float EffectTime;
    public bool ifDestory; 
    Timer timer;
    Transform start;
    Transform end;
    float speed;
    bool fly;
    Actor actor;
    string childEffect;
    bool channelEffect;
    void Awake()
    {
        
    }
    void Start()
    {
        pool =GameObject.Find("Pool").transform;
        
        ReadToThorw();
        
    }
    void Active()
    {
        
    }
    public void SetEffectTime(float time)
    {
        EffectTime =time;
    }
    public void ReadToThorw()
    {
        if(EffectTime>0)
        {
            StartCoroutine(ThrowToPool());
        }
    }
    void Update()
    {
        if(fly)
        {
            
            transform.position=Vector3.MoveTowards(transform.position,end.position,speed*Time.deltaTime);
            if(Vector3.Distance(transform.position,end.position)<=1f)
            {
                fly =false;
                gameObject.SetActive(false);
                transform.SetParent(pool);
                if(childEffect =="")
                {
                    return;
                }
                Transform e = TryGetFromPool(childEffect,actor.target);
                e.SetParent(end);
                e.localPosition =Vector3.zero;
                e.localScale =Vector3.one;
                e.gameObject.SetActive(true);
            }
        }
    }
    IEnumerator ThrowToPool()
    {
        yield return new WaitForSeconds(EffectTime);
        gameObject.SetActive(false);
        transform.SetParent(pool);
        // transform.localPosition =new Vector3(1000,1000,0);
        // transform.localScale =new Vector3(1,1,1);
        
    }
    public static Transform TryGetFromPool(string effectName,Actor actor)//如果池子里面有该特效，那么就拿出来用
    {
        if(effectName =="")
        {
            return null;
        }
        if(!pool)
        {
            pool =GameObject.Find("Pool").transform;
        }
        if(pool.childCount>0)
        {
            for(int i=0;i<pool.childCount;i++)
            {
                if(pool.GetChild(i).gameObject.name.Split('(')[0] ==effectName)
                {
                    pool.GetChild(i).gameObject.SetActive(true);
                    pool.GetChild(i).GetComponent<EffectManager>().actor = actor;
                    return pool.GetChild(i);
                }
            }
        }
        Transform ts =((GameObject)Instantiate(Resources.Load("Prefabs/Effects/"+effectName))).transform;
        ts.GetComponent<EffectManager>().actor =actor;
        return ts;
    }
    public static Transform TryGetFromPos(string effectName, Transform pos)//如果该位置有该特效，那么就拿出来用
    {
        if(effectName =="")
        {
            return null;
        }
        if(!pos)
        {
            return null;
        }
        if(pos.childCount>0)
        {
            for(int i=0;i<pos.childCount;i++)
            {
                if(pos.GetChild(i).gameObject.name.Split('(')[0] ==effectName)
                {
                    pos.GetChild(i).gameObject.SetActive(true);
                    return pos.GetChild(i);
                }
            }
        }

        return null;
    }
    public static void TryThrowInPool(string effectName,Transform pos)//从某个位置找到特效，移动到池子里
    {
        if(!pool)
        {
            pool =GameObject.Find("Pool").transform;
        }
        if(pos.childCount==0)
        {
            return;
        }
        for(int i=0;i<pos.childCount;i++)
        {
            if(pos.GetChild(i).gameObject.name.Split('(')[0] ==effectName)
            {
                // Debug.LogFormat("!抓到了！");
                pos.GetChild(i).gameObject.SetActive(false);
                pos.GetChild(i).SetParent(pool);
            }
        }
    }
    ///<summary>将某个位置的所有特效，移动到池子里</summary>
    public static void TryThrowInPool(Transform pos)
    {
        if(!pool)
        {
            pool =GameObject.Find("Pool").transform;
        }
        if(pos.childCount==0)
        {
            return;
        }
        for(int i=0;i<pos.childCount;i++)
        {
            pos.GetChild(i).gameObject.SetActive(false);
            pos.GetChild(i).SetParent(pool);
        }
    }
    ///<summary>将指定的一个特效，移动到池子里</summary>
    public static void TryThrowInPool(Transform effect,bool b)
    {
        if(!pool)
        {
            pool =GameObject.Find("Pool").transform;
        }
        if(effect== null)
        {
            return;
        }
        effect.gameObject.SetActive(false);
        effect.SetParent(pool);
    }
    public static void ClearPool()
    {
        for(int i=0;i<pool.childCount;i++)
        {
            Destroy(pool.GetChild(i).gameObject);
        }
    }
    public static void CastEffect(Transform effect,Transform hitPoint,float delay,string hitEffect)
    {
        if(effect==null)
        return;
        EffectManager e = effect.GetComponent<EffectManager>();
        e.end =hitPoint;
        float distance= Vector3.Distance(effect.position,e.end.position);
        e.speed =distance/delay;
        e.childEffect =hitEffect;
        e.fly =true;
        if(e.actor==Player.instance.playerActor)
        {
            e.transform.localScale = Vector3.one;
            for (int i = 0; i < e.transform.childCount; i++)
            {
                e.transform.GetChild(i).localScale =Vector3.one;
            }
        }
        else
        {
            e.transform.localScale = new Vector3(-1,1,1);
            for (int i = 0; i < e.transform.childCount; i++)
            {
                e.transform.GetChild(i).localScale =new Vector3(-1,1,1);
            }
        }
        
    }
    public static void CastEffect(Transform effect,float delay)
    {
        if(effect==null)
        return;
        EffectManager e = effect.GetComponent<EffectManager>();
        e.fly =false;
        if(e.actor!=Player.instance.playerActor)
        {
            e.transform.localScale = Vector3.one;
            for (int i = 0; i < e.transform.childCount-1; i++)
            {
                e.transform.GetChild(i).localScale =Vector3.one;
            }
        }
        else
        {
            e.transform.localScale = new Vector3(-1,1,1);
            for (int i = 0; i < e.transform.childCount-1; i++)
            {
                e.transform.GetChild(i).localScale =new Vector3(-1,1,1);
            }
        }        
    }
    // public void effectFly()
    // {
    //     this.transform.localPosition+=new Vector3(1,1,1);
    // }
    public static void ClearChannelEffect(Transform pos)
    {
        if(pos==null)
        {
            return;
        }
        for (int i = 0; i < pos.childCount; i++)
        {
            if(pos.GetChild(i).GetComponent<EffectManager>().channelEffect)
            {
                pos.GetChild(i).gameObject.SetActive(false);
                pos.GetChild(i).SetParent(pool);
            }
        }
    }
}
