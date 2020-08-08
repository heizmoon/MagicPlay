using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Data;
using System.IO;
using Data;
using Excel;

namespace EditorTool {
    public class ExcelTool {
        
        static DropGroup[] droupGroupArray;
        static Ability[] abilityArray;
        static AssetsItemData[] assetsItemArray;
        static AbyssGroupData[] abyssGroups;
        static CharacterData[] characterDatas;
        static DiagueDatabase[] diagueDatabases;
        static QuestionData[] questionDatas;
        static QuestionDatabase[] questionDatabases;
        static MonsterTypeData[] monsterTypeDatas;
        static MonsterGroupData[] monsterGroupDatas;
        static TraitData[] traitDatas;
        static RelicData[] relicDatas;
        static TaskEventsData[] taskEventsDatas;
        static TriggerEventsData[] triggerEventsDatas;
        static BuffData[] buffDatas;
        static SceneData[] sceneDatas;
        static GuildData[] guildDatas;
        static IdealData[] idealDatas;
        static RelicGroupData[] relicGroupDatas;







        public static void CreateItemArrayWithExcel(string filePath,string type)
        {
            switch(type)
            {
                
                case "Ability":
                CreateAbilityArray(filePath);
                break;
                case "AssetsItem":
                CreateAssetsItemArray(filePath);
                break;
                case "AbyssGroup":
                CreateAbyssGroupArray(filePath);
                break;
                case "Character":
                CreateCharacterArray(filePath);
                break;
                case "DropGroup":
                CreateDropGroupArray(filePath);
                break;
                case "Dialogue":
                CreateDialogueArray(filePath);
                break;
                case "Question":
                CreateQuestionArray(filePath);
                break;
                case "QuestionDatabase":
                CreateQuestionDatabaseArray(filePath);
                break;
                case "MonsterType":
                CreateMonsterTypeArray(filePath);
                break;
                case "MonsterGroup":
                CreateMonsterGroupArray(filePath);
                break;
                case "Relic":
                CreateRelicArray(filePath);
                break;
                case "TaskEvent":
                CreateTaskEventsArray(filePath);
                break;
                case "TriggerEvent":
                CreateTriggerEventsArray(filePath);
                break;
                case "Trait":
                CreateTraitArray(filePath);
                break;
                case "Buff":
                CreateBuffArray(filePath);
                break;
                case "Scene":
                CreateSceneArray(filePath);
                break;
                case "Guild":
                CreateGuildArray(filePath);
                break;
                case "Ideal":
                CreateIdealArray(filePath);
                break;
                case "RelicGroup":
                CreateRelicGroupArray(filePath);
                break;


            }

        }

