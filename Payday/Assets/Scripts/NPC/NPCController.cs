using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCController : MonoBehaviour, IDamagable
{
    public int npcHealth = 2;
    public int damageAmount = 1;
    public bool isDead;
    public Animator npcAnimator;
    public ParticleSystem bloodParticle;
    public Collider npcCollider;

    

    public void Damage(int value, Vector3 contactPoint, bool isGrenade = false)
    {
        
        npcAnimator.SetTrigger("hit");
        npcHealth -= 1;
        bloodParticle.Play();
        
        if (npcHealth <= 0)
        {
            Dead();
        }
    }

    public void Dead()
   {
        isDead = true;
        npcAnimator.SetTrigger("dead");
        npcCollider.enabled = false;
        Destroy(gameObject, 5);
            
   }
}
