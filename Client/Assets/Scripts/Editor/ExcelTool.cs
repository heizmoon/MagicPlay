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
        static SkillData[] skillDataArray;
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
        static ReformData[] reformDatas;
        static AbilityData[] abilityDatas;
        static SummonData[] summonDatas;
        static RandomEvent[] randomEventArray;
        static ShopData[] shopDatas;
        static LevelData[] levelDatas;








        public static void CreateItemArrayWithExcel(string filePath,string type)
        {
            switch(type)
            {
                
                case "Skill":
                CreateSkillArray(filePath);
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
                case "Reform":
                CreateReformArray(filePath);
                break;
                case "Ability":
                CreateAbilityArray(filePath);
                break;
                case "Summon":
                CreateSummonArray(filePath);
                break;
                case "RandomEvent":
                CreateRandomEventArray(filePath);
                break;
                case "ShopData":
                CreateShopArray(filePath);
                break;
                case "LevelData":
                CreateLevelArray(filePath);
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
                item.describe = collect[i][2].ToString();
                item.prefab = collect[i][3].ToString();
                item.hp =collect[i][4].ToString()==""?0:int.Parse(collect[i][4].ToString());
                item.mp =collect[i][5].ToString()==""?0:int.Parse(collect[i][5].ToString());
                item.attack =collect[i][6].ToString()==""?0:int.Parse(collect[i][6].ToString());
                item.skills =collect[i][7].ToString();
                item.portrait = collect[i][8].ToString();
                item.reMp =collect[i][9].ToString()==""?0:float.Parse(collect[i][9].ToString());
                item.crit =collect[i][10].ToString()==""?0:float.Parse(collect[i][10].ToString());
                item.initialUnlockSkills =collect[i][11].ToString();
                item.relic =collect[i][12].ToString()==""?0:int.Parse(collect[i][12].ToString());
                item.buildList =collect[i][13].ToString();

                item.initialUnlockSkillsList =GetListIntFromString(item.initialUnlockSkills);
                item._buildList =GetListIntFromString(item.buildList);

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
        public static void CreateLevelArray(string filePath) 
        {
            //获得表数据
            int columnNum = 0, rowNum = 0;
            DataRowCollection collect = ReadExcel(filePath, ref columnNum, ref rowNum);
 
            //根据excel的定义，第二行开始才是数据
            LevelData[] array = new LevelData[rowNum - 2];
            for(int i = 2; i < rowNum; i++) 
            {
                LevelData item = new LevelData();
                //解析每列的数据
                item.level = collect[i][0].ToString()==""?0:int.Parse(collect[i][0].ToString());
                item.exp = collect[i][1].ToString()==""?0:int.Parse(collect[i][1].ToString());
                item.addAttack = collect[i][2].ToString()==""?0:int.Parse(collect[i][2].ToString());
                item.addDefence = collect[i][3].ToString()==""?0:int.Parse(collect[i][3].ToString());
                item.addHPMax= collect[i][4].ToString()==""?0:int.Parse(collect[i][4].ToString());

                array[i - 2] = item;
            }
            levelDatas =array;
        }
        public static void CreateRandomEventArray(string filePath) 
        {
            //获得表数据
            int columnNum = 0, rowNum = 0;
            DataRowCollection collect = ReadExcel(filePath, ref columnNum, ref rowNum);
 
            //根据excel的定义，第二行开始才是数据
            RandomEvent[] array = new RandomEvent[rowNum - 2];
            for(int i = 2; i < rowNum; i++) 
            {
                RandomEvent item = new RandomEvent();
                //解析每列的数据
                item.id = collect[i][0].ToString()==""?0:int.Parse(collect[i][0].ToString());
                item.describe = collect[i][1].ToString();
                item.image = collect[i][2].ToString();
                item.rank = collect[i][3].ToString()==""?0:int.Parse(collect[i][3].ToString());
                item.des1 = collect[i][4].ToString();
                item.res1 = collect[i][5].ToString();
                item.img1 = collect[i][6].ToString();
                item.eff1 = collect[i][7].ToString();
                item.des2 = collect[i][8].ToString();
                item.res2 = collect[i][9].ToString();
                item.img2 = collect[i][10].ToString();
                item.eff2 = collect[i][11].ToString();
                item.des3 = collect[i][12].ToString();
                item.res3 = collect[i][13].ToString();
                item.img3 = collect[i][14].ToString();
                item.eff3 = collect[i][15].ToString();
                item.des4 = collect[i][16].ToString();
                item.des4 = collect[i][17].ToString();
                item.img4 = collect[i][18].ToString();
                item.eff4 = collect[i][19].ToString();
                item.eList1 =GetListIntFromString(item.eff1);
                item.eList2 =GetListIntFromString(item.eff2);
                item.eList3 =GetListIntFromString(item.eff3);
                item.eList4 =GetListIntFromString(item.eff4);

                array[i - 2] = item;
            }
            randomEventArray =array;
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
                item.describe = collect[i][2].ToString();
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
                item.describe = collect[i][2].ToString();
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
                item.removeType =collect[i][17].ToString()==""?0:int.Parse(collect[i][17].ToString());
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
                item.hp = collect[i][4].ToString()==""?0: int.Parse(collect[i][4].ToString());
                item.attack = collect[i][5].ToString()==""?0: int.Parse(collect[i][5].ToString());
                item.crit = collect[i][6].ToString()==""?0: int.Parse(collect[i][6].ToString());
                item.switchCondition1 = collect[i][7].ToString()==""?0: int.Parse(collect[i][7].ToString());
                item.addBuffList1 = collect[i][8].ToString();
                item.removeBuffList1 = collect[i][9].ToString();
                item.AIMode1 = collect[i][10].ToString()==""?0: int.Parse(collect[i][10].ToString());
                item.AIType1 = collect[i][11].ToString();
                item.listAttack1 = collect[i][12].ToString();
                item.listDefend1 = collect[i][13].ToString();
                item.listBuff1 = collect[i][14].ToString();
                item.listNerf1 = collect[i][15].ToString();
                item.switchCondition2 = collect[i][16].ToString()==""?0: int.Parse(collect[i][16].ToString());
                item.addBuffList2 = collect[i][17].ToString();
                item.removeBuffList2 = collect[i][18].ToString();
                item.AIMode2 = collect[i][19].ToString()==""?0: int.Parse(collect[i][19].ToString());
                item.AIType2 = collect[i][20].ToString();
                item.listAttack2 = collect[i][21].ToString();
                item.listDefend2 = collect[i][22].ToString();
                item.listBuff2 = collect[i][23].ToString();
                item.listNerf2 = collect[i][24].ToString();
                item.switchCondition3 = collect[i][25].ToString()==""?0: int.Parse(collect[i][25].ToString());
                item.addBuffList3 = collect[i][26].ToString();
                item.removeBuffList3 = collect[i][27].ToString();
                item.AIMode3 = collect[i][28].ToString()==""?0: int.Parse(collect[i][28].ToString());
                item.AIType3 = collect[i][29].ToString();
                item.listAttack3 = collect[i][30].ToString();
                item.listDefend3 = collect[i][31].ToString();
                item.listBuff3 = collect[i][32].ToString();
                item.listNerf3 = collect[i][33].ToString();

                item.m_aitype1 =GetListIntFromString(item.AIType1);
                item.m_aitype2 =GetListIntFromString(item.AIType2);
                item.m_aitype3 =GetListIntFromString(item.AIType3);

                item.m_addBuffList1 =GetListIntFromString(item.addBuffList1);
                item.m_addBuffList2 =GetListIntFromString(item.addBuffList2);
                item.m_addBuffList3 =GetListIntFromString(item.addBuffList3);
                item.m_removeBuffList1 =GetListIntFromString(item.removeBuffList1);
                item.m_removeBuffList2 =GetListIntFromString(item.removeBuffList2);
                item.m_removeBuffList3 =GetListIntFromString(item.removeBuffList3);

                item.m_attackSkills1 =GetListIntFromLongString(item.listAttack1,true);
                item.m_weightAttackSkills1 =GetListIntFromLongString(item.listAttack1,false);
                item.m_attackSkills2 =GetListIntFromLongString(item.listAttack2,true);
                item.m_weightAttackSkills2 =GetListIntFromLongString(item.listAttack2,false);
                item.m_attackSkills3 =GetListIntFromLongString(item.listAttack3,true);
                item.m_weightAttackSkills3 =GetListIntFromLongString(item.listAttack3,false);

                item.m_defendSkills1 =GetListIntFromLongString(item.listDefend1,true);
                item.m_weightDefendSkills1 =GetListIntFromLongString(item.listDefend1,false);
                item.m_defendSkills2 =GetListIntFromLongString(item.listDefend2,true);
                item.m_weightDefendSkills2 =GetListIntFromLongString(item.listDefend2,false);
                item.m_defendSkills3 =GetListIntFromLongString(item.listDefend3,true);
                item.m_weightDefendSkills3 =GetListIntFromLongString(item.listDefend3,false);

                item.m_buffSkills1 =GetListIntFromLongString(item.listBuff1,true);
                item.m_weightBuffSkills1 =GetListIntFromLongString(item.listBuff1,false);
                item.m_buffSkills2 =GetListIntFromLongString(item.listBuff2,true);
                item.m_weightBuffSkills1 =GetListIntFromLongString(item.listBuff2,false);
                item.m_buffSkills3 =GetListIntFromLongString(item.listBuff3,true);
                item.m_weightBuffSkills1 =GetListIntFromLongString(item.listBuff3,false);

                item.m_nerfSkills1 =GetListIntFromLongString(item.listNerf1,true);
                item.m_weightNerfSkills1 =GetListIntFromLongString(item.listNerf1,false);
                item.m_nerfSkills2 =GetListIntFromLongString(item.listNerf2,true);
                item.m_weightNerfSkills2 =GetListIntFromLongString(item.listNerf2,false);
                item.m_nerfSkills3 =GetListIntFromLongString(item.listNerf3,true);
                item.m_weightNerfSkills3 =GetListIntFromLongString(item.listNerf3,false);
               
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
        


        public static void CreateSkillArray(string filePath) {
            //获得表数据
            int columnNum = 0, rowNum = 0;
            DataRowCollection collect = ReadExcel(filePath, ref columnNum, ref rowNum);
 
            //根据excel的定义，第二行开始才是数据
            SkillData[] array = new SkillData[rowNum - 2];
            for(int i = 2; i < rowNum; i++) {
                SkillData skillData = new SkillData();
                //解析每列的数据
                skillData.id = collect[i][0].ToString()==""?0:int.Parse(collect[i][0].ToString());
                skillData.name = collect[i][1].ToString();
                skillData.describe = collect[i][2].ToString();
                skillData.icon = collect[i][3].ToString();
                skillData.spellEffect = collect[i][4].ToString();
                skillData.castEffect = collect[i][5].ToString();
                skillData.hitEffect = collect[i][6].ToString();

                skillData.spelllTime =collect[i][7].ToString()==""?0: float.Parse(collect[i][7].ToString());
                skillData.buildID = collect[i][8].ToString()==""?0:int.Parse(collect[i][8].ToString());
                
                skillData.ifActive = collect[i][9].ToString()=="1"?true:false;
                skillData.type =collect[i][10].ToString()==""?0: int.Parse(collect[i][10].ToString());
                skillData.heal =collect[i][11].ToString()==""?0: int.Parse(collect[i][11].ToString());

                skillData.manaCost =collect[i][12].ToString()==""?0: int.Parse(collect[i][12].ToString());
                skillData.damage =collect[i][13].ToString()==""?0: int.Parse(collect[i][13].ToString());
                skillData.addArmor =collect[i][14].ToString()==""?0: int.Parse(collect[i][14].ToString());
                skillData.damagePercent =collect[i][15].ToString()==""?0:float.Parse(collect[i][15].ToString());
                skillData.damageDistribution =collect[i][16].ToString();

                skillData.targetSelf =collect[i][17].ToString()=="1"?true:false;
                
                skillData.manaProduce =collect[i][18].ToString()==""?0: float.Parse(collect[i][18].ToString());

                skillData.buffID = collect[i][19].ToString()==""?0:int.Parse(collect[i][19].ToString());
                skillData.buffTarget = collect[i][20].ToString()=="1"?true:false;
                skillData.buffNum = collect[i][21].ToString()==""?0:int.Parse(collect[i][21].ToString());
                
                skillData.usedToRemove = collect[i][22].ToString()=="1"?true:false;
                skillData.usedChooseCard =collect[i][23].ToString()==""?0:int.Parse(collect[i][23].ToString());
                skillData.usedThrowCard =collect[i][24].ToString()==""?0:int.Parse(collect[i][24].ToString());
                skillData.updateID =collect[i][25].ToString()==""?0:int.Parse(collect[i][25].ToString());
                skillData.ifSeep =collect[i][26].ToString()=="1"?true:false;
                skillData.rank =collect[i][27].ToString()==""?0:int.Parse(collect[i][27].ToString());
                skillData.summonType =collect[i][28].ToString()==""?0:int.Parse(collect[i][28].ToString());
                skillData.summonNum =collect[i][29].ToString()==""?0:int.Parse(collect[i][29].ToString());
                skillData.EUSDamage =collect[i][30].ToString()==""?0:int.Parse(collect[i][30].ToString());
                skillData.EUSMP =collect[i][31].ToString()==""?0:int.Parse(collect[i][31].ToString());
                skillData.EUSHeal =collect[i][32].ToString()==""?0:int.Parse(collect[i][32].ToString());
                skillData.EUSArmor =collect[i][33].ToString()==""?0:int.Parse(collect[i][33].ToString());
                
                skillData.ELCDamage =collect[i][34].ToString()==""?0:int.Parse(collect[i][34].ToString());
                skillData.ELCMP =collect[i][35].ToString()==""?0:int.Parse(collect[i][35].ToString());
                skillData.ELCHeal =collect[i][36].ToString()==""?0:int.Parse(collect[i][36].ToString());
                skillData.ELCArmor =collect[i][37].ToString()==""?0:int.Parse(collect[i][37].ToString());

                skillData.exCrit =collect[i][38].ToString()==""?0:float.Parse(collect[i][38].ToString());
                skillData.checkBuff =collect[i][39].ToString()==""?0:int.Parse(collect[i][39].ToString());
                skillData.checkSelf = collect[i][40].ToString()=="1"?true:false;
                skillData.buffNumLimit =collect[i][41].ToString()==""?0:int.Parse(collect[i][41].ToString());
                skillData.CBDamage =collect[i][42].ToString()==""?0:int.Parse(collect[i][42].ToString());
                skillData.CBHeal =collect[i][43].ToString()==""?0:int.Parse(collect[i][43].ToString());
                skillData.CBArmor =collect[i][44].ToString()==""?0:int.Parse(collect[i][44].ToString());
                
                skillData.CBSeep = collect[i][45].ToString()=="1"?true:false;
                skillData.CBCrit =collect[i][46].ToString()==""?0:float.Parse(collect[i][46].ToString());
                skillData.CBManaCost =collect[i][47].ToString()==""?0:int.Parse(collect[i][47].ToString());
                skillData.CBmanaProduce =collect[i][48].ToString()==""?0:float.Parse(collect[i][48].ToString());
                skillData.CBBuff =collect[i][49].ToString()==""?0:int.Parse(collect[i][49].ToString());
                skillData.CBBuffTarget = collect[i][50].ToString()=="1"?true:false;

                skillData.CBBuffNum =collect[i][51].ToString()==""?0:int.Parse(collect[i][51].ToString());
                skillData.createCardNum =collect[i][52].ToString()==""?0:int.Parse(collect[i][52].ToString());
                skillData.createCardID =collect[i][53].ToString()==""?0:int.Parse(collect[i][53].ToString());
                skillData.createCardChar =collect[i][54].ToString()==""?0:int.Parse(collect[i][54].ToString());
                skillData.createCardType =collect[i][55].ToString()==""?0:int.Parse(collect[i][55].ToString());
                skillData.keepManaCost =collect[i][56].ToString()==""?0:int.Parse(collect[i][56].ToString());
                skillData.critTriggerSkill =collect[i][57].ToString()==""?0:int.Parse(collect[i][57].ToString());
                skillData.delaySpell =collect[i][58].ToString()==""?0:float.Parse(collect[i][58].ToString());
                skillData.protectSpell = collect[i][59].ToString()=="1"?true:false;
                skillData.initialUnlock = collect[i][60].ToString()=="1"?true:false;



                array[i - 2] = skillData;
            }
              skillDataArray =array;
            //   Debug.Log("skillDataArray:"+skillDataArray[0].name);
        }
        public static void CreateReformArray(string filePath) 
        {
            //获得表数据
            int columnNum = 0, rowNum = 0;
            DataRowCollection collect = ReadExcel(filePath, ref columnNum, ref rowNum);
 
            //根据excel的定义，第二行开始才是数据
            ReformData[] array = new ReformData[rowNum - 2];
            for(int i = 2; i < rowNum; i++) {
            ReformData data = new ReformData();
            //解析每列的数据
            data.id = collect[i][0].ToString()==""?0:int.Parse(collect[i][0].ToString());
            data.name = collect[i][1].ToString();
            data.describe = collect[i][2].ToString();
            data.icon = collect[i][3].ToString();
            data.price =collect[i][4].ToString()==""?0:int.Parse(collect[i][4].ToString());
            data.rewardPool =collect[i][5].ToString()==""?0:int.Parse(collect[i][5].ToString());
            data.perfect =collect[i][6].ToString();
            data.common =collect[i][7].ToString();
            data.fail =collect[i][8].ToString();
            data.percentP =collect[i][9].ToString()==""?0:float.Parse(collect[i][9].ToString());
            data.percentC =collect[i][10].ToString()==""?0:float.Parse(collect[i][10].ToString());
            data.percentF =collect[i][11].ToString()==""?0:float.Parse(collect[i][11].ToString());



            array[i - 2] = data;
            }
            reformDatas =array;
        }
        public static void CreateAbilityArray(string filePath) {
            //获得表数据
            int columnNum = 0, rowNum = 0;
            DataRowCollection collect = ReadExcel(filePath, ref columnNum, ref rowNum);
 
            //根据excel的定义，第二行开始才是数据
            AbilityData[] array = new AbilityData[rowNum - 2];
            for(int i = 2; i < rowNum; i++) {
            AbilityData data = new AbilityData();
            //解析每列的数据
            data.id = collect[i][0].ToString()==""?0:int.Parse(collect[i][0].ToString());
            data.name = collect[i][1].ToString();
            data.describe = collect[i][2].ToString();
            data.icon = collect[i][3].ToString();
            data.price =collect[i][4].ToString()==""?0:int.Parse(collect[i][4].ToString());
            data.buffID =collect[i][5].ToString()==""?0:int.Parse(collect[i][5].ToString());
            data.targetSelf =collect[i][6].ToString()==""?false:true;
            data.buffNum =collect[i][7].ToString()==""?0:int.Parse(collect[i][7].ToString());
            data.rank =collect[i][8].ToString()==""?0:int.Parse(collect[i][8].ToString());
            data.buildIDList =collect[i][9].ToString();
            data.attack =collect[i][10].ToString()==""?0:int.Parse(collect[i][10].ToString());
            data.defence =collect[i][11].ToString()==""?0:int.Parse(collect[i][11].ToString());
            data.hpMax =collect[i][12].ToString()==""?0:int.Parse(collect[i][12].ToString());
            data.mpMax =collect[i][13].ToString()==""?0:int.Parse(collect[i][13].ToString());
            data.reMp =collect[i][14].ToString()==""?0:float.Parse(collect[i][14].ToString());
            data.crit =collect[i][15].ToString()==""?0:float.Parse(collect[i][15].ToString());
            data.initialUnlock =collect[i][16].ToString()==""?false:true;

            data._buildList = GetListIntFromString(data.buildIDList);
            array[i - 2] = data;
            }
            abilityDatas =array;
            // Debug.Log("abilityDatas="+abilityDatas.Length);
        }
        public static void CreateSummonArray(string filePath) {
            //获得表数据
            int columnNum = 0, rowNum = 0;
            DataRowCollection collect = ReadExcel(filePath, ref columnNum, ref rowNum);
 
            //根据excel的定义，第二行开始才是数据
            SummonData[] array = new SummonData[rowNum - 2];
            for(int i = 2; i < rowNum; i++) {
            SummonData data = new SummonData();
            //解析每列的数据
            data.id = collect[i][0].ToString()==""?0:int.Parse(collect[i][0].ToString());
            data.name = collect[i][1].ToString();
            data.prefab = collect[i][2].ToString();

            data.lifeTime = collect[i][3].ToString()==""?0:float.Parse(collect[i][3].ToString());
            data.power = collect[i][4].ToString()==""?0:int.Parse(collect[i][4].ToString());
            data.speed =collect[i][5].ToString()==""?0:float.Parse(collect[i][5].ToString());
            data.skill = collect[i][6].ToString()==""?0:int.Parse(collect[i][6].ToString());

            array[i - 2] = data;
            }
            summonDatas =array;
        }
        public static void CreateShopArray(string filePath) {
            //获得表数据
            int columnNum = 0, rowNum = 0;
            DataRowCollection collect = ReadExcel(filePath, ref columnNum, ref rowNum);
 
            //根据excel的定义，第二行开始才是数据
            ShopData[] array = new ShopData[rowNum - 2];
            for(int i = 2; i < rowNum; i++) {
            ShopData data = new ShopData();
            //解析每列的数据
            data.id = collect[i][0].ToString()==""?0:int.Parse(collect[i][0].ToString());
            data.SellCardList = collect[i][1].ToString();
            data.SellRelicList = collect[i][2].ToString();

            data._sellCardList =GetListIntFromString(data.SellCardList); 
            data._sellRelicList =GetListIntFromString(data.SellRelicList); 

            array[i - 2] = data;
            }
            shopDatas =array;
        }
        public static DropGroup[] GetItemArray()
        {
            return droupGroupArray;
        }
        public static SkillData[] GetSkillArray()
        {
            return skillDataArray;
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
        public static AbilityData[] GetAbilityDataArray()
        {
            return abilityDatas;
        }
        public static IdealData[] GetIdealArray()
        {
            return idealDatas;
        }
        public static RelicGroupData[] GetRelicGroupArray()
        {
            return relicGroupDatas;
        }
        public static ReformData[] GetReformArray()
        {
            return reformDatas;
        }
        public static SummonData[] GetSummonDataArray()
        {
            return summonDatas;
        }
        public static RandomEvent[] GetRandomEventArray()
        {
            return randomEventArray;
        }
        public static ShopData[] GetShopDataArray()
        {
            return shopDatas;
        }
        public static LevelData[] GetLevelArray()
        {
            return levelDatas;
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

