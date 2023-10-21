using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace santaFlappy
{
    public class santaSleigh : MonoBehaviour
    {
        public float acceleration, drag;

        public sleigh sleigh;
        public reindeer reindeer;
        public leadReindeer leadReindeer;

        public void reset()
        {
            sleigh.reset();
            reindeer.reset();
            leadReindeer.reset();
        }

        private void FixedUpdate()
        {
            if (Keyboard.current.spaceKey.isPressed)
            {
                Vector3 Acceleration = Vector3.zero;
                Acceleration.y = acceleration;
                Acceleration.y -= leadReindeer.rb.velocity.y * drag;

                leadReindeer.rb.AddForce(Acceleration, ForceMode.Acceleration);
            }
            else
            {
                Vector3 Acceleration = Vector3.zero;
                Acceleration.y = -acceleration;
                Acceleration.y -= leadReindeer.rb.velocity.y * drag;

                leadReindeer.rb.AddForce(Acceleration, ForceMode.Acceleration);
            }
        }
    }
}