using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class characterBase : MonoBehaviour
{
    public bool stepUpCheck()
    {
        if (temp.ledgeCheck.hit)
        {
            if (!pushIsToOrientation()) return false;

            if (temp.ledgeCheck.position.y > skeleton.arma.position.y - (basis.kneeToArmature + basis.hipToArmature) * 0.5f) return false;

            if (temp.groundCheck.hit && temp.groundCheck.position.y + basis.legLength * 0.5f > temp.ledgeCheck.position.y) return false;
            
            return true;
        }
        return false;
    }
    //
    public void stepUpMove(stepMoveParams move)
    {
        terrainVelocityAndTurn(temp.ledgeCheck);
        turnOrientation(move.rotation);
        setRotation();

        stepUpMovementCalculations();
        stepUpForces(move);
        skeleton.arma.position += memory.velocity * Time.fixedDeltaTime;
        terrainCollisionXZ();

        velocityUpdate();
    }
    public void stepUpMovementCalculations()
    {
        temp.velocityXZ = memory.velocity;
        temp.velocityXZ.y = 0;
        temp.velocityXZmagnitude = temp.velocityXZ.magnitude;
    }
    public void stepUpForces(stepMoveParams move)
    {
        float endHeight = temp.ledgeCheck.position.y + move.distanceOffGround;
        float splitHeight = endHeight - basis.legLength * 0.25f;
        float splitDistance = basis.legLength * 0.5f;

        if (skeleton.arma.position.y < splitHeight)
        {
            float startDistance = help.distanceXZ(temp.ledgeCheck.position, memory.actionStartPosition);
            float phase = help.map(skeleton.arma.position.y, memory.actionStartPosition.y, splitHeight, 0, 1);
            float targetDistance = Mathf.Lerp(startDistance, splitDistance, phase);
            if (targetDistance < splitDistance) targetDistance = splitDistance;

            temp.acceleration = temp.Orientation * new Vector3(0, 
                move.accelerationFlat,
                move.yPositionSpring * (temp.ledgeCheck.distance - targetDistance)
                );

            memory.velocity.y -= move.dragFlat * memory.velocity.y * Time.fixedDeltaTime;

            memory.velocity -= move.yDrag * Time.fixedDeltaTime * temp.velocityXZ;

            memory.velocity += temp.acceleration * Time.fixedDeltaTime;
        }
        else
        {
            float targetHeight = help.map(help.distanceXZ(skeleton.arma.position, temp.ledgeCheck.position), splitDistance, 0, splitHeight, endHeight);

            temp.acceleration = temp.Orientation * new Vector3(0,
                move.yPositionSpring * (targetHeight - skeleton.arma.position.y),
                move.accelerationFlat);

            memory.velocity.y -= move.yDrag * memory.velocity.y * Time.fixedDeltaTime;

            memory.velocity -= move.dragFlat * Time.fixedDeltaTime * temp.velocityXZ;

            memory.velocity += temp.acceleration * Time.fixedDeltaTime;
        }
    }
}
