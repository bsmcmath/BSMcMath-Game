using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "maineParam", menuName = "ScriptableObjects/Characters/Maine")]
public class maineParams : ScriptableObject
{
    public stepStateParams walkState;

    public AnimationCurve strafeRotation;

    [Header("Jogging")]
    public stepMoveParams jogMove;
    public stepParams jogStep;
    public stepBodyParams jogStepBody;
    public armSwing jogLeftArmSwing, jogRightArmSwing;

    [Header("Falling")]
    public airMoveParams fallMove;
    public airBodyParams fallBody;

    [Header("Climbing")]
    public climbMoveParams climbMove;

    [Header("Sneaking")]
    public stepMoveParams sneakMove;
    public stepParams sneakStep;
    public bodyPose sneakBodyPose;
    public stepBodyParams sneakStepBody;
    public armPose sneakLeftArmPose, sneakRightArmPose;
    public armSwing sneakLeftArmSwing, sneakRightArmSwing;

    [Header("Sprinting")]
    public stepMoveParams sprintMove;
    public stepParams sprintStep;
    public stepBodyParams sprintStepBody;
    public armSwing sprintLeftArmSwing, sprintRightArmSwing;

    [Header("Attacking")]
    public moveBodyArmsAnimation rightJab;
    public moveBodyArmsAnimation leftHook, rightHook, leftBackHand, rightBackHand;
    public AnimationCurve rightJabStrength, rightJabDamage;

    [Header("Knockdown")]
    public stepMoveParams knockdownMove;
    public fullPose knockdownPose;

    [Header("Swimming")]
    public swimStateParams swimState;

    [Header("Poser")]
    public fullPose poserPose;

    //

    [Header("Interaction")]
    public Vector3 interactPosition;
    public float interactRadius;

    [Header("Knife")]
    public characterItem knife;

    public armPose knifeJogRightArmPose;
    public armSwing knifeJogRightArmSwing;

    [Header("Torch")]
    public characterItem torch;

    public armPose torchWalkLeftArmPose;
    public armSwing torchWalkLeftArmSwing;
    public blendSettings torchWalkLeftArmBlend;
    public AnimationCurve torchWalkStrafeRotation;

    [Header("Sword")]
    public characterItem sword;

    public armPose swordWalkRightArmPose;
    public armSwing swordWalkRightArmSwing;

    public armSwing swordJogRightArmSwing;

    public moveBodyArmsAnimation swordAttack;
    public armPose swordAttackRightArmPose;
    public lookParams swordAttackLook;

    [Header("Bow")]
    public characterItem bow;

    public armPose bowJogLeftArmPose;
    public armSwing bowJogLeftArmSwing;

    public fullPose bowReadyPose;
    public fullPose bowDrawPose;
    public Vector3 bowstringRightHand;

    [Header("Shield")]
    public characterItem shield;

    public armSwing shieldWalkLeftArmSwing;
}