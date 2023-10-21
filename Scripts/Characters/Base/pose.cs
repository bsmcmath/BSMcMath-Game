using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class characterBase : MonoBehaviour
{
    public void poseBody(bodyPose body, Quaternion rotation)
    {
        anim.bodyPos.position += rotation * body.position;

        anim.pelvisRot.y += body.pelvisTurn;
        anim.lowSpineRot.y += body.lowSpineTurn;
        anim.highSpineRot.y += body.highSpineTurn;
        anim.neckRot.y += body.neckTurn;
        anim.headRot.y += body.headTurn;
        
        anim.pelvisRot.tilt += rotation * body.pelvisTilt;
        anim.lowSpineRot.tilt += rotation * body.lowSpineTilt;
        anim.highSpineRot.tilt += rotation * body.highSpineTilt;
        anim.neckRot.tilt += rotation * body.neckTilt;
        anim.headRot.tilt += rotation * body.headTilt;
        
        anim.pelvisRot.euler += body.pelvisEuler;
        anim.lowSpineRot.euler += body.lowSpineEuler;
        anim.highSpineRot.euler += body.highSpineEuler;
        anim.neckRot.euler += body.neckEuler;
        anim.headRot.euler += body.headEuler;
    }
    public void poseLeftShoulder(armPose arm, Quaternion rotation)
    {
        anim.shoulderLRot.y += arm.shoulderTurn;
        anim.shoulderLRot.tilt += rotation * arm.shoulderTilt;
        anim.shoulderLRot.euler += arm.shoulderEuler;
    }
    public void poseRightShoulder(armPose arm, Quaternion rotation)
    {
        anim.shoulderRRot.y += arm.shoulderTurn;
        anim.shoulderRRot.tilt += rotation * arm.shoulderTilt;
        anim.shoulderRRot.euler += arm.shoulderEuler;
    }
    public void poseLeftArm(armPose arm, Quaternion rotation)
    {
        anim.handLPos.position = rotation * arm.handPosition + skeleton.highArmL.position;
        anim.elbowLPos.position = rotation * arm.elbowPosition + skeleton.highArmL.position;
        anim.handLRot.euler = arm.handRotation;
    }
    public void poseRightArm(armPose arm, Quaternion rotation)
    {
        anim.handRPos.position = rotation * arm.handPosition + skeleton.highArmR.position;
        anim.elbowRPos.position = rotation * arm.elbowPosition + skeleton.highArmR.position;
        anim.handRRot.euler = arm.handRotation;
    }
    public void poseLeftArm(armPose arm, Vector3 parentPosition, Quaternion rotation)
    {
        anim.handLPos.position = rotation * arm.handPosition + parentPosition;
        anim.elbowLPos.position = rotation * arm.elbowPosition + parentPosition;
        anim.handLRot.euler = arm.handRotation;
    }
    public void poseRightArm(armPose arm, Vector3 parentPosition, Quaternion rotation)
    {
        anim.handRPos.position = rotation * arm.handPosition + parentPosition;
        anim.elbowRPos.position = rotation * arm.elbowPosition + parentPosition;
        anim.handRRot.euler = arm.handRotation;
    }
    public void poseLeftLeg(legPose leg, Quaternion rotation)
    {
        anim.footLPos.position = skeleton.highLegL.position + rotation * leg.footPosition;

        memory.footLRotation = memory.rotation + leg.yRotation;
        Quaternion legRotation = Quaternion.Euler(0, memory.footLRotation, 0);

        anim.kneeLPos.position = skeleton.highLegL.position + legRotation * leg.kneePosition;

        anim.footLRot.rotation = legRotation * Quaternion.Euler(leg.footPoint, 0, 0);
    }
    public void poseRightLeg(legPose leg, Quaternion rotation)
    {
        anim.footRPos.position = skeleton.highLegR.position + rotation * leg.footPosition;

        memory.footRRotation = memory.rotation + leg.yRotation;
        Quaternion legRotation = Quaternion.Euler(0, memory.footRRotation, 0);

        anim.kneeRPos.position = skeleton.highLegR.position + legRotation * leg.kneePosition;

        anim.footRRot.rotation = legRotation * Quaternion.Euler(leg.footPoint, 0, 0);
    }
}

