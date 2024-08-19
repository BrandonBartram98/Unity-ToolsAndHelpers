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
public class EzSceneSwitchSettings : ScriptableObject
{
    public List<string> scenePaths = new();
}

public class EzSceneSwitch : EditorWindow
{
    private EzSceneSwitchSettings settings;
    private ReorderableList reorderableList;
    private Vector2 scrollPosition;

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
        settings = AssetDatabase.LoadAssetAtPath<EzSceneSwitchSettings>("Assets/Editor/EzSceneSwitchSettings.asset");
        if (settings == null)
        {
            settings = CreateInstance<EzSceneSwitchSettings>();
            AssetDatabase.CreateAsset(settings, "Assets/Editor/EzSceneSwitchSettings.asset");
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
                Texture2D sceneIcon = AssetPreview.GetMiniThumbnail(AssetDatabase.LoadAssetAtPath<SceneAsset>(path));

                rect.y += 2;
                rect.height = EditorGUIUtility.singleLineHeight;

                if (sceneIcon != null)
                {
                    GUI.DrawTexture(new Rect(rect.x, rect.y, rect.height, rect.height), sceneIcon);
                }

                EditorGUI.LabelField(new Rect(rect.x + rect.height + 5, rect.y, rect.width - 130, rect.height), sceneName);

                // Load Scene button (additive)
                if (GUI.Button(new Rect(rect.x + rect.width - 130, rect.y, 60, rect.height), "Load", EditorStyles.miniButtonLeft))
                {
                    LoadSceneAdditive(path);
                }

                // Unload Scene button
                if (GUI.Button(new Rect(rect.x + rect.width - 70, rect.y, 60, rect.height), "Unload", EditorStyles.miniButtonRight))
                {
                    UnloadScene(sceneName);
                }
            }
        };
    }

    private void OnGUI()
    {
        GUILayout.Space(10);
        GUILayout.BeginVertical("box");

        // Limit the height of the list area to around 4 scenes
        scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Height(EditorGUIUtility.singleLineHeight * 5 + 10));

        // Draw the reorderable list
        reorderableList.DoLayoutList();

        GUILayout.EndScrollView();

        GUILayout.Space(10);

        // Buttons at the bottom of the list
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
            SaveSettings();
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
            SaveSettings();
        }
    }

    private void SaveSettings()
    {
        EditorUtility.SetDirty(settings);
        AssetDatabase.SaveAssets();
        Repaint();
    }
}