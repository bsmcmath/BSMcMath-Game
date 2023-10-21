using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class characterBase : MonoBehaviour
{
    public void setNormalsToGround(float t)
    {
        setBodyNormals(Vector3.Slerp(Vector3.zero, temp.groundCheck.normal, t));
    }
    public void tiltBodyToActionDirection(float t)
    {
        Vector3 tilt = memory.actionDirection * t * 90;
        anim.pelvisRot.tilt += tilt;
        anim.lowSpineRot.tilt += tilt;
        anim.highSpineRot.tilt += tilt;
        anim.shoulderLRot.tilt += tilt;
        anim.shoulderRRot.tilt += tilt;
        anim.neckRot.tilt += tilt;
        anim.headRot.tilt += tilt;
    }
}

