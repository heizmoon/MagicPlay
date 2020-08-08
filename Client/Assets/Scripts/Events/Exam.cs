using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Exam : MonoBehaviour
{
    public Text examName;
    public GameObject examInfo;
    public Button examInfoBTN;
    public HPBar examResultBar;
    public Text textQuestionNum;
    public Text textQuestion;
    public Text textExamInfo;
    public Button[] BTN_answer;
    public Transform answersPosition;
    public GameObject imageQuestionFrame;
    public Image questionImage;
    public Button closeInfo;
    int questionNum=1;
    int maxQuestion =5;
    int correctAnswer=0;
    List<int> oldQuestionList =new List<int>();
    public Animation paperAnim;
    int goal;
    public EventTanser eventTanser;
    public ExamData examData;
    // public GameObject successUI;
    // public Text resultText;
    // public GameObject rewardsUI;
    // public GameObject resultUI;
    public GameObject failUI;
    public Slider Aslider;
    // public Slider Bslider;
    void Start()
    {
        gameObject.AddComponent<QuestionManager>();
        // eventTanser=gameObject.GetComponent<EventTanser>();
        
        this.transform.SetParent(Main.instance.allScreenUI);
        GetComponent<RectTransform>().sizeDelta = Vector2.zero;
		GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
        transform.localScale =Vector3.one;
        InitExam();
    }

    // Update is called once per frame
    public void InitExam()
    {
        
        StartCoroutine(WaitForPaperExam());

    }
    IEnumerator WaitForPaperExam()
    {
        yield return new WaitForSeconds(0.2f);
        examName.text =examData.examName;
        textExamInfo.text =examData.examInfo.text;
        textExamInfo.transform.localPosition =examData.transform.localPosition;
        maxQuestion =examData.questionNum;
        examResultBar.initHpBar(0,examData.totalGoal);
        Aslider.value =(examData.Apoint+0f)/examData.totalGoal;
        // Bslider.value =(examData.Bpoint+0f)/examData.totalGoal;
        yield return new WaitForSeconds(3f);
        OnPaperExam();

    }
    public void OnCloseInfo()
    {
        examInfo.SetActive(false);
    }
    public void OnOpenInfo()
    {
        examInfo.SetActive(true);
    }
    void OnPaperExam()
    {
        if(questionNum<=maxQuestion)
        {
            CreateQuestion();
            questionNum++;
        }
        else
        {
            //答题结束
            GetResult();

            Debug.Log("答题结束");
        }
    }
    void CreateQuestion()
    {
        //从对应编号的题库中随机一道题，
        //不能随机已经出现过的题，
        //将随到的题加入【已随过】列表
        
        QuestionDatabase database = QuestionManager.instance.GetBank(examData.paperQuestionDatabase);
        int id =0;
        do
        {
            id =Random.Range(database.minID,database.minID+database.num);
            Debug.Log(id);
        } while (oldQuestionList.Contains(id));
        oldQuestionList.Add(id);
        QuestionData table = QuestionManager.instance.GetInfo(id);

        //获取题号，题目，答案，正确答案
        //questionNum =1;question =“图中显示的怪兽属于哪个类别？”；questionImage="Actor_girl_01";answers="天使|恶魔|野兽|人型";correctAnswer="3";

        correctAnswer=table.correctAnswer;
        textQuestion.text =table.question;
        textQuestionNum.text =string.Format("第{0}题",questionNum);

        BTN_answer[0].GetComponentInChildren<Text>().text = table.answer0;
        BTN_answer[1].GetComponentInChildren<Text>().text = table.answer1;
        BTN_answer[2].GetComponentInChildren<Text>().text = table.answer2;
        BTN_answer[3].GetComponentInChildren<Text>().text = table.answer3;
        foreach (var item in BTN_answer)
        {
            item.interactable =true;
            item.image.color = new Color(1,1,1);
        }


        if(table.sprite!="")
        {
            answersPosition.GetComponent<RectTransform>().sizeDelta = new Vector2(-240, 400);
		    answersPosition.GetComponent<RectTransform>().anchoredPosition3D = new Vector3(0,-150,0);
            imageQuestionFrame.SetActive(true);
            questionImage.sprite = Resources.Load("Texture/"+table.sprite,typeof(Sprite)) as Sprite;
            questionImage.SetNativeSize();
        }
        else
        {
            answersPosition.GetComponent<RectTransform>().sizeDelta = new Vector2(-240, 400);
            answersPosition.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
            imageQuestionFrame.SetActive(false);
        }
        ShowAQuestion();
    }
    void ShowAQuestion()
    {
        paperAnim.clip = paperAnim.GetClip("paperExamShow");
        paperAnim.Play();
    }
    public void OnChooseAnswer(int num)
    {
        if(num ==correctAnswer)
        {
            // Debug.Log("回答正确");
            BTN_answer[num].image.color =new Color(0.31f,1,0.63f);
            goal+=examData.questionGoal;
            examResultBar.changeHPBar(goal,true);
        }
        else
        {
            BTN_answer[num].image.color =new Color(1,0.47f,0.63f);
            BTN_answer[correctAnswer].image.color =new Color(0.31f,1,0.63f);
        }
        foreach (var item in BTN_answer)
        {
            item.interactable =false;
            // Debug.Log("不能选了");
        }
        HideAQuestion();
    }
    ///<summary>回答完一次问题后，界面重新来过</summary>
    void HideAQuestion()
    {
        paperAnim.clip = paperAnim.GetClip("paperExamHide");
        StartCoroutine(WaitForAnimStop());
        
        
    }
    IEnumerator WaitForAnimStop()
    {
        yield return new WaitForSeconds(1f);
        paperAnim.Play();
        yield return new WaitForSeconds(1.5f);
        OnPaperExam();
    }
    void GetResult()
    {
        // resultUI.SetActive(true);

        // Text _text;
        // if(goal<examData.Bpoint)
        // {
        //     // failUI.SetActive(true);
        //     // resultText.text ="不及格";
        //     // _text = failUI.GetComponent<Text>();
        //     eventTanser.data.result =2;
        // }
        if(goal<examData.Apoint)
        {
            // successUI.SetActive(true);
            // resultText.text ="及格";
            // rewardsUI.SetActive(true);
            // _text = successUI.GetComponent<Text>();
            eventTanser.data.result =2;
        }
        else
        {
            // successUI.SetActive(true);
            // resultText.text ="优秀";
            // rewardsUI.SetActive(true);
            // _text = successUI.GetComponent<Text>();
            eventTanser.data.result =1;
        }
        // _text.text =string.Format(_text.text,eventTanser.data.timeCost,eventTanser.data.eventName);
        EndPaperExam();
    }
    public void EndPaperExam()
    {
        // eventTanser.orginTask.GetResult(eventTanser.data);
        examData.GetResult(eventTanser.data.result);
        StartCoroutine(DestroyExam());
    }
    IEnumerator DestroyExam()
    {
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }
}
