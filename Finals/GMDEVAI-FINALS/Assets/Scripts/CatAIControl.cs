using UnityEngine;
using UnityEngine.AI;

public class CatAIControl : MonoBehaviour
{
    public enum CatBehavior
    {
        Wander,
        Flee,
        SeekHome
    }

    public CatBehavior behavior;

    public Transform bruitus;
    public Transform homePoint;
    public Transform[] roamPoints;

    public float fleeDistance = 4f; //lower if u want cats to flee more
    public float homeSeekDistance = 6f; //lower if u want cats to seek home lesser
    public float waypointReachDistance = 1.5f;

    public float neighborRadius = 3f;
    public float separationDistance = 1.2f;

    private NavMeshAgent agent;
    private int currentRoamPoint = 0;
    private bool isInsideHome = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = Random.Range(3f, 4.5f);

        if (roamPoints.Length > 0)
        {
            currentRoamPoint = Random.Range(0, roamPoints.Length);
            agent.SetDestination(roamPoints[currentRoamPoint].position);
        }
    }

    void Update()
    {
        if (isInsideHome == true)
        {
            agent.isStopped = true;
            return;
        }

        float bruitusDistance = Vector3.Distance(transform.position, bruitus.position);
        float homeDistance = Vector3.Distance(transform.position, homePoint.position);

        if (bruitusDistance < fleeDistance)
        {
            behavior = CatBehavior.Flee;
            Flee(bruitus.position);
        }
        else if (homeDistance < homeSeekDistance)
        {
            behavior = CatBehavior.SeekHome;
            Seek(homePoint.position);
        }
        else
        {
            behavior = CatBehavior.Wander;
            Wander();
        }

        SmoothRotate();
    }

    void Seek(Vector3 targetPosition)
    {
        agent.SetDestination(targetPosition);
    }

    void Flee(Vector3 dangerPosition)
    {
        Vector3 fleeDirection = transform.position - dangerPosition;

        Vector3 targetPosition = transform.position + fleeDirection.normalized * 6f;

        agent.SetDestination(targetPosition);
    }

    void Wander()
    {
        if (roamPoints.Length == 0)
        {
            return;
        }

        if (Vector3.Distance(transform.position, roamPoints[currentRoamPoint].position) < waypointReachDistance)
        {
            currentRoamPoint = Random.Range(0, roamPoints.Length);
        }

        Vector3 flockDirection = GetFlockDirection();
        Vector3 targetPosition = roamPoints[currentRoamPoint].position + flockDirection;

        agent.SetDestination(targetPosition);
    }

    Vector3 GetFlockDirection()
    {
        Collider[] nearbyObjects = Physics.OverlapSphere(transform.position, neighborRadius);

        Vector3 groupCenter = Vector3.zero;
        Vector3 separationForce = Vector3.zero;
        int nearbyCats = 0;

        foreach (Collider nearbyObject in nearbyObjects)
        {
            if (nearbyObject.gameObject != gameObject && nearbyObject.CompareTag("Cat"))
            {
                groupCenter += nearbyObject.transform.position;
                nearbyCats++;

                float distance = Vector3.Distance(transform.position, nearbyObject.transform.position);

                if (distance < separationDistance)
                {
                    separationForce += (transform.position - nearbyObject.transform.position);
                }
            }
        }

        if (nearbyCats > 0)
        {
            groupCenter /= nearbyCats;

            Vector3 moveToGroup = (groupCenter - transform.position).normalized;
            Vector3 finalFlockDirection = moveToGroup + separationForce.normalized;

            return finalFlockDirection;
        }

        return Vector3.zero;
    }

    void SmoothRotate()
    {
        Vector3 moveDirection = agent.velocity;
        moveDirection.y = 0f;

        if (moveDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 8f * Time.deltaTime);
        }
    }

    public void EnterHome()
    {
        isInsideHome = true;
        gameObject.SetActive(false);
    }
}