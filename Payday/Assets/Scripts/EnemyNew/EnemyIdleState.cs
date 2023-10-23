using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdleState : EnemyState
{
    float scanTimer;
    public EnemyIdleState(EnemyFSM fsm) : base(fsm)
    {

    }

    public override void Enter()
    {
        scanTimer = 0;
    }
    public override void Exit()
    {

    }

    public override void Update()
    {
        scanTimer -= Time.deltaTime;
        if (scanTimer < 0)
        {
            scanTimer += fsm.enemyController.scanInterval;
            bool isInRange = fsm.enemyController.Scan();
            if (isInRange)
            {
                fsm.ChangeState(fsm.moveState);
            }
        }
    }
}
