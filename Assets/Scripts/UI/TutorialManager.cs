using UnityEngine;

public static class TutorialManager
{
    private const string TutorialPrefKey = "TutorialsEnabled";

    public static bool TutorialsEnabled
    {
        get => PlayerPrefs.GetInt(TutorialPrefKey, 1) == 1;
        private set => PlayerPrefs.SetInt(TutorialPrefKey, value ? 1 : 0);
    }

    public static void EnableTutorials()
    {
        TutorialsEnabled = true;
        PlayerPrefs.Save();
        Debug.Log("Tutorials enabled: " + TutorialsEnabled);
    }

    public static void DisableTutorials()
    {
        TutorialsEnabled = false;
        PlayerPrefs.Save();
        Debug.Log("Tutorials enabled: " + TutorialsEnabled);
    }
}