        /// <summary>
        /// 读取表数据，生成对应的数组
        /// </summary>
        /// <param name="filePath">excel文件全路径</param>
        /// <returns>Item数组</returns>
        public static void CreateCharacterArray(string filePath) 
        {
            //获得表数据
            int columnNum = 0, rowNum = 0;
            DataRowCollection collect = ReadExcel(filePath, ref columnNum, ref rowNum);
 
            //根据excel的定义，第二行开始才是数据
            CharacterData[] array = new CharacterData[rowNum - 2];
            for(int i = 2; i < rowNum; i++) 
            {
                CharacterData item = new CharacterData();
                //解析每列的数据
                item.id = collect[i][0].ToString()==""?0:int.Parse(collect[i][0].ToString());
                item.name = collect[i][1].ToString();
                item.gender = collect[i][2].ToString()==""?0:int.Parse(collect[i][2].ToString());
                item.birthday = collect[i][3].ToString();
                item.deathday = collect[i][4].ToString();
                item.marriageDay = collect[i][5].ToString();
                item.describe1 = collect[i][6].ToString();
                item.describe2 = collect[i][7].ToString();
                item.describe3 = collect[i][8].ToString();
                item.prefab = collect[i][9].ToString();
                item.rank =collect[i][10].ToString();
                item.traitList =collect[i][11].ToString();
                item.skillList =collect[i][12].ToString();
                item.assetsList =collect[i][13].ToString();
                item.gold =collect[i][14].ToString();
                item.updateTime =collect[i][15].ToString();
                item.skillGrow =collect[i][16].ToString()==""?0:int.Parse(collect[i][16].ToString());
                item.hpGrow =collect[i][17].ToString()==""?0:int.Parse(collect[i][17].ToString());
                item.mpGrow =collect[i][18].ToString()==""?0:int.Parse(collect[i][18].ToString());
                item.resistanceGrow =collect[i][19].ToString()==""?0:int.Parse(collect[i][19].ToString());
                item.resistances =collect[i][20].ToString();
                item.portrait =collect[i][21].ToString();
                item._traitList =GetListIntFromString(item.traitList);
                array[i - 2] = item;
            }
            characterDatas =array;
        }
        public static void CreateDropGroupArray(string filePath) 
        {
            //获得表数据
            int columnNum = 0, rowNum = 0;
            DataRowCollection collect = ReadExcel(filePath, ref columnNum, ref rowNum);
 
            //根据excel的定义，第二行开始才是数据
            DropGroup[] array = new DropGroup[rowNum - 2];
            for(int i = 2; i < rowNum; i++) 
            {
                DropGroup item = new DropGroup();
                //解析每列的数据
                item.id = collect[i][0].ToString()==""?0:int.Parse(collect[i][0].ToString());
                item.name = collect[i][1].ToString();
                item.list = collect[i][2].ToString();
                array[i - 2] = item;
            }
            droupGroupArray =array;
        }
        public static void CreateRelicGroupArray(string filePath) 
        {
            //获得表数据
            int columnNum = 0, rowNum = 0;
            DataRowCollection collect = ReadExcel(filePath, ref columnNum, ref rowNum);
 
            //根据excel的定义，第二行开始才是数据
            RelicGroupData[] array = new RelicGroupData[rowNum - 2];
            for(int i = 2; i < rowNum; i++) 
            {
                RelicGroupData item = new RelicGroupData();
                //解析每列的数据
                item.id = collect[i][0].ToString()==""?0:int.Parse(collect[i][0].ToString());
                item.tip = collect[i][1].ToString();
                item.list = collect[i][2].ToString();
                array[i - 2] = item;
            }
            relicGroupDatas =array;
        }
        public static void CreateIdealArray(string filePath) 
        {
            //获得表数据
            int columnNum = 0, rowNum = 0;
            DataRowCollection collect = ReadExcel(filePath, ref columnNum, ref rowNum);
 
            //根据excel的定义，第二行开始才是数据
            IdealData[] array = new IdealData[rowNum - 2];
            for(int i = 2; i < rowNum; i++) 
            {
                IdealData item = new IdealData();
                //解析每列的数据
                item.id = collect[i][0].ToString()==""?0:int.Parse(collect[i][0].ToString());
                item.name = collect[i][1].ToString();
                item.describe = collect[i][2].ToString();
                array[i - 2] = item;
            }
            idealDatas =array;
        }
        public static void CreateTaskEventsArray(string filePath) 
        {
            //获得表数据
            int columnNum = 0, rowNum = 0;
            DataRowCollection collect = ReadExcel(filePath, ref columnNum, ref rowNum);
            
            //根据excel的定义，第二行开始才是数据
            TaskEventsData[] array = new TaskEventsData[rowNum - 2];
            for(int i = 2; i < rowNum; i++) 
            {
                TaskEventsData item = new TaskEventsData();
                //解析每列的数据
                item.id = collect[i][0].ToString()==""?0:int.Parse(collect[i][0].ToString());
                item.eventName = collect[i][1].ToString();
                item.discribe = collect[i][2].ToString();
                item.icon = collect[i][3].ToString();
                item.type = collect[i][4].ToString()==""?0: int.Parse(collect[i][4].ToString());
                item.startDate =collect[i][5].ToString();
                item.timeCost = collect[i][6].ToString()==""?0: int.Parse(collect[i][6].ToString());
                item.timeLimit = collect[i][7].ToString()==""?0: int.Parse(collect[i][7].ToString());
                item.timeInterval = collect[i][8].ToString()==""?0: int.Parse(collect[i][8].ToString());
                item.rankLimit = collect[i][9].ToString()==""?0: int.Parse(collect[i][9].ToString());
                item.professionLimit = collect[i][10].ToString();
                item.successEvents = collect[i][11].ToString();
                item.failEvents = collect[i][12].ToString();
                item.rewards = collect[i][13].ToString();
                item.prefab = collect[i][14].ToString();
                item.trailList = collect[i][15].ToString();
                item.noTrailList = collect[i][16].ToString();
                item.idealList = collect[i][17].ToString();
                item.skillList = collect[i][18].ToString();
                item.skillTotalLevelList = collect[i][19].ToString();
                item.assetsList = collect[i][20].ToString();
                item.noAssetsList = collect[i][21].ToString();
                item.cashGreater =collect[i][22].ToString()==""?0: int.Parse(collect[i][22].ToString());
                item.cashLess =collect[i][23].ToString()==""?0: int.Parse(collect[i][23].ToString());
                item.assetsNum =collect[i][24].ToString();
                item.assetsTotalValueGreater =collect[i][25].ToString()==""?0: int.Parse(collect[i][25].ToString());
                item.assetsTotalValueLess =collect[i][26].ToString()==""?0: int.Parse(collect[i][26].ToString());
                item.needLike =collect[i][27].ToString();
                item.needRenown =collect[i][28].ToString();
                item.influenceGreater =collect[i][29].ToString()==""?0: int.Parse(collect[i][29].ToString());
                item.influenceLess =collect[i][30].ToString()==""?0: int.Parse(collect[i][30].ToString());
                item.abyssGreater =collect[i][31].ToString()==""?0: int.Parse(collect[i][31].ToString());
                item.abyssLess =collect[i][32].ToString()==""?0: int.Parse(collect[i][32].ToString());
                item.goldCost =collect[i][33].ToString()==""?0: int.Parse(collect[i][33].ToString());
                item.influenceCost =collect[i][34].ToString()==""?0: int.Parse(collect[i][34].ToString());
                item.SGold =collect[i][35].ToString()==""?0: int.Parse(collect[i][35].ToString());
                item.SLike =collect[i][36].ToString();
                item.SRenown =collect[i][37].ToString();
                item.SInfluence =collect[i][38].ToString()==""?0: int.Parse(collect[i][38].ToString());
                item.FGold =collect[i][39].ToString()==""?0: int.Parse(collect[i][43].ToString());
                item.FLike =collect[i][40].ToString();
                item.FRenown =collect[i][41].ToString();
                item.FInfluence =collect[i][42].ToString()==""?0: int.Parse(collect[i][46].ToString());
                item.SInfoLevel =collect[i][43].ToString();
                item.FInfoLevel =collect[i][44].ToString();
                item.ifDateLock =collect[i][45].ToString()==""?false:true;
                item.STrait =collect[i][46].ToString();
                item.FTrait =collect[i][47].ToString();
                item.SText =collect[i][48].ToString();
                item.FText =collect[i][49].ToString();
                item.SunlockSkill =collect[i][50].ToString();
                item.FunlockSkill =collect[i][51].ToString();


                item._professionLimit =GetListIntFromString(item.professionLimit);
                item._successEvents =GetListIntFromString(item.successEvents);
                item._failEvents =GetListIntFromString(item.failEvents);
                item._assetsList =GetListIntFromString(item.assetsList);
                item._noAssetslist =GetListIntFromString(item.noAssetsList);
                item._STrait =GetListIntFromString(item.STrait);
                item._FTrait =GetListIntFromString(item.FTrait);
                item._SInfoLevel =GetListIntFromString(item.SInfoLevel);
                item._FInfoLevel =GetListIntFromString(item.FInfoLevel);
                item._SlikeCharacters =GetListIntFromLongString(item.SLike,true);
                item._SlikeCharacterAdds =GetListIntFromLongString(item.SLike,false);
                item._FlikeCharacters =GetListIntFromLongString(item.FLike,true);
                item._FlikeCharacterAdds =GetListIntFromLongString(item.FLike,false);
                item._SUnlockSkill =GetListIntFromString(item.SunlockSkill);
                item._FUnlockSkill =GetListIntFromString(item.FunlockSkill);
                item._SRenown =GetListIntFromLongString(item.SRenown,true);
                item._FRenown =GetListIntFromLongString(item.FRenown,true);
                item._SRenownAdds =GetListIntFromLongString(item.SRenown,false);
                item._FRenownAdds =GetListIntFromLongString(item.FRenown,false);
               
                array[i - 2] = item;
            }
            taskEventsDatas =array;
        }
        public static void CreateTraitArray(string filePath) 
        {
            //获得表数据
            int columnNum = 0, rowNum = 0;
            DataRowCollection collect = ReadExcel(filePath, ref columnNum, ref rowNum);
 
            //根据excel的定义，第二行开始才是数据
            TraitData[] array = new TraitData[rowNum - 2];
            for(int i = 2; i < rowNum; i++) 
            {
                TraitData item = new TraitData();
                //解析每列的数据
                item.id = collect[i][0].ToString()==""?0:int.Parse(collect[i][0].ToString());
                item.name = collect[i][1].ToString();
                item.describe = collect[i][2].ToString();
                item.getDescribe = collect[i][3].ToString();
                item.unlockIdeal =collect[i][4].ToString();
                item.likeTrait =collect[i][5].ToString();
                item.unlikeTrait =collect[i][6].ToString();
                item.buffList =collect[i][7].ToString();
                item.getMethod = collect[i][8].ToString()==""?0:int.Parse(collect[i][8].ToString());
                item.lateDate = collect[i][9].ToString();
                item.skillLevel =collect[i][10].ToString();
                item.icon =collect[i][11].ToString();
                item.defaultShow =collect[i][12].ToString()=="1"?true:false;
                item.showMethod =collect[i][13].ToString()=="1"?true:false;

                item._buffList =GetListIntFromString(item.buffList);
                item._likeTrait =GetListIntFromString(item.likeTrait);
                item._unlikeTrait =GetListIntFromString(item.unlikeTrait);
                item._unlockIdeal =GetListIntFromString(item.unlockIdeal);
                
                item._needSkill =GetListIntFromLongString(item.skillLevel,true);
                item._needSkillLevel =GetListIntFromLongString(item.skillLevel,false);
                array[i - 2] = item;
            }
            traitDatas =array;
        }
        public static void CreateTriggerEventsArray(string filePath) 
        {
            //获得表数据
            int columnNum = 0, rowNum = 0;
            DataRowCollection collect = ReadExcel(filePath, ref columnNum, ref rowNum);
 
            //根据excel的定义，第二行开始才是数据
            TriggerEventsData[] array = new TriggerEventsData[rowNum - 2];
            for(int i = 2; i < rowNum; i++) 
            {
                TriggerEventsData item = new TriggerEventsData();
                //解析每列的数据
                item.id = collect[i][0].ToString()==""?0:int.Parse(collect[i][0].ToString());
                item.eventName = collect[i][1].ToString();
                item.discribe = collect[i][2].ToString();
                item.scene = collect[i][3].ToString();
                item.timeLimit = collect[i][4].ToString()==""?0: int.Parse(collect[i][4].ToString());
                item.timeInterval = collect[i][5].ToString()==""?0: int.Parse(collect[i][5].ToString());
                item.prefab = collect[i][6].ToString();
                item.rankLimit = collect[i][7].ToString()==""?0: int.Parse(collect[i][7].ToString());
                item.professionLimit = collect[i][8].ToString();
                item.successEvents = collect[i][9].ToString();
                item.failEvents = collect[i][10].ToString();
                item.rewards = collect[i][11].ToString();
                item.trailList = collect[i][12].ToString();
                item.noTrailList = collect[i][13].ToString();
                item.idealList = collect[i][14].ToString();
                item.skillList = collect[i][15].ToString();
                item.skillTotalLevelList = collect[i][16].ToString();
                item.assetsList = collect[i][17].ToString();
                item.noAssetsList = collect[i][18].ToString();
                item.cashGreater =collect[i][19].ToString()==""?0: int.Parse(collect[i][19].ToString());
                item.cashLess =collect[i][20].ToString()==""?0: int.Parse(collect[i][20].ToString());
                item.assetsNum =collect[i][21].ToString();
                item.assetsTotalValueGreater =collect[i][22].ToString()==""?0: int.Parse(collect[i][22].ToString());
                item.assetsTotalValueLess =collect[i][23].ToString()==""?0: int.Parse(collect[i][23].ToString());
                item.needLike =collect[i][24].ToString();
                item.needRenown =collect[i][25].ToString();
                item.influenceGreater =collect[i][26].ToString()==""?0: int.Parse(collect[i][26].ToString());
                item.influenceLess =collect[i][27].ToString()==""?0: int.Parse(collect[i][27].ToString());
                item.abyssGreater =collect[i][28].ToString()==""?0: int.Parse(collect[i][28].ToString());
                item.abyssLess =collect[i][29].ToString()==""?0: int.Parse(collect[i][29].ToString());
                item.SGold =collect[i][30].ToString()==""?0: int.Parse(collect[i][30].ToString());
                item.SLike =collect[i][31].ToString();
                item.SRenown =collect[i][32].ToString();
                item.SInfluence =collect[i][33].ToString()==""?0: int.Parse(collect[i][33].ToString());
                item.FGold =collect[i][34].ToString()==""?0: int.Parse(collect[i][34].ToString());
                item.FLike =collect[i][35].ToString();
                item.FRenown =collect[i][36].ToString();
                item.FInfluence =collect[i][37].ToString()==""?0: int.Parse(collect[i][37].ToString());
                item.successTrigger =collect[i][38].ToString()==""?0: int.Parse(collect[i][38].ToString());
                item.failTrigger=collect[i][39].ToString()==""?0: int.Parse(collect[i][39].ToString());
                item.doneTrigger =collect[i][40].ToString()==""?0: int.Parse(collect[i][40].ToString());
                item.successTask =collect[i][41].ToString()==""?0: int.Parse(collect[i][41].ToString());
                item.failTask =collect[i][42].ToString()==""?0: int.Parse(collect[i][42].ToString());
                item.doneTask =collect[i][43].ToString()==""?0: int.Parse(collect[i][43].ToString());
                item.SInfoLevel =collect[i][44].ToString();
                item.FInfoLevel =collect[i][45].ToString();
                item.earlyDate =collect[i][46].ToString();
                item.lateDate =collect[i][47].ToString();
                item.STrait =collect[i][48].ToString();
                item.FTrait =collect[i][49].ToString();

                item._professionLimit =GetListIntFromString(item.professionLimit);
                item._failEvents =GetListIntFromString(item.failEvents);
                item._successEvents =GetListIntFromString(item.successEvents);
                item._STrait =GetListIntFromString(item.STrait);
                item._FTrait =GetListIntFromString(item.FTrait);
                item._assetsList =GetListIntFromString(item.assetsList);
                item._noAssetslist =GetListIntFromString(item.noAssetsList);
                item._SInfoLevel =GetListIntFromString(item.SInfoLevel);
                item._FInfoLevel =GetListIntFromString(item.FInfoLevel);
                item._SlikeCharacters =GetListIntFromLongString(item.SLike,true);
                item._SlikeCharacterAdds =GetListIntFromLongString(item.SLike,false);
                item._FlikeCharacters =GetListIntFromLongString(item.FLike,true);
                item._FlikeCharacterAdds =GetListIntFromLongString(item.FLike,false);
                array[i - 2] = item;
            }
            triggerEventsDatas =array;
        }
        public static void CreateRelicArray(string filePath) 
        {
            //获得表数据
            int columnNum = 0, rowNum = 0;
            DataRowCollection collect = ReadExcel(filePath, ref columnNum, ref rowNum);
 
            //根据excel的定义，第二行开始才是数据
            RelicData[] array = new RelicData[rowNum - 2];
            for(int i = 2; i < rowNum; i++) 
            {
                RelicData item = new RelicData();
                //解析每列的数据
                item.id = collect[i][0].ToString()==""?0:int.Parse(collect[i][0].ToString());
                item.name = collect[i][1].ToString();
                item.icon = collect[i][2].ToString();
                item.describe = collect[i][3].ToString();
                item.group =collect[i][4].ToString()==""?0: int.Parse(collect[i][4].ToString());
                item.sole =collect[i][5].ToString()=="1"?true:false;
                item.judge =collect[i][6].ToString()=="1"?true:false;
                item.buff = collect[i][7].ToString();
                item._buffList =GetListIntFromString(item.buff);
                array[i - 2] = item;
            }
            relicDatas =array;
        }
        public static void CreateBuffArray(string filePath) 
        {
            //获得表数据
            int columnNum = 0, rowNum = 0;
            DataRowCollection collect = ReadExcel(filePath, ref columnNum, ref rowNum);
 
            //根据excel的定义，第二行开始才是数据
            BuffData[] array = new BuffData[rowNum - 2];
            for(int i = 2; i < rowNum; i++) 
            {
                BuffData item = new BuffData();
                //解析每列的数据
                item.id = collect[i][0].ToString()==""?0:int.Parse(collect[i][0].ToString());
                item.name = collect[i][1].ToString();
                item.describe = collect[i][2].ToString();
                item.icon = collect[i][3].ToString();
                item.prefab =collect[i][4].ToString();
                item.type =collect[i][5].ToString()==""?0:int.Parse(collect[i][5].ToString());
                item.time =collect[i][6].ToString()==""?0:float.Parse(collect[i][6].ToString());
                item.effectInterval =collect[i][7].ToString()==""?0:float.Parse(collect[i][7].ToString());
                item.genreList =collect[i][8].ToString();
                item.value =collect[i][9].ToString()==""?0:float.Parse(collect[i][9].ToString());
                item.valueGrow =collect[i][10].ToString()==""?0:float.Parse(collect[i][10].ToString());
                item.triggerEffect =collect[i][11].ToString();
                item.maxNum =collect[i][12].ToString()==""?0:int.Parse(collect[i][12].ToString());
                item.childrenBuff =collect[i][13].ToString();
                item.abilityID =collect[i][14].ToString()==""?0:int.Parse(collect[i][14].ToString());
                item.delay =collect[i][15].ToString()==""?0:float.Parse(collect[i][15].ToString());
                item.groupType =collect[i][16].ToString()==""?0:int.Parse(collect[i][16].ToString());
                item._type =(BuffType)item.type;
                item._genreList =GetListIntFromString(item.genreList);
                item._childrenBuff =GetListIntFromString(item.childrenBuff);
                array[i - 2] = item;
            }
            buffDatas =array;
        }
        public static void CreateSceneArray(string filePath) 
        {
            //获得表数据
            int columnNum = 0, rowNum = 0;
            DataRowCollection collect = ReadExcel(filePath, ref columnNum, ref rowNum);
 
            //根据excel的定义，第二行开始才是数据
            SceneData[] array = new SceneData[rowNum - 2];
            for(int i = 2; i < rowNum; i++) 
            {
                SceneData item = new SceneData();
                //解析每列的数据
                item.id = collect[i][0].ToString()==""?0:int.Parse(collect[i][0].ToString());
                item.sceneName = collect[i][1].ToString();
                item.prefab = collect[i][2].ToString();
                item.describe = collect[i][3].ToString();
                item.buff =collect[i][4].ToString();
                
                item._buffList =GetListIntFromString(item.buff);
                array[i - 2] = item;
            }
            sceneDatas =array;
        }
        public static void CreateGuildArray(string filePath) 
        {
            //获得表数据
            int columnNum = 0, rowNum = 0;
            DataRowCollection collect = ReadExcel(filePath, ref columnNum, ref rowNum);
 
            //根据excel的定义，第二行开始才是数据
            GuildData[] array = new GuildData[rowNum - 2];
            for(int i = 2; i < rowNum; i++) 
            {
                GuildData item = new GuildData();
                //解析每列的数据
                item.id = collect[i][0].ToString()==""?0:int.Parse(collect[i][0].ToString());
                item.name = collect[i][1].ToString();
                item.describe = collect[i][2].ToString();
                item.icon = collect[i][3].ToString();
                item.buff0 =collect[i][4].ToString();
                item.buff1 =collect[i][5].ToString();
                item.buff2 =collect[i][6].ToString();
                item.buff3 =collect[i][7].ToString();
                item.buff4 =collect[i][8].ToString();

                item._buff0 =GetListIntFromString(item.buff0);
                item._buff1 =GetListIntFromString(item.buff1);
                item._buff2 =GetListIntFromString(item.buff2);
                item._buff3 =GetListIntFromString(item.buff3);
                item._buff4 =GetListIntFromString(item.buff4);


                array[i - 2] = item;
            }
            guildDatas =array;
        }
        public static void CreateMonsterGroupArray(string filePath) 
        {
            //获得表数据
            int columnNum = 0, rowNum = 0;
            DataRowCollection collect = ReadExcel(filePath, ref columnNum, ref rowNum);
 
            //根据excel的定义，第二行开始才是数据
            MonsterGroupData[] array = new MonsterGroupData[rowNum - 2];
            for(int i = 2; i < rowNum; i++) 
            {
                MonsterGroupData item = new MonsterGroupData();
                //解析每列的数据
                item.id = collect[i][0].ToString()==""?0:int.Parse(collect[i][0].ToString());
                item.name = collect[i][1].ToString();
                item.monsterDistrubtion = collect[i][2].ToString();
                item.leader =collect[i][3].ToString()==""?0: int.Parse(collect[i][3].ToString());
                item.scene =collect[i][4].ToString()==""?0: int.Parse(collect[i][4].ToString());
                
                array[i - 2] = item;
            }
            monsterGroupDatas =array;
        }
        public static void CreateMonsterTypeArray(string filePath) 
        {
            //获得表数据
            int columnNum = 0, rowNum = 0;
            DataRowCollection collect = ReadExcel(filePath, ref columnNum, ref rowNum);
 
            //根据excel的定义，第二行开始才是数据
            MonsterTypeData[] array = new MonsterTypeData[rowNum - 2];
            for(int i = 2; i < rowNum; i++) 
            {
                MonsterTypeData item = new MonsterTypeData();
                //解析每列的数据
                item.id = collect[i][0].ToString()==""?0:int.Parse(collect[i][0].ToString());
                item.monsterName = collect[i][1].ToString();
                item.prefab = collect[i][2].ToString();
                item.type = collect[i][3].ToString()==""?0: int.Parse(collect[i][3].ToString());
                item.skills = collect[i][4].ToString();
                item.hp = collect[i][5].ToString()==""?0: int.Parse(collect[i][5].ToString());
                item.mp = collect[i][6].ToString()==""?0: int.Parse(collect[i][6].ToString());
                item.dodge = collect[i][7].ToString()==""?0: int.Parse(collect[i][7].ToString());
                item.tough = collect[i][8].ToString()==""?0: int.Parse(collect[i][8].ToString());
                item.aiType = collect[i][9].ToString()==""?0: int.Parse(collect[i][9].ToString());
                item.resistance0 = collect[i][10].ToString()==""?0: int.Parse(collect[i][10].ToString());
                item.resistance1 = collect[i][11].ToString()==""?0: int.Parse(collect[i][11].ToString());
                item.resistance2 = collect[i][12].ToString()==""?0: int.Parse(collect[i][12].ToString());
                item.resistance3 = collect[i][13].ToString()==""?0: int.Parse(collect[i][13].ToString());
                item.resistance4 = collect[i][14].ToString()==""?0: int.Parse(collect[i][14].ToString());
                item.resistance5 = collect[i][15].ToString()==""?0: int.Parse(collect[i][15].ToString());
                item.resistance6 = collect[i][16].ToString()==""?0: int.Parse(collect[i][16].ToString());
                item.resistance7 = collect[i][17].ToString()==""?0: int.Parse(collect[i][17].ToString());
                item.skillGrow = collect[i][18].ToString()==""?0: int.Parse(collect[i][18].ToString());
                item.hpGrow = collect[i][19].ToString()==""?0: int.Parse(collect[i][19].ToString());
                item.mpGrow = collect[i][20].ToString()==""?0: int.Parse(collect[i][20].ToString());
                item.dodgeGrow = collect[i][21].ToString()==""?0: int.Parse(collect[i][21].ToString());
                item.toughGrow = collect[i][22].ToString()==""?0: int.Parse(collect[i][22].ToString());
                item.resistanceGrow = collect[i][23].ToString()==""?0: int.Parse(collect[i][23].ToString());
               
                array[i - 2] = item;
            }
            monsterTypeDatas =array;
        }
        public static void CreateQuestionArray(string filePath) 
        {
            //获得表数据
            int columnNum = 0, rowNum = 0;
            DataRowCollection collect = ReadExcel(filePath, ref columnNum, ref rowNum);
 
            //根据excel的定义，第二行开始才是数据
            QuestionData[] array = new QuestionData[rowNum - 2];
            for(int i = 2; i < rowNum; i++) 
            {
                QuestionData item = new QuestionData();
                //解析每列的数据
                item.id = collect[i][0].ToString()==""?0:int.Parse(collect[i][0].ToString());
                item.bank = collect[i][1].ToString()==""?0:int.Parse(collect[i][1].ToString());
                item.question = collect[i][2].ToString();
                item.sprite = collect[i][3].ToString();
                item.answer0 = collect[i][4].ToString();
                item.answer1 = collect[i][5].ToString();
                item.answer2 = collect[i][6].ToString();
                item.answer3 = collect[i][7].ToString();
                item.correctAnswer = collect[i][1].ToString()==""?0:int.Parse(collect[i][8].ToString());
                
                array[i - 2] = item;
            }
            questionDatas =array;
        }
        public static void CreateQuestionDatabaseArray(string filePath) 
        {
            //获得表数据
            int columnNum = 0, rowNum = 0;
            DataRowCollection collect = ReadExcel(filePath, ref columnNum, ref rowNum);
 
            //根据excel的定义，第二行开始才是数据
            QuestionDatabase[] array = new QuestionDatabase[rowNum - 2];
            for(int i = 2; i < rowNum; i++) 
            {
                QuestionDatabase item = new QuestionDatabase();
                //解析每列的数据
                item.bankID = collect[i][0].ToString()==""?0:int.Parse(collect[i][0].ToString());
                item.num = collect[i][1].ToString()==""?0:int.Parse(collect[i][1].ToString());
                item.minID = collect[i][2].ToString()==""?0:int.Parse(collect[i][2].ToString());

                array[i - 2] = item;
            }
            questionDatabases =array;
        }
        public static void CreateDialogueArray(string filePath) 
        {
            //获得表数据
            int columnNum = 0, rowNum = 0;
            DataRowCollection collect = ReadExcel(filePath, ref columnNum, ref rowNum);
 
            //根据excel的定义，第二行开始才是数据
            DiagueDatabase[] array = new DiagueDatabase[rowNum - 2];
            for(int i = 2; i < rowNum; i++) 
            {
                DiagueDatabase item = new DiagueDatabase();
                //解析每列的数据
                item.id = collect[i][0].ToString()==""?0:int.Parse(collect[i][0].ToString());
                item.character = collect[i][1].ToString()==""?0:int.Parse(collect[i][1].ToString());
                item.maskName = collect[i][2].ToString();
                item.content = collect[i][3].ToString();
                item.nextID = collect[i][4].ToString()==""?0:int.Parse(collect[i][4].ToString());
                item.choose1 = collect[i][5].ToString();
                item.choose2 = collect[i][6].ToString();
                item.next1 = collect[i][7].ToString()==""?0:int.Parse(collect[i][7].ToString());
                item.next2 = collect[i][8].ToString()==""?0:int.Parse(collect[i][8].ToString());
                array[i - 2] = item;
            }
            diagueDatabases =array;
        }
        public static void CreateAbyssGroupArray(string filePath) {
            //获得表数据
            int columnNum = 0, rowNum = 0;
            DataRowCollection collect = ReadExcel(filePath, ref columnNum, ref rowNum);
 
            //根据excel的定义，第二行开始才是数据
            AbyssGroupData[] array = new AbyssGroupData[rowNum - 2];
            for(int i = 2; i < rowNum; i++) 
            {
                AbyssGroupData item = new AbyssGroupData();
                //解析每列的数据
                item.id = collect[i][0].ToString()==""?0:int.Parse(collect[i][0].ToString());
                item.groupName = collect[i][1].ToString();
                item.discribe = collect[i][2].ToString();
                item.icon = collect[i][3].ToString();
                item.background =collect[i][4].ToString();
                item.timeCost = collect[i][5].ToString()==""?0: int.Parse(collect[i][5].ToString());
                item.goldReward = collect[i][6].ToString()==""?0: int.Parse(collect[i][6].ToString());
                item.startLevel = collect[i][7].ToString()==""?0: int.Parse(collect[i][7].ToString());
                item.endLevel = collect[i][8].ToString()==""?0: int.Parse(collect[i][8].ToString());
                item.battles = collect[i][9].ToString()==""?0: int.Parse(collect[i][9].ToString());
                item.deathLevel = collect[i][10].ToString()==""?0: int.Parse(collect[i][10].ToString());
                item.monsterGroups = collect[i][11].ToString();
                item.relicGroupDistribution = collect[i][12].ToString();
                item.dropGropDistribution = collect[i][13].ToString();
                item.eventDistribution = collect[i][14].ToString();
                item.monsterLevelInterval =collect[i][15].ToString()==""?0: int.Parse(collect[i][15].ToString());
                item.bossLevelInterval =collect[i][16].ToString()==""?0: int.Parse(collect[i][16].ToString());
                item.monsterBasicLevel =collect[i][17].ToString()==""?0: int.Parse(collect[i][17].ToString());



                array[i - 2] = item;
            }
            abyssGroups =array;
            
        }


