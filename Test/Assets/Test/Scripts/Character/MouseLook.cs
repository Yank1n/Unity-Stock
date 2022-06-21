using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private float _turnSpeed = 300.0f;

    [SerializeField] private float _headUpperAngleLimit = 85.0f;
    [SerializeField] private float _headLowerAngleLimit = -80.0f;

    private float _yaw = 0.0f;
    private float _pitch = 0.0f;

    private Quaternion _bodyStartOrientation;
    private Quaternion _headStartOrientation;

    private Transform _head;

    private void Awake()
    {
        _head = GetComponentInChildren<Camera>().transform;

        _bodyStartOrientation = transform.localRotation;
        _headStartOrientation = _head.transform.localRotation;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
    }

    private void LateUpdate()
    {
        var horizontal = Input.GetAxis("Mouse X") * Time.deltaTime * _turnSpeed;
        var vertical = Input.GetAxis("Mouse Y") * Time.deltaTime * _turnSpeed;

        _yaw += horizontal;
        _pitch += vertical;

        _pitch = Mathf.Clamp(_pitch, _headLowerAngleLimit, _headUpperAngleLimit);

        var bodyRotation = Quaternion.AngleAxis(_yaw, Vector3.up);
        var headRotation = Quaternion.AngleAxis(-_pitch, Vector3.right);

        transform.localRotation = bodyRotation * _bodyStartOrientation;
        _head.localRotation = headRotation * _headStartOrientation;
    }
}
