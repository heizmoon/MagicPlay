using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorBackUp : MonoBehaviour
{
    // Start is called before the first frame update
    public int HpMax;//最大HP
	public int MpMax;
    public float Crit;
    public List<int> UsingSkillsID;
    public List<int> abilities =new List<int>();
    public float autoReduceMPAmount =0;
    public int basicAttack;
    public int dealCardsNumber;

    public void BackUp(Actor actor)
    {
        HpMax =actor.HpMax;
        MpMax =actor.MpMax;
        Crit =actor.Crit;
        UsingSkillsID = actor.UsingSkillsID;
        abilities =actor.abilities;
        autoReduceMPAmount =actor.autoReduceMPAmount;
        // basicAttack
        dealCardsNumber =actor.dealCardsNumber;
    }
    public void Recover(Actor actor)
    {
        actor.dealCardsNumber =dealCardsNumber;
        actor.autoReduceMPAmount =autoReduceMPAmount;
        actor.abilities =abilities;
        actor.UsingSkillsID =UsingSkillsID;
        actor.Crit =Crit;
        actor.MpMax =MpMax;
        actor.HpMax =HpMax;
    }
}
