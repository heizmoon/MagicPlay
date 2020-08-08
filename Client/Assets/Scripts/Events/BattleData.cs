using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleData : TypeEventTool
{
    public int MaxBattle;
    public enum BattleType
    {
        车轮战,
        限时战
    }
    public BattleType battleType;
    ///<summary>怪物列表</summary>
    public List<int> monsterList =new List<int>();
    public int level;
    public int scene;
    
    void Start()
    {
        MaxBattle =monsterList.Count;
        BattleEvent battleEvent= Instantiate((GameObject)Resources.Load("Prefabs/BattleEvent")).GetComponent<BattleEvent>();
        battleEvent.eventTanser =GetComponent<EventTanser>();
        battleEvent.battleData =this;
    }

    
}
