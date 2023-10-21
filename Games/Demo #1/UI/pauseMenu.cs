using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace forthright
{
    public class pauseMenu : UIMenu
    {
        public GameObject controlsUI, freeLookUI, settingsUI;
        public UIMenu warpUI;
        public override void makeSelection()
        {
            switch (selection)
            {
                case 0:
                    resume();
                    break;
                case 1:
                    reset();
                    break;
                case 2:
                    warp();
                    break;
                case 3:
                    controls();
                    break;
                case 4:
                    settings();
                    break;
                case 5:
                    freeLook();
                    break;
                case 6:
                    quit();
                    break;
            }
        }
        public override void goBack()
        {
            resume();
        }
        public void resume()
        {
            GM.gm.pause.unpause();
            gameObject.SetActive(false);
        }
        public void reset()
        {
            GM.gm.pause.unpause();
            SceneManager.LoadScene(0);
        }
        public void warp()
        {
            warpUI.reset(0);
            warpUI.gameObject.SetActive(true);
            gameObject.SetActive(false);
        }
        public void controls()
        {
            controlsUI.SetActive(true);
            gameObject.SetActive(false);
        }
        public void settings()
        {
            settingsUI.SetActive(true);
            gameObject.SetActive(false);
        }
        public void freeLook()
        {
            freeLookUI.SetActive(true);
            gameObject.SetActive(false);
        }
        public void quit()
        {
            Application.Quit();
        }
    }
}