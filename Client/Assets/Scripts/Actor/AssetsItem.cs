using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssetsItem : MonoBehaviour
{
    public Text nameText;
    public Text stateText;
    public int _id;
    public string _name;
    public string _describe;
    public string _icon;
    public int _type;//0=装备，1=房屋，2=魔兽，3=土地
    public int _equipType;//0=武器；1=服装；2=饰品
    public int _value;
    public int _hpBuffer;
    public int _mpBuffer;
    public int _dodgeBuffer;
    public int _toughBuffer;
    public int[] _resistance =new int[8];
    public int _valueGrow;
    public int _hpGrow;
    public int _mpGrow;
    public int _dodgeGrow;
    public int _toughGrow;
    public int _resistanceGrow;
    public int _skillBuffer;//对于某个技能的加成
    public int _skillBufferLevel;//对某个技能加成的等级
    public int _skillBufferGrow;
    public List<int> _unlockSkill =new List<int>();//解锁某些技能
    public List<int> _buffList =new List<int>();//附加某些buff
    ///<summary>资产的使用时长，单位：月</summary>
    public int _life;
    public int level;
    public int uid;//该资产的唯一ID；
    ///<summary>资产的获取时间，由游戏中的日期 年-月 转化而来</summary>
    public int freashTime;
    public bool equip;//该资产的状态
    AssetsItemData itemData;
   

    //获取时间可计算折旧
    void Start()
    {
        
    }

    ///<summary>创建一个资产</summary>
    ///<param name ="uid">资产的唯一ID</param>
    ///<param name ="id">资产的模板id</param>
    ///<param name ="name">自定义资产的名称</param>
    ///<param name ="level">资产的级别</param>
    ///<param name ="freashTime">资产的获取时间</param>
    public void CreateAssets(int uid,int id, string name,int level,int freashTime)
    {
        this.uid =uid;
        _id =id;
        itemData = AssetsManager.instance.GetInfo(id);
        LoadFromExcel();
        if(name!="")
        {
            _name =name;
        }
        this.level = level;
        ModiferLevel();
        this.freashTime =freashTime;
        nameText.text =_name;
        string skillName ="";
        if(_skillBuffer>0)
        {
            skillName = SkillManager.instance.GetInfo(_skillBuffer,"name");
        }
        string unlockListName ="";
        if(_unlockSkill.Count>0)
        {
            foreach (var item in _unlockSkill)
            {
                unlockListName =unlockListName+ "," +SkillManager.instance.GetInfo(item,"name");
            }
            unlockListName = unlockListName.Remove(0,1);
        }
        _describe =string.Format(_describe,_hpBuffer,_mpBuffer,_dodgeBuffer,_toughBuffer,_resistance[0],
        _resistance[1],_resistance[2],_resistance[3],_resistance[4],_resistance[5],_resistance[6],_resistance[7],
        skillName,_skillBufferLevel,unlockListName);
        ChangeItemState();
    }
    void LoadFromExcel()
    {
        _name =itemData.name;
        _icon =itemData.icon;
        _mpBuffer =itemData.mpBuffer;
        _hpBuffer=itemData.hpBuffer;
        _hpGrow =itemData.hpGrow;
        _mpGrow =itemData.mpGrow;
        _type =itemData.type;
        _describe =itemData.describe;
        _value =itemData.value;
        _equipType =itemData.equipType;
        _dodgeBuffer =itemData.dodgeBuffer;
        _dodgeGrow =itemData.dodgeGrow;
        _resistance[0] =itemData.resistance0;
        _resistance[1] =itemData.resistance1;
        _resistance[2] =itemData.resistance2;
        _resistance[3] =itemData.resistance3;
        _resistance[4] =itemData.resistance4;
        _resistance[5] =itemData.resistance5;
        _resistance[6] =itemData.resistance6;
        _resistance[7] =itemData.resistance7;
        _resistanceGrow =itemData.resistanceGrow;
        _skillBuffer =itemData.skillBuffer;
        _skillBufferLevel =itemData.skillBufferLevel;
        _skillBufferGrow =itemData.skillBufferGrow;
        _toughBuffer =itemData.toughBuffer;
        _toughGrow =itemData.toughGrow;
        _valueGrow =itemData.valueGrow;
        _life =itemData.life;

        if(itemData.unlockSkill!="")
        {
            string[] ss =itemData.buffList.Split(',');
            foreach (var item in ss)
            {
                _unlockSkill.Add(int.Parse(item));
            }
        }
        if(itemData.buffList!="")
        {
            string[] s0 =itemData.buffList.Split(',');
        
            foreach (var item in s0)
            {
                _buffList.Add(int.Parse(item));
            }
        }
         
    }
    void ModiferLevel()
    {
        if(level<2)
        {
            return;
        }
        _mpBuffer +=level*_mpGrow;
        _hpBuffer +=level*_hpGrow;
        _dodgeBuffer += level*_dodgeGrow;
        _toughBuffer += level* _toughGrow;
        _skillBufferLevel+=level*_skillBufferGrow;
        for(int i =0;i<_resistance.Length;i++)
        {
            if(_resistance[i]>0)
            {
                _resistance[i] += level*_resistanceGrow;
            }
        }
        _value += level* _valueGrow;

    }
    void PlayerGetThisAsset()
    {
        //生成改资产的uid
        //生成改资产的freashTime
        //将该资产加入玩家的资产列表
        //将该资产的数据保存
    }
    public void ChangeItemState()
    {
        if(equip)
        {
            switch(_type)
            {
                case 0:
                stateText.text ="已穿戴";
                break;
                case 1:
                stateText.text ="我的家";
                break;
                case 2:
                stateText.text ="骑着";
                break;
            }
            
        }
        else
        {
            stateText.text ="";
        }
    }
    public void DecayLife(int month)
    {
        if(_life!=-1)
        {
            _life-=month;
            if(_life<=0)
            {
                _life =0;
                //资产坏了
                //移除该资产
                Player.instance.PlayerUnEquipAssets(this);
                AssetsManager.instance.items.Remove(this);
                Destroy(this.gameObject);
            }
            else if(_life<int.Parse(AssetsManager.instance.GetInfo(_id,"life"))*0.2f)
            {
                //资产寿命低于20%

            }
            else if(_life<int.Parse(AssetsManager.instance.GetInfo(_id,"life"))*0.5f)
            {
                //资产寿命低于50%

            }
            else if(_life<int.Parse(AssetsManager.instance.GetInfo(_id,"life"))*0.8f)
            {
                //资产寿命低于80%

            }
        }
        
    }
    ///<summary>尝试贩卖</summary>
    public void TrySellItem()
    {
        //首先判断是否可以卖
        //判断是不是正装备着，装备着的话卸掉装备
        //影响各种数值
        //当前价值是多少
        //角色金钱增加
        //摧毁物品
        //
        // if(assetsItem.equip)
        // {
        //     Player.instance.PlayerUnEquipAssets(assetsItem);
        // }
        // Player.instance.gold+=assetsItem._value;
    }
}
