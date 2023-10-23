using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    public WaypointController[] waypoints;

    private int currentWaypointIndex = 0;
    [Header("NPC Animators")]
    public Animator NPCAnimator;
    public Animator NPCAnimator1;
    public Animator NPCAnimator2;
    public Animator NPCAnimator3;
    public Animator NPCAnimator4;
    public Animator NPCAnimator5;
    public Animator NPCAnimator6;
    private void Awake()
    {
        foreach (var waypoint in waypoints)
        {
            waypoint.ToggleWaypoint(false);
        }

        waypoints[currentWaypointIndex].ToggleWaypoint(true);
    }

    public void WaypointReached()
    {
        waypoints[currentWaypointIndex].ToggleWaypoint(false);
        waypoints[currentWaypointIndex].waypointReachedEvent?.Invoke();
        currentWaypointIndex++;
        if(currentWaypointIndex < waypoints.Length)
            waypoints[currentWaypointIndex].ToggleWaypoint(true);
        
    }

}
