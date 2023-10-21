using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class item : interactor
{
    public itemType itemType;
    public Rigidbody rb;
    public void pickup()
    {
        gameObject.layer = 12;
        GameObject.DestroyImmediate(rb);
    }
    public void drop()
    {
        gameObject.layer = 14;
        rb = gameObject.AddComponent<Rigidbody>();
        transform.parent = null;
    }
}
public enum itemType
{
    rock, knife, torch, sword, bow, shield
}

[Serializable]
public class characterItem
{
    public bool leftHand;
    public Vector3 localPosition;
    public Vector3 localRotation;
}
