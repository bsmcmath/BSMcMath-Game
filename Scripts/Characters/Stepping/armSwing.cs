using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class characterBase : MonoBehaviour
{
    public float gaitMagnitude(gaitMagnitudes mags)
    {
        float magnitude = mags.flat + mags.velocity * temp.actingVelocityMagnitude;
        magnitude *= temp.velocityAngleLocalUnsigned < 90 ?
            help.map(temp.velocityAngleLocalUnsigned, 0, 90, 1, mags.side) :
            help.map(temp.velocityAngleLocalUnsigned, 90, 180, mags.side, mags.back);
        return magnitude;
    }
    public void leftShoulderSwing(armSwing swing)
    {
        for (int i = 0; i < swing.shoulder.Length; i++)
        {
            gaitCurve g = swing.shoulder[i];
            switch (g.direction)
            {
                case 0:
                    anim.shoulderLRot.euler.x += g.curve.Evaluate(temp.gaitPhase) * gaitMagnitude(g.magnitude);
                    break;
                case 1:
                    anim.shoulderLRot.y += g.curve.Evaluate(temp.gaitPhase) * gaitMagnitude(g.magnitude);
                    break;
                case 2:
                    anim.shoulderLRot.euler.z += g.curve.Evaluate(temp.gaitPhase) * gaitMagnitude(g.magnitude);
                    break;
            }
        }
    }
    public void rightShoulderSwing(armSwing swing)
    {
        for (int i = 0; i < swing.shoulder.Length; i++)
        {
            gaitCurve g = swing.shoulder[i];
            switch (g.direction)
            {
                case 0:
                    anim.shoulderRRot.euler.x += g.curve.Evaluate(temp.gaitPhase) * gaitMagnitude(g.magnitude);
                    break;
                case 1:
                    anim.shoulderRRot.y += g.curve.Evaluate(temp.gaitPhase) * gaitMagnitude(g.magnitude);
                    break;
                case 2:
                    anim.shoulderRRot.euler.z += g.curve.Evaluate(temp.gaitPhase) * gaitMagnitude(g.magnitude);
                    break;
            }
        }
    }
    public void leftArmSwing(armSwing swing)
    {
        for (int i = 0; i < swing.arm.Length; i++)
        {
            gaitCurve g = swing.arm[i];
            Vector3 add = Vector3.zero;
            switch (g.direction)
            {
                case 0:
                    add = temp.Rotation * new Vector3(g.curve.Evaluate(temp.gaitPhase) * gaitMagnitude(g.magnitude), 0, 0);
                    break;
                case 1:
                    add = new Vector3(0, g.curve.Evaluate(temp.gaitPhase) * gaitMagnitude(g.magnitude), 0);
                    break;
                case 2:
                    add = g.curve.Evaluate(temp.gaitPhase) * gaitMagnitude(g.magnitude) * temp.actingVelocityNormal;
                    break;
            }
            anim.handLPos.position += add;
        }
    }
    public void rightArmSwing(armSwing swing)
    {
        for (int i = 0; i < swing.arm.Length; i++)
        {
            gaitCurve g = swing.arm[i];
            Vector3 add = Vector3.zero;
            switch (g.direction)
            {
                case 0:
                    add = temp.Rotation * new Vector3(g.curve.Evaluate(temp.gaitPhase) * gaitMagnitude(g.magnitude), 0, 0);
                    break;
                case 1:
                    add = new Vector3(0, g.curve.Evaluate(temp.gaitPhase) * gaitMagnitude(g.magnitude), 0);
                    break;
                case 2:
                    add = g.curve.Evaluate(temp.gaitPhase) * gaitMagnitude(g.magnitude) * temp.actingVelocityNormal;
                    break;
            }
            anim.handRPos.position += add;
        }
    }
    public void leftElbowSwing(armSwing swing)
    {
        for (int i = 0; i < swing.elbow.Length; i++)
        {
            gaitCurve g = swing.elbow[i];
            Vector3 add = Vector3.zero;
            switch (g.direction)
            {
                case 0:
                    add = temp.Rotation * new Vector3(g.curve.Evaluate(temp.gaitPhase) * gaitMagnitude(g.magnitude), 0, 0);
                    break;
                case 1:
                    add = new Vector3(0, g.curve.Evaluate(temp.gaitPhase) * gaitMagnitude(g.magnitude), 0);
                    break;
                case 2:
                    add = g.curve.Evaluate(temp.gaitPhase) * gaitMagnitude(g.magnitude) * temp.actingVelocityNormal;
                    break;
            }
            anim.elbowLPos.position += add;
        }
    }
    public void rightElbowSwing(armSwing swing)
    {
        for (int i = 0; i < swing.elbow.Length; i++)
        {
            gaitCurve g = swing.elbow[i];
            Vector3 add = Vector3.zero;
            switch (g.direction)
            {
                case 0:
                    add = temp.Rotation * new Vector3(g.curve.Evaluate(temp.gaitPhase) * gaitMagnitude(g.magnitude), 0, 0);
                    break;
                case 1:
                    add = new Vector3(0, g.curve.Evaluate(temp.gaitPhase) * gaitMagnitude(g.magnitude), 0);
                    break;
                case 2:
                    add = g.curve.Evaluate(temp.gaitPhase) * gaitMagnitude(g.magnitude) * temp.actingVelocityNormal;
                    break;
            }
            anim.elbowRPos.position += add;
        }
    }
}

[Serializable]
public class armSwing
{
    public gaitCurve[] shoulder;
    public gaitCurve[] arm;
    public gaitCurve[] elbow;
}

[Serializable]
public class gaitCurve
{
    public AnimationCurve curve;
    public int direction;
    public gaitMagnitudes magnitude;
}

[Serializable]
public class gaitMagnitudes
{
    public float flat, velocity, side, back;
}