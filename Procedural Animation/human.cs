using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class human : MonoBehaviour
{
    public humanSkeleton skeleton;
    public humanBasis basis;
    public humanComponents components;

#if UNITY_EDITOR
    [ContextMenu("Setup")]
    public void setup()
    {
        skeleton.getTransforms(transform);
        skeleton.standardLayers();
        getBasis();
        components.setup(skeleton);
    }
    public void getBasis()
    {
        if (!basis)
        {
            basis = (humanBasis)ScriptableObject.CreateInstance("humanBasis");
            AssetDatabase.CreateAsset(basis, "Assets/_Custom/" + gameObject.name + "Basis.asset");
        }

        basis.measurements.populate(skeleton);

        EditorUtility.SetDirty(basis);
        AssetDatabase.SaveAssets();
    }
#endif
}

[Serializable]
public class humanSkeleton
{
    public Transform arma, pelvis, highLegL, highLegR, lowSpine;
    public Transform lowLegL, footL, toeL;
    public Transform lowLegR, footR, toeR;
    public Transform highSpine, neck, shoulderL, shoulderR;
    public Transform head;
    public Transform highArmL, lowArmL, lowArmTwistL, handL;
    public Transform highArmR, lowArmR, lowArmTwistR, handR;

    public Transform fingerIndex1L, fingerIndex2L, fingerIndex3L;
    public Transform fingerMiddle1L, fingerMiddle2L, fingerMiddle3L;
    public Transform fingerRing1L, fingerRing2L, fingerRing3L;
    public Transform fingerPinky1L, fingerPinky2L, fingerPinky3L;
    public Transform thumb1L, thumb2L, thumb3L;

    public Transform fingerIndex1R, fingerIndex2R, fingerIndex3R;
    public Transform fingerMiddle1R, fingerMiddle2R, fingerMiddle3R;
    public Transform fingerRing1R, fingerRing2R, fingerRing3R;
    public Transform fingerPinky1R, fingerPinky2R, fingerPinky3R;
    public Transform thumb1R, thumb2R, thumb3R;

