using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace forthright
{
    public class effects : MonoBehaviour
    {
        public ParticleSystem hit;
        public ParticleSystem stepDust;
        public ParticleSystem kickDust;
        public ParticleSystem splash;

        public stepEffects stepFX()
        {
            stepEffects stepEffects = new();
            stepEffects.splashEffect = splash;
            stepEffects.plantEffect = stepDust;
            stepEffects.kickEffect = kickDust;
            stepEffects.plantSound = GM.gm.soundEffects.bootStep;
            return stepEffects;
        }
    }
}