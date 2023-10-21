using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class characterBase : MonoBehaviour
{
    public bool checkInteractors(Vector3 position, float radius, out interactor interactor, out float sqDistance)
    {
        Vector3 origin = skeleton.arma.position + Quaternion.Euler(0, memory.orientation.y, 0) * position;
        Collider[] cols = Physics.OverlapSphere(origin, radius, Main.main.layers.interactors);

        Debug.DrawLine(origin, origin + Vector3.forward * radius, Color.blue);
        Debug.DrawLine(origin, origin + Vector3.down * radius, Color.blue);

        bool found = false;

        int nearestIndex = 0;
        sqDistance = 100;
        for (int i = 0; i < cols.Length; i++)
        {
            float d = (cols[i].transform.position - origin).sqrMagnitude;
            if (d < sqDistance)
            {
                sqDistance = d;
                nearestIndex = i;
                found = true;
            }
        }

        if (found) interactor = cols[nearestIndex].GetComponent<interactor>(); 
        else interactor = null;

        return found;
    }
    public bool checkCharacters(Vector3 position, float radius, out characterBase o, out float sqDistance)
    {
        Vector3 origin = skeleton.arma.position + Quaternion.Euler(0, memory.orientation.y, 0) * position;
        Collider[] cols = Physics.OverlapSphere(origin, radius, Main.main.layers.characterRoots);

        bool found = false;
        int nearestIndex = 0;
        sqDistance = 100;
        for (int i = 0; i < cols.Length; i++)
        {
            float d = (cols[i].transform.position - origin).sqrMagnitude;
            if (d < sqDistance)
            {
                characterBase cb = cols[i].GetComponent<characterRoot>().c;
                if (cb != this)
                {
                    sqDistance = d;
                    nearestIndex = i;
                    found = true;
                }
            }
        }

        if (found) o = cols[nearestIndex].GetComponent<characterRoot>().c;
        else o = null;

        return found;
    }
    public characterBase[] nearbyCharacters(float radius)
    {
        Collider[] cols = Physics.OverlapSphere(skeleton.arma.position, radius, Main.main.layers.characterRoots);
        characterBase[] characters = new characterBase[cols.Length - 1];
        int n = 0;
        for (int i = 0; i < cols.Length; i++)
        {
            characterBase c = cols[i].GetComponent<characterRoot>().c;
            if (c != this)
            {
                characters[n] = c;
                n++;
            }
        }
        return characters;
    }
    public characterBase[] nearbyCharacters(Vector3 center, float radius)
    {
        Collider[] cols = Physics.OverlapSphere(center, radius, Main.main.layers.characterRoots);
        characterBase[] characters = new characterBase[cols.Length - 1];
        int n = 0;
        for (int i = 0; i < cols.Length; i++)
        {
            characterBase c = cols[i].GetComponent<characterRoot>().c;
            if (c != this)
            {
                characters[n] = c;
                n++;
            }
        }
        return characters;
    }
    public void environmentInfluences()
    {
        Colliders.PhysicsOverlap(components.armaCol, Main.main.layers.environmentInfluences, out Collider[] cols);

        for (int i = 0; i < cols.Length; i++)
        {
            environmentalInfluence ei = cols[i].GetComponent<environmentalInfluence>();
            temp.environmentInfluence += ei.push;
            temp.environmentDrag += ei.drag;
        }
    }
    //
    public void itemInHand(characterItem ci, item item, equipment equip)
    {
        item.pickup();

        if (ci.leftHand)
        {
            equip.leftHand.equipped = true;
            equip.leftHand.item = item;
            item.transform.parent = skeleton.handL;
        }
        else
        {
            equip.rightHand.equipped = true;
            equip.rightHand.item = item;
            item.transform.parent = skeleton.handR;
        }
        
        item.transform.localPosition = ci.localPosition;
        item.transform.localRotation = Quaternion.Euler(ci.localRotation);
    }
}
