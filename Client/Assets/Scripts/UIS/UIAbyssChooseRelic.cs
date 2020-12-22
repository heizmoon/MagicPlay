using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAbyssChooseRelic : MonoBehaviour
{
    public static UIAbyssChooseRelic instance;
    public Transform content;
    void Awake()
    {
        instance =this;
        gameObject.AddComponent<RelicManager>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // IEnumerator CreateRelic(int id,float delay)
    // {
    //     yield return new WaitForSeconds(delay);
    //     Debug.LogWarningFormat("随机到的id是{0}",id);
    //     RelicData data =RelicManager.instance.GetRelic(id);
    //     GameObject go = (GameObject)Instantiate(Resources.Load("Prefabs/Relic"));
    //     go.transform.SetParent(content);
    //     go.transform.localScale =Vector3.one;
    //     Relic relic = go.GetComponent<Relic>();
    //     relic.CreateRelic(data);
    //     Toggle toggle = go.GetComponent<Toggle>();
    //     toggle.onValueChanged.AddListener(IsOn=>OnChoose(IsOn,relic));
    // }
    IEnumerator CreateRelic(SkillData skillData,float delay)
    {
        yield return new WaitForSeconds(delay);
        Debug.LogWarningFormat("随机到的技能名是{0}",skillData.name);
        // RelicData data =RelicManager.instance.GetRelic(id);
        GameObject go = (GameObject)Instantiate(Resources.Load("Prefabs/Relic"));
        go.transform.SetParent(content);
        go.transform.localScale =Vector3.one;
        Relic relic = go.GetComponent<Relic>();
        relic.CreateRelic(skillData);
        Toggle toggle = go.GetComponent<Toggle>();
        toggle.onValueChanged.AddListener(IsOn=>OnChoose(IsOn,relic));
    }
    
    //创建UI
    // public void CreateUIs(string distribution)
    // {
    //     Debug.LogWarningFormat("distribution ={0}",distribution);

    //     for (int i = 0; i < 3; i++)
    //     {
    //         int groupID =ChooseGruop(distribution);
    //         Debug.LogWarningFormat("groupID={0}",groupID);
    //         List<int> relicList =RelicManager.instance.GetRelicGroup(groupID);
    //         int id =ChooseOne(relicList);
    //         StartCoroutine(CreateRelic(id,i*0.2f));
    //     }
        
    // }
    public void CreateUIs(int number)
    {
        SkillData[] skillDatas = SkillManager.instance.GetRandomSkills(number);
        for (int i = 0; i < 3; i++)
        {
            StartCoroutine(CreateRelic(skillDatas[i],i*0.2f));
        }
    }
    int ChooseGruop(string str)
    {
        int groupID =0;
        string[] ss = str.Split('|');
        List<int> GroupsID =new List<int>();
        List<int> GroupsWeight =new List<int>();
        int totalWeight =0;
        foreach (var item in ss)
        {
            GroupsID.Add(int.Parse(item.Split(',')[0]));
            GroupsWeight.Add(int.Parse(item.Split(',')[1])+totalWeight);
            totalWeight += int.Parse(item.Split(',')[1]);
        }
        int r =Random.Range(0,totalWeight+1);
        if(r<=GroupsWeight[0])
        {
            groupID =GroupsID[0];
        }
        for(int i =1;i<GroupsWeight.Count;i++)
        {
            if (r>GroupsWeight[i-1]&&r<=GroupsWeight[i])
            {
               groupID =GroupsID[i];
               break; 
            }
        }
        return groupID;
    }
    //随机出一个ID
    int ChooseOne(List<int> list)
    {
        int itemId =0;
        int total =list.Count-1;
        itemId =Random.Range(0,total+1);
        Debug.LogWarningFormat("总数为{0},随机到第{1}个",total,itemId);

        itemId =list[itemId];
        return itemId;
    }
    public void OnChoose(bool IsOn,Relic relic)
    {
        if(!IsOn)
        {
            return;
        }
        // Debug.LogWarningFormat("选择:{0}",relic.data.id);
        //选择了一个Relic,为玩家添加这个Relic的buff
        
        // if(relic.data.id ==1)
        // {
        //     //深渊泉水直接回血
        //     int hp =Mathf.FloorToInt(Player.instance.playerActor.HpMax*0.5f);
        //     // Debug.LogWarningFormat("深渊泉水恢复{0}",hp);
        //     Player.instance.playerActor.AddHp(hp);
        //     CloseUI();
        //     return;
        // }
        // if(relic.data.buff=="")
        // {
        //     CloseUI();
        //     return;
        // }
        // string[] s = relic.data.buff.Split(',');
        // foreach (var item in s)
        // {
        //     // BuffManager.instance.CreateBuffForActor(int.Parse(item),Player.instance.playerActor);
        // }
        Player.instance.playerActor.AddSkillCard(relic.data);
        CloseUI();
    }
    void CloseUI()
    {
        Debug.LogWarning("关闭");
        // Abyss.instance.EndChooseRelic();
        UIBattle.Instance.OnBattleGoOn();
        Destroy(gameObject);
    }
}
