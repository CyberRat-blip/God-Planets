using UnityEngine;

public class MusicManagerHelper : MonoBehaviour
{
    public void ToggleMusic(bool isPlaying)
    {
        if (isPlaying)
            MusicManager.Instance.PlayMusic();
        else
            MusicManager.Instance.StopMusic();
    }
}
