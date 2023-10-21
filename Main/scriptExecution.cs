using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptExecution : MonoBehaviour
{
    public List<controller> controllers;

    private void FixedUpdate()
    {
        for (int i = 0; i < controllers.Count; i++) controllers[i].movement();

        Physics.Simulate(Time.fixedDeltaTime);

        for (int i = 0; i < controllers.Count; i++) controllers[i].interactionDetection();

        for (int i = 0; i < controllers.Count; i++) controllers[i].finalizeFrame();
    }
}