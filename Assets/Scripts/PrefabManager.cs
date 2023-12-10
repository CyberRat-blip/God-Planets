using UnityEngine;
using System.Collections.Generic;

public class PrefabManager : MonoBehaviour
{
    public static PrefabManager Instance { get; private set; }
    public GameObject[] planetPrefabs; // Массив префабов планет
    public int[] pointsForPrefabs; // Очки за каждый префаб

    void Awake()
    {
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

    // Метод для создания следующей планеты
    public GameObject SpawnNextPrefab(Vector3 position, int prefabIndex)
    {
        if (prefabIndex >= 0 && prefabIndex < planetPrefabs.Length)
        {
            return Instantiate(planetPrefabs[prefabIndex], position, Quaternion.identity);
        }
        return null;
    }
}
