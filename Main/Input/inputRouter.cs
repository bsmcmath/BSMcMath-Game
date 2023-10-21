using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inputRouter : MonoBehaviour
{
    public Vector2 push, turn;

    public bool leftBump, leftTrigger, rightTrigger, rightBump;

    public bool start, select;

    public bool jump, interact, dodge, crouch;

    public bool up, down, left, right;

    public bool hurry, toggle;

    public Vector3 pushV3()
    {
        return new Vector3(push.x, 0, push.y);
    }
    public bool turnIsNonZero()
    {
        return turn.x > 0 || turn.y > 0 || turn.x < 0 || turn.y < 0;
    }
    public float turnAngle()
    {
        return Vector3.SignedAngle(Vector3.forward, Quaternion.Euler(0, Main.main.camera.transform.eulerAngles.y, 0) * new Vector3(turn.x, 0, turn.y), Vector3.up);
    }
}
