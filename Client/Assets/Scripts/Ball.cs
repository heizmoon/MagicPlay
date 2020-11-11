using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class Ball : MonoBehaviour
{
    public BallColor ballColor;
    Collider2D circle;
    Rigidbody2D rig;
    // public Animation anim;
    public bool highLight;
    public GameObject lights;

    private void Awake() {
        rig =GetComponent<Rigidbody2D>();
        circle =GetComponent<CircleCollider2D>();
    }
    public void CostBall()
    {
        // anim.Play("cost");
        circle.enabled =false;
        rig.gravityScale =-20;
        // transform.DOScale(2,1.5f);
        StartCoroutine(WaitForDestory());
    }
    //丢弃宝珠
    public void DiscardBall()
    {
        circle.enabled =false;
        StartCoroutine(WaitForDestory());
        // anim.Play("discard");
    }
    IEnumerator WaitForDestory()
    {
        yield return new WaitForSeconds(1.5f);
        DestorySelf();
    }
    public void DestorySelf()
    {
        Destroy(this.gameObject);
    }
    public void ShowHightLight()
    {
        highLight =true;
        lights.SetActive(true);
    }
}

