using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    [SerializeField] float chaseRange = 20f;
    NavMeshAgent navmesh;
    [SerializeField] Transform target;
    
    float distanceToTarget = Mathf.Infinity;
    
    Idle idleState;
       


    void Start()
    {
        navmesh = GetComponent<NavMeshAgent>();
        idleState = GetComponentInChildren<Idle>();
        target = GameObject.FindGameObjectWithTag("Player").transform;

    }


    void Update()
    {

        distanceToTarget = Vector3.Distance(target.position,transform.position);
        
        if(distanceToTarget <= chaseRange)
        {
            idleState.isProvoked = true;  
        }

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,chaseRange);
    }

}
