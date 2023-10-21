using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class gameSelect : MonoBehaviour
{
    public GraphicRaycaster graphicRaycaster;
    //public UIoption[] options;
    public UIoption selection;
    private void Update()
    {
        if (Main.main.input.p1_keyboard || !Main.main.input.p1_joined)
        {
            PointerEventData ped = new(null);
            ped.position = Mouse.current.position.ReadValue();
            List<RaycastResult> results = new();
            graphicRaycaster.Raycast(ped, results);
            UIoption u = null;
            for (int i = 0; i < results.Count; i++)
            {
                if (results[i].gameObject.layer == 31)
                {
                    u = results[i].gameObject.GetComponent<UIoption>();
                    break;
                }
            }

            if (u)
            {
                if (u == selection)
                {
                    selection.OnSelectStay();
                }
                else
                {
                    if (selection) selection.OnDeselect();
                    selection = u;
                    selection.OnSelect();
                }
            }
            else if (selection)
            {
                selection.OnDeselect();
                selection = null;
            }
        }

        if (Main.main.input.inputP1.interact || Main.main.input.inputP1.rightTrigger || Mouse.current.leftButton.isPressed)
        {
            Main.main.input.inputP1.interact = false;
            Main.main.input.inputP1.rightTrigger = false;
            if (selection) selection.MakeSelection();
        }
    }
}
