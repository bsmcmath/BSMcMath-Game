using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class spyController : characterController
//{
//    public spy s;
//    public inputRouter ir;
//    private void Start()
//    {
//        ir = GM.gm.input.inputP1;
//        //GM.gm.cam.target = s.skeleton.arma; GM.gm.cam.mode = cameraMode.thirdPerson; GM.gm.cam.localPosition = GM.gm.cam.cp.mainPosition;
//        s.equip = new equipment();
//    }
//    //
//    public override void movement()
//    {
//        if (s.equip.rightHand.equipped)
//        {
//            //switch (s.equip.rightHand.item.itemType)
//            //{
//            //    //case itemType.gun: holdingGun_Movement();
//            //    //    break;
//            //    //case itemType.melee: holdingMelee_Movement();
//            //    //    break;
//            //}
//        }
//        else handsFree_Movement();
//    }
//    public void handsFree_Movement()
//    {
//        //characterPlayer.push(s, ir);
//        //Stepping.pushCheck(s, s.basicStepping);
//        //Stepping.groundCheck(s, s.basicStepping);
//        {
//            if (ir.leftTrigger)
//            {
//                //characterPlayer.turn(s, ir);
//                //s.memory.lookTarget = new Vector3(0, s.memory.action.yRotation, 0);

//                //GM.gm.cam.disableTurn = true;
//            }
//            else
//            {
//                //turning.turnToPush(s);
//                //s.memory.lookTarget = new Vector3(0, s.memory.action.yRotation, 0);

//                //GM.gm.cam.disableTurn = false;
//            }
//            //GM.gm.cam.localPosition = GM.gm.cam.cp.mainPosition;

//            //s.basicMove();
//        }
//    }
//    public void holdingGun_Movement()
//    {
//        gun gun = (gun)s.equip.rightHand.item;
//        gun.frameUpdate();

//        //characterPlayer.push(s, ir);
//        //Stepping.pushCheck(s, s.basicStepping);
//        //Stepping.groundCheck(s, s.basicStepping);

//        if (s.aimingSights)
//        {
//            //turning.turnToCam(s);
//            //s.memory.lookTarget = new Vector3(s.memory.action.xRotate, s.memory.action.yRotate, 0);

//            //GM.gm.cam.disableTurn = false;
//            //GM.gm.cam.localPosition = gun.gunParams.sightsCameraPosition;

//            //s.aimSightsGun();
//            //GM.gm.cam.player3rdPerson();

//            if (!ir.leftTrigger) { s.aiming = true; s.aimingSights = false; GM.gm.cam.target = s.skeleton.arma; GM.gm.cam.startBlend(1); }
//            else if (ir.interact) { s.aimingSights = false; ir.interact = false; GM.gm.cam.target = s.skeleton.arma; gun.reset(); GM.gm.cam.startBlend(1); }
//        }
//        else if (s.aiming)
//        {
//            //turning.turnToCam(s);
//            //s.memory.lookTarget = new Vector3(s.memory.action.xRotate, s.memory.action.yRotate, 0);

//            //GM.gm.cam.localPosition = GM.gm.cam.cp.aimPosition;
//            //GM.gm.cam.disableTurn = false;

//            //s.aimGun();

//            if (ir.interact) { s.aiming = false; ir.interact = false; gun.reset(); GM.gm.cam.startBlend(1); }
//            else if (ir.leftTrigger) { s.aimingSights = true; s.aiming = false; GM.gm.cam.target = gun.transform; GM.gm.cam.startBlend(1); }
//        }
//        else
//        {
//            if (ir.leftTrigger)
//            {
//                //characterPlayer.turn(s, ir);
//                //s.memory.action.xRotate = 0;
//                //s.memory.lookTarget = new Vector3(0, s.memory.action.yRotation, 0);
//                //GM.gm.cam.disableTurn = true;
//            }
//            else
//            {
//                //turning.turnToPush(s);
//                //s.memory.action.xRotate = 0;
//                //s.memory.lookTarget = new Vector3(0, s.memory.action.yRotation, 0);
//                //GM.gm.cam.disableTurn = false;
//            }
//            //GM.gm.cam.localPosition = GM.gm.cam.cp.mainPosition;

