using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingTerrain : terrain
{
    public Rigidbody rb;
    public Vector3 previousPosition;
    public Quaternion previousRotation;
}
