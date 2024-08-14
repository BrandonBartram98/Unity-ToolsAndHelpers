/*********************************************************************************************************************

                    ▄█    ▄   ▄████  ▄█    ▄   ▄█    ▄▄▄▄▀ ▄███▄       ▄████  ████▄ █▄▄▄▄ █▀▄▀█ 
                    ██     █  █▀   ▀ ██     █  ██ ▀▀▀ █    █▀   ▀      █▀   ▀ █   █ █  ▄▀ █ █ █ 
                    ██ ██   █ █▀▀    ██ ██   █ ██     █    ██▄▄        █▀▀    █   █ █▀▀▌  █ ▄ █ 
                    ▐█ █ █  █ █      ▐█ █ █  █ ▐█    █     █▄   ▄▀     █      ▀████ █  █  █   █ 
                     ▐ █  █ █  █      ▐ █  █ █  ▐   ▀      ▀███▀        █             █      █  
                       █   ██   ▀       █   ██                           ▀           ▀      ▀   
                                                            
**********************************************[BRANDON BARTRAM]*******************************************************                                                                      
NOTES: Made this so I can switch between scenes easier without having to navigate to art team folder
*********************************************************************************************************************/
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
//********************************************************************************************************************
[System.Serializable]
public class SceneSwitcherSettings : ScriptableObject
{
    public List<string> scenePaths = new();
}

public class EzSceneSwitch : EditorWindow
{
    private SceneSwitcherSettings settings;
    private ReorderableList reorderableList;

    // Add a menu item to open the window
    [MenuItem("Tools/Ez Scene Switch")]
    public static void ShowWindow()
    {
        GetWindow<EzSceneSwitch>("Ez Scene Switch");
    }

    private void OnEnable()
    {
        LoadSettings();
        SetupReorderableList();
    }

    private void LoadSettings()
    {
        settings = AssetDatabase.LoadAssetAtPath<SceneSwitcherSettings>("Assets/Editor/SceneSwitcherSettings.asset");
        if (settings == null)
        {
            settings = CreateInstance<SceneSwitcherSettings>();
            AssetDatabase.CreateAsset(settings, "Assets/Editor/SceneSwitcherSettings.asset");
            AssetDatabase.SaveAssets();
        }
    }

    private void SetupReorderableList()
    {
        reorderableList = new ReorderableList(settings.scenePaths, typeof(string), true, true, false, false)
        {
            drawHeaderCallback = (Rect rect) =>
            {
                EditorGUI.LabelField(rect, "Scene List", EditorStyles.boldLabel);
            },

            drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                string path = settings.scenePaths[index];
                string sceneName = System.IO.Path.GetFileNameWithoutExtension(path);

                rect.y += 2;
                rect.height = EditorGUIUtility.singleLineHeight;

                EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width - 120, rect.height), sceneName);

            // Load Scene button (additive)
            if (GUI.Button(new Rect(rect.x + rect.width - 120, rect.y, 60, rect.height), "Load", EditorStyles.miniButton))
                {
                    LoadSceneAdditive(path);
                }

            // Unload button
            if (GUI.Button(new Rect(rect.x + rect.width - 60, rect.y, 60, rect.height), "Unload", EditorStyles.miniButton))
                {
                    UnloadScene(sceneName);
                }
            }
        };
    }

    private void OnGUI()
    {
        // Add some padding and a background color
        GUILayout.Space(10);
        GUILayout.BeginVertical("box");

        // Draw the reorderable list
        reorderableList.DoLayoutList();

        // Use EditorGUILayout for buttons to ensure proper layout handling
        if (GUILayout.Button("Add Current Scene", GUILayout.Height(30)))
        {
            AddCurrentScene();
        }

        GUILayout.Space(5);

        if (GUILayout.Button("Clear Scene List", GUILayout.Height(30)))
        {
            ClearSceneList();
        }

        GUILayout.EndVertical();
    }

    private void LoadSceneAdditive(string path)
    {
        if (EditorApplication.isPlaying)
        {
            SceneManager.LoadScene(System.IO.Path.GetFileNameWithoutExtension(path), LoadSceneMode.Additive);
        }
        else
        {
            EditorSceneManager.OpenScene(path, OpenSceneMode.Additive);
        }
    }

    private void UnloadScene(string sceneName)
    {
        if (EditorApplication.isPlaying)
        {
            SceneManager.UnloadSceneAsync(sceneName);
        }
        else
        {
            Scene scene = SceneManager.GetSceneByName(sceneName);
            if (scene.isLoaded)
            {
                EditorSceneManager.CloseScene(scene, true);
            }
        }
    }

    private void AddCurrentScene()
    {
        string activeScenePath = SceneManager.GetActiveScene().path;

        if (!settings.scenePaths.Contains(activeScenePath))
        {
            settings.scenePaths.Add(activeScenePath);
            EditorUtility.SetDirty(settings);
            AssetDatabase.SaveAssets();
        }
        else
        {
            EditorUtility.DisplayDialog("Scene Already Added", "This scene is already in the list.", "OK");
        }
    }

    private void ClearSceneList()
    {
        if (EditorUtility.DisplayDialog("Clear Scene List", "Are you sure you want to clear the scene list?", "Yes", "No"))
        {
            settings.scenePaths.Clear();
            EditorUtility.SetDirty(settings);
            AssetDatabase.SaveAssets();
        }
    }
}