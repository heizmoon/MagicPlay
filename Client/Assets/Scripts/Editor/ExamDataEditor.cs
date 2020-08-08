using UnityEditor;
using UnityEngine;

/// <summary>
/// 考试数据
/// </summary>
[CanEditMultipleObjects]
[CustomEditor(typeof(ExamData))]
public class ExamDataEditor : Editor {

    public override void OnInspectorGUI() {
        ExamData script = (ExamData)target;

        // 重绘GUI
        EditorGUI.BeginChangeCheck();

        // 公开属性
        drawProperty("examName", "考试名称");
        drawProperty("Apoint", "分数线");
        // drawProperty("Bpoint", "及格分数线");
        drawProperty("totalGoal", "总分");
        drawProperty("valueIdentificationList", "数值验证项目");
        drawProperty("valueRequirementList", "项目数值要求");
        drawProperty("valueGoalList", "每项达到要求得几分");
        drawProperty("paperQuestionDatabase", "题库id");
        drawProperty("questionGoal", "答对一题得几分");
        drawProperty("questionNum", "一共几道题");

        drawProperty("goodContinue", "好结局");
        drawProperty("badContinue", "坏结局");


        
        

        
       
        // if (script.repeatCount <= 0) EditorGUILayout.LabelField(" ", "<=0 时无限重复", GUILayout.ExpandWidth(true));
        // 只读属性
        GUI.enabled = false;
        // drawProperty("currentTime", "当前时间(秒)");
        // drawProperty("currentCount", "当前次数");
        GUI.enabled = true;

        // 回调事件
        
        if (EditorGUI.EndChangeCheck()) serializedObject.ApplyModifiedProperties();
    }

    private void drawProperty(string property, string label) {
        EditorGUILayout.PropertyField(serializedObject.FindProperty(property), new GUIContent(label), true);
    }

}