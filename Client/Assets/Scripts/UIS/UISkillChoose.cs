using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkillChoose : MonoBehaviour
{
    public Button confirmBTN;
    public Transform content;
    public List<SkillBox> choosenBoxes =new List<SkillBox>();
    int nowChoose;
    public List<SkillBox> choosenSkills =new List<SkillBox>();
    ToggleGroup group;
    List<SkillBox> allSkills =new List<SkillBox>();
    void Start()
    {
        choosenSkills[0].GetComponent<Toggle>().onValueChanged.AddListener(isOn =>OnChangeSelect(isOn,0));
        choosenSkills[1].GetComponent<Toggle>().onValueChanged.AddListener(isOn =>OnChangeSelect(isOn,1));
        choosenSkills[2].GetComponent<Toggle>().onValueChanged.AddListener(isOn =>OnChangeSelect(isOn,2));
        choosenSkills[3].GetComponent<Toggle>().onValueChanged.AddListener(isOn =>OnChangeSelect(isOn,3));
        choosenSkills[0].GetComponent<Toggle>().isOn =true;
        group = content.GetComponent<ToggleGroup>();
        CreateSkillBoxList();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    void OnChangeSelect(bool isOn,int id)
    {
        if(!isOn)
        {
            return;
        }
        nowChoose = id;
    }
    //
    void CreateSkillBoxList()
    {
        //从Player中找到角色等级大于1的技能
        //创建SkillCubes，显示相应的技能
        //SkillCubes add toggle
        List<int> learnSkills = Player.instance.GetLearnSkills();
        foreach (var item in learnSkills)
        {
            //主动技能
            if(item>0&&bool.Parse(SkillManager.instance.GetInfo(item,"ifActive"))==true)
            CreateSkillBox(item);
        }
        TryGetOldSkills();
    }
    void CreateSkillBox(int id)
    {
        GameObject go =Instantiate((GameObject)Resources.Load("Prefabs/SkillBox"));
        SkillBox box = go.GetComponent<SkillBox>();
        box.Init(id);
        allSkills.Add(box);
        go.transform.SetParent(content);
        go.transform.localPosition =Vector3.zero;
        go.transform.localScale =Vector3.one;
        Toggle tg = go.GetComponent<Toggle>();
        tg.onValueChanged.AddListener(isOn => OnSelectSkillBox(isOn,box));
        tg.group = group;

    }
    void OnSelectSkillBox(bool isOn,SkillBox box)
    {
        if(!isOn)
        {
            return;
        }
        box.GetComponent<Toggle>().isOn =false;
        //如果选了自己，则清空选择
        if(choosenSkills[nowChoose].id == box.id)
        {
            choosenBoxes[nowChoose] =null;
            choosenSkills[nowChoose].Clear();
            ChangeActorUsingSkillList();
            box.UnMark();
            return;
        }
        //如果已经被其他选择了，那么无法再次选择
        else if(choosenBoxes.Contains(box))
        {    
            return;
        }
        if(choosenBoxes[nowChoose])
        {
            choosenBoxes[nowChoose].UnMark();
        }
        choosenBoxes[nowChoose] =box;
        box.Mark();
        choosenSkills[nowChoose].Init(box.id);
        ChangeActorUsingSkillList();
    }
    void ChangeActorUsingSkillList()
    {
        // Player.instance.playerActor.UsingSkillsID =new List<int>();
        for (int i = 0; i < 4; i++)
        {
            Player.instance.playerActor.UsingSkillsID[i] = choosenSkills[i].id;   
        }
    }
    void SelectSkillBox(int id)
    {
        for (int i = 0; i < allSkills.Count; i++)
        {
            if(allSkills[i].id ==id)
            allSkills[i].GetComponent<Toggle>().isOn = true;
        }
    }
    void TryGetOldSkills()
    {
        List<int> list =Player.instance.GetUsedBattleSkillList();
        for (int i = 0; i < list.Count; i++)
        {
            SelectSkillBox(list[i]);
            //choosenSkills[nowChoose] =
            nowChoose++;
        }
        nowChoose =0;
    }
    public void SaveOldSkills()
    {
        List<int> list =new List<int>();
        foreach (var item in Player.instance.playerActor.UsingSkillsID)
        {
            list.Add(item);
        }
        Player.instance.SaveUsedBattleSkillList(list);
    }
}
