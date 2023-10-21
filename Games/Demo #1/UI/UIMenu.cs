using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class UIMenu : MonoBehaviour
{
    public menuOption[] options;

    public int selection;

    public float timer;

    public void reset(int to)
    {
        selection = to;
        for (int i = 0; i < options.Length; i++)
        {
            options[i].deselect();
        }
        options[to].select();
    }

    public void Update()
    {
        controlSelection();
    }

    public void controlSelection()
    {
        if (timer < 0)
        {
            if (Main.main.input.inputP1.push.y > 0.5f)
            {
                int previous = selection;
                selection -= 1;
                if (selection < 0) selection = options.Length - 1;
                timer = 0.25f;
                changeSelection(previous);
            }
            else if (Main.main.input.inputP1.push.y < -0.5f)
            {
                int previous = selection;
                selection += 1;
                if (selection > options.Length - 1) selection = 0;
                timer = 0.25f;
                changeSelection(previous);
            }
        }
        else timer -= Time.unscaledDeltaTime;

        if (Main.main.input.inputP1.interact)
        {
            Main.main.input.inputP1.interact = false;
            makeSelection();
        }
        else if (Main.main.input.inputP1.crouch)
        {
            Main.main.input.inputP1.crouch = false;
            goBack();
        }
    }

    public void changeSelection(int previous)
    {
        options[previous].deselect();
        options[selection].select();
    }

    public virtual void makeSelection()
    {

    }
    public virtual void goBack()
    {

    }
}

[Serializable]
public class menuOption
{
    public TextMeshProUGUI text;

    public void deselect()
    {
        text.color = Color.black;
    }
    public void select()
    {
        text.color = Color.white;
    }
}