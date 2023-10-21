using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class characterBase : MonoBehaviour
{
    public bool stayGroundCheck()
    {
        TerrainCasts.physicsRay(skeleton.arma.position, Vector3.down, basis.legLength * 0.5f + basis.footToArmature, out temp.groundCheck);
        return temp.groundCheck.hit;
    }
    public bool airGroundCheck()
    {
        TerrainCasts.physicsRay(skeleton.arma.position, Vector3.down, basis.footToArmature, out temp.groundCheck);
        return temp.groundCheck.hit;
    }
    public void pushCheck()
    {
        TerrainCasts.physicsRay(skeleton.arma.position + temp.push * basis.legLength, Vector3.down, basis.legLength * 4, out temp.pushCheck);
    }
    public void velocityCheck()
    {
        Vector3 origin = temp.velocityXZ * basis.legLength / temp.velocityXZmagnitude;
        origin += skeleton.arma.position;
        origin.y += basis.legLength * 0.5f;
        TerrainCasts.physicsRay(origin, Vector3.down, basis.legLength * 2, out temp.velocityCheck);
    }
    //
    public void stepMove(stepMoveParams move)
    {
        updateRecoil();

        stepMovementCalculations();

        stepForces(move);

        offGroundHeight(move);

        skeleton.arma.position += memory.velocity * Time.fixedDeltaTime;

        terrainCollisionXZ();

        velocityUpdate();
    }
    public void stepMove(stepMoveParams move, moveAnimation ma)
    {
        updateRecoil();

        stepMovementCalculations();

        stepForces(move);

        animationMovement(ma);

        offGroundHeight(move);

        skeleton.arma.position += memory.velocity * Time.fixedDeltaTime;

        terrainCollisionXZ();

        velocityUpdate();
    }
    //
    public void terrainVelocityAndTurn(terrainHit contact)
    {
        if (contact.obj.type == terrainType.moving)
        {
            movingTerrain terrain = (movingTerrain)contact.obj;
            Vector3 previousPosition = terrain.previousPosition + terrain.previousRotation * contact.localPosition;
            temp.terrainMove = contact.position - previousPosition;
            temp.terrainVelocity = temp.terrainMove / Time.fixedDeltaTime;
            temp.terrainTurn = Vector3.SignedAngle(terrain.previousRotation * Vector3.forward, terrain.transform.rotation * Vector3.forward, Vector3.up);
        }
        else
        {
            temp.terrainMove = Vector3.zero;
            temp.terrainVelocity = Vector3.zero;
            temp.terrainTurn = 0;
        }
    }
    public void stepMovementCalculations()
    {
        pushCheck();
        if (temp.pushCheck.hit)
        {
            temp.pushSlope = Vector3.Angle(temp.push, temp.pushCheck.position - temp.groundCheck.position);
            if (temp.pushCheck.position.y < temp.groundCheck.position.y) temp.pushSlope *= -1;
        }
        else
        {
            Vector3 PushSlope = Quaternion.FromToRotation(Vector3.up, temp.groundCheck.normal) * temp.push;
            temp.pushSlope = Vector3.Angle(temp.push, PushSlope);
            if (PushSlope.y < 0) temp.pushSlope *= -1;
        }
        temp.pushAngleLocalUnsigned = Mathf.Abs(Mathf.DeltaAngle(memory.rotation, temp.pushAngle));

        temp.velocityXZ = memory.velocity - temp.terrainVelocity; temp.velocityXZ.y = 0;
        temp.velocityXZmagnitude = temp.velocityXZ.magnitude;

        velocityCheck();
        if (temp.velocityCheck.hit)
        {
            temp.velocitySlope = Vector3.Angle(temp.velocityXZ, temp.velocityCheck.position - temp.groundCheck.position);
            if (temp.velocityCheck.position.y < temp.groundCheck.position.y) temp.velocitySlope *= -1;
        }
        else
        {
            temp.velocitySlope = Vector3.Angle(temp.velocityXZ, memory.velocity);
            if (memory.velocity.y < 0) temp.velocitySlope *= -1;
        }

        if (temp.velocityXZmagnitude > 0.05f || temp.pushMagnitude > 0.05f)
        {
            Vector3 previous = temp.previousRotation * memory.moveVector;
            memory.moveVector = (temp.velocityXZ + temp.push).normalized;
            memory.directionVariation += (previous - temp.Rotation * memory.moveVector).magnitude * Time.fixedDeltaTime;
        }
        memory.directionVariation -= memory.directionVariation * 3 * Time.fixedDeltaTime;
    }
    public void stepForces(stepMoveParams move)
    {
        float force;
        if (temp.pushSlope > 0) force = help.map(temp.pushSlope, 0, 90, move.accelerationFlat, move.accelerationUp);
        else force = help.map(temp.pushSlope, 0, -90, move.accelerationFlat, move.accelerationDown);

        if (temp.pushAngleLocalUnsigned < 90) force *= help.map(temp.pushAngleLocalUnsigned, 0, 90, 1, move.accelerationSide);
        else force *= help.map(temp.pushAngleLocalUnsigned, 90, 180, move.accelerationSide, move.accelerationBack);

        force *= (1 - Mathf.Abs(temp.turnVelocity) * move.accelerationTurn);

        temp.acceleration = force * temp.push;

        //temp.acceleration += temp.environmentInfluence * temp.pushMagnitude;

        characterSeparation(move.characterSeparationStrength);

        memory.velocity += temp.acceleration * Time.fixedDeltaTime;

        memory.velocity += temp.pushMagnitude * Time.fixedDeltaTime * temp.environmentInfluence;

        if (temp.velocitySlope > 0) temp.drag = help.map(temp.velocitySlope, 0, 90, move.dragFlat, move.dragUp);
        else temp.drag = help.map(temp.velocitySlope, 0, -90, move.dragFlat, move.dragDown);

        temp.drag += temp.environmentDrag;

        if (temp.inWater && temp.waterLevel > temp.groundCheck.position.y) temp.drag += help.map(temp.waterLevel - temp.groundCheck.position.y, 0, basis.footToArmature, 0, 2.5f);

        memory.velocity -= temp.velocityXZ.normalized * Mathf.Pow(temp.velocityXZmagnitude, 0.5f) * temp.drag * Time.fixedDeltaTime;
        //memory.velocity -= temp.velocityXZ * temp.drag * Time.fixedDeltaTime;
    }
    public void offGroundHeight(stepMoveParams move)
    {
        Vector3 origin = skeleton.arma.position + memory.moveVector * basis.legLength * 0.5f;
        TerrainCasts.physicsRay(origin, Vector3.down, basis.legLength * 3, out terrainHit nextHit);

        origin = skeleton.arma.position - memory.moveVector * basis.legLength * 0.5f;
        TerrainCasts.physicsRay(origin, Vector3.down, basis.legLength * 3, out terrainHit previousHit);

        terrainHit edge1, edge2;
        if (nextHit.hit && nextHit.position.y < temp.groundCheck.position.y + basis.legLength * 0.5f)
        {
            TerrainCasts.physicsEdge(temp.groundCheck.position, nextHit.position, out edge1);
        }
        else edge1 = new terrainHit();
        if (previousHit.hit && previousHit.position.y < temp.groundCheck.position.y + basis.legLength * 0.5f)
        {
            TerrainCasts.physicsEdge(temp.groundCheck.position, previousHit.position, out edge2);
        }
        else edge2 = new terrainHit();

        float basePosition;
        bool twoEdges = edge1.hit && edge2.hit;
        twoEdges &= (edge1.position - edge2.position).sqrMagnitude > 0.15f;
        twoEdges &= Mathf.Sign(edge1.position.y - temp.groundCheck.position.y) != Mathf.Sign(edge2.position.y - temp.groundCheck.position.y);

        if (twoEdges)
        {
            float d1 = help.distanceXZ(edge1.position, temp.groundCheck.position);
            float d2 = help.distanceXZ(edge2.position, temp.groundCheck.position);
            basePosition = Mathf.Lerp(edge1.position.y, edge2.position.y, d1 / (d1 + d2));
        }
        else if (edge1.hit)
        {
            float d = help.distanceXZ(edge1.position, temp.groundCheck.position);
            d = Mathf.Clamp(d, 0, basis.legLength) / basis.legLength;
            basePosition = Mathf.Lerp(edge1.position.y, temp.groundCheck.position.y, d);
        }
        else if (edge2.hit)
        {
            float d = help.distanceXZ(edge2.position, temp.groundCheck.position);
            d = Mathf.Clamp(d, 0, basis.legLength) / basis.legLength;
            basePosition = Mathf.Lerp(edge2.position.y, temp.groundCheck.position.y, d);
        }
        else basePosition = temp.groundCheck.position.y;

        float toTargetPosition = basePosition + move.distanceOffGround - skeleton.arma.position.y;

        float acceleration = toTargetPosition * move.yPositionSpring;

        acceleration += (temp.terrainVelocity.y - memory.velocity.y) * move.yDrag;

        memory.velocity.y += acceleration * Time.fixedDeltaTime;
    }
    public void velocityUpdate()
    {
        temp.velocityXZ = memory.velocity - temp.terrainVelocity; temp.velocityXZ.y = 0;
        temp.velocityXZmagnitude = temp.velocityXZ.magnitude;

        temp.velocityAngleLocal = Vector3.SignedAngle(temp.Rotation * Vector3.forward, temp.velocityXZ, Vector3.up);
        temp.velocityAngleLocalUnsigned = Mathf.Abs(temp.velocityAngleLocal);

        temp.finalVelocity = temp.acceleration / temp.drag;
        temp.finalVelocityMagnitude = temp.finalVelocity.magnitude;
        temp.finalVelocity *= temp.finalVelocityMagnitude;
        temp.finalVelocityMagnitude *= temp.finalVelocityMagnitude;

        temp.toFinalVelocity = temp.finalVelocity - temp.velocityXZ;

        if (temp.finalVelocityMagnitude > temp.velocityXZmagnitude)
        {
            temp.actingVelocity = temp.finalVelocity;
            temp.actingVelocityMagnitude = temp.finalVelocityMagnitude;
        }
        else
        {
            temp.actingVelocity = temp.velocityXZ;
            temp.actingVelocityMagnitude = temp.velocityXZmagnitude;
        }
        temp.actingVelocityNormal = temp.actingVelocity / temp.actingVelocityMagnitude;
        if (float.IsNaN(temp.actingVelocityNormal.x)) temp.actingVelocityNormal = Vector3.zero;
    }
    public void pushCalculations()
    {
        temp.pushMagnitude = temp.push.magnitude;
        if (temp.pushMagnitude > 0) temp.pushAngle = Vector3.SignedAngle(Vector3.forward, temp.push, Vector3.up);
    }
}

