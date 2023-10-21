using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class characterBase : MonoBehaviour
{
    public void animationMovement(moveAnimation ma)
    {
        for (int i = 0; i < ma.move.Length; i++)
        {
            float curve = ma.move[i].curve.Evaluate(temp.phase);
            Vector3 add = Vector3.zero;
            switch (ma.move[i].axis)
            {
                case 0:
                    add = new Vector3(curve, 0, 0);
                    break;
                case 1:
                    add = new Vector3(0, curve, 0);
                    break;
                case 2:
                    add = new Vector3(0, 0, curve);
                    break;
            }
            add = temp.Orientation * add;
            temp.acceleration += add;
            memory.velocity += add * Time.fixedDeltaTime;
        }
    }
    public void bodyAnimation(bodyAnimation ba, Quaternion rotation)
    {
        Vector3 position = Vector3.zero;
        for (int i = 0; i < ba.position.Length; i++)
        {
            float curve = ba.position[i].curve.Evaluate(temp.phase);
            switch (ba.position[i].axis)
            {
                case 0:
                    position.x += curve;
                    break;
                case 1:
                    position.y += curve;
                    break;
                case 2:
                    position.z += curve;
                    break;
            }
        }
        anim.bodyPos.position += rotation * position;

        Vector3 tilt = Vector3.zero;
        for (int i = 0; i < ba.pelvis.Length; i++)
        {
            float curve = ba.pelvis[i].curve.Evaluate(temp.phase);
            switch (ba.pelvis[i].axis)
            {
                case 0:
                    tilt.x += curve;
                    break;
                case 1:
                    anim.pelvisRot.y += curve;
                    break;
                case 2:
                    tilt.z += curve;
                    break;
            }
        }
        anim.pelvisRot.tilt += rotation * tilt;

        tilt = Vector3.zero;
        for (int i = 0; i < ba.lowSpine.Length; i++)
        {
            float curve = ba.lowSpine[i].curve.Evaluate(temp.phase);
            switch (ba.lowSpine[i].axis)
            {
                case 0:
                    tilt.x += curve;
                    break;
                case 1:
                    anim.lowSpineRot.y += curve;
                    break;
                case 2:
                    tilt.z += curve;
                    break;
            }
        }
        anim.lowSpineRot.tilt += rotation * tilt;

        tilt = Vector3.zero;
        for (int i = 0; i < ba.highSpine.Length; i++)
        {
            float curve = ba.highSpine[i].curve.Evaluate(temp.phase);
            switch (ba.highSpine[i].axis)
            {
                case 0:
                    tilt.x += curve;
                    break;
                case 1:
                    anim.highSpineRot.y += curve;
                    break;
                case 2:
                    tilt.z += curve;
                    break;
            }
        }
        anim.highSpineRot.tilt += rotation * tilt;

        tilt = Vector3.zero;
        for (int i = 0; i < ba.neck.Length; i++)
        {
            float curve = ba.neck[i].curve.Evaluate(temp.phase);
            switch (ba.neck[i].axis)
            {
                case 0:
                    tilt.x += curve;
                    break;
                case 1:
                    anim.neckRot.y += curve;
                    break;
                case 2:
                    tilt.z += curve;
                    break;
            }
        }
        anim.neckRot.tilt += rotation * tilt;

        tilt = Vector3.zero;
        for (int i = 0; i < ba.head.Length; i++)
        {
            float curve = ba.head[i].curve.Evaluate(temp.phase);
            switch (ba.head[i].axis)
            {
                case 0:
                    tilt.x += curve;
                    break;
                case 1:
                    anim.headRot.y += curve;
                    break;
                case 2:
                    tilt.z += curve;
                    break;
            }
        }
        anim.headRot.tilt += rotation * tilt;
    }
    public void leftShoulderAnimation(armAnimation aa)
    {
        for (int i = 0; i < aa.shoulderRotation.Length; i++)
        {
            float curve = aa.shoulderRotation[i].curve.Evaluate(temp.phase);
            switch (aa.shoulderRotation[i].axis)
            {
                case 0:
                    anim.shoulderLRot.euler.x += curve;
                    break;
                case 1:
                    anim.shoulderLRot.y += curve;
                    break;
                case 2:
                    anim.shoulderLRot.euler.z += curve;
                    break;
            }
        }
    }
    public void rightShoulderAnimation(armAnimation aa)
    {
        for (int i = 0; i < aa.shoulderRotation.Length; i++)
        {
            float curve = aa.shoulderRotation[i].curve.Evaluate(temp.phase);
            switch (aa.shoulderRotation[i].axis)
            {
                case 0:
                    anim.shoulderRRot.euler.x += curve;
                    break;
                case 1:
                    anim.shoulderRRot.y += curve;
                    break;
                case 2:
                    anim.shoulderRRot.euler.z += curve;
                    break;
            }
        }
    }
    public void leftArmAnimation(armAnimation aa, Quaternion rotation)
    {
        for (int i = 0; i < aa.handPosition.Length; i++)
        {
            float curve = aa.handPosition[i].curve.Evaluate(temp.phase);
            switch (aa.handPosition[i].axis)
            {
                case 0:
                    anim.handLPos.position += rotation * new Vector3(curve, 0, 0);
                    break;
                case 1:
                    anim.handLPos.position += rotation * new Vector3(0, curve, 0);
                    break;
                case 2:
                    anim.handLPos.position += rotation * new Vector3(0, 0, curve);
                    break;
            }
        }

        for (int i = 0; i < aa.elbowPosition.Length; i++)
        {
            float curve = aa.elbowPosition[i].curve.Evaluate(temp.phase);
            switch (aa.elbowPosition[i].axis)
            {
                case 0:
                    anim.elbowLPos.position += rotation * new Vector3(curve, 0, 0);
                    break;
                case 1:
                    anim.elbowLPos.position += rotation * new Vector3(0, curve, 0);
                    break;
                case 2:
                    anim.elbowLPos.position += rotation * new Vector3(0, 0, curve);
                    break;
            }
        }
    }
    public void rightArmAnimation(armAnimation aa, Quaternion rotation)
    {
        for (int i = 0; i < aa.handPosition.Length; i++)
        {
            float curve = aa.handPosition[i].curve.Evaluate(temp.phase);
            switch (aa.handPosition[i].axis)
            {
                case 0:
                    anim.handRPos.position += rotation * new Vector3(curve, 0, 0);
                    break;
                case 1:
                    anim.handRPos.position += rotation * new Vector3(0, curve, 0);
                    break;
                case 2:
                    anim.handRPos.position += rotation * new Vector3(0, 0, curve);
                    break;
            }
        }

        for (int i = 0; i < aa.elbowPosition.Length; i++)
        {
            float curve = aa.elbowPosition[i].curve.Evaluate(temp.phase);
            switch (aa.elbowPosition[i].axis)
            {
                case 0:
                    anim.elbowRPos.position += rotation * new Vector3(curve, 0, 0);
                    break;
                case 1:
                    anim.elbowRPos.position += rotation * new Vector3(0, curve, 0);
                    break;
                case 2:
                    anim.elbowRPos.position += rotation * new Vector3(0, 0, curve);
                    break;
            }
        }
    }
    public void leftHandAnimation(armAnimation aa)
    {
        for (int i = 0; i < aa.handRotation.Length; i++)
        {
            float curve = aa.handRotation[i].curve.Evaluate(temp.phase);
            switch (aa.handRotation[i].axis)
            {
                case 0:
                    anim.handLRot.euler.x += curve;
                    break;
                case 1:
                    anim.handLRot.euler.y += curve;
                    break;
                case 2:
                    anim.handLRot.euler.z += curve;
                    break;
            }
        }
    }
    public void rightHandAnimation(armAnimation aa)
    {
        for (int i = 0; i < aa.handRotation.Length; i++)
        {
            float curve = aa.handRotation[i].curve.Evaluate(temp.phase);
            switch (aa.handRotation[i].axis)
            {
                case 0:
                    anim.handRRot.euler.x += curve;
                    break;
                case 1:
                    anim.handRRot.euler.y += curve;
                    break;
                case 2:
                    anim.handRRot.euler.z += curve;
                    break;
            }
        }
    }
}

[Serializable]
public class moveAnimation
{
    public AnimationCurve turnSpeed;
    public axisCurve[] move;
}

[Serializable]
public class armAnimation
{
    public axisCurve[] handPosition;
    public axisCurve[] elbowPosition;
    public axisCurve[] handRotation;
    public axisCurve[] shoulderRotation;
}

[Serializable]
public class bodyAnimation
{
    public AnimationCurve rotation;
    public axisCurve[] position;

    public axisCurve[] pelvis;
    public axisCurve[] lowSpine;
    public axisCurve[] highSpine;
    public axisCurve[] neck;
    public axisCurve[] head;
}

[Serializable]
public class axisCurve
{
    public AnimationCurve curve;
    public int axis;
}