using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace forthright
{
    public class warping : MonoBehaviour
    {
        public Transform[] warpPositions;
        public int[] buildIndex;
        public int currentScene;
        public bool waiting;

        //public void warpTo(int point)
        //{
        //    if (waiting) return;

        //    //if (currentScene != -1) GM.gm.streaming.unloadScene(currentScene);
        //    currentScene = buildIndex[point];
        //    //GM.gm.streaming.loadScene(currentScene);

        //    if (GM.gm.playerSpawned) DestroyImmediate(GM.gm.playerCharacter.gameObject);
        //    GM.gm.playerSpawned = false;

        //    StartCoroutine(initialize(point));
        //}

        //IEnumerator initialize(int point)
        //{
        //    waiting = true;
        //    while (waiting)
        //    {
        //        if (GM.gm.streaming.buildScenes[currentScene].status == sceneInfo.Status.loaded)
        //        {
        //            GM.gm.playerBuilder.spawnCharacter(warpPositions[point].position);

        //            //GM.gm.streaming.autoloadCenter = GM.gm.playerCharacter.skeleton.arma;

        //            GM.gm.inventory.quick.gameObject.SetActive(true);

        //            waiting = false;
        //        }
        //        yield return null;
        //    }
        //}
    }
}