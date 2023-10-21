using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "bow", menuName = "ScriptableObjects/Bow")]
public class bowParams : ScriptableObject
{
    public Quaternion bow1base, bow2base;
    public Vector3 stringBase;

    public Vector3 bow1Drawn, bow2Drawn;
}
