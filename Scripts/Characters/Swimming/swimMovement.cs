using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class characterBase : MonoBehaviour
{
    public void swimMove(swimMoveParams move)
    {
        temp.pushAngleLocalUnsigned = Mathf.Abs(Mathf.DeltaAngle(memory.rotation, temp.pushAngle));

        temp.acceleration = temp.push * 
            (temp.pushAngleLocalUnsigned < 90 ? 
            help.map(temp.pushAngleLocalUnsigned, 0, 90, move.accelerationFwd, move.accelerationSide) :
            help.map(temp.pushAngleLocalUnsigned, 90, 180, move.accelerationSide, move.accelerationBack));

        memory.velocity -= memory.velocity * memory.velocity.magnitude * move.drag * Time.fixedDeltaTime;

        memory.velocity += temp.acceleration * Time.fixedDeltaTime;

        memory.velocity.y += (temp.waterLevel - basis.armatureToHighSpine - skeleton.arma.position.y) * 10 * Time.fixedDeltaTime;
    }
}

[Serializable]
public class swimMoveParams
{
    public blendSettings rotation;

    public float accelerationFwd, accelerationSide, accelerationBack;
    public float drag;
}