    public void getTransforms(Transform container)
    {
        arma = container.Find("armature");
        pelvis = arma.Find("pelvis");
        highLegL = pelvis.Find("highLegL");
        highLegR = pelvis.Find("highLegR");
        lowSpine = pelvis.Find("lowSpine");

        lowLegL = highLegL.GetChild(0);
        footL = lowLegL.GetChild(0);
        toeL = footL.GetChild(0);

        lowLegR = highLegR.GetChild(0);
        footR = lowLegR.GetChild(0);
        toeR = footR.GetChild(0);

        highSpine = lowSpine.GetChild(0);
        neck = highSpine.Find("neck");
        shoulderL = highSpine.Find("shoulderL");
        shoulderR = highSpine.Find("shoulderR");

        head = neck.GetChild(0);

        highArmL = shoulderL.GetChild(0);
        lowArmL = highArmL.GetChild(0);
        lowArmTwistL = lowArmL.Find("twistArmL");
        handL = lowArmL.Find("handL");

        highArmR = shoulderR.GetChild(0);
        lowArmR = highArmR.GetChild(0);
        lowArmTwistR = lowArmR.Find("twistArmR");
        handR = lowArmR.Find("handR");

        fingerIndex1L = handL.Find("fingerI1L");
        fingerIndex2L = fingerIndex1L.GetChild(0);
        fingerIndex3L = fingerIndex2L.GetChild(0);

        fingerMiddle1L = handL.Find("fingerM1L");
        fingerMiddle2L = fingerMiddle1L.GetChild(0);
        fingerMiddle3L = fingerMiddle2L.GetChild(0);

        fingerRing1L = handL.Find("fingerR1L");
        fingerRing2L = fingerRing1L.GetChild(0);
        fingerRing3L = fingerRing2L.GetChild(0);

        fingerPinky1L = handL.Find("fingerP1L");
        fingerPinky2L = fingerPinky1L.GetChild(0);
        fingerPinky3L = fingerPinky2L.GetChild(0);

        thumb1L = handL.Find("fingerT1L");
        thumb2L = thumb1L.GetChild(0);
        thumb3L = thumb2L.GetChild(0);

        fingerIndex1R = handR.Find("fingerI1R");
        fingerIndex2R = fingerIndex1R.GetChild(0);
        fingerIndex3R = fingerIndex2R.GetChild(0);

        fingerMiddle1R = handR.Find("fingerM1R");
        fingerMiddle2R = fingerMiddle1R.GetChild(0);
        fingerMiddle3R = fingerMiddle2R.GetChild(0);

        fingerRing1R = handR.Find("fingerR1R");
        fingerRing2R = fingerRing1R.GetChild(0);
        fingerRing3R = fingerRing2R.GetChild(0);

        fingerPinky1R = handR.Find("fingerP1R");
        fingerPinky2R = fingerPinky1R.GetChild(0);
        fingerPinky3R = fingerPinky2R.GetChild(0);

        thumb1R = handR.Find("fingerT1R");
        thumb2R = thumb1R.GetChild(0);
        thumb3R = thumb2R.GetChild(0);
    }
    public void standardLayers()
    {
        arma.gameObject.layer = 11;
        pelvis.gameObject.layer = 6;
        lowSpine.gameObject.layer = 6;
        highSpine.gameObject.layer = 6;
        neck.gameObject.layer = 6;
        head.gameObject.layer = 6;

        shoulderL.gameObject.layer = 6;
        highArmL.gameObject.layer = 6;
        lowArmL.gameObject.layer = 6;
        handL.gameObject.layer = 6;

        shoulderR.gameObject.layer = 6;
        highArmR.gameObject.layer = 6;
        lowArmR.gameObject.layer = 6;
        handR.gameObject.layer = 6;

        highLegL.gameObject.layer = 6;
        lowLegL.gameObject.layer = 6;
        footL.gameObject.layer = 6;

        highLegR.gameObject.layer = 6;
        lowLegR.gameObject.layer = 6;
        footR.gameObject.layer = 6;
    }
    public void ragdollLayers()
    {
        pelvis.gameObject.layer = 7;
        lowSpine.gameObject.layer = 7;
        highSpine.gameObject.layer = 7;
        neck.gameObject.layer = 7;
        head.gameObject.layer = 7;

        shoulderL.gameObject.layer = 7;
        highArmL.gameObject.layer = 7;
        lowArmL.gameObject.layer = 7;
        handL.gameObject.layer = 7;

        shoulderR.gameObject.layer = 7;
        highArmR.gameObject.layer = 7;
        lowArmR.gameObject.layer = 7;
        handR.gameObject.layer = 7;

        highLegL.gameObject.layer = 7;
        lowLegL.gameObject.layer = 7;
        footL.gameObject.layer = 7;

        highLegR.gameObject.layer = 7;
        lowLegR.gameObject.layer = 7;
        footR.gameObject.layer = 7;
    }
}

[Serializable]
public class humanComponents
{
    public humanColliders colliders;
    public humanRigidbodies rigidbodies;
    public humanJoints joints;
    public humanIK ik;
    public AudioSource audio;
    public humanShaderInteractors shaderInteractors;

    public void setup(humanSkeleton skeleton)
    {
        colliders.setup(skeleton);
        ik.setup(skeleton);

    }
}

[Serializable]
public class humanColliders
{
    public CapsuleCollider arma, pelvis, lowSpine, highSpine, 
        highArmL, lowArmL, highArmR, lowArmR,
        highLegL, lowLegL, footL, highLegR, lowLegR, footR;
    public SphereCollider neck, head, shoulderL, shoulderR, handL, handR;

