
using UnityEngine;

public class EnemyShootState : EnemyState
{
    public EnemyShootState(EnemyFSM fsm) : base(fsm)
    {
    }
    public override void Enter()
    {
        fsm.enemyController.Shoot();

        fsm.ChangeState(fsm.moveState);
    }
    public override void Exit()
    {

    }



    public override void Update()
    {

    }
}
