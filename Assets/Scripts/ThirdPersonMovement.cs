using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ThirdPersonMovement : MonoBehaviour
{
    public event Action Idle = delegate { };
    public event Action StartRunning = delegate { };
    public event Action StartJumping = delegate { };
    public event Action Landed = delegate { };
    public event Action StartSprinting = delegate { };

    [SerializeField] CharacterController controller;
    [SerializeField] Transform cam;

    [SerializeField] Vector3 playerVelocity;

    [SerializeField] float _speed = 6f;
    [SerializeField] float _maxSpeed = 10f;
    [SerializeField] float _turnSmoothTime = 0.1f;
    [SerializeField] float _jumpHeight = 1f;
    [SerializeField] float _gravityValue = -9.81f;

    float _turnSmoothVelocity;
    bool _isMoving = false;
    bool _isSprinting = false;
    bool _isGrounded = false;
    bool _allowJump = true;

    private void Start()
    {
        Idle?.Invoke();
    }

    private void Update()
    {
        _isGrounded = controller.isGrounded;
        if(_isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if(direction.magnitude >= 0.1f)
        {
            CheckIfStartedMoving();
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, targetAngle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                controller.Move(moveDir.normalized * _maxSpeed * Time.deltaTime);
                CheckIfSprinting();
            }
            else
            {
                controller.Move(moveDir.normalized * _speed * Time.deltaTime);
            }
        }
        else
        {
            CheckIfStoppedMoving();
        }

        if (_isGrounded)
        {
            if (Input.GetButtonDown("Jump"))
            {
                CheckIfJumping();
                _allowJump = false;
                playerVelocity.y += Mathf.Sqrt(_jumpHeight * -3f * _gravityValue);
            }
            else
            {
                CheckIfLanded();
                _allowJump = true;
            }
        }
        playerVelocity.y += _gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    private void CheckIfStartedMoving()
    {
        if(_isMoving == false)
        {
            StartRunning?.Invoke();
            Debug.Log("Started");
        }
        _isMoving = true;
    }

    private void CheckIfStoppedMoving()
    {
        if (_isMoving == true)
        {
            Idle?.Invoke();
            Debug.Log("Stopped");
        }
        if(_isSprinting == true)
        {
            Idle?.Invoke();
            Debug.Log("Stopped");
        }
        _isMoving = false;
        _isSprinting = false;
    }

    private void CheckIfSprinting()
    {
        if (_isSprinting == false)
        {
            StartSprinting?.Invoke();
            Debug.Log("Sprinting");
        }
        _isSprinting = true;
    }

    private void CheckIfLanded()
    {
        if(_allowJump == false && _isGrounded)
        {
            Landed?.Invoke();
            Debug.Log("Landed");
            _isMoving = true;
        }
        _allowJump = true;
    }

    private void CheckIfJumping()
    {
        if (_allowJump == true && _isGrounded)
        {
            StartJumping?.Invoke();
            Debug.Log("Jumped");
        }
        _allowJump = false;
    }
}
