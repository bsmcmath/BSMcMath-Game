using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fire : MonoBehaviour
{
    public Light Light;
    public MeshRenderer mr;
    public Material mat;
    public float timer;
    public float duration;
    public AnimationCurve yOffset, yScale, alphaCutoff, objScale, colorR, colorG, colorB;
    private void Start()
    {
        mat = mr.material;
    }
    private void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
        if (timer > duration) timer -= duration;
        float phase = timer / duration;

        Vector2 tex = mat.mainTextureOffset;
        tex.y = yOffset.Evaluate(phase);
        mat.mainTextureOffset = tex;

        tex = mat.mainTextureScale;
        tex.y = yScale.Evaluate(phase);
        mat.mainTextureScale = tex;

        mat.SetFloat(Shader.PropertyToID("_Cutoff"), alphaCutoff.Evaluate(phase));

        float scale = objScale.Evaluate(phase);
        transform.localScale = new Vector3(scale, scale, scale);

        Color c = new Color(colorR.Evaluate(phase), colorG.Evaluate(phase), colorB.Evaluate(phase), 1);
        mat.SetColor(Shader.PropertyToID("_BaseColor"), c);
    }
}
