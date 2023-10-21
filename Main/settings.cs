using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class settings : MonoBehaviour
{
    public int volume = 7;
    public int cameraSensitivity = 100;
    public float timeScale = 1;
    public bool fullscreen = true;
    public bool vsync = true;

    private void Awake()
    {
        apply();
    }

    public void apply()
    {
        Screen.fullScreen = fullscreen;
        QualitySettings.vSyncCount = vsync ? 1 : 0;
    }
}
