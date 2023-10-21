using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class characterBase : MonoBehaviour
{
    public void wallCheck()
    {
        TerrainCasts.physicsRay(skeleton.arma.position, Quaternion.Euler(0, memory.orientation.y, 0) * Vector3.forward, basis.armLength, out temp.wallCheck);

    }
    public bool climbCheck()
    {
        if (!pushIsToOrientation()) return false;

        if (temp.wallCheck.hit && temp.wallCheck.obj.type == terrainType.climbable)
        {
            return true;
        }
        return false;
    }
    public void climbMove(climbMoveParams move)
    {
        terrainVelocityAndTurn(temp.wallCheck);

        turnOrientation(move.rotationBlend);
        setRotation();

        climbForces(move);

        skeleton.arma.position += memory.velocity * Time.fixedDeltaTime;

        terrainCollision();


    }
    public void climbForces(climbMoveParams move)
    {
        temp.push = Quaternion.FromToRotation(Vector3.up, temp.wallCheck.normal) * temp.push;

        Vector3 add = move.accelerationUp * temp.push;
        
        Vector3 velocityNormal = Vector3.Project(memory.velocity, temp.wallCheck.normal);
        Vector3 velocityPerpendicular = memory.velocity - velocityNormal;

        add -= velocityPerpendicular * move.drag;

        add += (move.normalDistance - temp.wallCheck.distance) * move.normalSpring * temp.wallCheck.normal;

        add -= velocityNormal * move.normalDrag;

        memory.velocity += add * Time.fixedDeltaTime;
    }
    public void climbBody()
    {

    }
    public void getActionStart()
    {
        memory.actionStartPosition = skeleton.arma.position;
    }
}

[Serializable]
public class climbMoveParams
{
    public blendSettings rotationBlend;

    public float accelerationUp, accelerationSide, accelerationDown;
    public float drag;

    public float normalDistance;
    public float normalSpring;
    public float normalDrag;
}