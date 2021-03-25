using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Configs : MonoBehaviour
{
    public static Configs instance;
    public int initGold;
    public int battleLevelGold;
    public int priceRankGold;
    public int cardRank1;
    public int cardRank2;
    public int cardRank3;
    public int cardRank4;
    public float campRestoreHP;
    public int executeRestoreHP;
    public float eneryCubeRestoreMP;
    public int levelUpAddAttack;
    public int levelUpAddDefence;
    public int levelUpNeedSteps;

    void Awake()
    {
        instance =this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public int GetCardRank(int step)
    {
        int rank =0;
        if(step<cardRank1)
        {
            rank =0;
        }
        else if(cardRank1<=step&&step<cardRank2)
        {
            rank =1;
        }
        else if(cardRank2<=step&&step<cardRank3)
        {
            rank =2;
        }
        else if(cardRank3<=step&&step<cardRank4)
        {
            rank =3;
        }
        else
        {
            rank =4;
        }

        return rank;
    }
}
