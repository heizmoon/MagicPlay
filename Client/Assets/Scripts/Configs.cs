using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Configs : MonoBehaviour
{
    public static Configs instance;
    public int initGold;
    public int battleLevelGold;
    public int priceRankGold;
    void Awake()
    {
        instance =this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
