using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Chase : State
{
    
    public Attack attackState;

    public bool isInRange;

    float distanceToTarget = Mathf.Infinity;

    float attackRange = 15f;
    float releaseRange = 25f;

    Vector3 currentDestination;

    public ParticleSystem muzzleFlash;
    public ParticleSystem ejectCasing;

    private AudioSource enemyGunAudio;

    

    

    private void Start()
    {
        enemyGunAudio = GetComponent<AudioSource>();
    }
    public override State RunCurrentState()
    {

        
        currentDestination = transform.position;
        

        //Debug.Log("Can see player? ="+enemyfov.canSeePlayer + "  l�" +"Object Name: "+ transform.parent.parent.name + "Stop range= " + navMesh.stoppingDistance);

        if(distanceToTarget<=attackRange && enemyfov.canSeePlayer==true)
        {
            distanceToTarget = Vector3.Distance(target.position,transform.position);
            navMesh.SetDestination(currentDestination);
            isInRange = true;
            navMesh.stoppingDistance = 8;
            
            
        }
        else if(enemyfov.canSeePlayer==false && distanceToTarget<=attackRange)
        {
            distanceToTarget = Vector3.Distance(target.position,transform.position);
            navMesh.SetDestination(target.position);
            navMesh.stoppingDistance = 2;
            isInRange = false;
            
        }
        else
        {
            distanceToTarget = Vector3.Distance(target.position,transform.position);
            navMesh.SetDestination(target.position);
            isInRange = false;
            
        }


        if (distanceToTarget >= releaseRange)
        {
            Debug.Log("im tired");
            idleState.isProvoked = false;
            enemyAnimator.SetBool("idle", true);
            enemyAnimator.SetBool("chase", false);
            return idleState;
        }


        if (isInRange == true)
        {
            return attackState;
        }
        else
        {
            enemyAnimator.SetBool("chase", true);
            muzzleFlash.Stop();
            ejectCasing.Stop();
            enemyGunAudio.Stop();
            
            return this;
        }

        
    }
}
