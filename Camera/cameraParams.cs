using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Camera", menuName = "ScriptableObjects/Camera")]
public class cameraParams : ScriptableObject
{
    public Vector3 thirdPersonPosition;

    public Vector3 aimRaingedPosition;
    public float aimRangedOffset;
    
    public Vector3 isometricPosition;
    public float isometricRotation;

    public Vector3 duelPosition;
    public float maxDuelOffset, duelXRotation;

    public float collisionRadius;
}
