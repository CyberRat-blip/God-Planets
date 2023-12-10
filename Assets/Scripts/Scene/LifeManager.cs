using UnityEngine;
using System;

public class LifeManager : MonoBehaviour
{
    public static LifeManager Instance;
    public static readonly int maxLives = 5;
    public int lifeCount;
    private DateTime nextLifeTime;
    private TimeSpan lifeRecoveryTime = TimeSpan.FromMinutes(30f);

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadLives();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        UpdateLifeTimer();
    }

    public int GetLifeCount()
    {
        return lifeCount;
    }

    public void UseLife()
    {
        if (lifeCount > 0)
        {
            lifeCount--;
            SaveLives();
        }
        if (lifeCount == maxLives - 1)
        {
            nextLifeTime = DateTime.Now.Add(lifeRecoveryTime);
        }
    }

    public DateTime GetNextLifeTime()
    {
        return nextLifeTime;
    }

    public void UpdateLifeTimer()
    {
        if (lifeCount < maxLives && DateTime.Now >= nextLifeTime)
        {
            lifeCount = Math.Min(lifeCount + 1, maxLives);
            nextLifeTime = DateTime.Now.Add(lifeRecoveryTime);
            SaveLives();
        }
    }

    public void AddLife()
    {
        if (lifeCount < maxLives)
        {
            lifeCount++;
            SaveLives();
        }
    }

    public void AddLives(int amount)
    {
        lifeCount = Mathf.Min(lifeCount + amount, maxLives);
        SaveLives();
    }

    private void SaveLives()
    {
        PlayerPrefs.SetInt("LifeCount", lifeCount);
        PlayerPrefs.SetString("NextLifeTime", nextLifeTime.ToString());
        PlayerPrefs.Save();
    }

    private void LoadLives()
    {
        lifeCount = PlayerPrefs.GetInt("LifeCount", maxLives);

        string nextLifeTimeString = PlayerPrefs.GetString("NextLifeTime", "");
        if (!string.IsNullOrEmpty(nextLifeTimeString))
        {
            nextLifeTime = DateTime.Parse(nextLifeTimeString);
            CheckLifeRecovery();
        }
        else
        {
            nextLifeTime = DateTime.Now;
        }
    }

    private void CheckLifeRecovery()
    {
        if (DateTime.Now >= nextLifeTime && lifeCount < maxLives)
        {
            int livesToAdd = Math.Min((int)((DateTime.Now - nextLifeTime).TotalMinutes / lifeRecoveryTime.TotalMinutes), maxLives - lifeCount);
            lifeCount += livesToAdd;
            nextLifeTime = nextLifeTime.AddMinutes(livesToAdd * lifeRecoveryTime.TotalMinutes);
            SaveLives();
        }
    }

    public void SaveGameState()
    {
        PlayerPrefs.SetInt("LifeCount", lifeCount);
        PlayerPrefs.SetString("NextLifeTime", nextLifeTime.ToString());
    }

    public void LoadGameState()
    {
        lifeCount = PlayerPrefs.GetInt("LifeCount", maxLives);
        string nextLifeTimeString = PlayerPrefs.GetString("NextLifeTime", string.Empty);

        if (!string.IsNullOrEmpty(nextLifeTimeString))
        {
            nextLifeTime = DateTime.Parse(nextLifeTimeString);
        }
        else
        {
            nextLifeTime = DateTime.Now;
        }
    }
}
