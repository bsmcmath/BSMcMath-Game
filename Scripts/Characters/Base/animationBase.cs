using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class characterBase : MonoBehaviour
{
    public void setArma(capsuleSettings cs, Quaternion rotation)
    {
        blending.arma.update();

        Quaternion pelvis = skeleton.pelvis.rotation;
        if (blending.arma.blend)
        {
            float lerp = blending.arma.timer / blending.arma.duration;
            skeleton.arma.rotation = Quaternion.Slerp(skeleton.arma.rotation, rotation * basis.arma, lerp);
            components.armaCol.center = Vector3.Lerp(components.armaCol.center, cs.center, lerp);
            components.armaCol.height = Mathf.Lerp(components.armaCol.height, cs.height, lerp);
            components.armaCol.radius = Mathf.Lerp(components.armaCol.radius, cs.radius, lerp);
        }
        else
        {
            skeleton.arma.rotation = rotation * basis.arma;
            components.armaCol.center = cs.center;
            components.armaCol.height = cs.height;
            components.armaCol.radius = cs.radius;
        }
        skeleton.pelvis.rotation = pelvis;
    }
    //
    public void setBodyNormals(Vector3 normal)
    {
        anim.pelvisRot.normal = normal;
        anim.lowSpineRot.normal = normal;
        anim.highSpineRot.normal = normal;
        anim.neckRot.normal = normal;
        anim.headRot.normal = normal;
        anim.shoulderLRot.normal = normal;
        anim.shoulderRRot.normal = normal;
    }
    public void calculate_TorsoHeadShoulders()
    {
        anim.pelvisRot.rotation = 
            Quaternion.FromToRotation(Vector3.up, anim.pelvisRot.normal) *
            help.tiltRotation(anim.pelvisRot.tilt, anim.pelvisRot.tilt.magnitude) *
            Quaternion.Euler(0, anim.pelvisRot.y, 0) *
            Quaternion.Euler(anim.pelvisRot.euler);
        anim.lowSpineRot.rotation = 
            Quaternion.FromToRotation(Vector3.up, anim.lowSpineRot.normal) *
            help.tiltRotation(anim.lowSpineRot.tilt, anim.lowSpineRot.tilt.magnitude) *
            Quaternion.Euler(0, anim.lowSpineRot.y, 0) *
            Quaternion.Euler(anim.lowSpineRot.euler);
        anim.highSpineRot.rotation =
            Quaternion.FromToRotation(Vector3.up, anim.highSpineRot.normal) *
            help.tiltRotation(anim.highSpineRot.tilt, anim.highSpineRot.tilt.magnitude) *
            Quaternion.Euler(0, anim.highSpineRot.y, 0) *
            Quaternion.Euler(anim.highSpineRot.euler);
        anim.shoulderLRot.rotation =
            Quaternion.FromToRotation(Vector3.up, anim.shoulderLRot.normal) *
            help.tiltRotation(anim.shoulderLRot.tilt, anim.shoulderLRot.tilt.magnitude) *
            Quaternion.Euler(0, anim.shoulderLRot.y, 0) *
            Quaternion.Euler(anim.shoulderLRot.euler);
        anim.shoulderRRot.rotation =
            Quaternion.FromToRotation(Vector3.up, anim.shoulderRRot.normal) *
            help.tiltRotation(anim.shoulderRRot.tilt, anim.shoulderRRot.tilt.magnitude) *
            Quaternion.Euler(0, anim.shoulderRRot.y, 0) *
            Quaternion.Euler(anim.shoulderRRot.euler);
        anim.neckRot.rotation =
            Quaternion.FromToRotation(Vector3.up, anim.neckRot.normal) *
            help.tiltRotation(anim.neckRot.tilt, anim.neckRot.tilt.magnitude) *
            Quaternion.Euler(0, anim.neckRot.y, 0) *
            Quaternion.Euler(anim.neckRot.euler);
        anim.headRot.rotation =
            Quaternion.FromToRotation(Vector3.up, anim.headRot.normal) *
            help.tiltRotation(anim.headRot.tilt, anim.headRot.tilt.magnitude) *
            Quaternion.Euler(0, anim.headRot.y, 0) *
            Quaternion.Euler(anim.headRot.euler);
    }
    public void animate_TorsoHeadShoulders()
    {
        calculate_TorsoHeadShoulders();

        memory.localPosition = help.moveTo(memory.localPosition, anim.bodyPos.position, anim.bodyPos.blend);
        skeleton.pelvis.localPosition = skeleton.arma.InverseTransformDirection(memory.localPosition) + basis.pos;

        Quaternion pelvis = help.moveTo(skeleton.pelvis.rotation, anim.pelvisRot.rotation * basis.pelvis, anim.pelvisRot.blend);
        Quaternion lowSpine = help.moveTo(skeleton.lowSpine.rotation, anim.lowSpineRot.rotation * basis.lowSpine, anim.lowSpineRot.blend);
        Quaternion highSpine = help.moveTo(skeleton.highSpine.rotation, anim.highSpineRot.rotation * basis.highSpine, anim.highSpineRot.blend);
        Quaternion shoulderL = help.moveTo(skeleton.shoulderL.rotation, anim.shoulderLRot.rotation * basis.shoulderL, anim.shoulderLRot.blend);
        Quaternion shoulderR = help.moveTo(skeleton.shoulderR.rotation, anim.shoulderRRot.rotation * basis.shoulderR, anim.shoulderRRot.blend);
        Quaternion neck = help.moveTo(skeleton.neck.rotation, anim.neckRot.rotation * basis.neck, anim.neckRot.blend);
        Quaternion head = help.moveTo(skeleton.head.rotation, anim.headRot.rotation * basis.head, anim.headRot.blend);

        skeleton.pelvis.rotation = pelvis;
        skeleton.lowSpine.rotation = lowSpine;
        skeleton.highSpine.rotation = highSpine;
        skeleton.shoulderL.rotation = shoulderL;
        skeleton.shoulderR.rotation = shoulderR;
        skeleton.neck.rotation = neck;
        skeleton.head.rotation = head;
    }
    //
    public void animateLeftLeg()
    {
        limitLeftLegLength();

        memory.footLLocalPosition = anim.footLPos.blend.type == blendType.none ? anim.footLPos.position - skeleton.highLegL.position :
            help.moveTo(memory.footLLocalPosition, anim.footLPos.position - skeleton.highLegL.position, anim.footLPos.blend.lerp, anim.footLPos.blend.flat, anim.footLPos.blend.type == blendType.min);
        memory.kneeLLocalPosition = anim.kneeLPos.blend.type == blendType.none ? anim.kneeLPos.position - skeleton.highLegL.position :
            help.moveTo(memory.kneeLLocalPosition, anim.kneeLPos.position - skeleton.highLegL.position, anim.kneeLPos.blend.lerp, anim.kneeLPos.blend.flat, anim.kneeLPos.blend.type == blendType.min);

        components.legLIK.Target.position = memory.footLLocalPosition + skeleton.highLegL.position;
        components.legLIK.Pole.position = memory.kneeLLocalPosition + skeleton.highLegL.position;
        components.legLIK.ResolveIK();
        skeleton.lowLegL.position += components.legLIK.Target.position - skeleton.footL.position;

        anim.footLRot.rotation *= basis.footL;
        memory.footLLocalRotation = anim.footLRot.blend.type == blendType.none ? Quaternion.Inverse(skeleton.lowLegL.rotation) * anim.footLRot.rotation :
            help.moveTo(memory.footLLocalRotation, Quaternion.Inverse(skeleton.lowLegL.rotation) * anim.footLRot.rotation, anim.footLRot.blend.lerp, anim.footLRot.blend.flat, anim.footLRot.blend.type == blendType.min);
        skeleton.footL.rotation = skeleton.lowLegL.rotation * memory.footLLocalRotation;
    }
    public void animateRightLeg()
    {
        limitRightLegLength();

        memory.footRLocalPosition = anim.footRPos.blend.type == blendType.none ? anim.footRPos.position - skeleton.highLegR.position :
            help.moveTo(memory.footRLocalPosition, anim.footRPos.position - skeleton.highLegR.position, anim.footRPos.blend.lerp, anim.footRPos.blend.flat, anim.footRPos.blend.type == blendType.min);
        memory.kneeRLocalPosition = anim.kneeRPos.blend.type == blendType.none ? anim.kneeRPos.position - skeleton.highLegR.position :
            help.moveTo(memory.kneeRLocalPosition, anim.kneeRPos.position - skeleton.highLegR.position, anim.kneeRPos.blend.lerp, anim.kneeRPos.blend.flat, anim.kneeRPos.blend.type == blendType.min);

        components.legRIK.Target.position = memory.footRLocalPosition + skeleton.highLegR.position;
        components.legRIK.Pole.position = memory.kneeRLocalPosition + skeleton.highLegR.position;
        components.legRIK.ResolveIK();
        skeleton.lowLegR.position += components.legRIK.Target.position - skeleton.footR.position;

        anim.footRRot.rotation *= basis.footR;
        memory.footRLocalRotation = anim.footRRot.blend.type == blendType.none ? Quaternion.Inverse(skeleton.lowLegR.rotation) * anim.footRRot.rotation :
            help.moveTo(memory.footRLocalRotation, Quaternion.Inverse(skeleton.lowLegR.rotation) * anim.footRRot.rotation, anim.footRRot.blend.lerp, anim.footRRot.blend.flat, anim.footRRot.blend.type == blendType.min);
        skeleton.footR.rotation = skeleton.lowLegR.rotation * memory.footRLocalRotation;
    }
    public void animateLegs()
    {
        animateLeftLeg(); animateRightLeg();
    }
    //
    public void animateLeftArm()
    {
        limitLeftArmLength();

        blending.armL.update();

        if (blending.armL.blend)
        {
            float lerp = blending.armL.timer / blending.armL.duration;
            anim.handLPos.position = Vector3.Lerp(memory.handLLocalPosition + skeleton.highArmL.position, anim.handLPos.position, lerp);
            anim.elbowLPos.position = Vector3.Lerp(memory.elbowLLocalPosition + skeleton.highArmL.position, anim.elbowLPos.position, lerp);
        }

        memory.handLLocalPosition = anim.handLPos.blend.type == blendType.none ? anim.handLPos.position - skeleton.highArmL.position :
            help.moveTo(memory.handLLocalPosition, anim.handLPos.position - skeleton.highArmL.position, anim.handLPos.blend.lerp, anim.handLPos.blend.flat, anim.handLPos.blend.type == blendType.min);
        memory.elbowLLocalPosition = anim.elbowLPos.blend.type == blendType.none ? anim.elbowLPos.position - skeleton.highArmL.position :
            help.moveTo(memory.elbowLLocalPosition, anim.elbowLPos.position - skeleton.highArmL.position, anim.elbowLPos.blend.lerp, anim.elbowLPos.blend.flat, anim.elbowLPos.blend.type == blendType.min);

        components.armLIK.Target.position = memory.handLLocalPosition + skeleton.highArmL.position;
        components.armLIK.Pole.position = memory.elbowLLocalPosition + skeleton.highArmL.position;
        components.armLIK.ResolveIK();
        skeleton.lowArmL.position += components.armLIK.Target.position - skeleton.handL.position;
    }
    public void animateRightArm()
    {
        limitRightArmLength();

        blending.armR.update();
        if (blending.armR.blend)
        {
            blending.armR.value(out float lerp);
            anim.handRPos.position = Vector3.Lerp(memory.handRLocalPosition + skeleton.highArmR.position, anim.handRPos.position, lerp);
            anim.elbowRPos.position = Vector3.Lerp(memory.elbowRLocalPosition + skeleton.highArmR.position, anim.elbowRPos.position, lerp);
        }

        memory.handRLocalPosition = anim.handRPos.blend.type == blendType.none ? anim.handRPos.position - skeleton.highArmR.position :
            help.moveTo(memory.handRLocalPosition, anim.handRPos.position - skeleton.highArmR.position, anim.handRPos.blend.lerp, anim.handRPos.blend.flat, anim.handRPos.blend.type == blendType.min);
        memory.elbowRLocalPosition = anim.elbowRPos.blend.type == blendType.none ? anim.elbowRPos.position - skeleton.highArmR.position :
            help.moveTo(memory.elbowRLocalPosition, anim.elbowRPos.position - skeleton.highArmR.position, anim.elbowRPos.blend.lerp, anim.elbowRPos.blend.flat, anim.elbowRPos.blend.type == blendType.min);

        components.armRIK.Target.position = memory.handRLocalPosition + skeleton.highArmR.position;
        components.armRIK.Pole.position = memory.elbowRLocalPosition + skeleton.highArmR.position;
        components.armRIK.ResolveIK();
        skeleton.lowArmR.position += components.armRIK.Target.position - skeleton.handR.position;
    }
    public void animateLeftHand()
    {
        anim.handLRot.rotation *= Quaternion.Euler(anim.handLRot.euler);

        if (blending.armL.blend)
        {
            float lerp = blending.armL.timer / blending.armL.duration;
            anim.handLRot.rotation = Quaternion.Slerp(skeleton.lowArmL.rotation * memory.handLLocalRotation, anim.handLRot.rotation, lerp);
        }

        anim.handLRot.rotation *= basis.handL;
        if (anim.handLRotWorldSpace)
        {
            memory.handLWorldRotation = anim.handLRot.blend.type == blendType.none ? anim.handLRot.rotation :
            help.moveTo(memory.handLWorldRotation, anim.handLRot.rotation, anim.handLRot.blend.lerp, anim.handLRot.blend.flat, anim.handLRot.blend.type == blendType.min);
            skeleton.handL.rotation = memory.handLWorldRotation;
            memory.handLLocalRotation = Quaternion.Inverse(skeleton.lowArmL.rotation) * skeleton.handL.rotation;
        }
        else
        {
            memory.handLLocalRotation = anim.handLRot.blend.type == blendType.none ? Quaternion.Inverse(skeleton.lowArmL.rotation) * anim.handLRot.rotation :
            help.moveTo(memory.handLLocalRotation, Quaternion.Inverse(skeleton.lowArmL.rotation) * anim.handLRot.rotation, anim.handLRot.blend.lerp, anim.handLRot.blend.flat, anim.handLRot.blend.type == blendType.min);
            skeleton.handL.rotation = skeleton.lowArmL.rotation * memory.handLLocalRotation;
            memory.handLWorldRotation = skeleton.handL.rotation;
        }

        Vector3 axis = skeleton.lowArmL.position - skeleton.handL.position;
        Vector3 forward = skeleton.handL.rotation * basis.armLTwistFwdHand;
        float angle = Vector3.Angle(forward, axis);
        forward = Vector3.SlerpUnclamped(axis, forward, 90 / angle);
        skeleton.twistArmL.rotation = Quaternion.LookRotation(forward, axis);
    }
    public void animateRightHand()
    {
        anim.handRRot.rotation *= Quaternion.Euler(anim.handRRot.euler);

        if (blending.armR.blend)
        {
            float lerp = blending.armR.timer / blending.armR.duration;
            anim.handRRot.rotation = Quaternion.Slerp(skeleton.lowArmR.rotation * memory.handRLocalRotation, anim.handRRot.rotation, lerp);
        }

        anim.handRRot.rotation *= basis.handR;
        if (anim.handRRotWorldSpace)
        {
            memory.handRWorldRotation = anim.handRRot.blend.type == blendType.none ? anim.handRRot.rotation :
            help.moveTo(memory.handRWorldRotation, anim.handRRot.rotation, anim.handRRot.blend.lerp, anim.handRRot.blend.flat, anim.handRRot.blend.type == blendType.min);
            skeleton.handR.rotation = memory.handRWorldRotation;
            memory.handRLocalRotation = Quaternion.Inverse(skeleton.lowArmR.rotation) * skeleton.handR.rotation;
        }
        else
        {
            memory.handRLocalRotation = anim.handRRot.blend.type == blendType.none ? Quaternion.Inverse(skeleton.lowArmR.rotation) * anim.handRRot.rotation :
            help.moveTo(memory.handRLocalRotation, Quaternion.Inverse(skeleton.lowArmR.rotation) * anim.handRRot.rotation, anim.handRRot.blend.lerp, anim.handRRot.blend.flat, anim.handRRot.blend.type == blendType.min);
            skeleton.handR.rotation = skeleton.lowArmR.rotation * memory.handRLocalRotation;
            memory.handRWorldRotation = skeleton.handR.rotation;
        }

        Vector3 axis = skeleton.lowArmR.position - skeleton.handR.position;
        Vector3 forward = skeleton.handR.rotation * basis.armRTwistFwdHand;
        float angle = Vector3.Angle(forward, axis);
        forward = Vector3.SlerpUnclamped(axis, forward, 90 / angle);
        skeleton.twistArmR.rotation = Quaternion.LookRotation(forward, axis);
    }
}

