using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class characterBase : MonoBehaviour
{
    public void push_orientLook()
    {
        orientToPush_through();
        lookToOrientationTarget();
    }
    public void cam_orientLook()
    {
        orientToCamY();
        lookToCam();
    }
    public void cam_orientLook_strafe(AnimationCurve strafe)
    {
        orientToCamY();
        addStrafeOrientation(strafe);
        lookToCam();
    }
    public void camY_orientLook()
    {
        orientToCamY();
        lookToCamY();
    }
    public void camY_orientLook_strafe(AnimationCurve strafe)
    {
        orientToCamY();
        addStrafeOrientation(strafe);
        lookToCamY();
    }
    public void twinStick_orientLook(inputRouter input)
    {
        orientToRightStick_orPushThru(input);
        lookToRightStick_orOrientationTarget(input);
    }
    public void twinStick_orientLook_strafe(inputRouter input, AnimationCurve strafe)
    {
        orientToRightStick_orPushThru(input);
        lookToOrientationTarget();
        addStrafeOrientation(strafe);
    }
    public void twinStick_orient_target_look(inputRouter input, Transform target)
    {
        orientToRightStick_orPushThru(input);
        lookToTarget(target);
    }
    public void twinStick_strafe_orient_target_look(inputRouter input, AnimationCurve strafe, Transform target)
    {
        orientToRightStick_orPushThru(input);
        addStrafeOrientation(strafe);
        lookToTarget(target);
    }
    public void target_orientLook(Transform target)
    {
        orientToTarget(target);
        lookToOrientationTarget();
    }
    public void target_orientLook_strafe(Transform target, AnimationCurve strafe)
    {
        orientToTarget(target);
        lookToOrientationTarget();
        addStrafeOrientation(strafe);
    }
    public void push_orient_target_look(Transform target)
    {
        orientToPush_through();
        lookToTarget(target);
    }
    public void action_orientLook()
    {
        orientToActionY();
        lookToOrientationTarget();
    }
    public void action_orientLook_strafe(AnimationCurve strafe)
    {
        orientToActionY();
        lookToOrientationTarget();
        addStrafeOrientation(strafe);
    }
}     