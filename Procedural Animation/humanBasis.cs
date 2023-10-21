using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class humanBasis : ScriptableObject
{
    public humanMeasurements measurements;
}

[Serializable]
public struct humanMeasurements
{
    public Quaternion arma, pelvis, highLegL, highLegR, lowSpine;
    public Quaternion lowLegL, footL, toeL;
    public Quaternion lowLegR, footR, toeR;
    public Quaternion highSpine, neck, shoulderL, shoulderR;
    public Quaternion head;
    public Quaternion highArmL, lowArmL, handL;
    public Quaternion highArmR, lowArmR, handR;

    public Vector3 lowArmLTwist_fwdHand, lowArmRTwist_fwdHand;

    public Quaternion footL_local, footR_local;
    public Quaternion toeL_local, toeR_local;
    public Quaternion shoulderL_local, shoulderR_local;
    public Quaternion handL_local, handR_local;

    public Vector3 localPosition;

    public float armLength, legLength;

    public humanHandBaseRotations handL_fingers, handR_fingers;

    public void populate(humanSkeleton skeleton)
    {
        arma = skeleton.arma.rotation;
        pelvis = skeleton.pelvis.rotation;
        highLegL = skeleton.highLegL.rotation;
        highLegR = skeleton.highLegR.rotation;
        lowSpine = skeleton.lowSpine.rotation;

        lowLegL = skeleton.lowLegL.rotation;
        footL = skeleton.footL.rotation;
        toeL = skeleton.toeL.rotation;

        lowLegR = skeleton.lowLegR.rotation;
        footR = skeleton.footR.rotation;
        toeR = skeleton.toeR.rotation;

        highSpine = skeleton.highSpine.rotation;
        neck = skeleton.neck.rotation;
        shoulderL = skeleton.shoulderL.rotation;
        shoulderR = skeleton.shoulderR.rotation;

        head = skeleton.head.rotation;

        highArmL = skeleton.highArmL.rotation;
        lowArmL = skeleton.lowArmL.rotation;
        handL = skeleton.handL.rotation;

        highArmR = skeleton.highArmR.rotation;
        lowArmR = skeleton.lowArmR.rotation;
        handR = skeleton.handR.rotation;

        lowArmLTwist_fwdHand = skeleton.lowArmTwistL.rotation * Vector3.forward;
        lowArmLTwist_fwdHand = Quaternion.Inverse(skeleton.handL.rotation) * lowArmLTwist_fwdHand;

        lowArmRTwist_fwdHand = skeleton.lowArmTwistR.rotation * Vector3.forward;
        lowArmRTwist_fwdHand = Quaternion.Inverse(skeleton.handR.rotation) * lowArmRTwist_fwdHand;

        footL_local = skeleton.footL.localRotation;
        footR_local = skeleton.footR.localRotation;
        toeL_local = skeleton.toeL.localRotation;
        toeR_local = skeleton.toeR.localRotation;

        shoulderL_local = skeleton.shoulderL.localRotation;
        shoulderR_local = skeleton.shoulderR.localRotation;
        handL_local = skeleton.handL.localRotation;
        handR_local = skeleton.handR.localRotation;

        localPosition = skeleton.pelvis.localPosition;

        armLength = help.distance(skeleton.highArmL.position, skeleton.lowArmL.position)
            + help.distance(skeleton.lowArmL.position, skeleton.handL.position);
        armLength -= 0.001f;

        legLength = help.distance(skeleton.highLegL.position, skeleton.lowLegL.position)
            + help.distance(skeleton.lowLegL.position, skeleton.footL.position);
        legLength -= 0.001f;

        handL_fingers.populateLeft(skeleton);
        handR_fingers.populateRight(skeleton);
    }
}

[Serializable]
public struct humanHandBaseRotations
{
    public Quaternion index1, index2, index3;
    public Quaternion middle1, middle2, middle3;
    public Quaternion ring1, ring2, ring3;
    public Quaternion pinky1, pinky2, pinky3;
    public Quaternion thumb1, thumb2, thumb3;

    public void populateLeft(humanSkeleton skeleton)
    {
        index1 = skeleton.fingerIndex1L.localRotation;
        index2 = skeleton.fingerIndex2L.localRotation;
        index3 = skeleton.fingerIndex3L.localRotation;
        middle1 = skeleton.fingerMiddle1L.localRotation;
        middle2 = skeleton.fingerMiddle2L.localRotation;
        middle3 = skeleton.fingerMiddle3L.localRotation;
        ring1 = skeleton.fingerRing1L.localRotation;
        ring2 = skeleton.fingerRing2L.localRotation;
        ring3 = skeleton.fingerRing3L.localRotation;
        pinky1 = skeleton.fingerPinky1L.localRotation;
        pinky2 = skeleton.fingerPinky2L.localRotation;
        pinky3 = skeleton.fingerPinky3L.localRotation;
        thumb1 = skeleton.thumb1L.localRotation;
        thumb2 = skeleton.thumb2L.localRotation;
        thumb3 = skeleton.thumb3L.localRotation;
    }
    public void populateRight(humanSkeleton skeleton)
    {
        index1 = skeleton.fingerIndex1R.localRotation;
        index2 = skeleton.fingerIndex2R.localRotation;
        index3 = skeleton.fingerIndex3R.localRotation;
        middle1 = skeleton.fingerMiddle1R.localRotation;
        middle2 = skeleton.fingerMiddle2R.localRotation;
        middle3 = skeleton.fingerMiddle3R.localRotation;
        ring1 = skeleton.fingerRing1R.localRotation;
        ring2 = skeleton.fingerRing2R.localRotation;
        ring3 = skeleton.fingerRing3R.localRotation;
        pinky1 = skeleton.fingerPinky1R.localRotation;
        pinky2 = skeleton.fingerPinky2R.localRotation;
        pinky3 = skeleton.fingerPinky3R.localRotation;
        thumb1 = skeleton.thumb1R.localRotation;
        thumb2 = skeleton.thumb2R.localRotation;
        thumb3 = skeleton.thumb3R.localRotation;
    }
}