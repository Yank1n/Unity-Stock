using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;
#endif

[CreateAssetMenu(fileName = "New Quest", menuName = "Quest", order = 100)]
public class Quest : MonoBehaviour
{
    public enum Status
    {
        NotYetComplete,
        Complete,
        Failed
    }

    public string questName;
    public List<Objective> objectives;

    [System.Serializable]
    public class Objective
    {
        public string name = "New Objective";
        public bool optional = false;
        public bool visible = true;
        public Status initialStatus = Status.NotYetComplete;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Quest))]
public class QuestEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(
            serializedObject.FindProperty("questName"),
            new GUIContent("Name")
            );

        EditorGUILayout.LabelField("Objectives");

        var objectiveList = serializedObject.FindProperty("objectives");

        EditorGUI.indentLevel += 1;

        for (int i = 0, size = objectiveList.arraySize; i < size; ++i)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(
                objectiveList.GetArrayElementAtIndex(i),
                includeChildren: true
                );

            if (GUILayout.Button("Up", EditorStyles.miniButtonLeft, GUILayout.Width(25)))
            {
                objectiveList.MoveArrayElement(i, i - 1);
            }

            if (GUILayout.Button("Down", EditorStyles.miniButtonMid, GUILayout.Width(40)))
            {
                objectiveList.MoveArrayElement(i, i + 1);
            }

            if (GUILayout.Button("-", EditorStyles.miniButtonRight, GUILayout.Width(25)))
            {
                objectiveList.DeleteArrayElementAtIndex(i);
            }

            EditorGUILayout.EndHorizontal();
        }

        EditorGUI.indentLevel -= 1;

        if (GUILayout.Button("Add Objective"))
        {
            objectiveList.arraySize += 1;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif