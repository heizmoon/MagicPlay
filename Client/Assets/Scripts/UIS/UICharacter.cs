using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UICharacter : MonoBehaviour
{
    public static UICharacter instance;
    public Toggle tabCharacter;
    public Toggle tabGroup;
    public GameObject viewCharacter;
    public GameObject viewGroup;
    public Transform contentsCharacter;
    public Transform contentsGroup;
    public GameObject infomationChar;
    public Toggle info_char_tabBasic;
    public Toggle info_char_tabTrait;
    public Toggle info_char_tabDo;
    public GameObject info_char_viewBasic;
    public GameObject info_char_viewTrait;
    public GameObject info_char_viewDo;
    public Text info_char_basic_name;
    public Text info_char_basic_describe;
    public Text info_char_basic_basic;
    public Text info_char_basic_state;
    public HPBar hpBar_like;
    public Transform traitContent;
    public Text textTraitDescribe;
    public List<int> createdCharacters =new List<int>();

    CharacterData currentCharacter;
    void Awake()
    {
        instance =this;
        InitUI();
    }
    void InitUI()
    {
        tabCharacter.onValueChanged.AddListener(isOn =>OnTabChange(isOn,tabCharacter));
        tabGroup.onValueChanged.AddListener(isOn =>OnTabChange(isOn,tabGroup));
        info_char_tabBasic.onValueChanged.AddListener(isOn =>OnSelectInfoTab(isOn,info_char_tabBasic));
        info_char_tabTrait.onValueChanged.AddListener(isOn =>OnSelectInfoTab(isOn,info_char_tabTrait));
        info_char_tabDo.onValueChanged.AddListener(isOn =>OnSelectInfoTab(isOn,info_char_tabDo));
    }
    void OnTabChange(bool isOn,Toggle toggle)
    {
        if(!isOn)
        {
            return;
        }
        if(toggle ==tabCharacter)
        {
            viewGroup.SetActive(false);
            viewCharacter.SetActive(true);
        }
        else
        {
            viewCharacter.SetActive(false);
            viewGroup.SetActive(true);
        }
    }
    public void CreateCharacters()
    {
        CharacterManager.instance.RefreashCharacterState();
        RefreashCharacterItemData();
        foreach (var item in CharacterManager.instance.dataList)
        {
            if(createdCharacters.Contains(item.id))
            {
                Debug.LogFormat("此时infoLevel={0}",item._infoLevel);
                continue;
            }
            if(item._infoLevel>0)//正式版本改为>0--------------------------------------------------------------------------
            {
                Debug.LogFormat("创建时为{0}岁,hp={1}",item._age,item._hp);
                CreateCharacterItem(item);
                createdCharacters.Add(item.id);
            }
        }
    }
    void RefreashCharacterItemData()
    {
        for (int i = 0; i < contentsCharacter.childCount; i++)
        {
            var item =contentsCharacter.GetChild(i).GetComponent<CharacterItem>();
            item.RefreashData();
        }
    }
    public void CreateCharacterItem(CharacterData data)
    {
        CharacterItem item =Instantiate((GameObject)Resources.Load("Prefabs/CharacterItem")).GetComponent<CharacterItem>();
        item.transform.SetParent(contentsCharacter);
        item.transform.localPosition =Vector3.zero;
        item.transform.localScale =Vector3.one;
        item.CreateItem(data);
        item.GetComponent<Toggle>().onValueChanged.AddListener(isOn =>OnSelectCharacterItem(isOn,item));
    }
    void OnSelectCharacterItem(bool isOn,CharacterItem item)
    {
        infomationChar.SetActive(true);
        // Debug.LogFormat("item.data._infoLevel ={0}",item.data._infoLevel);
        currentCharacter =item.data;
        RefreashCharacterInfomation();
        LoadTrait();
    }
    void OnSelectInfoTab(bool isOn,Toggle toggle)
    {
        if(!isOn)
        {
            return;
        }
        if(toggle ==info_char_tabBasic)
        {
            info_char_viewTrait.SetActive(false);
            info_char_viewDo.SetActive(false);
            info_char_viewBasic.SetActive(true);
            
        }
        else if(toggle ==info_char_tabTrait)
        {
            info_char_viewBasic.SetActive(false);
            info_char_viewDo.SetActive(false);
            info_char_viewTrait.SetActive(true);
            
        }
        else if(toggle ==info_char_tabDo)
        {
            info_char_viewBasic.SetActive(false);
            info_char_viewTrait.SetActive(false);
            info_char_viewDo.SetActive(true);
        }
    }
    void RefreashCharacterInfomation()
    {
        info_char_basic_name.text =currentCharacter.name;
        if(currentCharacter._infoLevel ==1)
        {
            info_char_basic_describe.text =currentCharacter.describe1;
        }
        else if(currentCharacter._infoLevel ==2)
        {
            info_char_basic_describe.text =currentCharacter.describe2;
        }
        else if(currentCharacter._infoLevel >=3)
        {
            info_char_basic_describe.text =currentCharacter.describe3;
        }
        string gender;
        if(currentCharacter.gender==0)
        {
            gender="♀";
        }
        else
        {
            gender ="♂";
        }
        string age;
        // Debug.LogFormat("----{0}岁",currentCharacter._age);
        if(currentCharacter._Alive)
        {
            age=currentCharacter._age+"岁";
        }
        else
        {
            age="已故";
        }
        string marriage;
        if(currentCharacter._marriage)
        {
            marriage ="已婚";
        }
        else
        {
            marriage ="未婚";
        }
        string rank ="";
        switch(currentCharacter._nowRank)
        {
            case 0:
            rank ="魔法学徒";
            break;
        }
        info_char_basic_basic.text =string.Format("{0} {1} {2} {3}",gender,age,marriage,rank);
        string state="";
        int max = 40;
        int current =0;
        if(currentCharacter._like<=-60)
        {
            //-60 = 40; -100 = 0;
            current =currentCharacter._like+100;
            state = "厌恶";
        }
        else if(currentCharacter._like<=-20)
        {
            //-20 = 40; -60 =0;
            current =currentCharacter._like+60;
            state = "反感";
        }
        else if(currentCharacter._like<20)
        {
            //20 = 40; -20 =0;
            current =currentCharacter._like+20;
            state = "平淡";
        }
        else if(currentCharacter._like<60)
        {
            //20 = 0; 60 =40;
            current =currentCharacter._like-20;
            state = "友好";
        }
        else
        {
            //60 = 0; 100 =40;
            current =currentCharacter._like-60;
            state = "爱慕";
        }
        info_char_basic_state.text =state;
        hpBar_like.initHpBar(current,max);
        hpBar_like.HpText.text =string.Format("{0}",currentCharacter._like);
    }
    public void HideInformation()
    {
        infomationChar.SetActive(false);
    }
    void LoadTrait()
    {
        DestroyTraitBoxes();
        StartCoroutine(WaitForDestory());
    }
    IEnumerator WaitForDestory()
    {
        yield return new WaitForEndOfFrame();
        for (int i = 0; i < currentCharacter._traitList.Count; i++)
        {
            //创建traitBOX
            Toggle toggle =Instantiate((GameObject)Resources.Load("Prefabs/TraitBox")).GetComponent<Toggle>();
            var trait= toggle.GetComponent<TraitBox>();
            trait.data =TraitManager.instance.GetInfo(currentCharacter._traitList[i]);
            trait.id =i;
            toggle.onValueChanged.AddListener((bool isOn)=>SelectTrait(isOn,trait));
            if(currentCharacter._infoLevel<3)
            {
                if(i<currentCharacter._infoLevel)
                {
                    toggle.GetComponentInChildren<Text>().text =TraitManager.instance.GetInfo(currentCharacter._traitList[i],"name");
                    trait.isOpen = true;
                }
                else
                {
                    toggle.GetComponentInChildren<Text>().text ="???";
                    trait.isOpen = false;
                }
            }
            else
            {
                toggle.GetComponentInChildren<Text>().text =TraitManager.instance.GetInfo(currentCharacter._traitList[i],"name");
                trait.isOpen = true;
            }
            toggle.transform.SetParent(traitContent);
            toggle.transform.localPosition =Vector3.zero;
            toggle.transform.localScale =Vector3.one;
            toggle.group =traitContent.GetComponent<ToggleGroup>();
        }
    }
    void SelectTrait(bool isOn,TraitBox box)
    {
        if(!isOn)
        {
            return;
        }
        if(box.isOpen)
        {
            textTraitDescribe.text =box.data.describe;
        }
        else
        {
            textTraitDescribe.text ="你需要对ta了解更多以查看这个特质";
        }
    }
    void DestroyTraitBoxes()
    {
        for (int i = traitContent.childCount-1; i >=0 ; i--)
        {
            GameObject go =traitContent.GetChild(i).gameObject;
            go.GetComponent<Toggle>().onValueChanged.RemoveAllListeners();
            Destroy(go);
        }
    }

}
