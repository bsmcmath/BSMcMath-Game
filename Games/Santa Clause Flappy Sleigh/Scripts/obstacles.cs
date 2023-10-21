using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace santaFlappy
{
    public class obstacles : MonoBehaviour
    {
        public float speed;
        public float rangeEnd, rangeStart;
        public GameObject[] objects;
        public List<GameObject> onScreen;

        private void Awake()
        {
            onScreen = new();
            for (int i = 0; i < objects.Length; i++)
            {
                onScreen.Add(objects[i]);
            }
        }

        private void FixedUpdate()
        {
            for (int i = 0; i < onScreen.Count; i++)
            {
                Transform obj = onScreen[i].transform;
                obj.position += Vector3.left * speed * Time.fixedDeltaTime;
                if (obj.position.x < rangeEnd)
                {
                    Vector3 startPosition = obj.position;
                    startPosition.x = rangeStart;
                    obj.position = startPosition;
                }
            }
        }
    }
}