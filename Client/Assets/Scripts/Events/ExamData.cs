using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ExamData : TypeEventTool
{
    public string examName;
    public int Apoint;
    // public int Bpoint;
    public int totalGoal;
    
    ///<summary>考试类型，0=</summary>
    // public int examType;
    ///<summary>考试说明信息文字</summary>
    public Text examInfo;
    ///<summary>数值验证需要验证的数值列表</summary>
    public List<int> valueIdentificationList;
    ///<summary>数值验证，列表对应的数值要求</summary>
    public List<int> valueRequirementList;

    ///<summary>数值验证项目得分</summary>
    public List<int> valueGoalList;

    ///<summary>问答的题库id</summary>
    public int paperQuestionDatabase;
    ///<summary>答对一道题加几分</summary>
    public int questionGoal;

    ///<summary>一共几道题</summary>
    public int questionNum;

    ///<summary>实测类型，0=限时，1=胜负，2=炼金，3=烹饪</summary>
    // ///<summary>限时时间</summary>
    // public int limitTime;
    // ///<summary>对手的monsterID</summary>
    // public int enemyID;

    void Start()
    {
        examInfo =GetComponentInChildren<Text>();

        Exam exam = Instantiate((GameObject)Resources.Load("Prefabs/Exam")).GetComponent<Exam>();
        exam.eventTanser =GetComponent<EventTanser>();
        exam.examData =this;
        
    }
  

}
