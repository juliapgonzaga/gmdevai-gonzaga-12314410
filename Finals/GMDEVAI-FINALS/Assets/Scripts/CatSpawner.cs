using UnityEngine;

public class CatSpawner : MonoBehaviour
{
    public GameObject catPrefab;
    public Transform[] spawnPoints;

    public int catsToSpawn = 40;

    public Transform bruitus;
    public Transform homePoint;
    public Transform[] roamPoints;

    void Start()
    {
        SpawnCats();
    }

    void SpawnCats()
    {
        for (int catNumber = 0; catNumber < catsToSpawn; catNumber++)
        {
            int randomIndex = Random.Range(0, spawnPoints.Length);

            GameObject newCat = Instantiate(
                catPrefab,
                spawnPoints[randomIndex].position,
                Quaternion.identity
            );

            CatAIControl catAI = newCat.GetComponent<CatAIControl>();

            if (catAI != null)
            {
                catAI.bruitus = bruitus;
                catAI.homePoint = homePoint;
                catAI.roamPoints = roamPoints;
            }
        }
    }
}