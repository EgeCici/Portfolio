using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Idle : State
{
    public Chase chaseState;
    public bool isProvoked = false;

   
    //float distanceToTarget = Mathf.Infinity;
    //[SerializeField] float chaseRange = 10f;
    //[SerializeField] Transform target;

    public override State RunCurrentState()
    {
        

        //distanceToTarget = Vector3.Distance(target.position,transform.position);

        //Debug.Log(distanceToTarget);

        // if(distanceToTarget <= chaseRange)
        // {
        //     Debug.Log(isProvoked + " " + gameObject.name);
        //     isProvoked = true;
        // }

        if(isProvoked == true)
        {
            enemyAnimator.SetBool("idle", false);
            enemyAnimator.SetBool("chase", true);
            return chaseState;
            
        }
        else
        {
            enemyAnimator.SetBool("idle", true);
            return this;
        }
    }
}
