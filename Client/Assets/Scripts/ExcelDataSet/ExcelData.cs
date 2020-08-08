using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DropGroup { 
    public int id;
    public string name;
    public string list;
    
}
[System.Serializable]
public class Ability{
    public int id;
    public string name;
    public string discribe;
    public string icon;
    public string spellEffect;
    public string castEffect;
    public string hitEffect;
    public float spelllTime;
    public float CD;
    public bool isChannel;
    public float maxChannelTime;
    public bool ifActive;
    public int genre;
    public int manaCost;
    public float manaCostPercent;
    public int damage;
    public string damageDistribution;
    public float damagePercent;
    public bool targetSelf;
    public int fast;
    public int hit;
    public int crit;
    public int seep;
    public int manaProduce;
    public float manaProducePercent;
    public int manaCostGrow;
    public int damageGrow;
    public int hitGrow;
    public int fastGrow;
    public int critGrow;
    public int seepGrow;
    
    public int manaProduceGrow;
    public int buffID;
    public bool ifNoDamage;
}
[System.Serializable]
public class AssetsItemData
{            
    public int id;
    public string name;
    public string discribe;
    public string icon;
    public int type;
    public int equipType;
    public int value;
    public int hpBuffer;
    public int mpBuffer;
    public int dodgeBuffer;
    public int toughBuffer;
    public int resistance0;
    public int resistance1;
    public int resistance2;
    public int resistance3;
    public int resistance4;
    public int resistance5;
    public int resistance6;
    public int resistance7;
    public int valueGrow;
    public int hpGrow;
    public int mpGrow;
    public int dodgeGrow;
    public int toughGrow;
    public int resistanceGrow;
    public int skillBuffer;
    public int skillBufferLevel;
    public int skillBufferGrow;
    public string unlockSkill;
    public string buffList;
    public int life;
}
[System.Serializable]
public struct AbyssGroupData
{            
    public int id;
    public string groupName;
    public string discribe;
    public string icon;
    public string background;
    public int timeCost;
    public int goldReward;
    public int startLevel;
    public int endLevel;
    public int battles;
    ///<summary>死亡率：百分比*100</summary>        
    public int deathLevel;
    public string monsterGroups;
    public string relicGroupDistribution;
    public string dropGropDistribution;
    public string eventDistribution;
    public int monsterLevelInterval;
    public int bossLevelInterval;
    public int monsterBasicLevel;
}
[System.Serializable]
public struct CharacterData
{            
    public int id;
    public string name;
    public int gender;
    public string birthday;
    public string deathday;
    //如果到结婚日，其对玩家的好感度未达到爱慕以上，则角色会结婚
    //如果达到爱慕以上，则结婚日会推后1年，等待玩家
    public string marriageDay;
    public string describe1;
    public string describe2;
    public string describe3;
    public string prefab;
    public string rank;
    public string traitList;
    public string skillList;
    public string assetsList;
    public string gold;
    public string updateTime;
    public int skillGrow;
    public int hpGrow;
    public int mpGrow;
    public int resistanceGrow;
    public string resistances;
    public string portrait;
    public bool _Alive;
    public int _age;
    public bool _marriage;
    public int _state;
    public int _nowRank;
    public int _nowGold;
    // public List<AssetsItem> _nowAssets;
    public List<int> _nowSkill;
    public int _nowSkillLevel;
    public int _hp;
    public int _mp;
    public List<int> _resistance;
    public Date _birthday;
    public Date _deathday;
    public Date _marriageDay;
    public bool _waitingForMarriage;
    public int _like;
    ///<summary>好感度等级 0=平淡，1=友好，2=爱慕，-1=反感，-2=厌恶</summary>
    public int _likeState;
    ///<summary>情报等级</summary>
    public int _infoLevel;
    public List<int> _traitList;

}
[System.Serializable]
public struct DiagueDatabase
{
    public int id;
    public int character;
    public string maskName;
    public string content;
    public int nextID;
    public string choose1;
    public string choose2;
    public int next1;
    public int next2;
}
[System.Serializable]
///<summary>考试问题表</summary>
public struct QuestionData
{            
    public int id;
    public int bank;
    public string question;
    public string sprite;
    public string answer0;
    public string answer1;
    public string answer2;
    public string answer3;
    public int correctAnswer;

}
[System.Serializable]
///<summary>题库索引表</summary>
public struct QuestionDatabase
{
    public int bankID;
    public int num;
    public int minID;
}
[System.Serializable]
public struct MonsterTypeData
{            
    public int id;
    public string monsterName;
    public string prefab;
    public int type;
    public string skills;
    public int hp;
    public int mp;
    public int dodge;
    public int tough;
    public int aiType;
    public int resistance0;
    public int resistance1;
    public int resistance2;
    public int resistance3;
    public int resistance4;
    public int resistance5;
    public int resistance6;
    public int resistance7;
    public int skillGrow;
    public int hpGrow;
    public int mpGrow;
    public int dodgeGrow;
    public int toughGrow;
    public int resistanceGrow;
}
[System.Serializable]
public struct MonsterGroupData
{            
    public int id;
    public string name;
    public string monsterDistrubtion;
    public int leader;
    public int scene;
    
}
[System.Serializable]
public struct TraitData
{            
public int id;
public string name;
public string describe;
public string getDescribe;
public string unlockIdeal;
public string likeTrait;
public string unlikeTrait;
public string buffList;
public int getMethod;
public string lateDate;
public string skillLevel;
public string icon;
public bool defaultShow;
public bool showMethod;
public List<int> _unlockIdeal;   
public List<int> _likeTrait;
public List<int> _unlikeTrait;
public List<int> _buffList;
public Date _lateDate;
public List<int> _needSkill;
public List<int> _needSkillLevel;

}
[System.Serializable]
public struct RelicData
{            
    public int id;
    public string name;
    public string icon;
    public string describe;
    public int group;
    public bool sole;
    public bool judge;
    public string buff;
    public List<int> _buffList;       
}
[System.Serializable]
public struct RelicGroupData
{            
    public int id;
    public string tip;
    public string list;
             
}
[System.Serializable]
public class TriggerEventsData
{            
    public int id;
    public string eventName;
    public string discribe;
    public string scene;
    public int timeLimit;
    public int timeInterval;  
    public string prefab;      
    public int rankLimit;
    public string professionLimit;
    public string successEvents;
    public string failEvents;
    public string rewards;
    
