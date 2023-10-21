using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace forthright
{
    public class playerBuilder : MonoBehaviour
    {
        public GameObject playerPrefab;

        public void spawnCharacter(Vector3 position)
        {
            //GameObject player = Instantiate(playerPrefab, position, Quaternion.identity);
            //GM.gm.playerCharacter = player.GetComponent<characterBase>();
            //SceneManager.MoveGameObjectToScene(player, SceneManager.GetSceneByBuildIndex(Main.main.streaming.demo1Index));
            //GM.gm.playerSpawned = true;
        }
    }
}