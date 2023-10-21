using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ProceduralRotation
{
    public Quaternion rotation;

    public bool includeNormal, includeTilt, includeEuler1, includeEuler2;

    public Vector3 normal;
    public Vector3 tilt;
    public Vector3 euler1;
    public Vector3 euler2;

    public void SetNormal(Vector3 n)
    {
        includeNormal = true;
        normal = n;
    }
    public void AddTilt(Vector3 v)
    {
        includeTilt = true;
        tilt += v;
    }
    public void AddEuler1(Vector3 v)
    {
        includeEuler1 = true;
        euler1 += v;
    }
    public void AddEuler1(float y)
    {
        includeEuler1 = true;
        euler1.y += y;
    }
    public void AddEuler2(Vector3 v)
    {
        includeEuler2 = true;
        euler2 += v;
    }
    public void AddEuler2(float x, float y, float z)
    {
        includeEuler2 = true;
        euler2.x += x;
        euler2.y += y;
        euler2.z += z;
    }

    public void Apply()
    {
        if (includeNormal) rotation *= Quaternion.FromToRotation(Vector3.up, normal);
        if (includeTilt) rotation *= help.tiltRotation(tilt, tilt.magnitude);
        if (includeEuler1) rotation *= Quaternion.Euler(euler1);
        if (includeEuler2) rotation *= Quaternion.Euler(euler2);
    }
}

[Serializable]
public class RotationContribution
{
    public Vector3 rotation;
    public RotationType type;
}

public enum RotationType
{
    normal, tilt, euler1, euler2
}