using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public int score;
    public List<PlanetData> planetsData;
}

public static class GameDataController 
{
    private static string gameDataKey = "gameData";
    private static bool shouldLoadGame = false; // Флаг загрузки сохраненной игры

    public static void SaveGame(GameData data)
    {
        string jsonData = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(gameDataKey, jsonData);
        PlayerPrefs.Save();
    }

    public static GameData LoadGame()
    {
        string jsonData = PlayerPrefs.GetString(gameDataKey);
        if (!string.IsNullOrEmpty(jsonData))
        {
            return JsonUtility.FromJson<GameData>(jsonData);
        }
        return null;
    }
    public static void SetShouldLoadGame(bool value)
    {
        shouldLoadGame = value;
    }

    public static bool ShouldLoadGame()
    {
        return shouldLoadGame;
    }
    //Инкапсулируем работу с ключами сохранений из других классов.
    public static void ClearGameData()
    {
        PlayerPrefs.DeleteKey(gameDataKey);
        PlayerPrefs.Save();
    }

    public static bool HasSavedGame()
    {
        string jsonData = PlayerPrefs.GetString(gameDataKey);
        return !string.IsNullOrEmpty(jsonData) && jsonData != "{}";
    }
}
