using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordAttack : Ability
{
    int _damageAmount = 25;
    RaycastHit hit;
    bool _takeDamage = false;

    public GameObject _player;

    [SerializeField] ParticleSystem _abilityParticles;
    [SerializeField] AudioSource _abilityAudio;

    public override void Use(Transform origin, Transform target)
    {
        //audio
        if (_abilityAudio != null)
        {
            _abilityAudio.Play();
        }
        if (Physics.Raycast(_player.transform.position, _player.transform.TransformDirection(Vector3.forward), out hit, 0.5f))
        {
            Debug.Log("Slashed at " + target.gameObject.name);
            target.GetComponent<EnemyHealth>()?.Damage(_damageAmount);
            //particles
            if (_abilityParticles != null)
            {
                _abilityParticles.Play();
            }
        }
    }
}
