using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Grabbing : MonoBehaviour
{
    [SerializeField] private float _grabbingRange = 3.0f;
    [SerializeField] private float _pullingRange = 5.0f;

    [SerializeField] private Transform _holdPoint = null;

    [SerializeField] private KeyCode _grabKey = KeyCode.F;
    [SerializeField] private KeyCode _throwKey = KeyCode.Mouse0;

    [SerializeField] private float _throwForce = 100.0f;
    [SerializeField] private float _pullForce = 50.0f;
    [SerializeField] private float _grabBreakingForce = 100.0f;
    [SerializeField] private float _grabBreakingTorgue = 100.0f;

    private FixedJoint _grabJoint;

    private Rigidbody _grabbedRb;

    private void Awake()
    {
        if (_holdPoint == null)
        {
            Debug.LogError("Grab point must not be null!");
        }

        if (!_holdPoint.IsChildOf(transform))
        {
            Debug.LogError("Grab point should be child of this object!");
        }

        var playerCollider = GetComponent<Collider>();
        playerCollider.gameObject.layer = LayerMask.NameToLayer("Player");
    }

    private void Update()
    {
        if (Input.GetKeyDown(_grabKey) && _grabJoint == null)
        {
            AttempPull();
        }
        else if (Input.GetKeyDown(_grabKey) && _grabJoint != null)
        {
            Drop();
        }
        else if (Input.GetKeyDown(_throwKey) && _grabJoint != null)
        {
            Throw();
        }
    }

    private void AttempPull()
    {
        var ray = new Ray(transform.position, transform.forward);

        RaycastHit hit;

        var everythingExceptPlayers = ~(1 << LayerMask.NameToLayer("Player"));
        var layerMask = Physics.DefaultRaycastLayers & everythingExceptPlayers;

        var hitSomething = Physics.Raycast(ray, out hit, _pullingRange, layerMask);
        if (hitSomething == false)
        {
            return;
        }

        _grabbedRb = hit.rigidbody;

        if (_grabbedRb == null || _grabbedRb.isKinematic)
        {
            return;
        }

        if (hit.distance < _grabbingRange)
        {
            _grabbedRb.transform.position = _holdPoint.position;

            _grabJoint = gameObject.AddComponent<FixedJoint>();
            _grabJoint.connectedBody = _grabbedRb;
            _grabJoint.breakForce = _grabBreakingForce;
            _grabJoint.breakTorque = _grabBreakingTorgue;

            foreach (var myCollider in GetComponentsInParent<Collider>())
            {
                Physics.IgnoreCollision(myCollider, hit.collider, true);
            }
        }
        else
        {
            var pull = -transform.forward * _pullForce;
            _grabbedRb.AddForce(pull);
        }
    }

    private void Drop()
    {
        if (_grabJoint != null)
        {
            Destroy(_grabJoint);
        }

        if (_grabbedRb == null)
        {
            return;
        }

        foreach (var myCollider in GetComponentsInParent<Collider>())
        {
            Physics.IgnoreCollision(
                myCollider,
                _grabbedRb.GetComponent<Collider>(),
                false
                );
        }

        _grabbedRb = null;
    }

    private void Throw()
    {
        if (_grabbedRb == null)
        {
            return;
        }

        var thrownBody = _grabbedRb;

        var force = transform.forward * _throwForce;

        thrownBody.AddForce(force);

        Drop();
    }

    private void OnDrawGizmos()
    {
        if (_holdPoint == null)
        {
            return;
        }

        Gizmos.color = Color.magenta;

        Gizmos.DrawSphere(_holdPoint.position, 0.2f);
    }

    private void OnJointBreak(float breakForce)
    {
        Drop();
    }
}
