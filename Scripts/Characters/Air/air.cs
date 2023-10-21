using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class characterBase : MonoBehaviour
{
    public bool airCheck()
    {
        return false;
    }
    public void airMove(airMoveParams move)
    {
        turnOrientation(move.rotationBlend);
        setRotation();

        airForces(move);
        skeleton.arma.position += memory.velocity * Time.fixedDeltaTime;
        terrainCollisionXZ();

        airTempCalculations();
    }
    public void airForces(airMoveParams move)
    {
        Vector3 add = move.acceleration * temp.push;
        add -= memory.velocity.magnitude * move.drag * memory.velocity;
        add.y -= 9.81f;
        memory.velocity += add * Time.fixedDeltaTime;

        characterSeparation(move.characterSeparationStrength);
    }
    public void airTempCalculations()
    {
        temp.velocityXZ = memory.velocity; temp.velocityXZ.y = 0;
        temp.velocityXZmagnitude = temp.velocityXZ.magnitude;
        
    }
    public void applyAirBody(airBodyParams ab)
    {
        applyBodyTilt(temp.velocityXZ, ab.velocityTilt);
    }
    public void airLegs()
    {

    }
    public void airLeftLeg()
    {

    }
    public void airRightLeg()
    {

    }
    public void airLeftArm()
    {

    }
    public void airRightArm()
    {

    }
}

[Serializable]
public class airMoveParams
{
    public blendSettings rotationBlend;
    public float rotationOffset;

    public float acceleration, drag;

    public float characterSeparationStrength;
}

[Serializable]
public class airBodyParams
{
    public bodyFloats velocityTilt;
}

[Serializable]
public class airStepParams
{

}