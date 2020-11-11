using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISkillButton : MonoBehaviour
{
    public Text skillName;
    public Text CDNumber;

    public Image icon;
    public Image mask;
    public Skill skill;
    public Image MPBar;
    float CD;
    float currentTime;
    bool intoCD;
    float mpState;
    Button button;
    float changeTextInterval =0.1f;
    float currentChangeText;
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if(intoCD)
        {
            currentTime+=Time.deltaTime;
            currentChangeText+=Time.deltaTime;
            if(currentChangeText>=changeTextInterval)
            {
                ChangeCDText(CD-currentTime);
            }
            if(currentTime>= CD)
            {
                EndCD();
            }
        }
    }
    public void CommonCD()
    {
        if(intoCD&&CD- currentTime>=Player.instance.playerActor.commonCD)
        {
            return;
        }
        else
        {
            BeginCD(Player.instance.playerActor.commonCD);
        }    
    }
    public void InitButton(int id)
    {
        button =GetComponent<Button>();
        skill =Player.instance.playerActor.GetSkills(id);
        intoCD =false;
        skillName.text =skill.skillName;
        icon.sprite =Resources.Load("Texture/Skills/"+skill.icon,typeof(Sprite)) as Sprite;
        // button.onClick.AddListener(OnButtonClikc);
        InitMpBar();
    }
    public void OnButtonClikc()
    {
        if(UIBattle.Instance.isBattleOver)
        {
            return;
        }
        if(!Player.instance.playerActor.WanaSpell(skill)) 
        {
            return;
        }
        BeginCD(skill.CD);
        // UIBattle.Instance.CommonCD();
    }
    void InitMpBar()
    {
        mpState =(skill.realManaCost+0f)/(Player.instance.playerActor.MpMax+0f);
        // Debug.LogWarningFormat("realManaCost ={0},MPMax ={1},mpState ={2}",skill.realManaCost,Player.instance.playerActor.MpMax,mpState);
        MPBar.fillAmount = mpState>1?1:mpState;
        ChangeMpBar();
    }
    public void ChangeMpBar()
    {
        if(button ==null)
        {
            return;
        }
        if(Player.instance.playerActor.MpCurrent>=skill.realManaCost)
        {
            MPBar.color =Color.cyan;
            // Debug.LogWarningFormat("技能{0}→蓝够",skill.skillName);
        }
        else
        {
            MPBar.color =Color.red;
            // Debug.LogWarningFormat("技能{0}→蓝不不不够",skill.skillName);

        }
    }
    void BeginCD(float cd)
    {
        if(button ==null)
        {
            return;
        }
        if(cd ==0)
        {
            return;
        }
        //使用的技能进入CD
        currentTime =0;
        intoCD =true;
        ContrlButton(false);
        CD =cd;
        ChangeCDText(CD);
        //所有其他技能进入公共CD
    }
    public void PlusCD(Skill skill,float cd)
    {
        if(skill.id!=this.skill.id)
        {
            return;
        }
        if(CD>=cd)
        {
            currentTime+=cd;
        }
        else
        {
            currentTime=CD;
        }

    }
    ///<summary>控制按钮可用性</summary>
    ///<param name ="state">按钮是否可用</param>
    public void ContrlButton(bool state)
    {
        if(button ==null)
        {
            return;
        }
        if(UIBattle.Instance.isBattleOver)
        {
            button.interactable =false;
            mask.gameObject.SetActive(true);
            CDNumber.text ="";
            return;
        }
        button.interactable =state;
        mask.gameObject.SetActive(!state);
        
    }
    void EndCD()
    {
        intoCD =false;
        ContrlButton(true);
    }
    void ChangeCDText(float num)
    {
        currentChangeText =0;
        CDNumber.text =num.ToString("0.0");
    }
}
