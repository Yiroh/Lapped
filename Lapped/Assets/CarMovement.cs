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
            agent.autoBraking = false; // Prevent the car from stopping at each waypoint
        }
    }

    void Update()
    {
        // Wait until waypoints are assigned before proceeding
        if (!waypointsAssigned || waypoints.Length == 0)
            return;

        if (waypointsAssigned && agent != null && agent.isOnNavMesh)
        {
            // Check if the car is near the current waypoint
            if (agent.remainingDistance < 1.0f && !agent.pathPending) // Adjust threshold (e.g., 1.0f) for smoothness
            {
                MoveToNextWaypoint();
            }
        }

        if (agent != null && agent.velocity.magnitude > 0.1f)
        {
            // Smoothly rotate the car towards its velocity direction
            Quaternion targetRotation = Quaternion.LookRotation(agent.velocity.normalized);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 5.0f); // Adjust 5.0f for rotation speed
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

        // Set the next waypoint as the destination
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        agent.destination = waypoints[currentWaypointIndex].position;

        // Add a random offset to the waypoint position
        Vector3 waypointPosition = waypoints[currentWaypointIndex].position;
        float randomOffset = Random.Range(-2.0f, 2.0f); // Adjust the range for wider checkpoints
        Vector3 offsetPosition = waypointPosition + new Vector3(randomOffset, 0, randomOffset);

        agent.destination = offsetPosition;

        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
    }

    public void SetStartWaypointIndex(int index)
    {
        currentWaypointIndex = index;
    }
}
