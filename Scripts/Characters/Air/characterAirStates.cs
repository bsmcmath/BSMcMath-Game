using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class characterBase : MonoBehaviour
{
    public void airstep_state(airStepStateParams assp)
    {
        temp.rotationTarget = assp.move.rotationOffset;
        airMove(assp.move);

        //legs


        //body
        applyBodyBaseRotation();
        poseBody(assp.pose.body, temp.Rotation);
        poseLeftShoulder(assp.pose.leftArm, temp.Rotation);
        poseRightShoulder(assp.pose.rightArm, temp.Rotation);
        applyLook_XEuler_YRotation(assp.look);

        applyAirBody(assp.body);

        setBodyNormals(Vector3.up);
        setBodyBlend(assp.bodyRotationBlend, assp.bodyPositionBlend);

        animate_TorsoHeadShoulders();
        animateLegs();

        //arms
        poseLeftArm(assp.pose.leftArm, temp.Rotation);
        anim.handLPos.blend = assp.leftArmBlend;
        animateLeftArm();

        anim.handLRot.rotation = skeleton.lowArmL.rotation;
        anim.handLRot.blend = assp.leftArmBlend;
        animateLeftHand();

        poseRightArm(assp.pose.rightArm, temp.Rotation);
        anim.handRPos.blend = assp.rightArmBlend;
        animateRightArm();

        anim.handRRot.rotation = skeleton.lowArmR.rotation;
        anim.handRRot.blend = assp.rightArmBlend;
        animateRightHand();
    }
}

[Serializable]
public class airStepStateParams
{
    public airMoveParams move;
    public fullPose pose;
    public lookParams look;
    public airBodyParams body;
    public blendSettings bodyRotationBlend, bodyPositionBlend;
    public blendSettings leftArmBlend, rightArmBlend;
}