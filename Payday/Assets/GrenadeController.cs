using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class GrenadeController : MonoBehaviour
{
    [Header("Grenade")]
    public int damage;
    public float delay = 3f;

    public float explosionForce = 10f;
    public float radius = 20f;
    public AudioSource grenadeSFX;
    [SerializeField] ParticleSystem explosion;

    private bool isCollided;

    private void OnCollisionEnter(Collision collision)
    {
        if (isCollided)
            return;
        isCollided = true;
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);
        explosion.Play();
        grenadeSFX.Play();
        Debug.Log(colliders.Length);
        foreach (Collider near in colliders)
        {
            if (near.gameObject.TryGetComponent(out EnemyBodyPart enemy))
            {
                enemy.Damage(damage, near.ClosestPoint(transform.position), true);
            }

            if (near.gameObject.TryGetComponent(out NPCController npc))
            {
                npc.Damage(damage, near.ClosestPoint(transform.position), true);
            }
        }
        Destroy(gameObject, 1);
    }

}
