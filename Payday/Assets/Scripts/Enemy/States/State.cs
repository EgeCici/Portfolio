using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class State : MonoBehaviour
{
    public NavMeshAgent navMesh;
    public Idle idleState;

    public FieldOfView enemyfov;

    public Transform target;

    public Animator enemyAnimator;

    void Start()
    {
        navMesh = GetComponentInParent<NavMeshAgent>();
        enemyAnimator = GetComponentInParent<Animator>();
        idleState = GetComponentInParent<StateManager>().idle;
        enemyfov = GetComponentInParent<FieldOfView>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    public abstract State RunCurrentState();
}