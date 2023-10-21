using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class characterBase : MonoBehaviour
{
    
    public void bodyRecoil(staggerParams stag)
    {
        for (int i = 0; i < memory.recoil.Count; i++)
        {
            characterRecoil r = memory.recoil[i];
            Quaternion direction = Quaternion.FromToRotation(Vector3.up, new Vector3(r.direction.x, 0, r.direction.z));

            float influence = stag.pelvisHeight;
            if (influence > r.height) influence = help.map(influence, r.height, r.height + stag.influenceDistance, -1, 0);
            else influence = help.map(influence, r.height - stag.influenceDistance, r.height, 0, 1);
            Quaternion rotation = Quaternion.SlerpUnclamped(Quaternion.identity, direction, r.recoil * influence / 90);
            rotation *= Quaternion.Euler(0, r.recoil * Mathf.Abs(influence) * -r.perpendicular * stag.twistRecoil, 0);
            anim.pelvisRot.rotation = rotation * anim.pelvisRot.rotation;

            influence = stag.lowSpineHeight;
            if (influence > r.height) influence = help.map(influence, r.height, r.height + stag.influenceDistance, -1, 0);
            else influence = help.map(influence, r.height - stag.influenceDistance, r.height, 0, 1);
            rotation = Quaternion.SlerpUnclamped(Quaternion.identity, direction, r.recoil * influence / 90);
            rotation *= Quaternion.Euler(0, r.recoil * Mathf.Abs(influence) * -r.perpendicular * stag.twistRecoil, 0);
            anim.lowSpineRot.rotation = rotation * anim.lowSpineRot.rotation;

            influence = stag.highSpineHeight;
            if (influence > r.height) influence = help.map(influence, r.height, r.height + stag.influenceDistance, -1, 0);
            else influence = help.map(influence, r.height - stag.influenceDistance, r.height, 0, 1);
            rotation = Quaternion.SlerpUnclamped(Quaternion.identity, direction, r.recoil * influence / 90);
            rotation *= Quaternion.Euler(0, r.recoil * Mathf.Abs(influence) * -r.perpendicular * stag.twistRecoil, 0);
            anim.highSpineRot.rotation = rotation * anim.highSpineRot.rotation;

            influence = stag.neckHeight;
            if (influence > r.height) influence = help.map(influence, r.height, r.height + stag.influenceDistance, -1, 0);
            else influence = help.map(influence, r.height - stag.influenceDistance, r.height, 0, 1);
            rotation = Quaternion.SlerpUnclamped(Quaternion.identity, direction, r.recoil * influence / 90);
            rotation *= Quaternion.Euler(0, r.recoil * Mathf.Abs(influence) * -r.perpendicular * stag.twistRecoil, 0);
            anim.neckRot.rotation = rotation * anim.neckRot.rotation;

            influence = stag.headHeight;
            if (influence > r.height) influence = help.map(influence, r.height, r.height + stag.influenceDistance, -1, 0);
            else influence = help.map(influence, r.height - stag.influenceDistance, r.height, 0, 1);
            rotation = Quaternion.SlerpUnclamped(Quaternion.identity, direction, r.recoil * influence / 90);
            rotation *= Quaternion.Euler(0, r.recoil * Mathf.Abs(influence) * -r.perpendicular * stag.twistRecoil, 0);
            anim.headRot.rotation = rotation * anim.headRot.rotation;
        }
    }
}



[Serializable]
public class staggerParams
{
    public blendSettings recoilDecay, recoilMomentumDecay;

    public float pelvisHeight, lowSpineHeight, highSpineHeight, neckHeight, headHeight;
    public float influenceDistance;
    public float twistRecoil;
}