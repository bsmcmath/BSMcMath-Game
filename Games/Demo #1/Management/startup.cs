using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace forthright
{
    public class startup : MonoBehaviour
    {
        private void Start()
        {
            sceneStreaming streaming = Main.main.streaming;
            streaming.loadScene(streaming.demo1Index + 1);
            StartCoroutine(init());
        }

        IEnumerator init()
        {
            bool waiting = true;
            while (waiting)
            {
                yield return null;
                sceneStreaming streaming = Main.main.streaming;
                if (streaming.sceneLoadStatus[streaming.demo1Index + 1] == SceneLoadStatus.loaded)
                {
                    waiting = false;
                    Destroy(gameObject);
                }
            }
        }
    }
}