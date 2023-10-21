using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class terrain : MonoBehaviour
{
    public terrainType type;
    //public terrainMaterial material;
}
public enum terrainType
{
    stationary, moving, climbable
}
public enum terrainMaterial
{
    wood, dirt, stone, gravel, sand, grass
}