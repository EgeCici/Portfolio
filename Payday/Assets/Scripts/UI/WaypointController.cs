using UnityEngine;
using UnityEngine.Events;

public class WaypointController : MonoBehaviour
{
    WaypointManager waypointManager;

    Waypoint_Indicator waypointIndicator;

    public bool isWaypointActive;
    public UnityEvent waypointReachedEvent;

    
    private void Awake()
    {
        waypointIndicator = GetComponent<Waypoint_Indicator>();
        waypointManager = GetComponentInParent<WaypointManager>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!isWaypointActive) return;
        if (other.gameObject.CompareTag("Player"))
        {
            waypointManager.WaypointReached();
        }

        if (!waypointManager.waypoints[0].waypointIndicator.enabled)
        {
            waypointManager.NPCAnimator.SetTrigger("fear");
            waypointManager.NPCAnimator1.SetTrigger("fear");
            waypointManager.NPCAnimator2.SetTrigger("fear");
            waypointManager.NPCAnimator3.SetTrigger("fear");
            waypointManager.NPCAnimator4.SetTrigger("fear");
            waypointManager.NPCAnimator5.SetTrigger("fear");
            waypointManager.NPCAnimator6.SetTrigger("fear");
        }
    }

    public void ToggleWaypoint(bool value)
    {
        waypointIndicator.enabled = value;
        isWaypointActive = value;
    }
}
