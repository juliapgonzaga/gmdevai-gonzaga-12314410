using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public float timeLeft = 40f;

    public int catsCollected = 0;
    public int totalCats = 40;

    public TextMeshProUGUI timerText;
    public TextMeshProUGUI catsText;
    public TextMeshProUGUI resultText;

    private bool gameEnded = false;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        UpdateTimerText();
        UpdateCatsText();

        if (resultText != null)
        {
            resultText.text = "";
        }
    }

    void Update()
    {
        if (gameEnded == true)
        {
            return;
        }

        timeLeft -= Time.deltaTime;

        if (timeLeft < 0)
        {
            timeLeft = 0;
        }

        UpdateTimerText();

        if (timeLeft <= 0)
        {
            LoseGame();
        }
    }

    void UpdateTimerText()
    {
        if (timerText != null)
        {
            timerText.text = "Time Left: " + Mathf.CeilToInt(timeLeft).ToString();
        }
    }

    void UpdateCatsText()
    {
        if (catsText != null)
        {
            catsText.text = "Cats Collected: " + catsCollected + " / " + totalCats;
        }
    }

    public void AddCollectedCat()
    {
        if (gameEnded == true)
        {
            return;
        }

        catsCollected++;
        UpdateCatsText();

        if (catsCollected >= totalCats)
        {
            WinGame();
        }
    }

    public bool IsGameEnded()
    {
        return gameEnded;
    }
    void WinGame()
    {
        gameEnded = true;

        if (resultText != null)
        {
            resultText.text = "Bruitus collected all the CATS!";
        }
    }

    void LoseGame()
    {
        gameEnded = true;

        if (resultText != null)
        {
            resultText.text = "Time's up!";
        }
    }
}