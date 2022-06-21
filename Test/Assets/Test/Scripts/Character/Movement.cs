using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 6.0f;
    [SerializeField] private float _jumpHeight = 2.0f;

    [SerializeField] private float _gravity = 20.0f;
    [Range(0, 10), SerializeField] private float _airControl = 5.0f;

    private Vector3 _moveDirection = Vector3.zero;

    private CharacterController _controller;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
    }

    private void FixedUpdate()
    {
        var input = new Vector3(
            Input.GetAxis("Horizontal"), 
            0.0f, 
            Input.GetAxis("Vertical")
            );
        
        input *= _moveSpeed;
        input = transform.TransformDirection(input);

        if (_controller.isGrounded)
        {
            _moveDirection = input;

            if (Input.GetButton("Jump"))
            {
                _moveDirection.y = Mathf.Sqrt(2 * _gravity * _jumpHeight);
            }
            else
            {
                _moveDirection.y = 0.0f;
            }
        }
        else
        {
            input.y = _moveDirection.y;
            _moveDirection = Vector3.Lerp(
                _moveDirection, 
                input, 
                _airControl * Time.deltaTime
                );
        }

        _moveDirection.y -= _gravity * Time.deltaTime;

        _controller.Move(_moveDirection * Time.deltaTime);
    }
}
