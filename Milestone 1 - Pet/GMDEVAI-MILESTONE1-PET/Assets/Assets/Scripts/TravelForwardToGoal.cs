using UnityEngine;

public class TravelForwardToGoal : MonoBehaviour
{
    public Transform goal;
    public float speed = 5f; 
    public float currentSpeed = 0f; //
    public float acceleration = 5f; // 
    public float deceleration = 5f; //
    public float rotSpeed = 5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 lookAtGoal = new Vector3(goal.position.x, this.transform.position.y, goal.position.z);
        Vector3 direction = lookAtGoal - transform.position;

        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * rotSpeed);


        if (Vector3.Distance(lookAtGoal, transform.position) > 1)
        {
            // accelerate toward avatar chad
            currentSpeed = Mathf.Lerp(currentSpeed, speed, Time.deltaTime * acceleration);
        }
        else
        {
            // decelerate to stop
            currentSpeed = Mathf.Lerp(currentSpeed, 0f, Time.deltaTime * deceleration);
        }

        // apply movement using currentSpeed
        transform.Translate(0, 0, currentSpeed * Time.deltaTime);
    }
}
