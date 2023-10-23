using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadState : EnemyState
{
    public EnemyDeadState(EnemyFSM fsm) : base(fsm)
    {
    }

    public override void Exit()
    {
        
    }

    public override void Enter()
    {
      fsm.enemyController.animator.SetBool("dead", true);
    }

    public override void Update()
    {
        
    }
}
