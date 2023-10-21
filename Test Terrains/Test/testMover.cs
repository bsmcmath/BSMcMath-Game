using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testMover : movingTerrain
{
    public float duration;
    public AnimationCurve xt, yt, zt;
    public AnimationCurve yRt;

    float timer;
    [HideInInspector] public Vector3 startPosition;
    [HideInInspector] public Quaternion startRotation;
    private void OnValidate()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    private void FixedUpdate()
    {
        previousPosition = transform.position;
        previousRotation = transform.rotation;

        timer += Time.fixedDeltaTime;
        if (timer > duration) timer -= duration;

        float phase = timer / duration;

        Vector3 position = startPosition + new Vector3(xt.Evaluate(phase), yt.Evaluate(phase), zt.Evaluate(phase));
        Quaternion rotation = Quaternion.Euler(0, yRt.Evaluate(phase), 0) * startRotation;
        rb.Move(position, rotation);
    }
}
