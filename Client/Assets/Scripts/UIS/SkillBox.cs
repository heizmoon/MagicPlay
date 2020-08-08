using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillBox : MonoBehaviour
{
    // Start is called before the first frame update
    public int id;
    public Text levelText;
    public Text nameText;
    public Image icon;
    public GameObject mark;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Init(int id)
    {
        this.id =id;
        nameText.text = SkillManager.instance.GetInfo(id,"name");
        levelText.text = Player.instance.GetSkillLevel(id).ToString();
        icon.enabled =true;
        icon.sprite = Resources.Load("Texture/Skills/"+SkillManager.instance.GetInfo(id,"icon"),typeof(Sprite)) as Sprite;
    }
    public void Clear()
    {
        id =0;
        nameText.text = "";
        levelText.text ="";
        icon.enabled =false;
    }
    public void Mark()
    {
        mark.SetActive(true);
    }
    public void UnMark()
    {
        mark.SetActive(false);
    }
}
