using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace forthright
{
    public class cameraManager : MonoBehaviour
    {
        public cameraParams cp;

        public cameraMode mode;
        public Transform target, target2;
        public Vector3 targetPosition, targetRotation;

        public void setThirdPerson(Transform _target)
        {
            target = _target;
            mode = cameraMode.thirdPerson;
        }
        public void setIsometric(Transform _target)
        {
            target = _target;
            mode = cameraMode.isometric;
        }
        public void setDuel(Transform _target1, Transform _target2)
        {
            target = _target1; 
            target2 = _target2;
            mode = cameraMode.duel;
        }

        private void FixedUpdate()
        {
            switch (mode)
            {
                case cameraMode.thirdPerson:
                    Main.main.camera.turn_player_xy();
                    Main.main.camera.follow_cast(target, cp.thirdPersonPosition, cp.collisionRadius);
                    break;
                case cameraMode.aimRanged:

                    break;
                case cameraMode.isometric:
                    Main.main.camera.turn_player_y(cp.isometricRotation);
                    Main.main.camera.follow(target, cp.isometricPosition);
                    break;
                case cameraMode.stationary:
                    Main.main.camera.setPosition(targetPosition);
                    Main.main.camera.setRotation(Quaternion.Euler(targetRotation));
                    break;
                case cameraMode.duel:
                    Main.main.camera.turn_duel(target, target2, cp.maxDuelOffset, 0.5f, cp.duelXRotation);
                    Main.main.camera.follow(target, cp.duelPosition);
                    break;
            }
        }
    }
    public enum cameraMode
    {
        thirdPerson, aimRanged, isometric, stationary, duel
    }
}