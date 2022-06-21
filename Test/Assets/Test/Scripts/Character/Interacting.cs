using UnityEngine;

public class Interacting : MonoBehaviour
{
    [SerializeField] private KeyCode _interactionKey = KeyCode.E;
    [SerializeField] private float _interactionRange = 2.0f;

    private void Update()
    {
        if (Input.GetKeyDown(_interactionKey))
        {
            AttempInteract();
        }
    }

    private void AttempInteract()
    {
        var ray = new Ray(transform.position, transform.forward);

        RaycastHit hit;

        var everythingExceptPlayers = ~(1 << LayerMask.NameToLayer("Player"));

        var layerMask = Physics.DefaultRaycastLayers & everythingExceptPlayers;

        if (Physics.Raycast(ray, out hit, _interactionRange, layerMask))
        {
            var interactable = hit.collider.GetComponent<Interactable>();

            if (interactable != null)
            {
                interactable.Interact(this.gameObject);
            }
        }
    }
}