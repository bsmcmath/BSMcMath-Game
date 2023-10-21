using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace forthright
{
    public class GM : MonoBehaviour
    {
        public static GM gm;
        private void Awake()
        {
            gm = this;
            canvas.worldCamera = Main.main.uiCamera;
        }

        public cameraManager cameraManager;

        public captions captions;
        public soundEffects soundEffects;
        public effects effects;

        public Canvas canvas;
        public pauser pause;
        public warping warping;

        //public playerBuilder playerBuilder;
        //public bool playerSpawned;
        //public characterBase playerCharacter;

        public playerTargeting playerTargeting;

        public inventory inventory;
    }
}