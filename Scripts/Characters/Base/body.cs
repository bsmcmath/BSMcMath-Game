using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class characterBase : MonoBehaviour
{
    public void setLook(lookParams look)
    {
        Vector3 Look = temp.look;
        Look.y = help.angleDifference(memory.rotation, Look.y);

        Look.x = Mathf.Clamp(Look.x, -look.max.x, look.max.x);
        Look.y = Mathf.Clamp(Look.y, -look.max.y, look.max.y);

        memory.look = help.moveTo(memory.look, Look, look.blend);
    }
    public void applyLookEuler(lookParams look)
    {
        setLook(look);

        anim.lowSpineRot.euler += new Vector3(look.lowSpine.x * memory.look.x, look.lowSpine.y * memory.look.y, 0);
        Vector3 highSpine = new(look.highSpine.x * memory.look.x, look.highSpine.y * memory.look.y, 0);
        anim.highSpineRot.euler += highSpine;
        anim.shoulderLRot.euler += highSpine;
        anim.shoulderRRot.euler += highSpine;
        anim.neckRot.euler += new Vector3(look.neck.x * memory.look.x, look.neck.y * memory.look.y, 0);
        anim.headRot.euler += new Vector3(look.head.x * memory.look.x, look.head.y * memory.look.y, 0);
    }
    public void applyLook_XEuler_YRotation(lookParams look)
    {
        setLook(look);

        anim.lowSpineRot.euler.x += look.lowSpine.x * memory.look.x;
        float highSpine = look.highSpine.x * memory.look.x;
        anim.highSpineRot.euler.x += highSpine;
        anim.shoulderLRot.euler.x += highSpine;
        anim.shoulderRRot.euler.x += highSpine;
        anim.neckRot.euler.x += look.neck.x * memory.look.x;
        anim.headRot.euler.x += look.head.x * memory.look.x;

        anim.lowSpineRot.y += look.lowSpine.y * memory.look.y;
        highSpine = look.highSpine.y * memory.look.y;
        anim.highSpineRot.y += highSpine;
        anim.shoulderLRot.y += highSpine;
        anim.shoulderRRot.y += highSpine;
        anim.neckRot.y += look.neck.y * memory.look.y;
        anim.headRot.y += look.head.y * memory.look.y;
    }
    //
    public void applyBodyTilt(Vector3 vector)
    {
        anim.pelvisRot.tilt += vector;
        anim.lowSpineRot.tilt += vector;

        anim.highSpineRot.tilt += vector;
        anim.shoulderLRot.tilt += vector;
        anim.shoulderRRot.tilt += vector;

        anim.neckRot.tilt += vector;
        anim.headRot.tilt += vector;
    }
    public void applyBodyTilt(Vector3 vector, bodyFloats mags)
    {
        anim.pelvisRot.tilt += vector * mags.pelvis;
        anim.lowSpineRot.tilt += vector * mags.lowSpine;

        Vector3 highSpine = vector * mags.highSpine;
        anim.highSpineRot.tilt += highSpine;
        anim.shoulderLRot.tilt += highSpine;
        anim.shoulderRRot.tilt += highSpine;

        anim.neckRot.tilt += vector * mags.neck;
        anim.headRot.tilt += vector * mags.head;
    }
    public void applyBodyGaitTilt(Vector3 vector, bodyGaitRotation body)
    {
        float magnitude = gaitMagnitude(body.baseMagnitude);

        anim.pelvisRot.tilt += body.curve.Evaluate(help.gaitPhaseWrap(temp.gaitPhase, body.phaseOffsets.pelvis)) * body.magnitudes.pelvis * magnitude * vector;
        anim.lowSpineRot.tilt += body.curve.Evaluate(help.gaitPhaseWrap(temp.gaitPhase, body.phaseOffsets.lowSpine)) * body.magnitudes.lowSpine * magnitude * vector;

        Vector3 highSpine = body.curve.Evaluate(help.gaitPhaseWrap(temp.gaitPhase, body.phaseOffsets.highSpine)) * body.magnitudes.highSpine * magnitude * vector;
        anim.highSpineRot.tilt += highSpine;
        anim.shoulderLRot.tilt += highSpine;
        anim.shoulderRRot.tilt += highSpine;

        anim.neckRot.tilt += body.curve.Evaluate(help.gaitPhaseWrap(temp.gaitPhase, body.phaseOffsets.neck)) * body.magnitudes.neck * magnitude * vector;
        anim.headRot.tilt += body.curve.Evaluate(help.gaitPhaseWrap(temp.gaitPhase, body.phaseOffsets.head)) * body.magnitudes.head * magnitude * vector;
    }
    public void applyBodyGaitTurn(bodyGaitRotation body)
    {
        float magnitude = gaitMagnitude(body.baseMagnitude);

        anim.pelvisRot.y += body.curve.Evaluate(help.gaitPhaseWrap(temp.gaitPhase, body.phaseOffsets.pelvis)) * body.magnitudes.pelvis * magnitude;
        anim.lowSpineRot.y += body.curve.Evaluate(help.gaitPhaseWrap(temp.gaitPhase, body.phaseOffsets.lowSpine)) * body.magnitudes.lowSpine * magnitude;

        float highSpine = body.curve.Evaluate(help.gaitPhaseWrap(temp.gaitPhase, body.phaseOffsets.highSpine)) * body.magnitudes.highSpine * magnitude;
        anim.highSpineRot.y += highSpine;
        anim.shoulderLRot.y += highSpine;
        anim.shoulderRRot.y += highSpine;

        anim.neckRot.y += body.curve.Evaluate(help.gaitPhaseWrap(temp.gaitPhase, body.phaseOffsets.neck)) * body.magnitudes.neck * magnitude;
        anim.headRot.y += body.curve.Evaluate(help.gaitPhaseWrap(temp.gaitPhase, body.phaseOffsets.head)) * body.magnitudes.head * magnitude;
    }
    //
    public void applyBodyBaseRotation_Step()
    {
        anim.pelvisRot.y += memory.baseRotation;
        anim.lowSpineRot.y += Mathf.LerpAngle(memory.baseRotation, memory.rotation, 0.5f);
        anim.highSpineRot.y += memory.rotation;
        anim.neckRot.y += memory.rotation;
        anim.headRot.y += memory.rotation;
        anim.shoulderLRot.y += memory.rotation;
        anim.shoulderRRot.y += memory.rotation;
    }
    public void applyBodyBaseRotation()
    {
        anim.pelvisRot.y += memory.rotation;
        anim.lowSpineRot.y += memory.rotation;
        anim.highSpineRot.y += memory.rotation;
        anim.neckRot.y += memory.rotation;
        anim.headRot.y += memory.rotation;
        anim.shoulderLRot.y += memory.rotation;
        anim.shoulderRRot.y += memory.rotation;
    }
    //
    public void setBodyBlend(blendSettings rotation, blendSettings position)
    {
        anim.bodyPos.blend = position;
        anim.pelvisRot.blend = rotation;
        anim.lowSpineRot.blend = rotation;
        anim.highSpineRot.blend = rotation;
        anim.neckRot.blend = rotation;
        anim.headRot.blend = rotation;
        anim.shoulderLRot.blend = rotation;
        anim.shoulderRRot.blend = rotation;
    }
}

[Serializable]
public class lookParams
{
    public blendSettings blend;

    public Vector2 max;

    public Vector2 lowSpine, highSpine, neck, head;
}

[Serializable]
public class bodyFloats
{
    public float pelvis, lowSpine, highSpine, neck, head;
}

[Serializable]
public class bodyGaitRotation
{
    public AnimationCurve curve;
    public bodyFloats magnitudes;
    public bodyFloats phaseOffsets;
    public gaitMagnitudes baseMagnitude;
}