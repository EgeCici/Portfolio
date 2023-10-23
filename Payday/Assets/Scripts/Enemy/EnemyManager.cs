using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance;

    public List<Enemy> enemies = new List<Enemy>();
    private void Awake()
    {
        Instance = this;
    }
}
