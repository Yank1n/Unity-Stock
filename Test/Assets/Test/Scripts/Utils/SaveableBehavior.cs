using LitJson;
using UnityEngine;

public abstract class SaveableBehavior : MonoBehaviour,
    ISaveable,
    ISerializationCallbackReceiver
{
    [HideInInspector, SerializeField] private string _saveID;

    public string SaveID
    {
        get
        {
            return _saveID;
        }
        set
        {
            _saveID = value;
        }
    }

    public abstract JsonData SaveData { get; }
    public abstract void LoadFromData(JsonData data);

    public void OnBeforeSerialize()
    {
        if (_saveID == null)
        {
            _saveID = System.Guid.NewGuid().ToString();
        }
    }

    public void OnAfterDeserialize()
    {

    }
}