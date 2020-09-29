using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stamina : MonoBehaviour
{
    [SerializeField] Slider _staminaBar;

    [SerializeField] int _maxStamina = 100;

    [SerializeField] const float _staminaIncrease = 1.0f;
    [SerializeField] const float _staminaTimeToRegen = 5.0f;
    private float _staminaRegenTimer = 0.0f;

    public float _currentStamina;
    public bool _playerBlocked = false;

    private void Start()
    {
        _currentStamina = _maxStamina;
    }

    private void Update()
    {
        _staminaBar.value = _currentStamina;
        if (_currentStamina < _maxStamina)
        {
            if(_staminaRegenTimer >= _staminaTimeToRegen)
            {
                _currentStamina = Mathf.Clamp(_currentStamina + (_staminaIncrease * Time.deltaTime), 0.0f, _maxStamina);
            }
            else
            {
                _staminaRegenTimer += Time.deltaTime;
            }
        }
    }

    public void DecreaseStamina(int amount)
    {
        _currentStamina -= amount;
        _playerBlocked = true;
        Debug.Log("Player's Stamina: " + _currentStamina);
    }

    IEnumerator Recharge()
    {
        yield return new WaitForSeconds(5);
        _currentStamina += 1;
    }
}
