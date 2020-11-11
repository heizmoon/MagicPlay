using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BallColor
{
    Red =0,
    Green =1,
    Yellow =2,
    Blue =3
}
public class BallManager:MonoBehaviour
{
    public static BallManager instance;
    public List<Ball> balls =new List<Ball>();
    public Vector3 offset;
    public int maxBalls;
    private void Awake() 
    {
        if(instance==null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void CreateNewBall(BallColor color)
    {
        //判断1：是否已经达到最大值
        if(balls.Count>0&&balls.Count>=maxBalls)
        {
            balls[0].DiscardBall();
            balls.RemoveAt(0);
        }
        GameObject go =(GameObject)Instantiate(Resources.Load("Prefabs/Balls/"+color));
        go.transform.parent = this.transform;
        if(balls.Count>0)
        go.transform.localPosition = balls[balls.Count-1].transform.localPosition+offset;
        else
        go.transform.localPosition = new Vector2(-305,-126);
        go.transform.localScale =Vector3.one;
        balls.Add(go.GetComponent<Ball>());
        JudgeUnity();
    }
    public void CreateNewBall(int number,BallColor color)
    {
        if(number<1)
        {
            return;
        }
        for(int i =0;i<number;i++)
        {
            StartCoroutine(WaitForCreate(color));
        }
    }
    IEnumerator WaitForCreate(BallColor color)
    {
        yield return new WaitForSeconds(0.1f);
        CreateNewBall(color);
    }
    public int CostBalls()
    {
        if(balls.Count<1)
        {
            return 0;
        }
        if(!balls[0].highLight)
        {
            return 0;
        }
        int number =1;
        while(number<balls.Count&& balls[number].highLight)
        {
            number++;
        }
        if(number>2)
        {
            for (int i = 0; i < number; i++)
            {
                balls[0].CostBall();
                balls.RemoveAt(0);
            }

        }
        
        return number;
    }
    void JudgeUnity()
    {
        if(balls.Count<=2)
        {
            return;
        }
        int startIndex =0;
        int number =1;
        while(balls.Count-1-startIndex>0 &&  balls[balls.Count-1-startIndex].ballColor == balls[balls.Count-2-startIndex].ballColor)
        {
            startIndex++;
            number++;
            Debug.LogFormat("startIndex:{0},number:{1}",balls.Count-startIndex-1,number);
        }
        if(number>2)
        {
            ChangeHighLight(number);
        }
    }
    
    void ChangeHighLight(int number)
    {
        for (int i = balls.Count-1; i > balls.Count-number-1; i--)
        {
            balls[i].ShowHightLight();
        }
    }

    ///<summary>检查特定颜色宝珠的数量是否大于指定的数字</summary>
    public bool CheckBallNumber(BallColor color,int number)
    {
        int amount =0;
        foreach (var item in balls)
        {
            if(item.ballColor == color)
            {
                amount ++;
            }
        }
        if(amount>= number)
        return true;
        else
        return false;
    }

    ///<summary>检查宝珠的总数是否大于指定的数字</summary>
    public bool CheckBallNumber(int number)
    {
        if(balls.Count>=number)
        return true;
        else
        return false;
    }
    
    ///<summary>消耗特定颜色的宝珠，返回消耗了多少数量</summary>
    public int CostBalls(BallColor color)
    {
        int amount =0;
        for (int i = 0; i < balls.Count; i++)
        {
            if(balls[i].ballColor == color)
            {
                balls[i].CostBall();
                balls.RemoveAt(i);
                i--;
                amount++;
            }
        }
        return amount;
    }
    void Update()
    {
        if(Input.GetKeyDown("a"))
        {
            CreateNewBall(1,BallColor.Red);
            CostBalls();
        }
        if(Input.GetKeyDown("s"))
        {
            CreateNewBall(1,BallColor.Green);
            CostBalls();
        }if(Input.GetKeyDown("d"))
        {
            CreateNewBall(1,BallColor.Blue);
            CostBalls();
        }if(Input.GetKeyDown("f"))
        {
            CreateNewBall(1,BallColor.Yellow);
            CostBalls();
        }
    }

}