using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public partial class characterBase : MonoBehaviour
{
    public void step_bodyLeftArmAnimation_state()
    {

    }

}

[Serializable]
public class bodyArmAnimation
{
    public float duration;
    public bodyAnimation body;
    public armAnimation arm;
}

[Serializable]
public class bodyArmsAnimation
{
    public float duration;
    public bodyAnimation body;
    public armAnimation leftArm;
    public armAnimation rightArm;
}

[Serializable]
public class moveBodyArmsAnimation
{
    public float duration;
    public moveAnimation move;
    public bodyAnimation body;
    public armAnimation leftArm;
    public armAnimation rightArm;
}