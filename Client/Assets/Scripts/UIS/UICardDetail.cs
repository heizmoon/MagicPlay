using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UICardDetail : MonoBehaviour
{
    Button background;
    Transform point ;
    Transform tempParent;
    Transform target;
    int type;
    void Awake()
    {
        background =transform.Find("Background").gameObject.GetComponent<Button>();
        background.onClick.AddListener(CloseUI);
        point = transform.Find("Point");
    }
    public void Init(SkillCard skillCard)
    {
        target = skillCard.transform;
        tempParent =target.parent;
        target.SetParent(point);
        target.localPosition =Vector3.zero;
        target.localScale = new Vector3(2.5f,2.5f,2.5f);
        type =0;
        // Debug.Log("技能卡详情");
    }
    public void Init(ItemBox itemBox)
    {
        target = itemBox.transform;
        tempParent =target.parent;
        target.SetParent(point);
        target.localPosition =Vector3.zero;
        target.localScale = new Vector3(2.5f,2.5f,2.5f);
        type =1;
        itemBox.toggle.onValueChanged.RemoveAllListeners();
        // Debug.Log("查看详情");

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void CloseUI()
    {
        target.SetParent(tempParent);
        target.localPosition =Vector3.zero;
        target.localScale = Vector3.one;
        if(type == 0&&!UIBattle.Instance)
        {
            target.GetComponent<SkillCard>().canShow =true;
        }
        if(type == 1)
        {
            target.GetComponent<ItemBox>().HideToggleSelect();
        }
        Destroy(gameObject);
    }
    public static UICardDetail CreateUI()
    {
        UICardDetail cardDetail =((GameObject)Instantiate(Resources.Load("Prefabs/UICardDetail"))).GetComponent<UICardDetail>();
        cardDetail.transform.SetParent(Main.instance.allScreenUI);
        cardDetail.transform.localScale =Vector3.one;
        cardDetail.transform.localPosition =Vector3.zero;
        cardDetail.GetComponent<RectTransform>().sizeDelta =Vector3.one;
        cardDetail.GetComponent<RectTransform>().anchoredPosition =Vector2.zero;
        return cardDetail;
    }

}