        public static void CreateAbilityArray(string filePath) {
            //获得表数据
            int columnNum = 0, rowNum = 0;
            DataRowCollection collect = ReadExcel(filePath, ref columnNum, ref rowNum);
 
            //根据excel的定义，第二行开始才是数据
            Ability[] array = new Ability[rowNum - 2];
            for(int i = 2; i < rowNum; i++) {
                Ability ability = new Ability();
                //解析每列的数据
                ability.id = collect[i][0].ToString()==""?0:int.Parse(collect[i][0].ToString());
                ability.name = collect[i][1].ToString();
                ability.discribe = collect[i][2].ToString();
                ability.icon = collect[i][3].ToString();
                ability.spellEffect = collect[i][4].ToString();
                ability.castEffect = collect[i][5].ToString();
                ability.hitEffect = collect[i][6].ToString();

                ability.spelllTime =collect[i][7].ToString()==""?0: float.Parse(collect[i][7].ToString());
                ability.CD = collect[i][8].ToString()==""?0:float.Parse(collect[i][8].ToString());
                ability.isChannel =collect[i][9].ToString()=="1"?true:false;
                ability.maxChannelTime =collect[i][10].ToString()==""?0:float.Parse(collect[i][10].ToString());
                ability.ifActive = collect[i][11].ToString()=="1"?true:false;
                ability.genre =collect[i][12].ToString()==""?0: int.Parse(collect[i][12].ToString());
                ability.manaCost =collect[i][13].ToString()==""?0: int.Parse(collect[i][13].ToString());
                ability.manaCostPercent =collect[i][14].ToString()==""?0:float.Parse(collect[i][14].ToString());
                ability.damage =collect[i][15].ToString()==""?0: int.Parse(collect[i][15].ToString());
                ability.damagePercent =collect[i][16].ToString()==""?0:float.Parse(collect[i][16].ToString());
                ability.damageDistribution =collect[i][17].ToString();

                ability.targetSelf =collect[i][18].ToString()=="1"?true:false;
                
                ability.fast =collect[i][19].ToString()==""?0: int.Parse(collect[i][19].ToString());
                ability.hit =collect[i][20].ToString()==""?0: int.Parse(collect[i][20].ToString());
                ability.crit =collect[i][21].ToString()==""?0: int.Parse(collect[i][21].ToString());
                ability.seep =collect[i][22].ToString()==""?0: int.Parse(collect[i][22].ToString());
                ability.manaProduce =collect[i][23].ToString()==""?0: int.Parse(collect[i][23].ToString());
                ability.manaProducePercent =collect[i][24].ToString()==""?0:float.Parse(collect[i][24].ToString());

                ability.manaCostGrow =collect[i][25].ToString()==""?0: int.Parse(collect[i][25].ToString());
                ability.damageGrow =collect[i][26].ToString()==""?0: int.Parse(collect[i][26].ToString());               
                ability.hitGrow =collect[i][27].ToString()==""?0: int.Parse(collect[i][27].ToString());               
                ability.fastGrow =collect[i][28].ToString()==""?0: int.Parse(collect[i][28].ToString());               
                ability.critGrow =collect[i][29].ToString()==""?0: int.Parse(collect[i][29].ToString());
                ability.seepGrow =collect[i][30].ToString()==""?0: int.Parse(collect[i][30].ToString());
                ability.manaProduceGrow =collect[i][31].ToString()==""?0: int.Parse(collect[i][31].ToString());
                ability.buffID = collect[i][32].ToString()==""?0:int.Parse(collect[i][32].ToString());
                
                ability.ifNoDamage = collect[i][33].ToString()=="1"?true:false;


                array[i - 2] = ability;
            }
              abilityArray =array;
        }
        public static void CreateAssetsItemArray(string filePath) {
            //获得表数据
            int columnNum = 0, rowNum = 0;
            DataRowCollection collect = ReadExcel(filePath, ref columnNum, ref rowNum);
 
            //根据excel的定义，第二行开始才是数据
            AssetsItemData[] array = new AssetsItemData[rowNum - 2];
            for(int i = 2; i < rowNum; i++) {
            AssetsItemData data = new AssetsItemData();
            //解析每列的数据
            data.id = collect[i][0].ToString()==""?0:int.Parse(collect[i][0].ToString());
            data.name = collect[i][1].ToString();
            data.discribe = collect[i][2].ToString();
            data.icon = collect[i][3].ToString();
            data.type = collect[i][4].ToString()==""?0: int.Parse(collect[i][4].ToString());;
            data.equipType = collect[i][5].ToString()==""?0: int.Parse(collect[i][5].ToString());;
            data.value =collect[i][6].ToString()==""?0: int.Parse(collect[i][6].ToString());
            data.hpBuffer =collect[i][7].ToString()==""?0: int.Parse(collect[i][7].ToString());
            data.mpBuffer =collect[i][8].ToString()==""?0: int.Parse(collect[i][8].ToString());
            data.dodgeBuffer =collect[i][9].ToString()==""?0: int.Parse(collect[i][9].ToString());;
            data.toughBuffer = collect[i][10].ToString()==""?0: int.Parse(collect[i][10].ToString());
            data.resistance0 =collect[i][11].ToString()==""?0: int.Parse(collect[i][11].ToString());
            data.resistance1 =collect[i][12].ToString()==""?0: int.Parse(collect[i][12].ToString());
            data.resistance2 =collect[i][13].ToString()==""?0: int.Parse(collect[i][13].ToString());
            data.resistance3 =collect[i][14].ToString()==""?0: int.Parse(collect[i][14].ToString());
            data.resistance4 =collect[i][15].ToString()==""?0: int.Parse(collect[i][15].ToString());
            data.resistance5 =collect[i][16].ToString()==""?0: int.Parse(collect[i][16].ToString());
            data.resistance6 =collect[i][17].ToString()==""?0: int.Parse(collect[i][17].ToString());               
            data.resistance7 =collect[i][18].ToString()==""?0: int.Parse(collect[i][18].ToString());               
            data.valueGrow =collect[i][19].ToString()==""?0: int.Parse(collect[i][19].ToString());               
            data.hpGrow =collect[i][20].ToString()==""?0: int.Parse(collect[i][20].ToString());
            data.mpGrow =collect[i][21].ToString()==""?0: int.Parse(collect[i][21].ToString());
            data.dodgeGrow =collect[i][22].ToString()==""?0: int.Parse(collect[i][22].ToString());
            data.toughGrow =collect[i][23].ToString()==""?0: int.Parse(collect[i][23].ToString());
            data.resistanceGrow =collect[i][24].ToString()==""?0: int.Parse(collect[i][24].ToString());
            data.skillBuffer = collect[i][25].ToString()==""?0:int.Parse(collect[i][25].ToString());
            data.skillBufferLevel = collect[i][26].ToString()==""?0:int.Parse(collect[i][26].ToString());
            data.skillBufferGrow = collect[i][27].ToString()==""?0:int.Parse(collect[i][27].ToString());
            data.unlockSkill = collect[i][28].ToString();
            data.buffList = collect[i][29].ToString();
            data.life = collect[i][30].ToString()==""?-1:int.Parse(collect[i][30].ToString());
            array[i - 2] = data;
            }
            assetsItemArray =array;
        }

