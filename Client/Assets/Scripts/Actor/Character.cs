using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public CharacterData data;
    public string reform;
    public int HPMax;
    public int MPMax;
    public int attack;
    public float reMP;
    public float crit;
    public List<int> allSkillsList;
    int[] reforms = new int[4];
    List<int> reformIds =new List<int>();
    List<int> reformResults =new List<int>();
    public List<int> skills =new List<int>();
    public string saveCode;
    void Start()
    {
        
    }
    void loadReforms()
    {
        string tar =PlayerPrefs.GetString("CharacterReformData");
        if(tar =="")
        {
            return;
        }
        //解析结果： id:1,1;1,0;3,2;|id……
        string[] strs = tar.Split('|');
        string[] str =null;
        foreach (var item in strs)
        {
            if(int.Parse(item.Split(':')[0]) ==data.id)
            {
                str = item.Split(':')[1].Split(';');
            }
        }
        if(str == null)
        {
            Debug.Log("该角色没有进行过改造");
            return;
        }
        foreach (var item in str)
        {
            int id =int.Parse(item.Split(',')[0]);
            int result = int.Parse(item.Split(',')[1]);
            reformIds.Add(id);
            reformResults.Add(result);
            string s = ReformManager.instance.GetInfo(id,result);
            reforms[0]+= int.Parse(s.Split(',')[0]);
            reforms[1]+= int.Parse(s.Split(',')[1]);
            reforms[2]+= int.Parse(s.Split(',')[2]);
            reforms[3]+= int.Parse(s.Split(',')[3]);

        }
        
    }
    public void InitData()
    {
        HPMax =data.hp;
        MPMax =data.mp;
        attack = data.attack;
        reMP =data.reMp;
        crit =data.crit;
        allSkillsList = data.allSkillsList;
        Debug.LogWarning("基础攻击力是"+attack);
        loadReforms();
        AddReformPerporty();
        GetSkills();
        
    }
    void GetSkills()
    {
        string[] str = data.skills.Split(',');
        foreach (var item in str)
        {
            skills.Add(int.Parse(item));
        }
    }
    public void AddReform(ReformData reformData)
    {
        //首先判断结果
        int result = ReformManager.instance.GetResult(reformData,reformIds.Count);
        string str= ReformManager.instance.GetInfo(reformData.id,result);
        reforms[0]+= int.Parse(str.Split(',')[0]);
        reforms[1]+= int.Parse(str.Split(',')[1]);
        reforms[2]+= int.Parse(str.Split(',')[2]);
        reforms[3]+= int.Parse(str.Split(',')[3]);
        AddReformPerporty();
        SaveReformResult(reformData.id,result);
        //显示结果UI++++++++++++++++++++++++++++++++++++++++++++++++++待进行
    }

    void AddReformPerporty()
    {
        HPMax+=reforms[0];
        MPMax+=reforms[1];
        attack+=reforms[2];
        reMP+=reforms[3];
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    void SaveReformResult(int id,int result)
    {
        saveCode = "";
        reformIds.Add(id);
        reformResults.Add(result);
        for (int i = 0; i < reformIds.Count; i++)
        {
            saveCode+=reformIds[i];
            saveCode+=",";
            saveCode+=reformResults[i];
            saveCode+=";";
        }
        saveCode = saveCode.Remove(saveCode.Length-1);
        saveCode = string.Format("{0}:{1}",data.id,saveCode);
    }
}
