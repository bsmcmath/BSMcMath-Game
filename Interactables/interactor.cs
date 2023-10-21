using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interactor : MonoBehaviour
{
    public interactorType type;

    public virtual void interact() { }
}
public enum interactorType
{
    item, ladder, lever, door, chest, woodenBox
}