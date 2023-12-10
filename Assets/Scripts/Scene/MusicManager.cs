using UnityEngine;

public class MusicManager : MonoBehaviour
{
    private const string MusicPrefKey = "MusicEnabled";
    public static MusicManager Instance { get; private set; }
    private AudioSource audioSource;

    public static bool IsMusicPlaying
    {
        get => PlayerPrefs.GetInt(MusicPrefKey, 1) == 1;
        private set => PlayerPrefs.SetInt(MusicPrefKey, value ? 1 : 0);
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
            RestoreMusicState();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic()
    {
        if (audioSource != null)
        {
            audioSource.Play();
            IsMusicPlaying = true;
            PlayerPrefs.Save();
        }
    }

    public void StopMusic()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
            IsMusicPlaying = false;
            PlayerPrefs.Save();
        }
    }

    private void RestoreMusicState()
    {
        if (IsMusicPlaying)
        {
            audioSource.Play();
        }
        else
        {
            audioSource.Stop();
        }
    }
}
