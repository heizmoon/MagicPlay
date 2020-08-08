using UnityEngine;
using System.Collections;
using System.Data;
using System.IO;
using Excel;
using System.Collections.Generic;
using Data;


public struct CharacterDataBase
{
    //“characterData":"id,_Alive,_marriage,_like,_infoLevel"
    public int id;
    public bool _Alive;
    public bool _marriage;
    public int _like;
    public int _infoLevel;
}
public class CharacterManager : MonoBehaviour

{
    public static CharacterManager instance;
    //(2)根据需要，定义自己的结构        
    public List<CharacterData> dataList;
    public List<CharacterDataBase> dataBase;
    public CharacterData infoTd = new CharacterData();

    
    void Awake()
    {   
        instance =this;
        dataList = new List<CharacterData>();
        LoadInfo();
    }
    void HandleDynamicInfo()
    {
        for (int i = 0; i < dataList.Count; i++)
        {
            CharacterData data =dataList[i];
            data._birthday =DateManager.ConvertStringToDate(data.birthday);
            data._deathday =DateManager.ConvertStringToDate(data.deathday);
            data._marriageDay =DateManager.ConvertStringToDate(data.marriageDay);
            dataList[i] =data;
        }
       
    }


    
    //加载记录表中信息
    public void LoadInfo()
    {             
        LoadDatabase();
        CharacterDataSet manager = Resources.Load<CharacterDataSet>("DataAssets/Character");
        foreach (var item in manager.dataArray)
        {
            dataList.Add(item);
        }
        HandleDynamicInfo();
    }
    void LoadDatabase()
    {
        dataBase =new List<CharacterDataBase>();
        string s =PlayerPrefs.GetString("characterData");
        if(s=="")
        {
            return;
        }
        string[] ss =s.Split('|');
        foreach (var item in ss)
        {
            CharacterDataBase datas =new CharacterDataBase();
            datas.id = int.Parse(item.Split(',')[0]);
            datas._Alive = item.Split(',')[1]=="1"?true:false;
            datas._marriage =item.Split(',')[2]=="1"?true:false;
            datas._like =int.Parse(item.Split(',')[3]);
            datas._infoLevel =int.Parse(item.Split(',')[4]);
            dataBase.Add(datas);
        }

    }
    
