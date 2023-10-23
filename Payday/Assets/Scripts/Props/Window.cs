using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Window : MonoBehaviour, IDamagable
{
    public float propHealth = 1f;
    public AudioSource shatterSfx;
    public ParticleSystem glassVfx;
    public ParticleSystem glassVfx2;
    public GameObject prop;
   

    public void Damage(int damage, Vector3 contactPoint, bool isGrenade = false)
    {
        propHealth -= damage;

        if (propHealth <= 0)
        {
            StartCoroutine(ShatterGlass());
            

        }

    }

    IEnumerator ShatterGlass()
    {
        shatterSfx.Play();
        glassVfx.Play();
        glassVfx2.Play();
        yield return new WaitForSeconds(0.1f);
        Destroy(prop);
        yield return new WaitForSeconds(1f);
        Destroy(this);
    }
}