[Serializable]
public class bodyPose
{
    public Vector3 position;
    public Vector3 pelvisTilt, lowSpineTilt, highSpineTilt, neckTilt, headTilt;
    public Vector3 pelvisEuler, lowSpineEuler, highSpineEuler, neckEuler, headEuler;
    public float pelvisTurn, lowSpineTurn, highSpineTurn, neckTurn, headTurn;

    public bodyPose (bodyPose from, bodyPose to, float t)
    {
        position = Vector3.LerpUnclamped(from.position, to.position, t);
        pelvisTilt = Vector3.LerpUnclamped(from.pelvisTilt, to.pelvisTilt, t);
        lowSpineTilt = Vector3.LerpUnclamped(from.lowSpineTilt, to.lowSpineTilt, t);
        highSpineTilt = Vector3.LerpUnclamped(from.highSpineTilt, to.highSpineTilt, t);
        neckTilt = Vector3.LerpUnclamped(from.neckTilt, to.neckTilt, t);
        headTilt = Vector3.LerpUnclamped(from.headTilt, to.headTilt, t);
        pelvisEuler = Vector3.LerpUnclamped(from.pelvisEuler, to.pelvisEuler, t);
        lowSpineEuler = Vector3.LerpUnclamped(from.lowSpineEuler, to.lowSpineEuler, t);
        highSpineEuler = Vector3.LerpUnclamped(from.highSpineEuler, to.highSpineEuler, t);
        neckEuler = Vector3.LerpUnclamped(from.neckEuler, to.neckEuler, t);
        headEuler = Vector3.LerpUnclamped(from.headEuler, to.headEuler, t);
        pelvisTurn = Mathf.LerpUnclamped(from.pelvisTurn, to.pelvisTurn, t);
        lowSpineTurn = Mathf.LerpUnclamped(from.lowSpineTurn, to.lowSpineTurn, t);
        highSpineTurn = Mathf.LerpUnclamped(from.highSpineTurn, to.highSpineTurn, t);
        neckTurn = Mathf.LerpUnclamped(from.neckTurn, to.neckTurn, t);
        headTurn = Mathf.LerpUnclamped(from.headTurn, to.headTurn, t);
    }
}

[Serializable]
public class armPose
{
    public Vector3 handPosition, handRotation, elbowPosition;
    public Vector3 shoulderTilt, shoulderEuler;
    public float shoulderTurn;

    public armPose (armPose from, armPose to, float t)
    {
        handPosition = Vector3.LerpUnclamped(from.handPosition, to.handPosition, t);
        handRotation = Vector3.LerpUnclamped(from.handRotation, to.handRotation, t);
        elbowPosition = Vector3.LerpUnclamped(from.elbowPosition, to.elbowPosition, t);
        shoulderTilt = Vector3.LerpUnclamped(from.shoulderTilt, to.shoulderTilt, t);
        shoulderEuler = Vector3.LerpUnclamped(from.shoulderEuler, to.shoulderEuler, t);
        shoulderTurn = Mathf.LerpUnclamped(from.shoulderTurn, to.shoulderTurn, t);
    }
}

[Serializable]
public class legPose
{
    public Vector3 footPosition;
    public float yRotation, footPoint;
    public Vector3 kneePosition;

    public legPose (legPose from, legPose to, float t)
    {
        footPosition = Vector3.LerpUnclamped(from.footPosition, to.footPosition, t);
        yRotation = Mathf.LerpUnclamped(from.yRotation, to.yRotation, t);
        footPoint = Mathf.LerpUnclamped(from.footPoint, to.footPoint, t);
        kneePosition = Vector3.LerpUnclamped(from.kneePosition, to.kneePosition, t);
    }
}

[Serializable]
public class fullPose
{
    public bodyPose body;
    public legPose leftLeg, rightLeg;
    public armPose leftArm, rightArm;

    public fullPose (fullPose other)
    {
        body = other.body;
        leftLeg = other.leftLeg;
        rightLeg = other.rightLeg;
        leftArm = other.leftArm;
        rightArm = other.rightArm;
    }

    public fullPose (fullPose from, fullPose to, float t)
    {
        body = new(from.body, to.body, t);
        leftLeg = new(from.leftLeg, to.leftLeg, t);
        rightLeg = new(from.rightLeg, to.rightLeg, t);
        leftArm = new(from.leftArm, to.leftArm, t);
        rightArm = new(from.rightArm, to.rightArm, t);
    }
}