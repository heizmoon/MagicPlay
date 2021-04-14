using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MapPoint : MonoBehaviour
{
    Button button;
    Image image;
    public MapPointType mapPointType;
    public GameObject[] nextPoint;
    public bool isNowPoint;
    //事件，怪物，随机id列表
    public int[] idList;
    public int realID;
    bool canMove;
      
    void Awake()
    {
        button =GetComponent<Button>();
        image =GetComponent<Image>();
        button.onClick.AddListener(OnButton);
    }
    void Start()
    {

        RandomID();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void Refresh()
    {
        //根据状态，将下一步可到达的地点设置为可点击
        if(isNowPoint)
        {
            foreach (var item in nextPoint)
            {
                item.GetComponent<MapPoint>().SetButton(true);
            }
        }
        isNowPoint =false;
    }
    public void SetButton(bool b)
    {
        canMove = b;
        if(b)
        {
            image.sprite =Resources.Load<Sprite>("Texture/Map/"+mapPointType+"_on");
        }
        else
        {
            image.sprite =Resources.Load<Sprite>("Texture/Map/"+mapPointType+"_off");
        }
    }
    public void OnButton()
    {
        if(canMove)
        //local移动到此为止
        Map.instance.MoveLocal(this);
    }
    void RandomID()
    {
        if(idList!=null)
        {
            int r =Random.Range(0,idList.Length);
            realID =idList[r];
        }
    }
}
