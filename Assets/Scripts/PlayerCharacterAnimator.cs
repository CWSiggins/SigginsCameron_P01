using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerCharacterAnimator : MonoBehaviour
{
    [SerializeField] ThirdPersonMovement _thirdPersonMovement = null;
    [SerializeField] Health _health = null;
    //these names MUST allign with the naming in the Animator node
    const string IdleState = "Idle";
    const string RunState = "Run";
    const string JumpState = "Jumping";
    const string FallState = "Falling";
    const string SprintState = "Sprint";
    const string AttackState = "Attack";
    const string BlockState = "Block";
    const string DeadState = "Dead";
    const string DamageState = "Damage";
    const string DamageBlockedState = "DamageBlocked";

    Animator _animator = null;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void OnIdle()
    {
        _animator.CrossFadeInFixedTime(IdleState, .2f);
    }

    public void OnStartRunning()
    {
        _animator.CrossFadeInFixedTime(RunState, .2f);
    }

    public void OnStartSprinting()
    {
        _animator.CrossFadeInFixedTime(SprintState, .2f);
    }

    public void OnStartJumping()
    {
        _animator.CrossFadeInFixedTime(JumpState, .2f);
    }
    
    public void OnFalling()
    {
        _animator.CrossFadeInFixedTime(FallState, .2f);
    }

    public void OnAttacking()
    {
        _animator.CrossFadeInFixedTime(AttackState, .2f);
    }

    public void OnBlocking()
    {
        _animator.CrossFadeInFixedTime(BlockState, .2f);
    }

    public void OnDeath()
    {
        _animator.CrossFadeInFixedTime(DeadState, .2f);
    }

    public void OnDamage()
    {
        _animator.CrossFadeInFixedTime(DamageState, .2f);
    }

    public void OnDamageBlocked()
    {
        _animator.CrossFadeInFixedTime(DamageBlockedState, .2f);
    }

    private void OnEnable()
    {
        _thirdPersonMovement.Idle += OnIdle;
        _thirdPersonMovement.StartRunning += OnStartRunning;
        _thirdPersonMovement.StartJumping += OnStartJumping;
        _thirdPersonMovement.Landed += OnFalling;
        _thirdPersonMovement.StartSprinting += OnStartSprinting;
        _thirdPersonMovement.StartAttacking += OnAttacking;
        _thirdPersonMovement.StartBlocking += OnBlocking;
        _health.Dead += OnDeath;
        _thirdPersonMovement.Damaged += OnDamage;
        _thirdPersonMovement.DamageBlocked += OnDamageBlocked;
    }

    private void OnDisable()
    {
        _thirdPersonMovement.Idle -= OnIdle;
        _thirdPersonMovement.StartRunning -= OnStartRunning;
        _thirdPersonMovement.StartJumping -= OnStartJumping;
        _thirdPersonMovement.Landed -= OnFalling;
        _thirdPersonMovement.StartSprinting -= OnStartSprinting;
        _thirdPersonMovement.StartAttacking -= OnAttacking;
        _thirdPersonMovement.StartBlocking -= OnBlocking;
        _health.Dead -= OnDeath;
        _thirdPersonMovement.Damaged -= OnDamage;
        _thirdPersonMovement.DamageBlocked -= OnDamageBlocked;
    }
}
