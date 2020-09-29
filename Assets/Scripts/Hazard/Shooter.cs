using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] Transform _player;

    [SerializeField] Health _health;

    [SerializeField] float _range = 50.0f;
    [SerializeField] float _bulletImpulse = 20.0f;

    [SerializeField] bool _inRange = false;

    [SerializeField] Rigidbody _projectile;

    private void Start()
    {
        float _rand = UnityEngine.Random.Range(1.0f, 2.0f);
        InvokeRepeating("Shoot", 2, _rand);
    }

    private void Update()
    {
        _inRange = Vector3.Distance(transform.position, _player.position) < _range;

        if (_inRange)
        {
            transform.LookAt(_player);
        }
    }

    private void Shoot()
    {
        if (_inRange && _health._currentHealth > 0)
        {
            Rigidbody _bullet = (Rigidbody)Instantiate(_projectile, transform.position + transform.forward, transform.rotation);
            _bullet.AddForce(transform.forward * _bulletImpulse, ForceMode.Impulse);

            Destroy(_bullet.gameObject, 1);
        }
    }
}
