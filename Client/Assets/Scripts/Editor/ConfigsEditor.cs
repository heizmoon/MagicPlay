using UnityEditor;
using UnityEngine;


[CustomEditor(typeof(Configs))]
public class ConfigsEditor : Editor {

    public override void OnInspectorGUI() {
        Configs script = (Configs)target;

        // 重绘GUI
        EditorGUI.BeginChangeCheck();

        // 公开属性
        drawProperty("initGold", "初始金钱");
        drawProperty("battleLevelGold", "战斗胜利获得金钱系数");
        drawProperty("priceRankGold", "卡牌价格与卡牌级别相关系数");
        drawProperty("cardRank1", "最小rank1步数");
        drawProperty("cardRank2", "最小rank2步数");
        drawProperty("cardRank3", "最小rank3步数");
        drawProperty("cardRank4", "最小rank4步数");
        drawProperty("campRestoreHP", "营地回血百分比(小数)");
        drawProperty("executeRestoreHP", "斩杀回血数值");
        drawProperty("eneryCubeRestoreMP", "能量块回复能量数值");
        drawProperty("levelUpAddTalentPoint", "每级增加天赋点数");
        drawProperty("everyStepAddEXP", "每步增加多少经验值");
        drawProperty("cardWeightAddition", "获得卡牌时每张现有牌的权重加成");
        drawProperty("buildNumber", "全部流派数量");
        drawProperty("ifChangMode", "是否采用换牌模式");
        drawProperty("removeCardGold", "每次移除卡牌金钱增加");
        drawProperty("toolTips", "小提示");
        drawProperty("shopRestoreCost", "商店治疗费用");



        // 只读属性
        // GUI.enabled = false;
        // drawProperty("currentTime", "当前时间(秒)");
        // drawProperty("currentCount", "当前次数");
        // GUI.enabled = true;

        // // 回调事件
        // drawProperty("onIntervalEvent", "计时间隔事件");
        // drawProperty("onCompleteEvent", "计时完成事件");
        if (EditorGUI.EndChangeCheck()) serializedObject.ApplyModifiedProperties();
    }

    private void drawProperty(string property, string label) {
        EditorGUILayout.PropertyField(serializedObject.FindProperty(property), new GUIContent(label), true);
    }

}