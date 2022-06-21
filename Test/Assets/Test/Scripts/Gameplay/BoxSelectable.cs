using UnityEngine;

public class BoxSelectable : MonoBehaviour
{
    public void Selected()
    {
        Debug.LogFormat("{0} was selected!", gameObject.name);
    }
}
