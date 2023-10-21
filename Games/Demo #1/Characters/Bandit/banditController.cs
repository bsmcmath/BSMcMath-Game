using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace forthright
{
    public class banditController : characterController
    {
        public bandit b;

        public override void movement()
        {
            b.anim = new();
            b.temp = new();

            if (onTarget) targeted_nextState();
            else idle_nextState();
        }
        public override void interactionDetection()
        {

        }
        public override void receiveAttack(Vector3 position, Vector3 direction, float damage, float strength, hurtbox h)
        {
            b.health -= damage;
            if (b.health < 0)
            {

            }
            else
            {

            }
            b.applyRecoil(position, direction, strength);

        }
        public override void interact()
        {

        }
        public override void finalizeFrame()
        {

            b.anim = null;
            b.temp = null;
        }

        //
        public void idle_nextState()
        {
            b.stayGroundCheck();
            //if (GM.gm.playerSpawned) tryTargetCharacter(b, GM.gm.playerCharacter, 10);

            if (onTarget) targeted_movement();
            else idle_movement();
        }
        public void idle_movement()
        {
            b.push_orientLook();

            b.basicStep();
        }
        public void targeted_nextState()
        {
            b.stayGroundCheck();
            targetDistanceCheck(b, 10);

            if (onTarget) targeted_movement();
            else idle_movement();
        }
        public void targeted_movement()
        {
            b.target_orientLook(target);

            b.basicStep();
        }
    }
}