using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class characterController : controller
{
    [HideInInspector] public bool onTarget;
    [HideInInspector] public Transform target;
    [HideInInspector] public characterBase targetCharacter;

    public void playerTargeting_thirdPerson(characterBase c, inputRouter input, float radius, playerTargeting playerTargeting)
    {
        Vector3 toTarget;
        Vector3 camFwd = Main.main.camera.transform.rotation * Vector3.forward;
        float angle;
        if (onTarget)
        {
            //ADD raycast line of sight
            toTarget = target.position - Main.main.camera.transform.position;
            float ang = Vector3.Angle(toTarget, camFwd);
            if (toTarget.magnitude > radius + 1 || ang > 55)
            {
                onTarget = false;
                playerTargeting.targetingOff();
            }
            else
            {
                //difference.y = 0;
                //GM.gm.cam.orientation.y = Vector3.SignedAngle(Vector3.forward, difference, Vector3.up);
            }
        }

        if (!input.toggle) return;
        input.toggle = false;

        if (onTarget)
        {
            onTarget = false;
            playerTargeting.targetingOff();
            return;
        }

        characterBase[] characters = c.nearbyCharacters(Main.main.camera.transform.position, radius);
        if (characters.Length == 0) return;

        //target closest to camera angle
        target = characters[0].skeleton.arma;
        targetCharacter = characters[0];
        toTarget = target.position - Main.main.camera.transform.position;
        angle = Vector3.Angle(toTarget, camFwd);

        for (int i = 1; i < characters.Length; i++)
        {
            toTarget = characters[i].skeleton.arma.position - Main.main.camera.transform.position;
            float a = Vector3.Angle(toTarget, camFwd);
            if (a < angle)
            {
                target = characters[i].skeleton.arma;
                targetCharacter = characters[i];
                angle = a;
            }
        }

        if (angle > 55) return;

        onTarget = true;
        playerTargeting.targetingOn(targetCharacter.skeleton.head);
    }
    public void playerTargeting_twinStick(characterBase c, inputRouter input, float radius, playerTargeting playerTargeting)
    {
        //target too far, clear it
        if (onTarget)
        {
            if ((target.position - c.skeleton.arma.position).magnitude > radius + 1)
            {
                onTarget = false;
                playerTargeting.targetingOff();
            }
        }

        //no change
        if (!input.toggle) return;
        input.toggle = false;

        characterBase[] characters = c.nearbyCharacters(radius);
        if (characters.Length == 0) return;

        
        if (input.turn.sqrMagnitude < 0.5f)
        {
            if (onTarget)
            {
                onTarget = false;
                playerTargeting.targetingOff();
                return;
            }

            //target closest
            target = characters[0].skeleton.arma;
            targetCharacter = characters[0];
            float distance = help.distance(c.skeleton.arma.position, target.position);
            for (int i = 1; i < characters.Length; i++)
            {
                float d = help.distance(c.skeleton.arma.position, characters[i].skeleton.arma.position);
                if (d < distance)
                {
                    target = characters[i].skeleton.arma;
                    targetCharacter = characters[i];
                    distance = d;
                }
            }

            onTarget = true;
            playerTargeting.targetingOn(targetCharacter.skeleton.head);
        }
        else
        {
            //target closest angle
            Vector3 turnVector = Quaternion.Euler(0, Main.main.camera.orientation.y, 0) * new Vector3(input.turn.x, 0, input.turn.y);

            target = characters[0].skeleton.arma;
            targetCharacter = characters[0];
            Vector3 toTarget = target.position - c.skeleton.arma.position; toTarget.y = 0;
            float angle = Vector3.Angle(toTarget, turnVector);

            for (int i = 1; i < characters.Length; i++)
            {
                toTarget = characters[i].skeleton.arma.position - c.skeleton.arma.position;
                float a = Vector3.Angle(toTarget, turnVector);
                if (a < angle)
                {
                    target = characters[i].skeleton.arma;
                    targetCharacter = characters[i];
                    angle = a;
                }
            }

            onTarget = true;
            playerTargeting.targetingOn(targetCharacter.skeleton.head);
        }
    }
    public void tryTargetCharacter(characterBase c, characterBase toTarget, float radius)
    {
        characterBase[] characters = c.nearbyCharacters(radius);
        if (characters.Length == 0) return;

        for (int i = 0; i < characters.Length; i++)
        {
            if (characters[i] == toTarget)
            {
                target = characters[i].skeleton.head;
                targetCharacter = characters[i];
                onTarget = true;
                return;
            }
        }
    }
    public void targetDistanceCheck(characterBase c, float radius)
    {
        if ((target.position - c.skeleton.arma.position).magnitude > radius + 1)
        {
            onTarget = false;
        }
    }
    public void playerCameraJump(characterBase c, inputRouter input)
    {
        if (input.toggle)
        {
            input.toggle = false;
            if (c.temp.pushMagnitude > 0)
            {
                Main.main.camera.orientation.y = c.temp.pushAngle;
                Main.main.camera.startBlend(1);
            }
        }
    }
}
