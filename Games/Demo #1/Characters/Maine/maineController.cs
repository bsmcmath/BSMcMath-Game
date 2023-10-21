using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace forthright
{
    public class maineController : characterController
    {
        public maine m;
        public inputRouter input;

        public float targetingTimer;
        public bool cameraToggle;

        private void Start()
        {
            input = Main.main.input.inputP1;
            //GM.gm.cameraManager.setThirdPerson(m.skeleton.arma);
        }

        //
        public override void movement()
        {
            m.temp = new();
            m.anim = new();

            m.push(input);
            standardCameraControl();
            m.environmentInfluences();
            m.waterCheck();

            if (m.equip.leftHand.equipped)
            {
                switch (m.equip.leftHand.item.itemType)
                {
                    case itemType.torch: torch_movement(); break;
                    case itemType.bow: bow_movement(); break;
                    case itemType.shield:
                        if (m.equip.rightHand.equipped)
                        {
                            switch (m.equip.rightHand.item.itemType)
                            {
                                case itemType.sword: swordShield_movement(); break;
                            }
                        }
                        else shield_movement();
                        break;
                }
            }
            else if (m.equip.rightHand.equipped)
            {
                switch (m.equip.rightHand.item.itemType)
                {
                    case itemType.knife: knife_movement(); break;
                    case itemType.sword: movement_sword(); break;
                    case itemType.rock: knife_movement(); break;
                }
            }
            else movement_handsFree();
        }
        public override void interactionDetection()
        {
            if (m.equip.leftHand.equipped)
            {
                switch (m.equip.leftHand.item.itemType)
                {
                    case itemType.torch:
                        break;
                    case itemType.bow:
                        break;
                }
            }
            else if (m.equip.rightHand.equipped)
            {
                switch (m.equip.rightHand.item.itemType)
                {
                    case itemType.knife:
                        break;
                    case itemType.sword:
                        break;
                    case itemType.rock:
                        break;
                }
            }
            else handsFree_interaction();
        }
        public override void receiveAttack(Vector3 position, Vector3 direction, float damage, float strength, hurtbox h)
        {

        }
        public override void finalizeFrame()
        {


            m.temp = null;
            m.anim = null;
        }

        //
        public void standardCameraControl()
        {
            switch (GM.gm.cameraManager.mode)
            {
                case cameraMode.isometric:
                    if (input.leftBump)
                    {
                        targetingTimer += Time.fixedDeltaTime;
                        cameraToggle = true;
                    }
                    else
                    {
                        if (cameraToggle && targetingTimer < 0.25f)
                        {
                            GM.gm.cameraManager.setThirdPerson(m.skeleton.arma);
                        }
                        targetingTimer = 0;
                        cameraToggle = false;
                    }
                    playerTargeting_twinStick(m, input, 32, GM.gm.playerTargeting);
                    break;
                case cameraMode.thirdPerson:
                    if (input.leftBump)
                    {
                        targetingTimer += Time.fixedDeltaTime;
                        cameraToggle = true;
                    }
                    else
                    {
                        if (cameraToggle && targetingTimer < 0.25f)
                        {
                            GM.gm.cameraManager.setIsometric(m.skeleton.arma);
                        }
                        targetingTimer = 0;
                        cameraToggle = false;
                    }
                    playerTargeting_thirdPerson(m, input, 32, GM.gm.playerTargeting);
                    if (onTarget)
                    {
                        GM.gm.cameraManager.setDuel(m.skeleton.arma, target);
                    }
                    break;
                case cameraMode.aimRanged:
                    //GM.gm.cameraManager.setThirdPerson(m.skeleton.arma);
                    break;
                case cameraMode.duel:
                    if (input.leftBump)
                    {
                        targetingTimer += Time.fixedDeltaTime;
                        cameraToggle = true;
                    }
                    else
                    {
                        if (cameraToggle && targetingTimer < 0.25f)
                        {
                            GM.gm.cameraManager.setIsometric(m.skeleton.arma);
                        }
                        targetingTimer = 0;
                        cameraToggle = false;
                    }
                    playerTargeting_thirdPerson(m, input, 32, GM.gm.playerTargeting);
                    if (!onTarget) GM.gm.cameraManager.setThirdPerson(m.skeleton.arma);
                    break;
            }
        }
        public void standardOrientLook(AnimationCurve strafe)
        {
            switch (GM.gm.cameraManager.mode)
            {
                case cameraMode.isometric:
                    if (input.leftBump) m.push_orientLook();
                    else if (onTarget) m.twinStick_strafe_orient_target_look(input, strafe, targetCharacter.skeleton.head);
                    else m.twinStick_orientLook_strafe(input, strafe);
                    break;
                case cameraMode.thirdPerson:
                    m.push_orientLook();
                    break;
                case cameraMode.aimRanged:
                    //m.cam_orientLook();
                    if (input.leftBump) m.twinStick_orientLook_strafe(input, strafe);
                    else m.push_orientLook();
                    break;
                case cameraMode.duel:
                    m.twinStick_strafe_orient_target_look(input, strafe, targetCharacter.skeleton.head);
                    break;
            }
        }
        public void actionOrientation()
        {
            switch (GM.gm.cameraManager.mode)
            {
                case cameraMode.isometric:
                    if (input.leftBump) actionOrientation_push();
                    else actionOrientation_twinStick();
                    break;
                case cameraMode.thirdPerson: actionOrientation_push(); break;
                case cameraMode.aimRanged:
                    if (input.leftBump) actionOrientation_twinStick();
                    else actionOrientation_push();
                    break;
                case cameraMode.duel: actionOrientation_twinStick(); break;
            }
        }
        public void actionOrientation_push()
        {
            if (m.temp.pushMagnitude > 0) m.memory.actionDirection.y = m.temp.pushAngle;
            else m.memory.actionDirection.y = m.memory.orientationTarget.y;
        }
        public void actionOrientation_twinStick()
        {
            if (input.turnIsNonZero()) m.memory.actionDirection.y = input.turnAngle();
            else if (m.temp.pushMagnitude > 0) m.memory.actionDirection.y = m.temp.pushAngle;
            else m.memory.actionDirection.y = m.memory.orientationTarget.y;
        }
        public void aimOrientLook()
        {
            if (onTarget) m.target_orientLook(target);
            else m.cam_orientLook();
        }
        public void climbOrientLook()
        {
            m.orientToWall();
            m.lookToOrientationTarget();
        }
        public void ledgeOrientLook()
        {
            m.orientToLedge();
            m.lookToOrientationTarget();
        }

        //Hands Free
        public void movement_handsFree()
        {
            switch (m.state)
            {
                case maine.currentState.jog: nextState_jog(); break;
                case maine.currentState.airFall: nextState_airFall(); break;
                case maine.currentState.stepUp: nextState_stepUp(); break;
                case maine.currentState.vault: nextState_vault(); break;
                case maine.currentState.pullUp: nextState_pullUp(); break;
                case maine.currentState.climb: nextState_climb(); break;
                case maine.currentState.attack:
                    switch (m.memory.statePhase)
                    {
                        case 0: nextState_preFistAttack(); break;
                        case 1: nextState_leftHook(); break;
                        case 2: nextState_rightHook(); break;
                        case 3: nextState_leftBackHand(); break;
                        case 4: nextState_rightBackHand(); break;
                    }
                    break;
                case maine.currentState.holdBag: nextState_holdBag(); break;
                case maine.currentState.sprint: nextState_sprint(); break;
                case maine.currentState.layDown: nextState_layDown(); break;
                case maine.currentState.swim: nextState_swim(); break;
                case maine.currentState.pose: nextState_pose(); break;
            }
        }
        public void nextState_jog()
        {
            m.stayGroundCheck();

            m.ledgeCheck();
            if (m.stepUpCheck())
            {
                m.state = maine.currentState.stepUp;
                m.getActionStart();
                movement_stepUp();
            }
            else if (m.vaultCheck())
            {
                m.state = maine.currentState.vault;
                m.getActionStart();
                movement_vault();
            }
            else if (m.pullupCheck())
            {
                m.state = maine.currentState.pullUp;
                m.getActionStart();
                movement_pullUp();
            }
            else
            {
                m.wallCheck();
                if (m.climbCheck())
                {
                    m.state = maine.currentState.climb;
                    m.getActionStart();
                    movement_climb();
                }
                else
                {
                    if (m.swimCheck())
                    {
                        m.state = maine.currentState.swim;
                        m.blending.arma.start(1);
                        movement_swim();
                    }
                    else if (!m.temp.groundCheck.hit)
                    {
                        m.state = maine.currentState.airFall;
                        movement_airFall();
                    }
                    else if (input.down)
                    {
                        input.down = false;
                        GM.gm.inventory.bag.gameObject.SetActive(true);
                        m.state = maine.currentState.holdBag;
                        movement_holdBag();
                    }
                    else if (input.left)
                    {
                        input.left = false;
                        getItemFromBag_back(GM.gm.inventory.quick.left);
                    }
                    else if (input.up)
                    {
                        input.up = false;
                        getItemFromBag_back(GM.gm.inventory.quick.up);
                    }
                    else if (input.right)
                    {
                        input.right = false;
                        getItemFromBag_back(GM.gm.inventory.quick.right);
                    }
                    else if (input.rightTrigger)
                    {
                        m.state = maine.currentState.attack;
                        m.memory.timer = 0;
                        m.memory.statePhase = 0;
                        actionOrientation();
                        movement_preFistAttack();
                    }
                    else if (input.dodge)
                    {
                        input.dodge = false;
                        m.state = maine.currentState.sprint;
                        movement_sprint();
                    }
                    else if (input.crouch)
                    {
                        input.crouch = false;
                        m.state = maine.currentState.pose;
                        movement_pose();
                    }
                    else if (input.interact)
                    {
                        interactorCheck();
                        movement_jog();
                    }
                    else movement_jog();
                }
            }
        }
        public void movement_jog()
        {
            standardOrientLook(m.mp.strafeRotation);
            m.jog();
        }
        public void nextState_airFall()
        {
            m.airGroundCheck();
            m.ledgeCheck();
            m.wallCheck();
            if (!m.pushIsToOrientation()) m.temp.ledgeCheck.hit = false;

            if (m.stepUpCheck())
            {
                m.state = maine.currentState.stepUp;
                m.getActionStart();
                movement_stepUp();
            }
            else if (m.vaultCheck())
            {
                m.state = maine.currentState.vault;
                m.getActionStart();
                movement_vault();
            }
            else if (m.pullupCheck())
            {
                m.state = maine.currentState.pullUp;
                m.getActionStart();
                movement_pullUp();
            }
            else if (m.climbCheck())
            {
                m.state = maine.currentState.climb;
                m.getActionStart();
                movement_climb();
            }
            else if (m.temp.groundCheck.hit)
            {
                m.state = maine.currentState.jog;
                movement_jog();
            }
            else movement_airFall();
        }
        public void movement_airFall()
        {
            standardOrientLook(m.mp.strafeRotation);
            m.airFall();
        }
        //
        public void nextState_stepUp()
        {
            m.airGroundCheck();
            m.ledgeCheck();

            if (m.offLedgeCheck())
            {
                if (m.temp.groundCheck.hit)
                {
                    m.state = maine.currentState.jog;
                    movement_jog();
                }
                else
                {
                    m.state = maine.currentState.airFall;
                    movement_airFall();
                }
            }
            else movement_stepUp();
        }
        public void movement_stepUp()
        {
            ledgeOrientLook();

            m.stepUp();
        }
        public void nextState_vault()
        {
            m.airGroundCheck();
            m.ledgeCheck();

            if (m.offLedgeCheck())
            {
                if (m.temp.groundCheck.hit)
                {
                    m.state = maine.currentState.jog;
                    movement_jog();
                }
                else
                {
                    m.state = maine.currentState.airFall;
                    movement_airFall();
                }
            }
            else movement_vault();
        }
        public void movement_vault()
        {
            ledgeOrientLook();

            m.stepUp();
        }
        public void nextState_pullUp()
        {
            m.airGroundCheck();
            m.ledgeCheck();

            if (m.offLedgeCheck())
            {
                if (m.temp.groundCheck.hit)
                {
                    m.state = maine.currentState.jog;
                    movement_jog();
                }
                else
                {
                    m.state = maine.currentState.airFall;
                    movement_airFall();
                }
            }
            else movement_pullUp();
        }
        public void movement_pullUp()
        {
            ledgeOrientLook();

            m.stepUp();
        }
        public void nextState_climb()
        {
            m.airGroundCheck();
            m.ledgeCheck();

            if (m.pullupCheck())
            {
                m.state = maine.currentState.pullUp;
                movement_pullUp();
            }
            else if (!m.climbCheck())
            {
                if (!m.temp.groundCheck.hit)
                {
                    m.state = maine.currentState.airFall;
                    movement_airFall();
                }
                else
                {
                    m.state = maine.currentState.jog;
                    movement_jog();
                }
            }
            else movement_climb();
        }
        public void movement_climb()
        {
            climbOrientLook();

            m.climb();
        }
        //
        public void nextState_sprint()
        {
            m.push(input);
            m.stayGroundCheck();
            m.ledgeCheck();
            m.wallCheck();
            if (!m.pushIsToOrientation()) m.temp.ledgeCheck.hit = false;

            if (m.stepUpCheck())
            {
                m.state = maine.currentState.stepUp;
                m.getActionStart();
                movement_stepUp();
            }
            else if (m.vaultCheck())
            {
                m.state = maine.currentState.vault;
                m.getActionStart();
                movement_vault();
            }
            else if (m.pullupCheck())
            {
                m.state = maine.currentState.pullUp;
                m.getActionStart();
                movement_pullUp();
            }
            else if (m.climbCheck())
            {
                m.state = maine.currentState.climb;
                m.getActionStart();
                movement_climb();
            }
            else if (!m.temp.groundCheck.hit)
            {
                m.state = maine.currentState.airFall;
                movement_airFall();
            }
            else if (input.dodge)
            {
                input.dodge = false;
                m.state = maine.currentState.jog;
                movement_jog();
            }
            else if (input.interact)
            {
                interactorCheck();
                movement_sprint();
            }
            else movement_sprint();
        }
        public void movement_sprint()
        {
            standardOrientLook(m.mp.strafeRotation);
            m.sprint();
        }
        //
        public void nextState_preFistAttack()
        {
            m.stayGroundCheck();

            m.memory.timer += Time.fixedDeltaTime;
            if (!input.rightTrigger || m.memory.timer > 0.25f)
            {
                m.memory.timer = 0;
                m.temp.phase = 0;
                input.rightTrigger = false;

                float angle = m.memory.actionDirection.y;
                actionOrientation();
                angle = Mathf.DeltaAngle(angle, m.memory.actionDirection.y);
                if (angle > 20 && angle < 120)
                {
                    m.memory.statePhase = 1;
                    movement_leftHook();
                }
                else if (angle > 120)
                {
                    m.memory.statePhase = 4;
                    movement_rightBackHand();
                }
                else if (angle < -120)
                {
                    m.memory.statePhase = 3;
                    movement_leftBackHand();
                }
                else
                {
                    m.memory.statePhase = 2;
                    movement_rightHook();
                }
            }
            else movement_preFistAttack();
        }
        public void movement_preFistAttack()
        {
            m.action_orientLook();
            m.jog();
        }
        public void nextState_leftHook()
        {
            m.stayGroundCheck();

            m.memory.timer += Time.fixedDeltaTime;
            if (m.memory.timer > 0.5f)
            {
                m.state = maine.currentState.jog;
                movement_jog();
            }
            else
            {
                m.temp.phase = m.memory.timer / 0.5f;
                movement_leftHook();
            }
        }
        public void movement_leftHook()
        {
            m.action_orientLook();
            m.leftHook();
        }
        public void nextState_rightHook()
        {
            m.stayGroundCheck();

            m.memory.timer += Time.fixedDeltaTime;
            if (m.memory.timer > 0.5f)
            {
                m.state = maine.currentState.jog;
                movement_jog();
            }
            else
            {
                m.temp.phase = m.memory.timer / 0.5f;
                movement_rightHook();
            }
        }
        public void movement_rightHook()
        {
            m.action_orientLook();
            m.rightHook();
        }
        public void nextState_leftBackHand()
        {
            m.stayGroundCheck();

            m.memory.timer += Time.fixedDeltaTime;
            if (m.memory.timer > 0.5f)
            {
                m.state = maine.currentState.jog;
                movement_jog();
            }
            else
            {
                m.temp.phase = m.memory.timer / 0.5f;
                movement_leftBackHand();
            }
        }
        public void movement_leftBackHand()
        {
            m.action_orientLook();
            m.leftBackFist();
        }
        public void nextState_rightBackHand()
        {
            m.stayGroundCheck();

            m.memory.timer += Time.fixedDeltaTime;
            if (m.memory.timer > 0.5f)
            {
                m.state = maine.currentState.jog;
                movement_jog();
            }
            else
            {
                m.temp.phase = m.memory.timer / 0.5f;
                movement_rightBackHand();
            }
        }
        public void movement_rightBackHand()
        {
            m.action_orientLook();
            m.rightBackFist();
        }
        public void nextState_rightJab()
        {
            m.stayGroundCheck();

            m.memory.timer += Time.fixedDeltaTime;
            if (m.memory.timer > m.mp.rightJab.duration)
            {
                m.memory.orientation.y = m.memory.rotation;
                m.state = maine.currentState.jog;
                movement_jog();
            }
            else
            {
                m.temp.phase = m.memory.timer / m.mp.rightJab.duration;

            }
        }
        //
        public void nextState_holdBag()
        {
            m.stayGroundCheck();

            if (!m.temp.groundCheck.hit)
            {
                m.state = maine.currentState.airFall;
                GM.gm.inventory.bag.gameObject.SetActive(false);
                movement_airFall();
            }
            else if (input.down || input.crouch)
            {
                input.down = false;
                input.crouch = false;
                m.state = maine.currentState.jog;
                GM.gm.inventory.bag.gameObject.SetActive(false);
                movement_jog();
            }
            else movement_holdBag();
        }
        public void movement_holdBag()
        {
            m.orientToPush_through();
            m.temp.look = new Vector3(0, m.memory.orientationTarget.y, 0);

            m.holdBag();
        }
        //
        public void nextState_layDown()
        {
            m.stayGroundCheck();

            if (!m.temp.groundCheck.hit)
            {
                m.state = maine.currentState.airFall;
                movement_airFall();
            }
            else if (input.crouch)
            {
                input.crouch = false;
                m.state = maine.currentState.jog;
                movement_jog();
            }
            else movement_layDown();
        }
        public void movement_layDown()
        {
            standardOrientLook(m.mp.strafeRotation);

            m.layDown();
        }
        //
        public void nextState_swim()
        {
            m.airGroundCheck();

            if (m.staySwimCheck()) movement_swim();
            else
            {
                if (m.temp.groundCheck.hit)
                {
                    m.state = maine.currentState.jog;
                    movement_jog();
                }
                else
                {
                    m.state = maine.currentState.airFall;
                    movement_airFall();
                }
            }
        }
        public void movement_swim()
        {
            standardOrientLook(m.mp.strafeRotation);
            m.swim();
        }
        //
        public void nextState_pose()
        {
            m.stayGroundCheck();

            if (!m.temp.groundCheck.hit)
            {
                m.state = maine.currentState.airFall;
                movement_airFall();
            }
            else if (input.crouch)
            {
                input.crouch = false;
                m.state = maine.currentState.jog;
                movement_jog();
            }
            else movement_pose();
        }
        public void movement_pose()
        {
            standardOrientLook(m.mp.strafeRotation);

            m.poser();
        }
        //
        public void interactorCheck()
        {
            bool foundInteractor = m.checkInteractors(m.mp.interactPosition, m.mp.interactRadius, out m.memory.interactor, out float intDist);
            bool foundCharacter = m.checkCharacters(m.mp.interactPosition, m.mp.interactRadius, out characterBase other, out float charDist);
            if (foundInteractor && (intDist < charDist || !foundCharacter))
            {
                input.interact = false;
                switch (m.memory.interactor.type)
                {
                    case interactorType.item:
                        pickupItem((item)m.memory.interactor);
                        break;
                    case interactorType.door:
                        interactDoor();
                        break;
                    case interactorType.woodenBox:
                        m.memory.interactor.GetComponent<breakable>().breakObject();
                        break;
                }
            }
            else if (foundCharacter)
            {
                input.interact = false;
                other.controller.interact();
            }
        }
        public void pickupItem(item item)
        {
            switch (item.itemType)
            {
                case itemType.rock:
                    m.itemInHand(m.mp.knife, item, m.equip);
                    break;
                case itemType.knife:
                    m.itemInHand(m.mp.knife, item, m.equip);
                    break;
                case itemType.torch:
                    m.itemInHand(m.mp.torch, item, m.equip);
                    break;
                case itemType.sword:
                    m.itemInHand(m.mp.sword, item, m.equip);
                    break;
                case itemType.bow:
                    m.itemInHand(m.mp.bow, item, m.equip);
                    break;
                case itemType.shield:
                    m.itemInHand(m.mp.shield, item, m.equip);
                    break;
            }
        }
        public void interactDoor()
        {

        }
        //
        public void getItemFromBag_back(itemType type)
        {
            pickupItem(GM.gm.inventory.retrieveItem(type));

            switch (type)
            {
                case itemType.sword: movement_swordJog(); break;
                case itemType.bow: movement_bowJog(); break;
            }
        }
        //
        public void handsFree_interaction()
        {
            switch (m.state)
            {
                case maine.currentState.attack:
                    switch (m.memory.statePhase)
                    {
                        case 1: interaction_leftHook(); break;
                        case 2: interaction_rightHook(); break;
                        case 3: interaction_leftBackHand(); break;
                        case 4: interaction_rightBackHand(); break;
                    }
                    break;
            }
        }
        public void interaction_rightJab()
        {
            if (m.rightJab_hitCheck())
            {
                m.memory.orientation.y = m.memory.rotation;
                input.rightTrigger = false;
                m.state = maine.currentState.jog;
            }
        }
        public void interaction_leftHook()
        {
            if (m.leftHook_hitCheck())
            {
                m.memory.orientation.y = m.memory.rotation;
                input.rightTrigger = false;
                m.state = maine.currentState.jog;
            }
        }
        public void interaction_rightHook()
        {
            if (m.rightJab_hitCheck())
            {
                m.memory.orientation.y = m.memory.rotation;
                input.rightTrigger = false;
                m.state = maine.currentState.jog;
            }
        }
        public void interaction_leftBackHand()
        {
            if (m.leftHook_hitCheck())
            {
                m.memory.orientation.y = m.memory.rotation;
                input.rightTrigger = false;
                m.state = maine.currentState.jog;
            }
        }
        public void interaction_rightBackHand()
        {
            if (m.rightJab_hitCheck())
            {
                m.memory.orientation.y = m.memory.rotation;
                input.rightTrigger = false;
                m.state = maine.currentState.jog;
            }
        }
        //
        public void handsFree_finalizeFrame()
        {

        }

        //Bow
        public void bow_movement()
        {
            switch (m.state)
            {
                case maine.currentState.jog: nextState_bowJog(); break;
                case maine.currentState.ready: nextState_bowReady(); break;
                case maine.currentState.draw: nextState_bowDraw(); break;
            }
        }
        public void nextState_bowJog()
        {
            m.push(input);
            m.stayGroundCheck();

            if (input.interact)
            {
                input.interact = false;
                m.equip.leftHand.destroy();
                movement_jog();
            }
            else if (input.leftTrigger)
            {
                input.leftTrigger = false;
                m.state = maine.currentState.ready;
                movement_bowReady();
            }
            else movement_bowJog();
        }
        public void movement_bowJog()
        {
            standardOrientLook(m.mp.strafeRotation);
            m.bowJog();
        }
        public void nextState_bowReady()
        {
            m.push(input);
            m.stayGroundCheck();

            if (input.leftTrigger)
            {
                input.leftTrigger = false;
                m.state = maine.currentState.jog;
                movement_bowJog();
            }
            else if (input.rightTrigger)
            {
                m.state = maine.currentState.draw;
                movement_bowDraw();
            }
            else movement_bowReady();
        }
        public void movement_bowReady()
        {
            aimOrientLook();

            m.bowReady();
        }
        public void nextState_bowDraw()
        {
            m.push(input);
            m.stayGroundCheck();

            if (!input.rightTrigger)
            {
                m.state = maine.currentState.ready;
                movement_bowReady();
            }
            else movement_bowDraw();
        }
        public void movement_bowDraw()
        {
            aimOrientLook();

            m.bowDraw();
        }

        //Knife
        public void knife_movement()
        {
            nextState_knifeJog();
        }
        public void nextState_knifeJog()
        {
            m.push(input);
            m.stayGroundCheck();
            m.ledgeCheck();

            if (input.interact) { m.equip.rightHand.drop(); input.interact = false; }

            movement_knifeJog();
        }
        public void movement_knifeJog()
        {
            standardOrientLook(m.mp.strafeRotation);
            m.knifeJog();
        }

        //Torch
        public void torch_movement()
        {
            nextState_torchJog();
        }
        public void nextState_torchJog()
        {
            m.push(input);
            m.stayGroundCheck();
            m.ledgeCheck();

            if (input.interact) { m.equip.leftHand.drop(); input.interact = false; }

            movement_torchJog();
        }
        public void movement_torchJog()
        {
            standardOrientLook(m.mp.torchWalkStrafeRotation);
            m.torchWalk();
        }

        //Sword
        public void movement_sword()
        {
            switch (m.state)
            {
                case maine.currentState.jog: nextState_swordJog(); break;
                case maine.currentState.attack: nextState_swordAttack(); break;
            }
        }
        public void nextState_swordJog()
        {
            m.stayGroundCheck();

            if (input.interact)
            {
                input.interact = false;
                m.equip.rightHand.destroy();
                movement_jog();
            }
            else if (input.rightTrigger)
            {
                input.rightTrigger = false;
                m.state = maine.currentState.attack;
                m.memory.timer = 0;
                actionOrientation();
                m.blending.armL.start(0.15f);
                m.blending.armR.start(0.15f);
                movement_swordAttack();
            }
            else movement_swordJog();
        }
        public void movement_swordJog()
        {
            standardOrientLook(m.mp.strafeRotation);
            m.swordJog();
        }
        public void nextState_swordAttack()
        {
            m.stayGroundCheck();

            m.memory.timer += Time.fixedDeltaTime;
            if (m.memory.timer > m.mp.swordAttack.duration)
            {
                m.state = maine.currentState.jog;
                m.blending.armL.start(0.15f);
                m.blending.armR.start(0.25f);
                movement_swordJog();
            }
            else
            {
                m.temp.phase = m.memory.timer / m.mp.swordAttack.duration;
                movement_swordAttack();
            }
        }
        public void movement_swordAttack()
        {
            m.action_orientLook_strafe(m.mp.strafeRotation);
            m.swordAttack();
        }

        //Shield
        public void shield_movement()
        {
            nextState_shieldWalk();
        }
        public void nextState_shieldWalk()
        {
            m.push(input);
            m.stayGroundCheck();

            if (input.interact) { interactorCheck(); }

            movement_shieldWalk();
        }
        public void movement_shieldWalk()
        {
            standardOrientLook(m.mp.strafeRotation);
            m.shieldWalk();
        }

        //Sword Shield
        public void swordShield_movement()
        {
            nextState_swordShieldWalk();
        }
        public void nextState_swordShieldWalk()
        {
            m.push(input);
            m.stayGroundCheck();

            if (input.interact) { m.equip.leftHand.drop(); m.equip.rightHand.drop(); input.interact = false; }

            movement_swordShieldWalk();
        }
        public void movement_swordShieldWalk()
        {
            standardOrientLook(m.mp.strafeRotation);
            m.swordShieldWalk();
        }
    }
}