using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "gun", menuName = "ScriptableObjects/Gun")]
public class gunParams : ScriptableObject
{
    public int rounds;
    public float rechamberTime;
    public float trailCut;
    public Vector3 muzzlePosition;
    public Vector3 sightsCameraPosition;
    public firingMode firingMode;
    public gunType gunType;
    public float recoil;
    public float recoilPositionZ, recoilPositionY;
    public float bodyRecoil;
    public blendSettings recoilDecay, recoilMomentumDecay;
    public float hitMomentum;
    public float damage;
}
public enum firingMode
{
    semiAuto, automatic, boltAction
}