    public string trailList;
    public string noTrailList;
    public string idealList;
    public string skillList;
    public string skillTotalLevelList;
    public string assetsList;
    public string noAssetsList;
    public int cashGreater;
    public int cashLess;
    public string assetsNum;
    public int assetsTotalValueGreater;
    public int assetsTotalValueLess;
    public string needLike;
    public string needRenown;
    public int influenceGreater;
    public int influenceLess;
    public int abyssGreater;
    public int abyssLess;
    public int SGold;
    public string SLike;
    public string SRenown;
    public int SInfluence;
    public int FGold;
    public string FLike;
    public string FRenown;
    public int FInfluence;
    public int successTrigger;
    public int failTrigger;
    public int doneTrigger;
    ///<summary>成功后引出事件</summary>
    public int successTask;
    ///<summary>失败后引出事件</summary>
    public int failTask;
    ///<summary>完成后引出事件</summary>
    public int doneTask;
    public string SInfoLevel;
    public string FInfoLevel;
    public string earlyDate;
    public string lateDate;
    public string STrait;
    public string FTrait;


    ///<summary>上次事件时间</summary>
    public Date _lastDate;
    public List<int> _professionLimit;
    public List<int> _assetsList;
    public List <int> _noAssetslist;

    ///<summary>完成次数</summary>
    public int doneTime;
    ///<summary>事件完成的结果<summary>
    public int result;
    public Date _earlyDate;
    public Date _lateDate;
    public List<int> _successEvents;
    public List<int> _failEvents;
    public List<int> _STrait;
    public List<int> _FTrait;
    public List<int> _SInfoLevel;
    public List<int> _FInfoLevel;
    public List<int> _SlikeCharacters;
    public List<int> _SlikeCharacterAdds;
    public List<int> _FlikeCharacters;
    public List<int> _FlikeCharacterAdds;

}
[System.Serializable]
public class TaskEventsData
{            
    public int id;
    public string eventName;
    public string discribe;
    public string icon;
    ///<summary>0=常驻事件,1=世界事件,2=引出事件</summary>
    public int type;
    ///<summary>对于世界事件，此项为具体开始日期；对于引出事件，此项为引出后多久开始</summary>
    public string startDate;
    public int timeCost;
    ///<summary>次数限制</summary>
    public int timeLimit;
    public int timeInterval;        
    public int rankLimit;
    public string professionLimit;
    public string successEvents;
    public string failEvents;
    public string rewards;
    public string prefab;
    public string trailList;
    public string noTrailList;
    public string idealList;
    public string skillList;
    public string skillTotalLevelList;
    public string assetsList;
    public string noAssetsList;
    public int cashGreater;
    public int cashLess;
    public string assetsNum;
    public int assetsTotalValueGreater;
    public int assetsTotalValueLess;
    public string needLike;
    public string needRenown;
    public int influenceGreater;
    public int influenceLess;
    public int abyssGreater;
    public int abyssLess;
    public int goldCost;
    public int influenceCost;
    public int SGold;
    public string SLike;
    public string SRenown;
    public int SInfluence;
    public int FGold;
    public string FLike;
    public string FRenown;
    public int FInfluence;
    public string SInfoLevel;
    public string FInfoLevel;
    public bool ifDateLock;
    public string STrait;
    public string FTrait;
    public string SText;
    public string FText;
    public string SunlockSkill;
    public string FunlockSkill;
    public Date _startDate;
    ///<summary>上次事件时间</summary>
    public Date _lastDate;
    public List<int> _professionLimit;
    public List<int> _assetsList;
    public List <int> _noAssetslist;
    ///<summary>0="", 1=new!, 2="已预约"</summary>
    public int _state;
    ///<summary>事件结束时间</summary>
    public Date _endDate;
    ///<summary>完成次数</summary>
    public int doneTime;
    ///<summary>事件完成的结果:0=未完成，1=成功，2=失败<summary>
    public int result;
    public List<int> _successEvents;
    public List<int> _failEvents;
    public List<int> _SInfoLevel;
    public List<int> _FInfoLevel;
    public List<int> _SlikeCharacters;
    public List<int> _SlikeCharacterAdds;
    public List<int> _FlikeCharacters;
    public List<int> _FlikeCharacterAdds;
    public List<int> _STrait;
    public List<int> _FTrait;
    public List<int> _SUnlockSkill;
    public List<int> _FUnlockSkill;
    public List<int> _SRenown;
    public List<int> _SRenownAdds;
    public List<int> _FRenown;
    public List<int> _FRenownAdds;



}
[System.Serializable]
public class GuildData
{
    public int id;
    public string name;
    public string describe;
    public string icon;
    public string buff0;
    public string buff1;
    public string buff2;
    public string buff3;
    public string buff4;
    public List<int> _buff0;
    public List<int> _buff1;
    public List<int> _buff2;
    public List<int> _buff3;
    public List<int> _buff4;
}
[System.Serializable]
public class BuffData
{
    public int id;
    public string name;
    public string describe;
    public string icon;
    public string prefab;
    public int type;
    public float time;
    public float effectInterval;
    public string genreList;
    public float value;
    public float valueGrow;
    public string triggerEffect;
    public string childrenBuff;
    ///<summary>最大叠加层数</summary>
    public int maxNum;
    public int abilityID;
    public float delay;
    public int groupType;
    public List<int> _genreList;
    public List<int> _childrenBuff;
    public BuffType _type;

}
[System.Serializable]
public class SceneData
{
    public int id;
    public string sceneName;
    public string prefab;
    public string describe;
    public string buff;
    public List<int> _buffList;

}
[System.Serializable]
public class IdealData
{
    public int id;
    public string name;
    public string describe;

}
