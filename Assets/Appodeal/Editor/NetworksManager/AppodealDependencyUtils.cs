using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEditor;
using Appodeal.Editor.AppodealManager.Data;

// ReSharper disable All

namespace Appodeal.Editor.AppodealManager.AppodealDependencies
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    [SuppressMessage("ReSharper", "ReturnValueOfPureMethodIsNotUsed")]
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public static class AppodealDependencyUtils
    {
        #region Constants

        public const string PluginRequest = "https://mw-backend.appodeal.com/v2.1/unity";
        public const string AdaptersRequest = "https://mw-backend.appodeal.com/v2.1/unity/config/";
        public const string Network_configs_path = "Assets/Appodeal/Editor/NetworkConfigs/";
        public const string Replace_network_dependency_value = "com.appodeal.ads.sdk.networks:";
        public const string Replace_service_dependency_value = "com.appodeal.ads.sdk.services:";
        public const string Replace_dependency_core = "com.appodeal.ads.sdk:core:";
        public const string ReplaceAdmobDepValue = "com.appodeal.ads.sdk.networks:admob";
        public const string PackageName = "Name";
        public const string CurrentVersionHeader = "Current Version";
        public const string LatestVersionHeader = "Latest Version";
        public const string ActionHeader = "Action";
        public const string BoxStyle = "box";
        public const string ActionUpdate = "Update";
        public const string ActionImport = "Import";
        public const string ActionRemove = "Remove";
        public const string EmptyCurrentVersion = "    -  ";
        public const string AppodealUnityPlugin = "Appodeal Unity Plugin";
        public const string AppodealSdkManager = "Appodeal SDK Manager";
        public const string Appodeal = "Appodeal";
        public const string Loading = "Loading...";
        public const string ProgressBar_cancelled = "Progress bar canceled by the user";
        public const string AppodealCoreDependencies = "Appodeal Core Dependencies";
        public const string iOS = "iOS";
        public const string Android = "Android";
        public const string AppodealNetworkDependencies = "Appodeal Network Dependencies";
        public const string AppodealServiceDependencies = "Appodeal Service Dependencies";
        public const string SpecOpenDependencies = "<dependencies>\n";
        public const string SpecCloseDependencies = "</dependencies>";
        public const string XmlFileExtension = ".xml";
        public const string TwitterMoPub = "TwitterMoPub";
        public const string GoogleAdMob = "GoogleAdMob";
        public const string APDAppodealAdExchangeAdapter = "APDAppodealAdExchangeAdapter";
        public const string Dependencies = "Dependencies";

        #endregion

        public static FileInfo[] GetInternalDependencyPath()
        {
            var info = new DirectoryInfo(Network_configs_path);
            var fileInfo = info.GetFiles();

            return fileInfo.Length <= 0 ? null : fileInfo.Where(val => val.Name.EndsWith("Dependencies.xml")).ToArray();
        }

        public static void ShowInternalErrorDialog(EditorWindow editorWindow, string message, string debugLog)
        {
            EditorUtility.ClearProgressBar();
            Debug.LogError(message);
            var option = EditorUtility.DisplayDialog("Internal error",
                $"{message}. Please contact Appodeal support.",
                "Ok");
            if (option)
            {
                editorWindow.Close();
            }
        }

        public static void ShowInternalErrorDialog(EditorWindow editorWindow, string message)
        {
            EditorUtility.ClearProgressBar();
            Debug.LogError(message);
            var option = EditorUtility.DisplayDialog("Internal error",
                $"{message}.",
                "Ok");
            if (option)
            {
                editorWindow.Close();
            }
        }

        public static void FormatXml(string inputXml)
        {
            var document = new XmlDocument();
            document.Load(inputXml);
            using (var writer = new XmlTextWriter(inputXml, Encoding.UTF8))
            {
                writer.Formatting = Formatting.Indented;
                writer.Indentation = 4;
                document.Save(writer);
            }
        }

        public static string GetConfigName(string value)
        {
            var configName = value.Replace(Network_configs_path, string.Empty);
            return configName.Replace("Dependencies.xml", string.Empty);
        }

        public static string GetiOSContent(string path)
        {
            var iOSContent = string.Empty;
            var lines = File.ReadAllLines(path);
            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line)) continue;

                if (line.Contains("<iosPods>"))
                {
                    iOSContent += line + "\n";
                }

                if (line.Contains("<iosPod name="))
                {
                    iOSContent += line + "\n";
                }

                if (line.Contains("<sources>"))
                {
                    iOSContent += line + "\n";
                }

                if (line.Contains("<source>"))
                {
                    iOSContent += line + "\n";
                }

                if (line.Contains("</sources>"))
                {
                    iOSContent += line + "\n";
                }

                if (line.Contains("</iosPod>"))
                {
                    iOSContent += line + "\n";
                }

                if (line.Contains("</iosPods>"))
                {
                    iOSContent += line;
                }
            }

            return iOSContent;
        }

        public static string GetAndroidContent(string path)
        {
            var iOSContent = string.Empty;
            var lines = File.ReadAllLines(path);
            foreach (var line in lines)
            {
                if (string.IsNullOrEmpty(line)) continue;

                if (line.Contains("<androidPackages>"))
                {
                    iOSContent += line + "\n";
                }

                if (line.Contains("<androidPackage spec="))
                {
                    iOSContent += line + "\n";
                }

                if (line.Contains("<repositories>"))
                {
                    iOSContent += line + "\n";
                }

                if (line.Contains("<repository>"))
                {
                    iOSContent += line + "\n";
                }

                if (line.Contains("</repositories>"))
                {
                    iOSContent += line + "\n";
                }

                if (line.Contains("</androidPackages>"))
                {
                    iOSContent += line;
                }
            }

            return iOSContent;
        }

        public static string GetAndroidDependencyName(string value)
        {
            return value.Substring(value.IndexOf(':') + 1, value.LastIndexOf(':') - value.IndexOf(':') - 1);
        }

        public static string GetAndroidDependencyVersion(string value)
        {
            string androidDependencyVersion = value.Substring(value.LastIndexOf(':') + 1);
            if (androidDependencyVersion.Contains("@aar"))
            {
                androidDependencyVersion = androidDependencyVersion.Substring(0,
                    androidDependencyVersion.IndexOf('@'));
            }

            return androidDependencyVersion;
        }

        public static string GetMajorVersion(string value)
        {
            return value.Substring(0, 6).Remove(0, 5).Insert(0, string.Empty);
        }

        public static string GetAndroidDependencyCoreVersion(string value)
        {
            var androidDependencyVersion =
                value.Replace(Replace_dependency_core, string.Empty);
            if (androidDependencyVersion.Contains("@aar"))
            {
                androidDependencyVersion = androidDependencyVersion.Substring(0,
                    androidDependencyVersion.LastIndexOf("@", StringComparison.Ordinal));
            }

            return androidDependencyVersion;
        }

        public static string ReplaceBetaVersion(string value)
        {
            return Regex.Replace(value, "-Beta", string.Empty);
        }

        public static void ReplaceInFile(
            string filePath, string searchText, string replaceText)
        {
            string contentString;
            using (var reader = new StreamReader(filePath))
            {
                contentString = reader.ReadToEnd();
                reader.Close();
            }

            contentString = Regex.Replace(contentString.Replace("\r", ""), searchText, replaceText);

            using (var writer = new StreamWriter(filePath))
            {
                writer.Write(contentString);
                writer.Close();
            }
        }
