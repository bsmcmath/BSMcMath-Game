using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class characterBase : MonoBehaviour
{
    public bool pushIsToOrientation()
    {
        float angle = help.angleDifference(temp.pushAngle, memory.orientation.y);
        return temp.pushMagnitude > 0 && angle > -45 && angle < 45;
    }
    public bool ledgeCheck()
    {
        temp.Orientation = Quaternion.Euler(0, memory.orientation.y, 0);

        float height = basis.armatureToShoulder + basis.armLength * 1.25f;
        TerrainCasts.physicsRay(skeleton.arma.position + new Vector3(0, height), temp.Orientation * Vector3.forward, basis.armLength, out temp.ledgeCheck);
        if (temp.ledgeCheck.hit)
        {
            temp.ledgeCheck.hit = false;
            return false;
        }

        float depth = height + basis.footToArmature + basis.legLength * 0.25f;
        TerrainCasts.physicsRay(skeleton.arma.position + temp.Orientation * new Vector3(0, height, basis.armLength), Vector3.down, depth, out temp.ledgeCheck);

        if (temp.ledgeCheck.hit)
        {
            Vector3 topNormal = temp.ledgeCheck.normal;

            Vector3 endpoint = temp.ledgeCheck.position; 
            endpoint.y -= 0.02f;

            Vector3 direction = skeleton.arma.position - endpoint;
            direction.y = 0;
            direction = Quaternion.FromToRotation(Vector3.up, topNormal) * direction;

            Vector3 origin = endpoint + direction;
            
            TerrainCasts.physicsRay(origin, -direction, direction.magnitude, out temp.ledgeCheck);

            if (!temp.ledgeCheck.hit) return false;

            if (Vector3.Angle(topNormal, temp.ledgeCheck.normal) < 45)
            {
                temp.ledgeCheck.hit = false;
                return false;
            }

            return true;
        }
        return false;
    }
    public bool offLedgeCheck()
    {
        if (temp.ledgeCheck.hit)
        {
            if (!temp.groundCheck.hit) return false;
            
            TerrainCasts.physicsEdge(temp.ledgeCheck.position, temp.groundCheck.position, out terrainHit edge);

            if (temp.groundCheck.position.y > temp.ledgeCheck.position.y || !edge.hit) return true;
        }
        return true;
    }
}
