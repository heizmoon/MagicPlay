using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBird : MonoBehaviour
{
    // Start is called before the first frame update
    Animator _animator;
    Button button;
    float currentTime =0;
    float delayTime;
    public int type;
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
    }
    // Update is called once per frame
    void Update()
    {
        if(type ==0)
        {
            if(Time.realtimeSinceStartup>=delayTime+currentTime)
            {
                CanContrl(0);
            }
        }
        
    }
    void CanContrl(float delayTime)
    {
        Debug.Log("GOGOGOGO");
        delayTime =999999;
        button.onClick.AddListener(LeaveNewBird);
    }
    void LeaveNewBird()
    {
        Time.timeScale =1f;
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
