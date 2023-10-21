using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
public class BuildEditor : EditorWindow
{
    [MenuItem("Window/Build Editor")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(BuildEditor));
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Build All"))
        {
            buildAll();
        }
        if (GUILayout.Button("Build Demo 1"))
        {
            buildDemo1();
        }
        if (GUILayout.Button("Build Santa Flappy"))
        {
            buildSantaFlappy();
        }
    }

    public void buildAll()
    {
        List<SceneAsset> scenes = new();
        sceneStreaming streaming = GameObject.Find("Streaming").GetComponent<sceneStreaming>();

        addMain(scenes);
        addGameSelect(scenes, streaming);
        addDemo1(scenes, streaming);
        addSantaFlappy(scenes, streaming);

        GameObject.Find("Startup").GetComponent<startup>().onStart = startup.startupBehavior.gameSelect;
        ApplyBuildSettings(scenes, streaming);
    }
    public void buildDemo1()
    {
        List<SceneAsset> scenes = new();
        sceneStreaming streaming = GameObject.Find("Streaming").GetComponent<sceneStreaming>();

        addMain(scenes);
        addDemo1(scenes, streaming);

        GameObject.Find("Startup").GetComponent<startup>().onStart = startup.startupBehavior.demo1;
        ApplyBuildSettings(scenes, streaming);
    }
    public void buildSantaFlappy()
    {
        List<SceneAsset> scenes = new();
        sceneStreaming streaming = GameObject.Find("Streaming").GetComponent<sceneStreaming>();

        addMain(scenes);
        addSantaFlappy(scenes, streaming);

        GameObject.Find("Startup").GetComponent<startup>().onStart = startup.startupBehavior.santaFlappy;
        ApplyBuildSettings(scenes, streaming);
    }

    public void addMain(List<SceneAsset> scenes)
    {
        string[] guids = AssetDatabase.FindAssets("Main t:SceneAsset");
        string path = AssetDatabase.GUIDToAssetPath(guids[0]);
        scenes.Add(AssetDatabase.LoadAssetAtPath<SceneAsset>(path));
    }
    public void addGameSelect(List<SceneAsset> scenes, sceneStreaming streaming)
    {
        streaming.gameSelectIndex = scenes.Count;

        string[] guids = AssetDatabase.FindAssets("Game Select t:SceneAsset");
        string path = AssetDatabase.GUIDToAssetPath(guids[0]);
        scenes.Add(AssetDatabase.LoadAssetAtPath<SceneAsset>(path));
    }
    public void addDemo1(List<SceneAsset> scenes, sceneStreaming streaming)
    {
        streaming.demo1Index = scenes.Count;

        string[] guids = AssetDatabase.FindAssets("demo1 t:SceneAsset");
        foreach (string g in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(g);
            scenes.Add(AssetDatabase.LoadAssetAtPath<SceneAsset>(path));
        }
    }
    public void addSantaFlappy(List<SceneAsset> scenes, sceneStreaming streaming)
    {
        streaming.santaFlappyIndex = scenes.Count;

        string[] guids = AssetDatabase.FindAssets("santaFlappy t:SceneAsset");
        foreach (string g in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(g);
            scenes.Add(AssetDatabase.LoadAssetAtPath<SceneAsset>(path));
        }
    }

    public void ApplyBuildSettings(List<SceneAsset> scenes, sceneStreaming streaming)
    {
        streaming.sceneLoadStatus = new SceneLoadStatus[scenes.Count];
        
        List<EditorBuildSettingsScene> buildSettingsScenes = new();
        foreach (SceneAsset s in scenes)
        {
            string path = AssetDatabase.GetAssetPath(s);
            buildSettingsScenes.Add(new EditorBuildSettingsScene(path, true));
        }

        EditorBuildSettings.scenes = buildSettingsScenes.ToArray();
    }
}
#endif