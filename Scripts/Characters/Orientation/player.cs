using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class characterBase : MonoBehaviour
{
    public void push(inputRouter ir)
    {
        temp.push = Quaternion.Euler(0, Main.main.camera.transform.rotation.eulerAngles.y, 0) * new Vector3(ir.push.x, 0, ir.push.y);
        pushCalculations();
    }
}
public class equipment
{
    public equipSlot leftHand, rightHand;
}
public struct equipSlot
{
    public bool equipped;
    public item item;

    public void drop()
    {
        equipped = false;
        item.drop();
    }
    public void destroy()
    {
        equipped = false;
        GameObject.DestroyImmediate(item.gameObject);
    }
}