public class characterAnimationFrameInfo
{
    public positionSetting bodyPos;
    public positionSetting footLPos, kneeLPos, footRPos, kneeRPos;
    public positionSetting handLPos, elbowLPos, handRPos, elbowRPos;

    public rotationSetting footLRot, footRRot;
    public rotationSetting handLRot, handRRot;
    public bool handLRotWorldSpace, handRRotWorldSpace;

    public rotationSetting pelvisRot, lowSpineRot, highSpineRot, neckRot, headRot;

    public rotationSetting shoulderLRot, shoulderRRot;
}
public struct positionSetting
{
    public Vector3 position;
    public blendSettings blend;
}
public struct rotationSetting
{
    public float y;
    public Vector3 normal, tilt, euler;
    public Quaternion rotation;
    public blendSettings blend;
}
public enum blendType
{
    none, min, max
}
[Serializable]
public struct blendSettings
{
    public float lerp, flat;
    public blendType type;

    //public blendSettings() { }
    public blendSettings(blendType bt)
    {
        lerp = 0; flat = 0; type = bt;
    }
}
[Serializable]
public class blendParams
{
    public blendSettings body, leftLeg, rightLeg, leftArm, rightArm;
}
public struct characterTransientBlending
{
    public transientBlendSettings armL, armR;
    public transientBlendSettings arma;

}
public struct transientBlendSettings
{
    public bool blend;
    public float timer, duration;
    public void start(float time)
    {
        blend = true;
        timer = 0;
        duration = time;
    }
    public void update()
    {
        if (!blend) return;
        timer += Time.fixedDeltaTime;
        if (timer > duration) blend = false;
    }
    public void value(out float lerpPhase)
    {
        lerpPhase = timer / duration;
    }
}
[Serializable]
public class capsuleSettings
{
    public Vector3 center;
    public float height, radius;
}
[Serializable]
public class stateParams
{
    public fullPose pose;

    public capsuleSettings cap;

    public lookParams look;

    public blendParams blend;
}