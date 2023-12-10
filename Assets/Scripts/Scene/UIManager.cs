using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI lifeCountText;
    public TextMeshProUGUI timerText;
    public GameObject noLivesBlock;
    public string sceneToLoad; // Имя сцены для загрузки

    private void Update()
    {
        UpdateLifeUI();
        UpdateTimerUI();
    }

    private void UpdateLifeUI()
    {
        int lifeCount = LifeManager.Instance.GetLifeCount();
        lifeCountText.text = lifeCount.ToString();
    }

    private void UpdateTimerUI()
    {
        int lifeCount = LifeManager.Instance.GetLifeCount();

        if (lifeCount >= LifeManager.maxLives)
        {
            timerText.text = "Full";
        }
        else
        {
            DateTime nextLifeTime = LifeManager.Instance.GetNextLifeTime();
            if (DateTime.Now < nextLifeTime)
            {
                TimeSpan timeLeft = nextLifeTime - DateTime.Now;
                timerText.text = string.Format("{0:D2}:{1:D2}", timeLeft.Minutes, timeLeft.Seconds);
            }
            else
            {
                timerText.text = "Full";
                LifeManager.Instance.UpdateLifeTimer();
            }
        }
    }

    public void TryStartGame()
    {
        if (LifeManager.Instance.GetLifeCount() > 0)
        {
            LifeManager.Instance.UseLife();
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            noLivesBlock.SetActive(true);
        }
    }

    public void TryToLoadGame()
    {
        if (GameDataController.HasSavedGame())
        {
            GameDataController.SetShouldLoadGame(true);
            SceneManager.LoadScene(sceneToLoad);
        }
        else
        {
            TryStartGame(); // Если нет сохраненных данных, начинаем новую игру
        }
    }
}
