using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterTempMemory
{
    public terrainHit groundCheck;

    public Vector3 look;

    public Vector3 push;
    public float pushMagnitude;
    public float pushSlope;
    public float pushAngle, pushAngleLocalUnsigned;

    public terrainHit pushCheck;
    public terrainHit velocityCheck;

    public float rotationTarget;
    public float turnVelocity, turnSpeed;
    public Quaternion Rotation, Orientation;
    public Quaternion previousRotation;
    public Quaternion baseRotation;

    public Vector3 velocityXZ;
    public float velocityXZmagnitude;
    public float velocitySlope;
    public float velocityAngleLocal, velocityAngleLocalUnsigned;

    public Vector3 terrainMove, terrainVelocity;
    public float terrainTurn;

    public Vector3 environmentInfluence;
    public float environmentDrag;

    public Vector3 acceleration;
    public float drag;
    public Vector3 finalVelocity;
    public float finalVelocityMagnitude;
    public Vector3 toFinalVelocity;

    public Vector3 actingVelocity, actingVelocityNormal;
    public float actingVelocityMagnitude;

    public float pausePhase;
    public float gaitPhase;
    public float leftTimerInitial, rightTimerInitial;

    public float phase;

    public terrainHit wallCheck, ledgeCheck;

    public bool inWater;
    public float waterLevel;
}