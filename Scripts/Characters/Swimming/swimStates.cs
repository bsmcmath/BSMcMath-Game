using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class characterBase : MonoBehaviour
{
    public void swim_state(swimStateParams p)
    {
        turnOrientation(p.move.rotation);
        setRotation();
        setArma(p.cap, temp.Rotation * Quaternion.Euler(p.pose.body.highSpineEuler));

        //
        swimMove(p.move);

        skeleton.arma.position += memory.velocity * Time.fixedDeltaTime;

        terrainCollisionXZ();

        //

        poseLeftLeg(p.pose.leftLeg, temp.Rotation);
        poseRightLeg(p.pose.rightLeg, temp.Rotation);

        //

        applyBodyBaseRotation();
        poseBody(p.pose.body, temp.Rotation);
        poseLeftShoulder(p.pose.leftArm, temp.Rotation);
        poseRightShoulder(p.pose.rightArm, temp.Rotation);

        applyLook_XEuler_YRotation(p.look);

        setBodyNormals(Vector3.up);

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

[Serializable]
public class swimStateParams
{
    public capsuleSettings cap;

    public swimMoveParams move;

    public fullPose pose;

    public lookParams look;
}