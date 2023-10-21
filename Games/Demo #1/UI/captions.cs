using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class captions : MonoBehaviour
{
    public TextMeshProUGUI topLeft, bottom;

    public bool writing, tripled;
    public float timer;
    public float writeSpeed;
    public char[] chars;

    private void Update()
    {
        if (writing) writeCaption();
    }

    public void writeCaption()
    {
        timer += writeSpeed * Time.deltaTime;
        int totalChars = chars.Length;
        int numChars = Mathf.FloorToInt(timer);
        if (numChars > totalChars - 1) numChars = totalChars - 1;

        string text = "";
        for (int i = 0; i <= numChars; i++)
        {
            if (chars[i] != '_') text += chars[i];
        }
        bottom.text = text;

        if (numChars == totalChars - 1)
        {
            writing = false;
            tripled = false;
        }
    }
    public void startCaption(string cap, float speed)
    {
        chars = cap.ToCharArray();
        writeSpeed = speed;
        writing = true;
        tripled = false;
        timer = 0;
        bottom.text = "";
        bottom.enabled = true;
    }
    public void speedUpCheck()
    {
        if (!tripled && Main.main.input.inputP1.interact)
        {
            Main.main.input.inputP1.interact = false;
            speedUp();
        }
    }
    public void speedUp()
    {
        tripled = true;
        writeSpeed *= 3;
    }
    public void clearCaption()
    {
        writing = false;
        bottom.enabled = false;
    }
}
