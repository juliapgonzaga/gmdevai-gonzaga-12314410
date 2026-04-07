using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public Vector3 direction = new Vector3(8, 0, -4);
    float movementSpeed = 5;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        direction *= .01f;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        transform.Translate(direction.normalized * movementSpeed * Time.deltaTime);
    }
}
