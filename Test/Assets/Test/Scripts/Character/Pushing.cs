using UnityEngine;

public class Pushing : MonoBehaviour
{
    public enum PushMode
    {
        NoPushing,
        DirectlySetVelocity,
        ApplyForces
    }

    [SerializeField] private PushMode _pushMode = PushMode.DirectlySetVelocity;
    [SerializeField] private float _pushPower = 5.0f;

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (_pushMode == PushMode.NoPushing)
        {
            return;
        }

        var hitRb = hit.rigidbody;

        if (hitRb == null || hitRb.isKinematic)
        {
            return;
        }

        CharacterController controller = hit.controller;

        var footPosition = 
            controller.transform.position.y -
            controller.center.y -
            controller.height / 2;

        if (hit.point.y <= footPosition)
        {
            return;
        }

        switch (_pushMode)
        {
            case PushMode.DirectlySetVelocity:
                hitRb.velocity = controller.velocity;
                break;
            case PushMode.ApplyForces:
                Vector3 force = controller.velocity * _pushPower;
                hitRb.AddForceAtPosition(force, hit.point);
                break;
        }
    }
}