        public static DropGroup[] GetItemArray()
        {
            return droupGroupArray;
        }
        public static Ability[] GetAbilityArray()
        {
            return abilityArray;
        }
        public static AssetsItemData[] GetAssetsItemArray()
        {
            return assetsItemArray;
        }
        public static AbyssGroupData[] GetAbyssGroupArray()
        {
            return abyssGroups;
        }
        public static CharacterData[] GetCharacterArray()
        {
            return characterDatas;
        }
        public static DiagueDatabase[] GetDiagueDataArray()
        {
            return diagueDatabases;
        }
        public static QuestionData[] GetQuestionArray()
        {
            return questionDatas;
        }
        public static QuestionDatabase[] GetQuestionDatabaseArray()
        {
            return questionDatabases;
        }
        public static MonsterTypeData[] GetMonsterTypeArray()
        {
            return monsterTypeDatas;
        }
        public static MonsterGroupData[] GetMonsterGroupArray()
        {
            return monsterGroupDatas;
        }
        public static TraitData[] GetTraitArray()
        {
            return traitDatas;
        }
        public static RelicData[] GetRelicArray()
        {
            return relicDatas;
        }
        public static TaskEventsData[] GetTaskEventsArray()
        {
            return taskEventsDatas;
        }
        public static TriggerEventsData[] GetTriggerEventsArray()
        {
            return triggerEventsDatas;
        }
        public static SceneData[] GetSceneArray()
        {
            return sceneDatas;
        }
        public static GuildData[] GetGuildDataArray()
        {
            return guildDatas;
        }
        public static BuffData[] GetBuffDataArray()
        {
            return buffDatas;
        }
        public static IdealData[] GetIdealArray()
        {
            return idealDatas;
        }
        public static RelicGroupData[] GetRelicGroupArray()
        {
            return relicGroupDatas;
        }

