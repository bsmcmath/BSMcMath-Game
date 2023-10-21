using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class door : MonoBehaviour
{
    public Vector3 knob1, knob2;

    public Rigidbody rb;

    public Quaternion startingRotation;

    public float rotation;

    private void OnValidate()
    {
        startingRotation = transform.rotation;
    }

    public void moveRotation()
    {

    }
}
