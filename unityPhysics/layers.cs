using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Layers", menuName = "ScriptableObjects/Layers")]
public class layers : ScriptableObject
{
    public LayerMask terrain, 
        cameraObstructions,
        characterRoots, 
        exactTerrain,
        interactors,
        environmentInfluences,
        water,
        projectile,
        attack;
}
