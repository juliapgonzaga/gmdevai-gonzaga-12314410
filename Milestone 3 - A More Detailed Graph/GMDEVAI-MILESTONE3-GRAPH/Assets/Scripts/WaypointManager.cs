using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    public GameObject[] waypoints;
    public Link[] links;
    public Graph graph = new Graph();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    /*void Start()
    {
        if (waypoints.Length > 0)
        {
            foreach (GameObject wp in waypoints)
            {
                graph.AddNode(wp);
            }

            foreach (Link l in links)
            {
                graph.AddEdge(l.node1, l.node2);
                if (l.dir == Link.direction.BI)
                {
                    graph.AddEdge(l.node2, l.node1);
                }
            }
        }

    }*/

    void Start()
    {
        Debug.Log("=== WAYPOINT ARRAY ORDER ===");

        for (int index = 0; index < waypoints.Length; index++)
        {
            Debug.Log(index + ": " + waypoints[index].name);
        }

        if (waypoints.Length > 0)
        {
            foreach (GameObject wp in waypoints)
            {
                graph.AddNode(wp);
            }

            foreach (Link l in links)
            {
                graph.AddEdge(l.node1, l.node2);

                if (l.dir == Link.direction.BI)
                {
                    graph.AddEdge(l.node2, l.node1);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        graph.debugDraw();
    }

    [System.Serializable]

    public struct Link
    {
        public enum direction { UNI, BI };
        public GameObject node1;
        public GameObject node2;
        public direction dir;
    }
}