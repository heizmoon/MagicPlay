using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public CharacterData data;
    void Start()
    {
        
    }


    public static string GetCharacterName(int id)
    {
        if(id ==0)
        {
            return Player.instance.playerName;
        }
        return "石头人";
    }
}
