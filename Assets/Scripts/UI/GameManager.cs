using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro; // Для работы с TextMeshPro

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject[] planetPrefabs; // Массив префабов планет
    private bool shouldLoadSavedGame = false;
    public GameObject endGameCanvas; // UI элемент, отображаемый при завершении игры
    public TextMeshProUGUI endGameScoreText; // Текстовый элемент для отображения очков при Game Over


    public TextMeshProUGUI currentScoreText; // Текстовое поле текущего счета
   // public GameObject endGameCanvas; // Канвас, отображаемый при завершении игры
  //  public TextMeshProUGUI endGameScoreText; // Текстовое поле для счета на канвасе завершения игры
    public string nextSceneName; // Название следующей сцены


    void Awake()
    {
        // Установка синглтона
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        if (GameDataController.ShouldLoadGame())
        {
            LoadSavedGame();
            GameDataController.SetShouldLoadGame(false); // Сброс флага после загрузки
        }
    }

//<<<<<<< HEAD
    public void SetLoadSavedGameFlag(bool value)
    {
        shouldLoadSavedGame = value;
    }

    void OnLevelWasLoaded(int level)
    {
        if (shouldLoadSavedGame)
        {
            Debug.Log("loadgame");
            LoadSavedGame();
            shouldLoadSavedGame = false;
        }
    }

    private void LoadSavedGame()
    {
        GameData savedData = GameDataController.LoadGame();
        if (savedData != null)
        {
            ScoreManager.Instance.SetScore(savedData.score);
            foreach (PlanetData planetData in savedData.planetsData)
            {
                GameObject prefab = planetPrefabs[planetData.prefabIndex];
                Instantiate(prefab, planetData.position, Quaternion.identity);
            }
        }
    }
    public void EndGame()
    {
        if (endGameCanvas != null)
        {
            // Обновляем текст с текущим счетом
            if (endGameScoreText != null)
            {
                endGameScoreText.text = "Score: " + ScoreManager.Instance.GetScore().ToString();
            }

            endGameCanvas.SetActive(true);
            Time.timeScale = 0; // Останавливаем игру
        }
    }

    public void SaveGame()
    {
        List<PlanetData> planetsData = new List<PlanetData>();
        foreach (PrefabScript planet in FindObjectsOfType<PrefabScript>())
        {
            planetsData.Add(new PlanetData(planet.prefabIndex, planet.transform.position));
        }

        GameData data = new GameData
        {
            score = ScoreManager.Instance.GetScore(),
            planetsData = planetsData
        };

        GameDataController.SaveGame(data);
    }

    public void RestartGame()
    {
        if (LifeManager.Instance.GetLifeCount() > 0)
        {
            LifeManager.Instance.UseLife();
            ScoreManager.Instance.ResetScore(); // Сброс счета
            ReloadCurrentScene();
        }
        else
        {
            LoadScene("MainMenu"); // Название сцены главного меню
        }
    }

    public void ContinueGame()
    {
        // Удаление всех объектов, не находящихся в зоне NonGameOver
        foreach (var prefab in FindObjectsOfType<PrefabScript>())
        {
            if (!prefab.IsInSafeZone())
            {
                Destroy(prefab.gameObject);
            }
        }

        // Скрыть UI GameOver и продолжить игру
        if (endGameCanvas != null)
        {
            endGameCanvas.SetActive(false);
        }
        Time.timeScale = 1; // Возобновляем игру
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    // Вызывается при столкновении с объектом с тегом "GameOver"
/*    public void EndGame()
    {
        endGameCanvas.SetActive(true); // Показываем канвас завершения игры
        endGameScoreText.text = "Score: " + currentScoreText.text; // Переносим счет
        //Time.timeScale = 0; // Останавливаем игру
    }*/

    // Метод для остановки игры (например, при открытии настроек)
    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    // Метод для возобновления игры
    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    // Метод для перехода на другую сцену
/*    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1; // Возобновляем время перед переходом
        SceneManager.LoadScene(sceneName);
    }*/

    // Метод для перехода на следующую сцену
    public void LoadNextScene()
    {
        LoadScene(nextSceneName);
//>>>>>>> parent of b0a21ad (Ads Update v.0.3)
    }
}
