using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shaderInteractor : MonoBehaviour
{
    public float size;
    public int index;
    private void OnEnable()
    {
        Main.main.shaderInteract.add(this);
    }
    private void OnDisable()
    {
        Main.main.shaderInteract.remove(this);
    }
}