    public void setup(humanSkeleton skeleton)
    {
        if (!arma)
        {
            arma = skeleton.arma.gameObject.AddComponent<CapsuleCollider>();
            arma.height = Mathf.Abs(skeleton.head.position.y - skeleton.footL.position.y) * 1.1f;
            arma.radius = help.distance(skeleton.highArmL.position, skeleton.highArmR.position) * 0.6f;
            arma.center = new Vector3(0, 0, help.distance(skeleton.arma.position.y, skeleton.highSpine.position.y) * 0.5f);
            arma.direction = 2;
        }
        if (!pelvis)
        {
            pelvis = skeleton.pelvis.gameObject.AddComponent<CapsuleCollider>();
            pelvis.height = help.distance(skeleton.highLegL.position, skeleton.highLegR.position) * 1.3f;
            pelvis.radius = help.distance(skeleton.lowSpine.position, skeleton.pelvis.position) * 0.5f * 1.3f;
            pelvis.center = new Vector3(0, pelvis.radius, 0);
            pelvis.direction = 0;
        }
        if (!lowSpine)
        {
            lowSpine = skeleton.lowSpine.gameObject.AddComponent<CapsuleCollider>();
            lowSpine.height = help.distance(skeleton.highLegL.position, skeleton.highLegR.position) * 1.2f;
            lowSpine.radius = help.distance(skeleton.highSpine.position, skeleton.lowSpine.position) * 0.5f * 1.2f;
            lowSpine.center = new Vector3(0, lowSpine.radius, 0);
            lowSpine.direction = 0;
        }
        if (!highSpine)
        {
            highSpine = skeleton.highSpine.gameObject.AddComponent<CapsuleCollider>();
            highSpine.height = help.distance(skeleton.highLegL.position, skeleton.highLegR.position) * 1.3f;
            highSpine.radius = help.distance(skeleton.neck.position, skeleton.highSpine.position) * 0.5f * 1.3f;
            highSpine.center = new Vector3(0, highSpine.radius, 0);
            highSpine.direction = 0;
        }
        if (!neck)
        {
            neck = skeleton.neck.gameObject.AddComponent<SphereCollider>();
            neck.radius = help.distance(skeleton.neck.position, skeleton.head.position) * 0.5f;
            neck.center = new Vector3(0, neck.radius, 0);
        }
        if (!head)
        {
            head = skeleton.head.gameObject.AddComponent<SphereCollider>();
            head.radius = help.distance(skeleton.neck.position, skeleton.head.position) * 0.8f;
            head.center = new Vector3(0, head.radius, 0);
        }
        if (!shoulderL)
        {
            shoulderL = skeleton.shoulderL.gameObject.AddComponent<SphereCollider>();
            shoulderL.radius = help.distance(skeleton.shoulderL.position, skeleton.highArmL.position) * 0.5f;
            shoulderL.center = new Vector3(0, shoulderL.radius, 0);
        }
        if (!highArmL)
        {
            highArmL = skeleton.highArmL.gameObject.AddComponent<CapsuleCollider>();
            highArmL.height = help.distance(skeleton.highArmL.position, skeleton.lowArmL.position);
            highArmL.radius = highArmL.height * 0.3f;
            highArmL.center = new Vector3(0, highArmL.height * 0.5f, 0);
            highArmL.direction = 1;
        }
        if (!lowArmL)
        {
            lowArmL = skeleton.lowArmL.gameObject.AddComponent<CapsuleCollider>();
            lowArmL.height = help.distance(skeleton.lowArmL.position, skeleton.handL.position);
            lowArmL.radius = lowArmL.height * 0.3f;
            lowArmL.center = new Vector3(0, lowArmL.height * 0.5f, 0);
            lowArmL.direction = 1;
        }
        if (!handL)
        {
            handL = skeleton.handL.gameObject.AddComponent<SphereCollider>();
            handL.radius = lowArmL.radius;
        }
        if (!shoulderR)
        {
            shoulderR = skeleton.shoulderR.gameObject.AddComponent<SphereCollider>();
            shoulderR.radius = help.distance(skeleton.shoulderR.position, skeleton.highArmR.position) * 0.5f;
            shoulderR.center = new Vector3(0, shoulderR.radius, 0);
        }
        if (!highArmR)
        {
            highArmR = skeleton.highArmR.gameObject.AddComponent<CapsuleCollider>();
            highArmR.height = help.distance(skeleton.highArmR.position, skeleton.lowArmR.position);
            highArmR.radius = highArmR.height * 0.3f;
            highArmR.center = new Vector3(0, highArmR.height * 0.5f, 0);
            highArmR.direction = 1;
        }
        if (!lowArmR)
        {
            lowArmR = skeleton.lowArmR.gameObject.AddComponent<CapsuleCollider>();
            lowArmR.height = help.distance(skeleton.lowArmR.position, skeleton.handR.position);
            lowArmR.radius = lowArmR.height * 0.3f;
            lowArmR.center = new Vector3(0, lowArmR.height * 0.5f, 0);
            lowArmR.direction = 1;
        }
        if (!handR)
        {
            handR = skeleton.handR.gameObject.AddComponent<SphereCollider>();
            handR.radius = lowArmR.radius;
        }
        if (!highLegL)
        {
            highLegL = skeleton.highLegL.gameObject.AddComponent<CapsuleCollider>();
            highLegL.height = help.distance(skeleton.highLegL.position, skeleton.lowLegL.position);
            highLegL.radius = highLegL.height * 0.3f;
            highLegL.center = new Vector3(0, highLegL.height * 0.5f, 0);
            highLegL.direction = 1;
        }
        if (!lowLegL)
        {
            lowLegL = skeleton.lowLegL.gameObject.AddComponent<CapsuleCollider>();
            lowLegL.height = help.distance(skeleton.lowLegL.position, skeleton.footL.position);
            lowLegL.radius = lowLegL.height * 0.3f;
            lowLegL.center = new Vector3(0, lowLegL.height * 0.5f, 0);
            lowLegL.direction = 1;
        }
        if (!footL)
        {
            footL = skeleton.footL.gameObject.AddComponent<CapsuleCollider>();
            footL.height = lowLegL.height;
            footL.radius = lowLegL.radius;
        }
        if (!highLegR)
        {
            highLegR = skeleton.highLegR.gameObject.AddComponent<CapsuleCollider>();
            highLegR.height = help.distance(skeleton.highLegR.position, skeleton.lowLegR.position);
            highLegR.radius = highLegR.height * 0.3f;
            highLegR.center = new Vector3(0, highLegR.height * 0.5f, 0);
            highLegR.direction = 1;
        }
        if (!lowLegR)
        {
            lowLegR = skeleton.lowLegR.gameObject.AddComponent<CapsuleCollider>();
            lowLegR.height = help.distance(skeleton.lowLegR.position, skeleton.footR.position);
            lowLegR.radius = lowLegR.height * 0.3f;
            lowLegR.center = new Vector3(0, lowLegR.height * 0.5f, 0);
            lowLegR.direction = 1;
        }
        if (!footR)
        {
            footR = skeleton.footR.gameObject.AddComponent<CapsuleCollider>();
            footR.height = lowLegR.height;
            footR.radius = lowLegR.radius;
        }
    }
}

