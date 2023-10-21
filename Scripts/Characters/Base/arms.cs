using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class characterBase : MonoBehaviour
{
    public void limitLeftArmLength()
    {
        Vector3 delta = anim.handLPos.position - skeleton.highArmL.position;
        float d = delta.magnitude;
        if (d > basis.armLength) anim.handLPos.position += delta * (basis.armLength - d) / d;
    }
    public void limitRightArmLength()
    {
        Vector3 delta = anim.handRPos.position - skeleton.highArmR.position;
        float d = delta.magnitude;
        if (d > basis.armLength) anim.handRPos.position += delta * (basis.armLength - d) / d;
    }
}
