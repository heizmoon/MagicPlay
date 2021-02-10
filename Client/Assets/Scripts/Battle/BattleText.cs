using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleText : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject _text;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetText(int num,bool crit)
    {
        GameObject bt =GameObject.Instantiate(_text.gameObject);
        Text t = bt.GetComponentInChildren<Text>();
        bt.transform.SetParent(_text.transform.parent);
        float x = Random.Range(-15f,15f);
        float y = Random.Range(-15f,15f);
        bt.transform.localScale =Vector3.one;
        bt.transform.localPosition =new Vector3(x,y,0);
        
        if(num>0)
        {
            t.text = "-"+num.ToString();
            if(crit)
            {
                t.color =new Color(1,0.29f,0.2f);
                t.fontSize =42;
            }
        }
        else if(num == 0)
        {
            t.text ="";
        }
        else
        {
            t.color =Color.green;
            t.text = "+"+Mathf.Abs(num).ToString();
            if(crit)
            {
                //t.color =new Color(1,0.29f,0.2f);
                t.fontSize =42;

            }
        }
        bt.gameObject.SetActive(false);
        StartCoroutine(DestorySelf(bt));
    }
    public void SetText(string s)
    {
        GameObject bt =GameObject.Instantiate(_text.gameObject);
        Text t = bt.GetComponentInChildren<Text>();
        bt.transform.SetParent(_text.transform.parent);
        float x = Random.Range(-15f,15f);
        float y = Random.Range(-15f,15f);
        bt.transform.localScale =Vector3.one;
        bt.transform.localPosition =new Vector3(x,y,0);
        t.text = s;
        t.color =Color.white;
        StartCoroutine(DestorySelf(bt));
    }
    
    IEnumerator DestorySelf(GameObject bt)
    {
        Animation anim =bt.GetComponentInChildren<Animation>();
        float r1 =Random.Range(0.7f,1.4f);
        foreach(AnimationState s in anim)
        {
            s.speed=r1;
        }
        float r2 =Random.Range(0f,0.1f);
        yield return new WaitForSeconds(r2);
        anim.Play();
        bt.SetActive(true);
        yield return new  WaitForSeconds(1f);
        Destroy(bt);
    }
}
