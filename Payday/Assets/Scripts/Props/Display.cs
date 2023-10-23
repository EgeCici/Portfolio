using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Display : MonoBehaviour, IDamagable
{
    public float glassHealth = 1f;
    public AudioSource shatterSfx;
    public GameObject brokenMesh; 
    public GameObject unBrokenMesh;
    public ParticleSystem shatterVfx;
    public BoxCollider displayCollider;
   


  
    public void Damage(int damage, Vector3 contactPoint, bool isGrenade = false)
    {
        glassHealth -= damage;
        
        if (glassHealth <= 0)
        {
            StartCoroutine(Broken());
        }

    }

    IEnumerator Broken()
    {
        shatterSfx.Play();
        shatterVfx.Play();
        unBrokenMesh.SetActive(false);
        displayCollider.enabled = false;
        yield return new WaitForSeconds(0.1f);
        brokenMesh.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        displayCollider.enabled = true;
        Destroy(this);
    }

    
}
