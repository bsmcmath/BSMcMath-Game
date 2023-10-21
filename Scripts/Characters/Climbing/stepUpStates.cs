using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class characterBase : MonoBehaviour
{
    public void stepUp_state(stepStateParams p)
    {
        //movement, collision
        temp.rotationTarget = p.move.rotationOffset;
        stepUpMove(p.move);

        //feet

        //body
        applyBodyBaseRotation();
        
        setBodyNormals(Vector3.up);
        setBodyBlend(p.bodyRotationBlend, new blendSettings(blendType.none));

        animate_TorsoHeadShoulders();

        //arms
    }
}