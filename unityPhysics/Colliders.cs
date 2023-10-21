using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Colliders
{
    public static void PhysicsOverlap(CapsuleCollider col, LayerMask layers, out Collider[] hits)
    {
        float halfHeight = col.height * 0.5f - col.radius;
        Vector3 start = col.center + new Vector3(0, 0, halfHeight);
        Vector3 end = col.center + new Vector3(0, 0, -halfHeight);
        start = col.transform.TransformPoint(start);
        end = col.transform.TransformPoint(end);

        hits = Physics.OverlapCapsule(start, end, col.radius, layers);
    }
    public static void ComputePenetration(Collider c1, Collider c2, out Vector3 direction, out float distance)
    {
        Physics.ComputePenetration(c1, c1.transform.position, c1.transform.rotation, c2, c2.transform.position, c2.transform.rotation, out direction, out distance);
    }
    public static void PhysicsOverlap(SphereCollider col, Vector3 prevPosition, Quaternion prevRotation, LayerMask layers, out Collider[] hits)
    {
        Vector3 start = prevPosition + prevRotation * col.center;
        Vector3 end = col.transform.TransformPoint(col.center);
        hits = Physics.OverlapCapsule(start, end, col.radius, layers);
        Debug.DrawLine(start, end, Color.red);
    }
    public static void PhysicsOverlap(CapsuleCollider col, Vector3 prevPosition, Quaternion prevRotation, LayerMask layers, out RaycastHit[] hits)
    {
        float halfHeight = col.height * 0.5f - col.radius;
        Vector3 start = col.center + new Vector3(0, 0, halfHeight);
        Vector3 end = col.center + new Vector3(0, 0, -halfHeight);
        start = col.transform.TransformPoint(start);
        end = col.transform.TransformPoint(end);

        Vector3 center = col.transform.TransformPoint(col.center);
        Vector3 prevCenter = prevPosition + prevRotation * col.center;
        Vector3 direction = center - prevCenter;
        hits = Physics.CapsuleCastAll(start, end, col.radius, direction, direction.magnitude, layers);
    }
}
