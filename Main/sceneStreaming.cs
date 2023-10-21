using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class sceneStreaming : MonoBehaviour
{
    public SceneLoadStatus[] sceneLoadStatus;

    public int gameSelectIndex;
    public int demo1Index;
    public int santaFlappyIndex;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        sceneLoadStatus[scene.buildIndex] = SceneLoadStatus.loaded;
    }
    public void OnSceneUnloaded(Scene scene)
    {
        sceneLoadStatus[scene.buildIndex] = SceneLoadStatus.unloaded;
    }

    public void loadScene(int i)
    {
        sceneLoadStatus[i] = SceneLoadStatus.loading;
        SceneManager.LoadScene(i, LoadSceneMode.Additive);
    }
    public void unloadScene(int i)
    {
        sceneLoadStatus[i] = SceneLoadStatus.unloading;
        SceneManager.UnloadSceneAsync(i);
    }
}

public enum SceneLoadStatus
{
    unloaded, loaded, loading, unloading, 
}