using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Data;

public class CharacterManager : MonoBehaviour
{
    public static CharacterManager instance;
    ///<summary>已解锁的角色列表</summary>
    List<int> unlockCharacters =new List<int>();

    public List<Character> characters =new List<Character>();
    CharacterDataSet manager;
    LevelDataSet levelDataSet;
    void Awake()
    {
        instance =this;
        manager = Resources.Load<CharacterDataSet>("DataAssets/Character");
        levelDataSet = Resources.Load<LevelDataSet>("DataAssets/Level");
        unlockCharacters.Add(0);
        unlockCharacters.Add(1);
        unlockCharacters.Add(2);
        unlockCharacters.Add(3);
        
    }
    public void GetUnlockCharacter()
    {
        if(PlayerPrefs.GetString("UnlockCharacter")=="")
        {
            for(int i =0;i<manager.dataArray.Length;i++)
            {
                if(manager.dataArray[i].id<30)
                {
                    // datas.Add(datas[i]);
                    unlockCharacters.Add(manager.dataArray[i].id);
                }
            }
            return;
        }
        //从保存的玩家资产占有数据中获取玩家都拥有那些资产（uid）
        foreach (var item in PlayerPrefs.GetString("UnlockCharacter").Split(','))
        {
            unlockCharacters.Add(int.Parse(item));
        }
        
        
    }
    ///<summary>保存角色</summary>
    public void SaveUnlockCharacter()
    {
        string s ="";
        foreach (var item in unlockCharacters)
        {
            s+= ",";
            s+= item;
        }
        s = s.Remove(0,1);
        PlayerPrefs.SetString("UnlockCharacter",s);
        Debug.Log(s);    
    }
    public void SaveCharacterReformData()
    {
        string s ="";
        foreach (var item in characters)
        {
            s+="|";
            s+=item.saveCode;
        }
        s =s.Remove(0);
        PlayerPrefs.SetString("CharacterReformData",s);

    }
    void GetCharacterSaveCode(int id)
    {

    }
    ///<summary>解锁新角色</summary>
    public void UnlockCharacter(int id)
    {
        if(id ==100)
        {
            Debug.LogWarning("已经没有角色可供解锁");
        }
        else
        {
            unlockCharacters.Add(id);
            //UI处理++++++++++++++++++++++++++++++
            //
            //
            //
        }
    }
    public int UnlockCharacter()
    {
        List<int> allCharacters = new List<int>();
        foreach (var item in manager.dataArray)
        {
            allCharacters.Add(item.id);
        }
        List<int> lockCharaters = allCharacters.Except(unlockCharacters).ToList();
        if(lockCharaters==null)
        {
            return 100;
        }
        int r = Random.Range(0,lockCharaters.Count);

        return lockCharaters[r];
    }

    public string GetInfo(int id ,string content)
    {
        foreach(var item in manager.dataArray)
        {
            if(item.id==id)
            {
                switch(content)
                {
                    case "name":
                    // Debug.LogFormat("内容:{0}",item.name);
                    return item.name;
                    case "describe":
                    // Debug.LogFormat("内容:{0}",item.describe);
                    return item.describe;
                    case "prefab":
                    return item.prefab;
                    
                }     
            }    
        }
        return "";
    }
    public CharacterData GetInfo(int id)
    {
       foreach(var item in manager.dataArray)
        {
            if(item.id==id)
            {
                return item;       
            } 
        }
        return null;
    }
    public Character GetCharacter(int id)
    {
        foreach (var item in characters)
        {
            if(item.data.id == id)
            {
                return item;
            }
        }
        return null;
    }
    public LevelData GetLevelData(int level)
    {
        foreach (var item in levelDataSet.dataArray)
        {
            if(item.level == level)
            {
                return item;
            }
        }
        return null;
    }
    ///<summary>随机获取N个角色</summary>
    public CharacterData[] RandomCharacters(int N)
    {
        if(N<1)
        {
            return null;
        }
        CharacterData[] datas =new CharacterData[N];
        int temp =100;
        for(int i =0;i<N;i++)
        {
            int code = Random.Range(0,unlockCharacters.Count-i);
            if(code == temp)
            {
                i--;
            }
            else
            {
                datas[i] = GetInfo(code);
            }
        }
        return datas;
    }
    //创建所有角色
    public void CreateCharacters()
    {
        Debug.Log("CreateCharacters");
        foreach (var item in unlockCharacters)
        {
            Character character =new Character();
            character.data =GetInfo(item);
            characters.Add(character);
            character.InitData();
        }
    }
}
