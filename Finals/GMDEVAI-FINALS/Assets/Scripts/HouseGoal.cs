using UnityEngine;

public class HouseGoal : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Cat"))
        {
            CatAIControl catAI = other.GetComponent<CatAIControl>();

            if (catAI != null)
            {
                catAI.EnterHome();
                GameManager.instance.AddCollectedCat();
            }
        }
    }
}