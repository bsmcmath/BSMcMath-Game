using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class characterBase : MonoBehaviour
{
    public void climb_state(climbStateParams csp)
    {
        temp.rotationTarget = 0;
        climbMove(csp.move);

        //legs

        //body
        applyBodyBaseRotation();

        setBodyNormals(Vector3.up);
        setBodyBlend(csp.bodyRotationBlend, csp.bodyPositionBlend);

        animate_TorsoHeadShoulders();

        //arms
    }
}

public class climbStateParams
{
    public climbMoveParams move;
    public fullPose pose;
    public lookParams look;
    public blendSettings bodyRotationBlend, bodyPositionBlend;
    public blendSettings leftArmBlend, rightArmBlend;
}