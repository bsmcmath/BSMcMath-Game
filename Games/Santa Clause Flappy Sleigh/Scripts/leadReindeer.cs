using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace santaFlappy
{
    public class leadReindeer : MonoBehaviour
    {
        public Rigidbody rb;
        public Vector3 startPosition;
        private void OnValidate()
        {
            startPosition = transform.position;
        }

        public void reset()
        {
            transform.position = startPosition;
            rb.velocity = Vector3.zero;
        }

        private void OnCollisionEnter(Collision collision)
        {

        }
    }
}