    public void RefreashCharacterState()
    {
        for (int i = 0; i < dataList.Count; i++)
        {
            dataList[i] =RefreashNowInformations(dataList[i]);
            Debug.LogFormat("金钱：{0},infoLevel:{1}",dataList[i]._nowGold,dataList[i]._infoLevel);
        }
    }
    public CharacterData RefreashNowInformations(CharacterData data)
    {
        data._Alive =GetAlive(data);
        data._state =GetNowState(data);
        data._age =UpdateAge(data);
        data._state =GetNowState(data);
        data._nowSkill= GetNowSkill(data);
        data._nowSkillLevel=GetNowSkillLevel(data);
        data._nowRank= GetNowRank(data);
        data._hp =GetNowHp(data);
        data._mp =GetNowMp(data);
        data._nowGold= GetNowGold(data);
        data._resistance= Getresistance(data);
        data._like =GetLike(data);
        data._infoLevel =GetInfoLevel(data);
        Debug.LogFormat("data:{0},姓名={1}，岁数={2},hp={3},gold={4},infoLevel={5}",data.id,data.name,data._age,data._hp,data._nowGold,data._infoLevel);
        return data;
    }
    public string GetInfo(int id ,string content)
    {
        if(id ==0)
        {
            return Player.instance.playerName;
        }
        foreach(var item in dataList)
        {
            if(item.id==id)
            {
                switch(content)
                {
                    case "name":
                    
                    return item.name.ToString();

                }     
            }    
        }
        return "";
    }
    public CharacterData GetInfo(int id)
    {
        
       foreach(var item in dataList)
        {
            if(item.id==id)
            {
                return item;       
            } 
        }
        return infoTd;
    }
    public static int UpdateAge(CharacterData data)
    {
        if(!data._Alive)
        {
            return 0;
        }
        int y = DateManager.instance.now.year - data._birthday.year;
        if(DateManager.instance.now.month>data._birthday.month)
        {
            return y;
        }
        else if(DateManager.instance.now.month<data._birthday.month)
        {
            return y-1;
        }
        else
        {
            if(DateManager.instance.now.day>=data._birthday.day)
            {
                return y;
            }
            else
            return y-1;
        }
    }
    public Date ModifierMarriage(CharacterData data)
    {
        Date date =new Date();
        //如果表里面有数据，那么就读取表里的数据
        //如果表里面没有数据，判断是否处于等待玩家状态
        if(data._waitingForMarriage)
        {

        }
        if(data._marriage)
        {

        }

        return date;
    }
    public bool GetAlive(CharacterData data)
    {
        //先判断PlayerPrefs中是否已经死亡，如果没有死亡，再判断当前时间和表中的死日，是否死日大于当前时间
        if(dataBase!=null&&dataBase.Count>0)
        {
            foreach (var item in dataBase)
            {
                if(item.id==data.id)
                {
                    if(item._Alive)
                    {
                        break;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
        
        if(DateManager.WhichDateFirst(data._deathday,DateManager.instance.now))
        {
            return true;
        }
        else
        {
            return false;
        }
    } 
    public static int GetNowState(CharacterData data)
    {
        //判断当前时间与阶段时间，看当前处于什么时间
        int now = DateManager.ConvertDateToInt(DateManager.instance.now);
        string[] dt = data.updateTime.Split('|');
        List<int> dts =new List<int>();
        for (int i = 0; i < dt.Length; i++)
        {   
           dts.Add(DateManager.ConvertDateToInt(DateManager.ConvertStringToDate(dt[i])));
        }
        for (int i = 0; i < dts.Count; i++)
        {
            if(now< dts[i])
            {
                data._state =i;
                return i;
            }
        }
        data._state=dts.Count;
        return dts.Count;
    }
    public static int GetNowRank(CharacterData data)
    {
        if(data.rank=="")
        {
            return 0;
        }
        string[] ranks =data.rank.Split('|');
        return int.Parse(ranks[data._state]);
    }
    public static int GetNowGold(CharacterData data)
    {
        if(data.gold=="")
        {
            return 0;
        }
        string[] golds =data.gold.Split('|');
        return int.Parse(golds[data._state]);
    }
    public static List<int> GetNowSkill(CharacterData data)
    {
        
        if(data.skillList=="")
        {
            return null;
        }
        data._nowSkill =new List<int>();
        string[] skills =data.skillList.Split('|');
        string[] nowSkill =skills[data._state].Split(',');
        for (int i = 0; i < nowSkill.Length; i++)
        {
            data._nowSkill.Add(int.Parse(nowSkill[i]));
        }
        return data._nowSkill;
    }
    public static int GetNowSkillLevel(CharacterData data)
    {
        data._nowSkillLevel=data._age*data.skillGrow;
        return data._nowSkillLevel;
    }
    public static int GetNowHp(CharacterData data)
    {
        data._hp=data._age*data.hpGrow;
        return data._hp;
    }
    public static int GetNowMp(CharacterData data)
    {
        data._mp=data._age*data.mpGrow;
        return data._mp;
    }
    public static List<int> Getresistance(CharacterData data)
    {
        if(data.resistances=="")
        {
            return null;
        }
        string[] s = data.resistances.Split(',');
        List<int> r =new List<int>();
        foreach (var item in s)
        {
            r.Add(int.Parse(item));
        }
        for (int i = 0; i < r.Count; i++)
        {
            data._resistance[i]=data._age*data.resistanceGrow*r[i];
        }
        return data._resistance;
    }
    
    public int GetLike(CharacterData data)
    {
        //从PlayerPrefs中读取，若没有则为0；
        if(dataBase!=null&&dataBase.Count>0)
        {
            foreach (var item in dataBase)
            {
                if(item.id==data.id)
                {
                    return item._like;
                }
            }
        }
        return 0;
    }
    public int GetInfoLevel(CharacterData data)
    {
        //从PlayerPrefs中读取，若没有则为0；
        if(dataBase!=null&&dataBase.Count>0)
        {
            foreach (var item in dataBase)
            {
                if(item.id==data.id)
                {
                    return item._infoLevel;
                }
            }
        }
        return 0;
    }
    public void ChangeLikeInDatabase(int id,int vaule)
    {
        TryCreateDatabase(id);

        for (int i = 0; i < dataBase.Count; i++)
        {
            if(dataBase[i].id ==id)
                {
                    var data =dataBase[i];
                    data._like =vaule;
                    dataBase[i] =data;
                }   
        }
    }
    public void ChangeInfoLevelInDatabase(int id)
    {
        TryCreateDatabase(id);
        Debug.LogFormat("进来了@");
        for (int i = 0; i < dataBase.Count; i++)
        {
            if(dataBase[i].id ==id)
            {
                var data =dataBase[i];
                if(data._infoLevel<4)
                {
                    data._infoLevel ++;
                }
                dataBase[i] =data;
                Debug.LogFormat("已经增加infoLevel={0}",dataBase[i]._infoLevel);
            }   
        }
    }
    ///<summary>修改characterDatabase中的数据</summary>
    ///<param name ="id">database的id,如果database中没有该id，会自动添加一个</param>
    ///<param name ="vaule">alive,like,infoLevel,marriage</param>
    public void ChangeInfomationInDatabase(int id, string vaule)
    {
        if(vaule =="")
        {
            return;
        }
        TryCreateDatabase(id);
        bool hasAliveInfo=false;
        bool hasLikeInfo =false;
        bool hasLevelInfo =false;
        bool hasmarriageInfo =false;

        bool _Alive =true;
        if(vaule.Split(',')[0]!="")
        {
            hasAliveInfo=true;
            _Alive=vaule.Split(',')[0]=="1"?true:false;
        }
        int _like =0;
        if(vaule.Split(',')[1]!="")
        {
            hasLikeInfo =true;
            _like =int.Parse(vaule.Split(',')[1]);
        }
        int _infoLevel=0;
        if(vaule.Split(',')[2]!="")
        {
            hasLikeInfo =true;
            _infoLevel =int.Parse(vaule.Split(',')[2]);
        }
        bool _marriage =false;
        if(vaule.Split(',')[3]!="")
        {
            hasmarriageInfo=true;
            _marriage=vaule.Split(',')[3]=="1"?true:false;
        }
        for (int i = 0; i < dataBase.Count; i++)
        {
            if(dataBase[i].id ==id)
                {
                    var data =dataBase[i];
                    if(hasAliveInfo)
                    {
                        data._Alive =_Alive;
                    }
                    if(hasLikeInfo)
                    {
                        data._like =_like;
                    }
                    if(hasLevelInfo)
                    {
                        data._infoLevel =_infoLevel;
                    }
                    if(hasmarriageInfo)
                    {
                        data._marriage =_marriage;
                    }
                    dataBase[i] =data;
                }   
        }
    }
    void TryCreateDatabase(int id)
    {
        if(dataBase.Count>0)
        {
            foreach (var item in dataBase)
            {
                if(item.id ==id)
                {
                    Debug.LogFormat("database已经有这个id了");
                    return;
                }
            }
        }
        
        CharacterDataBase db =new CharacterDataBase();
        db.id =id;
        var data =GetInfo(id);
        db._Alive =data._Alive;
        db._like=data._like;
        db._infoLevel=data._infoLevel;
        db._marriage =data._marriage;
        dataBase.Add(db);
    }
    public void SaveDatabase()
    {
        string s ="";
        if(dataBase.Count==0)
        {
            return;
        }
        foreach (var item in dataBase)
        {
            s+= "|"+ item.id+","+(item._Alive==true?"1":"0")+","+(item._marriage==true?"1":"0")+","+item._like+","+item._infoLevel;
        }
        s = s.Remove(0,1);
        PlayerPrefs.SetString("characterData",s);
    }

    public CharacterData GetCharacter(int id)
    {
        CharacterData data =new CharacterData();
        foreach (var item in CharacterManager.instance.dataList)
        {
            if(item.id == id)
            {
                return item;
            }
        }
        return data;
    }
}
