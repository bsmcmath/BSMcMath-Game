using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class help
{
    public static Vector3 bezier(Vector3 pi, Vector3 pm, Vector3 pf, float t)
    {
        return (1.0f - t) * (1.0f - t) * pi + 2.0f * (1.0f - t) * t * pm + t * t * pf;
    }
    public static Vector3 bezier(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, float t)
    {
        return p1 * (1 - t) * (1 - t) * (1 - t) + p2 * 3 * t * (1 - t) * (1 - t) + p3 * 3 * t * t * (1 - t) + p4 * t * t * t;
    }
    public static float wrap(float x)
    {
        if (x > 360) x -= 360;
        else if (x < 0) x += 360;
        return x;
    }
    public static float wrap01(float x)
    {
        if (x < 0) x += 1;
        else if (x > 1) x -= 1;
        return x;
    }
    public static float wrap1minus1(float x)
    {
        if (x < -1) x += 2;
        else if (x > 1) x -= 2;
        return x;
    }
    public static float gaitPhaseWrap(float p, float o)
    {
        float g = p + o;
        if (p < 0)
        {
            if (g < -1) return g * -1 - 1;
            else if (g > 0) return g * -1 + 1;
            else return g;
        }
        else
        {
            if (g > 1) return g * -1 + 1;
            else if (g < 0) return g * -1 - 1;
            else return g;
        }
    }
    public static float map(float value, float low1, float high1, float low2, float high2) 
    {
        return low2 + (value - low1) * (high2 - low2) / (high1 - low1); 
    }
    public static float angleDifference(float from, float to)
    {
        float d = to - from;
        if (d > 180) d = help.map(d, 180, 360, -180, 0); else if (d < -180) d = help.map(d, -360, -180, 0, 180);
        return d;
    }
    public static float distance(Vector3 a, Vector3 b)
    {
        return (a - b).magnitude;
    }
    public static float distance(float a, float b)
    {
        return Mathf.Abs(a - b);
    }
    public static Vector3 lerpStartingPoint(Vector3 p0, Vector3 p1, Vector3 p1n, float t)
    {
        return (p0 * (1 - t) + p1 * t - p1n * t) / (1 - t);
    }
    public static Vector3 nearestPointOnLine(Vector3 point, Vector3 pointOnLine, Vector3 lineDirectionNorm)
    {
        Vector3 v = point - pointOnLine;
        float d = Vector3.Dot(v, lineDirectionNorm);
        v = pointOnLine + lineDirectionNorm * d;
        return v;
    }
    public static Quaternion tiltRotation(Vector3 direction, float degrees)
    {
        return Quaternion.RotateTowards(Quaternion.identity, Quaternion.FromToRotation(Vector3.up, direction), degrees);
    }
    public static bool lineSegmentIntersectCircle(Vector2 l1, Vector2 l2, Vector2 center, float radius, out Vector2 intersection)
    {
        Vector2 d = l2 - l1;
        Vector2 f = l1 - center;

        float a = Vector2.Dot(d, d);
        float b = 2 * Vector2.Dot(f, d);
        float c = Vector2.Dot(f, f) - radius * radius;

        float D = b * b - 4 * a * c;
        if (D < 0)
        {
            intersection = Vector2.zero;
            return false;
        }
        else
        {
            D = Mathf.Sqrt(D);

            float t = (-b - D) / (2 * a);

            if (t >= 0 && t <= 1)
            {
                intersection = l1 + t * d;
                return true;
            }
            else
            {
                intersection = Vector2.zero;
                return false;
            }
        }
    }
    public static bool lineSegmentIntersectLine(float x1, float y1, float x2, float y2, float x3, float y3, Vector2 direction)
    {
        float x4 = x3 + direction.x;
        float y4 = y3 + direction.y;

        float t1 = (x1 - x3) * (y3 - y4) - (y1 - y3) * (x3 - x4);
        t1 /= (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);

        float t2 = (x1 - x3) * (y1 - y2) - (y1 - y3) * (x1 - x2);
        t2 /= (x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4);

        return t1 > 0 && t1 < 1 && t2 > 0;
    }
    public static bool lineSegmentIntersectLine(Vector3 s1, Vector3 s2, Vector3 l0, Vector3 ld)
    {
        return lineSegmentIntersectLine(s1.x, s1.z, s2.x, s2.z, l0.x, l0.z, new Vector2(ld.x, ld.z));
    }
    public static Vector2 lineLineIntersection(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4)
    {
        return new Vector2(
            ((x1 * y2 - y1 * x2) * (x3 - x4) - (x1 - x2) * (x3 * y4 - y3 * x4)) / ((x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4)),
            ((x1 * y2 - y1 * x2) * (y3 - y4) - (y1 - y2) * (x3 * y4 - y3 * x4)) / ((x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4))
            );
    }
    public static Vector3 lineLineIntersection(Vector3 p1, Vector3 d1, Vector3 p2, Vector3 d2)
    {
        Vector2 i = lineLineIntersection(p1.x, p1.z, p1.x + d1.x, p1.z + d1.z, p2.x, p2.z, p2.x + d2.x, p2.z + d2.z);
        return new Vector3(i.x, 0, i.y);
    }
    public static bool lineSegmentsIntersection(float x1, float y1, float x2, float y2, float x3, float y3, float x4, float y4, out Vector2 intersection)
    {
        float t = ((x1 - x3) * (y3 - y4) - (y1 - y3) * (x3 - x4)) / ((x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4));
        float u = ((x1 - x3) * (y1 - y2) - (y1 - y3) * (x1 - x2)) / ((x1 - x2) * (y3 - y4) - (y1 - y2) * (x3 - x4));
        if (t > 0 && t < 1 && u > 0 && u < 1)
        {
            intersection.x = x1 + t * (x2 - x1);
            intersection.y = y1 + t * (y2 - y1);
            return true;
        }
        else
        {
            intersection = Vector2.zero;
            return false;
        }
    }
    public static bool lineSegmentsIntersection(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4, out Vector3 intersection)
    {
        bool intersected = lineSegmentsIntersection(p1.x, p1.z, p2.x, p2.z, p3.x, p3.z, p4.x, p4.z, out Vector2 intersect);
        intersection.x = intersect.x; intersection.y = 0; intersection.z = intersect.y;
        return intersected;
    }
    public static float distanceXZ(Vector3 v1, Vector3 v2)
    {
        Vector3 delta = v2 - v1; delta.y = 0; return delta.magnitude;
    }
    public static void moveMax(ref Vector3 value, Vector3 target, float lerp, float minSpeed)
    {
        target -= value;
        float d = target.magnitude;
        lerp *= d * Time.fixedDeltaTime;
        minSpeed *= Time.fixedDeltaTime;
        if (lerp > minSpeed) value += target * lerp / d;
        else value += target * Mathf.Min(minSpeed / d, 1); 
    }

    public static void moveMax(ref Quaternion value, Quaternion target, float lerp, float minSpeed)
    {
        float angle = Quaternion.Angle(value, target);
        lerp *= Time.fixedDeltaTime * angle;
        minSpeed *= Time.fixedDeltaTime;
        if (lerp > minSpeed) value = Quaternion.RotateTowards(value, target, lerp);
        else value = Quaternion.RotateTowards(value, target, minSpeed);
    }
    public static float moveTo(float value, float target, float lerp, float flat, bool min)
    {
        target -= value;
        float d = Mathf.Abs(target);
        if (min)
        {
            if (lerp * d < flat) value += target * lerp * Time.fixedDeltaTime;
            else value += target * Mathf.Min(flat * Time.fixedDeltaTime / d, 1);
        }
        else
        {
            if (lerp * d > flat) value += target * lerp * Time.fixedDeltaTime;
            else value += target * Mathf.Min(flat * Time.fixedDeltaTime / d, 1);
        }
        return value;
    }
    public static Vector3 moveTo(Vector3 value, Vector3 target, float lerp, float flat, bool min)
    {
        target -= value;
        float d = target.magnitude;
        if (min)
        {
            if (lerp * d < flat) value += target * lerp * Time.fixedDeltaTime;
            else value += target * Mathf.Min(flat * Time.fixedDeltaTime / d, 1);
        }
        else
        {
            if (lerp * d > flat) value += target * lerp * Time.fixedDeltaTime;
            else value += target * Mathf.Min(flat * Time.fixedDeltaTime / d, 1);
        }
        return value;
    }
    public static Vector3 moveTo(Vector3 value, Vector3 target, blendSettings blend)
    {
        if (blend.type == blendType.none) return target;
        else return moveTo(value, target, blend.lerp, blend.flat, blend.type == blendType.min);
    }
    public static Quaternion moveTo(Quaternion value, Quaternion target, float lerp, float flat, bool min)
    {
        float d = Quaternion.Angle(value, target);
        lerp *= d;
        if (min)
        {
            if (lerp < flat) value = Quaternion.RotateTowards(value, target, lerp * Time.fixedDeltaTime);
            else value = Quaternion.RotateTowards(value, target, flat * Time.fixedDeltaTime);
        }
        else
        {
            if (lerp > flat) value = Quaternion.RotateTowards(value, target, lerp * Time.fixedDeltaTime);
            else value = Quaternion.RotateTowards(value, target, flat * Time.fixedDeltaTime);
        }
        return value;
    }
    public static Quaternion moveTo(Quaternion value, Quaternion target, blendSettings blend)
    {
        if (blend.type == blendType.none) return target;
        else return moveTo(value, target, blend.lerp, blend.flat, blend.type == blendType.min);
    }
    public static void moveToAngle(ref float value, float target, float lerp, float flat, bool min, out float turnSpeed)
    {
        target = Mathf.DeltaAngle(value, target);
        float d = Mathf.Abs(target);
        if (min)
        {
            if (lerp * d < flat) turnSpeed = target * lerp * Time.fixedDeltaTime;
            else turnSpeed = target * Mathf.Min(flat * Time.fixedDeltaTime / d, 1);
        }
        else
        {
            if (lerp * d > flat) turnSpeed = target * lerp * Time.fixedDeltaTime;
            else turnSpeed = target * Mathf.Min(flat * Time.fixedDeltaTime / d, 1);
        }
        value = wrap(value + turnSpeed);
    }
    public static void moveToAngle(ref float value, float target, blendSettings blend, out float turnSpeed)
    {
        if (blend.type == blendType.none)
        {
            value = target;
            turnSpeed = 0;
        }
        else moveToAngle(ref value, target, blend.lerp, blend.flat, blend.type == blendType.min, out turnSpeed);
    }
    public static void moveToAngle(ref Vector2 value, Vector2 target, float lerp, float flat, bool min, out Vector2 turnSpeed)
    {
        target.x = Mathf.DeltaAngle(value.x, target.x);
        target.y = Mathf.DeltaAngle(value.y, target.y);
        float d = target.magnitude;
        if (min)
        {
            if (lerp * d < flat) turnSpeed = target * lerp * Time.fixedDeltaTime;
            else turnSpeed = target * Mathf.Min(flat * Time.fixedDeltaTime / d, 1);
        }
        else
        {
            if (lerp * d > flat) turnSpeed = target * lerp * Time.fixedDeltaTime;
            else turnSpeed = target * Mathf.Min(flat * Time.fixedDeltaTime / d, 1);
        }
        value += turnSpeed;
        value.x = help.wrap(value.x); value.y = help.wrap(value.y);
    }

}
