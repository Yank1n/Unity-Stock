using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;
    using System.Linq;
#endif

[System.Serializable]
public class ObjectiveTrigger
{
    public Quest quest;
    public Quest.Status statusToApply;

    public int objectiveNumber;

    public void Invoke()
    {
        var manager = Object.FindObjectOfType<QuestManager>();

        manager.UpdateObjectiveStatus(
            quest,
            objectiveNumber,
            statusToApply
            );
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ObjectiveTrigger))]
public class ObjectiveTriggerDrawer : PropertyDrawer
{
    public override void OnGUI(
        Rect position,
        SerializedProperty property,
        GUIContent label
        )
    {
        EditorGUI.BeginProperty(position, label, property);

        var questProperty = property.FindPropertyRelative("quest");
        var statusProperty = property.FindPropertyRelative("statusToApply");
        var objectiveNumberProperty = property.FindPropertyRelative("objectiveNumber");

        var lineSpacing = 2;

        var firstLinePosition = position;
        firstLinePosition.height = base.GetPropertyHeight(questProperty, label);

        var secondLinePosition = position;
        secondLinePosition.y = firstLinePosition.y + firstLinePosition.height + lineSpacing;
        secondLinePosition.height = base.GetPropertyHeight(statusProperty, label);

        var thirdLinePosition = position;
        thirdLinePosition.y = secondLinePosition.y + secondLinePosition.height + lineSpacing;
        thirdLinePosition.height = base.GetPropertyHeight(objectiveNumberProperty, label);

        EditorGUI.PropertyField(firstLinePosition, questProperty, new GUIContent("Quest"));
        EditorGUI.PropertyField(secondLinePosition, statusProperty, new GUIContent("Status"));

        thirdLinePosition = EditorGUI.PrefixLabel(thirdLinePosition, new GUIContent("Objective"));

        var quest = questProperty.objectReferenceValue as Quest;

        if (quest != null && quest.objectives.Count > 0)
        {
            var objectiveNames = quest.objectives.Select(o => o.name).ToArray();

            var selectedObjective = objectiveNumberProperty.intValue;

            if (selectedObjective >= quest.objectives.Count)
            {
                selectedObjective = 0;
            }

            var newSelectedObjective = EditorGUI.Popup(
                thirdLinePosition,
                selectedObjective,
                objectiveNames
                );

            if (newSelectedObjective != selectedObjective)
            {
                objectiveNumberProperty.intValue = newSelectedObjective;
            }
        }
        else
        {
            using (new EditorGUI.DisabledGroupScope(true))
            {
                EditorGUI.Popup(thirdLinePosition, 0, new[] { "-" });
            }
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(
        SerializedProperty property,
        GUIContent label)
    {
        var lineCount = 3;
        var lineSpacing = 2;
        var lineHeight = base.GetPropertyHeight(property, label);

        return (lineHeight * lineCount) + (lineSpacing * (lineCount - 1));
    }
}
#endif