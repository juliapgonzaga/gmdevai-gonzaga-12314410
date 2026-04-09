using UnityEditor.PackageManager;
using UnityEngine;

public class AgentManager : MonoBehaviour
{

    GameObject[] agents;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agents = GameObject.FindGameObjectsWithTag("AI");

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 1000))
            {
                foreach (GameObject agent in agents)
                {
                    agent.GetComponent<AIControl>().agent.SetDestination(hit.point);
                }
            }
        }
    }
}   
