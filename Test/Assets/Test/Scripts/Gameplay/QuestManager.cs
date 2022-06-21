using System.Collections.Generic;
using UnityEngine;

public class QuestStatus
{
    public Quest questData;

    public Dictionary<int, Quest.Status> objectiveStatuses;

    public QuestStatus(Quest questData)
    {
        this.questData = questData;

        objectiveStatuses = new Dictionary<int, Quest.Status>();

        for (int i = 0, count = questData.objectives.Count; i < count; ++i)
        {
            var objectiveData = questData.objectives[i];

            objectiveStatuses[i] = objectiveData.initialStatus;
        }
    }

    public Quest.Status questStatus
    {
        get
        {
            for (int i = 0, count = questData.objectives.Count; i < count; ++i)
            {
                var objectiveData = questData.objectives[i];

                if (objectiveData.optional)
                {
                    continue;
                }

                var objectiveStatus = objectiveStatuses[i];

                if (objectiveStatus == Quest.Status.Failed)
                {
                    return Quest.Status.Failed;
                }
                else if (objectiveStatus != Quest.Status.Complete)
                {
                    return Quest.Status.NotYetComplete;
                }
            }
            return Quest.Status.Complete;
        }
    }

    public override string ToString()
    {
        var stringBuilder = new System.Text.StringBuilder();

        for (int i = 0, count = questData.objectives.Count; i < count; ++i)
        {
            var objectiveData = questData.objectives[i];
            var objectiveStatus = objectiveStatuses[i];

            if (objectiveData.visible == false && 
                objectiveStatus == Quest.Status.NotYetComplete)
            {
                continue;
            }

            if (objectiveData.optional)
            {
                stringBuilder.AppendFormat(
                    "{0} (Optional) - {1}\n", 
                    objectiveData.name, 
                    objectiveData.ToString()
                    );
            }
            else
            {
                stringBuilder.AppendFormat(
                    "{0} - {1}\n",
                    objectiveData.name,
                    objectiveStatus.ToString()
                    );
            }

        }

        stringBuilder.AppendLine();
        stringBuilder.AppendFormat("Status: {0}", this.questStatus.ToString());

        return stringBuilder.ToString();
    }
}

public class QuestManager : MonoBehaviour
{
    [SerializeField] private Quest _startingQuest = null;
    [SerializeField] private UnityEngine.UI.Text _objectiveSummary = null;

    private QuestStatus _activeQuest;

    private void Awake()
    {
        if (_startingQuest != null)
        {
            StartQuest(_startingQuest);
        }
    }

    public void StartQuest(Quest quest)
    {
        _activeQuest = new QuestStatus(quest);

        UpdateObjectiveSummaryText();

        Debug.LogFormat("Started quest {0}", _activeQuest.questData.name);
    }

    public void UpdateObjectiveStatus(Quest quest, int objectiveNumber, Quest.Status status)
    {
        if (_activeQuest == null)
        {
            Debug.LogError(
                "Tried to set an objective status, but no quest" +
                "is active"
                );
            return;
        }

        if (_activeQuest.questData != quest)
        {
            Debug.LogWarningFormat("Tried to set an objective status " +
                "for quest {0}, but this is not the active quest. " +
                "Ignoring.",
                quest.questName);
            return;
        }

        _activeQuest.objectiveStatuses[objectiveNumber] = status;

        UpdateObjectiveSummaryText();
    }

    private void UpdateObjectiveSummaryText()
    {
        string label;

        if (_activeQuest == null)
        {
            label = "No active quest.";
        }
        else
        {
            label = _activeQuest.ToString();
        }

        _objectiveSummary.text = label;
    }
}
