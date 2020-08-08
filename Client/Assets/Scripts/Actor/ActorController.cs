using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    // Start is called before the first frame update
    public ExposedReference<GameObject> go ;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // public void SetAnim(string animName,GameObject obj)
    // {
    //     // animator.Play(animName);
    //     if(animName=="shock")
    //     {
    //         //添加震惊特效
    //     }
    // }
    // public void SetAnim(string animName)
    // {

    // }
    public void ActorShock(GameObject actor)
    {
        actor.GetComponentInChildren<Animator>().Play("shock");
    }
    public void ActorRun(GameObject actor)
    {
        actor.GetComponentInChildren<Animator>().Play("run");
    }
    public void ActorIdle(GameObject actor)
    {
        actor.GetComponentInChildren<Animator>().Play("idle");
    }
    public void ActorDead(ExposedReference<GameObject> go)
    {
        // actor.GetComponentInChildren<Animator>().Play("dead");
        
    }
    



    
}
