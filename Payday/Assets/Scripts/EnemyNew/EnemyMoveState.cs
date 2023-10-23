using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveState : EnemyState
{
    public EnemyMoveState(EnemyFSM fsm) : base(fsm)
    {
    }

    private bool isHaveDestination;
    private Vector3 lastGoPosition;
    private float lookRotTime;
    public override void Enter()
    {
        fsm.enemyController.animator.SetBool("move", true);
        fsm.enemyController.agent.isStopped = false;
        //fsm.enemyController.agent.SetDestination(fsm.enemyController.player.position);
        fsm.enemyController.agent.stoppingDistance = 0;
    }
    public override void Exit()
    {
        fsm.enemyController.animator.SetBool("move", false);

        if (fsm.enemyController.agent.isOnNavMesh)
        {
            fsm.enemyController.agent.isStopped = true;
        }

    }



    public override void Update()
    {
        if (lookRotTime > 1)
        {
            lookRotTime = 0;
            isHaveDestination = false;
        }
            
        Vector3 playerPos = fsm.enemyController.player.position;
        playerPos.y = 0;
        Vector3 enemyPos = fsm.enemyController.transform.position;
        enemyPos.y = 0;
        Quaternion lookPlayer = Quaternion.LookRotation(playerPos - enemyPos);
        fsm.enemyController.transform.rotation = Quaternion.Slerp(fsm.enemyController.transform.rotation, lookPlayer, lookRotTime);
        lookRotTime += Time.deltaTime ;

        if (fsm.enemyController.agent.isOnNavMesh && !isHaveDestination)
        {
            lastGoPosition = fsm.enemyController.GetRandomPositionInPlayerView();
            fsm.enemyController.agent.SetDestination(lastGoPosition);
            isHaveDestination = true;
        }

        if (fsm.enemyController.CanShoot)
        {
            var isInSight = fsm.enemyController.IsInSight(fsm.enemyController.player.gameObject);
            var inRange = fsm.enemyController.Scan();

            
            if (isInSight && inRange)
            {
                fsm.enemyController.Shoot();
                isHaveDestination = false;

            }
           
        }

        //if (fsm.enemyController.agent.remainingDistance < fsm.enemyController.agent.stoppingDistance + 0.2f && fsm.enemyController.CanShoot)
        //{
        //    if (fsm.enemyController.IsInSight(fsm.enemyController.player.gameObject) && fsm.enemyController.Scan())
        //    {
        //        if (fsm.enemyController.SHOOT_WHEN_PLAYER_INSIGHT)
        //        {
        //            fsm.enemyController.Shoot();
        //        }
        //        else
        //        {
        //            fsm.ChangeState(fsm.shootState);
        //        }
        //    }
        //    else
        //    {
        //        isHaveDestination = false;
        //    }

        //}

    }
}
