using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class characterBase : MonoBehaviour
{
    public void waterCheck()
    {
        Collider[] cols = Physics.OverlapSphere(skeleton.arma.position, basis.footToArmature * 3, Main.main.layers.water);

        if (cols.Length > 0)
        {
            temp.inWater = true;
            temp.waterLevel = cols[0].GetComponent<water>().height;
        }
    }
    public bool swimCheck()
    {
        if (temp.inWater && temp.waterLevel > skeleton.arma.position.y + basis.armatureToHighSpine) return true;

        return false;
    }
    public bool staySwimCheck()
    {
        if (!temp.inWater) return false;

        if (temp.groundCheck.hit && temp.waterLevel < temp.groundCheck.position.y + basis.footToArmature) return false;

        return true;
    }
}
