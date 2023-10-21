using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class characterBase : MonoBehaviour
{
    public void laying_state(layingStateParams p)
    {
        terrainVelocityAndTurn(temp.groundCheck);
        turnOrientation(p.move.rotation);
        setRotation();
        setArma(p.cap, temp.Rotation); //tilt

        stepMovementCalculations();

        stepForces(p.move);

        offGroundHeight(p.move);

        skeleton.arma.position += memory.velocity * Time.fixedDeltaTime;

        terrainCollisionXZ();

        velocityUpdate();

        //

        poseLeftLeg(p.pose.leftLeg, temp.Rotation);
        poseRightLeg(p.pose.rightLeg, temp.Rotation);

        //

        applyBodyBaseRotation();
        poseBody(p.pose.body, temp.Rotation);
        poseLeftShoulder(p.pose.leftArm, temp.Rotation);
        poseRightShoulder(p.pose.rightArm, temp.Rotation);

        applyBodyTilt(memory.actionDirection * 90);

        applyLook_XEuler_YRotation(p.look);

        setNormalsToGround(1);

        animate_TorsoHeadShoulders();
        animateLegs();

        //

        poseLeftArm(p.pose.leftArm, temp.Rotation);
        animateLeftArm();

        anim.handLRot.rotation = skeleton.lowArmL.rotation;
        animateLeftHand();

        poseRightArm(p.pose.rightArm, temp.Rotation);
        animateRightArm();

        anim.handRRot.rotation = skeleton.lowArmR.rotation;
        animateRightHand();
    }
}

public class layingStateParams
{
    public capsuleSettings cap;

    public stepMoveParams move;

    public fullPose pose;

    public lookParams look;
}