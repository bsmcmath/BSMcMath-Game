using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIoption : MonoBehaviour
{
    public TextMeshProUGUI text;
    public virtual void OnSelect()
    {
        text.color = Color.green;
    }
    public virtual void OnSelectStay()
    {

    }
    public virtual void OnDeselect()
    {
        text.color = Color.white;
    }
    public virtual void MakeSelection()
    {

    }
}
