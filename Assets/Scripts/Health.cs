using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Health : MonoBehaviour
{
    public event Action Dead = delegate { };

    [SerializeField] Slider _healthBar;
    
    [SerializeField] int _maxHealth = 100;

    [SerializeField] ParticleSystem _damageParticles;

    [SerializeField] AudioClip _killSound;
    [SerializeField] AudioClip _overSound;

    public int _currentHealth;
    public bool _playerDamaged = false;

    private void Start()
    {
        _currentHealth = _maxHealth;
    }

    private void Update()
    {
        _healthBar.value = _currentHealth;

    }

    public void DamagePlayer(int amount)
    {
        _currentHealth -= amount;
        _playerDamaged = true;
        Debug.Log("Player's Health: " + _currentHealth);
        if(_currentHealth <= 0)
        {
            Kill();
        }
    }

    public void Kill()
    {
        CheckIfDead();
        if (_damageParticles != null)
        {
            _damageParticles.Play();
        }
        if (_killSound != null)
        {
            AudioHelper.PlayClip2D(_killSound, 1f);
        }
        if (_overSound != null)
        {
            AudioHelper.PlayClip2D(_overSound, 1f);
        }
    }

    public void CheckIfDead()
    {
        Dead?.Invoke();
    }
}
