using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class bow : item
{
    public Transform bow1, bow2, bowstring;
    public bowParams bp;
    public float draw;

    public void moveDraw(float delta)
    {
        draw += delta;
        draw = Mathf.Clamp01(draw);
    }
    public void curveBow()
    {
        bow1.localRotation = bp.bow1base * Quaternion.Euler(bp.bow1Drawn * draw);
        bow2.localRotation = bp.bow2base * Quaternion.Euler(bp.bow2Drawn * draw);
    }

#if (UNITY_EDITOR)
    [ContextMenu("setup")]
    public void setup()
    {
        if (!bp)
        {
            bp = (bowParams)ScriptableObject.CreateInstance("bowParams");
            AssetDatabase.CreateAsset(bp, "Assets/_Custom/bowParams.asset");
        }
        bp.bow1base = bow1.localRotation;
        bp.bow2base = bow2.localRotation;
        bp.stringBase = bowstring.localPosition;
    }
#endif
}
