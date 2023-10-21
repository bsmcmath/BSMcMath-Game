using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class breakable : MonoBehaviour
{
    public GameObject pieces;

    [ContextMenu("breakObject")]
    public void breakObject()
    {
        pieces.SetActive(true);
        gameObject.SetActive(false);
    }
}
