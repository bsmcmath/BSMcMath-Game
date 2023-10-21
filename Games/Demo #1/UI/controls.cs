using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controls : MonoBehaviour
{
    public UIMenu pausedUI;
    private void Update()
    {
        if (Main.main.input.inputP1.crouch)
        {
            Main.main.input.inputP1.crouch = false;
            gameObject.SetActive(false);
            pausedUI.gameObject.SetActive(true);
            
        }
    }
}
