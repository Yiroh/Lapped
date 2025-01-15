using UnityEngine;
using UnityEngine.AI;

public class CarMovement : MonoBehaviour
{
    public Transform[] waypoints; // Array of waypoints to follow
    private NavMeshAgent agent;
    private int currentWaypointIndex = 0;
    private bool waypointsAssigned = false; // Check if waypoints have been assigned

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component is missing on this GameObject!");
        }
        else
        {
            Debug.Log($"Agent is on NavMesh: {agent.isOnNavMesh}");
        }
    }

    void Update()
    {
        // Wait until waypoints are assigned before proceeding
        if (!waypointsAssigned || waypoints.Length == 0)
            return;

        // Check if the NavMeshAgent is on a valid NavMesh and move to next waypoint
        if (agent.isOnNavMesh && !agent.pathPending && agent.remainingDistance < 0.5f)
        {
            MoveToNextWaypoint();
        }
    }

    public void AssignWaypoints(Transform[] assignedWaypoints)
    {
        if (assignedWaypoints == null)
        {
            Debug.LogError("Assigned waypoints array is null!");
            return;
        }

        if (assignedWaypoints.Length == 0)
        {
            Debug.LogError("Assigned waypoints array is empty!");
            return;
        }

        waypoints = assignedWaypoints;
        waypointsAssigned = true;

        if (agent != null && agent.isOnNavMesh)
        {
            MoveToNextWaypoint();
        }
        else
        {
            Debug.LogError("NavMeshAgent is null or not on a NavMesh!");
        }
    }

    void MoveToNextWaypoint()
    {
        if (waypoints.Length == 0 || !agent.isOnNavMesh)
            return;

        agent.destination = waypoints[currentWaypointIndex].position;
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
    }

    public void SetStartWaypointIndex(int index)
    {
        currentWaypointIndex = index;
    }
}
