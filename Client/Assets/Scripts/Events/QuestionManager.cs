using UnityEngine;
using System.Collections;
using System.Data;
using System.IO;
using Excel;
using System.Collections.Generic;
using Data;

public class QuestionManager : MonoBehaviour

{
    public static QuestionManager instance;
    //(2)根据需要，定义自己的结构        
    
    
    public QuestionData nullQuestionData = new QuestionData();
    public QuestionDatabase nullQuestionDatabase = new QuestionDatabase();
    QuestionDatabaseSet databaseSet;
    QuestionDataSet dataSet;
    void Awake()
    {   
        instance =this;
        dataSet =Resources.Load<QuestionDataSet>("DataAssets/Question");
        databaseSet =Resources.Load<QuestionDatabaseSet>("DataAssets/QuestionDatabase");
            
    }


    //找到并读取Excel，转换成DataSet型变量
    
    public string GetInfo(int id ,string content)
    {
        foreach(var item in dataSet.dataArray)
        {
            if(item.id==id)
            {
                switch(content)
                {
                    case "bank":
                    
                    return item.bank.ToString();

                }     
            }    
        }
        return "";
    }
    public QuestionData GetInfo(int id)
    {
        
       foreach(var item in dataSet.dataArray)
        {
            if(item.id==id)
            {
            return item;       
            } 
        }
        return nullQuestionData;
    }
    public QuestionDatabase GetBank(int id)
    {
        foreach(var item in databaseSet.dataArray)
        {
            if(item.bankID==id)
            {
                return item;       
            } 
        }
        return nullQuestionDatabase;
    }     
}
