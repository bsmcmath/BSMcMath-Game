using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TerrainCasts
{
    public static void stepSphere(Vector3 origin, Vector3 direction, float distance, float radius, out terrainHit hit)
    {
        hit = new terrainHit();
        if (Physics.SphereCast(origin, radius, direction, out RaycastHit rhit, distance, Main.main.layers.exactTerrain))
        {
            hit.hit = true;
            hit.position = rhit.point;
            hit.normal = rhit.normal;
            hit.obj = rhit.collider.GetComponent<terrain>();
            hit.localPosition = hit.position - hit.obj.transform.position;
            hit.localPosition = Quaternion.Inverse(hit.obj.transform.rotation) * hit.localPosition;
            hit.distance = rhit.distance;
            Debug.DrawLine(origin, rhit.point, Color.white);
        }
        else Debug.DrawRay(origin, direction.normalized * distance, Color.white);
    }
    public static void stepRay(Vector3 origin, Vector3 direction, float distance, out terrainHit hit)
    {
        hit = new terrainHit();
        if (Physics.Raycast(origin, direction, out RaycastHit rhit, distance, Main.main.layers.exactTerrain))
        {
            hit.hit = true;
            hit.position = rhit.point;
            hit.normal = rhit.normal;
            hit.obj = rhit.collider.GetComponent<terrain>();
            hit.localPosition = hit.position - hit.obj.transform.position;
            hit.localPosition = Quaternion.Inverse(hit.obj.transform.rotation) * hit.localPosition;
            hit.distance = rhit.distance;
            Debug.DrawLine(origin, rhit.point, Color.yellow);
        }
        else Debug.DrawRay(origin, direction.normalized * distance, Color.yellow);
    }
    public static void physicsRay(Vector3 origin, Vector3 direction, float distance, out terrainHit hit)
    {
        hit = new terrainHit();
        if (Physics.Raycast(origin, direction, out RaycastHit rhit, distance, Main.main.layers.terrain))
        {
            hit.hit = true;
            hit.position = rhit.point;
            hit.normal = rhit.normal;
            hit.obj = rhit.collider.GetComponent<terrain>();
            hit.localPosition = hit.position - hit.obj.transform.position;
            hit.localPosition = Quaternion.Inverse(hit.obj.transform.rotation) * hit.localPosition;
            hit.distance = rhit.distance;
            Debug.DrawLine(origin, rhit.point, Color.green);
        }
        else Debug.DrawRay(origin, direction.normalized * distance, Color.green);
    }
    public static void stepEdge(Vector3 from, Vector3 to, out terrainHit hit)
    {
        Vector3 direction = to - from;
        Vector3 origin = from;
        origin.y += 0.02f;
        stepRay(origin, direction, direction.magnitude, out hit);
        if (hit.hit)
        {
            if (hit.position.y >= origin.y)
            {
                origin = hit.position + direction.normalized * 0.02f;
                origin.y = to.y + 1;
                stepRay(origin, Vector3.down, to.y - from.y + 1, out hit);
            }
            else
            {
                origin = to;
                origin.y = hit.position.y - 0.02f;
                direction.y = 0;
                direction *= -1;
                stepRay(origin, direction, direction.magnitude, out hit);
            }
        }
    }
    public static void physicsEdge(Vector3 from, Vector3 to, out terrainHit hit)
    {
        Vector3 direction = to - from;
        Vector3 origin = from;
        origin.y += 0.02f;
        physicsRay(origin, direction, direction.magnitude, out hit);
        if (hit.hit)
        {
            if (hit.position.y >= origin.y)
            {
                origin = hit.position + direction.normalized * 0.02f;
                origin.y = to.y + 1;
                physicsRay(origin, Vector3.down, to.y - from.y + 1, out hit);
            }
            else
            {
                origin = to;
                origin.y = hit.position.y - 0.02f;
                direction.y = 0;
                direction *= -1;
                physicsRay(origin, direction, direction.magnitude, out hit);
            }
        }
    }
}

public class terrainHit
{
    public Vector3 position, localPosition, normal;
    public terrain obj;
    public bool hit;
    public float distance;

    public terrainHit() { }
    public terrainHit(Vector3 pos)
    {
        position = pos;
        normal = Vector3.up;
    }
}