#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace AppodealAds.Unity.Editor.Utils
{
    public class AppodealEditorSettings : ScriptableObject
    {
        [MenuItem("Appodeal/SDK Documentation")]
        public static void OpenDocumentation()
        {
            Application.OpenURL("https://docs.appodeal.com/unity/get-started?distribution=manual");
        }

        [MenuItem("Appodeal/Appodeal Homepage")]
        public static void OpenAppodealHome()
        {
            Application.OpenURL("https://appodeal.com/");
        }

        [MenuItem("Appodeal/Appodeal Settings")]
        public static void SetAdMobAppId()
        {
            AppodealInternalSettings.ShowAppodealInternalSettings();
        }

        [MenuItem("Appodeal/Remove plugin")]
        public static void RemoveAppodealPlugin()
        {
            RemoveHelper.RemovePlugin();
        }
    }
}
#endif
