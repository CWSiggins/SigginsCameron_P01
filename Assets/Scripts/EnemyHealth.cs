using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    int _currentHealth = 100;
    public GameObject _target;

    [SerializeField] ParticleSystem _impactParticles;
    [SerializeField] AudioSource _impactAudio;

    public void Damage(int amount)
    {
        _currentHealth -= amount;
        Debug.Log("Target Dummy Health: " + _currentHealth);
        StartCoroutine("DealDamage");
        if (_currentHealth <= 0)
        {
            StartCoroutine("Die");
        }
    }

    IEnumerator DealDamage()
    {
        yield return new WaitForSeconds(0.8f);
        //particles
         if (_impactParticles != null)
        {
            _impactParticles.Play();
        }
        //audio
        if (_impactAudio != null)
        {
            _impactAudio.Play();
        }
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(1);
        gameObject.SetActive(false);
    }
}
