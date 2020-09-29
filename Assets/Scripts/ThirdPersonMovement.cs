using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class ThirdPersonMovement : MonoBehaviour
{
    public event Action Idle = delegate { };
    public event Action StartRunning = delegate { };
    public event Action StartJumping = delegate { };
    public event Action Landed = delegate { };
    public event Action StartSprinting = delegate { };
    public event Action StartAttacking = delegate { };
    public event Action StartBlocking = delegate { };
    public event Action Damaged = delegate { };
    public event Action DamageBlocked = delegate { };

    [SerializeField] CharacterController controller;
    [SerializeField] Transform cam;

    [SerializeField] Vector3 playerVelocity;

    [SerializeField] float _speed = 6f;
    [SerializeField] float _maxSpeed = 10f;
    [SerializeField] float _turnSmoothTime = 0.1f;
    [SerializeField] float _jumpHeight = 1f;
    [SerializeField] float _gravityValue = -9.81f;

    [SerializeField] AbilityLoadout _abilityLoadout;
    [SerializeField] Ability _startingAbility;
    [SerializeField] Ability _newAbilityToTest;

    [SerializeField] Transform _targetDummy = null;

    [SerializeField] Canvas _playerHUD;

    [SerializeField] Health _health;
    [SerializeField] Stamina _stamina;
    [SerializeField] Bullet _bullet;

    public Transform CurrentTarget { get; private set; }

    float _turnSmoothVelocity = 0f;
    bool _allowMove = true;
    bool _isMoving = false;
    bool _isSprinting = false;
    bool _isGrounded = false;
    bool _allowJump = true;
    bool _allowAttack = true;
    bool _isAttacking = false;
    bool _allowBlock = true;
    bool _isDamaged = false;
    public bool _isBlocking = false;
    public bool _attackBlocked = false;

    RaycastHit hit;

    private void Awake()
    {
        if (_startingAbility != null)
        {
            _abilityLoadout?.EquipAbility(_startingAbility);
        }
    }

    //TODO consider breaking into separate scripts
    public void SetTarget(Transform newTarget)
    {
        CurrentTarget = newTarget;
    }


    private void Start()
    {
        Idle?.Invoke();
    }

    private void Update()
    {
        if (_health._currentHealth > 0)
        {
            Control();
            if (_health._playerDamaged)
            {
                CheckIfDamaged();
            }
            CheckIfDamageBlocked();
        }
        if (_health._currentHealth <= 0)
        {
            Release();
        }
    }

    private void Control()
    {
        _playerHUD.enabled = true;

        //equip new weapon 
        //TODO- add new weapons later 
        //Will have to use new model due to the current weapon being part of the current model
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _abilityLoadout.EquipAbility(_newAbilityToTest);
        }
        //set a target, for testing
        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            //target the dummy for now
            SetTarget(_targetDummy);
        }

        _isGrounded = controller.isGrounded;
        if (_isGrounded && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f && _allowMove)
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
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                _isSprinting = true;
                CheckIfStoppedMoving();
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

            if (_allowBlock)
            {
                if (Input.GetKey(KeyCode.Mouse1) && _stamina._currentStamina >= 10)
                {
                    _allowMove = false;
                    CheckIfBlocking();
                }
                else
                {
                    _allowMove = true;
                }
            }
            else
            {
                CheckIfStoppedMoving();
            }

            if (_isBlocking && Physics.Raycast(this.transform.position, this.transform.TransformDirection(Vector3.forward), out hit, 50f))
            {
                _attackBlocked = true;
            }
            else
            {
                _attackBlocked = false;
            }

            if (_allowAttack == true)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0) && _isAttacking == false && _stamina._currentStamina >= 10)
                {
                    _isAttacking = true;
                    _abilityLoadout.UseEquippedAbility(CurrentTarget);
                    CheckIfAttacking();
                }
            }
            else
            {
                CheckIfStoppedMoving();
            }

        }
        playerVelocity.y += _gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    private void Release()
    {
        _playerHUD.enabled = false;
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
        if (_isBlocking == true)
        {
            Idle?.Invoke();
            Debug.Log("Stopped");
        }
        if (_allowAttack == false)
        {
            StartCoroutine("Attack");
            Debug.Log("Stopped");
        }
        if (_isDamaged)
        {
            StartCoroutine("Damage");
            Debug.Log("Stopped");
        }
        _isMoving = false;
        _isSprinting = false;
        _isBlocking = false;
        _allowBlock = true;
        _allowAttack = true;
        _isDamaged = false;
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

    private void CheckIfAttacking()
    {
        if(_allowAttack == true && _isGrounded)
        {
            StartAttacking?.Invoke();
            Debug.Log("Attacking");
            _stamina.DecreaseStamina(10);
        }
        _allowAttack = false; 
    }

    private void CheckIfBlocking()
    {
        if (_isBlocking == false && _allowMove == false)
        {
            StartBlocking?.Invoke();
            Debug.Log("Blocking");
        }
        _isBlocking = true;
        _allowBlock = false;
    }

    private void CheckIfDamaged()
    {
        if (_health._playerDamaged)
        {
            Damaged?.Invoke();
            Debug.Log("Damaged");
        }
        _health._playerDamaged = false;
        _isDamaged = true;
        if(_health._playerDamaged == false)
        {
            CheckIfStoppedMoving();
        }
    }

    private void CheckIfDamageBlocked()
    {
        if (_stamina._playerBlocked && _isAttacking == false)
        {
            DamageBlocked?.Invoke();
        }
        _stamina._playerBlocked = false;
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(1.2f);
        _isAttacking = false;
        Idle?.Invoke();
    }

    IEnumerator Damage()
    {
        yield return new WaitForSeconds(1.0f);
        Idle?.Invoke();
    }
}
