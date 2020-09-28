using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] Slider _healthBar;
    
    [SerializeField] int _maxHealth = 100;

    public int _currentHealth;

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
        Debug.Log("Player's Health: " + _currentHealth);
        if(_currentHealth <= 0)
        {
            Kill();
        }
    }

    public void Kill()
    {
        //gameObject.SetActive(false);
        _healthBar.value = 0;
        //play feedback
    }
}
