using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public string sceneToLoad; // ��� ����� ��� ��������

    public void StartGame()
    {
            SceneManager.LoadScene(sceneToLoad);
    }


}
