using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class fpsDisplay : MonoBehaviour
{
    public TextMeshProUGUI text;

    private void Update()
    {
        if (Time.timeScale > 0)
        text.text = Mathf.CeilToInt(1 / Time.deltaTime) + " fps";
    }
}
