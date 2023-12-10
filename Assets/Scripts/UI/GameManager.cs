using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro; // ��� ������ � TextMeshPro

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public GameObject[] planetPrefabs; // ������ �������� ������
    private bool shouldLoadSavedGame = false;
    public GameObject endGameCanvas; // UI �������, ������������ ��� ���������� ����
    public TextMeshProUGUI endGameScoreText; // ��������� ������� ��� ����������� ����� ��� Game Over


    public TextMeshProUGUI currentScoreText; // ��������� ���� �������� �����
   // public GameObject endGameCanvas; // ������, ������������ ��� ���������� ����
  //  public TextMeshProUGUI endGameScoreText; // ��������� ���� ��� ����� �� ������� ���������� ����
    public string nextSceneName; // �������� ��������� �����


    void Awake()
    {
        // ��������� ���������
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
            GameDataController.SetShouldLoadGame(false); // ����� ����� ����� ��������
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
            // ��������� ����� � ������� ������
            if (endGameScoreText != null)
            {
                endGameScoreText.text = "Score: " + ScoreManager.Instance.GetScore().ToString();
            }

            endGameCanvas.SetActive(true);
            Time.timeScale = 0; // ������������� ����
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
            ScoreManager.Instance.ResetScore(); // ����� �����
            ReloadCurrentScene();
        }
        else
        {
            LoadScene("MainMenu"); // �������� ����� �������� ����
        }
    }

    public void ContinueGame()
    {
        // �������� ���� ��������, �� ����������� � ���� NonGameOver
        foreach (var prefab in FindObjectsOfType<PrefabScript>())
        {
            if (!prefab.IsInSafeZone())
            {
                Destroy(prefab.gameObject);
            }
        }

        // ������ UI GameOver � ���������� ����
        if (endGameCanvas != null)
        {
            endGameCanvas.SetActive(false);
        }
        Time.timeScale = 1; // ������������ ����
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    // ���������� ��� ������������ � �������� � ����� "GameOver"
/*    public void EndGame()
    {
        endGameCanvas.SetActive(true); // ���������� ������ ���������� ����
        endGameScoreText.text = "Score: " + currentScoreText.text; // ��������� ����
        //Time.timeScale = 0; // ������������� ����
    }*/

    // ����� ��� ��������� ���� (��������, ��� �������� ��������)
    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    // ����� ��� ������������� ����
    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    // ����� ��� �������� �� ������ �����
/*    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1; // ������������ ����� ����� ���������
        SceneManager.LoadScene(sceneName);
    }*/

    // ����� ��� �������� �� ��������� �����
    public void LoadNextScene()
    {
        LoadScene(nextSceneName);
//>>>>>>> parent of b0a21ad (Ads Update v.0.3)
    }
}
