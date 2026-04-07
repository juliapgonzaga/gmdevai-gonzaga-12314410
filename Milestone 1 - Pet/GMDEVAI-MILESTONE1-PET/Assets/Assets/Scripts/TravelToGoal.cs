using UnityEngine;

public class TravelToGoal : MonoBehaviour
{
    public Transform goal;
    float speed = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 direction = goal.position - transform.position;

        transform.LookAt(goal);

        if (direction.magnitude > 1f)
        {
            transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);
        }
    }
}
