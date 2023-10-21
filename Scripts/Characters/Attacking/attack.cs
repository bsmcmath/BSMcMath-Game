using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class characterBase : MonoBehaviour
{
    public void savePreviousTransform(Collider col)
    {
        memory.hitboxPosition = col.transform.position;
        memory.hitboxRotation = col.transform.rotation;
    }
    public void hitCheck(SphereCollider col, List<hurtbox> hits)
    {
        Colliders.PhysicsOverlap(col, memory.hitboxPosition, memory.hitboxRotation, Main.main.layers.attack, out Collider[] cols);

        for (int i = 0; i < cols.Length; i++)
        {
            hurtbox h = cols[i].GetComponent<hurtbox>();
            if (h.c != this) hits.Add(h);
        }
    }
    public void applyRecoil(Vector3 position, Vector3 direction, float strength)
    {
        characterRecoil r = new();
        r.direction = direction;
        r.momentum = strength;

        memory.recoil.Add(r);
    }
}