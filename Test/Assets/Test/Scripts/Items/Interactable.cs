using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Interactable : MonoBehaviour
{
    public void Interact(GameObject fromObject)
    {
        Debug.Log("Interact with {0}", fromObject);
    }
}
