using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FieldOfView : MonoBehaviour
{
    public float radius;
    [Range(0,360)]
    public float angle;

    public GameObject player;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public bool canSeePlayer;

    public Transform raycastPos;

    NavMeshAgent enemyNavMesh;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
    }

    IEnumerator FOVRoutine()
    {

        while(true)
        {
            FieldofViewCheck();

            yield return new WaitForSeconds(0.2f);
        }
    }

    private void FieldofViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(raycastPos.position, radius, targetMask);

        if(rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - raycastPos.position).normalized;

            if(Vector3.Angle(raycastPos.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(raycastPos.position, target.position);

                if(!Physics.Raycast(raycastPos.position,directionToTarget,distanceToTarget,obstacleMask))
                {
                    canSeePlayer = true;
                }
                else
                {
                    canSeePlayer = false;
                }
            }
            else
            {
                canSeePlayer = false;
            }
        }
        else if(canSeePlayer)
        {
            canSeePlayer = false;
        }
    }
}
