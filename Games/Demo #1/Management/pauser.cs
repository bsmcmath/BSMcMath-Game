using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace forthright
{
    public class pauser : MonoBehaviour
    {
        public pauseMenu menu;
        public bool paused = false;
        public virtual void FixedUpdate()
        {
            pauseCheck();
        }
        public void pauseCheck()
        {
            if (Main.main.input.inputP1.start)
            {
                Main.main.input.inputP1.start = false;
                paused = true;
                pause();
            }
        }
        public virtual void pause()
        {
            menu.gameObject.SetActive(true);
            Time.timeScale = 0;
            menu.reset(0);
        }
        public virtual void unpause()
        {
            Time.timeScale = Main.main.settings.timeScale;
        }
    }
}