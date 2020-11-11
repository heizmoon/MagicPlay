using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prepare : MonoBehaviour
{
    //1.从Character中随机出3个供玩家选择
    //2.当玩家选择角色完毕，改变画面，选择主动技能，神器
    //3.当玩家点击准备完毕后，正式进入战斗
    public CharacterData finalCharacter;
    CharacterData[] characters =new CharacterData[3];
    
    void Start()
    {
        characters =CharacterManager.instance.RandomCharacters(3);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPlayerChooseOneCharacter(CharacterData data)
    {
        finalCharacter =data;
    }
    //点击任意一个可更换的技能，会弹出技能窗口，供选择
    //点击不可更换的技能，会弹出提示：固有技能无法更换
    public void OnPressChooseSkill()
    {
        //显示技能搭配界面
    }
    public void OnPressChooseAsset()
    {
        //显示神器选择界面
    }

}