#if UNITY_2018_1_OR_NEWER
        public static int CompareVersion(string interal, string latest)
        {
            var xParts = interal.Split('.');
            var yParts = latest.Split('.');
            var partsLength = Math.Max(xParts.Length, yParts.Length);
            if (partsLength <= 0) return string.Compare(interal, latest, StringComparison.Ordinal);
            for (var i = 0; i < partsLength; i++)
            {
                if (xParts.Length <= i) return -1;
                if (yParts.Length <= i) return 1;
                var xPart = xParts[i];
                var yPart = yParts[i];
                if (string.IsNullOrEmpty(xPart)) xPart = "0";
                if (string.IsNullOrEmpty(yPart)) yPart = "0";
                if (!int.TryParse(xPart, out var xInt) || !int.TryParse(yPart, out var yInt))
                {
                    var abcCompare = string.Compare(xPart, yPart, StringComparison.Ordinal);
                    if (abcCompare != 0)
                        return abcCompare;
                    continue;
                }

                if (xInt != yInt) return xInt < yInt ? -1 : 1;
            }

            return 0;
        }
#endif

        public static void GuiHeaders(GUIStyle headerInfoStyle, GUILayoutOption btnFieldWidth)
        {
            using (new EditorGUILayout.HorizontalScope(GUILayout.ExpandWidth(false)))
            {
                GUILayout.Button(PackageName, headerInfoStyle, GUILayout.Width(150));
                GUILayout.Space(25);
                GUILayout.Button(CurrentVersionHeader, headerInfoStyle, GUILayout.Width(110));
                GUILayout.Space(90);
                GUILayout.Button(LatestVersionHeader, headerInfoStyle);
                GUILayout.Button(ActionHeader, headerInfoStyle, btnFieldWidth);
                GUILayout.Button(string.Empty, headerInfoStyle, GUILayout.Width(5));
            }
        }

        public static AppodealDependency GetAppodealDependency(SortedDictionary<string, AppodealDependency> dependencies)
        {
            return dependencies.FirstOrDefault(dep => dep.Key.Contains(Appodeal) && dep.Value != null).Value;
        }
    }

    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public static class JsonHelper
    {
        public static T[] FromJson<T>(string json)
        {
            var wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
            return wrapper.Items;
        }

        public static string ToJson<T>(T[] array)
        {
            var wrapper = new Wrapper<T> {Items = array};
            return JsonUtility.ToJson(wrapper);
        }

        public static string ToJson<T>(T[] array, bool prettyPrint)
        {
            var wrapper = new Wrapper<T>();
            wrapper.Items = array;
            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        public static string fixJson(string value)
        {
            value = "{\"Items\":" + value + "}";
            return value;
        }

        [Serializable]
        private class Wrapper<T>
        {
            public T[] Items;
        }
    }
}
