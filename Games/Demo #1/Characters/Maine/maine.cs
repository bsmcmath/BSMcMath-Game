using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace forthright
{
    public partial class maine : characterBase
    {
        public maineParams mp;
        public equipment equip;
        public currentState state;
        public enum currentState
        {
            jog, airFall, stepUp, vault, pullUp, climb,
            sneak, sprint, walk, layDown, swim,
            attack,
            holdBag,
            nock, ready, draw, loose,
            pose,
        }

        public override void OnEnable()
        {
            base.OnEnable();
            equip = new();
        }

        //hands free
        public void jog()
        {
            stepStateParams p = new stepStateParams(mp.walkState);
            p.move = mp.jogMove;
            p.step = mp.jogStep;
            p.stepBody = mp.jogStepBody;
            p.leftArmSwing = mp.jogLeftArmSwing;
            p.rightArmSwing = mp.jogRightArmSwing;
            p.effects = GM.gm.effects.stepFX();
            step_standard_state(p);
        }
        public void airFall()
        {
            airStepStateParams p = new();
            p.move = mp.fallMove;
            p.pose = mp.walkState.pose;
            p.look = mp.walkState.look;
            p.body = mp.fallBody;
            p.bodyRotationBlend = mp.walkState.bodyRotationBlend;
            p.bodyPositionBlend = mp.walkState.bodyPositionBlend;
            p.leftArmBlend = mp.walkState.leftArmBlend;
            p.rightArmBlend = mp.walkState.rightArmBlend;
            airstep_state(p);
        }
        public void stepUp()
        {
            stepStateParams p = new(mp.walkState);
            p.move = mp.jogMove;
            p.effects = GM.gm.effects.stepFX();
            stepUp_state(p);
        }
        public void vault()
        {

        }
        public void pullUp()
        {

        }
        public void climb()
        {
            climbStateParams p = new();
            p.move = mp.climbMove;
            p.pose = mp.walkState.pose;
            p.look = mp.walkState.look;
            p.bodyRotationBlend = mp.walkState.bodyRotationBlend;
            p.bodyPositionBlend = mp.walkState.bodyPositionBlend;
            p.leftArmBlend = mp.walkState.leftArmBlend;
            p.rightArmBlend = mp.walkState.rightArmBlend;
            climb_state(p);
        }
        public void walk()
        {
            step_standard_state(mp.walkState);
        }
        public void sneak()
        {
            stepStateParams p = new stepStateParams(mp.walkState);
            p.move = mp.sneakMove;
            p.step = mp.sneakStep;
            p.pose = new fullPose(p.pose);
            p.pose.body = mp.sneakBodyPose;
            p.pose.leftArm = mp.sneakLeftArmPose;
            p.pose.rightArm = mp.sneakRightArmPose;
            p.stepBody = mp.sneakStepBody;
            p.leftArmSwing = mp.sneakLeftArmSwing;
            p.rightArmSwing = mp.sneakRightArmSwing;
            p.effects = GM.gm.effects.stepFX();
            step_standard_state(p);
        }
        public void sprint()
        {
            stepStateParams p = new stepStateParams(mp.walkState);
            p.move = mp.sprintMove;
            p.step = mp.sprintStep;
            p.stepBody = mp.sprintStepBody;
            p.leftArmSwing = mp.sprintLeftArmSwing;
            p.rightArmSwing = mp.sprintRightArmSwing;
            p.effects = GM.gm.effects.stepFX();
            step_standard_state(p);
        }
        //
        public void leftHook()
        {
            savePreviousTransform(components.handLCol);

            stepStateParams p = new stepStateParams(mp.walkState);
            p.move = mp.jogMove;
            p.step = mp.jogStep;
            p.leftArmBlend = new blendSettings(blendType.none);
            p.effects = GM.gm.effects.stepFX();
            step_moveBodyArmsAnimation_state(p, mp.leftHook);
        }
        public void rightHook()
        {
            savePreviousTransform(components.handRCol);

            stepStateParams p = new stepStateParams(mp.walkState);
            p.move = mp.jogMove;
            p.step = mp.jogStep;
            p.rightArmBlend = new blendSettings(blendType.none);
            p.effects = GM.gm.effects.stepFX();
            step_moveBodyArmsAnimation_state(p, mp.rightHook);
        }
        public void leftBackFist()
        {
            savePreviousTransform(components.handLCol);

            stepStateParams p = new stepStateParams(mp.walkState);
            p.move = mp.jogMove;
            p.step = mp.jogStep;
            p.leftArmBlend = new blendSettings(blendType.none);
            p.effects = GM.gm.effects.stepFX();
            step_moveBodyArmsAnimation_state(p, mp.leftBackHand);
        }
        public void rightBackFist()
        {
            savePreviousTransform(components.handRCol);

            stepStateParams p = new stepStateParams(mp.walkState);
            p.move = mp.jogMove;
            p.step = mp.jogStep;
            p.rightArmBlend = new blendSettings(blendType.none);
            p.effects = GM.gm.effects.stepFX();
            step_moveBodyArmsAnimation_state(p, mp.rightBackHand);
        }
        public void leftJab()
        {

        }
        public void rightStraight()
        {

        }
        public void rightJab()
        {
            savePreviousTransform(components.handRCol);

            stepStateParams p = new stepStateParams(mp.walkState);
            p.move = mp.jogMove;
            p.step = mp.jogStep;
            p.stepBody = mp.jogStepBody;
            p.rightArmBlend = new blendSettings(blendType.none);
            p.effects = GM.gm.effects.stepFX();
            step_moveBodyArmsAnimation_state(p, mp.rightJab);
        }
        public void leftStraight()
        {

        }
        public bool rightJab_hitCheck()
        {
            List<hurtbox> hits = new();
            hitCheck(components.handRCol, hits);
            int count = hits.Count;
            if (count == 0) return false;

            Vector3 position = components.handRCol.transform.TransformPoint(components.handRCol.center);
            GM.gm.effects.hit.transform.position = position;
            GM.gm.effects.hit.Play();
            Vector3 direction = Vector3.Normalize(position - memory.hitboxPosition);
            for (int i = 0; i < count; i++)
            {
                hits[i].receiveAttack(position, direction, 1, 10);
            }
            return true;
        }
        public bool leftHook_hitCheck()
        {
            List<hurtbox> hits = new();
            hitCheck(components.handLCol, hits);
            int count = hits.Count;
            if (count == 0) return false;

            Vector3 position = components.handLCol.transform.TransformPoint(components.handRCol.center);
            GM.gm.effects.hit.transform.position = position;
            GM.gm.effects.hit.Play();
            Vector3 direction = Vector3.Normalize(position - memory.hitboxPosition);
            for (int i = 0; i < count; i++)
            {
                hits[i].receiveAttack(position, direction, 1, 10);
            }
            return true;
        }
        //
        public void holdBag()
        {
            stepStateParams p = new stepStateParams(mp.walkState);
            p.move = mp.jogMove;
            p.step = mp.jogStep;
            p.stepBody = mp.jogStepBody;
            p.leftArmSwing = mp.jogLeftArmSwing;
            p.rightArmSwing = mp.jogRightArmSwing;
            p.effects = GM.gm.effects.stepFX();
            step_standard_state(p);
        }
        public void layDown()
        {
            layingStateParams p = new();
            p.move = mp.knockdownMove;
            p.pose = mp.knockdownPose;
            p.look = mp.walkState.look;

            laying_state(p);
        }
        public void swim()
        {
            swim_state(mp.swimState);
        }

        //knife
        public void knifeJog()
        {
            stepStateParams p = new stepStateParams(mp.walkState);
            p.move = mp.jogMove;
            p.step = mp.jogStep;
            p.stepBody = mp.jogStepBody;
            p.leftArmSwing = mp.jogLeftArmSwing;
            p.pose = new(mp.walkState.pose);
            p.pose.rightArm = mp.knifeJogRightArmPose;
            p.rightArmSwing = mp.knifeJogRightArmSwing;
            p.effects = GM.gm.effects.stepFX();
            step_standard_state(p);
        }

        //torch
        public void torchWalk()
        {
            stepStateParams p = new stepStateParams(mp.walkState);
            p.pose = new(mp.walkState.pose);
            p.pose.leftArm = mp.torchWalkLeftArmPose;
            p.leftArmSwing = mp.torchWalkLeftArmSwing;
            p.leftArmBlend = mp.torchWalkLeftArmBlend;
            p.effects = GM.gm.effects.stepFX();
            step_leftHandLook_state(p);
        }

        //sword
        public void swordWalk()
        {
            stepStateParams p = new(mp.walkState);
            p.pose = new(p.pose);
            p.pose.rightArm = mp.swordWalkRightArmPose;
            p.rightArmSwing = mp.swordWalkRightArmSwing;
            p.effects = GM.gm.effects.stepFX();
            step_standard_state(p);
        }
        public void swordJog()
        {
            stepStateParams p = new(mp.walkState);
            p.move = mp.jogMove;
            p.step = mp.jogStep;
            p.stepBody = mp.jogStepBody;
            p.pose = new(p.pose);
            p.pose.rightArm = mp.swordWalkRightArmPose;
            p.leftArmSwing = mp.jogLeftArmSwing;
            p.rightArmSwing = mp.swordJogRightArmSwing;
            p.effects = GM.gm.effects.stepFX();
            step_standard_state(p);
        }
        public void swordAttack()
        {
            stepStateParams p = new(mp.walkState);
            p.move = mp.jogMove;
            p.step = mp.jogStep;
            p.stepBody = mp.jogStepBody;
            p.look = mp.swordAttackLook;
            p.pose = new(p.pose);
            p.pose.rightArm = mp.swordAttackRightArmPose;
            p.rightArmBlend = new(blendType.none);
            p.effects = GM.gm.effects.stepFX();
            step_rightStrafeAttack_state(p, mp.swordAttack);
        }

        //bow
        public void bowJog()
        {
            stepStateParams p = new stepStateParams(mp.walkState);
            p.move = mp.jogMove;
            p.step = mp.jogStep;
            p.stepBody = mp.jogStepBody;
            p.pose = new(mp.walkState.pose);
            p.pose.leftArm = mp.bowJogLeftArmPose;
            p.leftArmSwing = mp.bowJogLeftArmSwing;
            p.rightArmSwing = mp.jogRightArmSwing;
            p.effects = GM.gm.effects.stepFX();
            step_standard_state(p);
        }
        public void bowReady()
        {
            bow bow = (bow)equip.leftHand.item;
            bow.moveDraw(-Time.fixedDeltaTime * 4);
            bow.curveBow();

            stepStateParams p = new(mp.walkState);
            p.pose = mp.bowReadyPose;
            p.effects = GM.gm.effects.stepFX();
            step_lerpPose_handsHeadLook_state(p, mp.bowDrawPose, bow.draw);

            bow.bowstring.position = skeleton.handR.TransformPoint(mp.bowstringRightHand);
        }
        public void bowDraw()
        {
            bow bow = (bow)equip.leftHand.item;
            bow.moveDraw(Time.fixedDeltaTime * 2);
            bow.curveBow();

            stepStateParams p = new(mp.walkState);
            p.pose = mp.bowReadyPose;
            p.effects = GM.gm.effects.stepFX();
            step_lerpPose_handsHeadLook_state(p, mp.bowDrawPose, bow.draw);

            bow.bowstring.position = skeleton.handR.TransformPoint(mp.bowstringRightHand);
        }
        public void bowLoose()
        {

        }
        public void bowNock()
        {

        }

        //shield
        public void shieldWalk()
        {
            stepStateParams p = new(mp.walkState);
            p.leftArmSwing = mp.shieldWalkLeftArmSwing;
            p.effects = GM.gm.effects.stepFX();
            step_standard_state(p);
        }

        //sword shield
        public void swordShieldWalk()
        {
            stepStateParams p = new(mp.walkState);
            p.pose = new(p.pose);
            p.pose.rightArm = mp.swordWalkRightArmPose;
            p.leftArmSwing = mp.shieldWalkLeftArmSwing;
            p.rightArmSwing = mp.swordWalkRightArmSwing;
            p.effects = GM.gm.effects.stepFX();
            step_standard_state(p);
        }
    }
}