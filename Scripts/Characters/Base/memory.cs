using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct characterMemory
{
    public Vector3 moveVector;
    public float directionVariation;

    public Vector2 orientation, orientationTarget;
    public float rotation;
    public float baseRotation;

    public Vector3 look;

    public Vector3 velocity;

    public Vector3 localPosition;

    public float footLRotation, footRRotation;

    public Vector3 footLLocalPosition, footRLocalPosition;
    public Vector3 kneeLLocalPosition, kneeRLocalPosition;
    public Quaternion footLLocalRotation, footRLocalRotation;

    public Vector3 handLLocalPosition, handRLocalPosition;
    public Quaternion handLLocalRotation, handRLocalRotation;
    public Vector3 elbowLLocalPosition, elbowRLocalPosition;

    public Quaternion handLWorldRotation, handRWorldRotation;

    public stepMemory legLStep, legRStep;
    public stepMemory armLStep, armRStep;
    public bool leftStepLast;

    public float timer;

    public Vector3 actionStartPosition;
    public Vector3 actionDirection;
    public float actionPower;

    public interactor interactor;

    public int statePhase;

    public List<characterRecoil> recoil;

    public Vector3 hitboxPosition;
    public Quaternion hitboxRotation;


}