using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class SavedPlanetData
{
    public int index;
    public Vector3 position;

    public SavedPlanetData(int index, Vector3 position)
    {
        this.index = index;
        this.position = position;
    }
}

public static class SaveLoadManager
{
    public static void SavePlanetData(List<SavedPlanetData> data)
    {
        for (int i = 0; i < data.Count; i++)
        {
            PlayerPrefs.SetInt($"PlanetData_{i}_index", data[i].index);
            PlayerPrefs.SetFloat($"PlanetData_{i}_x", data[i].position.x);
            PlayerPrefs.SetFloat($"PlanetData_{i}_y", data[i].position.y);
            PlayerPrefs.SetFloat($"PlanetData_{i}_z", data[i].position.z);
        }
        PlayerPrefs.SetInt("PlanetDataCount", data.Count);
        PlayerPrefs.Save();
    }

    public static List<SavedPlanetData> LoadPlanetData()
    {
        List<SavedPlanetData> data = new List<SavedPlanetData>();
        int count = PlayerPrefs.GetInt("PlanetDataCount", 0);
        for (int i = 0; i < count; i++)
        {
            int index = PlayerPrefs.GetInt($"PlanetData_{i}_index", -1);
            float x = PlayerPrefs.GetFloat($"PlanetData_{i}_x", 0);
            float y = PlayerPrefs.GetFloat($"PlanetData_{i}_y", 0);
            float z = PlayerPrefs.GetFloat($"PlanetData_{i}_z", 0);
            data.Add(new SavedPlanetData(index, new Vector3(x, y, z)));
        }
        return data;
    }
}
