using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class characterBase : MonoBehaviour
{
    public void applyStepBody(stepBodyParams body)
    {
        applyBodyGaitTilt(temp.velocityXZ, body.velocityTilt);

        applyBodyGaitTilt(temp.toFinalVelocity, body.toTargetVelocityTilt);

        applyBodyTilt(temp.environmentInfluence * (1 - temp.pushMagnitude), body.environmentInfluenceTilt);

        applyBodyGaitTilt(temp.finalVelocity, body.targetVelocityGait);

        applyBodyGaitTilt(temp.baseRotation * new Vector3(1, 0, 0), body.swagger);

        applyBodyGaitTurn(body.twist);

        applyPlantFootLean(body);
    }
    public void applyPlantFootLean(stepBodyParams body)
    {
        if (temp.gaitPhase < 0)
        {
            Vector3 direction = memory.legRStep.from.position - skeleton.pelvis.position; direction.y = 0;
            applyBodyGaitTilt(direction, body.plantFootLean);
        }
        else
        {
            Vector3 direction = memory.legLStep.from.position - skeleton.pelvis.position; direction.y = 0;
            applyBodyGaitTilt(direction, body.plantFootLean);
        }
    }
}

[Serializable]
public class stepBodyParams
{
    public bodyGaitRotation velocityTilt;
    public bodyGaitRotation toTargetVelocityTilt;
    public bodyFloats environmentInfluenceTilt;
    public bodyGaitRotation targetVelocityGait;
    public bodyGaitRotation swagger;
    public bodyGaitRotation twist;
    public bodyGaitRotation plantFootLean;
}