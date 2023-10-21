using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class hurtbox : MonoBehaviour
    {
        public characterBase c;
        public bodyPart bodyPart;

        public void receiveAttack(Vector3 position, Vector3 direction, float damage, float strength)
        {
            c.controller.receiveAttack(position, direction, damage, strength, this);
        }
    }
    public enum bodyPart
    {
        arm, leg, body, head
    }