using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;
#endif

public class EnemyVisibility : MonoBehaviour
{
    [SerializeField] private Transform _target = null;

    [SerializeField] private float _maxDistance = 10.0f;
    public float MaxDistance
    {
        get
        {
            return _maxDistance;
        }
        set
        {
            _maxDistance = value;
        }
    }
    [SerializeField, Range(0f, 360f)] private float _angle = 45f;
    public float Angle
    {
        get
        {
            return _angle;
        }
    }
    [SerializeField] private bool _visualize = true;

    public bool TargetIsVisible { get; private set; }

    private Color _color;

    private void Awake()
    {
        _color = GetComponent<Renderer>().material.color;
    }

    private void Update()
    {
        TargetIsVisible = CheckVisibility();
        if (_visualize)
        {
            var color = TargetIsVisible ? Color.yellow : Color.white;
            _color = color;
        }
    }

    private bool CheckVisibilityToPoint(Vector3 worldPoint)
    {
        var directionToTarget = worldPoint - transform.position;

        var degreesToTarget = Vector3.Angle(transform.forward, directionToTarget);

        var withinArc = degreesToTarget < (_angle / 2);

        if (withinArc == false)
        {
            return false;
        }

        var distanceToTarget = directionToTarget.magnitude;
        var rayDistance = Mathf.Min(_maxDistance, distanceToTarget);
        var ray = new Ray(transform.position, directionToTarget);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            if (hit.collider.transform == _target)
            {
                return true;
            }
            return false;
        }
        else
        {
            return true;
        }
    }

    private bool CheckVisibility()
    {
        var directionToTarget = _target.position - transform.position;

        var degreesToTarget = Vector3.Angle(transform.forward, directionToTarget);

        var withinArc = degreesToTarget < (_angle / 2);

        if (withinArc == false)
        {
            return false;
        }

        var distanceToTarget = directionToTarget.magnitude;
        var rayDistance = Mathf.Min(_maxDistance, distanceToTarget);
        var ray = new Ray(transform.position, directionToTarget);
        RaycastHit hit;

        var canSee = false;

        if (Physics.Raycast(ray, out hit, rayDistance))
        {
            if (hit.collider.transform == _target)
            {
                canSee = true;
            }

            Debug.DrawLine(transform.position, hit.point);
        }
        else
        {
            Debug.DrawRay(transform.position, directionToTarget.normalized * rayDistance);
        }

        return canSee;
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(EnemyVisibility))]
public class EnemyVisibilityEditor : Editor
{
    private void OnSceneGUI()
    {
        var visibility = target as EnemyVisibility;

        Handles.color = new Color(1, 1, 1, 0.1f);

        var forwardPointMinusHalfAngle = 
            Quaternion.Euler(0, -visibility.Angle / 2, 0) * 
            visibility.transform.forward;

        Vector3 arcStart = forwardPointMinusHalfAngle * visibility.MaxDistance;

        Handles.DrawSolidArc(
            visibility.transform.position,
            Vector3.up,
            arcStart,
            visibility.Angle,
            visibility.MaxDistance
            );

        Handles.color = Color.white;

        Vector3 handlePosition = 
            visibility.transform.position + 
            visibility.transform.forward * visibility.MaxDistance;

        visibility.MaxDistance = Handles.ScaleValueHandle(
            visibility.MaxDistance, 
            handlePosition,
            visibility.transform.rotation,
            1,
            Handles.ConeHandleCap,
            0.25f
            );
    }
}
#endif