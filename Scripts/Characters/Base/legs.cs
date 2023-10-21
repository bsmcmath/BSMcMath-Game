using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class characterBase : MonoBehaviour
{
    public void limitLeftLegLength()
    {
        Vector3 hipToFoot = anim.footLPos.position - skeleton.highLegL.position;
        float distance = hipToFoot.magnitude;
        if (distance > basis.legLength) anim.footLPos.position = skeleton.highLegL.position + hipToFoot * basis.legLength / distance;
    }
    public void limitRightLegLength()
    {
        Vector3 hipToFoot = anim.footRPos.position - skeleton.highLegR.position;
        float distance = hipToFoot.magnitude;
        if (distance > basis.legLength) anim.footRPos.position = skeleton.highLegR.position + hipToFoot * basis.legLength / distance;
    }
}
