using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthKit : MonoBehaviour
{
    public int increaseHealthValue = 10;

    public void Collect()
    {
        
        Destroy(gameObject);
    }

}
