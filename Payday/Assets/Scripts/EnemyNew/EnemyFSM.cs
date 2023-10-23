using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFSM 
{
    public EnemyState CurrentState;

    //sadece constructor'da new'leyebiliriz
    public readonly EnemyIdleState idleState;
    public readonly EnemyMoveState moveState;
    public readonly EnemyShootState shootState;
    public readonly EnemyDeadState deadState;

    public EnemyController enemyController;
    public EnemyFSM(EnemyController enemyController)
    {
        this.enemyController = enemyController;

        //Cache states for memory optimization
        idleState = new EnemyIdleState(this);
        moveState = new EnemyMoveState(this);
        shootState = new EnemyShootState(this);
        deadState = new EnemyDeadState(this);  

        //First state is always idle
        ChangeState(idleState);
    }

    public void ChangeState(EnemyState newState)
    {
        if (CurrentState != null)
            CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
    public void RunUpdate()
    {
        if(CurrentState != null)
        {
            CurrentState.Update();
        }
    }
}

public abstract class EnemyState
{
    protected EnemyFSM fsm;
    public EnemyState(EnemyFSM fsm)
    {
        this.fsm = fsm; 
    }


    public abstract void Enter();
    public abstract void Exit();
    public abstract void Update();

}