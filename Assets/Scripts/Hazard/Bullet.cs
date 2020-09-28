using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] GameObject _player;
    [SerializeField] Health _health;

    [SerializeField] int _damageAmount = 10;

    private void Start()
    {
        _player = GameObject.Find("Player");
        _health = (Health)_player.GetComponent<Health>();
    }

    private void OnCollisionEnter(Collision collision)
        {
            if(collision.gameObject.tag == "Player")
            {
                _health.DamagePlayer(_damageAmount);
            }
        }
}
