using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class characterBase : MonoBehaviour
{
    public void stepLegs(stepParams step, legPose leftLeg, legPose rightLeg, stepEffects effects)
    {
        stepPausePhase(step);

        if (memory.legLStep.stepping)
        {
            temp.leftTimerInitial = memory.legLStep.timer;
            memory.legLStep.timer += memory.legLStep.speed * Time.fixedDeltaTime;

            if (memory.legLStep.timer > temp.pausePhase)
            {
                if (!memory.legLStep.planted) legLPlantStep(step, effects);
                legLPlanted(step, leftLeg);
            }
            else
            {
                if (!memory.legLStep.landing) adjustLegLStep(step, leftLeg);
                else landingLegLStep(step);
                legLStepping(step, leftLeg);
            }
            legRPlantKick(step, rightLeg, effects);
        }
        else if (memory.legRStep.stepping)
        {
            temp.rightTimerInitial = memory.legRStep.timer;
            memory.legRStep.timer += memory.legRStep.speed * Time.fixedDeltaTime;

            if (memory.legRStep.timer > temp.pausePhase)
            {
                if (!memory.legRStep.planted) legRPlantStep(step, effects);
                legRPlanted(step, rightLeg);
            }
            else
            {
                if (!memory.legRStep.landing) adjustLegRStep(step, rightLeg);
                else landingLegRStep(step);
                legRStepping(step, rightLeg);
            }
            legLPlantKick(step, leftLeg, effects);
        }
        else
        {
            legLPlantKick(step, leftLeg, effects);
            legRPlantKick(step, rightLeg, effects);
        }

        if (memory.legLStep.stepping)
        {
            if (memory.legLStep.timer > 1)
            {
                memory.legLStep.stepping = false;
                if (!memory.legLStep.planted) legLPlantStep(step, effects);
                chooseLegStep(step, leftLeg, rightLeg, memory.legLStep.timer - 1, effects);
            }
        }
        else if (memory.legRStep.stepping)
        {
            if (memory.legRStep.timer > 1)
            {
                memory.legRStep.stepping = false;
                if (!memory.legRStep.planted) legRPlantStep(step, effects);
                chooseLegStep(step, leftLeg, rightLeg, memory.legRStep.timer - 1, effects);
            }
        }
        else chooseLegStep(step, leftLeg, rightLeg, 0, effects);

        gaitPhase();
        baseRotation(step);
        setKnees(leftLeg, rightLeg);
        splashEffect(step, effects);
    }
    //
    public void adjustLegLStep(stepParams step, legPose leg)
    {
        stepGaitSpeed(step, out float speed);
        //memory.legLStep.speed = Mathf.Max(memory.legLStep.speed, speed);
        memory.legLStep.speed = speed;
        float phase = memory.legLStep.timer / temp.pausePhase;
        float remainingStepTime = (1 - phase) / memory.legLStep.speed;
        characterMovementPredictions predictions = new characterMovementPredictions(this, remainingStepTime);

        legLStepBase(step, leg, predictions, out Vector3 position);
        legLStepAvoidClipping(step, ref position);
        position.y = temp.groundCheck.position.y + basis.legLength * 2;
        legLStepCheck(position, out terrainHit target);
        if (!target.hit) { }

        memory.legLStep.currentDirect += temp.terrainMove;
        legLStepPathing(memory.legLStep.currentDirect, target.position, out pathingInfo path);
        Vector3 newCurrentPosition = memory.legLStep.currentDirect;
        if (path.obstructed)
        {
            Vector3 localPlant = memory.legLStep.currentDirect - memory.legRStep.from.position;
            newCurrentPosition = temp.Rotation * Quaternion.Inverse(temp.previousRotation) * localPlant + memory.legRStep.from.position;
            legLStepPathing(newCurrentPosition, target.position, out path);
        }

        legLDistanceLimiting(step, target.position, path, out position);

        position.y = temp.groundCheck.position.y + basis.legLength * 2;
        legLStepCheck(position, out target);
        if (!target.hit) { }

        legLStepPathing(newCurrentPosition, target.position, out path);

        Vector3 direction = path.pt1 - newCurrentPosition;
        bool directionConstrained = Vector3.Angle(direction, memory.legLStep.direction) > 150;
        if (directionConstrained) //plant in place
        {
            position = memory.legLStep.currentDirect; position.y += basis.legLength * 2;
            legLStepCheck(position, out target);
            if (!target.hit) { }
            memory.legLStep.to = target;

            legLStepPathing(memory.legLStep.currentDirect, memory.legLStep.to.position, out memory.legLStep.path);
            memory.legLStep.landing = true;
            memory.legLStep.speed *= 2;
        }
        else
        {
            bool distanceConstrained = (path.distance / remainingStepTime) > step.maxStepSpeed;
            if (distanceConstrained) //use previous target
            {
                legLStepPathing(memory.legLStep.currentDirect, memory.legLStep.to.position, out memory.legLStep.path);
            }
            else //new path works
            {
                memory.legLStep.direction = direction;
                memory.legLStep.currentDirect = newCurrentPosition;
                memory.legLStep.to = target;
                memory.legLStep.path = path;
            }
        }
    }
    public void adjustLegRStep(stepParams step, legPose leg)
    {
        stepGaitSpeed(step, out float speed);
        //memory.legRStep.speed = Mathf.Max(memory.legRStep.speed, speed);
        memory.legRStep.speed = speed;
        float phase = memory.legRStep.timer / temp.pausePhase;
        float remainingStepTime = (1 - phase) / memory.legRStep.speed;
        characterMovementPredictions predictions = new characterMovementPredictions(this, remainingStepTime);

        legRStepBase(step, leg, predictions, out Vector3 position);
        legRStepAvoidClipping(step, ref position);
        position.y = temp.groundCheck.position.y + basis.legLength * 2;
        legRStepCheck(position, out terrainHit target);
        if (!target.hit) { }

        memory.legRStep.currentDirect += temp.terrainMove;
        legRStepPathing(memory.legRStep.currentDirect, target.position, out pathingInfo path);
        Vector3 newCurrentPosition = memory.legRStep.currentDirect;
        if (memory.legRStep.path.obstructed)
        {
            Vector3 localPlant = memory.legRStep.currentDirect - memory.legLStep.from.position;
            newCurrentPosition = temp.Rotation * Quaternion.Inverse(temp.previousRotation) * localPlant + memory.legLStep.from.position;
            legRStepPathing(newCurrentPosition, target.position, out path);
        }

        legRDistanceLimiting(step, target.position, path, out position);

        position.y = temp.groundCheck.position.y + basis.legLength * 2;
        legRStepCheck(position, out target);
        if (!target.hit) { }

        legRStepPathing(newCurrentPosition, target.position, out path);

        Vector3 direction = path.pt1 - newCurrentPosition;
        bool directionConstrained = Vector3.Angle(direction, memory.legRStep.direction) > 150;
        if (directionConstrained) //plant in place
        {
            position = memory.legRStep.currentDirect; position.y += basis.legLength * 2;
            legRStepCheck(position, out target);
            if (!target.hit) { }
            memory.legLStep.to = target;

            legRStepPathing(memory.legRStep.currentDirect, memory.legRStep.to.position, out memory.legRStep.path);
            memory.legRStep.landing = true;
            memory.legRStep.speed *= 2;
        }
        else
        {
            bool distanceConstrained = (path.distance / remainingStepTime) > step.maxStepSpeed;
            if (distanceConstrained) //use previous target
            {
                legRStepPathing(memory.legRStep.currentDirect, memory.legRStep.to.position, out memory.legRStep.path);
            }
            else //new path works
            {
                memory.legRStep.direction = direction;
                memory.legRStep.currentDirect = newCurrentPosition;
                memory.legRStep.to = target;
                memory.legRStep.path = path;
            }
        }
    }
    public void landingLegLStep(stepParams step)
    {
        legLStepPathing(memory.legLStep.currentDirect, memory.legLStep.to.position, out memory.legLStep.path);

        //avoid clipping
    }
    public void landingLegRStep(stepParams step)
    {
        legRStepPathing(memory.legRStep.currentDirect, memory.legRStep.to.position, out memory.legRStep.path);

        //avoid clipping
    }
    public void chooseLegStep(stepParams step, legPose leftLeg, legPose rightLeg, float remainder, stepEffects effects)
    {
        stepGaitSpeed(step, out float speed);
        memory.legLStep.speed = speed;
        memory.legRStep.speed = speed;

        memory.legLStep.timer = remainder;
        temp.leftTimerInitial = memory.legLStep.timer;
        memory.legRStep.timer = remainder;
        temp.rightTimerInitial = memory.legRStep.timer;

        float startPhase = remainder / temp.pausePhase;
        float remainingStepTime = (1 - startPhase) / speed;
        characterMovementPredictions predictions = new characterMovementPredictions(this, remainingStepTime);

        memory.legLStep.currentDirect = memory.legLStep.from.position;
        memory.legRStep.currentDirect = memory.legRStep.from.position;

        legLStepBase(step, leftLeg, predictions, out Vector3 leftTarget);
        legRStepBase(step, rightLeg, predictions, out Vector3 rightTarget);

        float leftTargetDistance = help.distanceXZ(leftTarget, memory.legLStep.from.position);
        float rightTargetDistance = help.distanceXZ(rightTarget, memory.legRStep.from.position);

        legLStepAvoidClipping(step, ref leftTarget);
        legRStepAvoidClipping(step, ref rightTarget);

        //float leftDistance = help.distanceXZ(leftTarget, memory.legLStep.from.position);
        //if (leftDistance < leftTargetDistance) memory.legLStep.speed *= help.map(leftDistance / leftTargetDistance, 0.5f, 1, 2, 1);

        //float rightDistance = help.distanceXZ(rightTarget, memory.legRStep.from.position);
        //if (rightDistance < rightTargetDistance) memory.legRStep.speed *= help.map(rightDistance / rightTargetDistance, 0.5f, 1, 2, 1);

        leftTarget.y = temp.groundCheck.position.y + basis.legLength * 2;
        legLStepCheck(leftTarget, out memory.legLStep.to);
        if (!memory.legLStep.to.hit) { }

        rightTarget.y = temp.groundCheck.position.y + basis.legLength * 2;
        legRStepCheck(rightTarget, out memory.legRStep.to);
        if (!memory.legRStep.to.hit) { }

        memory.legLStep.distanceDirect = Mathf.Max(step.maxStepDistance, help.distance(memory.legLStep.currentDirect, memory.legLStep.to.position) * 1.2f);
        memory.legRStep.distanceDirect = Mathf.Max(step.maxStepDistance, help.distance(memory.legRStep.currentDirect, memory.legRStep.to.position) * 1.2f);

        legLStepPathing(memory.legLStep.currentDirect, memory.legLStep.to.position, out memory.legLStep.path);
        legRStepPathing(memory.legRStep.currentDirect, memory.legRStep.to.position, out memory.legRStep.path);
        //

        legLDistanceLimiting(step, memory.legLStep.to.position, memory.legLStep.path, out leftTarget);
        legRDistanceLimiting(step, memory.legRStep.to.position, memory.legRStep.path, out rightTarget);

        leftTarget.y = temp.groundCheck.position.y + basis.legLength * 2;
        legLStepCheck(leftTarget, out memory.legLStep.to);
        if (!memory.legLStep.to.hit) { }

        rightTarget.y = temp.groundCheck.position.y + basis.legLength * 2;

        legRStepCheck(rightTarget, out memory.legRStep.to);
        if (!memory.legRStep.to.hit) { }

        legLStepPathing(memory.legLStep.currentDirect, memory.legLStep.to.position, out memory.legLStep.path);
        legRStepPathing(memory.legRStep.currentDirect, memory.legRStep.to.position, out memory.legRStep.path);
        //

        float leftDist = help.distanceXZ(memory.legLStep.from.position, memory.legLStep.to.position);
        float rightDist = help.distanceXZ(memory.legRStep.from.position, memory.legRStep.to.position);

        bool leftOutOfPlace = leftDist > step.stepDistance;
        bool rightOutOfPlace = rightDist > step.stepDistance;

        if (leftOutOfPlace && rightOutOfPlace)
        {
            if (memory.leftStepLast) startLegRStep(step, rightLeg, effects);
            else startLegLStep(step, leftLeg, effects);
        }
        else if (leftOutOfPlace) startLegLStep(step, leftLeg, effects);
        else if (rightOutOfPlace) startLegRStep(step, rightLeg, effects);
    }
    public void startLegLStep(stepParams step, legPose leg, stepEffects effects)
    {
        memory.legLStep.direction = memory.legLStep.path.pt1 - memory.legLStep.currentDirect;

        memory.legLStep.stepping = true;
        memory.legLStep.planted = false;
        memory.legLStep.landing = false;
        legLStepping(step, leg);

        memory.leftStepLast = true;

        if (!memory.legLStep.kicking) emitKickEffectLeft(step, effects);
        memory.legLStep.kicking = false;
    }
    public void startLegRStep(stepParams step, legPose leg, stepEffects effects)
    {
        memory.legRStep.direction = memory.legRStep.path.pt1 - memory.legRStep.currentDirect;

        memory.legRStep.stepping = true;
        memory.legRStep.planted = false;
        memory.legRStep.landing = false;
        legRStepping(step, leg);

        memory.leftStepLast = false;

        if (!memory.legRStep.kicking) emitKickEffectRight(step, effects);
        memory.legRStep.kicking = false;
    }
    //
    public void stepPausePhase(stepParams step)
    {
        float toBaseRotation = help.angleDifference(memory.rotation, memory.baseRotation);
        toBaseRotation = Mathf.Abs(toBaseRotation);

        temp.pausePhase = help.map(toBaseRotation, 0, 90, step.pausePhaseVelocity.Evaluate(temp.actingVelocityMagnitude), 1);
        temp.pausePhase = Mathf.Clamp01(temp.pausePhase);
    }
    public void stepGaitSpeed(stepParams step, out float speed)
    {
        //Vector3 fvt = temp.acceleration + temp.environmentInfluence * temp.pushMagnitude;
        //fvt /= temp.drag;
        
        speed = step.gaitSpeedVelocity.Evaluate(temp.finalVelocityMagnitude)

            * (temp.velocitySlope > 0 ?
                help.map(temp.velocitySlope, 0, 90, step.gaitSpeedFlat, step.gaitSpeedUp) :
                help.map(temp.velocitySlope, 0, -90, step.gaitSpeedFlat, step.gaitSpeedDown))

            * (temp.velocityAngleLocalUnsigned < 90 ?
                help.map(temp.velocityAngleLocalUnsigned, 0, 90, 1, step.gaitSpeedSide) :
                help.map(temp.velocityAngleLocalUnsigned, 90, 180, step.gaitSpeedSide, step.gaitSpeedBack));

        speed += step.gaitSpeedDirectionVariation * memory.directionVariation;

        speed += step.gaitSpeedTurn * Mathf.Abs(temp.turnVelocity);

        float toBaseRotation = help.angleDifference(memory.rotation, memory.baseRotation);
        toBaseRotation = Mathf.Abs(toBaseRotation);

        speed *= help.map(toBaseRotation, 0, 180, 1, 2);
    }
    public void legLStepBase(stepParams step, legPose leg, characterMovementPredictions predictions, out Vector3 position)
    {
        position = predictions.position
            + predictions.velocity * step.stepVelocity
            + (temp.finalVelocity - predictions.velocity) * step.stepVelocityFinalDelta;

        float toBaseRotation = help.angleDifference(memory.rotation, memory.baseRotation);
        toBaseRotation = Mathf.Abs(toBaseRotation);

        float stride = 1 / (2 * memory.legLStep.speed);
        //stride *= help.map(toBaseRotation, 0, 180, 1, 0);
        position += predictions.velocity * stride;

        Quaternion rotationPrediction = Quaternion.Euler(0, predictions.rotation, 0);
        position += rotationPrediction * leg.footPosition;

        //
        float angle = Vector3.Angle(rotationPrediction * Vector3.forward, temp.velocityXZ);
        float sideways = angle < 90 ? help.map(angle, 0, 90, 0, 1) : help.map(angle, 90, 180, 1, 0);

        float spread = step.spreadVelocity.Evaluate(temp.velocityXZmagnitude);
        Vector3 sideSpread = memory.legLStep.currentDirect - memory.legRStep.currentDirect; sideSpread.y = 0;
        sideSpread = Vector3.Project(sideSpread, rotationPrediction * Vector3.forward);

        position += Vector3.Lerp(rotationPrediction * new Vector3(-spread, 0, 0), sideSpread.normalized * basis.footSizeZ, sideways * temp.pushMagnitude);
    }
    public void legRStepBase(stepParams step, legPose leg, characterMovementPredictions predictions, out Vector3 position)
    {
        position = predictions.position
            + predictions.velocity * step.stepVelocity
            + (temp.finalVelocity - predictions.velocity) * step.stepVelocityFinalDelta;

        float toBaseRotation = help.angleDifference(memory.rotation, memory.baseRotation);
        toBaseRotation = Mathf.Abs(toBaseRotation);

        float stride = 1 / (2 * memory.legRStep.speed);
        //stride *= help.map(toBaseRotation, 0, 180, 1, 0);
        position += predictions.velocity * stride;

        Quaternion rotationPrediction = Quaternion.Euler(0, predictions.rotation, 0);
        position += rotationPrediction * leg.footPosition;

        //
        float angle = Vector3.Angle(rotationPrediction * Vector3.forward, temp.velocityXZ);
        float sideways = angle < 90 ? help.map(angle, 0, 90, 0, 1) : help.map(angle, 90, 180, 1, 0);

        float spread = step.spreadVelocity.Evaluate(temp.velocityXZmagnitude);
        Vector3 sideSpread = memory.legRStep.currentDirect - memory.legLStep.currentDirect; sideSpread.y = 0;
        sideSpread = Vector3.Project(sideSpread, rotationPrediction * Vector3.forward);

        position += Vector3.Lerp(rotationPrediction * new Vector3(spread, 0, 0), sideSpread.normalized * basis.footSizeZ, sideways * temp.pushMagnitude);
    }
    public void legLStepAvoidClipping(stepParams step, ref Vector3 check)
    {
        Vector3 nearFoot1 = memory.legRStep.from.position + temp.Rotation * new Vector3(-basis.footSizeX * 2, 0, basis.footSizeZ * 2);
        Vector3 nearFoot2 = memory.legRStep.from.position + temp.Rotation * new Vector3(-basis.footSizeX * 2, 0, -basis.footSizeZ * 2);

        Vector3 conePoint = help.lineLineIntersection(nearFoot1, Quaternion.Euler(0, memory.rotation - step.legClipCone, 0) * Vector3.right,
            nearFoot2, Quaternion.Euler(0, memory.rotation + step.legClipCone, 0) * Vector3.right);

        Vector3 targetCone = check - conePoint; targetCone.y = 0;
        float targetAngle = Vector3.SignedAngle(temp.Rotation * Vector3.right, targetCone, Vector3.up);

        if (targetAngle > -step.legClipCone && targetAngle < step.legClipCone)
        {
            if (help.lineSegmentsIntersection(nearFoot1, nearFoot2, conePoint, check, out Vector3 intersection))
            {
                if (targetAngle >= 0) check = conePoint + Quaternion.Euler(0, step.legClipCone - targetAngle, 0) * targetCone;
                else check = conePoint + Quaternion.Euler(0, -step.legClipCone - targetAngle, 0) * targetCone;
            }
        }

        //if (!checkCurrentPosition) return;

        //Vector3 currentPosCone = memory.legLStep.currentDirect - conePoint; currentPosCone.y = 0;
        //float currentPosAngle = Vector3.SignedAngle(temp.Rotation * Vector3.right, currentPosCone, Vector3.up);

        //if (currentPosAngle > -step.legCrossLimit && currentPosAngle < step.legCrossLimit)
        //{
        //    if (help.lineSegmentsIntersection(nearFoot1, nearFoot2, conePoint, memory.legLStep.currentDirect, out Vector3 intersection))
        //    {
        //        float y = memory.legLStep.currentDirect.y;
        //        if (currentPosAngle >= 0) memory.legLStep.currentDirect = conePoint + Quaternion.Euler(0, step.legCrossLimit - currentPosAngle, 0) * currentPosCone;
        //        else memory.legLStep.currentDirect = conePoint + Quaternion.Euler(0, -step.legCrossLimit - currentPosAngle, 0) * currentPosCone;
        //        memory.legLStep.currentDirect.y = y;
        //    }
        //}
    }
    public void legRStepAvoidClipping(stepParams step, ref Vector3 check)
    {
        Vector3 nearFoot1 = memory.legLStep.from.position + temp.Rotation * new Vector3(basis.footSizeX * 2, 0, basis.footSizeZ * 2);
        Vector3 nearFoot2 = memory.legLStep.from.position + temp.Rotation * new Vector3(basis.footSizeX * 2, 0, -basis.footSizeZ * 2);

        Vector3 conePoint = help.lineLineIntersection(nearFoot1, Quaternion.Euler(0, memory.rotation + step.legClipCone, 0) * Vector3.left,
            nearFoot2, Quaternion.Euler(0, memory.rotation - step.legClipCone, 0) * Vector3.left);

        Vector3 targetCone = check - conePoint; targetCone.y = 0;
        float targetAngle = Vector3.SignedAngle(temp.Rotation * Vector3.left, targetCone, Vector3.up);

        if (targetAngle > -step.legClipCone && targetAngle < step.legClipCone)
        {
            if (help.lineSegmentsIntersection(nearFoot1, nearFoot2, conePoint, check, out Vector3 intersection))
            {
                if (targetAngle >= 0) check = conePoint + Quaternion.Euler(0, step.legClipCone - targetAngle, 0) * targetCone;
                else check = conePoint + Quaternion.Euler(0, -step.legClipCone - targetAngle, 0) * targetCone;
            }
        }

        //if (!checkCurrentPosition) return;

        //Vector3 currentPosCone = memory.legRStep.currentDirect - conePoint; currentPosCone.y = 0;
        //float currentPosAngle = Vector3.SignedAngle(temp.Rotation * Vector3.left, currentPosCone, Vector3.up);

        //if (currentPosAngle > -step.legCrossLimit && currentPosAngle < step.legCrossLimit)
        //{
        //    if (help.lineSegmentsIntersection(nearFoot1, nearFoot2, conePoint, memory.legRStep.currentDirect, out Vector3 intersection))
        //    {
        //        float y = memory.legRStep.currentDirect.y;
        //        if (currentPosAngle >= 0) memory.legRStep.currentDirect = conePoint + Quaternion.Euler(0, step.legCrossLimit - currentPosAngle, 0) * currentPosCone;
        //        else memory.legRStep.currentDirect = conePoint + Quaternion.Euler(0, -step.legCrossLimit - currentPosAngle, 0) * currentPosCone;
        //        memory.legRStep.currentDirect.y = y;
        //    }
        //}
    }
    public void legLStepCheck(Vector3 position, out terrainHit hit)
    {
        TerrainCasts.stepSphere(position, Vector3.down, basis.legLength * 4, basis.footSizeZ, out hit);
        if (!hit.hit) return;

        position = -hit.normal; position.y = 0;
        if (position.sqrMagnitude > 0.02f) position = position.normalized * 0.02f;
        position += hit.position;
        position.y = hit.position.y + basis.legLength;

        TerrainCasts.stepRay(position, Vector3.down, basis.legLength * 2, out hit);

        TerrainCasts.stepRay(hit.position + hit.normal * basis.footSizeZ, Vector3.down, basis.legLength * 4, out hit);
    }
    public void legRStepCheck(Vector3 position, out terrainHit hit)
    {
        TerrainCasts.stepSphere(position, Vector3.down, basis.legLength * 4, basis.footSizeZ, out hit);
        if (!hit.hit) return;

        position = -hit.normal; position.y = 0;
        if (position.sqrMagnitude > 0.02f) position = position.normalized * 0.02f;
        position += hit.position;
        position.y = hit.position.y + basis.legLength;

        TerrainCasts.stepRay(position, Vector3.down, basis.legLength * 2, out hit);

        TerrainCasts.stepRay(hit.position + hit.normal * basis.footSizeZ, Vector3.down, basis.legLength * 4, out hit);
    }
    //
    public void legLDistanceLimiting(stepParams step, Vector3 target, pathingInfo path, out Vector3 position)
    {
        if (path.distance == 0)
        {
            position = target; return;
        }

        float toPhase = memory.legLStep.distanceDirect / path.distance;
        
        if (toPhase < path.d1) position = Vector3.Lerp(memory.legLStep.currentDirect, path.pt1, help.map(toPhase, 0, path.d1, 0, 1));
        else if (toPhase < path.d2) position = Vector3.Lerp(path.pt1, path.pt2, help.map(toPhase, path.d1, path.d2, 0, 1));
        else position = Vector3.Lerp(path.pt2, target, help.map(toPhase, path.d2, 1, 0, 1));
    }
    public void legRDistanceLimiting(stepParams step, Vector3 target, pathingInfo path, out Vector3 position)
    {
        if (path.distance == 0)
        {
            position = target; return;
        }

        float toPhase = memory.legRStep.distanceDirect / path.distance;

        if (toPhase < path.d1) position = Vector3.Lerp(memory.legRStep.currentDirect, path.pt1, help.map(toPhase, 0, path.d1, 0, 1));
        else if (toPhase < path.d2) position = Vector3.Lerp(path.pt1, path.pt2, help.map(toPhase, path.d1, path.d2, 0, 1));
        else position = Vector3.Lerp(path.pt2, target, help.map(toPhase, path.d2, 1, 0, 1));
    }
    public void legLStepPathing(Vector3 currentPos, Vector3 target, out pathingInfo path)
    {
        path = new pathingInfo();

        Vector3 Tpoint = memory.legRStep.from.position + temp.baseRotation * new Vector3(-basis.footSizeX * 2, 0, 0);
        path.obstructed = help.lineSegmentIntersectLine(currentPos, target, Tpoint, temp.baseRotation * Vector3.right);

        Vector3 nearFoot1 = memory.legRStep.from.position + temp.baseRotation * new Vector3(-basis.footSizeX * 2, 0, basis.footSizeZ * 2);
        Vector3 nearFoot2 = memory.legRStep.from.position + temp.baseRotation * new Vector3(-basis.footSizeX * 2, 0, -basis.footSizeZ * 2);

        path.obstructed |= help.lineSegmentsIntersection(currentPos, target, nearFoot1, nearFoot2, out Vector3 intersection);

        if (path.obstructed)
        {
            float currentAngle = Vector3.SignedAngle(temp.baseRotation * Vector3.right, currentPos - Tpoint, Vector3.up);
            float targetAngle = Vector3.SignedAngle(temp.baseRotation * Vector3.right, target - Tpoint, Vector3.up);

            if (currentAngle > 0 && currentAngle < 90)
            {
                if (targetAngle < 0 && targetAngle > -90)
                {
                    path.pt1 = nearFoot2 + temp.baseRotation * new Vector3(-0.02f, 0, -0.02f);
                    path.pt2 = nearFoot1 + temp.baseRotation * new Vector3(-0.02f, 0, 0.02f);
                }
                else
                {
                    path.pt1 = nearFoot2 + temp.baseRotation * new Vector3(-0.02f, 0, -0.02f);
                    path.pt2 = path.pt1;
                }
            }
            else if (currentAngle < 0 && currentAngle > -90)
            {
                if (targetAngle > 0 && targetAngle < 90)
                {
                    path.pt1 = nearFoot1 + temp.baseRotation * new Vector3(-0.02f, 0, 0.02f);
                    path.pt2 = nearFoot2 + temp.baseRotation * new Vector3(-0.02f, 0, -0.02f);
                }
                else
                {
                    path.pt1 = nearFoot1 + temp.baseRotation * new Vector3(-0.02f, 0, 0.02f);
                    path.pt2 = path.pt1;
                }
            }
            else if (targetAngle > 0 && targetAngle < 90)
            {
                path.pt1 = nearFoot2 + temp.baseRotation * new Vector3(-0.02f, 0, -0.02f);
                path.pt2 = path.pt1;
            }
            else if (targetAngle < 0 && targetAngle > -90)
            {
                path.pt1 = nearFoot1 + temp.baseRotation * new Vector3(-0.02f, 0, 0.02f);
                path.pt2 = path.pt1;
            }
            else
            {
                path.pt1 = target; path.pt2 = target;
            }
        }
        else
        {
            path.pt1 = target; path.pt2 = target;
        }

        Vector3 fromTo = target - memory.legLStep.from.position;
        float d = fromTo.magnitude;
        if (d == 0)
        {
            path.d1 = 1; path.d2 = 1; path.distance = 0;
        }
        else
        {
            fromTo /= d;

            //height
            Vector3 p = path.pt1 - memory.legLStep.from.position;
            p = Vector3.Project(p, fromTo);
            path.pt1.y = Mathf.Lerp(memory.legLStep.from.position.y, target.y, p.magnitude / d);
            
            p = path.pt2 - memory.legLStep.from.position;
            p = Vector3.Project(p, fromTo);
            path.pt2.y = Mathf.Lerp(memory.legLStep.from.position.y, target.y, p.magnitude / d);

            //phase
            path.d1 = help.distance(path.pt1, currentPos);
            path.d2 = help.distance(path.pt2, path.pt1);
            path.distance = help.distance(path.pt2, target);
            path.distance += path.d1 + path.d2;
            path.d2 += path.d1;
            if (path.distance != 0)
            {
                path.d1 /= path.distance;
                path.d2 /= path.distance;
            }
        }
    }
    public void legRStepPathing(Vector3 currentPos, Vector3 target, out pathingInfo path)
    {
        path = new pathingInfo();

        Vector3 Tpoint = memory.legLStep.from.position + temp.baseRotation * new Vector3(basis.footSizeX * 2, 0, 0);
        path.obstructed = help.lineSegmentIntersectLine(currentPos, target, Tpoint, temp.baseRotation * Vector3.left);

        Vector3 nearFoot1 = memory.legLStep.from.position + temp.baseRotation * new Vector3(basis.footSizeX * 2, 0, basis.footSizeZ * 2);
        Vector3 nearFoot2 = memory.legLStep.from.position + temp.baseRotation * new Vector3(basis.footSizeX * 2, 0, -basis.footSizeZ * 2);

        path.obstructed |= help.lineSegmentsIntersection(currentPos, target, nearFoot1, nearFoot2, out Vector3 intersection);

        if (path.obstructed)
        {
            float currentAngle = Vector3.SignedAngle(temp.baseRotation * Vector3.left, currentPos - Tpoint, Vector3.up);
            float targetAngle = Vector3.SignedAngle(temp.baseRotation * Vector3.left, target - Tpoint, Vector3.up);

            if (currentAngle > 0 && currentAngle < 90)
            {
                if (targetAngle < 0 && targetAngle > -90)
                {
                    path.pt1 = nearFoot1 + temp.baseRotation * new Vector3(0.02f, 0, 0.02f);
                    path.pt2 = nearFoot2 + temp.baseRotation * new Vector3(0.02f, 0, -0.02f);
                }
                else
                {
                    path.pt1 = nearFoot1 + temp.baseRotation * new Vector3(0.02f, 0, 0.02f);
                    path.pt2 = path.pt1;
                }
            }
            else if (currentAngle < 0 && currentAngle > -90)
            {
                if (targetAngle > 0 && targetAngle < 90)
                {
                    path.pt1 = nearFoot2 + temp.baseRotation * new Vector3(0.02f, 0, -0.02f);
                    path.pt2 = nearFoot1 + temp.baseRotation * new Vector3(0.02f, 0, 0.02f);
                }
                else
                {
                    path.pt1 = nearFoot2 + temp.baseRotation * new Vector3(0.02f, 0, -0.02f);
                    path.pt2 = path.pt1;
                }
            }
            else if (targetAngle > 0 && targetAngle < 90)
            {
                path.pt1 = nearFoot1 + temp.baseRotation * new Vector3(0.02f, 0, 0.02f);
                path.pt2 = path.pt1;
            }
            else if (targetAngle < 0 && targetAngle > -90)
            {
                path.pt1 = nearFoot2 + temp.baseRotation * new Vector3(0.02f, 0, -0.02f);
                path.pt2 = path.pt1;
            }
            else
            {
                path.pt1 = target; path.pt2 = target;
            }
        }
        else
        {
            path.pt1 = target; path.pt2 = target;
        }

        Vector3 fromTo = target - memory.legRStep.from.position;
        float d = fromTo.magnitude;

        if (d == 0)
        {
            path.d1 = 1; path.d2 = 1; path.distance = 0;
        }
        else
        {
            fromTo /= d;

            Vector3 p = path.pt1 - memory.legRStep.from.position;
            p = Vector3.Project(p, fromTo);
            path.pt1.y = Mathf.Lerp(memory.legRStep.from.position.y, target.y, p.magnitude / d);
            
            p = path.pt2 - memory.legRStep.from.position;
            p = Vector3.Project(p, fromTo);
            path.pt2.y = Mathf.Lerp(memory.legRStep.from.position.y, target.y, p.magnitude / d);

            path.d1 = help.distance(path.pt1, currentPos);
            path.d2 = help.distance(path.pt2, path.pt1);
            path.distance = help.distance(path.pt2, target);
            path.distance += path.d1 + path.d2;
            path.d2 += path.d1;
            if (path.distance != 0)
            {
                path.d1 /= path.distance;
                path.d2 /= path.distance;
            }
        }
    }
    //
    public void legLStepping(stepParams step, legPose leg)
    {
        Vector3 delta = memory.legLStep.currentDirect;

        float initialPhase = step.stepPhase.Evaluate(temp.leftTimerInitial / temp.pausePhase);
        float currentPhase = step.stepPhase.Evaluate(memory.legLStep.timer / temp.pausePhase);
        float normalizedPhase = help.map(currentPhase, initialPhase, 1, 0, 1);

        if (normalizedPhase < memory.legLStep.path.d1) memory.legLStep.currentDirect = 
                Vector3.Lerp(memory.legLStep.currentDirect, memory.legLStep.path.pt1, help.map(normalizedPhase, 0, memory.legLStep.path.d1, 0, 1));
        else if (normalizedPhase < memory.legLStep.path.d2) memory.legLStep.currentDirect = 
                Vector3.Lerp(memory.legLStep.path.pt1, memory.legLStep.path.pt2, help.map(normalizedPhase, memory.legLStep.path.d1, memory.legLStep.path.d2, 0, 1));
        else memory.legLStep.currentDirect = 
                Vector3.Lerp(memory.legLStep.path.pt2, memory.legLStep.to.position, help.map(normalizedPhase, memory.legLStep.path.d2, 1, 0, 1));

        legLStepHeight(step, currentPhase);
        anim.footLPos.position = memory.legLStep.currentDirect;
        memory.legLStep.distanceDirect -= help.distance(delta, memory.legLStep.currentDirect);

        help.moveToAngle(ref memory.footLRotation, memory.rotation + leg.yRotation, step.footRotation, out float ftTurn);
        Quaternion yRotation = Quaternion.Euler(0, memory.footLRotation, 0);

        float footPoint = step.footPointZ.Evaluate(Vector3.Dot(anim.footLPos.position - skeleton.pelvis.position, yRotation * Vector3.forward));
        footPoint += leg.footPoint;

        Vector3 normal = Vector3.Lerp(memory.legLStep.from.normal, memory.legLStep.to.normal, currentPhase);
        Quaternion Normal = Quaternion.FromToRotation(Vector3.up, normal);
        
        anim.footLRot.rotation = Normal * yRotation * Quaternion.Euler(footPoint, 0, 0);
        anim.footLRot.blend.type = blendType.none;

        if (footPoint > 0) anim.footLPos.position += Normal * yRotation * basis.targetToToe + anim.footLRot.rotation * basis.toeToFoot;
        else
        {
            Vector3 targetToHeel = basis.targetToToe + basis.toeToFoot - basis.heelToFoot;
            anim.footLPos.position += Normal * yRotation * targetToHeel + anim.footLRot.rotation * basis.heelToFoot;
        }
        anim.footLPos.blend.type = blendType.none;
    }
    public void legRStepping(stepParams step, legPose leg)
    {
        Vector3 delta = memory.legRStep.currentDirect;

        float initialPhase = step.stepPhase.Evaluate(temp.rightTimerInitial / temp.pausePhase);
        float currentPhase = step.stepPhase.Evaluate(memory.legRStep.timer / temp.pausePhase);
        float normalizedPhase = help.map(currentPhase, initialPhase, 1, 0, 1);

        if (normalizedPhase < memory.legRStep.path.d1) memory.legRStep.currentDirect =
                Vector3.Lerp(memory.legRStep.currentDirect, memory.legRStep.path.pt1, help.map(normalizedPhase, 0, memory.legRStep.path.d1, 0, 1));
        else if (normalizedPhase < memory.legRStep.path.d2) memory.legRStep.currentDirect =
                Vector3.Lerp(memory.legRStep.path.pt1, memory.legRStep.path.pt2, help.map(normalizedPhase, memory.legRStep.path.d1, memory.legRStep.path.d2, 0, 1));
        else memory.legRStep.currentDirect =
                Vector3.Lerp(memory.legRStep.path.pt2, memory.legRStep.to.position, help.map(normalizedPhase, memory.legRStep.path.d2, 1, 0, 1));

        legRStepHeight(step, currentPhase);
        anim.footRPos.position = memory.legRStep.currentDirect;
        memory.legRStep.distanceDirect -= help.distance(delta, memory.legRStep.currentDirect);

        help.moveToAngle(ref memory.footRRotation, memory.rotation + leg.yRotation, step.footRotation, out float ftTurn);
        Quaternion yRotation = Quaternion.Euler(0, memory.footRRotation, 0);

        float footPoint = step.footPointZ.Evaluate(Vector3.Dot(anim.footRPos.position - skeleton.pelvis.position, yRotation * Vector3.forward));
        footPoint += leg.footPoint;

        Vector3 normal = Vector3.Lerp(memory.legRStep.from.normal, memory.legRStep.to.normal, currentPhase);
        Quaternion Normal = Quaternion.FromToRotation(Vector3.up, normal);

        anim.footRRot.rotation = Normal * yRotation * Quaternion.Euler(footPoint, 0, 0);
        anim.footRRot.blend.type = blendType.none;

        if (footPoint > 0) anim.footRPos.position += Normal * yRotation * basis.targetToToe + anim.footRRot.rotation * basis.toeToFoot;
        else
        {
            Vector3 targetToHeel = basis.targetToToe + basis.toeToFoot - basis.heelToFoot;
            anim.footRPos.position += Normal * yRotation * targetToHeel + anim.footRRot.rotation * basis.heelToFoot;
        }
        anim.footRPos.blend.type = blendType.none;
    }
    public void legLStepHeight(stepParams step, float stepPhase)
    {
        float curveHeight = step.stepHeight.Evaluate(stepPhase) * step.heightVelocity.Evaluate(temp.velocityXZmagnitude);

        TerrainCasts.stepEdge(memory.legLStep.from.position, memory.legLStep.to.position, out terrainHit edge);
        if (edge.hit)
        {
            Vector3 line = memory.legLStep.to.position - memory.legLStep.from.position;
            float distance = line.magnitude;
            line /= distance;

            float posPhase;// = Vector3.Dot(anim.footLPos.position - memory.legLStep.from.position, line);
            //if (posPhase < 0) posPhase = 0;
            //else
            //{
            //    Vector3 posToLine = help.nearestPointOnLine(anim.footLPos.position, memory.legLStep.to.position, line);
            //    posPhase = (posToLine - memory.legLStep.from.position).magnitude / distance;
            //    if (posPhase > 1) posPhase = 1;
            //}
            posPhase = stepPhase;

            Vector3 edge1ToLine = help.nearestPointOnLine(edge.position, memory.legLStep.to.position, line);
            float edge1Phase = (edge1ToLine - memory.legLStep.from.position).magnitude / distance;
            edge1Phase = Mathf.Min(edge1Phase, 0.85f);

            Vector3 footOffset = -line; footOffset.y = 0; footOffset = footOffset.normalized * basis.footSizeZ; footOffset.y = basis.footSizeZ * 0.5f;

            Vector3 curve;
            TerrainCasts.stepEdge(memory.legLStep.to.position, memory.legLStep.from.position, out terrainHit edge2);
            if (edge2.hit && (edge.position - edge2.position).sqrMagnitude > 0.15f)
            {
                edge.position += footOffset;

                Vector3 edge2ToLine = help.nearestPointOnLine(edge2.position, memory.legLStep.to.position, line);
                float edge2Phase = (edge2ToLine - memory.legLStep.from.position).magnitude / distance;
                edge2Phase = Mathf.Min(edge2Phase, 0.85f);
                edge2.position += footOffset;

                if (posPhase < edge1Phase) curve = Vector3.Lerp(memory.legLStep.from.position, edge.position, help.map(posPhase, 0, edge1Phase, 0, 1));
                else if (posPhase < edge2Phase) curve = Vector3.Lerp(edge.position, edge2.position, help.map(posPhase, edge1Phase, edge2Phase, 0, 1));
                else curve = Vector3.Lerp(edge2.position, memory.legLStep.to.position, help.map(posPhase, edge2Phase, 1, 0, 1));
            }
            else
            {
                edge.position += footOffset;

                if (posPhase < edge1Phase) curve = Vector3.Lerp(memory.legLStep.from.position, edge.position, help.map(posPhase, 0, edge1Phase, 0, 1));
                else curve = Vector3.Lerp(edge.position, memory.legLStep.to.position, help.map(posPhase, edge1Phase, 1, 0, 1));
            }

            //curve -= memory.legLStep.currentDirect;
            //curve -= Vector3.Project(curve, line);
            //curve -= Vector3.Project(curve, (anim.footLPos.position - memory.legRStep.from.position).normalized);

            //compare with y position on direct line + curveheight
            memory.legLStep.currentDirect.y = 
            //anim.footLPos.position.y =
                Mathf.Max(curve.y, curveHeight + Mathf.Lerp(memory.legLStep.from.position.y, memory.legLStep.to.position.y, stepPhase));

            //anim.footLPos.position.y += curve.y;
        }
        else
        {
            //compare with y position on direct line + curveheight
            //anim.footLPos.position.y += curveHeight;
            memory.legLStep.currentDirect.y = 
            //anim.footLPos.position.y =
                curveHeight + Mathf.Lerp(memory.legLStep.from.position.y, memory.legLStep.to.position.y, stepPhase);
        }
    }
    public void legRStepHeight(stepParams step, float stepPhase)
    {
        float curveHeight = step.stepHeight.Evaluate(stepPhase) * step.heightVelocity.Evaluate(temp.velocityXZmagnitude);

        TerrainCasts.stepEdge(memory.legRStep.from.position, memory.legRStep.to.position, out terrainHit edge);
        if (edge.hit)
        {
            Vector3 line = memory.legRStep.to.position - memory.legRStep.from.position;
            float distance = line.magnitude;
            line /= distance;

            float posPhase;// = Vector3.Dot(anim.footRPos.position - memory.legRStep.from.position, line);
            //if (posPhase < 0) posPhase = 0;
            //else
            //{
            //    Vector3 posToLine = help.nearestPointOnLine(anim.footRPos.position, memory.legRStep.to.position, line);
            //    posPhase = (posToLine - memory.legRStep.from.position).magnitude / distance;
            //    if (posPhase > 1) posPhase = 1;
            //}
            posPhase = stepPhase;

            Vector3 edge1ToLine = help.nearestPointOnLine(edge.position, memory.legRStep.to.position, line);
            float edge1Phase = (edge1ToLine - memory.legRStep.from.position).magnitude / distance;
            edge1Phase = Mathf.Min(edge1Phase, 0.85f);

            Vector3 footOffset = -line; footOffset.y = 0; footOffset = footOffset.normalized * basis.footSizeZ; footOffset.y = basis.footSizeZ * 0.5f;

            Vector3 curve;
            TerrainCasts.stepEdge(memory.legRStep.to.position, memory.legRStep.from.position, out terrainHit edge2);
            if (edge2.hit && (edge.position - edge2.position).sqrMagnitude > 0.15f)
            {
                edge.position += footOffset;

                Vector3 edge2ToLine = help.nearestPointOnLine(edge2.position, memory.legRStep.to.position, line);
                float edge2Phase = (edge2ToLine - memory.legRStep.from.position).magnitude / distance;
                edge2Phase = Mathf.Min(edge2Phase, 0.85f);
                edge2.position += footOffset;

                if (posPhase < edge1Phase) curve = Vector3.Lerp(memory.legRStep.from.position, edge.position, help.map(posPhase, 0, edge1Phase, 0, 1));
                else if (posPhase < edge2Phase) curve = Vector3.Lerp(edge.position, edge2.position, help.map(posPhase, edge1Phase, edge2Phase, 0, 1));
                else curve = Vector3.Lerp(edge2.position, memory.legRStep.to.position, help.map(posPhase, edge2Phase, 1, 0, 1));
            }
            else
            {
                edge.position += footOffset;

                if (posPhase < edge1Phase) curve = Vector3.Lerp(memory.legRStep.from.position, edge.position, help.map(posPhase, 0, edge1Phase, 0, 1));
                else curve = Vector3.Lerp(edge.position, memory.legRStep.to.position, help.map(posPhase, edge1Phase, 1, 0, 1));
            }

            //curve -= anim.footRPos.position;
            //curve -= Vector3.Project(curve, line);
            //curve -= Vector3.Project(curve, (anim.footRPos.position - memory.legLStep.from.position).normalized);
            memory.legRStep.currentDirect.y = Mathf.Max(curve.y, curveHeight + Mathf.Lerp(memory.legRStep.from.position.y, memory.legRStep.to.position.y, stepPhase));
        }
        else
        {
            //anim.footRPos.position.y += curveHeight;
            memory.legRStep.currentDirect.y = curveHeight + Mathf.Lerp(memory.legRStep.from.position.y, memory.legRStep.to.position.y, stepPhase);
        }
    }
    //
    public void legLPlantKick(stepParams step, legPose leg, stepEffects effects)
    {
        if (memory.legLStep.kicking) legLKicking(step, leg);
        else
        {
            if (Vector3.Dot(memory.legLStep.currentDirect - skeleton.arma.position, temp.Rotation * Vector3.back) > step.kickDistance)
            {
                memory.legLStep.kicking = true;
                legLKicking(step, leg);
                emitKickEffectLeft(step, effects);
            }
            else legLPlanted(step, leg);
        }
    }
    public void legRPlantKick(stepParams step, legPose leg, stepEffects effects)
    {
        if (memory.legRStep.kicking) legRKicking(step, leg);
        else
        {
            if (Vector3.Dot(memory.legRStep.currentDirect - skeleton.arma.position, temp.Rotation * Vector3.back) > step.kickDistance)
            {
                memory.legRStep.kicking = true;
                legRKicking(step, leg);
                emitKickEffectRight(step, effects);
            }
            else legRPlanted(step, leg);
        }
    }
    public void legLPlantStep(stepParams step, stepEffects effects)
    {
        memory.legLStep.planted = true;
        memory.legLStep.from = memory.legLStep.to;
        memory.legLStep.from.localPosition = memory.legLStep.from.position - memory.legLStep.from.obj.transform.position;
        memory.legLStep.from.localPosition = Quaternion.Inverse(memory.legLStep.from.obj.transform.rotation) * memory.legLStep.from.localPosition;

        playStepSound(step, effects);
        emitStepEffectLeft(step, effects);
    }
    public void legRPlantStep(stepParams step, stepEffects effects)
    {
        memory.legRStep.planted = true;
        memory.legRStep.from = memory.legRStep.to;
        memory.legRStep.from.localPosition = memory.legRStep.from.position - memory.legRStep.from.obj.transform.position;
        memory.legRStep.from.localPosition = Quaternion.Inverse(memory.legRStep.from.obj.transform.rotation) * memory.legRStep.from.localPosition;

        playStepSound(step, effects);
        emitStepEffectRight(step, effects);
    }
    public void legLPlanted(stepParams step, legPose leg)
    {
        if (memory.legLStep.from.obj)
            anim.footLPos.position = memory.legLStep.from.obj.transform.position + memory.legLStep.from.obj.transform.rotation * memory.legLStep.from.localPosition;
        else
            anim.footLPos.position = memory.legLStep.from.position;

        memory.legLStep.from.position = anim.footLPos.position;
        memory.legLStep.currentDirect = anim.footLPos.position;

        memory.footLRotation += temp.terrainTurn;
        float delta = Mathf.DeltaAngle(memory.footLRotation, memory.footRRotation);
        if (delta > step.duckFootMax) memory.footLRotation = help.wrap(memory.footRRotation - step.duckFootMax);
        else if (delta < step.duckFootMin) memory.footLRotation = help.wrap(memory.footRRotation - step.duckFootMin);
        Quaternion yRotation = Quaternion.Euler(0, memory.footLRotation, 0);

        float footPoint = step.footPointZ.Evaluate(Vector3.Dot(anim.footLPos.position - skeleton.pelvis.position, yRotation * Vector3.forward));
        footPoint += leg.footPoint;

        Quaternion Normal = Quaternion.FromToRotation(Vector3.up, memory.legLStep.from.normal);
        
        anim.footLRot.rotation = Normal * yRotation * Quaternion.Euler(footPoint, 0, 0);
        anim.footLRot.blend.type = blendType.none;

        if (footPoint > 0) anim.footLPos.position += Normal * yRotation * basis.targetToToe + anim.footLRot.rotation * basis.toeToFoot;
        else
        {
            Vector3 targetToHeel = basis.targetToToe + basis.toeToFoot - basis.heelToFoot;
            anim.footLPos.position += Normal * yRotation * targetToHeel + anim.footLRot.rotation * basis.heelToFoot;
        }
        anim.footLPos.blend.type = blendType.none;
    }
    public void legRPlanted(stepParams step, legPose leg)
    {
        if (memory.legRStep.from.obj)
            anim.footRPos.position = memory.legRStep.from.obj.transform.position + memory.legRStep.from.obj.transform.rotation * memory.legRStep.from.localPosition;
        else
            anim.footRPos.position = memory.legRStep.from.position;

        memory.legRStep.from.position = anim.footRPos.position;
        memory.legRStep.currentDirect = anim.footRPos.position;

        memory.footRRotation += temp.terrainTurn;
        float delta = Mathf.DeltaAngle(memory.footLRotation, memory.footRRotation);
        if (delta > step.duckFootMax) memory.footRRotation = help.wrap(memory.footLRotation + step.duckFootMax);
        else if (delta < step.duckFootMin) memory.footRRotation = help.wrap(memory.footLRotation + step.duckFootMin);
        Quaternion yRotation = Quaternion.Euler(0, memory.footRRotation, 0);

        float footPoint = step.footPointZ.Evaluate(Vector3.Dot(anim.footRPos.position - skeleton.pelvis.position, yRotation * Vector3.forward));
        footPoint += leg.footPoint;

        Quaternion Normal = Quaternion.FromToRotation(Vector3.up, memory.legRStep.from.normal);
        
        anim.footRRot.rotation = Normal * yRotation * Quaternion.Euler(footPoint, 0, 0);
        anim.footRRot.blend.type = blendType.none;

        if (footPoint > 0) anim.footRPos.position += Normal * yRotation * basis.targetToToe + anim.footRRot.rotation * basis.toeToFoot;
        else
        {
            Vector3 targetToHeel = basis.targetToToe + basis.toeToFoot - basis.heelToFoot;
            anim.footRPos.position += Normal * yRotation * targetToHeel + anim.footRRot.rotation * basis.heelToFoot;
        }
        anim.footRPos.blend.type = blendType.none;
    }
    public void legLKicking(stepParams step, legPose leg)
    {
        float kick = Vector3.Angle(temp.finalVelocity, temp.Rotation * Vector3.forward);
        if (kick > 90) kick = 0;
        else kick = help.map(kick, 0, 90, 1, 0);
        kick *= temp.finalVelocityMagnitude;

        Quaternion direction = Quaternion.Euler(0, Vector3.SignedAngle(Vector3.forward, temp.finalVelocity, Vector3.up), 0);
        memory.legLStep.currentDirect += direction * new Vector3(-step.kickVector.x, step.kickVector.y, step.kickVector.z) * kick * Time.fixedDeltaTime;
        anim.footLPos.position = memory.legLStep.currentDirect;

        help.moveToAngle(ref memory.footLRotation, memory.rotation + leg.yRotation, step.footRotation, out float ftTurn);
        Quaternion yRotation = Quaternion.Euler(0, memory.footLRotation, 0);

        float footPoint = step.footPointZ.Evaluate(Vector3.Dot(anim.footLPos.position - skeleton.pelvis.position, yRotation * Vector3.forward));
        footPoint += leg.footPoint;

        Quaternion Normal = Quaternion.FromToRotation(Vector3.up, memory.legLStep.from.normal);

        anim.footLRot.rotation = Normal * yRotation * Quaternion.Euler(footPoint, 0, 0);
        anim.footLRot.blend.type = blendType.none;

        if (footPoint > 0) anim.footLPos.position += Normal * yRotation * basis.targetToToe + anim.footLRot.rotation * basis.toeToFoot;
        else
        {
            Vector3 targetToHeel = basis.targetToToe + basis.toeToFoot - basis.heelToFoot;
            anim.footLPos.position += Normal * yRotation * targetToHeel + anim.footLRot.rotation * basis.heelToFoot;
        }
        anim.footLPos.blend.type = blendType.none;
    }
    public void legRKicking(stepParams step, legPose leg)
    {
        float kick = Vector3.Angle(temp.finalVelocity, temp.Rotation * Vector3.forward);
        if (kick > 90) kick = 0;
        else kick = help.map(kick, 0, 90, 1, 0);
        kick *= temp.finalVelocityMagnitude;

        Quaternion direction = Quaternion.Euler(0, Vector3.SignedAngle(Vector3.forward, temp.finalVelocity, Vector3.up), 0);
        memory.legRStep.currentDirect += direction * step.kickVector * kick * Time.fixedDeltaTime;
        anim.footRPos.position = memory.legRStep.currentDirect;

        help.moveToAngle(ref memory.footRRotation, memory.rotation + leg.yRotation, step.footRotation, out float ftTurn);
        Quaternion yRotation = Quaternion.Euler(0, memory.footRRotation, 0);

        float footPoint = step.footPointZ.Evaluate(Vector3.Dot(anim.footRPos.position - skeleton.pelvis.position, yRotation * Vector3.forward));
        footPoint += leg.footPoint;

        Quaternion Normal = Quaternion.FromToRotation(Vector3.up, memory.legRStep.from.normal);

        anim.footRRot.rotation = Normal * yRotation * Quaternion.Euler(footPoint, 0, 0);
        anim.footRRot.blend.type = blendType.none;

        if (footPoint > 0) anim.footRPos.position += Normal * yRotation * basis.targetToToe + anim.footRRot.rotation * basis.toeToFoot;
        else
        {
            Vector3 targetToHeel = basis.targetToToe + basis.toeToFoot - basis.heelToFoot;
            anim.footRPos.position += Normal * yRotation * targetToHeel + anim.footRRot.rotation * basis.heelToFoot;
        }
        anim.footRPos.blend.type = blendType.none;
    }
    //
    public void gaitPhase()
    {
        if (memory.legLStep.stepping)
        {
            if (memory.legRStep.stepping)
            {
                if (memory.legLStep.timer > memory.legRStep.timer) temp.gaitPhase = memory.legRStep.timer * memory.legRStep.timer - memory.legLStep.timer;
                else temp.gaitPhase = memory.legRStep.timer - memory.legLStep.timer * memory.legLStep.timer;
            }
            else temp.gaitPhase = -memory.legLStep.timer;
        }
        else if (memory.legRStep.stepping) temp.gaitPhase = memory.legRStep.timer;
        else temp.gaitPhase = 0;
    }
    public void baseRotation(stepParams step)
    {
        float feetAngle = Vector3.SignedAngle(Vector3.forward, memory.legLStep.currentDirect - memory.legRStep.currentDirect, Vector3.up);
        feetAngle += 90;

        float twist = help.angleDifference(memory.rotation, feetAngle);

        float toBaseRotation = help.angleDifference(memory.rotation, memory.baseRotation);
        if (toBaseRotation < 5 && toBaseRotation > -5)
        {
            if (twist < -180 + step.legCrossLimit) memory.baseRotation = feetAngle + 180 - step.legCrossLimit;
            else if (twist > 180 - step.legCrossLimit) memory.baseRotation = feetAngle - 180 + step.legCrossLimit;
            else memory.baseRotation = memory.rotation;
        }
        else if (toBaseRotation < 0)
        {
            if (twist > 0) twist -= 360;
            if (twist < -180 + step.legCrossLimit) memory.baseRotation = feetAngle + 180 - step.legCrossLimit;
            else memory.baseRotation = memory.rotation;
        }
        else
        {
            if (twist < 0) twist += 360;
            if (twist > 180 - step.legCrossLimit) memory.baseRotation = feetAngle - 180 + step.legCrossLimit;
            else memory.baseRotation = memory.rotation;
        }
    }
    public void setKnees(legPose leftLeg, legPose rightLeg)
    {
        anim.kneeLPos.position = anim.footLPos.position + anim.footLRot.rotation * leftLeg.kneePosition;
        anim.kneeLPos.blend.type = blendType.none;

        anim.kneeRPos.position = anim.footRPos.position + anim.footRRot.rotation * rightLeg.kneePosition;
        anim.kneeRPos.blend.type = blendType.none;
    }
    //
    public void playStepSound(stepParams step, stepEffects effects)
    {
        float volume = help.map(temp.velocityXZmagnitude, 0, step.highPositionSpeed, 0.1f, 1);
        volume *= 0.01f * Main.main.settings.volume;
        components.audio.PlayOneShot(effects.plantSound, volume);
    }
    public void emitStepEffectLeft(stepParams step, stepEffects effects)
    {
        ParticleSystem.EmitParams emit = new();
        emit.position = memory.legLStep.from.position;
        emit.applyShapeToPosition = true;

        if (temp.inWater && emit.position.y < temp.waterLevel)
        {
            emit.startSize = help.map(temp.velocityXZmagnitude, 0, step.highPositionSpeed, basis.footSizeZ * 0.3f, basis.footSizeZ * 3);
            effects.splashEffect.Emit(emit, 15);
        }
        else
        {
            emit.startSize = help.map(temp.velocityXZmagnitude, 0, step.highPositionSpeed, basis.footSizeZ * 0.2f, basis.footSizeZ * 1.4f);
            effects.plantEffect.Emit(emit, 15);
        }
    }
    public void emitStepEffectRight(stepParams step, stepEffects effects)
    {
        ParticleSystem.EmitParams emit = new();
        emit.position = memory.legRStep.from.position;
        emit.applyShapeToPosition = true;

        if (temp.inWater && emit.position.y < temp.waterLevel)
        {
            emit.startSize = help.map(temp.velocityXZmagnitude, 0, step.highPositionSpeed, basis.footSizeZ * 0.3f, basis.footSizeZ * 3);
            effects.splashEffect.Emit(emit, 15);
        }
        else
        {
            emit.startSize = help.map(temp.velocityXZmagnitude, 0, step.highPositionSpeed, basis.footSizeZ * 0.2f, basis.footSizeZ * 1.4f);
            effects.plantEffect.Emit(emit, 15);
        }
    }
    public void emitKickEffectLeft(stepParams step, stepEffects effects)
    {
        ParticleSystem.EmitParams emit = new();
        emit.position = memory.legLStep.from.position;
        emit.applyShapeToPosition = true;

        if (temp.inWater && emit.position.y < temp.waterLevel)
        {
            emit.startSize = help.map(temp.velocityXZmagnitude, 0, step.highPositionSpeed, basis.footSizeZ * 0.3f, basis.footSizeZ * 3);
            effects.splashEffect.Emit(emit, 15);
        }
        else
        {
            emit.startSize = help.map(temp.velocityXZmagnitude, 0, step.highPositionSpeed, basis.footSizeZ * 0.2f, basis.footSizeZ * 2);
            effects.kickEffect.Emit(emit, 15);
        }
    }
    public void emitKickEffectRight(stepParams step, stepEffects effects)
    {
        ParticleSystem.EmitParams emit = new();
        emit.position = memory.legRStep.from.position;
        emit.applyShapeToPosition = true;

        if (temp.inWater && emit.position.y < temp.waterLevel)
        {
            emit.startSize = help.map(temp.velocityXZmagnitude, 0, step.highPositionSpeed, basis.footSizeZ * 0.3f, basis.footSizeZ * 3);
            effects.splashEffect.Emit(emit, 15);
        }
        else
        {
            emit.startSize = help.map(temp.velocityXZmagnitude, 0, step.highPositionSpeed, basis.footSizeZ * 0.2f, basis.footSizeZ * 2);
            effects.kickEffect.Emit(emit, 15);
        }
    }
    public void splashEffect(stepParams step, stepEffects effects)
    {
        if (temp.inWater)
        {
            if (temp.velocityXZmagnitude < step.highPositionSpeed * 0.15f) return;

            if (anim.footLPos.position.y + basis.heelToFoot.y < temp.waterLevel)
            {
                ParticleSystem.EmitParams emit = new();
                Vector3 position = anim.footLPos.position;
                position.y = temp.waterLevel;
                emit.position = position;
                emit.applyShapeToPosition = true;
                emit.startSize = help.map(temp.velocityXZmagnitude, 0, step.highPositionSpeed, 0, basis.footSizeZ * 2);
                effects.splashEffect.Emit(emit, 2);
            }
            if (anim.footRPos.position.y + basis.heelToFoot.y < temp.waterLevel)
            {
                ParticleSystem.EmitParams emit = new();
                Vector3 position = anim.footRPos.position;
                position.y = temp.waterLevel;
                emit.position = position;
                emit.applyShapeToPosition = true;
                emit.startSize = help.map(temp.velocityXZmagnitude, 0, step.highPositionSpeed, 0, basis.footSizeZ * 2);
                effects.splashEffect.Emit(emit, 2);
            }
        }
    }
    //
    public void baseHeight(stepMoveParams move)
    {
        float lowestPosition = Mathf.Min(anim.footLPos.position.y, anim.footRPos.position.y);
        anim.bodyPos.position.y += (move.distanceOffGround + lowestPosition - skeleton.arma.position.y);
    }
    public void stepBodyPosition(stepParams step, stepMoveParams move)
    {
        baseHeight(move);

        float lowhigh = help.map(temp.velocityXZmagnitude, 0, step.highPositionSpeed, 0, 1);
        float gaitY;
        if (temp.velocityAngleLocalUnsigned < 90)
        {
            gaitY = Mathf.Lerp(0, step.gaitYpositionHigh.Evaluate(temp.gaitPhase), lowhigh);
            gaitY = Mathf.Lerp(gaitY,
                Mathf.Lerp(0, step.gaitYpositionSideHigh.Evaluate(temp.gaitPhase), lowhigh),
                help.map(temp.velocityAngleLocalUnsigned, 0, 90, 0, 1));
        }
        else
        {
            gaitY = Mathf.Lerp(0, step.gaitYpositionSideHigh.Evaluate(temp.gaitPhase), lowhigh);
            gaitY = Mathf.Lerp(gaitY,
                Mathf.Lerp(0, step.gaitYpositionBackHigh.Evaluate(temp.gaitPhase), lowhigh),
                help.map(temp.velocityAngleLocalUnsigned, 90, 180, 0, 1));
        }
        anim.bodyPos.position.y += gaitY;

        float distanceToFarFoot = Mathf.Max(
            help.distanceXZ(memory.legLStep.currentDirect, skeleton.highLegL.position),
            help.distanceXZ(memory.legRStep.currentDirect, skeleton.highLegR.position));
        anim.bodyPos.position.y += step.farFootYposition.Evaluate(distanceToFarFoot);

        anim.bodyPos.position.y += step.turnYposition * Mathf.Abs(Mathf.DeltaAngle(memory.orientation.y, memory.orientationTarget.y));

        anim.bodyPos.position += gaitMagnitude(step.lurch.magnitude) * step.lurch.curve.Evaluate(temp.gaitPhase) * temp.actingVelocityNormal;

        anim.bodyPos.position += temp.Rotation * new Vector3(gaitMagnitude(step.sway.magnitude) * step.sway.curve.Evaluate(temp.gaitPhase), 0, 0);
    }
}

