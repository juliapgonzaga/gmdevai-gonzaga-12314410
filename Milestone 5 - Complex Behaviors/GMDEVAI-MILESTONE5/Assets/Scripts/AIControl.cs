using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class AIControl : MonoBehaviour
{
    public enum AgentBehavior
    {
        Pursue, Hide, Evade
    }
    public AgentBehavior behavior;

    NavMeshAgent agent;
    public GameObject target;
    public WASDMovement playerMovement;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        playerMovement = target.GetComponent<WASDMovement>();
    }

    void Seek(Vector3 location) // zombie seeks police
    {
        agent.SetDestination(location);
    }

    void Flee(Vector3 location) // zombie flees from police
    {
        Vector3 fleeDirection = location - this.transform.position;
        agent.SetDestination(this.transform.position - fleeDirection);
    }

    void Pursue() // zombie pursues police by predicting where police will be 
    {
        Vector3 targetDirection = target.transform.position - this.transform.position;
        float lookAhead = targetDirection.magnitude / (agent.speed + playerMovement.currentSpeed);
        Seek(target.transform.position + targetDirection * lookAhead);
    }

    void Evade() // police evades zombie by predicting where zombie will be
    {
        Vector3 targetDirection = target.transform.position - this.transform.position;
        float lookAhead = targetDirection.magnitude / (agent.speed + playerMovement.currentSpeed);
        Flee(target.transform.position + target.transform.forward * lookAhead);
    }

    Vector3 wanderTarget;

    void Wander() // agent wanders around navmesh area
    {
        float wanderRadius = 20;
        float wanderDistance = 10;
        float wanderJitter = 1;

        wanderTarget += new Vector3(Random.Range(-1f, 1f) * wanderJitter, 0, Random.Range(-1f, 1f) * wanderJitter);
        wanderTarget.Normalize();
        wanderTarget *= wanderRadius;

        Vector3 targetLocal = wanderTarget + new Vector3(0, 0, wanderDistance);
        Vector3 targetWorld = this.gameObject.transform.TransformPoint(targetLocal); 

        Seek(targetWorld);
    }

    void Hide() // uses objects with Hide tags to hide but does not check if the hiding spot is actually hiding the agent from the target hidden or not
    {
        float distance = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;

        int hidingSpotsCount = World.Instance.GetHidingSpots().Length;
        for (int i = 0; i < hidingSpotsCount; i++)
        {
            Vector3 hideDirection = World.Instance.GetHidingSpots()[i].transform.position - target.transform.position;
            Vector3 hidePosition = World.Instance.GetHidingSpots()[i].transform.position + hideDirection.normalized * 5;

            float spotDistance = Vector3.Distance(this.transform.position, hidePosition);
            if (spotDistance < distance)
            {
                chosenSpot = hidePosition;
                distance = spotDistance;
            }

            Seek(chosenSpot);
        }
    }

    void CleverHide() // uses objects with Hide tags to hide but also checks if the hiding spot is actually hiding the agent from the target 
    {
        float distance = Mathf.Infinity;
        Vector3 chosenSpot = Vector3.zero;
        Vector3 chosenDir = Vector3.zero;
        GameObject chosenGameObject = World.Instance.GetHidingSpots()[0];

        int hidingSpotsCount = World.Instance.GetHidingSpots().Length;
        for (int i = 0; i < hidingSpotsCount; i++)
        {
            Vector3 hideDirection = World.Instance.GetHidingSpots()[i].transform.position - target.transform.position;
            Vector3 hidePosition = World.Instance.GetHidingSpots()[i].transform.position + hideDirection.normalized * 5;

            float spotDistance = Vector3.Distance(this.transform.position, hidePosition);
            if (spotDistance < distance)
            {
                chosenSpot = hidePosition;
                chosenDir = hideDirection;
                chosenGameObject = World.Instance.GetHidingSpots()[i];
                distance = spotDistance;
            }

            Collider hideCol = chosenGameObject.GetComponent<Collider>();
            Ray back = new Ray(chosenSpot, -chosenDir.normalized);
            RaycastHit info;
            float rayDistance = 100f;
            hideCol.Raycast(back, out info, rayDistance);

            Seek(info.point + chosenDir.normalized * 5);
        }
    }

    bool CanSeeTarget()
    {
        RaycastHit raycastInfo;
        Vector3 rayToTarget = target.transform.position - this.transform.position;
        if (Physics.Raycast(this.transform.position, rayToTarget, out raycastInfo))
        {
            return raycastInfo.transform.gameObject.tag == "Player";
        }
        return false;
    }

    bool IsTargetInRange()
    {
        if (target == null)
        {
            return false;
        }

        float distanceToTarget = Vector3.Distance(this.transform.position, target.transform.position);
        return distanceToTarget <= 10f;
    }

    bool IsTargetFacingAgent() 
    {
        if (target == null)
        {
            return false;
        }

        Vector3 directionToAgent = (this.transform.position - target.transform.position).normalized;
        float dot = Vector3.Dot(target.transform.forward, directionToAgent);

        return dot > 0.5f;
    }

    bool CanTargetSeeAgent() 
    {
        if (target == null)
        {
            return false;
        }

        RaycastHit raycastInfo;
        Vector3 rayToAgent = this.transform.position - target.transform.position;

        if (Physics.Raycast(target.transform.position, rayToAgent.normalized, out raycastInfo, rayToAgent.magnitude))
        {
            return raycastInfo.transform.gameObject == this.gameObject;
        }

        return false;
    }

    // Update is called once per frame
    void Update()
    {
        if (target == null)
        {
            Wander();
            return;
        }

        if (!IsTargetInRange())
        {
            Wander();
            return;
        }

        switch (behavior)
        {
            case AgentBehavior.Pursue:
                Pursue(); 
                break;

            case AgentBehavior.Hide:
                Hide();
                break;

            case AgentBehavior.Evade:
                Evade(); 
                break;
        }
    }
}





