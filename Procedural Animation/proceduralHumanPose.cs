using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ProceduralHumanPose
{
    public RotationContribution[] pelvis, lowSpine, highSpine, leftShoulder, rightShoulder, neck, head;

    public Vector3 position;

    public Vector3 leftLegTarget, leftLegPole, rightLegTarget, rightLegPole;

    public Vector3 leftArmTarget, leftArmPole, rightArmTarget, rightArmPole;
}