//            //s.holdGun();

//            if (ir.rightTrigger) { s.aiming = true; ir.rightTrigger = false; GM.gm.cam.startBlend(1); }
//            else if (ir.interact) { s.equip.rightHand.drop(); ir.interact = false; }
//        }
//    }
//    public void holdingMelee_Movement()
//    {
//        //characterPlayer.push(s, ir);
//        //Stepping.pushCheck(s, s.basicStepping);
//        //Stepping.groundCheck(s, s.basicStepping);
//        {
//            if (ir.leftTrigger)
//            {
//                //characterPlayer.turn(s, ir);
//                //s.memory.lookTarget = new Vector3(0, s.memory.action.yRotation, 0);

//                //GM.gm.cam.disableTurn = true;
//            }
//            else
//            {
//                //turning.turnToPush(s);
//                //s.memory.lookTarget = new Vector3(0, s.memory.action.yRotation, 0);

//                //GM.gm.cam.disableTurn = false;
//            }
//            //GM.gm.cam.localPosition = GM.gm.cam.cp.mainPosition;

//            //s.holdMelee();

//            if (ir.interact) { s.equip.rightHand.drop(); ir.interact = false; }
//        }
//    }
//    //
//    public override void interactionDetection()
//    {
//        if (s.equip.rightHand.equipped)
//        {
//            //switch (s.equip.rightHand.item.itemType)
//            //{
//            //    //case itemType.gun:
//            //    //    holdingGun_Interaction();
//            //    //    break;
//            //    //case itemType.melee:
//            //    //    holdingMelee_Interaction();
//            //    //    break;
//            //}
//        }
//        else handsFree_Interaction();
//    }
//    public void handsFree_Interaction()
//    {
//        if (ir.interact)
//        {
//            bool foundInteractor = s.checkInteractors(s.spyParams.interactPosition, s.spyParams.interactRadius, out s.memory.interactor, out float intDist);
//            bool foundCharacter = s.checkCharacters(s.spyParams.interactPosition, s.spyParams.interactRadius, out characterBase other, out float charDist);
//            if (foundInteractor && (intDist < charDist || !foundCharacter))
//            {
//                ir.interact = false;
//                switch (s.memory.interactor.type)
//                {
//                    case interactorType.item:
//                        pickupItem((item)s.memory.interactor);
//                        break;
//                }
//            }
//            else if (foundCharacter)
//            {
//                ir.interact = false;
//                other.controller.interact();
//            }
//        }
//    }
//    public void pickupItem(item item)
//    {
//        //switch (item.itemType)
//        //{
//        //    case itemType.gun:

//        //        switch (((gun)item).gunParams.gunType)
//        //        {
//        //            case gunType.pistol:
//        //                s.itemInHand(s.spyParams.pistol, item, s.equip);
//        //                break;
//        //            case gunType.machinegun:
//        //                s.itemInHand(s.spyParams.machineGun, item, s.equip);
//        //                break;
//        //        }
//        //        break;

//        //    case itemType.melee:

//        //        switch (((melee)item).meleeParams.meleeType)
//        //        {
//        //            case meleeType.knife:
//        //                s.itemInHand(s.spyParams.knife, item, s.equip);
//        //                break;
//        //        }
//        //        break;
//        //}
//    }
//    public void holdingGun_Interaction()
//    {
//        if (s.aimingSights || s.aiming)
//        {
//            if (ir.rightTrigger) 
//            { 
//                gun g = (gun)s.equip.rightHand.item; 
//                g.pullTrigger(); 
//                if (g.gunParams.firingMode != firingMode.automatic) ir.rightTrigger = false; 
//            }
//        }
//    }
//    public void holdingMelee_Interaction()
//    {

//    }
//    //
//    public override void finalizeFrame()
//    {

//    }
//    //


//    public override void interact()
//    {

//    }

//}
