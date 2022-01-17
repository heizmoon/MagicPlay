using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public enum MapPointType
{
    events,
    battle,
    shop,
    boss,
    camp,
    treasure,
    elite

}
public class Map : MonoBehaviour,IBeginDragHandler,IDragHandler
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
    Vector3 orginPoint;
   float mapHeight =1280;
   private GameObject newBirdPoint;
    void Awake()
    {
        instance = this;
    }
    public void InitMap()
    {
        mapHeight =GetComponent<RectTransform>().sizeDelta.y;
        mapPoints =pointBase.GetComponentsInChildren<MapPoint>();
        // local.transform.localPosition =new Vector3(startPos.localPosition.x,startPos.localPosition.y+1280,0) ;
        local.transform.position = startPos.position;
        startPos.GetComponent<MapPoint>().isNowPoint = true;
        Sprite sprite =Instantiate(Resources.Load<Sprite>("Texture/Character/Char_0"+Player.instance.CharID));
        Debug.Log(Player.instance.playerActor.character.data.prefab);
        float _width = sprite.texture.width;
        float _height = sprite.texture.height;

        Image localImage = local.GetComponent<Image>();
        localImage.sprite =sprite;
        localImage.GetComponent<RectTransform>().sizeDelta =new Vector2(_width,_height)*0.6f;
        // +Player.instance.playerActor.character.data.prefab
        if(name=="Map_00(Clone)")
        {
            newBirdPoint =transform.Find("Pos/Point2-1/newBirdPoint").gameObject;
            newBirdPoint.SetActive(false);
        }
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
        if(BattleScene.instance.ifLevelUp)
        {
            Player.instance.playerActor.LevelUp();
            BattleScene.instance.ifLevelUp =false;
            // UIBasicBanner.instance.F_LevelUp.SetActive(true);
            // UIBasicBanner.instance.ShowNewTalent();
        }
        //---新手引导指向下一个地点
        if(newBirdPoint != null &&Main.instance.ifNewBird>5&&Main.instance.ifNewBird<=8)
        {
            newBirdPoint.SetActive(true);
        }
        
    }

    public void MoveLocal(MapPoint point)
    {
        local.DOMove(point.transform.position,0.6f,false);
        Refresh();
        BattleScene.instance.steps++;
        
        // BattleScene.instance.exp+= Configs.instance.everyStepAddEXP;
        // if(BattleScene.instance.exp>=CharacterManager.instance.GetLevelData(Player.instance.playerActor.level).exp)
        // {
        //    BattleScene.instance.ifLevelUp = true; 
        // }
        StartCoroutine(WaitForPoint(point));
    }
    IEnumerator WaitForPoint(MapPoint point)
    {
        yield return new WaitForSeconds(1f);
        if(newBirdPoint!=null&&newBirdPoint.activeSelf)
        {
            newBirdPoint.SetActive(false);
        }
        switch(point.mapPointType)
        {
            case MapPointType.battle:
            BattleScene.instance.InitBattle(point.realID,sceneID,0);
            break;
            case MapPointType.boss:
            BattleScene.instance.InitBattle(point.realID,sceneID,2);
            break;
            case MapPointType.camp:
            BattleScene.instance.InitCamp();
            break;
            case MapPointType.events:
            BattleScene.instance.InitRandomEvent();
            break;
            case MapPointType.shop:
            BattleScene.instance.InitShop(point.idList[0]);//此处需要之后优化为对应每个角色
            break;
            case MapPointType.treasure:
            BattleScene.instance.InitTreasure();
            break;
            case MapPointType.elite:
            BattleScene.instance.InitBattle(point.realID,sceneID,1);
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
    
    public void OnBeginDrag (PointerEventData eventData)
    {
        orginPoint = new Vector3(0,Input.mousePosition.y,0);
    }
    public void OnDrag(PointerEventData eventData)
    {
        // Debug.Log("dragMap:"+transform.localPosition.y);
        Vector3 currentPoint  =Input.mousePosition;
        Vector3 moveDir = currentPoint - orginPoint;
        moveDir = new Vector3(0, moveDir.y, 0);
        if (moveDir == Vector3.zero)
        {
            return;
        }
        
        moveDir.y = moveDir.y /16;
        transform.Translate(moveDir);
        if(transform.GetComponent<RectTransform>().anchoredPosition.y>0)
        {
            transform.GetComponent<RectTransform>().anchoredPosition =Vector2.zero;
        }
        if(transform.GetComponent<RectTransform>().anchoredPosition.y<-mapHeight+1280)
        {
            transform.GetComponent<RectTransform>().anchoredPosition =new Vector2(0,-mapHeight+1280);
        }
        if (orginPoint != currentPoint)
        {
            orginPoint = currentPoint;
        }
    }
    
    
}
