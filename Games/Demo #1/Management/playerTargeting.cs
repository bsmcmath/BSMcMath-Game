using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerTargeting : MonoBehaviour
{
    [HideInInspector] public bool isTargeting;
    public RectTransform targetIcon;
    [HideInInspector] public Transform target;
    public void updateTargetIcon()
    {
        if (isTargeting)
        {
            Vector2 position = Camera.main.WorldToScreenPoint(target.position);
            position.y += 50;
            targetIcon.anchoredPosition = position;
        }
    }
    public void targetingOn(Transform targ)
    {
        isTargeting = true;
        targetIcon.gameObject.SetActive(true);
        target = targ;
    }
    public void targetingOff()
    {
        isTargeting = false;
        targetIcon.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (isTargeting) updateTargetIcon();
    }
}
