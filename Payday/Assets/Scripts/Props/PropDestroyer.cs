using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropDestroyer : MonoBehaviour, IDamagable
{
    public float propHealth = 1f;
    public AudioSource shatterSfx;
    public ParticleSystem smokeVfx;
    public GameObject prop;
   

    public void Damage(int damage, Vector3 contactPoint, bool isGrenade = false )
    {
        propHealth -= damage;

        if (propHealth <= 0)
        {
            StartCoroutine(Explode());
        }
    }

    IEnumerator Explode()
    {
        shatterSfx.Play();
        smokeVfx.Play();
        yield return new WaitForSeconds(0.1f);
        Destroy(prop);
        yield return new WaitForSeconds(0.5f);
        Destroy(this);
    }
}
