using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Vector3[] _points = { };
    [SerializeField] private float _speed = 10.0f;

    private int _nextPoint = 0;

    private Vector3 _startPosition;

    public Vector3 Velocity { get; private set; }
    private Vector3 CurrentPoint
    {
        get
        {
            if (_points == null || _points.Length == 0)
            {
                return transform.position;
            }

            return _points[_nextPoint] + _startPosition;
        }
    }

    private void Awake()
    {
        if (_points == null || _points.Length < 2)
        {
            Debug.LogError("Platform needs 2 or more points!");
        }

        _startPosition = transform.position;

        transform.position = CurrentPoint;
    }

    private void FixedUpdate()
    {
        var newPosition = Vector3.MoveTowards(
            transform.position,
            CurrentPoint,
            _speed * Time.deltaTime
            );

        if (Vector3.Distance(newPosition, CurrentPoint) < 0.001f)
        {
            newPosition = CurrentPoint;

            _nextPoint += 1;
            _nextPoint %= _points.Length;
        }

        Velocity = (newPosition - transform.position) / Time.deltaTime;

        transform.position = newPosition;
    }

    private void OnDrawGizmosSelected()
    {
        if (_points == null || _points.Length < 2)
        {
            return;
        }

        Vector3 offsetPosition = transform.position;

        if (Application.isPlaying)
        {
            offsetPosition = _startPosition;
        }

        Gizmos.color = Color.blue;

        for (int p = 0, length = _points.Length; p < length; ++p)
        {
            var p1 = _points[p];
            var p2 = _points[(p + 1) % _points.Length];

            Gizmos.DrawSphere(offsetPosition + p1, 0.1f);
            Gizmos.DrawLine(offsetPosition + p1, offsetPosition + p2);
        }
    }
}
