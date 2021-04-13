using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBird : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] steps;
    Animator _animator;
    Button button;
    float currentTime =0;
    float delayTime;
    public int type;
    int nowStep=0;
    bool CanContrl;
    void Start()
    {
        if(type ==0)
        {
            Init();
        }
        
    }
    void Init()
    {
        Time.timeScale=0;
        button = GetComponent<Button>();
        _animator = GetComponent<Animator>();
        _animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        delayTime =1f;
        currentTime = Time.realtimeSinceStartup;
        button.onClick.AddListener(GoOn);
    }
    // Update is called once per frame
    void Update()
    {
        if(type ==0 && CanContrl == false)
        {
            if(Time.realtimeSinceStartup>=delayTime+currentTime)
            {
                CanContrl =true;
            }
        }
        
    }
    
    void GoOn()
    {
        if(!CanContrl)
        {
            return;
        }
        if(steps.Length<=nowStep+1)
        {
            LeaveNewBird();
        }
        else
        {
            steps[nowStep].SetActive(false);
            nowStep++;
            steps[nowStep].SetActive(true);
            CanContrl=false;
        }
    }
    void LeaveNewBird()
    {
        Time.timeScale =1f;
        UIBuffDetail g = Main.instance.allScreenUI.GetComponentInChildren<UIBuffDetail>();
        NewBird b = UIBattle.Instance.enemyBarText.GetComponentInChildren<NewBird>();
        if(g!=null)
        {
            Destroy(g.gameObject);
        }
        if(b!=null&&Main.instance.ifNewBird ==13)
        {
            Destroy(b.gameObject);
        }
        Destroy(gameObject);
    }
    public static NewBird LoadNewBird(int i)
    {
        GameObject go = Instantiate((GameObject)Resources.Load("Prefabs/NewBird/NewBird_"+i));
        go.transform.SetParent(Main.instance.allScreenUI);
        go.transform.localPosition =Vector3.zero;
        go.transform.localScale =Vector3.one;
        go.GetComponent<RectTransform>().sizeDelta =Vector2.one;
        go.GetComponent<RectTransform>().anchoredPosition =Vector2.zero;
        return go.GetComponent<NewBird>();
    }

}
