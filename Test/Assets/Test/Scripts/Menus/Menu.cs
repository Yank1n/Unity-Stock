using UnityEngine;
using UnityEngine.Events;

public class Menu : MonoBehaviour
{
    public UnityEvent menuDidAppear = new UnityEvent();

    public UnityEvent menuWillDisappear = new UnityEvent();
}
