using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class characterBase : MonoBehaviour
{
    public void terrainCollisionXZ()
    {
        Colliders.PhysicsOverlap(components.armaCol, Main.main.layers.terrain, out Collider[] hits);
        for (int i = 0; i < hits.Length; i++)
        {
            Colliders.ComputePenetration(components.armaCol, hits[i], out Vector3 direction, out float distance);

            direction.y = 0;
            skeleton.arma.position += direction * distance;

            float xzMagnitude = direction.magnitude;
            direction /= xzMagnitude;

            memory.velocity -= Vector3.Project(memory.velocity, direction);
            temp.acceleration -= Vector3.Project(temp.acceleration, direction);
        }
    }
    public void terrainCollision()
    {
        Colliders.PhysicsOverlap(components.armaCol, Main.main.layers.terrain, out Collider[] hits);
        for (int i = 0; i < hits.Length; i++)
        {
            Colliders.ComputePenetration(components.armaCol, hits[i], out Vector3 direction, out float distance);

            skeleton.arma.position += direction * distance;

            float xzMagnitude = direction.magnitude;
            direction /= xzMagnitude;

            memory.velocity -= Vector3.Project(memory.velocity, direction);
            temp.acceleration -= Vector3.Project(temp.acceleration, direction);
        }
    }
    public void characterSeparation(float strength)
    {
        Colliders.PhysicsOverlap(components.armaCol, Main.main.layers.characterRoots, out Collider[] hits);
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i] == components.armaCol) continue;
            Colliders.ComputePenetration(components.armaCol, hits[i], out Vector3 direction, out float distance);

            direction.y = 0;
            temp.acceleration += direction * distance * strength;
        }
    }
}
