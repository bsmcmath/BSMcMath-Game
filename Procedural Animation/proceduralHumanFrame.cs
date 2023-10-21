using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralHumanFrame
{
    public ProceduralRotation pelvis, lowSpine, highSpine, leftShoulder, rightShoulder, neck, head;

    public Vector3 position;
    
    public Vector3 leftLegTarget, leftLegPole, rightLegTarget, rightLegPole;

    public ProceduralRotation leftFoot, rightFoot;

    public Vector3 leftArmTarget, leftArmPole, rightArmTarget, rightArmPole;

    public ProceduralRotation leftHand, rightHand;
}
