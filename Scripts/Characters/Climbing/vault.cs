using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class characterBase : MonoBehaviour
{
    public bool vaultCheck()
    {
        if (temp.ledgeCheck.hit)
        {
            if (!pushIsToOrientation()) return false;

            if (temp.ledgeCheck.position.y > skeleton.arma.position.y + basis.armatureToHighSpine) return false;

            if (temp.groundCheck.hit && temp.groundCheck.position.y + basis.legLength * 0.5f > temp.ledgeCheck.position.y) return false;

            return true;
        }
        return false;
    }
}