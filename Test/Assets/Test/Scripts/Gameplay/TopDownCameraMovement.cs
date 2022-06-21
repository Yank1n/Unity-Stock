using UnityEngine;

public class TopDownCameraMovement : MonoBehaviour
{
    [SerializeField] private float _movementSpeed = 20.0f;

    [SerializeField] private Vector2 _minimumLimit = -Vector2.one;
    [SerializeField] private Vector2 _maximumLimit = Vector2.one;

    Bounds Bound
    {
        get
        {
            var cameraHeight = transform.position.y;

            Vector3 minLimit = new Vector3(_minimumLimit.x, cameraHeight, _minimumLimit.y);
            Vector3 maxLimit = new Vector3(_maximumLimit.x, cameraHeight, _maximumLimit.y);

            var newBounds = new Bounds();
            newBounds.min = minLimit;
            newBounds.max = maxLimit;

            return newBounds;
        }
    }

    private void Update()
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");

        var offset = new Vector3(horizontal, 0, vertical) * Time.deltaTime * _movementSpeed;

        var newPosition = transform.position + offset;

        if (Bound.Contains(newPosition))
        {
            transform.position = newPosition;
        }
        else
        {
            transform.position = Bound.ClosestPoint(newPosition);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(Bound.center, Bound.size);
    }

}
