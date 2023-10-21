using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class settingsUI : MonoBehaviour
{
    public GameObject pauseUI;
    public settings settings;

    public menuOption[] options;
    public menuOption[] values;

    public float timer;

    public int selection;

    public bool selected;

    private void OnEnable()
    {
        updateValues();
        options[selection].deselect();
        selection = 0;
        options[selection].select();
    }
    public void updateValues()
    {
        values[0].text.text = settings.volume.ToString();
        values[1].text.text = settings.timeScale.ToString();
        values[2].text.text = settings.fullscreen ? "fullscreen" : "windowed";
        values[3].text.text = settings.vsync ? "on" : "off";
    }
    private void Update()
    {
        if (selected)
        {
            if (timer > 0) timer -= Time.unscaledDeltaTime;
            else
            {
                if (selectionUp())
                {
                    switch (selection)
                    {
                        case 0:
                            settings.volume++;
                            break;
                        case 1:
                            settings.timeScale = Mathf.Min(settings.timeScale + 0.1f, 1.5f);
                            break;
                        case 2:
                            settings.fullscreen = !settings.fullscreen;
                            Screen.fullScreen = settings.fullscreen;
                            break;
                        case 3:
                            settings.vsync = !settings.vsync;
                            QualitySettings.vSyncCount = settings.vsync ? 1 : 0;
                            break;
                    }
                    updateValues();
                }
                else if (selectionDown())
                {
                    switch (selection)
                    {
                        case 0:
                            if (settings.volume > 0) settings.volume--;
                            break;
                        case 1:
                            settings.timeScale = Mathf.Max(settings.timeScale - 0.1f, 0);
                            break;
                        case 2:
                            settings.fullscreen = !settings.fullscreen;
                            Screen.fullScreen = settings.fullscreen;
                            break;
                        case 3:
                            settings.vsync = !settings.vsync;
                            QualitySettings.vSyncCount = settings.vsync ? 1 : 0;
                            break;
                    }
                    updateValues();
                }
            }

            if (Main.main.input.inputP1.crouch)
            {
                options[selection].select();
                values[selection].deselect();
                selected = false;
                Main.main.input.inputP1.crouch = false;
            }
        }
        else
        {
            if (timer > 0) timer -= Time.unscaledDeltaTime;
            else
            {
                if (selectionUp())
                {
                    options[selection].deselect();
                    selection--;
                    if (selection < 0) selection = options.Length - 1;
                    options[selection].select();
                }
                else if (selectionDown())
                {
                    options[selection].deselect();
                    selection++;
                    if (selection > options.Length - 1) selection = 0;
                    options[selection].select();
                }
            }

            if (Main.main.input.inputP1.interact)
            {
                options[selection].deselect();
                values[selection].select();
                selected = true;
            }
            else if (Main.main.input.inputP1.crouch)
            {
                pauseUI.SetActive(true);
                gameObject.SetActive(false);
                Main.main.input.inputP1.crouch = false;
            }
        }
    }

    public bool selectionUp()
    {
        if (Main.main.input.inputP1.push.x > 0.5f || Main.main.input.inputP1.push.y > 0.5f)
        {
            timer = 0.25f;
            return true;
        }
        return false;
    }
    public bool selectionDown()
    {
        if (Main.main.input.inputP1.push.x < -0.5f || Main.main.input.inputP1.push.y < -0.5f)
        {
            timer = 0.25f;
            return true;
        }
        return false;
    }
}