[Serializable]
public class humanRigidbodies
{
    public Rigidbody pelvis, highSpine, head,
        highArmL, lowArmL, highArmR, lowArmR, highLegL, lowLegL, highLegR, lowLegR;

    public void ragdollOn(humanSkeleton skeleton)
    {

    }
}

[Serializable]
public class humanJoints
{
    public CharacterJoint highSpine, head,
        highArmL, lowArmL, highArmR, lowArmR, highLegL, lowLegL, highLegR, lowLegR;

    public void ragdollOn(humanSkeleton skeleton)
    {

    }
}

[Serializable]
public class humanIK
{
    public FastIKFabric armL, armR, legL, legR;

    public void setup(humanSkeleton skeleton)
    {
        if (!armL)
        {
            armL = skeleton.handL.gameObject.AddComponent<FastIKFabric>();
            armL.Target = new GameObject("armLIKTarget").transform;
            armL.Target.parent = skeleton.arma.parent;
            armL.Pole = new GameObject("armLIKPole").transform;
            armL.Pole.parent = skeleton.arma.parent;
            armL.Iterations = 20;
        }
        if (!armR)
        {
            armR = skeleton.handR.gameObject.AddComponent<FastIKFabric>();
            armR.Target = new GameObject("armRIKTarget").transform;
            armR.Target.parent = skeleton.arma.parent;
            armR.Pole = new GameObject("armRIKPole").transform;
            armR.Pole.parent = skeleton.arma.parent;
            armR.Iterations = 20;
        }
        if (!legL)
        {
            legL = skeleton.footL.gameObject.AddComponent<FastIKFabric>();
            legL.Target = new GameObject("legLIKTarget").transform;
            legL.Target.parent = skeleton.arma.parent;
            legL.Pole = new GameObject("legLIKPole").transform;
            legL.Pole.parent = skeleton.arma.parent;
            legL.Iterations = 20;
        }
        if (!legR)
        {
            legR = skeleton.footR.gameObject.AddComponent<FastIKFabric>();
            legR.Target = new GameObject("legRIKTarget").transform;
            legR.Target.parent = skeleton.arma.parent;
            legR.Pole = new GameObject("legRIKPole").transform;
            legR.Pole.parent = skeleton.arma.parent;
            legR.Iterations = 20;
        }
    }
}

[Serializable]
public class humanShaderInteractors
{

}