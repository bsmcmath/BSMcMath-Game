using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gun : item
{
    public ParticleSystem muzzleFlash;
    public ParticleSystem groundHit, characterHit;
    public LineRenderer bulletTrail;

    public gunParams gunParams;

    public float recoil;
    public float recoilMomentum;
    public int rounds;
    public float timer;

    public void pullTrigger()
    {
        if (timer > 0) return;
        if (rounds == 0) { } //click

        timer = gunParams.rechamberTime;
        recoilMomentum += gunParams.recoil;
        rounds -= 1;

        //shoot
        muzzleFlash.Play();
        bulletTrail.SetPosition(0, bulletTrail.transform.position);

        if (Physics.Raycast(bulletTrail.transform.position, transform.rotation * Vector3.forward, out RaycastHit hit, 100, Main.main.layers.projectile))
        {
            bulletTrail.SetPosition(1, hit.point);
            //hit effect
            switch (hit.collider.gameObject.layer)
            {
                case 0: 
                    hitTerrain(hit); 
                    break;
                case 6: //character
                    hitCharacter(hit);
                    break;
                case 10: //terrain
                    hitTerrain(hit);
                    break;
            }
        }
        else bulletTrail.SetPosition(1, bulletTrail.transform.position + transform.rotation * new Vector3(0, 0, 30));
        bulletTrail.enabled = true;
    }
    public void hitTerrain(RaycastHit hit)
    {
        groundHit.transform.position = hit.point;
        groundHit.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
        groundHit.Play();
    }
    public void hitCharacter(RaycastHit hit)
    {
        characterHit.transform.position = hit.point;
        characterHit.transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
        characterHit.Play();

        hurtbox h = hit.collider.GetComponent<hurtbox>();
        characterBase c = h.c;
        characterRecoil r = new characterRecoil();
        r.direction = transform.rotation * Vector3.forward;
        r.height = help.map(hit.point.y, c.skeleton.footL.position.y, c.skeleton.head.position.y, 0, 1);
        Vector3 perpendicular = hit.point - c.skeleton.highSpine.position;
        r.perpendicular = Vector3.Dot(perpendicular, transform.rotation * Vector3.right);
        r.momentum = gunParams.hitMomentum;
        c.memory.recoil.Add(r);

        float damage = gunParams.damage;
        //c.controller.recieveAttack(damage, h);
    }
    public void frameUpdate()
    {
        bulletTrail.enabled = timer > gunParams.trailCut;
        timer -= Time.fixedDeltaTime;

        if (recoilMomentum > 0)
        {
            recoil += recoilMomentum * Time.fixedDeltaTime;
            recoilMomentum = help.moveTo(recoilMomentum, 0, gunParams.recoilMomentumDecay.lerp, gunParams.recoilMomentumDecay.flat, gunParams.recoilMomentumDecay.type == blendType.min);
        }

        recoil = help.moveTo(recoil, 0, gunParams.recoilDecay.lerp, gunParams.recoilDecay.flat, gunParams.recoilDecay.type == blendType.min);
    }
    public void reset()
    {
        timer = 0;
        recoil = 0;
        recoilMomentum = 0;
        bulletTrail.enabled = false;
    }
}
public enum gunType
{
    pistol, machinegun
}