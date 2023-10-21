using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controller : MonoBehaviour
{
    private void OnEnable()
    {
        Main.main.scriptExecution.controllers.Add(this);
    }
    private void OnDisable()
    {
        Main.main.scriptExecution.controllers.Remove(this);
    }

    //Update Functions
    public virtual void movement()
    {
        //input, state flow, movement, environment collision
        //main animation
    }

    public virtual void interactionDetection()
    {

    }

    public virtual void finalizeFrame()
    {
        //interaction response
    }
}
