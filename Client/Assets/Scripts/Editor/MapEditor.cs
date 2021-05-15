using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using System.IO;

//继承自EditorWindow类
public class MapEditor : EditorWindow 
{
    string bugReporterName = "";
    int nextPointNumber =3;
    GameObject mapPoint;
    int totalLine;//排除了第一层和最后一层
    Transform _pos;
    Transform _line;
    GameObject road;
    List<List<MapPoint>> mapList =new List<List<MapPoint>>();

    //利用构造函数来设置窗口名称
    MapEditor()
    {
        this.titleContent = new GUIContent("地图编辑器");
    }
	
    //添加菜单栏用于打开窗口
    [MenuItem("地图编辑器/MapEditor")]
    static void showWindow()
    {
        EditorWindow.GetWindow(typeof(MapEditor));
        

    }
    void OnGUI()
    {
        GUILayout.BeginVertical();

        //绘制标题
        GUILayout.Space(10);
        GUI.skin.label.fontSize = 24;
        GUI.skin.label.alignment = TextAnchor.MiddleCenter;
        GUILayout.Label("地图编辑器");

        

        //绘制对象
        // GUILayout.Space(10);
        // mapPoint = (GameObject)EditorGUILayout.ObjectField("MapPoint",mapPoint,typeof(GameObject),true);
        

        // //绘制描述文本区域
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        GUI.skin.label.fontSize = 16;
        GUI.skin.label.alignment = TextAnchor.MiddleLeft;
        GUILayout.Label("下一行点位数");
        nextPointNumber= EditorGUILayout.IntField(nextPointNumber);
        GUILayout.EndHorizontal();

        EditorGUILayout.Space();

        //添加名为"Save Bug"按钮，用于调用SaveBug()函数
        if(GUILayout.Button("新增一行")){
            CreateALine();
        }

        EditorGUILayout.Space();

        if(GUILayout.Button("刷新路径")){
            Refeash();
        }

        GUILayout.EndVertical();
        MapPoint _mapPoint =UnityEditor.SceneManagement.StageUtility.GetCurrentStageHandle().FindComponentOfType<MapPoint>();
        if(_mapPoint ==null)
        return;
        mapPoint =_mapPoint.gameObject;
        _pos =mapPoint.transform.parent;
        _line =_pos.parent.Find("Line");

        totalLine =GetNumber();
    }
    void CreateALine()
    {
        if(mapPoint ==null)
        return;
        
        for (int i = 0; i < nextPointNumber; i++)
        {
            GameObject g = Instantiate(mapPoint);
            g.transform.SetParent(mapPoint.transform.parent);
            g.name = "Point"+(totalLine+1)+"-"+(i+1);
            float _x =0;
            if(i==0&&nextPointNumber==1)
                _x =0;
            else if(i==0&&nextPointNumber==2)
                _x =-75;
            else if(i==0&&nextPointNumber==3)
                _x =-150;
            else if(i==1&&nextPointNumber==2)
                _x =75;
            else if(i==1&&nextPointNumber==3)
                _x =0;
            else
                _x =150;
            g.transform.localPosition = new Vector3(_x,(totalLine+2)*110-1280,0);
            g.GetComponent<MapPoint>().nextPoint =new GameObject[0];
        }
        
    }
    void Refeash()
    {
        totalLine =GetNumber();
        //删除所有旧连线
        for (int i = _line.childCount-1; i >0; i--)
        {
            DestroyImmediate(_line.GetChild(0).gameObject);
        }
        road =_line.GetChild(0).gameObject;
        mapList = new List<List<MapPoint>>();
        for (int i = 0; i < totalLine+2; i++)
        {
            List<MapPoint> _list =new List<MapPoint>();
            mapList.Add(_list);
        }
        foreach (var item in _pos.GetComponentsInChildren<MapPoint>())
        {
            item.SetIcon();
            SaveList(item);
        }
        for (int i = 0; i < mapList.Count; i++)
        {
            for (int j = 0; j < mapList[i].Count; j++)
            {
                List<GameObject> nextList = GetNextPoint(mapList[i][j],i,mapList[i].Count);
                mapList[i][j].AutoSetNextPoint(nextList);
                CreateLine(mapList[i][j]);
            }
        }
        DestroyImmediate(road.gameObject);
        //创建新连线：规则-创建一条线，位置与点的位置相同，点有几个nextPoint，创建几条线 根据nextPoint 的位置，设置角度
        

        GameObject p =_pos.Find("Point").gameObject;
        EditorGUIUtility.PingObject(p);
		Selection.activeGameObject = p;
    }
    void SaveList(MapPoint point)
    {
        //如果下一行的点位数与本行相同，那么一一对应
        //如果下一行只有1个点，那么都连接到这个点
        int _y = GetLine(point.transform);
        Debug.Log(point.name+","+_y);
        mapList[_y].Add(point);
        return;
    }
    ///<summary>自动获取一个点的下一个点集合</summary>
    ///<param name ="point">当前的点</param>
    ///<param name ="currentLine">当前是第几行</param>
    ///<param name ="LineNumber">当前这一行有几个点</param>
    List<GameObject> GetNextPoint(MapPoint point,int currentLine,int LineNumber)
    {
        if(point.name =="Point_end")
        return null;

        List<GameObject> _list = new List<GameObject>();
        int _x = GetIndex(point.transform);
        //如果当前这一行的点的数量为1，那么下一行的所有点都是nextPoint
        Debug.Log(point.name+"点数："+LineNumber+"_x="+_x);
        if(LineNumber==1)
        {
            for (int i = 0; i < mapList[currentLine+1].Count; i++)
            {
                _list.Add(mapList[currentLine+1][i].gameObject);  
            }
        }
        else if(mapList[currentLine+1].Count ==LineNumber)//如果当前这一行的点的数量与下一行的相同，那么一一对应
        {
            for (int i = 0; i < mapList[currentLine+1].Count; i++)
            {
                Debug.Log("测试_x="+GetIndex(mapList[currentLine+1][i].transform));
                if(GetIndex(mapList[currentLine+1][i].transform) == _x)
                {
                    _list.Add(mapList[currentLine+1][i].gameObject);
                    Debug.Log(_list[0].name);
                    return _list;
                }
            }
        }
        else if(mapList[currentLine+1].Count ==1)
        {
             _list.Add(mapList[currentLine+1][0].gameObject);
        }
        return _list;
    }
    int GetNumber()
    {
        int number = 0;
        int childIndex =0;
        Transform _lastPoint;
        for (int i = 0; i < _pos.childCount; i++)
        {
            _lastPoint = _pos.GetChild(i);
            if(_lastPoint.name =="Point_end")
            continue;
            if(_lastPoint.GetSiblingIndex()>number)
            {
                number =  _lastPoint.GetSiblingIndex();
                childIndex =i;
            }
        }
        _lastPoint =_pos.GetChild(childIndex);
        
        
        number =GetLine(_lastPoint);
        
        return number;
    }
    int GetLine(Transform _point)
    {
        int number =0;
        if(_point.name == "Point_end")
        {
            number = totalLine+1;
        }
        else if(_point.name == "Point")
        {
            number = 0;
        }
        else
        {
            number =int.Parse(_point.name.Split('t')[1].Split('-')[0]);
        }
        return number;
    }
    int GetIndex(Transform _point)
    {
        int number =0;
        if(_point.name == "Point_end")
        {
            number = 0;
        }
        else if(_point.name == "Point")
        {
            number = 0;
        }
        else
        {
            number =int.Parse(_point.name.Split('t')[1].Split('-')[1]);
        }
        return number;
    }
    void CreateLine(MapPoint point)
    {
        if(point.nextPoint== null||point.nextPoint.Length<=0)
        return;
        for (int i = 0; i < point.nextPoint.Length; i++)
        {
            if(point.nextPoint[i]==null)
            Debug.LogError("这个point有问题："+point.name+"，第"+i+"个");
            
            GameObject r = Instantiate(road);
            r.name ="road";
            r.transform.SetParent(_line);
            r.transform.localPosition = new Vector3(point.transform.localPosition.x,point.transform.localPosition.y,0);
            r.GetComponent<RectTransform>().sizeDelta =new Vector2(12,Vector2.Distance(point.transform.localPosition,point.nextPoint[i].transform.localPosition));
            Vector3 targetDir =  point.nextPoint[i].transform.localPosition-point.transform.localPosition;
            float angle = Vector3.Angle(point.transform.up,targetDir);
            int _angle =(int)(angle*100);
            angle =_angle/100f;
            Vector3 vector;
            if(Math.Abs(point.transform.localPosition.y)>Math.Abs(point.nextPoint[i].transform.localPosition.y))
            {
                vector = Vector3.Cross (point.transform.localPosition,point.nextPoint[i].transform.localPosition);
            }
            else
            {
                vector = Vector3.Cross (point.nextPoint[i].transform.localPosition,point.transform.localPosition);
            }
            // Debug.Log(vector);
            float dir = vector.z < 0 ? 1 : -1;
            angle *= dir;
            r.transform.localEulerAngles = new Vector3(0,0,angle);
        }
    }
}
