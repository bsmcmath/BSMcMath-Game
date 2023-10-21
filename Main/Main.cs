using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{
    public static Main main;
    private void Awake()
    {
        main = this;
    }

    public input input;
    public sceneStreaming streaming;
    public scriptExecution scriptExecution;
    public new camera camera;
    public Camera uiCamera;
    public settings settings;
    public layers layers;
    public shaderInteraction shaderInteract;
}
