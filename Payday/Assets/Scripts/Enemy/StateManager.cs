using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    [SerializeField] State currentState;
    public int damageAmount = 10;

    Attack atck;

    public Idle idle;

    private Enemy enemy;
   

    void Awake()
    {
        enemy = GetComponent<Enemy>();

        atck = GetComponentInChildren<Attack>();
        idle = GetComponentInChildren<Idle>();
        StartCoroutine(ShootGun(1f));
    }

    


    void Update()
    {
        RunStateMachine();
       
    }

    private void RunStateMachine()
    {
        State newState = currentState?.RunCurrentState();

        if(newState!=null)
        {
            SetState(newState);
        }
    }

    private void SetState(State newState)
    {
        currentState = newState;
                
    }

    IEnumerator ShootGun(float firerate)
    {
        while(enemy.isDead == false)
        {
            if(atck.isInRange && enemy.isGettingDamage == false)
            {
                enemy.animator.SetBool("attack", true);
                enemy.animator.SetBool("chase", false);
                enemy.muzlleFlash.Play();
                enemy.ejectCasing.Play();
                enemy.enemyAudio.Play();
                if (PlayerController.Instance != null && PlayerController.Instance.isDead == false)
                {
                    PlayerController.Instance.Damage(damageAmount);
                }
            }    
            yield return new WaitForSeconds(firerate);
        }
    }

}