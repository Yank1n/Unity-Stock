using UnityEngine;
using UnityEngine.EventSystems;

public class TriggerObjectiveOnClick : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] ObjectiveTrigger _objective = new ObjectiveTrigger();

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        _objective.Invoke();

        this.enabled = false;
    }
}