public class characterMovementPredictions
{
    public Vector3 position, velocity;
    public float rotation;

    public characterMovementPredictions(characterBase c, float t)
    {
        //float e = 2.71828f;
        //Vector3 A = c.temp.acceleration;// + c.temp.environmentInfluence * c.temp.pushMagnitude;
        //float expMinusDt = Mathf.Pow(e, -c.temp.drag * t);

        ////predicted character position, velocity, and rotation at end of t
        //position = expMinusDt * (A - c.temp.drag * c.temp.velocityXZ) + A * c.temp.drag * t;
        //position /= c.temp.drag * c.temp.drag;
        //position += c.skeleton.arma.position;

        //velocity = expMinusDt * (A / expMinusDt - A + c.temp.drag * c.temp.velocityXZ) / c.temp.drag;

        predictPow(c, t);

        rotation = Mathf.MoveTowardsAngle(c.memory.rotation, c.memory.orientation.y, Mathf.Abs(c.temp.turnVelocity) * t / Time.fixedDeltaTime);
    }

    public void predictPow(characterBase c, float t)
    {
        int frames = Mathf.CeilToInt(t / Time.fixedDeltaTime);
        velocity = c.temp.velocityXZ;
        position = c.skeleton.arma.position;
        for (int i = 0; i < frames; i++)
        {
            float velocityMagnitude = velocity.magnitude;
            velocity -= velocity.normalized * Mathf.Pow(velocityMagnitude, 0.5f) * Time.fixedDeltaTime * c.temp.drag;
            velocity += c.temp.acceleration * Time.fixedDeltaTime;
            position += velocity * Time.fixedDeltaTime;
        }
    }
}

[Serializable]
public class stepMoveParams
{
    public blendSettings rotation;
    public float rotationOffset;

    public float accelerationFlat, accelerationUp, accelerationDown;
    public float accelerationTurn, accelerationSide, accelerationBack;
    public float dragFlat, dragUp, dragDown;

    public float distanceOffGround;
    public float yPositionSpring;
    public float yDrag;

    public float characterSeparationStrength;
}