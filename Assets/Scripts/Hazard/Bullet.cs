using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] GameObject _player;
    [SerializeField] Health _health;
    [SerializeField] Stamina _stamina;
    [SerializeField] ThirdPersonMovement _movement;

    [SerializeField] int _damageAmount = 10;

    private void Start()
    {
        _player = GameObject.Find("Player");
        _health = (Health)_player.GetComponent<Health>();
        _stamina = (Stamina)_player.GetComponent<Stamina>();
        _movement = (ThirdPersonMovement)_player.GetComponent<ThirdPersonMovement>();
    }

    private void OnCollisionEnter(Collision collision)
        {
            if(collision.gameObject.tag == "Player" && _movement._attackBlocked == false)
            {
                _health.DamagePlayer(_damageAmount);
            }

            if(collision.gameObject.tag == "Player" && _movement._attackBlocked == true)
            {
                _stamina.DecreaseStamina(10);
            }
        }
}
