using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float speed = 10f;

    public float zoomSpeed = 10f;
    public float minZoom = 20f;
    public float maxZoom = 70f;

    void Update()
    {
        Vector3 move = Vector3.zero;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            move += transform.forward;

        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            move -= transform.forward;

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            move += transform.right;

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            move -= transform.right;

        move.y = 0f;

        transform.position += move * speed * Time.deltaTime;

        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0f)
        {
            Vector3 pos = transform.position;

            pos.y -= scroll * zoomSpeed;

            pos.y = Mathf.Clamp(pos.y, minZoom, maxZoom);

            transform.position = pos;
        }
    }
}