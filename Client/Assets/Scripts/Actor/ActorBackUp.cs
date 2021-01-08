using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorBackUp : MonoBehaviour
{
    // Start is called before the first frame update
    public int HpMax;//最大HP
	public int MpMax;
    public float Crit;
    public float dodge;
    public List<int> UsingSkillsID;
    public List<int> abilities =new List<int>();
    public float autoReduceMPAmount =0;
    public int basicAttack;
    public int basicDefence;

    public int dealCardsNumber;

//开战前进行backUp
    public void BackUp(Actor actor)
    {
        HpMax =actor.HpMax;
        MpMax =actor.MpMax;
        Crit =actor.Crit;
        dodge =actor.dodge;
        UsingSkillsID = actor.UsingSkillsID;
        abilities =actor.abilities;
        autoReduceMPAmount =actor.autoReduceMPAmount;
        basicAttack =actor.basicAttack;
        basicDefence =actor.basicDefence;
        dealCardsNumber =actor.dealCardsNumber;
    }
//战斗结束后进行recover
//如果出现角色当前生命值大于最大生命值的情况，角色当前生命值要回归到最大生命值
    public void Recover(Actor actor)
    {
        actor.dealCardsNumber =dealCardsNumber;
        actor.autoReduceMPAmount =autoReduceMPAmount;
        actor.abilities =abilities;
        actor.UsingSkillsID =UsingSkillsID;
        actor.Crit =Crit;
        actor.MpMax =MpMax;
        actor.HpMax =HpMax;
        if(actor.HpCurrent>HpMax)
        actor.HpCurrent =actor.HpMax;
        if(actor.MpCurrent>MpMax)
        actor.MpCurrent =actor.MpMax;
        actor.basicAttack = basicAttack;
        actor.basicDefence = basicDefence;
    }
}
