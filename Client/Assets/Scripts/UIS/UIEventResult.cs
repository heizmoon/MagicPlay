using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIEventResult : MonoBehaviour
{
    public TaskEventsData data;
    public Text resultText;
    public Text describeText;
    public Button BTNok;
    public GameObject assetsRewards;
    public Transform assetsContent;
    public GameObject skillRewards;
    public Transform skillContent;
    public GameObject traitRewards;
    public Transform traitContent;
    public GameObject likeRewards;
    public Transform likeContent;
    public GameObject infoLevelRewards;
    public Transform infoLevelContent;
    public GameObject guildRewards;
    public Transform guildContent;
    public GameObject goldObject;
    public GameObject influenceObject;
    public Text goldRewardText;
    public Text influenceRewardText;
    void Start()
    {
        ShowResultUI();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void ShowResultUI()
    {
        //结算界面根据结果
        string describe="";
        switch (data.result)
        {
            case 0:
            //显示失败时的界面
            resultText.text ="失败";
            describe =data.FText;
            break;
            case 1:
            //显示成功时的界面
            
            resultText.text ="成功";
            describe =data.SText;
            
            break;
            case 2:
            //显示逃跑时的界面
            
            resultText.text ="失败";
            describe =data.FText;

            
            break;
            default:
            
            break;
        }
        //复活角色
        Player.instance.playerActor.GetComponent<Actor>().ReLiveActor();
        describeText.text =string.Format(describe,data.timeCost);
        
        ShowAssetsReward();
        ShowSkillReward(data.result);
        ShowTraitReward(data.result);
        ShowLikeReward(data.result);
        ShowInfoLevelReward(data.result);
        ShowGold(data.result);
        ShowInfluence(data.result);
    }
    void ShowAssetsReward()
    {
        if(data.rewards=="")
        {
            return;
        }
        string[] rewardsList =data.rewards.Split('|');
        for(int num =0;num< rewardsList.Length;num++)
        {
            AssetsItem assetsItem = AssetsManager.instance.CreateNewAssets(int.Parse(rewardsList[num].Split(',')[0]),int.Parse(rewardsList[num].Split(',')[1]));
            AssetsManager.instance.GiveAssetsToPlayer(assetsItem);
            ItemBox box =((GameObject)Instantiate(Resources.Load("Prefabs/itemBox"))).GetComponent<ItemBox>();
            box.Init(assetsItem);
            box.transform.SetParent(assetsContent);
            box.transform.localScale =Vector3.one;
        }
        assetsRewards.SetActive(true);
        
    }
    void ShowTraitReward(int result)
    {
        if(result==1&&data.STrait=="")
        {
            return;
        }
        if(result==2&&data.FTrait=="")
        {
            return;
        }
        List<int> list ;
        if(result==1)
        {
            list =data._STrait;
        }
        else
        {
            list =data._FTrait;

        }
        
        for(int num =0;num< list.Count;num++)
        {
            ItemBox box =((GameObject)Instantiate(Resources.Load("Prefabs/itemBox"))).GetComponent<ItemBox>();
            box.transform.SetParent(traitContent);
            box.transform.localScale =Vector3.one;
            TraitData data = TraitManager.instance.GivePlayerTrait(list[num]);
            box.Init(data);
        }
        traitRewards.SetActive(true);
        
    }
    void ShowSkillReward(int result)
    {
        if(result==1&&data.SunlockSkill=="")
        {
            return;
        }
        if(result==2&&data.FunlockSkill=="")
        {
            return;
        }
        List<int> list ;
        if(result==1)
        {
            list =data._SUnlockSkill;
        }
        else
        {
            list =data._FUnlockSkill;
        }
        
        for(int num =0;num< list.Count;num++)
        {
            Ability data = SkillManager.instance.GetInfo(list[num]);
            ItemBox box =((GameObject)Instantiate(Resources.Load("Prefabs/itemBox"))).GetComponent<ItemBox>();
            box.transform.SetParent(skillContent);
            box.transform.localScale =Vector3.one;
            box.Init(data);
        }
        skillRewards.SetActive(true);
        
    }
    void ShowLikeReward(int result)
    {
        if(result==1&&data.SLike=="")
        {
            return;
        }
        if(result==2&&data.FLike=="")
        {
            return;
        }
        List<int> list ;
        List<int> list2 ;

        if(result==1)
        {
            list =data._SlikeCharacters;
            list2 =data._SlikeCharacterAdds;
        }
        else
        {
            list =data._FlikeCharacters;
            list2 =data._FlikeCharacterAdds;
        }
        
        for(int num =0;num< list.Count;num++)
        {
            CharacterData data = CharacterManager.instance.GetInfo(list[num]);
            ItemBox box =((GameObject)Instantiate(Resources.Load("Prefabs/itemBox"))).GetComponent<ItemBox>();
            box.transform.SetParent(likeContent);
            box.transform.localScale =Vector3.one;
            box.Init(data,list2[num]);
        }
        likeRewards.SetActive(true);
        
    }
    void ShowInfoLevelReward(int result)
    {
        if(result==1&&data.SLike=="")
        {
            return;
        }
        if(result==2&&data.FLike=="")
        {
            return;
        }
        List<int> list ;

        if(result==1)
        {
            list =data._SlikeCharacters;    
        }
        else
        {
            list =data._FlikeCharacters;
        }
        int count =0;
        for(int num =0;num< list.Count;num++)
        {
            CharacterData data = CharacterManager.instance.GetInfo(list[num]);
            if(data._infoLevel>=3)
            {
                continue;
            }
            ItemBox box =((GameObject)Instantiate(Resources.Load("Prefabs/itemBox"))).GetComponent<ItemBox>();
            box.transform.SetParent(infoLevelContent);
            box.transform.localScale =Vector3.one;
            box.Init(data);
            count++;
        }
        if(count>0)
        infoLevelRewards.SetActive(true);
        
    }
    void ShowGuildReward(int result)
    {
        if(result==1&&data.SRenown=="")
        {
            return;
        }
        if(result==2&&data.FRenown=="")
        {
            return;
        }
        List<int> list ;
        List<int> list2 ;

        if(result==1)
        {
            list =data._SRenown;
            list2 =data._SRenownAdds;
        }
        else
        {
            list =data._FRenown;
            list2 =data._FRenownAdds;
        }
        
        for(int num =0;num< list.Count;num++)
        {
            // GuildData data = GuildManager.instance.GetInfo(list[num]);
            ItemBox box =((GameObject)Instantiate(Resources.Load("Prefabs/itemBox"))).GetComponent<ItemBox>();
            box.transform.SetParent(guildContent);
            box.transform.localScale =Vector3.one;
            // box.Init(data,list2[num]);
        }
        guildRewards.SetActive(true);
        
    }
    void ShowGold(int result)
    {
        if(result ==1&& data.SGold==0)
        {
            return;
        }
        if(result ==2&& data.FGold==0)
        {
            return;
        }
        int gold =0;
        if(result ==1)
        {
            gold = data.SGold;
        }
        else
        {
            gold = data.FGold;
        }
        goldObject.SetActive(true);
        goldRewardText.text = gold.ToString();
        
    }
    void ShowInfluence(int result)
    {
        if(result ==1&& data.SInfluence==0)
        {
            return;
        }
        if(result ==2&& data.FInfluence==0)
        {
            return;
        }
        int gold =0;
        if(result ==1)
        {
            gold = data.SInfluence;
        }
        else
        {
            gold = data.FInfluence;
        }
        influenceObject.SetActive(true);
        influenceRewardText.text =gold.ToString();  
    }
    
    public void CloseUI()
    {
        Destroy(gameObject);
    }
}
