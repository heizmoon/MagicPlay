using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MovingActor : MonoBehaviour
{
    public Transform[] transforms;
    Actor actor;
    public float timeInterval=4f;
    public float speed =160f;
    float currentTime;
    int currentPoint;
    Tweener tweener;
    public bool ifCanRun =true;
    void Start()
    {
        actor =GetComponentInChildren<Actor>();
        if(transforms.Length>1)
        {
            int r =Random.Range(0,transforms.Length);
            transform.localPosition = transforms[r].localPosition;
            currentPoint =r;
        }
    }

    // Update is called once per frame
    void Update()
    {
        currentTime+=Time.deltaTime;
        if(currentTime>=timeInterval)
        {
            currentTime =0;
            DoSomething();
        }
    }
    void DoSomething()
    {
        if(!ifCanRun)
        return;
        int r =Random.Range(0,10);
        if(r>4)
        {
            RunToNextPoint();
        }
    }
    void RunToNextPoint()
    {
        Transform nextPoint;
        if(currentPoint ==transforms.Length-1)
        {
            currentPoint-=1;
        }
        else if(currentPoint ==0)
        {
            currentPoint+=1;
        }
        else
        {
            int r =Random.Range(0,10);
            if(r>5)
            {
                currentPoint+=1;
            }
            else
            {
                currentPoint-=1;   
            }
        }
        nextPoint =transforms[currentPoint];
        if(nextPoint.localPosition.x<transform.localPosition.x)
        transform.localScale = new Vector3(-1,1,1);
        else
        transform.localScale = Vector3.one;
        float moveTime =Vector3.Distance(transform.localPosition,nextPoint.localPosition)/speed;
        tweener= transform.DOLocalMove(nextPoint.localPosition,moveTime,true);
        actor.animator.Play("run");
        StartCoroutine(PlayerIdle(moveTime));
    }
    IEnumerator PlayerIdle(float moveTime)
    {
        yield return new WaitForSeconds(moveTime);
        actor.animator.Play("idle");
    }
    public void Stop()
    {
        ifCanRun =false;
        if(tweener!=null)
        tweener.Pause();
        actor.animator.Play("idle");
    }

}
