using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Data;
using System.IO;
using System.Threading.Tasks;

namespace EditorTool {
    
    public class CharacterBuild : Editor {
 
        [MenuItem("生成表格数据/Character表")]
        public static void CreateItemAsset() {
            CharacterDataSet manager = ScriptableObject.CreateInstance<CharacterDataSet>();
            //赋值
            ExcelTool.CreateItemArrayWithExcel(ExcelConfig.excelsFolderPath + "Character.xlsx","Character");
            manager.dataArray = ExcelTool.GetCharacterArray();
            //确保文件夹存在
            if(!Directory.Exists(ExcelConfig.assetPath)) {
                Directory.CreateDirectory(ExcelConfig.assetPath);
            }
 
            //asset文件的路径 要以"Assets/..."开始，否则CreateAsset会报错
            string assetPath = string.Format("{0}{1}.asset", ExcelConfig.assetPath, "Character");
            //生成一个Asset文件
            AssetDatabase.CreateAsset(manager, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
    
    public class SkillBuild : Editor {
 
        [MenuItem("生成表格数据/Skill表")]
        public static void CreateItemAsset() {
            SkillDataSet manager = ScriptableObject.CreateInstance<SkillDataSet>();
            //赋值
            ExcelTool.CreateItemArrayWithExcel(ExcelConfig.excelsFolderPath + "Skill.xlsx","Skill");
            manager.dataArray = ExcelTool.GetSkillArray();
            //确保文件夹存在
            if(!Directory.Exists(ExcelConfig.assetPath)) {
                Directory.CreateDirectory(ExcelConfig.assetPath);
            }
 
            //asset文件的路径 要以"Assets/..."开始，否则CreateAsset会报错
            string assetPath = string.Format("{0}{1}.asset", ExcelConfig.assetPath, "Skill");
            //生成一个Asset文件
            AssetDatabase.CreateAsset(manager, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
    public class DropGroupBuild : Editor {
 
        [MenuItem("生成表格数据/DropGroup表")]
        public static void CreateItemAsset() {
            DropGroupSet manager = ScriptableObject.CreateInstance<DropGroupSet>();
            //赋值
            ExcelTool.CreateItemArrayWithExcel(ExcelConfig.excelsFolderPath + "DropGroup.xlsx","DropGroup");
            manager.dataArray = ExcelTool.GetItemArray();
            //确保文件夹存在
            if(!Directory.Exists(ExcelConfig.assetPath)) {
                Directory.CreateDirectory(ExcelConfig.assetPath);
            }
 
            //asset文件的路径 要以"Assets/..."开始，否则CreateAsset会报错
            string assetPath = string.Format("{0}{1}.asset", ExcelConfig.assetPath, "DropGroup");
            //生成一个Asset文件
            AssetDatabase.CreateAsset(manager, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
    public class DialogueBuild : Editor 
    {
        static DialogueDataSet manager;
        [MenuItem("生成表格数据/对话表")]
        public static void CreateItemAsset()
        {
            DirectoryInfo folder = new DirectoryInfo(Application.dataPath +"/Data/Dialogue/");
            foreach (FileInfo file in folder.GetFiles("*.xlsx"))
            {
                Debug.LogFormat("Dialog--文件名:{0}",file.Name);
                CreateOneByOne(file.Name.Split('.')[0]);
            }   
        }
        static System.Func<Task> func = async () =>
        {
            await Task.Delay(System.TimeSpan.FromSeconds(1));
            //需要延迟执行的方法体...
            
        };
        
        public static void CreateOneByOne(string dialogueName)
        {
            //赋值
            manager = ScriptableObject.CreateInstance<DialogueDataSet>();
            ExcelTool.CreateItemArrayWithExcel("Assets/Data/Dialogue/" +dialogueName+ ".xlsx","Dialogue");
            manager.dataArray = ExcelTool.GetDiagueDataArray();
            //确保文件夹存在
            if(!Directory.Exists(ExcelConfig.assetPath)) {
                Directory.CreateDirectory(ExcelConfig.assetPath);
            }
 
            //asset文件的路径 要以"Assets/..."开始，否则CreateAsset会报错
            string assetPath = string.Format("{0}{1}.asset", ExcelConfig.assetPath+"Dialogue/", dialogueName);
            AssetDatabase.CreateAsset(manager, assetPath);            
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
    public class QuestionDataBuild : Editor {
 
        [MenuItem("生成表格数据/题库表")]
        public static void CreateItemAsset() {
            QuestionDataSet manager = ScriptableObject.CreateInstance<QuestionDataSet>();
            //赋值
            ExcelTool.CreateItemArrayWithExcel(ExcelConfig.excelsFolderPath + "Question.xlsx","Question");
            manager.dataArray = ExcelTool.GetQuestionArray();
            //确保文件夹存在
            if(!Directory.Exists(ExcelConfig.assetPath)) {
                Directory.CreateDirectory(ExcelConfig.assetPath);
            }
 
            //asset文件的路径 要以"Assets/..."开始，否则CreateAsset会报错
            string assetPath = string.Format("{0}{1}.asset", ExcelConfig.assetPath, "Question");
            //生成一个Asset文件
            AssetDatabase.CreateAsset(manager, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
    public class QuestionDatabaseBuild : Editor {
 
        [MenuItem("生成表格数据/题库索引表")]
        public static void CreateItemAsset() {
            QuestionDatabaseSet manager = ScriptableObject.CreateInstance<QuestionDatabaseSet>();
            //赋值
            ExcelTool.CreateItemArrayWithExcel(ExcelConfig.excelsFolderPath + "QuestionDatabase.xlsx","QuestionDatabase");
            manager.dataArray = ExcelTool.GetQuestionDatabaseArray();
            //确保文件夹存在
            if(!Directory.Exists(ExcelConfig.assetPath)) {
                Directory.CreateDirectory(ExcelConfig.assetPath);
            }
 
            //asset文件的路径 要以"Assets/..."开始，否则CreateAsset会报错
            string assetPath = string.Format("{0}{1}.asset", ExcelConfig.assetPath, "QuestionDatabase");
            //生成一个Asset文件
            AssetDatabase.CreateAsset(manager, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
    public class MonsterTypeBuild : Editor 
    {
 
        [MenuItem("生成表格数据/MonsterType表")]
        public static void CreateItemAsset() 
        {
            MonsterTypeSet manager = ScriptableObject.CreateInstance<MonsterTypeSet>();
            //赋值
            ExcelTool.CreateItemArrayWithExcel(ExcelConfig.excelsFolderPath + "MonsterType.xlsx","MonsterType");
            manager.dataArray = ExcelTool.GetMonsterTypeArray();
            //确保文件夹存在
            if(!Directory.Exists(ExcelConfig.assetPath)) {
                Directory.CreateDirectory(ExcelConfig.assetPath);
            }
 
            //asset文件的路径 要以"Assets/..."开始，否则CreateAsset会报错
            string assetPath = string.Format("{0}{1}.asset", ExcelConfig.assetPath, "MonsterType");
            //生成一个Asset文件
            AssetDatabase.CreateAsset(manager, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
    public class MonsterGroupBuild : Editor 
    {
 
        [MenuItem("生成表格数据/MonsterGroup表")]
        public static void CreateItemAsset() 
        {
            MonsterGroupSet manager = ScriptableObject.CreateInstance<MonsterGroupSet>();
            //赋值
            ExcelTool.CreateItemArrayWithExcel(ExcelConfig.excelsFolderPath + "MonsterGroup.xlsx","MonsterGroup");
            manager.dataArray = ExcelTool.GetMonsterGroupArray();
            //确保文件夹存在
            if(!Directory.Exists(ExcelConfig.assetPath)) {
                Directory.CreateDirectory(ExcelConfig.assetPath);
            }
 
            //asset文件的路径 要以"Assets/..."开始，否则CreateAsset会报错
            string assetPath = string.Format("{0}{1}.asset", ExcelConfig.assetPath, "MonsterGroup");
            //生成一个Asset文件
            AssetDatabase.CreateAsset(manager, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
    public class TraitBuild : Editor 
    {
 
        [MenuItem("生成表格数据/Trait表")]
        public static void CreateItemAsset() 
        {
            TraitDataSet manager = ScriptableObject.CreateInstance<TraitDataSet>();
            //赋值
            ExcelTool.CreateItemArrayWithExcel(ExcelConfig.excelsFolderPath + "Trait.xlsx","Trait");
            manager.dataArray = ExcelTool.GetTraitArray();
            //确保文件夹存在
            if(!Directory.Exists(ExcelConfig.assetPath)) {
                Directory.CreateDirectory(ExcelConfig.assetPath);
            }
 
            //asset文件的路径 要以"Assets/..."开始，否则CreateAsset会报错
            string assetPath = string.Format("{0}{1}.asset", ExcelConfig.assetPath, "Trait");
            //生成一个Asset文件
            AssetDatabase.CreateAsset(manager, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
    public class RelicBuild : Editor 
    {
 
        [MenuItem("生成表格数据/Relic表")]
        public static void CreateItemAsset() 
        {
            RelicDataSet manager = ScriptableObject.CreateInstance<RelicDataSet>();
            //赋值
            ExcelTool.CreateItemArrayWithExcel(ExcelConfig.excelsFolderPath + "Relic.xlsx","Relic");
            manager.dataArray = ExcelTool.GetRelicArray();
            //确保文件夹存在
            if(!Directory.Exists(ExcelConfig.assetPath)) {
                Directory.CreateDirectory(ExcelConfig.assetPath);
            }
 
            //asset文件的路径 要以"Assets/..."开始，否则CreateAsset会报错
            string assetPath = string.Format("{0}{1}.asset", ExcelConfig.assetPath, "Relic");
            //生成一个Asset文件
            AssetDatabase.CreateAsset(manager, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
    public class TaskEventsBuild : Editor 
    {
 
        [MenuItem("生成表格数据/TaskEvent表")]
        public static void CreateItemAsset() 
        {
            TaskEventDataSet manager = ScriptableObject.CreateInstance<TaskEventDataSet>();
            //赋值
            ExcelTool.CreateItemArrayWithExcel(ExcelConfig.excelsFolderPath + "TaskEvent.xlsx","TaskEvent");
            manager.dataArray = ExcelTool.GetTaskEventsArray();
            //确保文件夹存在
            if(!Directory.Exists(ExcelConfig.assetPath)) {
                Directory.CreateDirectory(ExcelConfig.assetPath);
            }
 
            //asset文件的路径 要以"Assets/..."开始，否则CreateAsset会报错
            string assetPath = string.Format("{0}{1}.asset", ExcelConfig.assetPath, "TaskEvent");
            //生成一个Asset文件
            AssetDatabase.CreateAsset(manager, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
    public class TriggerEventBuild : Editor 
    {
 
        [MenuItem("生成表格数据/TriggerEvent表")]
        public static void CreateItemAsset() 
        {
            TriggerEventDataSet manager = ScriptableObject.CreateInstance<TriggerEventDataSet>();
            //赋值
            ExcelTool.CreateItemArrayWithExcel(ExcelConfig.excelsFolderPath + "TriggerEvent.xlsx","TriggerEvent");
            manager.dataArray = ExcelTool.GetTriggerEventsArray();
            //确保文件夹存在
            if(!Directory.Exists(ExcelConfig.assetPath)) {
                Directory.CreateDirectory(ExcelConfig.assetPath);
            }
 
            //asset文件的路径 要以"Assets/..."开始，否则CreateAsset会报错
            string assetPath = string.Format("{0}{1}.asset", ExcelConfig.assetPath, "TriggerEvent");
            //生成一个Asset文件
            AssetDatabase.CreateAsset(manager, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
    public class SceneBuild : Editor 
    {
 
        [MenuItem("生成表格数据/Scene表")]
        public static void CreateItemAsset() 
        {
            SceneDataSet manager = ScriptableObject.CreateInstance<SceneDataSet>();
            //赋值
            ExcelTool.CreateItemArrayWithExcel(ExcelConfig.excelsFolderPath + "Scene.xlsx","Scene");
            manager.dataArray = ExcelTool.GetSceneArray();
            //确保文件夹存在
            if(!Directory.Exists(ExcelConfig.assetPath)) {
                Directory.CreateDirectory(ExcelConfig.assetPath);
            }
 
            //asset文件的路径 要以"Assets/..."开始，否则CreateAsset会报错
            string assetPath = string.Format("{0}{1}.asset", ExcelConfig.assetPath, "Scene");
            //生成一个Asset文件
            AssetDatabase.CreateAsset(manager, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
    public class GuildBuild : Editor 
    {
 
        [MenuItem("生成表格数据/势力表")]
        public static void CreateItemAsset() 
        {
            GuildDataSet manager = ScriptableObject.CreateInstance<GuildDataSet>();
            //赋值
            ExcelTool.CreateItemArrayWithExcel(ExcelConfig.excelsFolderPath + "Guild.xlsx","Guild");
            manager.dataArray = ExcelTool.GetGuildDataArray();
            //确保文件夹存在
            if(!Directory.Exists(ExcelConfig.assetPath)) {
                Directory.CreateDirectory(ExcelConfig.assetPath);
            }
 
            //asset文件的路径 要以"Assets/..."开始，否则CreateAsset会报错
            string assetPath = string.Format("{0}{1}.asset", ExcelConfig.assetPath, "Guild");
            //生成一个Asset文件
            AssetDatabase.CreateAsset(manager, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
    public class BuffBuild : Editor 
    {
 
        [MenuItem("生成表格数据/buff表")]
        public static void CreateItemAsset() 
        {
            BuffDataSet manager = ScriptableObject.CreateInstance<BuffDataSet>();
            //赋值
            ExcelTool.CreateItemArrayWithExcel(ExcelConfig.excelsFolderPath + "Buff.xlsx","Buff");
            manager.dataArray = ExcelTool.GetBuffDataArray();
            //确保文件夹存在
            if(!Directory.Exists(ExcelConfig.assetPath)) {
                Directory.CreateDirectory(ExcelConfig.assetPath);
            }
 
            //asset文件的路径 要以"Assets/..."开始，否则CreateAsset会报错
            string assetPath = string.Format("{0}{1}.asset", ExcelConfig.assetPath, "Buff");
            //生成一个Asset文件
            AssetDatabase.CreateAsset(manager, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
    public class AbilityBuild : Editor 
    {
 
        [MenuItem("生成表格数据/Ability表")]
        public static void CreateItemAsset() 
        {
            // Debug.Log("什么问题？");
            AbilityDataSet manager = ScriptableObject.CreateInstance<AbilityDataSet>();
            //赋值
            ExcelTool.CreateItemArrayWithExcel(ExcelConfig.excelsFolderPath + "Ability.xlsx","Ability");
            manager.dataArray = ExcelTool.GetAbilityDataArray();
            // Debug.Log("长度？"+manager.dataArray.Length);

            //确保文件夹存在
            if(!Directory.Exists(ExcelConfig.assetPath)) {
                Directory.CreateDirectory(ExcelConfig.assetPath);
            }
 
            //asset文件的路径 要以"Assets/..."开始，否则CreateAsset会报错
            string assetPath = string.Format("{0}{1}.asset", ExcelConfig.assetPath, "Ability");
            //生成一个Asset文件
            AssetDatabase.CreateAsset(manager, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
    public class SummonBuild : Editor 
    {
 
        [MenuItem("生成表格数据/Summon表")]
        public static void CreateItemAsset() 
        {
            // Debug.Log("什么问题？");
            SummonDataSet manager = ScriptableObject.CreateInstance<SummonDataSet>();
            //赋值
            ExcelTool.CreateItemArrayWithExcel(ExcelConfig.excelsFolderPath + "Summon.xlsx","Summon");
            manager.dataArray = ExcelTool.GetSummonDataArray();
            // Debug.Log("长度？"+manager.dataArray.Length);

            //确保文件夹存在
            if(!Directory.Exists(ExcelConfig.assetPath)) {
                Directory.CreateDirectory(ExcelConfig.assetPath);
            }
 
            //asset文件的路径 要以"Assets/..."开始，否则CreateAsset会报错
            string assetPath = string.Format("{0}{1}.asset", ExcelConfig.assetPath, "Summon");
            //生成一个Asset文件
            AssetDatabase.CreateAsset(manager, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
    public class RandomEventBuild : Editor 
    {
 
        [MenuItem("生成表格数据/Summon表")]
        public static void CreateItemAsset() 
        {
            // Debug.Log("什么问题？");
            RandomEventDataSet manager = ScriptableObject.CreateInstance<RandomEventDataSet>();
            //赋值
            ExcelTool.CreateItemArrayWithExcel(ExcelConfig.excelsFolderPath + "RandomEvent.xlsx","RandomEvent");
            manager.dataArray = ExcelTool.GetRandomEventArray();
            // Debug.Log("长度？"+manager.dataArray.Length);

            //确保文件夹存在
            if(!Directory.Exists(ExcelConfig.assetPath)) {
                Directory.CreateDirectory(ExcelConfig.assetPath);
            }
 
            //asset文件的路径 要以"Assets/..."开始，否则CreateAsset会报错
            string assetPath = string.Format("{0}{1}.asset", ExcelConfig.assetPath, "RandomEvent");
            //生成一个Asset文件
            AssetDatabase.CreateAsset(manager, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
    public class IdealBuild : Editor 
    {
 
        [MenuItem("生成表格数据/理想表")]
        public static void CreateItemAsset() 
        {
            IdealDataSet manager = ScriptableObject.CreateInstance<IdealDataSet>();
            //赋值
            ExcelTool.CreateItemArrayWithExcel(ExcelConfig.excelsFolderPath + "Ideal.xlsx","Ideal");
            manager.dataArray = ExcelTool.GetIdealArray();
            //确保文件夹存在
            if(!Directory.Exists(ExcelConfig.assetPath)) {
                Directory.CreateDirectory(ExcelConfig.assetPath);
            }
 
            //asset文件的路径 要以"Assets/..."开始，否则CreateAsset会报错
            string assetPath = string.Format("{0}{1}.asset", ExcelConfig.assetPath, "Ideal");
            //生成一个Asset文件
            AssetDatabase.CreateAsset(manager, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
    public class RelicGroupBuild : Editor 
    {
 
        [MenuItem("生成表格数据/RelicGroup")]
        public static void CreateItemAsset() 
        {
            RelicGroupDataSet manager = ScriptableObject.CreateInstance<RelicGroupDataSet>();
            //赋值
            ExcelTool.CreateItemArrayWithExcel(ExcelConfig.excelsFolderPath + "RelicGroup.xlsx","RelicGroup");
            manager.dataArray = ExcelTool.GetRelicGroupArray();
            //确保文件夹存在
            if(!Directory.Exists(ExcelConfig.assetPath)) {
                Directory.CreateDirectory(ExcelConfig.assetPath);
            }
 
            //asset文件的路径 要以"Assets/..."开始，否则CreateAsset会报错
            string assetPath = string.Format("{0}{1}.asset", ExcelConfig.assetPath, "RelicGroup");
            //生成一个Asset文件
            AssetDatabase.CreateAsset(manager, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
    public class ReformDataBuild : Editor {
 
        [MenuItem("生成表格数据/Reform表")]
        public static void CreateItemAsset() {
            ReformDataSet manager = ScriptableObject.CreateInstance<ReformDataSet>();
            //赋值
            ExcelTool.CreateItemArrayWithExcel(ExcelConfig.excelsFolderPath + "Reform.xlsx","Reform");
            manager.dataArray = ExcelTool.GetReformArray();
            //确保文件夹存在
            if(!Directory.Exists(ExcelConfig.assetPath)) {
                Directory.CreateDirectory(ExcelConfig.assetPath);
            }
 
            //asset文件的路径 要以"Assets/..."开始，否则CreateAsset会报错
            string assetPath = string.Format("{0}{1}.asset", ExcelConfig.assetPath, "Reform");
            //生成一个Asset文件
            AssetDatabase.CreateAsset(manager, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
    public class ShopDataBuild : Editor {
 
        [MenuItem("生成表格数据/Shop表")]
        public static void CreateItemAsset() {
            ShopDataSet manager = ScriptableObject.CreateInstance<ShopDataSet>();
            //赋值
            ExcelTool.CreateItemArrayWithExcel(ExcelConfig.excelsFolderPath + "Shop.xlsx","ShopData");
            manager.dataArray = ExcelTool.GetShopDataArray();
            //确保文件夹存在
            if(!Directory.Exists(ExcelConfig.assetPath)) {
                Directory.CreateDirectory(ExcelConfig.assetPath);
            }
 
            //asset文件的路径 要以"Assets/..."开始，否则CreateAsset会报错
            string assetPath = string.Format("{0}{1}.asset", ExcelConfig.assetPath, "Shop");
            //生成一个Asset文件
            AssetDatabase.CreateAsset(manager, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
    public class LevelDataBuild : Editor {
 
        [MenuItem("生成表格数据/Level表")]
        public static void CreateItemAsset() {
            LevelDataSet manager = ScriptableObject.CreateInstance<LevelDataSet>();
            //赋值
            ExcelTool.CreateItemArrayWithExcel(ExcelConfig.excelsFolderPath + "Level.xlsx","LevelData");
            manager.dataArray = ExcelTool.GetLevelArray();
            //确保文件夹存在
            if(!Directory.Exists(ExcelConfig.assetPath)) {
                Directory.CreateDirectory(ExcelConfig.assetPath);
            }
 
            //asset文件的路径 要以"Assets/..."开始，否则CreateAsset会报错
            string assetPath = string.Format("{0}{1}.asset", ExcelConfig.assetPath, "Level");
            //生成一个Asset文件
            AssetDatabase.CreateAsset(manager, assetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
    public class AllBuild : Editor 
    {
 
        [MenuItem("生成表格数据/---所有表---")]
    public static void CreateAll()
    {
        Debug.Log("-----开始生成----");
        CharacterBuild.CreateItemAsset();
        SkillBuild.CreateItemAsset();
        DropGroupBuild.CreateItemAsset();
        QuestionDataBuild.CreateItemAsset();
        QuestionDatabaseBuild.CreateItemAsset();
        MonsterTypeBuild.CreateItemAsset();
        MonsterGroupBuild.CreateItemAsset();
        TraitBuild.CreateItemAsset();
        RelicBuild.CreateItemAsset();
        TaskEventsBuild.CreateItemAsset();
        TriggerEventBuild.CreateItemAsset();
        SceneBuild.CreateItemAsset();
        GuildBuild.CreateItemAsset();
        BuffBuild.CreateItemAsset();
        RelicGroupBuild.CreateItemAsset();
        ReformDataBuild.CreateItemAsset();
        AbilityBuild.CreateItemAsset();
        SummonBuild.CreateItemAsset();
        RandomEventBuild.CreateItemAsset();
        ShopDataBuild.CreateItemAsset();
        LevelDataBuild.CreateItemAsset();


        Debug.Log("-----所有表格生成完毕----");
    }
    }
    
}

