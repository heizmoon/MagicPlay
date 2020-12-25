using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Summoned : MonoBehaviour
{
    float LifeTime =15f;
    int Damage =2;
    //每隔多长时间攻击一次
    float attackSpeed =3f; 
    void Start()
    {
        //事件订阅
        Player.instance.playerActor.OnUpdateSummonedAttack+=OnExtendLifeTime;
        Player.instance.playerActor.OnUpdateSummonedSpeed+=OnExtendAttackSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnExtendLifeTime(int num)
    {

    }
    void OnExtendAttackSpeed(float num)
    {

    }
    void OnDeath()
    {
        //解除订阅
        Player.instance.playerActor.OnUpdateSummonedAttack-=OnExtendLifeTime;
        Player.instance.playerActor.OnUpdateSummonedSpeed-=OnExtendAttackSpeed;

        
    }
}
