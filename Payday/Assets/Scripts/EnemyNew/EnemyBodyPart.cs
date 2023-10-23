using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBodyPart : MonoBehaviour, IDamagable
{
    public int damageMultiplier;

    [HideInInspector]
    public EnemyController enemyController;

    private void Awake()
    {
        enemyController = GetComponentInParent<EnemyController>();
    }
    public void Damage(int value, Vector3 contactPoint, bool isGrenade = false)
    {
        enemyController.Damage(value * damageMultiplier, contactPoint);
        if(damageMultiplier > 1 && !isGrenade)
        {
            PlayerController.Instance.OpenHeadShotIcon();
        }
    }
}
