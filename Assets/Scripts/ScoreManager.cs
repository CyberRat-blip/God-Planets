using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public TextMeshProUGUI scoreText;
    private int score = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreText();
    }

    public void SetScore(int newScore)
    {
        score = newScore;
        UpdateScoreText();
    }

    public void ResetScore()
    {
        score = 0;
        UpdateScoreText();
    }
    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }

    public int GetScore()
    {
        return score;
    }
}
