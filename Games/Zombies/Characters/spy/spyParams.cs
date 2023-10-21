using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "spy", menuName = "ScriptableObjects/NuclearSolution/Spy")]
public class spyParams : ScriptableObject
{
    public Vector3 interactPosition;
    public float interactRadius;
    //

    public characterItem pistol;

    //public armsGaitConfig holdPistol;

    //public armsGaitConfig aimPistol;
    public float aimPistolYOffset;

    //public armsGaitConfig aimPistolSights;
    public float aimPistolSightsYOffset;
    //

    public characterItem machineGun;

    //public armsGaitConfig holdMachineGun;

    //public armsGaitConfig aimMachineGun;
    public float aimMachineGunYOffset;

    //public armsGaitConfig aimSightsMachineGun;
    public float aimSightsMachineGunYOffset;
    //

    public characterItem knife;

    //public armsGaitConfig holdKnife;
}
