using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{
    public Vector3 orientation;

    public void turn_player_xy()
    {
        float turnSpeed = Main.main.settings.cameraSensitivity * Time.fixedDeltaTime;

        orientation.x -= Main.main.input.inputP1.turn.y * turnSpeed;
        orientation.x = Mathf.Clamp(orientation.x, -85, 85);

        orientation.y += Main.main.input.inputP1.turn.x * turnSpeed;
        orientation.y = help.wrap(orientation.y);

        setRotation(Quaternion.Euler(orientation));
    }
    public void turn_player_xy(float xOffset, float yOffset)
    {
        float turnSpeed = Main.main.settings.cameraSensitivity * Time.fixedDeltaTime;

        orientation.x -= Main.main.input.inputP1.turn.y * turnSpeed;
        orientation.x = Mathf.Clamp(orientation.x, -85, 85);

        orientation.y += Main.main.input.inputP1.turn.x * turnSpeed;
        orientation.y = help.wrap(orientation.y);

        setRotation(Quaternion.Euler(orientation.x + xOffset, orientation.y + yOffset, 0));
    }
    public void turn_player_y(float xRotation)
    {
        float turnSpeed = Main.main.settings.cameraSensitivity * Time.fixedDeltaTime;

        orientation.x = xRotation;

        orientation.y += Main.main.input.inputP1.turn.x * turnSpeed;
        orientation.y = help.wrap(orientation.y);

        setRotation(Quaternion.Euler(orientation));
    }
    public void turn_duel(Transform target, Transform target2, float yClamp, float xFollow, float xOffset)
    {
        Vector3 toTarget = target2.position - target.position;
        Vector3 toTargetXZ = toTarget; toTargetXZ.y = 0;

        float angle = Vector3.SignedAngle(Vector3.forward, toTargetXZ, Vector3.up);
        orientation.y = Mathf.DeltaAngle(angle, orientation.y);
        orientation.y = Mathf.Clamp(orientation.y, -yClamp, yClamp);
        orientation.y += angle;

        orientation.x = Vector3.Angle(toTarget, toTargetXZ) * xFollow;
        if (toTarget.y > 0) orientation.x *= -1;
        orientation.x += xOffset;

        setRotation(Quaternion.Euler(orientation));
    }

    //

    public void follow_cast(Transform target, Vector3 localPosition, float collisionRadius)
    {
        Vector3 offset = transform.rotation * localPosition;
        float distance = localPosition.magnitude;

        if (Physics.SphereCast(target.position, collisionRadius, offset, out RaycastHit hit, distance, Main.main.layers.cameraObstructions, QueryTriggerInteraction.Ignore))
        {
            Vector3 position = target.position + offset / distance * (hit.distance - collisionRadius);
            setPosition(position);
        }
        else
        {
            Vector3 position = target.position + offset;
            setPosition(position);
        }
    }
    public void follow(Transform target, Vector3 localPosition)
    {
        setPosition(target.position + transform.rotation * localPosition);
    }

    //

    public bool blend;
    public float blendTimer, blendDuration;
    public void startBlend(float duration)
    {
        blend = true; blendTimer = 0; blendDuration = duration;
    }
    public void setPosition(Vector3 position)
    {
        if (blend)
        {
            blendTimer += Time.fixedDeltaTime;
            if (blendTimer > blendDuration) blend = false;
        }
        if (blend)
        {
            float lerp = blendTimer / blendDuration;
            transform.position = Vector3.Lerp(transform.position, position, lerp);
        }
        else transform.position = position;
    }
    public void setRotation(Quaternion rotation)
    {
        if (blend)
        {
            float lerp = blendTimer / blendDuration;
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, lerp);
        }
        else transform.rotation = rotation;
    }
}
public enum cameraMode
{
    thirdPerson, aimRanged, isometric, duel, stationary, freeLook
}