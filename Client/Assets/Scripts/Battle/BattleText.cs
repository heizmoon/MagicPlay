using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleText : MonoBehaviour
{
    // Start is called before the first frame update
    Animation anim;
    Text text;
    GameObject critPic;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetText(int num,Transform ts,bool ifCrit,bool isDamage)
    {
        anim =GetComponent<Animation>();
        text =GetComponentInChildren<Text>();
        if(isDamage)
        text.text =string.Format("-{0}",num);
        else
        text.text =string.Format("+{0}",-num);
        transform.SetParent(ts);
        float x = Random.Range(-10f,10f);
        float y = Random.Range(-20f,20f);
        transform.localPosition = new Vector3(x,y,0);
        if(ifCrit)
        {
            transform.localScale = new Vector3(1.5f,1.5f,1);
            text.color = Color.red;
        }
        else
        {
            transform.localScale =Vector3.one;
            if(isDamage)
            text.color = Color.white;
            else
            text.color =Color.green;
        }
        int r = Random.Range(0,4);
        anim.Play("text"+r);
        StartCoroutine(DestorySelf());

    }
    public void SetText(string str,Transform ts)
    {
        anim =GetComponent<Animation>();
        text =GetComponentInChildren<Text>();
       
        text.text =str;
        transform.SetParent(ts);
        float x = Random.Range(-10f,10f);
        float y = Random.Range(-20f,20f);
        transform.localPosition = new Vector3(x,y,0);
        transform.localScale = Vector3.one;
        text.color = Color.white;
        int r = Random.Range(0,4);
        anim.Play("text"+r);
        StartCoroutine(DestorySelf());

    }
    IEnumerator DestorySelf()
    {
        yield return new WaitForSeconds(0.9f);
        Destroy(gameObject);
    }
    public static BattleText CreateBattleText()
    {
        BattleText bt = Instantiate((GameObject)Resources.Load("Prefabs/battleText")).GetComponent<BattleText>();
        return bt;
    }
}
