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
        _healthBar.value = 0;
        //play feedback
    }

    public void CheckIfDead()
    {
        Dead?.Invoke();
    }
}
