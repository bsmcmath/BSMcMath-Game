using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace santaFlappy
{
    public class santaFlappyGM : MonoBehaviour
    {
        public static santaFlappyGM gm;

        public overlay overlay;
        public santaSleigh santaSleigh;
        public obstacles obstacles;

        public gamePhase phase;
        public enum gamePhase
        {
            standby, countdown, playing, gameover
        }

        public float timer;
        public int score;

        private void Awake()
        {
            gm = this;
            overlay.reset();
        }

        private void FixedUpdate()
        {
            switch (phase)
            {
                case gamePhase.standby: standby(); break;
                case gamePhase.countdown: countdown(); break;
                case gamePhase.playing: playing(); break;
                case gamePhase.gameover: gameover(); break;
            }
        }

        public void standby()
        {
            if (Keyboard.current.spaceKey.isPressed)
            {
                phase = gamePhase.countdown;
                timer = 3;
            }
        }
        public void countdown()
        {
            timer -= Time.fixedDeltaTime;
            overlay.caption.text = ((int)timer + 1).ToString();
            if (timer < 0)
            {
                overlay.caption.text = "go";
                phase = gamePhase.playing;
                timer = 0;
                score = 0;
                santaSleigh.enabled = true;
                obstacles.enabled = true;
            }
        }
        public void playing()
        {
            timer += Time.fixedDeltaTime;
            if (timer > 1) overlay.caption.text = "";
            score = (int)timer;
            overlay.score.text = score.ToString();
            Physics.Simulate(Time.fixedDeltaTime);
        }
        public void gameover()
        {
            timer += Time.fixedDeltaTime;

            if (Keyboard.current.spaceKey.isPressed && timer > 1)
            {

            }
        }
    }
}