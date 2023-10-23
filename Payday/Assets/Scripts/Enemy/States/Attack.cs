using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Attack : State
{
    

    public Chase chaseState;

    [SerializeField] public bool isInRange;

    public float attackRange = 15f;

    float distanceToTarget = Mathf.Infinity;

    

    public override State RunCurrentState()
    {

        Vector3 relativePos = target.transform.position - enemyfov.transform.position;

        distanceToTarget = Vector3.Distance(target.position,transform.position);

        if(distanceToTarget>attackRange)
        {
            isInRange = false;
        }
        else if(distanceToTarget<attackRange && enemyfov.canSeePlayer == false)
        {
            isInRange = false;
        }
        else
        {
            Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
            transform.parent.parent.rotation = rotation;
            isInRange = true;
        }

        if(isInRange == true)
        {
           // enemyAnimator.SetBool("attack", true);
            return this;
        }
        else
        {
            enemyAnimator.SetBool("attack", false);
            enemyAnimator.SetBool("chase", true);
            return chaseState;
        }
    }

}
