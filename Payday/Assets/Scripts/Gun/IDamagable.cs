using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    
    void Damage(int value, Vector3 contactPoint, bool isGrenade = false);

    
}
