using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleText : MonoBehaviour
{
    // Start is called before the first frame update
    public Text _text;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetText(int num,bool crit)
    {
        
        Text t = GameObject.Instantiate(_text.gameObject).GetComponent<Text>();
        t.transform.SetParent(_text.transform.parent);
        // float x = Random.Range(-5f,5f);
        // float y = Random.Range(-5f,5f);
        t.transform.localScale =Vector3.one;
        t.transform.localPosition =Vector3.zero;
        Animation anim =t.gameObject.GetComponent<Animation>();
        if(num>0)
        {
            t.text = "-"+num.ToString();
            if(crit)
            {
                t.color =new Color(1,0.29f,0.2f);
                t.fontSize =42;
                // foreach(AnimationState s in anim)
                // {
                //     s.speed=0.5f;
                // }
            }
        }
        else
        {
            t.color =Color.green;
            t.text = "+"+Mathf.Abs(num).ToString();
            if(crit)
            {
                //t.color =new Color(1,0.29f,0.2f);
                t.fontSize =42;
                // foreach(AnimationState s in anim)
                // {
                //     s.speed=0.5f;
                // }
            }
        }
        
        
          
        anim.Play();
        StartCoroutine(DestorySelf(t));
    }
    public void SetText(string s)
    {
        Text t = GameObject.Instantiate(_text.gameObject).GetComponent<Text>();
        t.transform.SetParent(_text.transform.parent);
        t.transform.localScale =Vector3.one;
        t.transform.localPosition =Vector3.zero;
        t.text = s;
        t.color =Color.white;
        Animation anim =t.gameObject.GetComponent<Animation>();
        anim.Play();
        StartCoroutine(DestorySelf(t));
    }
    IEnumerator DestorySelf(Text t)
    {
        yield return new  WaitForSeconds(1f);
        Destroy(t.gameObject);
    }
}
