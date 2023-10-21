using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class inventory : MonoBehaviour
{
    public bagMenu bag;
    public inventoryQuickMenu quick;
    public inventoryPrefabs prefabs;

    public item retrieveItem(itemType itemType)
    {
        GameObject go;
        switch (itemType)
        {
            default: go = new(); break;
            case itemType.sword: go = Instantiate(prefabs.items.sword); break;
            case itemType.bow: go = Instantiate(prefabs.items.bow); break;
        }
        return go.GetComponent<item>();
    }

}

[Serializable]
public class inventoryItems
{
    public GameObject rock, stick, knife, sling, sword, bow, shield;
}