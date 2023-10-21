using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class characterBase : MonoBehaviour
{
    public void orientToCam()
    {
        memory.orientationTarget = Main.main.camera.orientation;
    }
    public void orientToCamY()
    {
        memory.orientationTarget = Main.main.camera.orientation;
        memory.orientationTarget.x = 0;
    }
    public void addStrafeOrientation(AnimationCurve strafe)
    {
        if (temp.push.sqrMagnitude > 0) memory.orientationTarget.y += strafe.Evaluate(help.angleDifference(memory.orientationTarget.y, temp.pushAngle));
        //else memory.orientationTarget.y += strafe.Evaluate(0);
        memory.orientationTarget.y = help.wrap(memory.orientationTarget.y);
    }
    public void orientToPush_through()
    {
        memory.orientationTarget.x = 0;

        if (temp.push.sqrMagnitude > 0) memory.orientationTarget.y = temp.pushAngle;
    }
    public void orientToPush_stop()
    {
        if (temp.push.sqrMagnitude > 0) memory.orientationTarget.y = temp.pushAngle;
        else memory.orientationTarget.y = memory.orientation.y;
    }
    public void orientToRightStick_orPushThru(inputRouter input)
    {
        if (input.turn.sqrMagnitude > 0) memory.orientationTarget.y = Vector3.SignedAngle(Vector3.forward, Quaternion.Euler(0, Main.main.camera.transform.eulerAngles.y, 0) * new Vector3(input.turn.x, 0, input.turn.y), Vector3.up);
        else orientToPush_through();
    }
    public void orientToTarget(Transform target)
    {
        Vector3 toTarget = target.position - skeleton.head.position;
        Vector3 toTargetXZ = toTarget; toTargetXZ.y = 0;
        memory.orientationTarget.x = Vector3.Angle(toTarget, toTargetXZ);
        if (toTarget.y > 0) memory.orientationTarget.x = -memory.orientationTarget.x;
        memory.orientationTarget.y = Vector3.SignedAngle(Vector3.forward, toTargetXZ, Vector3.up);
    }
    public void orientToWall()
    {
        memory.orientationTarget.y = Vector3.SignedAngle(Vector3.back, 
            new Vector3(temp.wallCheck.normal.x, 0, temp.wallCheck.normal.z), Vector3.up);
    }
    public void orientToLedge()
    {
        memory.orientationTarget.y = Vector3.SignedAngle(Vector3.back, 
            new Vector3(temp.ledgeCheck.normal.x, 0, temp.ledgeCheck.normal.z), Vector3.up);
    }
    public void orientToActionY()
    {
        memory.orientationTarget.y = memory.actionDirection.y;
        memory.orientationTarget.x = 0;
    }
    //
    public void lookToCam()
    {
        temp.look = Main.main.camera.orientation;
    }
    public void lookToCamY()
    {
        temp.look.x = 0;
        temp.look.y = Main.main.camera.orientation.y;
    }
    public void lookHalfToCamY_OrientationTarget()
    {
        temp.look.x = 0;
        temp.look.y = Mathf.LerpAngle(Main.main.camera.orientation.y, memory.orientationTarget.y, 0.5f);
    }
    public void lookToOrientation()
    {
        temp.look = memory.orientation;
    }
    public void lookToOrientationTarget()
    {
        temp.look = memory.orientationTarget;
    }
    public void lookToRightStick_orOrientationTarget(inputRouter input)
    {
        if (input.turn.sqrMagnitude > 0) temp.look.y = Vector3.SignedAngle(Vector3.forward, Quaternion.Euler(0, Main.main.camera.transform.eulerAngles.y, 0) * new Vector3(input.turn.x, 0, input.turn.y), Vector3.up);
        else temp.look.y = memory.orientationTarget.y;
    }
    public void lookToTarget(Transform target)
    {
        Vector3 toTarget = target.position - skeleton.head.position;
        Vector3 toTargetXZ = toTarget; toTargetXZ.y = 0;
        temp.look.x = Vector3.Angle(toTarget, toTargetXZ);
        if (toTarget.y > 0) temp.look.x = -temp.look.x;
        temp.look.y = Vector3.SignedAngle(Vector3.forward, toTargetXZ, Vector3.up);
    }
    //
    public void turnOrientation(blendSettings speed)
    {
        help.moveToAngle(ref memory.orientation.y, memory.orientationTarget.y, speed, out float ts);

        memory.orientation.y += temp.terrainTurn;

        temp.Orientation = Quaternion.Euler(0, memory.orientation.y, 0);
    }
    public void turnOrientation(blendSettings speed, float speedMod)
    {
        blendSettings blend = new blendSettings();
        blend.type = speed.type;
        blend.flat = speed.flat * speedMod;
        blend.lerp = speed.lerp * speedMod;
        help.moveToAngle(ref memory.orientation.y, memory.orientationTarget.y, speed, out float ts);

        memory.orientation.y += temp.terrainTurn;

        temp.Orientation = Quaternion.Euler(0, memory.orientation.y, 0);
    }
    public void setRotation()
    {
        float previousRotation = memory.rotation;
        temp.previousRotation = Quaternion.Euler(0, memory.rotation, 0);

        memory.rotation = help.wrap(temp.rotationTarget + memory.orientation.y);
        temp.turnVelocity = Mathf.DeltaAngle(previousRotation, memory.rotation);

        temp.Rotation = Quaternion.Euler(0, memory.rotation, 0);

        temp.baseRotation = Quaternion.Euler(0, memory.baseRotation, 0);
    }
}
