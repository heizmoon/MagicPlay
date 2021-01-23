using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public enum MapPointType
{
    events,
    battle,
    shop,
    boss,
    camp,
    treasure

}
public class Map : MonoBehaviour
{
    //层级关系：Scene → Map → Point
    //一个Scene 拥有 3张Map ,每个Map上有数个Point
    public static Map instance;
    public Transform local;
    public Transform startPos;
    public Transform pointBase;
    public int sceneID;
    MapPoint[] mapPoints;
    public string nextMap;
    void Awake()
    {
        instance = this;
    }
    public void InitMap()
    {
        mapPoints =pointBase.GetComponentsInChildren<MapPoint>();
        local.transform.localPosition =startPos.localPosition;
        startPos.GetComponent<MapPoint>().isNowPoint = true;
        Refresh();
    }
    public void Refresh()
    {
        foreach (var item in mapPoints)
        {
            item.SetButton(false);
        }
        foreach (var item in mapPoints)
        {
            item.Refresh();
        }
    }
    public void MoveLocal(MapPoint point)
    {
        local.DOLocalMove(point.transform.localPosition,0.75f,false);
        Refresh();
        BattleScene.instance.steps++;
        StartCoroutine(WaitForPoint(point));
    }
    IEnumerator WaitForPoint(MapPoint point)
    {
        yield return new WaitForSeconds(1f);
        
        switch(point.mapPointType)
        {
            case MapPointType.battle:
            BattleScene.instance.InitBattle(point.realID,sceneID,false);
            break;
            case MapPointType.boss:
            BattleScene.instance.InitBattle(point.realID,sceneID,true);
            break;
            case MapPointType.camp:
            BattleScene.instance.InitCamp();
            break;
            case MapPointType.events:
            BattleScene.instance.InitRandomEvent();
            break;
            case MapPointType.shop:
            BattleScene.instance.InitShop();
            break;
            case MapPointType.treasure:
            BattleScene.instance.InitTreasure();
            break;
        }
        point.isNowPoint = true;
        CloseMap();
    }
    void CloseMap()
    {
        gameObject.SetActive(false);
    }
    public void DestoryMap()
    {
        Destroy(gameObject);
    }
}
