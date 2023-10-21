using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

    public partial class characterBase : MonoBehaviour
    {
        public characterController controller;

        public characterBones skeleton;
        public characterComponents components;
        public characterBasis basis;

        public characterMemory memory;
        public characterTransientBlending blending;

        public characterTempMemory temp;
        public characterAnimationFrameInfo anim;

        public virtual void OnEnable()
        {
            initializeMemory();
        }
        public void initializeMemory()
        {
            memory.legLStep.from = new(skeleton.footL.position);
            memory.legRStep.from = new(skeleton.footR.position);
            memory.recoil = new List<characterRecoil>();
        }

#if (UNITY_EDITOR)
        [ContextMenu("Setup")]
        public void setup()
        {
            skeleton.getReferences(this);
            skeleton.assignLayers();
            components.addIK(this);
            components.addMainRB(this);
            components.addColliders(this);
            components.addHurtboxes(this);
            components.addAudio(this);
            getBasis();
        }
        public void getBasis()
        {
            if (!basis)
            {
                basis = (characterBasis)ScriptableObject.CreateInstance("characterBasis");
                AssetDatabase.CreateAsset(basis, "Assets/_Custom/" + gameObject.name + "basis.asset");
            }

            basis.arma = skeleton.arma.rotation;
            basis.pelvis = skeleton.pelvis.rotation;
            basis.lowSpine = skeleton.lowSpine.rotation;
            basis.highSpine = skeleton.highSpine.rotation;
            basis.neck = skeleton.neck.rotation;
            basis.head = skeleton.head.rotation;
            basis.shoulderL = skeleton.shoulderL.rotation;
            basis.shoulderR = skeleton.shoulderR.rotation;
            basis.handL = skeleton.handL.localRotation;
            basis.handR = skeleton.handR.localRotation;
            basis.footL = skeleton.footL.rotation;
            basis.footR = skeleton.footR.rotation;
            basis.footLegL = skeleton.footL.localRotation;
            basis.footLegR = skeleton.footR.localRotation;
            basis.pos = skeleton.pelvis.localPosition;

            basis.armLength = (skeleton.highArmL.position - skeleton.lowArmL.position).magnitude
                + (skeleton.lowArmL.position - skeleton.handL.position).magnitude;
            basis.legLength = (skeleton.highLegL.position - skeleton.lowLegL.position).magnitude
                + (skeleton.lowLegL.position - skeleton.footL.position).magnitude;

            basis.armLength -= 0.002f;
            basis.legLength -= 0.002f;

            basis.armLTwistFwdHand = skeleton.twistArmL.rotation * Vector3.forward;
            basis.armLTwistFwdHand = Quaternion.Inverse(skeleton.handL.rotation) * basis.armLTwistFwdHand;

            basis.armRTwistFwdHand = skeleton.twistArmR.rotation * Vector3.forward;
            basis.armRTwistFwdHand = Quaternion.Inverse(skeleton.handR.rotation) * basis.armRTwistFwdHand;

            basis.footToArmature = skeleton.arma.position.y - skeleton.footL.position.y;
            basis.kneeToArmature = skeleton.arma.position.y - skeleton.lowLegL.position.y;
            basis.hipToArmature = skeleton.arma.position.y - skeleton.highLegL.position.y;
            basis.armatureToShoulder = skeleton.shoulderL.position.y - skeleton.arma.position.y;
            basis.armatureToHighSpine = skeleton.highSpine.position.y - skeleton.arma.position.y;

            EditorUtility.SetDirty(basis);
            AssetDatabase.SaveAssets();
        }
        [ContextMenu("Replace Colliders")]
        public void replaceColliders()
        {
            components.removeColliders();
            components.addColliders(this);
        }
        [ContextMenu("Add Shader Interaction")]
        public void addShaderInteraction()
        {
            components.addShaderInteractors(this);
        }
#endif
    }

    [Serializable]
    public struct characterBones
    {
        public Transform arma, pelvis, lowSpine, highSpine, neck, head,
            shoulderL, highArmL, lowArmL, handL, twistArmL,
            shoulderR, highArmR, lowArmR, handR, twistArmR,
            highLegL, lowLegL, footL,
            highLegR, lowLegR, footR;

        public void getReferences(characterBase character)
        {
            Transform[] transforms = character.GetComponentsInChildren<Transform>();

            foreach (Transform t in transforms)
            {
                if (t.name.Contains("Arma") || t.name.Contains("arma")) arma = t;
                switch (t.name)
                {
                    case "pelvis":
                        pelvis = t;
                        break;
                    case "lowSpine":
                        lowSpine = t;
                        break;
                    case "highSpine":
                        highSpine = t;
                        break;
                    case "neck":
                        neck = t;
                        break;
                    case "head":
                        head = t;
                        break;
                    case "shoulderL":
                        shoulderL = t;
                        break;
                    case "highArmL":
                        highArmL = t;
                        break;
                    case "lowArmL":
                        lowArmL = t;
                        break;
                    case "handL":
                        handL = t;
                        break;
                    case "twistArmL":
                        twistArmL = t;
                        break;
                    case "shoulderR":
                        shoulderR = t;
                        break;
                    case "highArmR":
                        highArmR = t;
                        break;
                    case "lowArmR":
                        lowArmR = t;
                        break;
                    case "handR":
                        handR = t;
                        break;
                    case "twistArmR":
                        twistArmR = t;
                        break;
                    case "highLegL":
                        highLegL = t;
                        break;
                    case "lowLegL":
                        lowLegL = t;
                        break;
                    case "footL":
                        footL = t;
                        break;
                    case "highLegR":
                        highLegR = t;
                        break;
                    case "lowLegR":
                        lowLegR = t;
                        break;
                    case "footR":
                        footR = t;
                        break;
                }
            }
        }
        public void assignLayers()
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
    public struct characterComponents
    {
        public FastIKFabric armLIK, armRIK, legLIK, legRIK;

        public CapsuleCollider armaCol;

        public CapsuleCollider pelvisCol, lowSpineCol, highSpineCol,
            highArmLCol, lowArmLCol,
            highArmRCol, lowArmRCol,
            highLegLCol, lowLegLCol, footLCol,
            highLegRCol, lowLegRCol, footRCol;

        public SphereCollider neckCol, headCol,
            shoulderLCol, shoulderRCol,
            handLCol, handRCol;

        public Rigidbody armaRB, pelvisRB, highSpineRB, headRB,
            highArmLRB, lowArmLRB, highArmRRB, lowArmRRB,
            highLegLRB, lowLegLRB, highLegRRB, lowLegRRB;

        public CharacterJoint highSpineJoint, headJoint,
            highArmLJoint, lowArmLJoint, highArmRJoint, lowArmRJoint,
            highLegLJoint, lowLegLJoint, highLegRJoint, lowLegRJoint;

        public AudioSource audio;

        public shaderInteractor footLShInt, footRShint;

        public void addIK(characterBase c)
        {
            if (!armLIK)
            {
                armLIK = c.skeleton.handL.gameObject.AddComponent<FastIKFabric>();
                armLIK.Target = new GameObject("armLIKTarget").transform;
                armLIK.Target.parent = c.transform;
                armLIK.Pole = new GameObject("armLIKPole").transform;
                armLIK.Pole.parent = c.transform;
                armLIK.Iterations = 20;
            }
            if (!armRIK)
            {
                armRIK = c.skeleton.handR.gameObject.AddComponent<FastIKFabric>();
                armRIK.Target = new GameObject("armRIKTarget").transform;
                armRIK.Target.parent = c.transform;
                armRIK.Pole = new GameObject("armRIKPole").transform;
                armRIK.Pole.parent = c.transform;
                armRIK.Iterations = 20;
            }
            if (!legLIK)
            {
                legLIK = c.skeleton.footL.gameObject.AddComponent<FastIKFabric>();
                legLIK.Target = new GameObject("legLIKTarget").transform;
                legLIK.Target.parent = c.transform;
                legLIK.Pole = new GameObject("legLIKPole").transform;
                legLIK.Pole.parent = c.transform;
                legLIK.Iterations = 20;
            }
            if (!legRIK)
            {
                legRIK = c.skeleton.footR.gameObject.AddComponent<FastIKFabric>();
                legRIK.Target = new GameObject("legRIKTarget").transform;
                legRIK.Target.parent = c.transform;
                legRIK.Pole = new GameObject("legRIKPole").transform;
                legRIK.Pole.parent = c.transform;
                legRIK.Iterations = 20;
            }
        }
        public void addMainRB(characterBase c)
        {
            if (!armaRB)
            {
                armaRB = c.skeleton.arma.gameObject.AddComponent<Rigidbody>();
                armaRB.isKinematic = true;
            }
        }
        public void addColliders(characterBase c)
        {
            if (!armaCol)
            {
                armaCol = c.skeleton.arma.gameObject.AddComponent<CapsuleCollider>();
                armaCol.height = Mathf.Abs(c.skeleton.head.position.y - c.skeleton.footL.position.y) * 1.1f;
                armaCol.radius = help.distance(c.skeleton.highArmL.position, c.skeleton.highArmR.position) * 0.6f;
                armaCol.center = new Vector3(0, 0, help.distance(c.skeleton.arma.position.y, c.skeleton.highSpine.position.y) * 0.5f);
                armaCol.direction = 2;
            }
            if (!pelvisCol)
            {
                pelvisCol = c.skeleton.pelvis.gameObject.AddComponent<CapsuleCollider>();
                pelvisCol.height = help.distance(c.skeleton.highLegL.position, c.skeleton.highLegR.position) * 1.3f;
                pelvisCol.radius = help.distance(c.skeleton.lowSpine.position, c.skeleton.pelvis.position) * 0.5f * 1.3f;
                pelvisCol.center = new Vector3(0, pelvisCol.radius, 0);
                pelvisCol.direction = 0;
            }
            if (!lowSpineCol)
            {
                lowSpineCol = c.skeleton.lowSpine.gameObject.AddComponent<CapsuleCollider>();
                lowSpineCol.height = help.distance(c.skeleton.highLegL.position, c.skeleton.highLegR.position) * 1.2f;
                lowSpineCol.radius = help.distance(c.skeleton.highSpine.position, c.skeleton.lowSpine.position) * 0.5f * 1.2f;
                lowSpineCol.center = new Vector3(0, lowSpineCol.radius, 0);
                lowSpineCol.direction = 0;
            }
            if (!highSpineCol)
            {
                highSpineCol = c.skeleton.highSpine.gameObject.AddComponent<CapsuleCollider>();
                highSpineCol.height = help.distance(c.skeleton.highLegL.position, c.skeleton.highLegR.position) * 1.3f;
                highSpineCol.radius = help.distance(c.skeleton.neck.position, c.skeleton.highSpine.position) * 0.5f * 1.3f;
                highSpineCol.center = new Vector3(0, highSpineCol.radius, 0);
                highSpineCol.direction = 0;
            }
            if (!neckCol)
            {
                neckCol = c.skeleton.neck.gameObject.AddComponent<SphereCollider>();
                neckCol.radius = help.distance(c.skeleton.neck.position, c.skeleton.head.position) * 0.5f;
                neckCol.center = new Vector3(0, neckCol.radius, 0);
            }
            if (!headCol)
            {
                headCol = c.skeleton.head.gameObject.AddComponent<SphereCollider>();
                headCol.radius = help.distance(c.skeleton.neck.position, c.skeleton.head.position) * 0.8f;
                headCol.center = new Vector3(0, headCol.radius, 0);
            }
            if (!shoulderLCol)
            {
                shoulderLCol = c.skeleton.shoulderL.gameObject.AddComponent<SphereCollider>();
                shoulderLCol.radius = help.distance(c.skeleton.shoulderL.position, c.skeleton.highArmL.position) * 0.5f;
                shoulderLCol.center = new Vector3(0, shoulderLCol.radius, 0);
            }
            if (!highArmLCol)
            {
                highArmLCol = c.skeleton.highArmL.gameObject.AddComponent<CapsuleCollider>();
                highArmLCol.height = help.distance(c.skeleton.highArmL.position, c.skeleton.lowArmL.position);
                highArmLCol.radius = highArmLCol.height * 0.3f;
                highArmLCol.center = new Vector3(0, highArmLCol.height * 0.5f, 0);
                highArmLCol.direction = 1;
            }
            if (!lowArmLCol)
            {
                lowArmLCol = c.skeleton.lowArmL.gameObject.AddComponent<CapsuleCollider>();
                lowArmLCol.height = help.distance(c.skeleton.lowArmL.position, c.skeleton.handL.position);
                lowArmLCol.radius = lowArmLCol.height * 0.3f;
                lowArmLCol.center = new Vector3(0, lowArmLCol.height * 0.5f, 0);
                lowArmLCol.direction = 1;
            }
            if (!handLCol)
            {
                handLCol = c.skeleton.handL.gameObject.AddComponent<SphereCollider>();
                handLCol.radius = lowArmLCol.radius;
            }
            if (!shoulderRCol)
            {
                shoulderRCol = c.skeleton.shoulderR.gameObject.AddComponent<SphereCollider>();
                shoulderRCol.radius = help.distance(c.skeleton.shoulderR.position, c.skeleton.highArmR.position) * 0.5f;
                shoulderRCol.center = new Vector3(0, shoulderRCol.radius, 0);
            }
            if (!highArmRCol)
            {
                highArmRCol = c.skeleton.highArmR.gameObject.AddComponent<CapsuleCollider>();
                highArmRCol.height = help.distance(c.skeleton.highArmR.position, c.skeleton.lowArmR.position);
                highArmRCol.radius = highArmRCol.height * 0.3f;
                highArmRCol.center = new Vector3(0, highArmRCol.height * 0.5f, 0);
                highArmRCol.direction = 1;
            }
            if (!lowArmRCol)
            {
                lowArmRCol = c.skeleton.lowArmR.gameObject.AddComponent<CapsuleCollider>();
                lowArmRCol.height = help.distance(c.skeleton.lowArmR.position, c.skeleton.handR.position);
                lowArmRCol.radius = lowArmRCol.height * 0.3f;
                lowArmRCol.center = new Vector3(0, lowArmRCol.height * 0.5f, 0);
                lowArmRCol.direction = 1;
            }
            if (!handRCol)
            {
                handRCol = c.skeleton.handR.gameObject.AddComponent<SphereCollider>();
                handRCol.radius = lowArmRCol.radius;
            }
            if (!highLegLCol)
            {
                highLegLCol = c.skeleton.highLegL.gameObject.AddComponent<CapsuleCollider>();
                highLegLCol.height = help.distance(c.skeleton.highLegL.position, c.skeleton.lowLegL.position);
                highLegLCol.radius = highLegLCol.height * 0.3f;
                highLegLCol.center = new Vector3(0, highLegLCol.height * 0.5f, 0);
                highLegLCol.direction = 1;
            }
            if (!lowLegLCol)
            {
                lowLegLCol = c.skeleton.lowLegL.gameObject.AddComponent<CapsuleCollider>();
                lowLegLCol.height = help.distance(c.skeleton.lowLegL.position, c.skeleton.footL.position);
                lowLegLCol.radius = lowLegLCol.height * 0.3f;
                lowLegLCol.center = new Vector3(0, lowLegLCol.height * 0.5f, 0);
                lowLegLCol.direction = 1;
            }
            if (!footLCol)
            {
                footLCol = c.skeleton.footL.gameObject.AddComponent<CapsuleCollider>();
                footLCol.height = lowLegLCol.height;
                footLCol.radius = lowLegLCol.radius;
            }
            if (!highLegRCol)
            {
                highLegRCol = c.skeleton.highLegR.gameObject.AddComponent<CapsuleCollider>();
                highLegRCol.height = help.distance(c.skeleton.highLegR.position, c.skeleton.lowLegR.position);
                highLegRCol.radius = highLegRCol.height * 0.3f;
                highLegRCol.center = new Vector3(0, highLegRCol.height * 0.5f, 0);
                highLegRCol.direction = 1;
            }
            if (!lowLegRCol)
            {
                lowLegRCol = c.skeleton.lowLegR.gameObject.AddComponent<CapsuleCollider>();
                lowLegRCol.height = help.distance(c.skeleton.lowLegR.position, c.skeleton.footR.position);
                lowLegRCol.radius = lowLegRCol.height * 0.3f;
                lowLegRCol.center = new Vector3(0, lowLegRCol.height * 0.5f, 0);
                lowLegRCol.direction = 1;
            }
            if (!footRCol)
            {
                footRCol = c.skeleton.footR.gameObject.AddComponent<CapsuleCollider>();
                footRCol.height = lowLegRCol.height;
                footRCol.radius = lowLegRCol.radius;
            }
        }
        public void removeColliders()
        {
            GameObject.DestroyImmediate(armaCol);

            GameObject.DestroyImmediate(pelvisCol);
            GameObject.DestroyImmediate(lowSpineCol);
            GameObject.DestroyImmediate(highSpineCol);
            GameObject.DestroyImmediate(highArmLCol);
            GameObject.DestroyImmediate(lowArmLCol);
            GameObject.DestroyImmediate(highArmRCol);
            GameObject.DestroyImmediate(lowArmRCol);
            GameObject.DestroyImmediate(highLegLCol);
            GameObject.DestroyImmediate(lowLegLCol);
            GameObject.DestroyImmediate(footLCol);
            GameObject.DestroyImmediate(highLegRCol);
            GameObject.DestroyImmediate(lowLegRCol);
            GameObject.DestroyImmediate(footRCol);

            GameObject.DestroyImmediate(neckCol);
            GameObject.DestroyImmediate(headCol);
            GameObject.DestroyImmediate(shoulderLCol);
            GameObject.DestroyImmediate(shoulderRCol);
            GameObject.DestroyImmediate(handLCol);
            GameObject.DestroyImmediate(handRCol);
        }
        public void addHurtboxes(characterBase c)
        {
            if (!c.skeleton.arma.GetComponent<characterRoot>())
            {
                c.skeleton.arma.gameObject.AddComponent<characterRoot>().c = c;
            }
            if (!c.skeleton.pelvis.GetComponent<hurtbox>())
            {
                c.skeleton.pelvis.gameObject.AddComponent<hurtbox>().c = c;
            }
            if (!c.skeleton.lowSpine.GetComponent<hurtbox>())
            {
                c.skeleton.lowSpine.gameObject.AddComponent<hurtbox>().c = c;
            }
            if (!c.skeleton.highSpine.GetComponent<hurtbox>())
            {
                c.skeleton.highSpine.gameObject.AddComponent<hurtbox>().c = c;
            }
            if (!c.skeleton.neck.GetComponent<hurtbox>())
            {
                c.skeleton.neck.gameObject.AddComponent<hurtbox>().c = c;
            }
            if (!c.skeleton.head.GetComponent<hurtbox>())
            {
                c.skeleton.head.gameObject.AddComponent<hurtbox>().c = c;
            }
            if (!c.skeleton.shoulderL.GetComponent<hurtbox>())
            {
                c.skeleton.shoulderL.gameObject.AddComponent<hurtbox>().c = c;
            }
            if (!c.skeleton.highArmL.GetComponent<hurtbox>())
            {
                c.skeleton.highArmL.gameObject.AddComponent<hurtbox>().c = c;
            }
            if (!c.skeleton.lowArmL.GetComponent<hurtbox>())
            {
                c.skeleton.lowArmL.gameObject.AddComponent<hurtbox>().c = c;
            }
            if (!c.skeleton.handL.GetComponent<hurtbox>())
            {
                c.skeleton.handL.gameObject.AddComponent<hurtbox>().c = c;
            }
            if (!c.skeleton.shoulderR.GetComponent<hurtbox>())
            {
                c.skeleton.shoulderR.gameObject.AddComponent<hurtbox>().c = c;
            }
            if (!c.skeleton.highArmR.GetComponent<hurtbox>())
            {
                c.skeleton.highArmR.gameObject.AddComponent<hurtbox>().c = c;
            }
            if (!c.skeleton.lowArmR.GetComponent<hurtbox>())
            {
                c.skeleton.lowArmR.gameObject.AddComponent<hurtbox>().c = c;
            }
            if (!c.skeleton.handR.GetComponent<hurtbox>())
            {
                c.skeleton.handR.gameObject.AddComponent<hurtbox>().c = c;
            }
            if (!c.skeleton.highLegL.GetComponent<hurtbox>())
            {
                c.skeleton.highLegL.gameObject.AddComponent<hurtbox>().c = c;
            }
            if (!c.skeleton.lowLegL.GetComponent<hurtbox>())
            {
                c.skeleton.lowLegL.gameObject.AddComponent<hurtbox>().c = c;
            }
            if (!c.skeleton.footL.GetComponent<hurtbox>())
            {
                c.skeleton.footL.gameObject.AddComponent<hurtbox>().c = c;
            }
            if (!c.skeleton.highLegR.GetComponent<hurtbox>())
            {
                c.skeleton.highLegR.gameObject.AddComponent<hurtbox>().c = c;
            }
            if (!c.skeleton.lowLegR.GetComponent<hurtbox>())
            {
                c.skeleton.lowLegR.gameObject.AddComponent<hurtbox>().c = c;
            }
            if (!c.skeleton.footR.GetComponent<hurtbox>())
            {
                c.skeleton.footR.gameObject.AddComponent<hurtbox>().c = c;
            }
        }
        public void addAudio(characterBase c)
        {
            if (!audio) audio = c.skeleton.arma.gameObject.AddComponent<AudioSource>();
            audio.playOnAwake = false;
        }
        public void addShaderInteractors(characterBase c)
        {
            if (!footLShInt)
            {
                footLShInt = c.skeleton.footL.gameObject.AddComponent<shaderInteractor>();
                footLShInt.size = help.distance(c.skeleton.lowLegL.position, c.skeleton.footL.position) * 0.5f;
            }

            if (!footRShint)
            {
                footRShint = c.skeleton.footR.gameObject.AddComponent<shaderInteractor>();
                footRShint.size = help.distance(c.skeleton.lowLegL.position, c.skeleton.footL.position) * 0.5f;
            }
        }
        public void enableRagdoll(characterBase c)
        {
            pelvisRB = c.skeleton.pelvis.gameObject.AddComponent<Rigidbody>();
            highSpineRB = c.skeleton.highSpine.gameObject.AddComponent<Rigidbody>();
            headRB = c.skeleton.head.gameObject.AddComponent<Rigidbody>();
            highArmLRB = c.skeleton.highArmL.gameObject.AddComponent<Rigidbody>();
            lowArmLRB = c.skeleton.lowArmL.gameObject.AddComponent<Rigidbody>();
            highArmRRB = c.skeleton.highArmR.gameObject.AddComponent<Rigidbody>();
            lowArmRRB = c.skeleton.lowArmR.gameObject.AddComponent<Rigidbody>();
            highLegLRB = c.skeleton.highLegL.gameObject.AddComponent<Rigidbody>();
            lowLegLRB = c.skeleton.lowLegL.gameObject.AddComponent<Rigidbody>();
            highLegRRB = c.skeleton.highLegR.gameObject.AddComponent<Rigidbody>();
            lowLegRRB = c.skeleton.lowLegR.gameObject.AddComponent<Rigidbody>();

            highSpineJoint = c.skeleton.highSpine.gameObject.AddComponent<CharacterJoint>();
            headJoint = c.skeleton.head.gameObject.AddComponent<CharacterJoint>();
            highArmLJoint = c.skeleton.highArmL.gameObject.AddComponent<CharacterJoint>();
            lowArmLJoint = c.skeleton.lowArmL.gameObject.AddComponent<CharacterJoint>();
            highArmRJoint = c.skeleton.highArmR.gameObject.AddComponent<CharacterJoint>();
            lowArmRJoint = c.skeleton.lowArmR.gameObject.AddComponent<CharacterJoint>();
            highLegLJoint = c.skeleton.highLegL.gameObject.AddComponent<CharacterJoint>();
            lowLegLJoint = c.skeleton.lowLegL.gameObject.AddComponent<CharacterJoint>();
            highLegRJoint = c.skeleton.highLegR.gameObject.AddComponent<CharacterJoint>();
            lowLegRJoint = c.skeleton.lowLegR.gameObject.AddComponent<CharacterJoint>();

            highSpineJoint.connectedBody = pelvisRB;
            headJoint.connectedBody = highSpineRB;
            highArmLJoint.connectedBody = highSpineRB;
            lowArmLJoint.connectedBody = highArmLRB;
            highArmRJoint.connectedBody = highSpineRB;
            lowArmRJoint.connectedBody = highArmRRB;
            highLegLJoint.connectedBody = pelvisRB;
            lowLegLJoint.connectedBody = highLegLRB;
            highLegRJoint.connectedBody = pelvisRB;
            lowLegRJoint.connectedBody = highLegRRB;

            c.skeleton.ragdollLayers();
        }
        public void disableRagdoll(characterBase c)
        {
            GameObject.DestroyImmediate(highSpineJoint);
            GameObject.DestroyImmediate(headJoint);
            GameObject.DestroyImmediate(highArmLJoint);
            GameObject.DestroyImmediate(lowArmLJoint);
            GameObject.DestroyImmediate(highArmRJoint);
            GameObject.DestroyImmediate(lowArmRJoint);
            GameObject.DestroyImmediate(highLegLJoint);
            GameObject.DestroyImmediate(lowLegLJoint);
            GameObject.DestroyImmediate(highLegRJoint);
            GameObject.DestroyImmediate(lowLegRJoint);

            GameObject.DestroyImmediate(pelvisRB);
            GameObject.DestroyImmediate(highSpineRB);
            GameObject.DestroyImmediate(headRB);
            GameObject.DestroyImmediate(highArmLRB);
            GameObject.DestroyImmediate(lowArmLRB);
            GameObject.DestroyImmediate(highArmRRB);
            GameObject.DestroyImmediate(lowArmRRB);
            GameObject.DestroyImmediate(highLegLRB);
            GameObject.DestroyImmediate(lowLegLRB);
            GameObject.DestroyImmediate(highLegRRB);
            GameObject.DestroyImmediate(lowLegRRB);

            c.skeleton.assignLayers();
        }
    }