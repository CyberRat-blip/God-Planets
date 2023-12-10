using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public string sceneToLoad; // Имя сцены для загрузки

    public void StartGame()
    {
            SceneManager.LoadScene(sceneToLoad);
    }


}
