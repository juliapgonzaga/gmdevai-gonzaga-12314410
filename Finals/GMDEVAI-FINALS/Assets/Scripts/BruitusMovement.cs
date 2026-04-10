using UnityEngine;

public class BruitusMovement : MonoBehaviour
{
    public float moveSpeed = 6.5f;
    void Update()
    {
        if (GameManager.instance != null && GameManager.instance.IsGameEnded())
        {
            return;
        }

        Vector3 movement = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            movement += new Vector3(0, 0, 1);
        }

        if (Input.GetKey(KeyCode.S))
        {
            movement += new Vector3(0, 0, -1);
        }

        if (Input.GetKey(KeyCode.A))
        {
            movement += new Vector3(-1, 0, 0);
        }

        if (Input.GetKey(KeyCode.D))
        {
            movement += new Vector3(1, 0, 0);
        }

        transform.Translate(movement.normalized * moveSpeed * Time.deltaTime, Space.World);

        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 8f * Time.deltaTime);
        }
    }
}