using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterBasis : ScriptableObject
{
    public float footSizeX, footSizeZ;
    public Vector3 heelToFoot, toeToFoot, targetToToe;

    public float weight;

    [Header("Automatic Measurements")]
    public Vector3 pos;
    public Quaternion
        arma, pelvis, lowSpine, highSpine, neck, head,
        shoulderL, shoulderR, handL, handR, 
        footL, footR, footLegL, footLegR;

    public float armLength;
    public float legLength;

    public Vector3 armLTwistFwdHand;
    public Vector3 armRTwistFwdHand;

    public float footToArmature, kneeToArmature, hipToArmature;
    public float armatureToShoulder, armatureToHighSpine;
}
