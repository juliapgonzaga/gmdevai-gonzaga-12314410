using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float speed = 10f;

    public float zoomSpeed = 10f;
    public float minZoom = 10f;
    public float maxZoom = 30f;

    private Vector3 lastMousePosition; 

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - lastMousePosition;

            Vector3 move = new Vector3(-delta.x, 0f, -delta.y); 

            transform.Translate(move * speed * Time.deltaTime, Space.Self);

            lastMousePosition = Input.mousePosition;
        }

        float scroll = Input.mouseScrollDelta.y;

        if (scroll != 0f)
        {
            Vector3 pos = transform.position;

            pos.y -= scroll * zoomSpeed;

            pos.y = Mathf.Clamp(pos.y, minZoom, maxZoom);

            transform.position = pos;
        }
    }
}