using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class characterBase : MonoBehaviour
{
    public void updateRecoil()
    {
        for (int i = 0; i < memory.recoil.Count; i++)
        {
            if (memory.recoil[i].momentum > 0)
            {
                float delta = Mathf.Min(500 * Time.fixedDeltaTime, memory.recoil[i].momentum);
                memory.recoil[i].momentum -= delta;
                memory.recoil[i].recoil += delta;

                memory.velocity += memory.recoil[i].direction * delta * 0.1f;
            }
            else
            {
                memory.recoil[i].recoil -= 200 * Time.fixedDeltaTime;

                if (memory.recoil[i].recoil <= 0)
                {
                    memory.recoil.RemoveAt(i);
                    i--;
                }
            }
        }
    }

    public void bodyRecoil()
    {
        for (int i = 0; i < memory.recoil.Count; i++)
        {
            characterRecoil r = memory.recoil[i];
            applyBodyTilt(r.direction * r.recoil);
        }
    }
}

public class characterRecoil
{
    public Vector3 direction;
    public float height;
    public float perpendicular;
    public float momentum, recoil;
}