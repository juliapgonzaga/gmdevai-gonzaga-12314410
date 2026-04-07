using UnityEngine;

public class AvatarMovement : MonoBehaviour
{
    public float moveSpeed = 5f;

    void Update()
    {
        Vector3 move = Vector3.zero;

        // wasd movement 
        if (Input.GetKey(KeyCode.W))
            move += transform.forward;

        if (Input.GetKey(KeyCode.S))
            move -= transform.forward;

        if (Input.GetKey(KeyCode.A))
            move -= transform.right; // strafe left

        if (Input.GetKey(KeyCode.D))
            move += transform.right; // strafe right

        transform.position += move.normalized * moveSpeed * Time.deltaTime;

        // mouse rotation
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, transform.position);

        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            Vector3 direction = point - transform.position;

            float angle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, angle, 0);
        }
    }
}