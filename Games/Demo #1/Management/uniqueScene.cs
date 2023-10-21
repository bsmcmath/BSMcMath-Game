using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace forthright
{
    public class uniqueScene
    {
        public float activeDistance, inactiveDistance;

        public uniqueScene[] adjacentScenes;

        public int hqScene, proxyScene;

        public sceneLoadConfig sceneLoadConfig;
    }
}