[Serializable]
public class stepParams
{
    public AnimationCurve gaitSpeedVelocity;
    public float gaitSpeedFlat, gaitSpeedUp, gaitSpeedDown;
    public float gaitSpeedSide, gaitSpeedBack;
    public float gaitSpeedTurn, gaitSpeedDirectionVariation;

    public AnimationCurve pausePhaseVelocity;

    public float stepDistance;

    public float stepVelocity, stepVelocityFinalDelta;

    public AnimationCurve spreadVelocity;

    public float legCrossLimit;
    public float legClipCone;

    public float maxStepSpeed, maxStepDistance;

    public AnimationCurve stepPhase;

    public float duckFootMin, duckFootMax;

    public blendSettings footRotation;

    public AnimationCurve footPointZ;

    public AnimationCurve stepHeight;
    public AnimationCurve heightVelocity;

    public AnimationCurve gaitYpositionHigh;
    public float highPositionSpeed;
    public AnimationCurve gaitYpositionSideHigh;
    public AnimationCurve gaitYpositionBackHigh;

    public AnimationCurve farFootYposition;

    public float turnYposition;

    public gaitCurve sway, lurch;

    public float kickDistance;
    public Vector3 kickVector;
}

public struct stepMemory
{
    public float speed;

    public terrainHit from, to;

    public Vector3 currentDirect;

    public float timer;

    public bool stepping, planted, landing, kicking;

    public pathingInfo path;

    public Vector3 direction;

    public float distanceDirect;
}

public class pathingInfo
{
    public bool obstructed;
    public Vector3 pt1, pt2;
    public float d1, d2;
    public float distance;
}

public class stepEffects
{
    public AudioClip plantSound;
    public ParticleSystem plantEffect, kickEffect;
    public ParticleSystem splashEffect;
}