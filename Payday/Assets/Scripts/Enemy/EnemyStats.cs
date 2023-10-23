using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyStats : MonoBehaviour
{
    [SerializeField] float enemyHealth;
    [SerializeField] Image image;

    private const float defaultHealth = 100;

    void Start()
    {
        enemyHealth = defaultHealth;
        image.fillAmount= enemyHealth / 100;
    }

    public void GetDamage(int dmg)
    {
        enemyHealth-= dmg;
        image.fillAmount = enemyHealth / 100;
    }
}
