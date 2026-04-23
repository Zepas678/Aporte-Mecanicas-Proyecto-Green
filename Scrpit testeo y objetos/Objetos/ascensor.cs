using UnityEngine;
using UnityEngine.AI;

public class MovimientoPlataforma : MonoBehaviour
{
    
    public Transform[] waypoints;
    public float moveSpeed = 5f; 
    public float waitTime = 1f; 

    private int currentWaypointIndex = 0;
    private bool isWaiting = false;
    private float waitTimer;

    
    private NavMeshAgent agent; 

    void Start()
    {
        
        agent = GetComponent<NavMeshAgent>();

        if (waypoints.Length == 0)
        {
            //Debug.LogError("MovimientoPlataforma: No se han asignado waypoints.");
            enabled = false;
        }
        else
        {
            
            MoveToWaypoint(0);
        }
    }

    void Update()
    {
        if (waypoints.Length == 0) return;

        
        if (isWaiting)
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0)
            {
                isWaiting = false;
                
                currentWaypointIndex++;
                if (currentWaypointIndex >= waypoints.Length)
                {
                    currentWaypointIndex = 0; 
                }
                MoveToWaypoint(currentWaypointIndex);
            }
        }
        else 
        {
            
            if (agent != null && agent.enabled)
            {
                
                if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + 0.1f)
                {
                    StartWaiting();
                }
            }
            
            else
            {
                
                transform.position = Vector3.MoveTowards(transform.position, waypoints[currentWaypointIndex].position, moveSpeed * Time.deltaTime);
                
                
                if (Vector3.Distance(transform.position, waypoints[currentWaypointIndex].position) < 0.1f)
                {
                    StartWaiting();
                }
            }
        }
    }

    void MoveToWaypoint(int index)
    {
        if (agent != null && agent.enabled)
        {
            agent.speed = moveSpeed;
            agent.SetDestination(waypoints[index].position);
        }
        else
        {
            
        }
    }
    
    void StartWaiting()
    {
        isWaiting = true;
        waitTimer = waitTime;
    }
}
