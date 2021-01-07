using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIBattleCharacter : MonoBehaviour
{
    public static UIBattleCharacter instance;
    Text charName;
    Transform charPoint;
    Text charHP;
    Text charMP;
    Text charReMp;
    Text charAtt;
    Text charDef;
    Text charDeal;
    Actor playerActor;
    private void Awake() 
    {
        instance = this;
        charName =transform.Find("CharacterName").gameObject.GetComponent<Text>();
        charPoint =transform.Find("CharacterPoint");
        charHP =transform.Find("CharacterHP").gameObject.GetComponent<Text>();
        charMP =transform.Find("CharacterMP").gameObject.GetComponent<Text>();
        charReMp =transform.Find("CharacterMPRE").gameObject.GetComponent<Text>();
        charAtt =transform.Find("CharacterAttack").gameObject.GetComponent<Text>();
        charDef =transform.Find("CharacterDefence").gameObject.GetComponent<Text>();
        charDeal =transform.Find("CharacterDealNumber").gameObject.GetComponent<Text>();
        playerActor=Player.instance.playerActor;
    }

    void Start()
    {
        
    }
    public void Init()
    {
        playerActor.transform.SetParent(charPoint);
        playerActor.transform.localScale =Vector3.one;
        playerActor.transform.localPosition=Vector3.zero;
        charName.text = playerActor.character.data.name;
        charHP.text =string.Format("生命值：{0}/{1}",playerActor.HpCurrent,playerActor.HpMax);
        charHP.text =string.Format("能量值：{0}",playerActor.MpMax); 
        charHP.text =string.Format("每秒能量回复：{0}",playerActor.autoReduceMPAmount*5);
        charAtt.text =string.Format("攻击力：{0}",playerActor.basicAttack); 
        // charHP.text =string.Format("防御力：{0}",playerActor.); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
