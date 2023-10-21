using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class characterBase : MonoBehaviour
{
    public void step_bare_state(stepStateParams p)
    {
        //base rotation
        temp.rotationTarget = p.move.rotationOffset;
        terrainVelocityAndTurn(temp.groundCheck);
        turnOrientation(p.move.rotation);
        setRotation();
        setArma(p.cap, temp.Rotation * Quaternion.Euler(p.pose.body.highSpineEuler));

        //movement, collision
        stepMove(p.move);

        //feet
        stepLegs(p.step, p.pose.leftLeg, p.pose.rightLeg, p.effects);
        anim.footLPos.blend = p.legsBlend;
        anim.footRPos.blend = p.legsBlend;

        //body
        stepBodyPosition(p.step, p.move);

        applyBodyBaseRotation_Step();
        poseBody(p.pose.body, temp.Rotation);
        poseLeftShoulder(p.pose.leftArm, temp.Rotation);
        poseRightShoulder(p.pose.rightArm, temp.Rotation);

        applyLook_XEuler_YRotation(p.look);

        setBodyNormals(Vector3.up);
        setBodyBlend(p.bodyRotationBlend, p.bodyPositionBlend);

        animate_TorsoHeadShoulders();
        animateLegs();

        //arms
        poseLeftArm(p.pose.leftArm, temp.Rotation);
        anim.handLPos.blend = p.leftArmBlend;
        anim.elbowLPos.blend = p.leftArmBlend;
        animateLeftArm();

        anim.handLRot.rotation = skeleton.lowArmL.rotation;
        anim.handLRot.blend = p.leftArmBlend;
        animateLeftHand();

        poseRightArm(p.pose.rightArm, temp.Rotation);
        anim.handRPos.blend = p.rightArmBlend;
        anim.elbowRPos.blend = p.rightArmBlend;
        animateRightArm();

        anim.handRRot.rotation = skeleton.lowArmR.rotation;
        anim.handRRot.blend = p.rightArmBlend;
        animateRightHand();
    }
    public void step_standard_state(stepStateParams p)
    {
        //base rotation
        temp.rotationTarget = p.move.rotationOffset;
        terrainVelocityAndTurn(temp.groundCheck);
        turnOrientation(p.move.rotation);
        setRotation();
        setArma(p.cap, temp.Rotation * Quaternion.Euler(p.pose.body.highSpineEuler));

        //movement, collision
        stepMove(p.move);

        //feet
        stepLegs(p.step, p.pose.leftLeg, p.pose.rightLeg, p.effects);
        anim.footLPos.blend = p.legsBlend;
        anim.footRPos.blend = p.legsBlend;

        //body
        stepBodyPosition(p.step, p.move);

        applyBodyBaseRotation_Step();
        poseBody(p.pose.body, temp.Rotation);
        poseLeftShoulder(p.pose.leftArm, temp.Rotation);
        poseRightShoulder(p.pose.rightArm, temp.Rotation);

        applyLook_XEuler_YRotation(p.look);

        applyStepBody(p.stepBody);

        leftShoulderSwing(p.leftArmSwing);
        rightShoulderSwing(p.rightArmSwing);

        bodyRecoil();

        setBodyNormals(Vector3.up);
        setBodyBlend(p.bodyRotationBlend, p.bodyPositionBlend);

        animate_TorsoHeadShoulders();
        animateLegs();

        //arms
        poseLeftArm(p.pose.leftArm, temp.Rotation);
        leftArmSwing(p.leftArmSwing);
        leftElbowSwing(p.leftArmSwing);
        anim.handLPos.blend = p.leftArmBlend;
        anim.elbowLPos.blend = p.leftArmBlend;
        animateLeftArm();

        anim.handLRot.rotation = skeleton.lowArmL.rotation;
        anim.handLRot.blend = p.leftArmBlend;
        animateLeftHand();

        poseRightArm(p.pose.rightArm, temp.Rotation);
        rightArmSwing(p.rightArmSwing);
        rightElbowSwing(p.rightArmSwing);
        anim.handRPos.blend = p.rightArmBlend;
        anim.elbowRPos.blend = p.rightArmBlend;
        animateRightArm();

        anim.handRRot.rotation = skeleton.lowArmR.rotation;
        anim.handRRot.blend = p.rightArmBlend;
        animateRightHand();
    }
    public void step_leftHandLook_state(stepStateParams p)
    {
        //base rotation
        temp.rotationTarget = p.move.rotationOffset;
        terrainVelocityAndTurn(temp.groundCheck);
        turnOrientation(p.move.rotation);
        setRotation();
        setArma(p.cap, temp.Rotation * Quaternion.Euler(p.pose.body.highSpineEuler));

        //movement, collision
        stepMove(p.move);

        //feet
        stepLegs(p.step, p.pose.leftLeg, p.pose.rightLeg, p.effects);
        anim.footLPos.blend = p.legsBlend;
        anim.footRPos.blend = p.legsBlend;

        //body
        stepBodyPosition(p.step, p.move);

        applyBodyBaseRotation_Step();
        poseBody(p.pose.body, temp.Rotation);
        poseLeftShoulder(p.pose.leftArm, temp.Rotation);
        poseRightShoulder(p.pose.rightArm, temp.Rotation);

        applyLook_XEuler_YRotation(p.look);

        applyStepBody(p.stepBody);

        leftShoulderSwing(p.leftArmSwing);
        rightShoulderSwing(p.rightArmSwing);

        setBodyNormals(Vector3.up);
        setBodyBlend(p.bodyRotationBlend, p.bodyPositionBlend);

        animate_TorsoHeadShoulders();
        animateLegs();

        //arms
        Quaternion look = Quaternion.Euler(0, memory.rotation + memory.look.y, 0);
        poseLeftArm(p.pose.leftArm, look);
        leftArmSwing(p.leftArmSwing);
        leftElbowSwing(p.leftArmSwing);
        anim.handLPos.blend = p.leftArmBlend;
        anim.elbowLPos.blend = p.leftArmBlend;
        animateLeftArm();

        anim.handLRot.rotation = look;
        anim.handLRot.blend = p.leftArmBlend;
        anim.handLRotWorldSpace = true;
        animateLeftHand();

        poseRightArm(p.pose.rightArm, temp.Rotation);
        rightArmSwing(p.rightArmSwing);
        rightElbowSwing(p.rightArmSwing);
        anim.handRPos.blend = p.rightArmBlend;
        anim.elbowRPos.blend = p.rightArmBlend;
        animateRightArm();

        anim.handRRot.rotation = skeleton.lowArmR.rotation;
        anim.handRRot.blend = p.rightArmBlend;
        animateRightHand();
    }
    public void step_moveBodyArmsAnimation_state(stepStateParams p, moveBodyArmsAnimation ba)
    {
        //base rotation
        temp.rotationTarget = p.move.rotationOffset;
        temp.rotationTarget += ba.body.rotation.Evaluate(temp.phase);
        terrainVelocityAndTurn(temp.groundCheck);
        turnOrientation(p.move.rotation);
        setRotation();
        setArma(p.cap, temp.Rotation * Quaternion.Euler(p.pose.body.highSpineEuler));

        //movement, collision
        stepMove(p.move, ba.move);

        //feet
        stepLegs(p.step, p.pose.leftLeg, p.pose.rightLeg, p.effects);
        anim.footLPos.blend = p.legsBlend;
        anim.footRPos.blend = p.legsBlend;

        //body
        stepBodyPosition(p.step, p.move);

        applyBodyBaseRotation_Step();
        poseBody(p.pose.body, temp.Rotation);
        poseLeftShoulder(p.pose.leftArm, temp.Rotation);
        poseRightShoulder(p.pose.rightArm, temp.Rotation);

        applyLook_XEuler_YRotation(p.look);

        bodyAnimation(ba.body, temp.Orientation);
        leftShoulderAnimation(ba.leftArm);
        rightShoulderAnimation(ba.rightArm);

        setBodyNormals(Vector3.up);
        setBodyBlend(p.bodyRotationBlend, p.bodyPositionBlend);

        animate_TorsoHeadShoulders();
        animateLegs();

        //arms
        poseLeftArm(p.pose.leftArm, temp.Orientation);
        leftArmAnimation(ba.leftArm, temp.Orientation);
        anim.handLPos.blend = p.leftArmBlend;
        animateLeftArm();

        anim.handLRot.rotation = skeleton.lowArmL.rotation;
        anim.handLRot.blend = p.leftArmBlend;
        animateLeftHand();

        poseRightArm(p.pose.rightArm, temp.Orientation);
        rightArmAnimation(ba.rightArm, temp.Orientation);
        anim.handRPos.blend = p.rightArmBlend;
        animateRightArm();

        anim.handRRot.rotation = skeleton.lowArmR.rotation;
        anim.handRRot.blend = p.rightArmBlend;
        animateRightHand();
    }
    public void step_rightStrafeAttack_state(stepStateParams p, moveBodyArmsAnimation ba)
    {
        Quaternion actionRotation = Quaternion.Euler(0, memory.actionDirection.y, 0);

        //base rotation
        temp.rotationTarget = p.move.rotationOffset;
        temp.rotationTarget += ba.body.rotation.Evaluate(temp.phase);
        terrainVelocityAndTurn(temp.groundCheck);
        turnOrientation(p.move.rotation);
        setRotation();
        setArma(p.cap, temp.Rotation * Quaternion.Euler(p.pose.body.highSpineEuler));

        //movement, collision
        stepMove(p.move, ba.move);

        //feet
        stepLegs(p.step, p.pose.leftLeg, p.pose.rightLeg, p.effects);
        anim.footLPos.blend = p.legsBlend;
        anim.footRPos.blend = p.legsBlend;

        //body
        stepBodyPosition(p.step, p.move);

        applyBodyBaseRotation_Step();
        poseBody(p.pose.body, temp.Rotation);
        poseLeftShoulder(p.pose.leftArm, temp.Rotation);
        poseRightShoulder(p.pose.rightArm, temp.Rotation);

        applyLook_XEuler_YRotation(p.look);

        applyStepBody(p.stepBody);

        bodyAnimation(ba.body, temp.Orientation);
        leftShoulderAnimation(ba.leftArm);
        rightShoulderAnimation(ba.rightArm);

        setBodyNormals(Vector3.up);
        setBodyBlend(p.bodyRotationBlend, p.bodyPositionBlend);

        animate_TorsoHeadShoulders();
        animateLegs();

        //arms
        poseLeftArm(p.pose.leftArm, temp.Orientation);
        leftArmAnimation(ba.leftArm, temp.Orientation);
        anim.handLPos.blend = p.leftArmBlend;
        animateLeftArm();

        anim.handLRot.rotation = skeleton.lowArmL.rotation;
        leftHandAnimation(ba.leftArm);
        anim.handLRot.blend = p.leftArmBlend;
        animateLeftHand();

        poseRightArm(p.pose.rightArm, actionRotation);
        rightArmAnimation(ba.rightArm, actionRotation);
        anim.handRPos.blend = p.rightArmBlend;
        animateRightArm();

        anim.handRRot.rotation = actionRotation;
        rightHandAnimation(ba.rightArm);
        anim.handRRot.blend = p.rightArmBlend;
        animateRightHand();
    }
    public void step_lerpPose_handsHeadLook_state(stepStateParams p, fullPose to, float t)
    {
        fullPose lerpPose = new(p.pose, to, t);

        //base rotation
        temp.rotationTarget = p.move.rotationOffset;
        terrainVelocityAndTurn(temp.groundCheck);
        turnOrientation(p.move.rotation);
        setRotation();
        setArma(p.cap, temp.Rotation * Quaternion.Euler(lerpPose.body.highSpineEuler));

        //movement, collision
        stepMove(p.move);

        //feet
        stepLegs(p.step, lerpPose.leftLeg, lerpPose.rightLeg, p.effects);
        anim.footLPos.blend = p.legsBlend;
        anim.footRPos.blend = p.legsBlend;

        Quaternion lookRotation = Quaternion.Euler(memory.look.x, memory.look.y + memory.rotation, 0);

        //body
        stepBodyPosition(p.step, p.move);

        applyBodyBaseRotation_Step();
        poseBody(lerpPose.body, lookRotation);
        poseLeftShoulder(lerpPose.leftArm, lookRotation);
        poseRightShoulder(lerpPose.rightArm, lookRotation);

        applyLook_XEuler_YRotation(p.look);

        bodyRecoil();

        setBodyNormals(Vector3.up);
        setBodyBlend(p.bodyRotationBlend, p.bodyPositionBlend);

        animate_TorsoHeadShoulders();
        animateLegs();

        //arms
        poseLeftArm(lerpPose.leftArm, skeleton.head.position, lookRotation);
        anim.handLPos.blend = p.leftArmBlend;
        anim.elbowLPos.blend = p.leftArmBlend;
        animateLeftArm();

        anim.handLRot.rotation = lookRotation;
        anim.handLRot.blend = p.leftArmBlend;
        anim.handLRotWorldSpace = true;
        animateLeftHand();

        poseRightArm(lerpPose.rightArm, skeleton.head.position, lookRotation);
        anim.handRPos.blend = p.rightArmBlend;
        anim.elbowRPos.blend = p.rightArmBlend;
        animateRightArm();

        anim.handRRot.rotation = lookRotation;
        anim.handRRot.blend = p.rightArmBlend;
        anim.handRRotWorldSpace = true;
        animateRightHand();
    }
}

[Serializable]
public class stepStateParams
{
    public capsuleSettings cap;

    public stepMoveParams move;
    
    public fullPose pose;

    public stepParams step;
    public blendSettings legsBlend;

    public stepEffects effects;

    public lookParams look;

    public stepBodyParams stepBody;
    public blendSettings bodyRotationBlend, bodyPositionBlend;

    public armSwing leftArmSwing, rightArmSwing;
    public blendSettings leftArmBlend, rightArmBlend;

    public stepStateParams (stepStateParams other)
    {
        cap = other.cap;
        move = other.move;
        pose = other.pose;
        step = other.step;
        legsBlend = other.legsBlend;
        look = other.look;
        stepBody = other.stepBody;
        bodyRotationBlend = other.bodyRotationBlend;
        bodyPositionBlend = other.bodyPositionBlend;
        leftArmSwing = other.leftArmSwing;
        rightArmSwing = other.rightArmSwing;
        leftArmBlend = other.leftArmBlend;
        rightArmBlend = other.rightArmBlend;
    }
}