        /// <summary>
        /// 读取excel文件内容
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <param name="columnNum">行数</param>
        /// <param name="rowNum">列数</param>
        /// <returns></returns>
        static DataRowCollection ReadExcel(string filePath, ref int columnNum, ref int rowNum) {
            FileStream stream = File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
 
            DataSet result = excelReader.AsDataSet();
            //Tables[0] 下标0表示excel文件中第一张表的数据
            columnNum = result.Tables[0].Columns.Count;
            rowNum = result.Tables[0].Rows.Count;
            return result.Tables[0].Rows;
        }
        ///<summary>字符串要求用【,】分开</summary>
        static List<int> GetListIntFromString(string s)
        {
            List<int> list =new List<int>();
            if(s =="")
            {
                return list;
            }
            string[] ss =s.Split(',');
            foreach (var item in ss)
            {
                list.Add(int.Parse(item));
            }
            return list;
        }
        ///<summary>字符串要求用【|】分开</summary>
        ///<param name ="s">字符串</param>
        ///<param name ="b">ture =【,】前面的部分，false=后面的部分</param>
        static List<int> GetListIntFromLongString(string s,bool b)
        {
            List<int> listfront =new List<int>();
            List<int> listback =new List<int>();
            if(s =="")
            {
                return listfront;
            }
            string[] ss =s.Split('|');
            foreach (var item in ss)
            {
                listfront.Add(int.Parse(item.Split(',')[0]));
                listback.Add(int.Parse(item.Split(',')[1]));

            }
            if(b)
            {
                return listfront;
            }
            else
            {
                return listback;
            }
            
        }
    }
}

