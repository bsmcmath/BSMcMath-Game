using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zombieController : characterController
{
    //public zombie z;
    //public float timer, patrolLoopTime;
    ////public override void movement()
    ////{
    ////    if (z.dead) dead();
    ////    else movePatrol();
    ////}
    ////

    ////
    //public void movePatrol()
    //{
    //    //timer += Time.fixedDeltaTime;
    //    //if (timer > patrolLoopTime) timer -= patrolLoopTime;

    //    //z.memory.pushAngle = timer * 360 / patrolLoopTime;
    //    //z.memory.push = Vector3.zero;
    //    //Stepping.pushCheck(z, z.stepping);
    //    //Stepping.groundCheck(z, z.stepping);

    //    //z.memory.action.yRotate = 90;
    //    //z.memory.action.xRotate = 0;
    //    //turning.turnDirect(z);

    //    //z.memory.lookTarget = new Vector3(0, z.memory.action.yRotate, 0);

    //    z.basicMove();
    //}
    //public void killed()
    //{
    //    //z.components.enableRagdoll(z);
    //    z.health = 100;
    //    z.dead = true;
    //    timer = 3;
    //    //z.memory.action.recoil.Clear();
    //}
    //public void dead()
    //{
    //    timer -= Time.fixedDeltaTime;
    //    if (timer < 0)
    //    {
    //        //z.components.disableRagdoll(z);
    //        z.dead = false;
    //        z.health = 100;
    //    }
    //}

}
